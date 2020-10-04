﻿using System;
using System.Collections;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class WebDownload : MonoBehaviour
{
    private bool isLoadModel = false;
    //设置下载模型的网址
    private string bundle_url = g_scan_result;

    private TextMeshProUGUI text;
    private AssetBundle abs = null;

    private RawImage rawImage;
    private VideoPlayer videoPlayer;


    // Start is called before the first frame update
    void Start()
    {
        rawImage = GameObject.Find("PlayerCanva").GetComponent<RawImage>();
        videoPlayer = GameObject.Find("PlayerCanva").GetComponent<VideoPlayer>();
        //设置播放视频的链接
        videoPlayer.url = g_video_url;
        //videoPlayer.Pause();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickStartDownload()
    {
        StartCoroutine(LoadModel(bundle_url));
    }

    public void OnClickReturnScanScene()
    {
        SceneManager.LoadScene("UWPScan");
    }

    public void OnClickPrintHello()
    {
        Debug.Log("Hello");
    }

    public void OnClickToPlay()
    {
        if (videoPlayer.isPlaying == false)
        {

            videoPlayer.Play();

            //text_PlayOrPause.text = "播放";

        }
    }

    public void OnClickToPause()
    {
        if (videoPlayer.isPlaying == true)
        {
            videoPlayer.Pause();

        }
    }

public IEnumerator LoadModel(string bundle_url)
    {
        Debug.Log("进入网络下载");
        //image = GameObject.Find("Image").GetComponent<Image>(); 
        //动态获取UI上的组件
        //text = GameObject.Find("Text(TMP)").GetComponent<TextMeshProUGUI>();
        //Get_all_component(GameObject.Find("Text(TMP)"));
        //text.text = "开始下载";


        while (Caching.ready == false)
        {
            yield return null;
        }

        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(bundle_url);                 //传入地址2
        if (request.isDone)
        {
            //text.text = "true down";
        }
        //text.text = (request.downloadProgress * 100).ToString() + "%";

        yield return request.Send();

        //text.text = "开始从AssetBundle中获取资源 ";

        abs = (request.downloadHandler as DownloadHandlerAssetBundle).assetBundle;                //获取连接请求，返回AssetBundle资源
        //text.text = "AssetBundle ab2" + abs.ToString();

        //GameObject objPart = (GameObject)Instantiate(abs.LoadAsset("ugTest1"));
        //text.text = "GameObject objPart";

        string[] fbx_names = abs.GetAllAssetNames();

        for(int i = 0; i < fbx_names.Length; i++)
        {
            int length = fbx_names[i].Split('/').Length;
            string name = fbx_names[i].Split('/')[length-1].Split('.')[0];
            fbx_names[i] = name;
            Debug.Log(fbx_names[i]);
        }

        for(int j = 0; j < fbx_names.Length; j++)
        {
            GameObject _ = (GameObject)Instantiate(abs.LoadAsset(fbx_names[j]));
            _.transform.localPosition = new Vector3(j, 0, 2);
            _.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            AddComponent(_);
        }

    }

    void AddComponent(GameObject obj)
    {
        obj.AddComponent<BoxCollider>();
        obj.AddComponent<NearInteractionGrabbable>();
        obj.AddComponent<ObjectManipulator>();
        obj.transform.localPosition = new Vector3(0, 0, 2);
        obj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
    }

    void Get_all_component(GameObject obj)
    {
        foreach (var component in obj.GetComponents<Component>())
        {
            Debug.Log("所有的组件是:");
            Debug.Log(component.GetType());
        }
    }
}
