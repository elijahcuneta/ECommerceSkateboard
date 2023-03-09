using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkateBoardPartsManager : MonoBehaviour {

   public SkateboardParts skateboardParts = SkateboardParts.Deck;

    public static string currentSkateboardPanel;

    [SerializeField]
    GameObject[] skateboardParts_Object;

    [Space(10)]
    [SerializeField]
    Text Parts_Text, Parts_Name, Parts_Price, Parts_TotalPrice, name_TEXT;

    SkateboardDataManager skateboardDM;
    SkateboardCustomizeManager skateboardCM;
    SkateboardBehavior mainSkateboard;
    FillManager fillManager;

    void Start() {
        skateboardDM = FindObjectOfType<SkateboardDataManager>();
        skateboardCM = FindObjectOfType<SkateboardCustomizeManager>();
        mainSkateboard = FindObjectOfType<SkateboardBehavior>();
        fillManager = FindObjectOfType<FillManager>();
        Parts_SetActive((int)skateboardParts, true);

        name_TEXT.text = "WELCOME TO";
        Parts_Name.text = "MELROSE'S SKATEBOARD";
    }

    public void Parts_Next() {
        skateboardParts++;
        if ((int)skateboardParts >= skateboardParts_Object.Length) {
            skateboardParts = 0;
        }
        fillManager.progress.value = (int)skateboardParts;
        Parts_SetActive((int)skateboardParts, true);
        mainSkateboard.changeSkateboard((int)skateboardParts);
    }

    public void Parts_Back() {
        skateboardParts--;
        if (skateboardParts < 0) {
            skateboardParts = (SkateboardParts)skateboardParts_Object.Length - 1;
        }
        fillManager.progress.value = (int)skateboardParts;
        Parts_SetActive((int)skateboardParts, true);
        mainSkateboard.changeSkateboard((int)skateboardParts);
    }

    public void Parts_SetActive(int partsIndex, bool activeState) {
        foreach (GameObject skateboardPart in skateboardParts_Object) {
            skateboardPart.SetActive(false);
        }
        mainSkateboard.changeSkateboard((int)skateboardParts);
        skateboardParts = (SkateboardParts)partsIndex;
        Parts_ClearInformation();
        SetPanelName(skateboardParts.ToString());
        skateboardParts_Object[partsIndex].SetActive(activeState);
        currentSkateboardPanel = skateboardParts.ToString();
        if (!skateboardCM.allColorMode) {
            skateboardCM.FocusPart(skateboardParts.ToString());
        }
        skateboardCM.CheckNextBackAvail(partsIndex);
    }

    public void SetPanelName(string newName) {
        Parts_Text.text = newName.ToUpper();
    }

    public void SetNameForPreview(string nameText, string partText) { // only used in preview mode
        name_TEXT.text = nameText;
        Parts_Name.text = partText;
    }

    public void Parts_SetInformation(string name, float price) {
        Parts_Name.text = name;
        Parts_Price.text = price.ToString();
        name_TEXT.text = "NAME";

        skateboardDM.SkateboardSetInformation(skateboardParts.ToString(), name, price);
        fillManager.UpdateFillCircle((int)skateboardParts);
    }

    void Parts_ClearInformation() {
        Parts_Name.text = Parts_Price.text = "";
    }

    public void SetTotalPrice() {
        Parts_TotalPrice.text = SkateboardData.totalPrice.ToString();
    }
  
}
