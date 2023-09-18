using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

public class BackedServiceHandler : MonoBehaviour
{
    public InputField emailInputField;
    public TMP_Text validationMessageText;
    public TMP_Text otpErrorMessageText;
    public InputField otpInputField;

   
    private string email ;




  //  public void SubmitLoginDetails()
  //  {
  //      string email = emailInputField.text;
  //      StartCoroutine(PostLoginData(email));
  //  }
  //  public class User
  //  {
  //      public string id;
  //      public string name;
  //      public DateTime updatedAt;
  //      public string fullName;
  //      public string email;
  //      public string phone;
  //      public List<string> myWalletIds;
  //      public string dateofBirth;
  //      public string gender;
  //      public string avatarImageUrl;
  //      public string deviceIPAddress;
  //      public string role;
  //      public Dictionary<string, int> myModules;
  //      public string password;  
  //  }
  //
  //
  //  private IEnumerator PostLoginData(string email)
  //  {
//          User user = new User()
//          {
//              id = "",
//              name = "SonuSingh",
//              updatedAt = DateTime.UtcNow,
//              fullName = "Singh",
//              email = email,
//              phone = "+917861616160",
//              myWalletIds = new List<string>() { "" },
//              dateofBirth = "",
//              gender = "M",
//              avatarImageUrl = "",
//              deviceIPAddress = "",
//              role = "Student",
//              myModules = new Dictionary<string, int>(),
//              password = "",          
//          };
//
//        string json = JsonUtility.ToJson(user);
//        StartCoroutine(PostRoutine("https://api.wizar.startuped.xyz/api/v1/User",json));
//
//         yield return null;
//
//     }
//    IEnumerator PostRoutine(string url, string bodyJson)
//    {
//        using (UnityWebRequest unityWebRequest = UnityWebRequest.PostWwwForm(url,bodyJson))
//          {
//              unityWebRequest.SetRequestHeader("Content-Type", "application/json");
//
//            byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJson);
//            unityWebRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
//            unityWebRequest.downloadHandler = new DownloadHandlerBuffer();
//
//            yield return unityWebRequest.SendWebRequest();
//
//            if (!String.IsNullOrEmpty(unityWebRequest.error))
//            {
//                Debug.Log("Error " + unityWebRequest.error);
//            }
//            else if (unityWebRequest.isDone)
//
//            {              
//                Debug.Log("Data Succesefully Sent.");        
//            }
//        }
//  //  }

    public void ValidateEmailAndRequestOTP()
    {
        string email = emailInputField.text;    
        if (IsEmailValid(email))
        {
            StartCoroutine(SendOTPTask(email));          
        }
        else
        {
            validationMessageText.text = "Invalid email format. Please enter a valid email.";
        }
    }

    private bool IsEmailValid(string email)
    {
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        return Regex.IsMatch(email, pattern);
    }

    IEnumerator SendOTPTask(string email)
    {
        string url = $"https://startuped.dev/api/User/GetOTP?Email={email}";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error sending OTP: " + request.error);
            }
            else
            {
                Debug.Log("OTP sent successfully.");
                MenuManager.instance.OTPUIBackground.SetActive(true);
                //MenuManager.instance;
            }
        }
    }


    public void VerifyOTP()
    {
        string email = emailInputField.text;
        string otp = otpInputField.text;
        StartCoroutine(VerifyOTPTask(email, otp));
    }

    
    IEnumerator VerifyOTPTask(string email, string otp)
    {
        string url = $"https://startuped.dev/api/User/VerifyOTP?Email={email}&OTP={otp}";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error verifying OTP: " + request.error);
                otpErrorMessageText.text ="Wrong OTP";
            }
            else
            {
                string responseText = request.downloadHandler.text;
                if (responseText.Contains("User with email"))
                {
                // Debug.Log("OTP verification failed.");  
                   otpErrorMessageText.text ="Enter Correct OTP";
                }
                else
                {
                    Debug.Log("OTP verification successful.");
                    MenuManager.instance.EnterDetailsUI.SetActive(false);
                    MenuManager.instance.OTPUIBackground.SetActive(false);
                    MenuManager.instance.playUIParent.SetActive(true);
                    wallet();  
                }
            }
        }
    }


    public Text walletAmountText;
    public string apiEndpoint = "https://api.wizar.startuped.xyz/api/v1/Wallet/Email/{email}";

    public int UpdateAmount;

    public void wallet()
    {
       
        StartCoroutine(FetchWalletAmount());
    }

    IEnumerator FetchWalletAmount()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(apiEndpoint))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                
                string jsonResponse = webRequest.downloadHandler.text;
                WalletData wallets = JsonConvert.DeserializeObject<WalletData>(jsonResponse);
                UpdateAmount = wallets.Wallets["Ipt"];
                walletAmountText.text = "$" + UpdateAmount.ToString();
                
            }
        }
    }





    [Serializable]
    public class WalletData : BaseModel
    {
        public Dictionary<string, int> Wallets { get; set; }
        public string? UserId { get; set; }   // Foregin Key

                // Reference not is use in a REST based transactional only system
    }

    [Serializable]
    public abstract class BaseModel
    {
    

        public string Id { get; set; }             // Unique & Primary (Also, used as Foreign Key by other Models) // Created DateTime is already hashed inside.
        public string? Name { get; set; }
    
        public string Email { get; set; }
        // Generic Name Descriptors
        public DateTime? UpdatedAt { get; set; }
    }


   
  
    public string currency;
    public int newAmount;
    
    public void SubmitIptButton()
    {
        string email = emailInputField.text;
        string currency = "Ipt";
        int newAmount =  100;

       
        StartCoroutine(SendUpdateRequest(email,currency,newAmount));
    }
   
     IEnumerator SendUpdateRequest(string email, string currency, int newAmount)
{
    string apiUrl = $"https://api.wizar.startuped.xyz/api/v1/Wallet/Email={email}/Deposit/Currency={currency}&Amount={newAmount}";

    Debug.Log("Here");
    UnityWebRequest request = UnityWebRequest.Put(apiUrl, ""); // The second argument should be the data you want to send, but in this case, it's empty.

    request.SetRequestHeader("Content-Type", "application/json");

    yield return request.SendWebRequest();

    if (request.result == UnityWebRequest.Result.Success)
    {
        Debug.Log("Update successful!"); 
        UpdateAmount += newAmount;
        walletAmountText.text = "$" + UpdateAmount.ToString();
    }
    else
    {
        Debug.LogError("Update failed: " + request.error);   
    }
}


}


