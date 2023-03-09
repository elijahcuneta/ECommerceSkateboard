using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PartInformation : MonoBehaviour {

    public int id;
    public GameObject prefab;
    public string partName;
    [HideInInspector]
    public float partPrice;
    public SkateboardParts partType;
    public int count;
    
    //Text label;
    Button button;

    private void Start() {
        button = GetComponent<Button>();
        //button.interactable = false;
        //label = GetComponentInChildren<Text>();
        GetComponentInChildren<Text>().text = "";
        GetComponentInChildren<Text>().alignment = TextAnchor.LowerRight;
        //label.alignment = TextAnchor.LowerRight;
        //count = 20;
    }


    public IEnumerator getCount() {
        WWWForm form = new WWWForm();
        print("ew");
        form.AddField("request_id", id);
        WWW www = new WWW(SendToWebForm.url + "getProductStock.php", form);
        yield return www;
        string[] split = www.text.Split(',');//name,price,stock
        partName = split[0];
        if (!float.TryParse(split[1], out partPrice)) {
            partPrice = 0;
        }
        if (!int.TryParse(split[2], out count)) {
            count = 0;
        }
        //label.text = count.ToString();
        if (count > 0) {
            //button.interactable = true;
        } else {
            //button.interactable = false;
        }
        //label.text = count + "";
        if (www.error == null) {
            print("SUCCESS");
        }
    }

    public void SetPartInformation() {
        FindObjectOfType<SkateBoardPartsManager>().Parts_SetInformation(partName, partPrice);
        FindObjectOfType<SkateBoardPartsManager>().SetTotalPrice();
        //StartCoroutine(setCount());
    }
}
