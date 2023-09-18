using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    public InputField nameInputField;
    public GameObject enterNameParent, playUIParent, EnterDetailsUI, OTPUIBackground;
    public GameObject mainMenuUIParent, activityUIParent;
    public Text welcomeNameText;

    [Header("Gender Selection")]
    public Toggle femaleToggle;

    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        ThirdPersonController.instance.isControllingEnabled = false;
        enterNameParent.SetActive(!PlayerPrefs.HasKey("playerName"));
        playUIParent.SetActive(PlayerPrefs.HasKey("playerName"));

        if (PlayerPrefs.HasKey("playerName"))
        {
            welcomeNameText.text = "Welcome! " + PlayerPrefs.GetString("playerName");
            int f = PlayerPrefs.GetInt("isFemale");

            if (f==0)
            {
                PlayerSelectionManager.instance.SetPlayer(false);
            }
            else
            {
                PlayerSelectionManager.instance.SetPlayer(true);
            }
        }

    }

    public void Click_SubmitName()
    {
        PlayerSelectionManager.instance.SetPlayer(femaleToggle.isOn);

        if (femaleToggle.isOn)
        {
            PlayerPrefs.SetInt("isFemale", 1);
        }
        else
        {
            PlayerPrefs.SetInt("isFemale", 0);
        }
        

        if (nameInputField.text.Equals(""))
        {
            SetNameToDB("Avtar");
        }
        else
        {
            SetNameToDB(nameInputField.text);
        }
      
    }


   // public void Otp_Verify()
   // {
   //     OTPBackground.SetActive(false);
   // }
    public void PlayGame()
    {
        playUIParent.SetActive(false);
        mainMenuUIParent.SetActive(false);
        activityUIParent.SetActive(true);
        ThirdPersonController.instance.isControllingEnabled = true;
        PlayerSelectionManager.instance.currPlayerData.nameText.SetText(PlayerPrefs.GetString("playerName"));
    }

    void SetNameToDB(string name)
    {
        PlayerPrefs.SetString("playerName", name);
        welcomeNameText.text = "Welcome " + name;
        //PlayerSelectionManager.instance.currPlayerData.nameText.SetText(PlayerPrefs.GetString("playerName"));
    }

    public void ClickChangeName()
    {
        enterNameParent.SetActive(true);
        playUIParent.SetActive(false);
        nameInputField.text = PlayerPrefs.GetString("playerName");
    }
}
