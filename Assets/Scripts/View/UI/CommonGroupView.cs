using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static ArticleManager;
using static DictManager;
//todo:这个面板做成prefab加载，可以显示多个
public class CommonGroupView : MonoBehaviour
{
    public PopViewType currViewType;
    public Button returnBtn;
    public Button addBtn;
    public Text titleText;
    //DicGroupPopView
    public ItemDicGroupWordView wordItem;
    DicGroupInfo dicGroupInfo;
    ArticleGroupInfo articleGroupInfo;
    public GameObject aboutPage;
    public void InitDicGroupWordView(DicGroupInfo _dicGroupInfo)
    {
        currViewType = PopViewType.SaveDic;
        dicGroupInfo = _dicGroupInfo;
        addBtn.gameObject.SetActive(false);
        titleText.text = dicGroupInfo.groupName;
        RefreshGroupList();
    }
    public void InitArticleGroupWordView(ArticleGroupInfo _articleGroupInfo)
    {
        currViewType = PopViewType.SaveArticle;
        articleGroupInfo = _articleGroupInfo;
        addBtn.gameObject.SetActive(false);
        titleText.text = articleGroupInfo.groupName;
        RefreshGroupList();
    }
    public void InitAboutView()
    {
        currViewType = PopViewType.About;
        addBtn.gameObject.SetActive(false);
        titleText.text = "关于wikipali";
        aboutPage.SetActive(true);
    }
    void Start()
    {
        returnBtn.onClick.AddListener(OnCloseBtnClick);
        addBtn.onClick.AddListener(OnAddBtnClick);
    }
    public void OnCloseBtnClick()
    {
        DelAllListGO();
        aboutPage.SetActive(false);
        this.gameObject.SetActive(false);
    }
    public void OnAddBtnClick()
    {

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
