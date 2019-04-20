using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZXing;

public class CreateBarCodeBtn : MonoBehaviour
{
    private GameObject manager;
    private Reader cam;
    private BarcodeFormat format;
    private string code;
    private Creator cr;

    public void lateStart()
    {
        manager = GameObject.FindGameObjectWithTag("Manager");
        cam = manager.GetComponent<Reader>();
        cr = manager.GetComponent<Creator>();
        string frm = transform.parent.GetChild(1).GetComponent<Text>().text;
        format = (BarcodeFormat)Enum.Parse(typeof(BarcodeFormat), frm, true);
        code = GetComponentInChildren<Text>().text;
    }

    public void ShowCodebar()
    {
        cam.StopVideo();
        cr.StartCode(format,code.Trim());
    }
}
