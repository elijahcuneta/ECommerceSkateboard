using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OtherButtons : MonoBehaviour {

    [SerializeField]
    AudioSource[] audios;

    public void MuteUnmute(Image toggleImage) {
        foreach(AudioSource audioSource in audios) {
            audioSource.mute = !audioSource.mute;
        }
        toggleImage.color = new Color(toggleImage.color.r, toggleImage.color.g, toggleImage.color.b, toggleImage.color.a == 0 ? 1 : 0);
    }
}
