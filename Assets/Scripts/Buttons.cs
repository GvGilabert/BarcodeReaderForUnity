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
    Reader reader;

    void Start()
    {
        reader = GetComponent<Reader>();
    }

    public void EditScreenOC(GameObject p)
    {
        SwitchActive(editScreen);
        id.text = p.transform.GetChild(0).transform.GetComponent<Text>().text;
        nombre.text = p.transform.GetChild(1).transform.GetComponent<Text>().text;
        rubro.text = p.transform.GetChild(2).transform.GetComponent<Text>().text;
        tipo.text = p.transform.GetChild(3).transform.GetComponent<Text>().text;
    }

    public void Cancel()
    {
        SwitchActive(editScreen);
    }

    void SwitchActive(GameObject go)
    {
        go.SetActive(!go.activeInHierarchy);
    }

    public void UpdateRegister()
    {
        SqLiteDBManager.instance.UpdateData(int.Parse(id.text), nombre.text, rubro.text, tipo.text);
        Cancel();
        reader.ReadDbToScreen();
    }

    public void Search()
    {
        reader.SearchInDB(int.Parse(search.text));
        print("search " + int.Parse(search.text));
    }
}
