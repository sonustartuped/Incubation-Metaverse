using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoSkip : MonoBehaviour
{
    public GameObject panelToClose;

    public void Close()
    {
        panelToClose.SetActive(false);
    }
}
