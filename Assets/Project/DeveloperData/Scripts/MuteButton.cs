using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuteButton : MonoBehaviour
{
    public AudioSource audioSource;
    public Sprite muteIcon;
    public Sprite unmuteIcon;

    private bool isMuted = false;
    private Image buttonImage;

    private void Start()
    {
        buttonImage = GetComponent<Image>();
        UpdateButtonIcon();
    }
    public void ToggleMuted()
    {
        isMuted = !isMuted;
        UpdateButtonIcon();

        if(audioSource != null) 
        {
            
            audioSource.mute = isMuted;
            
        }
    }

    private void UpdateButtonIcon()
    {
        if(isMuted)
        {
            buttonImage.sprite = unmuteIcon;
        }

        else
        {
            buttonImage.sprite = muteIcon;
        }
    }
}
