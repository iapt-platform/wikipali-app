using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static ArticleManager;
using static DictManager;
using static UpdateManager;
//todo:这个面板做成prefab加载，可以显示多个
public class CommonGroupView : MonoBehaviour
{
    public PopViewType currViewType;
    public Button returnBtn;
    public Button addBtn;
    public Text titleText;
    //DicGroupPopView
    //收藏
    public ItemDicGroupWordView wordItem;
    DicGroupInfo dicGroupInfo;
    ArticleGroupInfo articleGroupInfo;
    //关于
    public GameObject aboutPage;
    //更新部分
    public Text updateBtnText;
    public Text updateText;
    public Button updateBtn;
    public GameObject updatePage;
    //选项
    public ItemSettingOptionView settingOption;

    //单词本
    public void InitDicGroupWordView(DicGroupInfo _dicGroupInfo)
    {
        currViewType = PopViewType.SaveDic;
        dicGroupInfo = _dicGroupInfo;
        addBtn.gameObject.SetActive(false);
        titleText.text = dicGroupInfo.groupName;
        RefreshGroupList();
    }
    //文章收藏
    public void InitArticleGroupWordView(ArticleGroupInfo _articleGroupInfo)
    {
        currViewType = PopViewType.SaveArticle;
        articleGroupInfo = _articleGroupInfo;
        addBtn.gameObject.SetActive(false);
        titleText.text = articleGroupInfo.groupName;
        RefreshGroupList();
    }
    //离线包
    public void InitOfflinePackView(OtherInfo currentOInfo)
    {
        currViewType = PopViewType.OfflinePack;
        string uStr = "";
        uStr += "当前离线包更新时间：" + SettingManager.Instance().GetDBPackTime() + "\r\n";
        uStr += "最新离线包更新时间：" + currentOInfo.json.create_at + "\r\n\r\n";
        uStr += "章节数对比："+ SettingManager.Instance().GetDBPackChapterCount()+ "→<color=Red>"+ currentOInfo.json.chapter+ "</color>\r\n\r\n";
        uStr += "更新请预留出1G存储空间";
        updateText.text = uStr;
        addBtn.gameObject.SetActive(false);
        titleText.text = "离线包更新";
        updatePage.SetActive(true);
        int fileSize = (int)((float)currentOInfo.json.filesize / 1024 / 1024);
        updateBtnText.text = "点击更新(" + fileSize + "M)";
    }
    //更新说明
    public void InitUpdateView(UpdateInfo currentUInfo)
    {
        currViewType = PopViewType.Update;
        string uStr = "";
        uStr += "当前版本：" + Application.version + "\r\n";
        uStr += "最新版本：" + currentUInfo.version + "\r\n";
        uStr += currentUInfo.updateContent;
        updateText.text = uStr;
        addBtn.gameObject.SetActive(false);
        titleText.text = "版本更新";
        updatePage.SetActive(true);
        updateBtnText.text = "点击更新(" + currentUInfo.apkSize + ")";
    }
    //关于界面
    public void InitAboutView()
    {
        currViewType = PopViewType.About;
        addBtn.gameObject.SetActive(false);
        titleText.text = "关于wikipāli";
        aboutPage.SetActive(true);
    }
    //设置选项
    public void InitSettingOptions(string title, List<string> nameList, int selection, Func<object, object> fin)
    {
        titleText.text = title;
        settingOption.Init(nameList, selection, fin);
    }
    void Start()
    {
        returnBtn.onClick.AddListener(OnCloseBtnClick);
        addBtn.onClick.AddListener(OnAddBtnClick);
        updateBtn.onClick.AddListener(OnUpdateBtnClick);
    }
    public void OnCloseBtnClick()
    {
        DelAllListGO();
        aboutPage.SetActive(false);
        updatePage.SetActive(false);
        settingOption.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }
    public void OnAddBtnClick()
    {

    }
    public void OnUpdateBtnClick()
    {
        if (currViewType == PopViewType.Update)
        { //下载
            DownloadManager dm = new DownloadManager();
            GameManager.Instance().public_dm = dm;
            dm.DownloadAPK(this);
        }
        else if (currViewType == PopViewType.OfflinePack)
        {
            string url = UpdateManager.Instance().currentOInfo.offlinePackUrl + UpdateManager.Instance().currentOInfo.json.filename;
            UpdateManager.Instance().UpdateDBPack(url);
        }
    }
    public void DelAllListGO()
    {
        int l = itemList.Count;
        for (int i = 0; i < l; i++)
        {
            Destroy(itemList[i].gameObject);
        }
        itemList.Clear();

    }
    void RefreshDicGList()
    {
        int l = itemList.Count;
        for (int i = 0; i < l; i++)
        {
            Destroy(itemList[i].gameObject);
        }
        itemList.Clear();

        int gl = dicGroupInfo.wordList.Count;
        for (int i = 0; i < gl; i++)
        {
            GameObject inst = Instantiate(wordItem.gameObject, wordItem.transform.parent, false);
            inst.transform.position = wordItem.transform.position;
            //inst.GetComponent<RectTransform>().position -= Vector3.up * height;
            ItemDicGroupWordView iv = inst.GetComponent<ItemDicGroupWordView>();
            iv.Init(dicGroupInfo.wordList[i], dicGroupInfo.groupID, this);
            inst.SetActive(true);
            itemList.Add(iv);
        }
    }
    void RefreshArticleGList()
    {
        int l = itemList.Count;
        for (int i = 0; i < l; i++)
        {
            Destroy(itemList[i].gameObject);
        }
        itemList.Clear();

        int gl = articleGroupInfo.bookTitleList.Count;
        for (int i = 0; i < gl; i++)
        {
            GameObject inst = Instantiate(wordItem.gameObject, wordItem.transform.parent, false);
            inst.transform.position = wordItem.transform.position;
            //inst.GetComponent<RectTransform>().position -= Vector3.up * height;
            ItemDicGroupWordView iv = inst.GetComponent<ItemDicGroupWordView>();
            iv.Init(articleGroupInfo.bookTitleList[i], articleGroupInfo.bookIDList[i], articleGroupInfo.bookParagraphList[i], articleGroupInfo.bookChapterLenList[i],
                articleGroupInfo.channelIDList[i], articleGroupInfo.channelNameList[i], articleGroupInfo.groupID, this);
            inst.SetActive(true);
            itemList.Add(iv);
        }
    }
    List<ItemDicGroupWordView> itemList = new List<ItemDicGroupWordView>();
    /// <summary> 
    /// 刷新分组信息
    /// </summary>
    public void RefreshGroupList()
    {
        DelAllListGO();
        if (currViewType == PopViewType.SaveDic)
        {
            RefreshDicGList();
        }
        else if (currViewType == PopViewType.SaveArticle)
        {
            RefreshArticleGList();
        }

    }
}
