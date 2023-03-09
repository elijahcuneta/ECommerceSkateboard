using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SkateboardCustomizeManager : MonoBehaviour {

    public SkateboardParts skateboardParts = SkateboardParts.Deck;

    [HideInInspector]
    public bool allColorMode = true;

    [SerializeField]
    GameObject[] skateboardPartsPosition;

    [SerializeField]
    GameObject Skateboard;

    [SerializeField]
    Transform spawnPoint;

    [SerializeField]
    Button purchaseButton;

    [SerializeField]
    Button next, back;

    [SerializeField]
    Material opaque, transparent;

    SkateBoardPartsManager skateboardPM;
    FillManager fillManager;
    [SerializeField]
    PartInformation[] info;
    public List<PartInformation> partInformation;
    Button[] progressButton;
    Text availability;


    void Start() {
        availability = GameObject.Find("Availability").GetComponent<Text>();
        availability.text = "";
        progressButton = new Button[3];
        progressButton[0] = GameObject.Find("Next").GetComponent<Button>();
        progressButton[1] = GameObject.Find("Back").GetComponent<Button>();
        progressButton[2] = GameObject.Find("Purchase").GetComponent<Button>();
        partInformation = new List<PartInformation>();
        next.interactable = back.interactable = purchaseButton.interactable = false;
        skateboardPM = GetComponent<SkateBoardPartsManager>();
        fillManager = FindObjectOfType<FillManager>();
        StartCoroutine(initData());
    }

    IEnumerator initData() {
        PleaseWait.activate();
        for (int i = 0; i < info.Length; i++) {
            yield return StartCoroutine(info[i].getCount());
            PleaseWait.value = (float)i / (float)(info.Length-1);
        }
        PleaseWait.deactivate();
    }

    public void SetPart(PartInformation info) {
        GameObject newPart = info.prefab;
        Part skateboardPart = newPart.GetComponent<Part>();
        DeletePart(skateboardPart.skateboardParts.ToString());
        info.partType = skateboardPart.skateboardParts;
        partInformation.Add(info);
        //info.updateText();
        GameObject part = Instantiate(newPart, spawnPoint.transform.position, skateboardPartsPosition[(int)skateboardPart.skateboardParts].transform.rotation);
        part.transform.parent = skateboardPartsPosition[(int)skateboardPart.skateboardParts].transform;
        part.GetComponent<Part>().GoToPosition(newPart.transform.position);

        purchaseButton.interactable = CheckParts();
        fillManager.SetProgressInteractable(CheckParts());
        CheckNextBackAvail((int)skateboardPM.skateboardParts);
        if (info.count <= 0) {
            availability.text = info.partName + " is out of stock!";
            foreach (Button b in progressButton) {
                b.interactable = false;
            }
            fillManager.SetProgressInteractable(false);
        } else {
            availability.text = "";
        }
    }

    void DeletePart(string skateboardParts) {
        for (int i = 0; i < partInformation.Count; i++) {
            if (partInformation[i].partType.ToString() == skateboardParts) {
                partInformation.Remove(partInformation[i]);
            }
        }
        foreach (PartPosition p in Skateboard.GetComponentsInChildren<PartPosition>()) {
            if (p.skateboardParts.ToString() == skateboardParts) {
                if (p.transform.childCount != 0) {
                    Destroy(p.transform.GetChild(0).gameObject);
                }
            }
        }
    }

    public void ColorMode(Image buttonImg) {
        allColorMode = !allColorMode;
        if (allColorMode) {
            AllColoredView();
            Color newColor = buttonImg.color;
            newColor.a = 0f;
            buttonImg.color = newColor;
        } else {
            FocusPart(SkateBoardPartsManager.currentSkateboardPanel);
            Color newColor = buttonImg.color;
            newColor.a = 1f;
            buttonImg.color = newColor;
        }
    } // For FocusPart() and AllColoredView() ; For Button

    public void FocusPart(string skateboardParts) { //Change transparency of other objs | Called by skateboardPartsManager
        StopAllCoroutines();
        foreach (PartPosition p in Skateboard.GetComponentsInChildren<PartPosition>()) {
            if (p.skateboardParts.ToString() != skateboardParts) {
                p.transform.localScale = Vector3.one;
                if (p.transform.childCount != 0) {
                    foreach (Renderer r in p.GetComponentsInChildren<Renderer>()) {
                        Color newColor = r.material.color;
                        newColor.a = 0.2f;
                        r.material.shader = transparent.shader;
                        r.material.SetColor("_Color", newColor);
                    }
                }
            } else {
                p.transform.localScale = Vector3.one * 3f;
                StartCoroutine(scaleDown(p.transform));
                if (p.transform.childCount != 0) {
                    foreach (Renderer r in p.GetComponentsInChildren<Renderer>()) {
                        Color newColor = r.material.color;
                        newColor.a = 1f;
                        r.material.shader = opaque.shader;
                        r.material.SetColor("_Color", newColor);
                    }
                }
            }
        }
    }

    IEnumerator scaleDown(Transform t) {
        float timer = 0;
        while (t.transform.localScale != Vector3.one) {
            t.transform.localScale = Vector3.Lerp(t.transform.localScale, Vector3.one, timer);
            timer += Time.deltaTime;
            yield return null;
        }
    }

    public void turnOffFocus(Image buttonImg) {
        AllColoredView();
        Color newColor = buttonImg.color;
        newColor.a = 0f;
        buttonImg.color = newColor;
    }

    public void AllColoredView() {
        foreach (PartPosition p in Skateboard.GetComponentsInChildren<PartPosition>()) {
            p.transform.localScale = Vector3.one;
            if (p.transform.childCount != 0) {
                foreach (Renderer r in p.GetComponentsInChildren<Renderer>()) {
                    Color newColor = r.material.color;
                    newColor.a = 1f;
                    r.material.shader = opaque.shader;
                    r.material.SetColor("_Color", newColor);
                }
            }
        }
    }

    public void ResetParts() {
        foreach (PartPosition p in Skateboard.GetComponentsInChildren<PartPosition>()) {
            if (p.transform.childCount != 0) {
                Destroy(p.transform.GetChild(0).gameObject);
            }
        }
        partInformation.Clear();
        skateboardPM.Parts_SetActive(0, true);
        CheckNextBackAvail((int)skateboardPM.skateboardParts);
        fillManager.RemoveAllFillCircle();
        fillManager.SetProgressValue(0);
        fillManager.SetProgressInteractable(false);
        FindObjectOfType<SkateboardDataManager>().ResetPrice();
        skateboardPM.SetTotalPrice();
    }

    public bool CheckParts() {
        int partCount = 0;
        foreach (PartPosition p in Skateboard.GetComponentsInChildren<PartPosition>()) {
            if (p.transform.childCount != 0) {
                partCount++;
            }
        }
        if (partCount == Enum.GetNames(typeof(SkateboardParts)).Length) {
            return true;
        } else {
            return false;
        }
    }

    public void CheckNextBackAvail(int skateboardPartsIndex) {
        next.interactable = back.interactable = false;
        if (skateboardPartsPosition[skateboardPartsIndex].transform.childCount != 0 && skateboardPartsIndex != Enum.GetNames(typeof(SkateboardParts)).Length - 1) {
            next.interactable = true;
        }
        if (skateboardPartsIndex != 0 && skateboardPartsPosition[skateboardPartsIndex - 1].transform.childCount != 0) {
            back.interactable = true;
        }
    }

    public void RestartCustomization() {
        SceneManager.LoadScene(0);
    }

    public void QuitApplication() {
        Application.Quit();
        //AndroidJavaClass myClass = new AndroidJavaClass("com.melrose.skateboard.homepage");
        //myClass.Call("quitUnityActivity");
    }
}
