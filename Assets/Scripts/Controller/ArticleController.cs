using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ArticleManager;
/// <summary>
/// 圣典控制类 单例
/// </summary>
public class ArticleController
{
    //懒汉式单例类.在第一次调用的时候实例化自己 
    private ArticleController() { }
    private static ArticleController controller = null;
    //静态工厂方法 
    public static ArticleController Instance()
    {
        if (controller == null)
        {
            controller = new ArticleController();
        }
        return controller;
    }
    ArticleManager manager = ArticleManager.Instance();
    //圣典分类方式
    public enum Category
    {
        Defualt,
        CSCD4
    }
    public Category category = Category.Defualt;
    #region 文章标题树形结构
    //文章标题树形结构
    //public class ArticleTreeNode
    //{
    //    public List<ArticleTreeNode> children;
    //    public int lv;
    //    public ArticleTreeNode parent;
    //}
    //文章标题树形结构
    [Serializable]
    public class ArticleTreeNode
    {
        public string name;
        //public string tag;
        public List<string> tag;
        public List<ArticleTreeNode> children;
        //public ArticleTreeNode parent;
    }
    [Serializable]
    public class ArticleTreeNodes
    {
        public List<ArticleTreeNode> info;
    }
    public ArticleTreeNodes articleTreeNodes;
    public void GetArticleTreeNodeData()
    {
        string json = "";
        switch (category)
        {
            case Category.Defualt:
                json = manager.ReadDefualtJson();
                break;
            case Category.CSCD4:
                json = manager.ReadCSCDJson();
                break;
        }
        articleTreeNodes = JsonUtility.FromJson<ArticleTreeNodes>(json);
    }

    #endregion
    #region bookID

    public class Book
    {
        public int id;
        public string toc;
        public int level;
        public int paragraph;//段落数
        public int parentP;//是父的paragraph?
        public Book parent;
        public List<Book> children;// = new List<Book>();
        public string translateName;
    }
    public List<Book> currentBookList;
    public List<Book> GetBooks(ArticleTreeNode node)
    {
        List<Book> res = new List<Book>();
        if (node == null)
            return res;
        List<BookDBData> bookDBList = manager.GetBooksFromTags(node.tag);
        //List<Book> tempChildren = new List<Book>();
        //ket = paragraph,value = book
        Dictionary<int, Book> bookKVP = new Dictionary<int, Book>();
        int length = bookDBList.Count;
        for (int i = 0; i < length; i++)
        {
            Book book = new Book()
            {
                id = bookDBList[i].id,
                toc = bookDBList[i].toc,
                level = bookDBList[i].level,
                paragraph = bookDBList[i].paragraph,
                parentP = bookDBList[i].parent,
            };
            //??????????????
            if (bookKVP.ContainsKey(book.paragraph))
                bookKVP.Clear();
            bookKVP.Add(book.paragraph, book);
            if (book.level == 1)
                res.Add(book);
            if (bookKVP.ContainsKey(book.parentP))
            {
                Book bookP = bookKVP[book.parentP];
                if (bookP.children == null)
                    bookP.children = new List<Book>();
                bookP.children.Add(book);
                book.parent = bookP;
            }
        }
        currentBookList = res;
        return res;
    }
    #endregion
}
