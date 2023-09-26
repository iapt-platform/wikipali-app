using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static ArticleController;
using static ArticleManager;
using static DictManager;

public class ItemDicGroupPopView : MonoBehaviour
{
    public PopViewType currViewType;
    public Toggle selectToggle;
    public Text wordCountText;
    public Text wordGroupName;
    public Text countNameText;
    public DicGroupInfo dicGroupInfo;
    public ArticleGroupInfo articleGroupInfo;
    public ArticleView articleView;
    public void Init(DicGroupInfo _dicInfo)
    {
        countNameText.text = "单词数";
        currViewType = PopViewType.SaveDic;
        dicGroupInfo = _dicInfo;
        if (dicGroupInfo.wordList != null)
            wordCountText.text = dicGroupInfo.wordList.Count.ToString();
        wordGroupName.text = dicGroupInfo.groupName;
        bool isOn = DictManager.Instance().IsContainsWord(dicGroupInfo.groupID, DictManager.Instance().currWord);
        SetToggleValue(isOn);
    }
    public void Init(ArticleGroupInfo _articleGroupInfo)
    {
        countNameText.text = "文章数";
        currViewType = PopViewType.SaveArticle;
        articleGroupInfo = _articleGroupInfo;
        if (articleGroupInfo.bookTitleList != null)
            wordCountText.text = articleGroupInfo.bookTitleList.Count.ToString();
        wordGroupName.text = articleGroupInfo.groupName;
        Book currentBook = articleView.currentBook;
        ChapterDBData currentChapterData = articleView.currentChapterData;
        string channelID = currentChapterData == null ? "" : currentChapterData.id;
        bool isOn = ArticleManager.Instance().IsContainsArticle(articleGroupInfo.groupID, currentBook.translateName,currentBook.id, currentBook.paragraph, currentBook.chapter_len, channelID);
        SetToggleValue(isOn);
    }
    public bool GetSelectState()
    {
        return selectToggle.isOn;
    }
    public void SetToggleValue(bool isOn)
    {
        selectToggle.isOn = isOn;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
