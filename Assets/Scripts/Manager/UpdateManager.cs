using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Networking;
using static ArticleController;
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
    #region 更新APK与版本与Azure等
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
    public OtherInfo currentOInfo;
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
        GetOtherInfo();
        GetAzureKey();
    }
    public class UpdateInfo
    {
        public string version;
        public string apkSize;//包体大小
        public string downLoadUrl1;//国内
        public string downLoadUrl2;//国外
        public string updateContent;//更新内容
    }
    //const string UPDATE_ONFO_URl_1 = "https://gitee.com/wolf96/wikipali-app/releases/download/version/version.txt";//国内
    //const string UPDATE_ONFO_URl_1 = "https://gitee.com/wolf96/wikipali-app/releases/download/apk/wpa_T_1.0.1.apk";//国内
    //红点信息
    const string UPDATE_INFO_URl_1 = "https://gitee.com/wolf96/wikipali-app/raw/main/version.txt";//国内
    //const string UPDATE_ONFO_URl_2 = "https://github.com/ariyamaggika/wikipali-app/releases/download/apk/version.txt";//国外
    const string UPDATE_INFO_URl_2 = "https://raw.githubusercontent.com/ariyamaggika/wikipali-app/master/version.txt";//国外

    const string AZURE_INFO_URl_1 = "https://gitee.com/wolf96/wikipali-app/raw/main/font.font";//国内
    const string AZURE_INFO_URl_2 = "https://raw.githubusercontent.com/ariyamaggika/wikipali-app/master/font.font";//国外
    //其他信息
    const string OTHER_INFO_URl_1 = "https://gitee.com/wolf96/wikipali-app/raw/main/info.txt";//国内
    const string OTHER_INFO_URl_2 = "https://raw.githubusercontent.com/ariyamaggika/wikipali-app/master/info.txt";//国外


    /// <summary>
    /// 获取更新信息显示红点
    /// 从网站上获取版本号&国内国外下载地址W
    /// </summary>
    public void GetUpdateInfoRedPoint()
    {
        DownloadManager dm = new DownloadManager();
        dm.DownLoad(Application.persistentDataPath, UPDATE_INFO_URl_1, OnDownLoadVersionOverRedPoint, "version.txt");
    }
    /// <summary>
    /// 下载AzureKey
    /// </summary>
    public void GetAzureKey()
    {
        DownloadManager dm = new DownloadManager();
        dm.DownLoad(Application.persistentDataPath, AZURE_INFO_URl_1, OnDownLoadAzure, "font.font");
    }
    object OnDownLoadVersionOverRedPoint(object _realSavePath)
    {
        string realSavePath = _realSavePath.ToString();

        if (File.Exists(realSavePath))
        {
            string[] lines = null;
            lines = File.ReadAllLines(realSavePath);
            if (lines.Length >= 4)
            {
                UpdateInfo uInfo = new UpdateInfo();
                uInfo.version = lines[0];
                CreatQR.QrCodeStr = lines[2];
                CreatQR.CreatQr();
                if (uInfo.version == GameManager.Instance().appVersion)
                {
                    // UITool.ShowToastMessage(GameManager.Instance(), "当前已是最新版本", 35);
                    return false;
                }
                else
                {
                    //显示红点
                    GameManager.Instance().ShowSettingViewUpdateRedPoint();
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
    object OnDownLoadAzure(object _realSavePath)
    {
        string realSavePath = _realSavePath.ToString();

        if (File.Exists(realSavePath))
        {
            string str = (string)CommonTool.DeserializeObjectFromFile(realSavePath, "wikipaliapp12345");
            str = CommonTool.SwapString(str);
            //Debug.LogError(str);
            string[] splits = str.Split(',');
            if (splits.Length > 1)
            {
                SpeechGeneration.SPEECH_KEY = splits[0];
                SpeechGeneration.SPEECH_REGION = splits[1];
            }
            else
            {
                SpeechGeneration.SPEECH_KEY = null;
                SpeechGeneration.SPEECH_REGION = null;
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
        DownloadManager dm = new DownloadManager();
        dm.DownLoad(Application.persistentDataPath, UPDATE_INFO_URl_1, OnDownLoadVersionOver, "version.txt");
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
            if (lines.Length >= 4)
            {
                UpdateInfo uInfo = new UpdateInfo();
                uInfo.version = lines[0];
                uInfo.apkSize = lines[1];
                uInfo.downLoadUrl1 = lines[3];
                uInfo.downLoadUrl2 = lines[4];
                uInfo.updateContent += "更新内容：\r\n";
                for (int i = 5; i < lines.Length; i++)
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
    //离线包流程
    //1.进app跟随红点流程，先下载，在settingview设置红点
    //2.点击离线包按钮，判断红点流程是否下载了文件，如果未下载，就再下载一遍
    //3.判断日期是否不同，日期相同弹出提示，日期不同就能点进去，显示chapter数量差和包体大小
    //4.点击下载按钮下载离线包
    //5.下载后解压缩，替换位置，删除压缩包
    public class OtherInfo
    {
        public string offlinePackIndexUrl;//离线包index.Json
        public string offlinePackUrl;//离线包URL前缀
        public OfflinePackIndexJson json;//index信息
    }
    public void GetOtherInfo()
    {
        OtherInfo oInfo = new OtherInfo();
        DownloadManager dm = new DownloadManager();
        dm.DownLoad(Application.persistentDataPath, OTHER_INFO_URl_1, OnDownLoadOtherInfoOver, "info.txt");

    }
    object OnDownLoadOtherInfoOver(object _realSavePath)
    {
        string realSavePath = _realSavePath.ToString();

        if (File.Exists(realSavePath))
        {
            string[] lines = null;
            lines = File.ReadAllLines(realSavePath);
            if (lines.Length >= 1)
            {
                OtherInfo oInfo = new OtherInfo();
                oInfo.offlinePackIndexUrl = lines[0];
                oInfo.offlinePackUrl = lines[1];
                currentOInfo = oInfo;
                GetOfflinePackIndex(oInfo.offlinePackIndexUrl);

            }
            else
            {
                UITool.ShowToastMessage(GameManager.Instance(), "无网络连接", 35);
            }
        }

        return null;
    }
    [Serializable]
    public class OfflinePackIndexJson
    {
        public string filename;
        public string url;
        public string create_at;
        public int chapter;
        public int filesize;//size/1024/1024
    }
    public OfflinePackIndexJson GetOfflinePackIndex(string url)
    {
        OfflinePackIndexJson indexJson = new OfflinePackIndexJson();
        DownloadManager dm = new DownloadManager();
        dm.DownLoad(Application.persistentDataPath, url, OnDownLoadIndexJsonOver, "index.json");
        //下载版本信息
        return indexJson;
    }
    //bool showOfflinwPackVerLog = false;
    object OnDownLoadIndexJsonOver(object _realSavePath)
    {
        string realSavePath = _realSavePath.ToString();

        if (File.Exists(realSavePath))
        {
            string lines = File.ReadAllText(realSavePath);
            if (!string.IsNullOrEmpty(lines))
            {
                //去掉首尾大括号
                lines = lines.Substring(1).Substring(0, lines.Length - 2);
                OfflinePackIndexJson indexJson = JsonUtility.FromJson<OfflinePackIndexJson>(lines);
                currentOInfo.json = indexJson;
                if (indexJson.create_at == SettingManager.Instance().GetDBPackTime())
                {
                    //if(showOfflinwPackVerLog)
                    //{ UITool.ShowToastMessage(GameManager.Instance(), "当前已是最新版本", 35);
                    //showOfflinwPackVerLog = false;
                    //    }
                    return false;
                }
                else
                {
                    GameManager.Instance().ShowSettingViewOfflinePackRedPoint();
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
        // DownloadManager.Instance().DownLoad("", "", OnDownLoadApkOver, "");
    }
    object OnDownLoadApkOver(object obj)
    {


        return null;
    }
    #endregion
    #region 更新服务端数据
    //更新Channel
    //流程
    //1.本地存一个时间点，和channel安装包
    //2.每次更新channl，更新时间点到现在的所有数据
    //3.成功保存数据后，同步时间点为现在时间
    #region channel部分

    #endregion
    #region 离线数据库包
    //流程
    //1.进app时下载json，比对时间和chapter数量，不一致就标红点
    //2.点进去标注包体大小与chapter数量比对，
    //3.点击下载后，如果有就删除上次压缩包，下载压缩包
    //4.解压压缩包替换到数据库位置
    //5.删除压缩包

    //第3&4步骤，下载与解压
    public void UpdateDBPack(string url)
    {
        //GameManager.Instance().StartDownLoadProgress();
        //下载
        DownloadManager dm = new DownloadManager();
        GameManager.Instance().public_dm = dm;
        dm.DownloadPack(GameManager.Instance(), DownLoadDBPackOver, Application.persistentDataPath, url, "Sentence.lzma");
    }
    object DownLoadDBPackOver(object obj)
    {
        GameManager.Instance().DownLoadProgressOver();
        ZipManager.Instance().UnZipDBPack();
        //解压压缩包
        // UpdateManager.Instance().InstallApk(realSavePath);
        //覆盖原始压缩包
        return null;
    }

    #endregion



    #endregion
}
