using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static ArticleController;
using static ArticleManager;

public class ArticleNodeItemView : MonoBehaviour
{
    public Text titleText;
    public Text subTitleText;
    public ArticleTreeNode article;
    public Book book;
    public ChapterDBData channel;
    public Button btn;
    public ArticleView articleView;
    //进度部分
    public GameObject progress;
    public Image progressImg;
    public Text progressText;
    //是否脱离了tag树形结构进入到选书和章节
    public bool isBook;
    //是否脱离了选书到显示版本风格
    public bool isChannel;

    // Start is called before the first frame update
    void Start()
    {
        btn.onClick.AddListener(OnBtnClick);

    }
    public void Init(ArticleTreeNode aNode)
    {
        isBook = false;
        article = aNode;
        //节点翻译
        string ts = "";
        bool isHaveTs = ArticleController.Instance().tsDic.TryGetValue(aNode.name, out ts);
        if (isHaveTs)
            titleText.text = ts;
        else
        {
            isHaveTs = ArticleController.Instance().tsDic.TryGetValue(aNode.name.ToLower(), out ts);
            if (isHaveTs)
                titleText.text = ts;
            else
                titleText.text = aNode.name;
        }
        subTitleText.text = aNode.name;
    }
    public void Init(Book bNode)
    {
        isBook = true;
        book = bNode;
        if (string.IsNullOrEmpty(bNode.translateName))
            titleText.text = bNode.toc;
        else
            titleText.text = bNode.translateName;
        subTitleText.text = bNode.toc;
        //显示百分比
        if (bNode.isHaveProgress)
        {
            progress.SetActive(true);
            progressImg.fillAmount = bNode.progress;
            int progressP = (int)(bNode.progress * 100);
            progressText.text = progressP + "%";
        }
    }
    public void Init(Book bNode, ChapterDBData cNode)
    {
        isChannel = true;
        book = bNode;
        channel = cNode;
        titleText.text = cNode.channelData.name;
        subTitleText.text = cNode.channelData.summary;
        //显示百分比
        progress.SetActive(true);
        progressImg.fillAmount = cNode.progress;
        int progressP = (int)(cNode.progress * 100);
        progressText.text = progressP + "%";

    }
    //channel章节 pali原文按钮
    public void InitPali(Book bNode)
    {
        isChannel = true;
        book = bNode;
        titleText.text = "Pali原文";
        subTitleText.gameObject.SetActive(false);

    }
    public void OnBtnClick()
    {
        if (isBook)
            articleView.BookNodeBtnClick(book);
        else if (isChannel)
        {

            if (channel != null)    //pali&翻译
                articleView.ShowPaliContentTrans(book, channel);
            else                    //pali原文
                articleView.ShowPaliContent(book);
        }
        else
            articleView.ArticleNodeBtnClick(article);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
