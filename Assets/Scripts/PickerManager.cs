using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickerManager : MonoBehaviour {

    [SerializeField]
    GameObject[] Picker_Window;

    int pickerIndex;

    void Start() {
        pickerIndex = 0;    
    }

    public void Picker_Next() {
        pickerIndex++;
        if(pickerIndex >= Picker_Window.Length) {
            pickerIndex = 0;
        }

        Picker_SetActive(pickerIndex);
    }

    public void Picker_Back() {
        pickerIndex--;
        if (pickerIndex < 0) {
            pickerIndex = Picker_Window.Length - 1;
        }
        Picker_SetActive(pickerIndex);
    }

    void Picker_SetActive(int pickerWindowIndex) {
        foreach (GameObject p in Picker_Window) {
            p.SetActive(false);
        }
        Picker_Window[pickerWindowIndex].SetActive(true);
    }

}
