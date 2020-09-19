using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXing;
using UnityEngine.UI;

public class QRCodeTest : MonoBehaviour
{
    public RawImage cameraTexture;//存储摄像头拍到的内容
    public RawImage show_cameraTexture;//展示摄像头拍到的内容

    private WebCamTexture webCamTexture;//摄像头的内容

    Color32[] data;

    BarcodeReader barcodeReader;//Zxing提供的读取摄像头内容的方法

    float interval = 0f;//做定时器用
    // Use this for initialization
    void Start()
    {
        //打开了摄像头
        WebCamDevice[] devices = WebCamTexture.devices;
        string deviceName = devices[0].name;
        webCamTexture = new WebCamTexture(deviceName, 350, 350);
        //cameraTexture.texture = webCamTexture;
        cameraTexture.texture = webCamTexture;
        //show_cameraTexture.texture = webCamTexture;
        webCamTexture.Play();

        barcodeReader = new BarcodeReader();
    }

    // Update is called once per frame
    void Update()
    {
        interval += Time.deltaTime;
        if (interval >= 3f)
        {
            ScanQRCode();
            interval = 0f;
        }
    }

    void ScanQRCode()
    {
        Debug.Log("进入扫描函数");
        Debug.Log("width is ");
        Debug.Log(webCamTexture.width);
        Debug.Log(webCamTexture.height);
        //GetPixels32是把格式转换为Color32的方法
        data = webCamTexture.GetPixels32();
        show_cameraTexture.texture = webCamTexture;
        //result存储读取的内容
        var result = barcodeReader.Decode(data, webCamTexture.width, webCamTexture.height);

        if (result != null)
        {
            Debug.Log(result.Text);
        }
    }
}