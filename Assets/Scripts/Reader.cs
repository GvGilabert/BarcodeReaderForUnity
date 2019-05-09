using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Reader : MonoBehaviour
{

    public GameObject scroll;
    public GameObject inputFieldPrefab;
    public List<TestModel> itemsList;
    public List<GameObject> Pool;
    public GameObject poolContainer;
    public GameObject rowPrefab;
    public int position;
    public int rowsPerPage;
    double maxPage = 1;

    public void Start()
    {
        foreach (Transform child in poolContainer.transform)
        {
            Pool.Add(child.gameObject);
        }
        ReadDbToScreen();
        maxPage = System.Math.Ceiling((float)itemsList.Count / rowsPerPage);
    }


    public void ReadDbToScreen()
    {
        SearchInDB(0);
        position = 0;
        MoveDown(); 
    }

    public void SearchInDB(int id)
    {
        itemsList = SqLiteDBManager.instance.GetCodes(id);
        position = 0;
        MoveDown();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void MoveDown()
    {
        int direction = 1;
        if (position < maxPage)
        {
            position += direction;
            int skipValue = (position - 1) * rowsPerPage;
            List<TestModel> lista = itemsList.Skip(skipValue).Take(rowsPerPage).ToList();
            int poolIndex = 0;
            foreach (var item in Pool)
            {
                item.transform.GetChild(0).GetComponent<Text>().text = "";
                item.transform.GetChild(1).GetComponent<Text>().text = "";
            }

            foreach (var item in lista)
            {
                Pool[poolIndex].transform.GetChild(0).GetComponent<Text>().text = item.Id.ToString();
                Pool[poolIndex].transform.GetChild(1).GetComponent<Text>().text = item.ProductCode;
                poolIndex++;
            }
        }
    }

    public void MoveUp()
    {
        int direction = -1;
        if (position > 1)
        {
            position += direction;
            int skipValue = (position - 1) * rowsPerPage;
            List<TestModel> lista = itemsList.Skip(skipValue).Take(rowsPerPage).ToList();
            int poolIndex = 0;
            foreach (var item in Pool)
            {
                item.transform.GetChild(0).GetComponent<Text>().text = "";
                item.transform.GetChild(1).GetComponent<Text>().text = "";
            }

            foreach (var item in lista)
            {
                Pool[poolIndex].transform.GetChild(0).GetComponent<Text>().text = item.Id.ToString();
                Pool[poolIndex].transform.GetChild(1).GetComponent<Text>().text = item.ProductCode;
                poolIndex++;
            }
        }
    }
}