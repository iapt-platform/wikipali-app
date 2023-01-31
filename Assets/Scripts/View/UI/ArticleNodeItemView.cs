using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static ArticleController;

public class ArticleNodeItemView : MonoBehaviour
{
    public Text titleText;
    public Text subTitleText;
    public ArticleTreeNode article;
    public Book book;
    public Button btn;
    public ArticleView articleView;
    //是否脱离了tag树形结构进入到选书和章节
    public bool isBook;

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
    }
    public void OnBtnClick()
    {
        if (isBook)
            articleView.BookNodeBtnClick(book);

        else
            articleView.ArticleNodeBtnClick(article);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
