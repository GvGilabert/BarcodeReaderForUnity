using System.Collections;
using System.Collections.Generic;
using ExampleProject;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;

public class Reader : MonoBehaviour
{

    public WebCamTexture camTexture;
    public Text text;
    public RawImage rawImg;
    public GameObject scroll;
    public GameObject inputF;
    public bool reading;
    public Text btn;
    public AudioSource audioS;

    void Start()
    {
        //Start cam
        camTexture = new WebCamTexture();
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
                    ExampleDB.instance.InsertCodes
                                (result.BarcodeFormat.ToString(),result.Text);

                    ReadDbToScreen();
                    audioS.Play();
                    SwitchBtn();

                }
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning(ex.Message);
            }
        }
    }

    public void StartCam()
    {
        if (camTexture != null)
        {
            camTexture.Play();
        }
        //Show cam on texture
        rawImg.texture = camTexture;
        rawImg.material.mainTexture = camTexture;
    }

    public void SwitchBtn()
    {
        if (btn.text == "READ")
        {
            reading = true;
            btn.text = "STOP";
            StartCam();
        }
        else
        {
            StopVideo();
        } 
    }

    public void ReadDbToScreen()
    {
        foreach (Transform item in scroll.transform)
        {
            Destroy(item.gameObject);
        }
        var data = ExampleDB.instance.GetCodes(50);
        for (int i = 1; i < data.GetLength(0); i++)
        {
            if (string.IsNullOrEmpty(data[i,0]))
                return;
            GameObject go = Instantiate(inputF, scroll.transform);
            go.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = data[i, 1];
            go.transform.GetChild(1).GetComponent<Text>().text = data[i, 0];
            go.transform.GetChild(0).GetComponent<CreateBarCodeBtn>().lateStart();
        }
    }

    public void StopVideo()
    {
        camTexture.Stop();
        reading = false;
        btn.text = "READ";
    }
    
}

