using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reader : MonoBehaviour
{

    public Text text;
    public GameObject scroll;
    public GameObject inputF;
    public AudioSource audioS;

    public void ReadDbToScreen()
    {
        SearchInDB(0);
    }

    public void SearchInDB(int id)
    {

        foreach (Transform item in scroll.transform)
        {
            //Clear the UI
            Destroy(item.gameObject);
        }

        //Draw data on UI
        foreach (var item in SqLiteDBManager.instance.GetCodes(id))
        {
            GameObject go = Instantiate(inputF, scroll.transform);
            //Fields on screen
            //ID
            go.transform.GetChild(0).GetComponent<Text>().text = item.Id.ToString();
            //NOMBRE
            go.transform.GetChild(1).GetComponent<Text>().text = item.Nombre;
            //RUBRO
            go.transform.GetChild(2).GetComponent<Text>().text = item.Rubro;
            //TIPO
            go.transform.GetChild(3).GetComponent<Text>().text = item.Tipo;
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}

