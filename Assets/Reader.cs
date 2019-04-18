using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;

public class Reader : MonoBehaviour
{

    private WebCamTexture camTexture;
    public Text text;
    public RawImage rawImg;
    public List<string> codes;
    public GameObject scroll;
    public InputField inputF;
    public bool reading;

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
                    codes.Add(result.Text);
                    InputField go = Instantiate(inputF, scroll.transform);
                    go.text = result.Text;
                    reading = false;
                    codes.Add(result.Text);
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning(ex.Message);
            }
        }
    }

    public void SwitchBtn(Text btn)
    {
        reading = !reading;
        btn.text = (btn.text == "READ") ? "STOP" : "READ";
    }
}
