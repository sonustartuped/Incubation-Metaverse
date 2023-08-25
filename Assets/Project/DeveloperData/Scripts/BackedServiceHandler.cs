using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Networking;
using UnityEngine.UI;


public class BackedServiceHandler : MonoBehaviour
{
    public InputField emailInput;
    public void SubmitLoginDetails()
    {
        string email = emailInput.text;
        StartCoroutine(PostLoginData(email));
    }


    public class User
    {
        public string id;
      //public string name;
        public DateTime updatedAt;
      //public string fullName;
        public string email;
      //public string phone;
        public List<string> myWalletIds;
     // public string dateofBirth;
        public string gender;
        public string avatarImageUrl;
        public string deviceIPAddress;
        public string role;
        public Dictionary<string, int> myModules;
        public string password;
      
    }

  
    private IEnumerator PostLoginData(string email)
    {
        User user = new User()
        {

            id = "",
          //name = "SonuSingh",
            updatedAt = DateTime.UtcNow,
          //fullName = "Singh",
            email = email,
          //phone = "+917861616160",
            myWalletIds = new List<string>() { "" },
         //dateofBirth = "",
            gender = "M",
            avatarImageUrl = "",
            deviceIPAddress = "",
            role = "Student",
            myModules = new Dictionary<string, int>(),
            password = "",
           
        };

        string json = JsonUtility.ToJson(user);
        StartCoroutine(PostRoutine("https://api.wizar.startuped.xyz/api/v1/User",json));

         yield return null;

     }
    IEnumerator PostRoutine(string url, string bodyJson)
    {
        using (UnityWebRequest unityWebRequest = UnityWebRequest.PostWwwForm(url,bodyJson))
        {
            unityWebRequest.SetRequestHeader("Content-Type", "application/json");

            byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJson);
            unityWebRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            unityWebRequest.downloadHandler = new DownloadHandlerBuffer();

            yield return unityWebRequest.SendWebRequest();

            if (!String.IsNullOrEmpty(unityWebRequest.error))
            {
                Debug.Log("Error " + unityWebRequest.error);
            }
            else if (unityWebRequest.isDone)

            {              
                Debug.Log("Email Succesefully Sent.");        
            }
        }
    }

    


}