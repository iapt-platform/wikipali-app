using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static ArticleController;
using static ArticleManager;

public class NextPrevGroupView : MonoBehaviour
{
    public Button prevBtn;
    public Button nextBtn;
    public Text prevTitleText;
    public Text nextTitleText;
    public ArticleView articleView;
    public Book currBook;
    public string currChannlID;
    public bool currIsTrans;
    public Book prevBook;
    public Book nextBook;
    // Start is called before the first frame update
    void Start()
    {
        prevBtn.onClick.AddListener(OnPrevBtnClick);
        nextBtn.onClick.AddListener(OnNextBtnClick);

    }
    void FailedToGetNeighbor()
    {
        SetAllBtnOff();
    }
    public void SetChapter(Book book, string channlID, bool isTrans)
    {
        currBook = book;
        currChannlID = channlID;
        currIsTrans = isTrans;
        Book parent = book.parent;
        if (parent == null)
        {
            FailedToGetNeighbor();
            Debug.LogError("【预警】book id:" + book.id + "  没有parent,隐藏上下章节按钮");
            return;
        }
        List<Book> neighborList = parent.children;
        if (neighborList == null || !neighborList.Contains(book))
        {
            FailedToGetNeighbor();
            Debug.LogError("【预警】book id:" + book.id + "  没有neighbor,或父里无子");
            return;
        }
        //无邻居
        if (neighborList.Count == 1)
        {
            SetAllBtnOff();
            return;
        }
        int index = neighborList.IndexOf(book);
        if (index == 0)//第一章
        {
            SetPOffNOn();
            prevBook = null;
            nextBook = neighborList[index + 1];

            nextTitleText.text = CommonTool.GetBookTranslateName(nextBook);
        }
        else if (index == neighborList.Count - 1)//最后一章
        {
            SetNOffPOn();
            prevBook = neighborList[index - 1];
            nextBook = null;
            prevTitleText.text = CommonTool.GetBookTranslateName(prevBook);
        }
        else//中间章节
        {
            SetAllBtnOn();
            prevBook = neighborList[index - 1];
            nextBook = neighborList[index + 1];
            prevTitleText.text = CommonTool.GetBookTranslateName(prevBook);
            nextTitleText.text = CommonTool.GetBookTranslateName(nextBook);
        }

        //book.translateName
    }
    public void OnPrevBtnClick()
    {
        articleView.SetChapterPath(prevBook);
        //如果上一章没有对应版本风格翻译，就跳转到pali原文显示
        if (currIsTrans && !string.IsNullOrEmpty(currChannlID) && prevBook.chapterDBDatas != null && prevBook.chapterDBDatas.Count > 0)
        {
            List<ChapterDBData> cDatas = prevBook.chapterDBDatas;
            int l = cDatas.Count;
            for (int i = 0; i < l; i++)
            {
                if (cDatas[i].channel_id == currChannlID)
                {
                    articleView.ShowPaliContentTrans(prevBook, cDatas[i], true);
                    return;
                }
            }
        }
        /// else
        articleView.ShowPaliContentTrans(prevBook, null, false);
    }
    public void OnNextBtnClick()
    {
        articleView.SetChapterPath(nextBook);
        //如果下一章没有对应版本风格翻译，就跳转到pali原文显示
        if (currIsTrans && !string.IsNullOrEmpty(currChannlID) && nextBook.chapterDBDatas != null && nextBook.chapterDBDatas.Count > 0)
        {
            List<ChapterDBData> cDatas = nextBook.chapterDBDatas;
            int l = cDatas.Count;
            for (int i = 0; i < l; i++)
            {
                if (cDatas[i].channel_id == currChannlID)
                {
                    articleView.ShowPaliContentTrans(nextBook, cDatas[i], true);
                    return;
                }
            }
        }
        /// else
        articleView.ShowPaliContentTrans(nextBook, null, false);
    }
    public void SetAllBtnOff()
    {
        prevBtn.gameObject.SetActive(false);
        nextBtn.gameObject.SetActive(false);
    }
    public void SetAllBtnOn()
    {
        prevBtn.gameObject.SetActive(true);
        nextBtn.gameObject.SetActive(true);
    }
    public void SetPOffNOn()
    {
        prevBtn.gameObject.SetActive(false);
        nextBtn.gameObject.SetActive(true);
    }
    public void SetNOffPOn()
    {
        prevBtn.gameObject.SetActive(true);
        nextBtn.gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
