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
    private bool reading;
    private Creator cr;

    public Text text;
    public RawImage rawImg;
    public GameObject scroll;
    public GameObject inputF;
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
        cr = GetComponent<Creator>();
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
                    // Update DB
                    SqLiteDBManager.instance.InsertCodes
                                (result.BarcodeFormat.ToString(),result.Text);
                    ReadDbToScreen();
                    audioS.Play();
                    SwitchBtn();
                    cr.StartCode(result.BarcodeFormat,result.Text);
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
        if (btn.text == "READ")
        {
            reading = true;
            btn.text = "STOP";
            StartCam();
        }
        else
        {
            StopCam();
        } 
    }

    public void ReadDbToScreen()
    {
        foreach (Transform item in scroll.transform)
        {
            Destroy(item.gameObject);
        }
        var data = SqLiteDBManager.instance.GetCodes(50);
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

    public void StartCam()
    {
        if (camTexture != null)
        {
            camTexture.Play();
        }
        //Show cam on texture
        rawImg.transform.localEulerAngles = new Vector3(0, 0, 180);
        rawImg.texture = camTexture;
        rawImg.material.mainTexture = camTexture;
        text.text = "";
    }

    public void StopCam()
    {
        camTexture.Stop();
        reading = false;
        btn.text = "READ";
        rawImg.transform.localEulerAngles = new Vector3(0, 0, 0);
    }

    public void Quit()
    {
        Application.Quit();
    }
}

