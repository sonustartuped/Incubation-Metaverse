using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClickkAudio : MonoBehaviour
{
    public AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();


        Button button = GetComponent<Button>();
        button.onClick.AddListener(PlayButtonClickSound);
    }

    void PlayButtonClickSound()
    {
        audioSource.Play();
    }
}
