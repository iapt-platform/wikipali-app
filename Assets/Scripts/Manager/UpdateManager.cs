using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Networking;
//版本更新
public class UpdateManager
{
    private UpdateManager() { }
    private static UpdateManager manager = null;
    //静态工厂方法 
    public static UpdateManager Instance()
    {
        if (manager == null)
        {
            manager = new UpdateManager();
        }
        return manager;
    }
    //安装APK
    public bool InstallApk(string apkPath)
    {
        AndroidJavaClass javaClass = new AndroidJavaClass("com.wikipali.apkupdatelibrary.Install");
        return javaClass.CallStatic<bool>("InstallApk", apkPath);
    }
    //void Start()
    //{
    //    StartCoroutine(Test());
    //}

    public IEnumerator Test()
    {
        if (!File.Exists(Application.persistentDataPath + "/a.apk"))
        {
            UnityWebRequest request = UnityWebRequest.Get(Application.streamingAssetsPath + "/a.apk");
            yield return request.SendWebRequest();
            File.WriteAllBytes(Application.persistentDataPath + "/a.apk", request.downloadHandler.data);
            InstallApk(Application.persistentDataPath + "/a.apk");
        }
        else
        {
            //print("已经存在，");
        }
    }
    public UpdateInfo currentUInfo;
    //点击检测更新
    public void CheckUpdateOpenPage(MonoBehaviour ui)
    {
        ////测试代码
        //UITool.ShowToastMessage(ui, Application.internetReachability.ToString(), 35);
     
        //try
        //{
        //    System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();
        //    PingReply pr = ping.Send("www.baidu.com", 3000);
        //    if (pr.Status == IPStatus.Success)
        //    {
        //        UITool.ShowToastMessage(ui, "ping成功", 35);
        //        return ;
        //    }
        //    else
        //    {
        //        UITool.ShowToastMessage(ui, "ping失败", 35);
        //        return;
        //    }
        //}
        //catch (Exception e)
        //{
        //    return;
        //}


        if (!NetworkMangaer.Instance().PingNetAddress())
        {
            UITool.ShowToastMessage(ui, "无网络连接", 35);
            return;
            // return false;
        }
        GetUpdateInfo();

    }
    //打开App自动检测更新显示红点
    public void CheckUpdateRedPoint()
    {
        if (!NetworkMangaer.Instance().PingNetAddress())
        {
            return;
            // return false;
        }
        GetUpdateInfoRedPoint();

    }
    public class UpdateInfo
    {
        public string version;
        public string downLoadUrl1;//国内
        public string downLoadUrl2;//国外
        public string updateContent;//更新内容
    }
    //const string UPDATE_ONFO_URl_1 = "https://gitee.com/wolf96/wikipali-app/releases/download/version/version.txt";//国内
    //const string UPDATE_ONFO_URl_1 = "https://gitee.com/wolf96/wikipali-app/releases/download/apk/wpa_T_1.0.1.apk";//国内
    const string UPDATE_ONFO_URl_1 = "https://gitee.com/wolf96/wikipali-app/raw/main/version.txt";//国内
    //const string UPDATE_ONFO_URl_2 = "https://github.com/ariyamaggika/wikipali-app/releases/download/apk/version.txt";//国外
    const string UPDATE_ONFO_URl_2 = "https://raw.githubusercontent.com/ariyamaggika/wikipali-app/master/version.txt";//国外

    /// <summary>
    /// 获取更新信息显示红点
    /// 从网站上获取版本号&国内国外下载地址W
    /// </summary>
    public void GetUpdateInfoRedPoint()
    {
        DownloadManager.Instance().DownLoad(Application.persistentDataPath, UPDATE_ONFO_URl_1, OnDownLoadVersionOverRedPoint, "version.txt");
    }
    object OnDownLoadVersionOverRedPoint(object _realSavePath)
    {
        string realSavePath = _realSavePath.ToString();

        if (File.Exists(realSavePath))
        {
            string[] lines = null;
            lines = File.ReadAllLines(realSavePath);
            if (lines.Length >= 3)
            {
                UpdateInfo uInfo = new UpdateInfo();
                uInfo.version = lines[0];

                if (uInfo.version == GameManager.Instance().appVersion)
                {
                    // UITool.ShowToastMessage(GameManager.Instance(), "当前已是最新版本", 35);
                    return false;
                }
                else
                {
                    //显示红点
                    GameManager.Instance().ShowSettingViewRedPoint();
                    return true;
                }
            }
            else
            {
                return null;
                // UITool.ShowToastMessage(GameManager.Instance(), "无网络连接", 35);
            }
        }

        return null;
    }
    /// <summary>
    /// 获取更新信息详情界面
    /// 从网站上获取版本号&国内国外下载地址W
    /// </summary>
    public UpdateInfo GetUpdateInfo()
    {
        UpdateInfo uInfo = new UpdateInfo();
        DownloadManager.Instance().DownLoad(Application.persistentDataPath, UPDATE_ONFO_URl_1, OnDownLoadVersionOver, "version.txt");
        //下载版本信息
        return uInfo;
    }
    object OnDownLoadVersionOver(object _realSavePath)
    {
        string realSavePath = _realSavePath.ToString();

        if (File.Exists(realSavePath))
        {
            string[] lines = null;
            lines = File.ReadAllLines(realSavePath);
            if (lines.Length >= 3)
            {
                UpdateInfo uInfo = new UpdateInfo();
                uInfo.version = lines[0];
                uInfo.downLoadUrl1 = lines[1];
                uInfo.downLoadUrl2 = lines[2];
                uInfo.updateContent += "更新内容：\r\n";
                for (int i = 3; i < lines.Length; i++)
                {
                    uInfo.updateContent += lines[i] + "\r\n";
                }
                currentUInfo = uInfo;
                if (uInfo.version == GameManager.Instance().appVersion)
                {
                    UITool.ShowToastMessage(GameManager.Instance(), "当前已是最新版本", 35);
                    return false;
                }
                else
                {
                    currentUInfo = uInfo;
                    //点开更新内容页面
                    GameManager.Instance().ShowSettingViewUpdatePage(currentUInfo);
                    return true;
                }
            }
            else
            {
                UITool.ShowToastMessage(GameManager.Instance(), "无网络连接", 35);
            }
        }

        return null;
    }
    public void UpdateAPK()
    {
        DownloadManager.Instance().DownLoad("", "", OnDownLoadApkOver, "");
    }
    object OnDownLoadApkOver(object obj)
    {


        return null;
    }
}
