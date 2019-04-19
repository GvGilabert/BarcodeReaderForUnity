using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZXing;

public class CreateBarCodeBtn : MonoBehaviour
{
    private GameObject manager;
    private WebCamTexture cam;
    private BarcodeFormat format;
    private string code;

    private Creator cr;
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("Manager");
        cam = manager.GetComponent<Reader>().camTexture;
        cr = manager.GetComponent<Creator>();
        string frm = transform.parent.GetChild(1).GetComponent<Text>().text;
        if (!string.IsNullOrEmpty(frm))
            format = (BarcodeFormat) Enum.Parse(typeof(BarcodeFormat), frm, true);
        else
            format = BarcodeFormat.EAN_13;
        code = GetComponentInChildren<Text>().text;
    }

    public void ShowCodebar()
    {
        cam.Stop();
        print(code+" "+code.Length);
        cr.StartCode(format,code.Trim());
    }
}
