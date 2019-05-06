using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

[System.Serializable]
public class TestModel
{
    public int Id;
    public string Nombre;
    public string Rubro;
    public string Tipo;
}

public class ReadFromWebService : MonoBehaviour
{
    public static ReadFromWebService instance;
    public List<TestModel> data;
    public SqLiteDBManager dBManager;
    private string ApiUrl = "http://localhost:52313/api/values/";

    void Awake()
    {
        //Singleton
        if (instance != null)
            Destroy(this.gameObject);
        else
            instance = this;
        dBManager = GetComponent<SqLiteDBManager>();   
    }

    public void UpdateDBFromCloud()
    {
        //Clear local db
        dBManager.ClearData();
        //Call Webservice
        StartCoroutine(GetRequest(ApiUrl));
    }

    public void UploadDb()
    {
        StartCoroutine(PostRequest(ApiUrl, SqLiteDBManager.instance.GetCodes(0)));
    }
 

    IEnumerator GetRequest(string uri)
    {
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
                data = JsonConvert.DeserializeObject<List<TestModel>>(webRequest.downloadHandler.text);
                //Insert data to db
                foreach (var item in data)
                {
                    dBManager.InsertCodes(item.Id,item.Nombre,item.Rubro,item.Tipo);
                }
            }
            GetComponent<Reader>().ReadDbToScreen();
        }
    }
    IEnumerator PostRequest(string uri, List<TestModel> dbData)
    {
        var jsonString = JsonConvert.SerializeObject(dbData);
        byte[] byteData = System.Text.Encoding.ASCII.GetBytes(jsonString.ToCharArray());

        UnityWebRequest unityWebRequest = new UnityWebRequest(uri, "POST");
        unityWebRequest.uploadHandler = new UploadHandlerRaw(byteData);
        unityWebRequest.SetRequestHeader("Content-Type", "application/json");
        yield return unityWebRequest.Send();

        if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
        {
            Debug.Log(unityWebRequest.error);
        }
        else
        {
            Debug.Log("Form upload complete! Status Code: " + unityWebRequest.responseCode);
            Debug.Log(unityWebRequest);
        }
    }
}
