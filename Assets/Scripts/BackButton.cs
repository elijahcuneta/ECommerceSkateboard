using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButton : MonoBehaviour {

    [SerializeField]
    GameObject quitPanel;

    void Update () {
        if (Application.platform == RuntimePlatform.Android && Input.GetKeyDown(KeyCode.Escape)) {
            quitPanel.SetActive(true);
        }
	}
}
