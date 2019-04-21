using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.Common;

[RequireComponent(typeof(RawImage))]
public class Creator : MonoBehaviour {

    public RawImage cRawImage;
    public Text txt;

    public void StartCode(BarcodeFormat format,string code)
    {
        // Generate the texture
        Texture2D tex = GenerateBarcode(code, format,cRawImage.texture.width,cRawImage.texture.height);
        // Setup the RawImage
        cRawImage.texture = tex;
        txt.text = code;
    }

    public Texture2D GenerateBarcode(string data, BarcodeFormat format, int width, int height)
    {
        BarcodeWriter writer = new BarcodeWriter
        {
            Format = format,
            Options = new EncodingOptions
            {
                Height = height,
                Width = width
            }
        };
        Color32[] pixels = writer.Write(data);
        Texture2D tex = new Texture2D(width, height);
        tex.SetPixels32(pixels);
        tex.Apply();
        return tex;
    }

}
