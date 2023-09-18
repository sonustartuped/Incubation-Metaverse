using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class helpbtn : MonoBehaviour
{
    
    public GameObject helpPanel;

    public void ToggleHelpPanel()
    {
        helpPanel.SetActive(true);
    }

    public void CloseHelpPanel()
    {
        helpPanel.SetActive(false);
    }
}
