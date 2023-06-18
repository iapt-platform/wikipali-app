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
    //更新说明
    public void InitUpdateView(UpdateInfo currentUInfo)
    {
        currViewType = PopViewType.SaveArticle;
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
        this.gameObject.SetActive(false);
    }
    public void OnAddBtnClick()
    {

    }
    public void OnUpdateBtnClick()
    {
        //下载
        DownloadManager.Instance().DownloadAPK(this);
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
