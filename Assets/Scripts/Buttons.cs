using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buttons : MonoBehaviour
{
    public GameObject editScreen;
    public Text id;
    public InputField nombre;
    public InputField rubro;
    public InputField tipo;
    public InputField search;
    public AudioSource audioS;
    public SqLiteDBManager sqlManager;
    public ReadFromWebService webServiceManager;
    bool newData;
    Reader reader;


    void Start()
    {
        reader = GetComponent<Reader>();
        sqlManager = SqLiteDBManager.instance;
        webServiceManager = GetComponent<ReadFromWebService>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            Search();
    }

    public void EditScreenOC(GameObject p)
    {
        SwitchActive(editScreen);
        id.text = p.transform.GetChild(0).transform.GetComponent<Text>().text;
        nombre.text = p.transform.GetChild(1).transform.GetComponent<Text>().text;
        //rubro.text = p.transform.GetChild(2).transform.GetComponent<Text>().text;
        //tipo.text = p.transform.GetChild(3).transform.GetComponent<Text>().text;
    }

    public void Cancel()
    {
        SwitchActive(editScreen);
        newData = false;
    }

    void SwitchActive(GameObject go)
    {
        go.SetActive(!go.activeInHierarchy);
    }

    public void UpdateLocalRegister()//PUT local db only 
    {
        sqlManager.UpdateData(int.Parse(id.text), nombre.text);
        Cancel();
        reader.ReadDbToScreen();
    }

    public void UpdateApiRegister()//PUT web service and local
    {
        sqlManager.UpdateData(int.Parse(id.text), nombre.text);
        webServiceManager.UpdateRegister(new TestModel { Id = int.Parse(id.text), ProductCode = nombre.text });
        Cancel();
        reader.ReadDbToScreen();
    }

    public void AddApiRegister()//POST web service and local
    {

        webServiceManager.UploadRegister(new TestModel { ProductCode = nombre.text });
        webServiceManager.UpdateDBFromCloud();
        Cancel();
    }

    public void AddRegisterLocal()//POST local
    {
        if (string.IsNullOrEmpty(nombre.text))
            GetComponent<PopUpMsg>().NewMsg("Error, campo vacio");
        else
        { 
        sqlManager.InsertCodesLocal(nombre.text);
        Cancel();
        reader.ReadDbToScreen();
        }
    }

    public void Search()
    {
        int val = 0;
        if(int.TryParse(search.text, out val))
        {
            reader.SearchInDB(val);
            audioS.Play();
        }
    }

    public void NewReg()
    {
        id.text = "";
        nombre.text = "";
        SwitchActive(editScreen);
        newData = true;
        
    }
    public void SaveLocal()
    {
        if (newData)
        {
            AddRegisterLocal();
        }
        else
        {
            UpdateLocalRegister();
        }
    }
    public void SaveCloud()
    {
        if (newData)
        {
            AddApiRegister();
        }
        else
        {
            UpdateApiRegister();
        }
    }

    public void UploadChanges()
    {
        webServiceManager.UploadDb();
        
    }
}
