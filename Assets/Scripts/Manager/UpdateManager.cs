using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Networking;
using static ArticleController;
//�汾����
public class UpdateManager
{
    private UpdateManager() { }
    private static UpdateManager manager = null;
    //��̬�������� 
    public static UpdateManager Instance()
    {
        if (manager == null)
        {
            manager = new UpdateManager();
        }
        return manager;
    }
    #region ����APK��汾��Azure��
    //��װAPK
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
            //print("�Ѿ����ڣ�");
        }
    }
    public UpdateInfo currentUInfo;
    public OtherInfo currentOInfo;
    //���������
    public void CheckUpdateOpenPage(MonoBehaviour ui)
    {
        ////���Դ���
        //UITool.ShowToastMessage(ui, Application.internetReachability.ToString(), 35);

        //try
        //{
        //    System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();
        //    PingReply pr = ping.Send("www.baidu.com", 3000);
        //    if (pr.Status == IPStatus.Success)
        //    {
        //        UITool.ShowToastMessage(ui, "ping�ɹ�", 35);
        //        return ;
        //    }
        //    else
        //    {
        //        UITool.ShowToastMessage(ui, "pingʧ��", 35);
        //        return;
        //    }
        //}
        //catch (Exception e)
        //{
        //    return;
        //}


        if (!NetworkMangaer.Instance().PingNetAddress())
        {
            UITool.ShowToastMessage(ui, "����������", 35);
            return;
            // return false;
        }
        GetUpdateInfo();

    }
    //��App�Զ���������ʾ���
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
        public string apkSize;//�����С
        public string downLoadUrl1;//����
        public string downLoadUrl2;//����
        public string updateContent;//��������
    }
    //const string UPDATE_ONFO_URl_1 = "https://gitee.com/wolf96/wikipali-app/releases/download/version/version.txt";//����
    //const string UPDATE_ONFO_URl_1 = "https://gitee.com/wolf96/wikipali-app/releases/download/apk/wpa_T_1.0.1.apk";//����
    //�����Ϣ
    const string UPDATE_INFO_URl_1 = "https://gitee.com/wolf96/wikipali-app/raw/main/version.txt";//����
    //const string UPDATE_ONFO_URl_2 = "https://github.com/ariyamaggika/wikipali-app/releases/download/apk/version.txt";//����
    const string UPDATE_INFO_URl_2 = "https://raw.githubusercontent.com/ariyamaggika/wikipali-app/master/version.txt";//����

    const string AZURE_INFO_URl_1 = "https://gitee.com/wolf96/wikipali-app/raw/main/font.font";//����
    const string AZURE_INFO_URl_2 = "https://raw.githubusercontent.com/ariyamaggika/wikipali-app/master/font.font";//����
    //������Ϣ
    const string OTHER_INFO_URl_1 = "https://gitee.com/wolf96/wikipali-app/raw/main/info.txt";//����
    const string OTHER_INFO_URl_2 = "https://raw.githubusercontent.com/ariyamaggika/wikipali-app/master/info.txt";//����


    /// <summary>
    /// ��ȡ������Ϣ��ʾ���
    /// ����վ�ϻ�ȡ�汾��&���ڹ������ص�ַW
    /// </summary>
    public void GetUpdateInfoRedPoint()
    {
        DownloadManager dm = new DownloadManager();
        dm.DownLoad(Application.persistentDataPath, UPDATE_INFO_URl_1, OnDownLoadVersionOverRedPoint, "version.txt");
    }
    /// <summary>
    /// ����AzureKey
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
                    // UITool.ShowToastMessage(GameManager.Instance(), "��ǰ�������°汾", 35);
                    return false;
                }
                else
                {
                    //��ʾ���
                    GameManager.Instance().ShowSettingViewUpdateRedPoint();
                    return true;
                }
            }
            else
            {
                return null;
                // UITool.ShowToastMessage(GameManager.Instance(), "����������", 35);
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
    /// ��ȡ������Ϣ�������
    /// ����վ�ϻ�ȡ�汾��&���ڹ������ص�ַW
    /// </summary>
    public UpdateInfo GetUpdateInfo()
    {
        UpdateInfo uInfo = new UpdateInfo();
        DownloadManager dm = new DownloadManager();
        dm.DownLoad(Application.persistentDataPath, UPDATE_INFO_URl_1, OnDownLoadVersionOver, "version.txt");
        //���ذ汾��Ϣ
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
                uInfo.updateContent += "�������ݣ�\r\n";
                for (int i = 5; i < lines.Length; i++)
                {
                    uInfo.updateContent += lines[i] + "\r\n";
                }
                currentUInfo = uInfo;
                if (uInfo.version == GameManager.Instance().appVersion)
                {
                    UITool.ShowToastMessage(GameManager.Instance(), "��ǰ�������°汾", 35);
                    return false;
                }
                else
                {
                    currentUInfo = uInfo;
                    //�㿪��������ҳ��
                    GameManager.Instance().ShowSettingViewUpdatePage(currentUInfo);
                    return true;
                }
            }
            else
            {
                UITool.ShowToastMessage(GameManager.Instance(), "����������", 35);
            }
        }

        return null;
    }
    //���߰�����
    //1.��app���������̣������أ���settingview���ú��
    //2.������߰���ť���жϺ�������Ƿ��������ļ������δ���أ���������һ��
    //3.�ж������Ƿ�ͬ��������ͬ������ʾ�����ڲ�ͬ���ܵ��ȥ����ʾchapter������Ͱ����С
    //4.������ذ�ť�������߰�
    //5.���غ��ѹ�����滻λ�ã�ɾ��ѹ����
    public class OtherInfo
    {
        public string offlinePackIndexUrl;//���߰�index.Json
        public string offlinePackUrl;//���߰�URLǰ׺
        public OfflinePackIndexJson json;//index��Ϣ
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
                UITool.ShowToastMessage(GameManager.Instance(), "����������", 35);
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
        //���ذ汾��Ϣ
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
                //ȥ����β������
                lines = lines.Substring(1).Substring(0, lines.Length - 2);
                OfflinePackIndexJson indexJson = JsonUtility.FromJson<OfflinePackIndexJson>(lines);
                currentOInfo.json = indexJson;
                if (indexJson.create_at == SettingManager.Instance().GetDBPackTime())
                {
                    //if(showOfflinwPackVerLog)
                    //{ UITool.ShowToastMessage(GameManager.Instance(), "��ǰ�������°汾", 35);
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
                UITool.ShowToastMessage(GameManager.Instance(), "����������", 35);
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
    #region ���·��������
    //����Channel
    //����
    //1.���ش�һ��ʱ��㣬��channel��װ��
    //2.ÿ�θ���channl������ʱ��㵽���ڵ���������
    //3.�ɹ��������ݺ�ͬ��ʱ���Ϊ����ʱ��
    #region channel����

    #endregion
    #region �������ݿ��
    //����
    //1.��appʱ����json���ȶ�ʱ���chapter��������һ�¾ͱ���
    //2.���ȥ��ע�����С��chapter�����ȶԣ�
    //3.������غ�����о�ɾ���ϴ�ѹ����������ѹ����
    //4.��ѹѹ�����滻�����ݿ�λ��
    //5.ɾ��ѹ����

    //��3&4���裬�������ѹ
    public void UpdateDBPack(string url)
    {
        //GameManager.Instance().StartDownLoadProgress();
        //����
        DownloadManager dm = new DownloadManager();
        GameManager.Instance().public_dm = dm;
        dm.DownloadPack(GameManager.Instance(), DownLoadDBPackOver, Application.persistentDataPath, url, "Sentence.lzma");
    }
    object DownLoadDBPackOver(object obj)
    {
        GameManager.Instance().DownLoadProgressOver();
        ZipManager.Instance().UnZipDBPack();
        //��ѹѹ����
        // UpdateManager.Instance().InstallApk(realSavePath);
        //����ԭʼѹ����
        return null;
    }

    #endregion



    #endregion
}
