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
    public string ProductCode;
    public string Description;
    public float Price;
    public TestModel() { Price = 10; Description = "description"; }
}

public class ReadFromWebService : MonoBehaviour
{
    public static ReadFromWebService instance;
    public List<TestModel> data;
    public SqLiteDBManager dBManager;
    private string ApiUrl = "http://192.168.7.6:8866/api/products";
    PopUpMsg popUpMsg;

    void Awake()
    {
        //Singleton
        if (instance != null)
            Destroy(this.gameObject);
        else
            instance = this;   
    }

    void Start()
    {
        dBManager = GetComponent<SqLiteDBManager>();
        popUpMsg = GetComponent<PopUpMsg>();
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

    public void UploadRegister(TestModel data)
    {
        StartCoroutine(PostRequestOneReg(ApiUrl, data));
    }

    public void UpdateRegister(TestModel data)
    {
        string url = ApiUrl + "/" + data.Id;
        StartCoroutine(PutRequest(url, data));
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
                popUpMsg.NewMsg(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                data = JsonConvert.DeserializeObject<List<TestModel>>(webRequest.downloadHandler.text);
                //Insert data to db
                foreach (var item in data)
                {
                    dBManager.InsertCodes(item.Id,item.ProductCode);
                }
            }
            GetComponent<Reader>().ReadDbToScreen();
        }
    }
    IEnumerator PostRequest(string uri, List<TestModel> dbData)
    {
        foreach (var item in dbData)
        {
            var jsonString = JsonConvert.SerializeObject(item);
            byte[] byteData = System.Text.Encoding.ASCII.GetBytes(jsonString.ToCharArray());

            UnityWebRequest unityWebRequest = new UnityWebRequest(uri, "POST");
            unityWebRequest.uploadHandler = new UploadHandlerRaw(byteData);
            unityWebRequest.SetRequestHeader("Content-Type", "application/json");
            yield return unityWebRequest.Send();

            if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
            {
                popUpMsg.NewMsg(unityWebRequest.error);
            }
            else
            {
                //popUpMsg.NewMsg("Form upload complete! Status Code: " + unityWebRequest.responseCode);
            }
        }
         
    }
    IEnumerator PostRequestOneReg(string uri, TestModel dbData)
    {
        var jsonString = JsonConvert.SerializeObject(dbData);
        byte[] byteData = System.Text.Encoding.ASCII.GetBytes(jsonString.ToCharArray());

        UnityWebRequest unityWebRequest = new UnityWebRequest(uri, "POST");
        unityWebRequest.uploadHandler = new UploadHandlerRaw(byteData);
        unityWebRequest.SetRequestHeader("Content-Type", "application/json");
        yield return unityWebRequest.Send();

        if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
        {
            popUpMsg.NewMsg(unityWebRequest.error);
        }
        else
        {
            popUpMsg.NewMsg("Form upload complete! Status Code: " + unityWebRequest.responseCode);
        }
    }

    IEnumerator PutRequest(string uri, TestModel model)
    {
        var jsonString = JsonConvert.SerializeObject(model);
        byte[] byteData = System.Text.Encoding.ASCII.GetBytes(jsonString.ToCharArray());

        UnityWebRequest unityWebRequest = new UnityWebRequest(uri, "PUT");
        unityWebRequest.uploadHandler = new UploadHandlerRaw(byteData);
        unityWebRequest.SetRequestHeader("Content-Type", "application/json");
        yield return unityWebRequest.Send();

        if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
        {
            popUpMsg.NewMsg(unityWebRequest.error);
        }
        else
        {
            popUpMsg.NewMsg("Form upload complete! Status Code: " + unityWebRequest.responseCode);
        }
    }
}
