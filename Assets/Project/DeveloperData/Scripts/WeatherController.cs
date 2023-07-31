using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.IO;
using UnityEngine.Networking;
//using Unity.Plastic.Newtonsoft.Json;

public class WeatherController : MonoBehaviour
{
    const string api_Key = "88169749096b61e3b85398905927f53c";
    public string city_Id;
    [Header("Time in minutes")]
    public float apiCheckMaxTime;

    float apiCheckCountDown;
    //WeatherInfo weatherInfo;

    // Start is called before the first frame update
    void Start()
    {
        if (city_Id == "")
        {
            city_Id = "6453383";
        }

        apiCheckCountDown = apiCheckMaxTime * 60;
        CheckWeatherStatus();
    }

    //private void Update()
    //{
    //    apiCheckCountDown -= Time.deltaTime;
    //    if (apiCheckCountDown <= 0)
    //    {
    //        apiCheckCountDown = apiCheckMaxTime * 60;
    //        CheckWeatherStatus();
    //    }
    //}

    public void CheckWeatherStatus()
    {
        StartCoroutine(GetWeather_Coroutine());
    }

    IEnumerator GetWeather_Coroutine()
    {
        //string.Format("http://api.openweathermap.org/data/2.5/weather?id={0}&APPID={1}", city_Id, api_Key)
        using (UnityWebRequest request = UnityWebRequest.Get("https://catfact.ninja/fact"))
        {
            yield return request.SendWebRequest();

            while (!request.isDone)
            {
                yield return null;
            }

            byte[] result = request.downloadHandler.data;

            string jsonResponse = System.Text.Encoding.Default.GetString(result);
            //weatherInfo = JsonUtility.FromJson<WeatherInfo>(jsonResponse);

            Fact fact = JsonUtility.FromJson<Fact>(request.downloadHandler.text);
            Debug.Log("Fact - " + fact.fact);
            //Debug.Log("Current Weather - " + weatherInfo.name);
        }
    }
}

[SerializeField]
public class Fact
{
    public string fact { get; set;}
    public int length { get; set;}
}

//[SerializeField]
//public class Weather
//{
//    public int id;
//    public string main;
//}
//[SerializeField]
//public class WeatherInfo
//{
//    public int id;
//    public string name;
//    public List<Weather> weather;
//}
