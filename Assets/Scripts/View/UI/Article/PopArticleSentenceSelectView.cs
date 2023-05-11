using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using static ArticleController;
using static ArticleManager;

public class PopArticleSentenceSelectView : MonoBehaviour
{
    public ShareView shareView;
    public ItemArticleSentenceView itemTemp;
    public Button selectAllBtn;
    public Button returnBtn;
    public Button okBtn;
    public ArticleView articleView;
    // Start is called before the first frame update
    void Start()
    {
        selectAllBtn.onClick.AddListener(OnSelectAllBtnClick);
        returnBtn.onClick.AddListener(OnReturnBtnClick);
        okBtn.onClick.AddListener(OnOkBtnClick);
    }
    List<string> content;
    ChapterDBData currentChapterData;
    Book currentBook;
    public void Init()
    {
        content = articleView.articleContent;
        RefreshGroupList();
        shareView.gameObject.SetActive(true);
        currentChapterData = articleView.currentChapterData;
        currentBook = articleView.currentBook;
    }
    bool isSelectedAll = false;
    public void OnSelectAllBtnClick()
    {
        if (itemList.Count <= 0)
            return;
        isSelectedAll = !isSelectedAll;
        for (int i = 0; i < itemList.Count; i++)
        {
            itemList[i].selectToggle.isOn = isSelectedAll;
        }
    }
    public void OnReturnBtnClick()
    {
        shareView.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }
    string GetAllShareSentence()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i].selectToggle.isOn)
                sb.AppendLine(itemList[i].sentenceContent);
        }
        return sb.ToString();
    }
    //检测选择合法性
    bool CheckSelect()
    {
        int selectCount = 0;
        int textCount = 0;
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i].selectToggle.isOn)
            {
                ++selectCount;
                textCount += itemList[i].sentenceContent.Length;
            }
        }
        if (selectCount == 0)
        {
            UITool.ShowToastMessage(this, "请选择分享的句子", 35);
            return false;
        }
        //根据字数判断
        else if (textCount > 3000)
        {
            UITool.ShowToastMessage(this, "字数不能超过3000字", 35);
            return false;
        }
        return true;
    }
    public void OnOkBtnClick()
    {
        if (!CheckSelect())
            return;
        string contents = GetAllShareSentence();
        //pali原文
        if (currentChapterData == null)
        {
            shareView.SelectArticle(currentBook.translateName, "pāli原文", contents);

        }
        else//版本风格
            shareView.SelectArticle(currentChapterData.title, "版本风格：" + currentChapterData.channelData.name, contents);
        this.gameObject.SetActive(false);
    }
    List<ItemArticleSentenceView> itemList = new List<ItemArticleSentenceView>();
    public void RefreshGroupList()
    {
        DestroyAllItems();
        for (int i = 0; i < content.Count; i++)
        {
            GameObject inst = Instantiate(itemTemp.gameObject, itemTemp.transform.parent, false);
            inst.transform.position = itemTemp.transform.position;
            ItemArticleSentenceView iasv = inst.GetComponent<ItemArticleSentenceView>();
            iasv.Init(content[i]);
            inst.SetActive(true);
            itemList.Add(iasv);
        }
        //StartCoroutine(SetHeight());
    }
    //IEnumerator SetHeight()
    //{
    //    yield return null;
    //    int length = detailDicItemList.Count;
    //    float height = 0;
    //    for (int i = 0; i < length; i++)
    //    {
    //        //detailDicItemList[i].transform.position = detailDicItem.transform.position;
    //        //detailDicItemList[i].GetComponent<RectTransform>().position -= Vector3.up * height;
    //        DetailDicItemView ddiv = detailDicItemList[i].GetComponent<DetailDicItemView>();
    //        float textHeight = ddiv.GetHeight();
    //        detailDicItemList[i].GetComponent<RectTransform>().sizeDelta += new Vector2(0, textHeight);
    //        ddiv.itemHeight = detailDicItemList[i].GetComponent<RectTransform>().sizeDelta.y;
    //        //?为啥会缩100？
    //        //height += ddiv.GetHeight() + 200;
    //    }
    //    detailScrollContent.sizeDelta = new Vector2(detailScrollContent.sizeDelta.x, height);

    //}
    void DestroyAllItems()
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            Destroy(itemList[i].gameObject);
        }
        itemList.Clear();
    }
    // Update is called once per frame
    void Update()
    {

    }
}
