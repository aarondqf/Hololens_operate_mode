using UnityEngine;
using System.Collections;
using ZXing;
using UnityEngine.UI;

public class QRcode : MonoBehaviour
{
    /// <summary> 包含RGBA </summary>
    public Color32[] data;
    /// <summary> 判断是否可以开始扫描 </summary>
    private bool isScan;
    /// <summary> canvas上的RawImage，显示相机捕捉到的图像 </summary>
    public RawImage cameraTexture;
    /// <summary> canvas上的Text，显示获取的二维码内部信息 </summary>
    public Text QRcodeText;
    /// <summary> 相机捕捉到的图像 </summary>
    private WebCamTexture webCameraTexture;
    /// <summary> ZXing中的方法，可读取二维码中的内容 </summary>
    private BarcodeReader barcodeReader;
    /// <summary> 计时，0.5s扫描一次 </summary>
    private float timer = 0;

    /// <summary>
    /// 初始化
    /// </summary>
    /// <returns></returns>
    IEnumerator Start()
    {
        barcodeReader = new BarcodeReader();
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);//请求授权使用摄像头
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            WebCamDevice[] devices = WebCamTexture.devices;//获取摄像头设备
            string devicename = devices[0].name;
            Debug.Log("devicename is " + devicename);
            webCameraTexture = new WebCamTexture(devicename, 400, 400);//获取摄像头捕捉到的画面
            cameraTexture.texture = webCameraTexture;
            webCameraTexture.Play();
            isScan = true;
        }

    }

    /// <summary>
    /// 循环扫描，0.5秒扫描一次
    /// </summary>
    void Update()
    {
        if (isScan)
        {
            timer += Time.deltaTime;

            if (timer > 5f) //0.5秒扫描一次
            {
                Debug.Log("timer is ");
                Debug.Log(timer);
                StartCoroutine(ScanQRcode());//扫描
                timer = 0;
            }
        }
    }

    IEnumerator ScanQRcode()
    {
        data = webCameraTexture.GetPixels32();//相机捕捉到的纹理
        DecodeQR(webCameraTexture.width, webCameraTexture.height);
        yield return 0;
    }

    /// <summary>
    /// 识别二维码并显示其中包含的文字、URL等信息
    /// </summary>
    /// <param name="width">相机捕捉到的纹理的宽度</param>
    /// <param name="height">相机捕捉到的纹理的高度</param>
    private void DecodeQR(int width, int height)
    {
        var br = barcodeReader.Decode(data, width, height);
        if (br != null)
        {
            Debug.Log("解码结果：" + br.Text);
            QRcodeText.text = br.Text;
        }
        else
        {
            Debug.Log("没有解码出任何东西");
        }
    }
}