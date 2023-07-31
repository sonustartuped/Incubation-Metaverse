using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullMapHandler : MonoBehaviour
{
    public GameObject fullMap, fullMapCamera;

    [Header("Locations")]
    public MapLocations[] mapLocations;

    // Start is called before the first frame update
    void Start()
    {

    }


    public void Click_ViewFullMap(bool status)
    {
        fullMap.SetActive(status);
        fullMapCamera.SetActive(status);
        ThirdPersonController.instance.isControllingEnabled = !status;
    }

    public void SelectLocation(int index)
    {
        PlayerSelectionManager.instance.currPlayerData.player.transform.position = mapLocations[index].locationTransform.position;
    }
}

[System.Serializable]
public class MapLocations
{
    public Transform locationTransform;
}
