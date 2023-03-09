using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PleaseWait : MonoBehaviour {
    static GameObject instance;
    public static float value;
    Slider slider;
    private void Awake() {
        instance = gameObject;
        slider = GetComponentInChildren<Slider>();
        slider.value = 0;
        deactivate();
    }

    public static void activate() {
        instance.SetActive(true);
    }

    public static void deactivate() {
        instance.SetActive(false);
    }
    private void Update() {
        slider.value = value;
    }
}
