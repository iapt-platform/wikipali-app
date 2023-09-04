using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UpdateManager;

public class GameManager : MonoBehaviour
{
    private static GameManager manager = null;
    //静态工厂方法 
    public static GameManager Instance()
    {
        if (manager == null)
        {
            manager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>();
        }
        return manager;
    }
    public InitView initView;
    public SettingView settingView;
    public StarGroupDictView dicStarGroup;
    public StarGroupArticleView articleStarGroup;
    public MainView mainView;
    public ArticleView articleView;

    public string appVersion;//= Application.version;
    //public bool canUpdate = false;
    void Awake()
    {
        appVersion = Application.version;
        SettingManager.Instance().InitGame();
        DictManager.Instance().dicStarGroup = dicStarGroup;
        ArticleManager.Instance().articleStarGroup = articleStarGroup;
        ArticleManager.Instance().articleView = articleView;
    }
    bool isStartUnZipProgress = false;
    bool isDownLoadProgress = false;
    Func<object> unZipCallback;
    public void StartUnZipProgress(Func<object> callback, string title = "正在解压")
    {
        initView.gameObject.SetActive(true);
        initView.Init(title);
        isStartUnZipProgress = true;
        unZipCallback = callback;
    }
    public void SetInitViewProgress(float progress)
    {
        initView.SetProgess(progress);
    }
    public void EndUnZipProgress()
    {
        initView.gameObject.SetActive(false);
        isStartUnZipProgress = false;
    }
    public object EndUnZipDB()
    {
        SettingManager.Instance().UnZipFin();
        return null;
    }
    // Start is called before the first frame update
    void Start()
    {
        CheckUpdate();
        if (CreatQR.LoadQR())
            CreatQR.CreatQr();
        SpeechGeneration.Instance().LoadTxt();

        //打开上次浏览记录
        SettingManager.Instance().OpenLast();
    }
    public DownloadManager public_dm = null;
    // Update is called once per frame
    void Update()
    {
        if (isStartUnZipProgress)
        {
            int progressFin = ZipManager.Instance().lzmafileProgress[0];

            ulong sizeOfEntry = ZipManager.Instance().sizeOfEntry;
            ulong bwrite = lzma.getBytesWritten();
            float progress = (bwrite / sizeOfEntry);
            //Debug.LogError("s:" + sizeOfEntry);
            //Debug.LogError("b:" + bwrite);
            SetInitViewProgress(progress);
            if (100 == progressFin)
            {
                EndUnZipProgress();
                unZipCallback();
            }
        }
        if (isDownLoadProgress)
        {
            SetInitViewProgress(public_dm.progress * 0.01f);
        }
    }
    //检测更新
    void CheckUpdate()
    {
        //检测红点和下载AzureKey
        UpdateManager.Instance().CheckUpdateRedPoint();
    }

    public void ShowSettingViewOfflineDBPackPage(OtherInfo currentOInfo)
    {
        settingView.SetOfflinePackPage(currentOInfo);
    }
    public void HideSettingViewOfflineDBPackPage()
    {
        settingView.HideCommonGroupView();
    }
    public void UpdateSettingViewOfflineDBTimeText()
    { 
        settingView.UpdateSettingViewOfflineDBTimeText();
    }
    public void ShowSettingViewUpdatePage(UpdateInfo currentUInfo)
    {
        settingView.SetUpdatePage(currentUInfo);
    }
    public void ShowSettingViewUpdateRedPoint()
    {
        //canUpdate = true;
        settingView.SetUpdateRedPoint();
    }
    public void ShowSettingViewOfflinePackRedPoint()
    {
        settingView.SetOfflinePackRedPoint(true);
    }
    public void HideSettingViewOfflinePackRedPoint()
    {
        settingView.SetOfflinePackRedPoint(false);
    }
    public void StartDownLoadProgress()
    {
        initView.gameObject.SetActive(true);
        initView.Init("下载进度");
        isDownLoadProgress = true;
    }
    public void DownLoadProgressOver()
    {
        isDownLoadProgress = false;
        //直接跳转到安装APK，就不关闭InitView防止误触
        //initView.gameObject.SetActive(false);
    }

    //延迟一帧
    public void ShowArticle(int bookID, int bookParagraph, int bookChapterLen, string channelId)
    {
        StartCoroutine(ShowArticleC(bookID, bookParagraph, bookChapterLen, channelId));
    }
    IEnumerator ShowArticleC(int bookID, int bookParagraph, int bookChapterLen, string channelId)
    {
        yield return null;
        mainView.SetArticleOn();
        mainView.articleView.ShowPaliContentFromStar(bookID, bookParagraph, bookChapterLen, channelId);
    }
    public void ShowDicWord(string word)
    {
        mainView.SetDicOn();
        mainView.dicView.OnItemDicClick(word);
    }
}
