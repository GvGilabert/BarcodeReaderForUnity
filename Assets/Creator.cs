using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.Common;

[RequireComponent(typeof(RawImage))]
public class Creator : MonoBehaviour {
    [SerializeField] private BarcodeFormat format = BarcodeFormat.QR_CODE;
    [SerializeField] private string data = "test";
    [SerializeField] private int width = 512;
    [SerializeField] private int height = 512;
    public RawImage cRawImage;

    private void Start()
    {
        // Generate the texture
        Texture2D tex = GenerateBarcode(data, format, width, height);
        // Setup the RawImage
        cRawImage.texture = tex;
        cRawImage.rectTransform.sizeDelta = new Vector2(tex.width, tex.height);

    }
    private Texture2D GenerateBarcode(string data, BarcodeFormat format, int width, int height)
    {
        BarcodeWriter writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new EncodingOptions
            {
                Height = height,
                Width = width
            }
        };
        Color32[] pixels = writer.Write("data to encode");
        Texture2D tex = new Texture2D(width, height);
        tex.SetPixels32(pixels);
        tex.Apply();
        return tex;
    }

}
