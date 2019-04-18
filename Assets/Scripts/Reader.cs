using System.Collections;
using System.Collections.Generic;
using ExampleProject;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;

public class Reader : MonoBehaviour
{

    private WebCamTexture camTexture;
    public Text text;
    public RawImage rawImg;
    public GameObject scroll;
    public GameObject inputF;
    public bool reading;
    public Text btn;

    void Start()
    {
        //Start cam
        camTexture = new WebCamTexture();
        //Show cam on texture
        rawImg.texture = camTexture;
        rawImg.material.mainTexture = camTexture;

        if (camTexture != null)
        {
            camTexture.Play();
        }
        //Get elements from Db and list them
        ReadDbToScreen();
    }

    void Update()
    {
        if (reading)
        {
            try
            {
                IBarcodeReader barcodeReader = new BarcodeReader();
                // decode the current frame
                var result = barcodeReader.Decode(camTexture.GetPixels32(),
                    camTexture.width, camTexture.height);
                if (result != null)
                {
                    text.text = result.Text;
                    SwitchBtn();
                    ExampleDB.instance.InsertCodes(result.BarcodeFormat.ToString(),result.Text);
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning(ex.Message);
            }
        }
    }

    public void SwitchBtn()
    {
        reading = !reading;
        btn.text = (btn.text == "READ") ? "STOP" : "READ";
    }

    public void ReadDbToScreen()
    {
        var data = ExampleDB.instance.GetCodes(50);
        for (int i = 0; i < data.GetLength(0); i++)
        {
            GameObject go = Instantiate(inputF, scroll.transform);
            go.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = data[i, 1];
            go.transform.GetChild(1).GetComponent<Text>().text = data[i, 0];
        }
    }
    
}

