using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillManager : MonoBehaviour {

    public Slider progress;

    [SerializeField]
    Image[] fillCircle;

    void Start() {
        progress.interactable = false;    
    }

    public void UpdateFillCircle(int partsIndex) {
        fillCircle[partsIndex].color = new Color(0, 1, 0, 1);
    }

    public void RemoveAllFillCircle() {
        foreach(Image i in fillCircle) {
            i.color = new Color(1,1,1,1);
        }
    }

    public void SetProgressValue(float progressValue) {
        progress.value = progressValue;
    }

    public void SetProgressInteractable(bool status) {
        progress.interactable = status; 
    }

    public void ProgressBar(Slider slider) {
        FindObjectOfType<SkateBoardPartsManager>().Parts_SetActive((int)slider.value, true);
    }
}
