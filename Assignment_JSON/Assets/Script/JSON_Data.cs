using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using UnityEngine.UI;
using UnityEngine.Networking;

public class JSON_Data : MonoBehaviour
{

    public Text MessageOfTheDay;
    string json;
    public List<Root> _Root;

    public GameObject cell;
    public GameObject content;

    int DataCount;
    Root response;
    // Use this for initialization
    void Start()
    {

        
        StartCoroutine(GetRequest("https://api.jsonbin.io/b/5f56071c4d8ce411138a7d14"));
    }

    int n = 0;
    IEnumerator GetRequest(string uri)
    {
        MessageOfTheDay.text = "Loading... Please Wait";
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                json = webRequest.downloadHandler.text;
                Debug.Log(pages[page] + ":\nReceived: " + json);
                
                response = JsonConvert.DeserializeObject<Root>(json);

                DataCount = response.items.Count;

            }
        }
        if (response.items.Count != 0)
        {
            Debug.Log("Data Pulled Success");
            Invoke("SetDataToUI", 1f);
        }
        else
        {
            MessageOfTheDay.text = "Loading Faild or Network Error";
        }
       
    }
  



    /// <summary>
    /// 
    /// </summary>
    public void SetDataToUI()
    {
        SetData();
       // StartCoroutine(setImage(imageLink, DataCount));
    }
    void SetData()
    {

        MessageOfTheDay.text = response.messageOfTheDay;
       for (int i = 0; i < DataCount; i++)
        {

            string path = response.imagePath.ToString() + response.items[i].image.ToString();
            string title = response.items[i].title.ToString();
            GameObject row = Instantiate(cell);
            row.transform.SetParent(content.transform,false);
            row.GetComponent<TableData>().SetData(path,title,response.items[i].address, response.items[i].available); 
        }
    }

   
}


[System.Serializable]
public class Item
{
    public string title { get; set; }
    public string image { get; set; }
    public bool available { get; set; }
    public string address { get; set; }
}

[System.Serializable]
public class Root
{
    public string messageOfTheDay { get; set; }
    public string imagePath { get; set; }
    public List<Item> items { get; set; }
}


