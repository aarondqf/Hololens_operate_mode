using System;
using System.Collections;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class WebDownload : MonoBehaviour
{
    private bool isLoadModel = false;
    private string bundle_url = "http://localhost:8000/Documents/_Projects/Hololens_projects/Resources/Bundles/hololens_fbxs.unity3d";

    private Text text;
    private AssetBundle abs = null;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickStartDownload()
    {
        StartCoroutine(LoadModel(bundle_url));
    }

    public IEnumerator LoadModel(string bundle_url)
    {
        Debug.Log("进入网络下载");
        //image = GameObject.Find("Image").GetComponent<Image>();                                                //动态获取UI上的组件
        text = GameObject.Find("Text").GetComponent<Text>();
        text.text = "开始下载";


        while (Caching.ready == false)
        {
            yield return null;
        }

        UnityWebRequest request2 = UnityWebRequestAssetBundle.GetAssetBundle(bundle_url);                 //传入地址2
        if (request2.isDone)
        {
            text.text = "true down";
        }
        text.text = (request2.downloadProgress * 100).ToString() + "%";

        yield return request2.Send();

        text.text = "开始从AssetBundle中获取资源 ";

        abs = (request2.downloadHandler as DownloadHandlerAssetBundle).assetBundle;                //获取连接请求2，返回AssetBundle资源
        text.text = "AssetBundle ab2" + abs.ToString();

        GameObject objPart = (GameObject)Instantiate(abs.LoadAsset("ugTest1"));
        text.text = "GameObject objPart";

        string[] fbx_names = abs.GetAllAssetNames();

        for(int i = 0; i < fbx_names.Length; i++)
        {
            Debug.Log(fbx_names[i]);
        }

    }
}
