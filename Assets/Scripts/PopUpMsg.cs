using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpMsg : MonoBehaviour
{
    public Text msg;
    public GameObject popUpwindow;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
   public void NewMsg(string msgText)
    {
        if (!popUpwindow.activeSelf)
        {
            msg.text = msgText;
            popUpwindow.SetActive(true);
        }
    }

    public void Close()
    {
        popUpwindow.SetActive(false);
    }
}
