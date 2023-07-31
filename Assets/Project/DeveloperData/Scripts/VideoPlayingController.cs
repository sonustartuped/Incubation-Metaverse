using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlayingController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    //public string videoUrl;
    public string videoName;

    [Header("UI buttons")]
    public GameObject videoUiParent;
    public GameObject playBtn, pauseBtn, restartBtn;

    bool isMouseOnTvUI, isVideoLoaded;

    private void Start()
    {
        //videoPlayer.url = videoUrl;
        videoPlayer.clip = Resources.Load<VideoClip>(videoName);
        videoUiParent.SetActive(false);
        SetPlayUI(true);
        //videoPlayer.prepareCompleted += VideoPrepared;
        //videoPlayer.Prepare();
        isVideoLoaded = true;
    }

    //void VideoPrepared(VideoPlayer vPlayer)
    //{
    //    isVideoLoaded = true;
    //}

    public void PlayVideo()
    {
        if (isVideoLoaded)
        {
            videoPlayer.Play();
            SetPlayUI(false);
            restartBtn.SetActive(true);
        }
        else
        {
            Debug.Log("Video Not loaded");
        }
        
    }

    public void PauseVideo()
    {
        videoPlayer.Pause();
        SetPlayUI(true);
    }

    public void RestartVideo()
    {
        videoPlayer.Stop();
        PlayVideo();
    }

    void SetPlayUI(bool status)
    {
        playBtn.SetActive(status);
        pauseBtn.SetActive(!status);
    }

    public void SetTVUI_OnMouseHover(bool status)
    {
        StartCoroutine(SetTvUI_Coroutine(status));
    }

    IEnumerator SetTvUI_Coroutine(bool status)
    {
        yield return new WaitForEndOfFrame();
        if (!isMouseOnTvUI)
        {
            videoUiParent.SetActive(status);
            Debug.Log("Set tv ui" + status);
        }
    }

    public void CheckMouseHoverOnTVUI(bool status)
    {
        isMouseOnTvUI = status;
        if (status)
        {
            videoUiParent.SetActive(true);
        }
        Debug.Log("Is mouse on tv ui" + status);
    }
}
