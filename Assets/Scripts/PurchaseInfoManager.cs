using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseInfoManager : MonoBehaviour {

    [SerializeField]
    Text deckName, trackName, wheelsName, bearingsName, hardwareName,
         deckPrice, trackPrice, wheelsPrice, bearingsPrice, hardwarePrice, totalPrice, defaultName, defaultPrice;

    byte[] bytes;

    SkateboardCustomizeManager cm;
    SkateBoardPartsManager pm;

    string product_ids;
    [SerializeField]
    Canvas HUD;

    void Start() {
        cm = FindObjectOfType<SkateboardCustomizeManager>();
        pm = FindObjectOfType<SkateBoardPartsManager>();
    }

    public void SetInformation() {
        deckName.text = SkateboardData.skateboardDeck;
        trackName.text = SkateboardData.skateboardTrack;
        wheelsName.text = SkateboardData.skateboardWheels;
        bearingsName.text = SkateboardData.skateboardBearings;
        hardwareName.text = SkateboardData.skateboardHardware;

        deckPrice.text = SkateboardData.deckPrice.ToString("F2");
        trackPrice.text = SkateboardData.trackPrice.ToString("F2");
        wheelsPrice.text = SkateboardData.wheelsPrice.ToString("F2");
        bearingsPrice.text = SkateboardData.bearingsPrice.ToString("F2");
        hardwarePrice.text = SkateboardData.hardwarePrice.ToString("F2");

        foreach(DefaultItemsEntity d in FindObjectOfType<SkateboardDefaultItems>().defaultItems) {
            SkateboardData.totalPrice += d.partPrice;
            defaultName.text += d.partName + "\n";
            defaultPrice.text += d.partPrice.ToString("F2") + "\n";
        }

        totalPrice.text = SkateboardData.totalPrice.ToString();
    }

    IEnumerator refreshData() {
        PleaseWait.value = 0;
        for(int i = 0; i < cm.partInformation.Count; i++) {
            yield return StartCoroutine(cm.partInformation[i].getCount());
            PleaseWait.value = (float)i / (float)(cm.partInformation.Count - 1);
        }
        product_ids = "";
        bool valid = true;
        for (int i = 0; i < cm.partInformation.Count; i++) {
            if (cm.partInformation[i].count > 0) {
                //id0-stock[0],id[1]-stock[1]
                product_ids += cm.partInformation[i].id + "-" + (cm.partInformation[i].count - 1) + ",";
            } else {
                valid = false;
                break;
            }
        }
        if (valid) {
            StartCoroutine(sendPurchasedItem());
        }
    }

    public void purchase() {
        StartCoroutine(refreshData());
        pm.Parts_SetActive((int)pm.skateboardParts, false);
        pm.SetPanelName("PREVIEW");
        pm.SetNameForPreview("Welcome to", "Melrose's Skateboard");
        pm.SetTotalPrice();
    }

    IEnumerator sendPurchasedItem() {
        WWWForm form = new WWWForm();
        form.AddField("purchased_item", product_ids);
        WWW www = new WWW(SendToWebForm.url + "buyItem.php", form);
        PleaseWait.activate();
        yield return www;
        PleaseWait.deactivate();
        StartCoroutine(readScreen());
    }

    IEnumerator readScreen() {
        cm.AllColoredView();
        FindObjectOfType<SkateboardBehavior>().printMode();
        HUD.enabled = false;
        yield return new WaitForEndOfFrame();
        int width = Screen.width;
        int height = Screen.height;
        var tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        //tex.Resize(width / 2, height / 2);
        tex.Apply();
        bytes = tex.EncodeToPNG();
        SendToWebForm.send(bytes);
        Destroy(tex);
        HUD.enabled = true;
        gameObject.SetActive(false);
    }

}
