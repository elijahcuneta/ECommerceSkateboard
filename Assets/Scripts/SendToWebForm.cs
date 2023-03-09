using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendToWebForm : MonoBehaviour {

    public static string url = "https://melroseboardshopph.000webhostapp.com/";

    //string onlineUrl = "https://melroseboardshopph.000webhostapp.com/";
    //string offlineUrl = "http://localhost/Skateboard_Test/";

    public static void send(byte[] bytes) {
        WWWForm form = new WWWForm();
        string productName = SkateboardData.skateboardDeck + "," + SkateboardData.skateboardTrack + ","
            + SkateboardData.skateboardWheels + "," + SkateboardData.skateboardBearings + "," + SkateboardData.skateboardHardware;
        string productPrice = SkateboardData.deckPrice + "," + SkateboardData.trackPrice + "," + SkateboardData.wheelsPrice + ","
            + SkateboardData.bearingsPrice + "," + SkateboardData.hardwarePrice;
        form.AddField("productName", productName);
        form.AddField("productPrice", productPrice);
        form.AddField("productImg", System.Convert.ToBase64String(bytes));

        WWW data_post = new WWW(url + "insertData.php", form);
    }

}