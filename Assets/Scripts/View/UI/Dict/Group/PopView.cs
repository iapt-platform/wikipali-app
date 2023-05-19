using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static ArticleController;
using static ArticleManager;
using static DictManager;
public enum PopViewType
{
    SaveDic = 1,
    SaveArticle = 2,
}
public class PopView : MonoBehaviour
{

    public PopViewType currViewType;
    public Button editGroupBtn;
    public Button closeBackGroupBtn;
    public Button okBtn;
    public DicGroupView dicGroupView;
    public ItemDicGroupPopView groupItem;
    public ArticleView articleView;
    // Start is called before the first frame update
    void Start()
    {
        editGroupBtn.onClick.AddListener(OnEditBtnClick);
        closeBackGroupBtn.onClick.AddListener(OnCloseBackBtnClick);
        okBtn.onClick.AddListener(OnOkBtnClick);

    }
    public void Init(PopViewType pvt)
    {
        currViewType = pvt;
    }
    public void OnEditBtnClick()
    {
        dicGroupView.Init(currViewType);
        dicGroupView.RefreshGroupList();
        dicGroupView.gameObject.SetActive(true);
    }
    //public void OnCloseBtnClick()
    //{
    //    DictManager.Instance().SetWordStar(DictManager.Instance().currWord);
    //    this.gameObject.SetActive(false);
    //}
    public void OnCloseBackBtnClick()
    {
        DictManager.Instance().SetWordStar(DictManager.Instance().currWord);
        this.gameObject.SetActive(false);
    }
    void SaveDicGroup()
    {
        int l = itemList.Count;
        bool isDirty = false;
        for (int i = 0; i < l; i++)
        {
            if (itemList[i].GetSelectState())
            {
                if (!itemList[i].dicGroupInfo.wordList.Contains(DictManager.Instance().currWord))
                {
                    itemList[i].dicGroupInfo.wordList.Add(DictManager.Instance().currWord);
                    isDirty = true;
                }
            }
            else
            {
                if (itemList[i].dicGroupInfo.wordList.Contains(DictManager.Instance().currWord))
                {
                    itemList[i].dicGroupInfo.wordList.Remove(DictManager.Instance().currWord);
                    isDirty = true;
                }
            }
        }
        this.gameObject.SetActive(false);
        if (isDirty)
            DictManager.Instance().ModifyDicGroup();
        DictManager.Instance().SetWordStar(DictManager.Instance().currWord);

    }
    void SaveArticleGroup()
    {
        int l = itemList.Count;
        bool isDirty = false;

        Book currentBook = articleView.currentBook;
        ChapterDBData currentChapterData = articleView.currentChapterData;
        string channelID = currentChapterData == null ? "" : currentChapterData.id;
        for (int i = 0; i < l; i++)
        {

            if (itemList[i].GetSelectState())
            {
                bool isMatch = false;
                for (int j = 0; j < itemList[i].articleGroupInfo.bookTitleList.Count; j++)
                {
                    if (itemList[i].articleGroupInfo.bookTitleList[j] == currentBook.translateName &&
                        itemList[i].articleGroupInfo.bookIDList[j] == currentBook.id &&
                        itemList[i].articleGroupInfo.channelIDList[j] == channelID)
                    {
                        isMatch = true;
                    }
                }
                isDirty = !isMatch;
                if (isDirty)
                {
                    itemList[i].articleGroupInfo.bookTitleList.Add(currentBook.translateName);
                    itemList[i].articleGroupInfo.bookIDList.Add(currentBook.id);
                    itemList[i].articleGroupInfo.channelIDList.Add(channelID);
                }
            }
            else
            {
                for (int j = 0; j < itemList[i].articleGroupInfo.bookTitleList.Count; j++)
                {
                    if (itemList[i].articleGroupInfo.bookTitleList[j] == currentBook.translateName &&
                        itemList[i].articleGroupInfo.bookIDList[j] == currentBook.id &&
                        itemList[i].articleGroupInfo.channelIDList[j] == channelID)
                    {
                        itemList[i].articleGroupInfo.bookTitleList.RemoveAt(j);
                        itemList[i].articleGroupInfo.bookIDList.RemoveAt(j);
                        itemList[i].articleGroupInfo.channelIDList.RemoveAt(j);
                        isDirty = true;
                        break;
                    }
                }
            }
        }
        this.gameObject.SetActive(false);
        if (isDirty)
            ArticleManager.Instance().ModifyArticleGroup();
        ArticleManager.Instance().SetArticleStar(currentBook.translateName, currentBook.id, channelID);
    }
    public void OnOkBtnClick()
    {
        if (currViewType == PopViewType.SaveDic)
        {
            SaveDicGroup();
        }
        else if (currViewType == PopViewType.SaveArticle)
        {
            SaveArticleGroup();
        }
    }
    void RefreshDicGList()
    {
        List<DicGroupInfo> allDicGroup = DictManager.Instance().allDicGroup;
        int gl = allDicGroup.Count;
        for (int i = 0; i < gl; i++)
        {
            GameObject inst = Instantiate(groupItem.gameObject, groupItem.transform.parent, false);
            inst.transform.position = groupItem.transform.position;
            //inst.GetComponent<RectTransform>().position -= Vector3.up * height;
            ItemDicGroupPopView iv = inst.GetComponent<ItemDicGroupPopView>();
            iv.Init(allDicGroup[i]);
            inst.SetActive(true);
            itemList.Add(iv);
        }
    }
    void RefreshArticleGList()
    {
        List<ArticleGroupInfo> allArticleGroup = ArticleManager.Instance().allArticleGroup;
        int gl = allArticleGroup.Count;
        for (int i = 0; i < gl; i++)
        {
            GameObject inst = Instantiate(groupItem.gameObject, groupItem.transform.parent, false);
            inst.transform.position = groupItem.transform.position;
            //inst.GetComponent<RectTransform>().position -= Vector3.up * height;
            ItemDicGroupPopView iv = inst.GetComponent<ItemDicGroupPopView>();
            iv.Init(allArticleGroup[i]);
            inst.SetActive(true);
            itemList.Add(iv);
        }
    }
    List<ItemDicGroupPopView> itemList = new List<ItemDicGroupPopView>();
    /// <summary> 
    /// 刷新分组信息
    /// </summary>
    public void RefreshGroupList()
    {
        int l = itemList.Count;
        for (int i = 0; i < l; i++)
        {
            Destroy(itemList[i].gameObject);
        }
        itemList.Clear();

        if (currViewType == PopViewType.SaveDic)
        {
            RefreshDicGList();
        }
        else if (currViewType == PopViewType.SaveArticle)
        {
            RefreshArticleGList();
        }



    }
    // Update is called once per frame
    void Update()
    {

    }
}
