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

using UnityEngine.SceneManagement;

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
    private float m_delayTime = 1f;
    #endregion

    private string scan_info = "default";


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
        m_webCameraTexture = new WebCamTexture(tDeviceName, 300, 300);//名字,宽,高
        //m_cameraTexture.texture = m_webCameraTexture;   //赋值图片信息
        m_webCameraTexture.Play();  //开始实时显示

        InvokeRepeating("CheckQRCode", 0, m_delayTime);
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
            scan_info = tResult.Text;
            Global.g_scan_result = scan_info.Split('?')[0];
            Global.g_video_url = scan_info.Split('?')[1];

            Debug.Log("Global.g_scan_result is ");
            Debug.Log(Global.g_scan_result);
            Debug.Log("Global.g_video_url");
            Debug.Log(Global.g_video_url);
            m_webCameraTexture.Stop();
            SceneManager.LoadScene("Web_load");
        }
        else
        {
            Debug.Log("scan fail");
        }
    }
    #endregion

}
