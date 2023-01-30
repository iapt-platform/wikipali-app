using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ArticleController;
using static ArticleManager;

public class ArticleView : MonoBehaviour
{
    ArticleController controller = ArticleController.Instance();
    public ArticleNodeItemView nodeItem;
    public ArticleTitleReturnBtn returnBtn;
    Stack<ArticleTreeNode> articleTreeNodeStack;
    Stack<Book> bookTreeNodeStack;
    // Start is called before the first frame update
    void Start()
    {
        articleTreeNodeStack = new Stack<ArticleTreeNode>();
        bookTreeNodeStack = new Stack<Book>();
        controller.GetArticleTreeNodeData();
        InitNodeItem(controller.articleTreeNodes.info);
    }
    List<GameObject> nodeList = new List<GameObject>();

    void InitNodeItem(List<ArticleTreeNode> info)
    {
        DestroyNodeList();
        int length = info.Count;
        float height = nodeItem.GetComponent<RectTransform>().sizeDelta.y;
        for (int i = 0; i < length; i++)
        {
            GameObject inst = Instantiate(nodeItem.gameObject, nodeItem.transform.parent);
            inst.transform.position = nodeItem.transform.position;
            inst.GetComponent<RectTransform>().position -= Vector3.up * height * i;

            inst.GetComponent<ArticleNodeItemView>().Init(info[i]);
            inst.SetActive(true);
            nodeList.Add(inst);
        }
    }
    void InitNodeItem(List<Book> info)
    {
        DestroyNodeList();
        int length = info.Count;
        float height = nodeItem.GetComponent<RectTransform>().sizeDelta.y;
        for (int i = 0; i < length; i++)
        {
            GameObject inst = Instantiate(nodeItem.gameObject, nodeItem.transform.parent);
            inst.transform.position = nodeItem.transform.position;
            inst.GetComponent<RectTransform>().position -= Vector3.up * height * i;

            inst.GetComponent<ArticleNodeItemView>().Init(info[i]);
            inst.SetActive(true);
            nodeList.Add(inst);
        }
    }
    //销毁下拉列表GO
    private void DestroyNodeList()
    {
        int length = nodeList.Count;
        if (length == 0)
            return;
        for (int i = 0; i < length; i++)
        {
            Destroy(nodeList[i]);
        }
        nodeList.Clear();
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void ArticleNodeBtnClick(ArticleTreeNode info, bool isReturn = false)
    {
        if (info.children != null && info.children.Count > 0)
        {
            InitNodeItem(info.children);
            if (!isReturn)
                articleTreeNodeStack.Push(info);
            string aPath = "";
            int length = articleTreeNodeStack.Count;
            ArticleTreeNode[] arr = articleTreeNodeStack.ToArray();
            for (int i = length - 1; i > -1; i--)
            {
                aPath += arr[i].name + "/";
            }
            returnBtn.SetPath(aPath);
        }
        else
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            List<Book> book = controller.GetBooks(info);
            sw.Stop();
            Debug.LogError(sw.ElapsedMilliseconds);
            InitNodeItem(book);
        }
    }
    public void BookNodeBtnClick(Book info, bool isReturn = false)
    {
        if (info.children != null && info.children.Count > 0)
        {
            InitNodeItem(info.children);
            if (!isReturn)
                bookTreeNodeStack.Push(info);
            string aPath = "";
            int length = bookTreeNodeStack.Count;
            Book[] arr = bookTreeNodeStack.ToArray();
            for (int i = length - 1; i > -1; i--)
            {
                aPath += arr[i].toc + "/";
            }
            returnBtn.SetPath(aPath);
        }
        else
        {

            ;
        }
    }
    public void ReturnBtnClick()
    {
        if (articleTreeNodeStack.Count == 0)
            return;
        articleTreeNodeStack.Pop();
        if (articleTreeNodeStack.Count > 0)
        {
            ArticleNodeBtnClick(articleTreeNodeStack.Peek(), true);
        }
        else
        {
            returnBtn.Init();
            InitNodeItem(controller.articleTreeNodes.info);
        }
    }
}
