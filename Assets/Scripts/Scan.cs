/*************************************************
 * 项目名称：Unity实现启用摄像头扫描与生成二维码
 * 脚本创建人：魔卡
 * 脚本创建时间：2017.12.20
 * 脚本功能：二维码识别生成控制类
 * ***********************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;

public class Scan : MonoBehaviour
{
    #region 扫描二维码
    //定义一个用于存储调用电脑或手机摄像头画面的RawImage
    public RawImage m_cameraTexture;

    //摄像头实时显示的画面
    private WebCamTexture m_webCameraTexture;

    //申请一个读取二维码的变量
    private BarcodeReader m_barcodeRender = new BarcodeReader();

    //多久检索一次二维码
    private float m_delayTime = 3f;
    #endregion

    #region 生成二维码
    //用于显示生成的二维码RawImage
    public RawImage m_QRCode;

    //申请一个写二维码的变量
    private BarcodeWriter m_barcodeWriter;
    #endregion


    #region 扫描二维码
    void Start()
    {
        //调用摄像头并将画面显示在屏幕RawImage上
        WebCamDevice[] tDevices = WebCamTexture.devices;    //获取所有摄像头
        for (int i = 0; i < tDevices.Length; i++)
        {
            Debug.LogError("length cam is " + tDevices.Length);
            Debug.LogError(tDevices[i].name);
        }
        string tDeviceName = tDevices[0].name;  //获取第一个摄像头，用第一个摄像头的画面生成图片信息
        m_webCameraTexture = new WebCamTexture(tDeviceName, 256, 256);//名字,宽,高
        m_cameraTexture.texture = m_webCameraTexture;   //赋值图片信息
        m_webCameraTexture.Play();  //开始实时显示

        InvokeRepeating("CheckQRCode", 0, m_delayTime);

        ShowQRCode("aarond", 256, 256);
    }
    /// <summary>
    /// 检索二维码方法
    /// </summary>
    void CheckQRCode()
    {
        //存储摄像头画面信息贴图转换的颜色数组
        Color32[] m_colorData = m_webCameraTexture.GetPixels32();

        //将画面中的二维码信息检索出来
        var tResult = m_barcodeRender.Decode(m_colorData, m_webCameraTexture.width, m_webCameraTexture.height);

        if (tResult != null)
        {
            Debug.Log(tResult.Text);
        }
        else
        {
            Debug.Log("scan fail");
        }
    }
    #endregion

    #region 传递字符串生成二维码
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //在这种写法中  宽高必须256  否则报错
            ShowQRCode("魔卡先生", 256, 256);
        }
    }
    /// <summary>
    /// 显示绘制的二维码
    /// </summary>
    /// <param name="s_formatStr">扫码信息</param>
    /// <param name="s_width">码宽</param>
    /// <param name="s_height">码高</param>
    void ShowQRCode(string s_str, int s_width, int s_height)
    {
        //定义Texture2D并且填充
        Texture2D tTexture = new Texture2D(s_width, s_height);

        //绘制相对应的贴图纹理
        tTexture.SetPixels32(GeneQRCode(s_str, s_width, s_height));

        tTexture.Apply();

        //赋值贴图
        m_QRCode.texture = tTexture;

        Debug.LogError("ShowQRCode");
    }
    /// <summary>
    /// 返回对应颜色数组
    /// </summary>
    /// <param name="s_formatStr">扫码信息</param>
    /// <param name="s_width">码宽</param>
    /// <param name="s_height">码高</param>
    Color32[] GeneQRCode(string s_formatStr, int s_width, int s_height)
    {
        //设置中文编码格式，否则中文不支持
        QrCodeEncodingOptions tOptions = new QrCodeEncodingOptions();
        tOptions.CharacterSet = "UTF-8";
        //设置宽高
        tOptions.Width = s_width;
        tOptions.Height = s_height;
        //设置二维码距离边缘的空白距离
        tOptions.Margin = 3;

        //重置申请写二维码变量类       (参数为：码格式（二维码、条形码...）    编码格式（支持的编码格式）    )
        m_barcodeWriter = new BarcodeWriter { Format = BarcodeFormat.QR_CODE, Options = tOptions };

        //将咱们需要隐藏在码后面的信息赋值上
        return m_barcodeWriter.Write(s_formatStr);
    }
    #endregion

}
