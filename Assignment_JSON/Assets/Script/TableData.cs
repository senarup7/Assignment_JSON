using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
public class TableData : MonoBehaviour
{
    [SerializeField]
    Sprite LoackImage;

    public RowData rowData;
    Root response;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetData(string url,string title, string address, bool available)
    {
        StartCoroutine(setImage(url,title,address,available));
    }
    public IEnumerator setImage(string url, string title,string address, bool available)
    {

            Texture2D tex;
            tex = new Texture2D(4, 4, TextureFormat.DXT1, false);
            //url = response.imagePath + response.items[i].image;
            using (WWW www = new WWW(url))
            {
                yield return www;
                www.LoadImageIntoTexture(tex);


                rowData.SL_NO.texture = tex;
                rowData.TITLE.text = title;
                
                if (!available)
                {
                    rowData.LOCK.overrideSprite = LoackImage;
                    rowData.ADDRESS.text = null;
                    rowData.button.interactable = false;
            }
                else
                {
                    rowData.LOCK.transform.gameObject.SetActive(false);
                    rowData.ADDRESS.text = address;
                    rowData.button.interactable =  true;
                    
                }
                
            }
        
    }

    public void OnClick()
    {
        Application.OpenURL(rowData.ADDRESS.text);
    }
}
[System.Serializable]
public class RowData
{
    public RawImage SL_NO;
    public Button button;
    public Text TITLE;
    public Image LOCK;
    public Text ADDRESS;
    public string AVAILABLE;
}
