using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
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
    //初始化读取数据
    public void Init()
    {
        GetArticleTreeNodeData();
        GetArticleTSData();
    }
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
    void GetArticleTreeNodeData()
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

    //文章标题翻译
    [Serializable]
    public class ArticleTreeTS
    {
        public string name;
        public int row;//book ID
        public string id;
        public string folder;
        public string title;
        public string c1;
        public string c2;
        public string c3;
        public string c4;
    }
    [Serializable]
    public class ArticleTreeTSs
    {
        public List<ArticleTreeTS> info;
    }
    public ArticleTreeTSs defaultTS;
    public ArticleTreeTSs languageTS;
    void GetArticleTSDataJson()
    {
        defaultTS = JsonUtility.FromJson<ArticleTreeTSs>(manager.ReadPaliBookJson());
        languageTS = JsonUtility.FromJson<ArticleTreeTSs>(manager.ReadCurrLanguageBookJson());
    }
    //翻译词典 key = pali，value = 翻译
    public Dictionary<string, string> tsDic = new Dictionary<string, string>();
    //翻译词典 key = bookID，value = 翻译
    public Dictionary<int, string> tsBookDic = new Dictionary<int, string>();
    void GetArticleTSData()
    {
        GetArticleTSDataJson();
        int length = defaultTS.info.Count;
        for (int i = 0; i < length; i++)
        {
            ArticleTreeTS lInfo = languageTS.info[i];
            ArticleTreeTS dInfo = defaultTS.info[i];
            tsBookDic.Add(lInfo.row, lInfo.title);
            if (!string.IsNullOrEmpty(dInfo.title) && !tsDic.ContainsKey(dInfo.title))
            {
                tsDic.Add(dInfo.title, lInfo.title);
            }
            if (!string.IsNullOrEmpty(dInfo.title) && !tsDic.ContainsKey(dInfo.title))
            {
                tsDic.Add(dInfo.title, lInfo.title);
            }
            if (!string.IsNullOrEmpty(dInfo.c1) && !tsDic.ContainsKey(dInfo.c1))
            {
                tsDic.Add(dInfo.c1, lInfo.c1);
            }
            if (!string.IsNullOrEmpty(dInfo.c2) && !tsDic.ContainsKey(dInfo.c2))
            {
                tsDic.Add(dInfo.c2, lInfo.c2);
            }
            if (!string.IsNullOrEmpty(dInfo.c3) && !tsDic.ContainsKey(dInfo.c3))
            {
                tsDic.Add(dInfo.c3, lInfo.c3);
            }
            if (!string.IsNullOrEmpty(dInfo.c4) && !tsDic.ContainsKey(dInfo.c4))
            {
                tsDic.Add(dInfo.c4, lInfo.c4);
            }
        }
    }
    #endregion
    #region book树形结构

    public class Book
    {
        public int id;
        public string toc;
        public int level;
        public int paragraph;//段落数
        public int parentP;//是父的paragraph?
        public int chapter_len;//章节paragraph长度
        public Book parent;
        public List<Book> children;// = new List<Book>();
        public string translateName;
        //章节部分需要
        public bool isHaveProgress = false;//是否有进度条
        public float progress;
        public List<ChapterDBData> chapterDBDatas;
        //是否已获取channelData
        public bool isGetChannelData = false;
    }
    public List<Book> currentBookList;
    //获取level<=2的所有书籍&章节
    public List<Book> GetBooks(ArticleTreeNode node)
    {
        List<Book> res = new List<Book>();
        if (node == null)
            return res;
        List<BookDBData> bookDBList;
        List<int> bookIDList;
        //此处只能获取book level <=2的内容
        (bookDBList, bookIDList) = manager.GetBooksFromTags(node.tag);
        List<ChapterDBData> cList = manager.GetChaptersFromBookIDs(bookIDList);
        Dictionary<int, Dictionary<int, Book>> bookKVP = new Dictionary<int, Dictionary<int, Book>>();
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
                chapter_len = bookDBList[i].chapter_len,
                translateName = bookDBList[i].toc,
            };

            if (!bookKVP.ContainsKey(book.id))
            {
                bookKVP.Add(book.id, new Dictionary<int, Book>());
            }
            bookKVP[book.id].Add(book.paragraph, book);
            if (book.level == 1)
            {
                res.Add(book);
                //翻译
                string ts = "";
                bool isHaveTs = tsBookDic.TryGetValue(book.id, out ts);
                if (isHaveTs)
                    book.translateName = ts;
            }
            //获取章节的版本风格信息
            else if (book.level < 100)
            {
                List<ChapterDBData> cDataList = GetChapterListByBookData(book.id, book.paragraph, cList);
                SetChapterListByBookData(book, cDataList);
            }
            if (bookKVP[book.id].ContainsKey(book.parentP))
            {
                Book bookP = bookKVP[book.id][book.parentP];
                if (bookP.children == null)
                    bookP.children = new List<Book>();
                bookP.children.Add(book);
                book.parent = bookP;
            }
        }
        currentBookList = res;
        return res;
    }
    Book SetChapterListByBookData(Book book, List<ChapterDBData> cDataList)
    {
        if (cDataList == null || cDataList.Count == 0)
        {
            //??????????
            book.isHaveProgress = true;
            book.progress = 0;
            book.chapterDBDatas = cDataList;
        }
        else
        {
            book.isHaveProgress = true;
            //选取最高的进度
            book.progress = cDataList[0].progress;
            book.chapterDBDatas = cDataList;
            if (!string.IsNullOrEmpty(cDataList[0].title))
            {
                book.translateName = cDataList[0].title;
            }
        }
        return book;
    }
    List<ChapterDBData> GetChapterListByBookData(int bookID, int paragraph, List<ChapterDBData> cDataList)
    {
        List<ChapterDBData> res = new List<ChapterDBData>();
        int length = cDataList.Count;
        //由于排了序，相同在一起，只要不同就退出循环
        bool isMatch = false;
        for (int i = 0; i < length; i++)
        {
            if (cDataList[i].bookID == bookID && cDataList[i].paragraph == paragraph)
            {
                res.Add(cDataList[i]);
                isMatch = true;
            }
            else if (isMatch)
            {
                break;
            }
        }
        return res;
    }
    //获取level>2的所有书籍&章节,是level2书籍的子章节书籍
    public void GetBooksChildrenLevel(Book node)
    {
        List<Book> res = new List<Book>();
        if (node == null || node.chapter_len == 0)
            return;
#if DEBUG_PERFORMANCE
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        sw.Start();
#endif
        List<BookDBData> bookDBList;
        //此处只能获取指定pargraph范围内的level>2&&<100的book数据
        bookDBList = manager.GetBookChildrenFromID(node.id, node.paragraph, node.paragraph + node.chapter_len);
        int length = bookDBList.Count;
        List<ChapterDBData> cList = manager.GetChaptersFromBookID(node.id);
        for (int i = 0; i < length; i++)
        {
            Book book = new Book()
            {
                id = bookDBList[i].id,
                toc = bookDBList[i].toc,
                level = bookDBList[i].level,
                paragraph = bookDBList[i].paragraph,
                parentP = bookDBList[i].parent,
                translateName = bookDBList[i].toc,
            };
            res.Add(book);
            List<ChapterDBData> cDataList = GetChapterListByBookData(book.id, book.paragraph, cList);
            SetChapterListByBookData(book, cDataList);
        }
#if DEBUG_PERFORMANCE
        sw.Stop();
        Debug.LogError("【性能】查询子文章耗时：" + sw.ElapsedMilliseconds);
#endif
        node.children = res;
    }
    //获取章节版本风格列表
    public void GetChapterChannelData(Book book)
    {
        if (book.chapterDBDatas == null || book.chapterDBDatas.Count == 0 || book.isGetChannelData)
            return;
        List<string> channelList = new List<string>();
        int l = book.chapterDBDatas.Count;
        for (int i = 0; i < l; i++)
        {
            channelList.Add(book.chapterDBDatas[i].channel_id);
        }
        //todo channel
        Dictionary<string, ChannelChapterDBData> data = manager.GetChannelDataByIDs(channelList);
        for (int i = 0; i < l; i++)
        {
            string cID = book.chapterDBDatas[i].channel_id;
            if (data.ContainsKey(cID))
            {
                book.chapterDBDatas[i].channelData = data[cID];
            }
        }
        book.isGetChannelData = true;
    }
    #endregion

    #region 文章内容部分
    //获取巴利原文句子
    List<SentenceDBData> GetPaliSentenceByBook(Book book)
    {
        if (book == null)
            return new List<SentenceDBData>();
        BookDBData bookData = manager.GetBookChildrenFromID(book.id, book.paragraph);
        int chapter_len = book.chapter_len;
        if (bookData != null)
            chapter_len = bookData.chapter_len;
        List<SentenceDBData> res = manager.GetPaliSentenceByBookParagraph(book.id, book.paragraph, book.paragraph + chapter_len);
        return res;
    }
    //获取版本风格翻译
    List<SentenceDBData> GetPaliSentenceTranslateByBookChannel(Book book, ChannelChapterDBData channel)
    {
        if (book == null || channel == null)
            return new List<SentenceDBData>();
        BookDBData bookData = manager.GetBookChildrenFromID(book.id, book.paragraph);
        int chapter_len = book.chapter_len;
        if (bookData != null)
            chapter_len = bookData.chapter_len;
        List<SentenceDBData> res = manager.GetPaliSentenceTranslationByBookParagraph(book.id, book.paragraph, book.paragraph + chapter_len, channel.channel_id);
        return res;
    }
    //获取整个巴利原文
    //public string GetPaliContentText(Book book)
    //{
    //    if (book == null)
    //        return "";
    //    List<SentenceDBData> sentence = GetPaliSentenceByBook(book);
    //    if (sentence == null || sentence.Count == 0)
    //        return "";
    //    StringBuilder sb = new StringBuilder("");
    //    int l = sentence.Count;
    //    for (int i = 0; i < l; i++)
    //    {
    //        sb.AppendLine(sentence[i].content);
    //        sb.AppendLine("");
    //    }
    //    return sb.ToString();
    //}

    const int LINE_COUNT_LIMIT = 50;
    //获取整个巴利原文以及所有翻译
    //由于文字过长无法显示的问题，每50行，返回一个数组
    //todo 用行数衡量是否不准，是否需要改为字节数量
    public List<string> GetPaliContentTransText(Book book, ChannelChapterDBData channel, bool isTrans)
    {
        if (book == null)
            return null;
        List<SentenceDBData> sentence = GetPaliSentenceByBook(book);
        List<SentenceDBData> sentenceTrans = null;
        if (isTrans)
            sentenceTrans = GetPaliSentenceTranslateByBookChannel(book, channel);
        if (sentence == null || sentence.Count == 0)
            return null;
        List<string> res = new List<string>();
        StringBuilder sb = new StringBuilder("");
        int l = sentence.Count;
        int tl = 0;
        if (isTrans)
            tl = sentenceTrans.Count;
        int lineCount = 0;
        for (int i = 0; i < l; i++)
        {
            sb.AppendLine(sentence[i].content);
            sb.AppendLine("");
            lineCount += 2;
            //todo 优化
            if (isTrans)
                for (int j = 0; j < tl; j++)
                {
                    if (sentenceTrans[j].paragraph == sentence[i].paragraph && sentenceTrans[j].word_start == sentence[i].word_start)
                    {
                        //sb.AppendLine();
                        sb.AppendFormat("<color=#5895FF>{0}</color>", sentenceTrans[j].content);
                        sb.AppendLine("");
                        sb.AppendLine("");
                        lineCount += 3;
                        // break;
                    }
                }
            if (lineCount >= LINE_COUNT_LIMIT)
            {
                lineCount = 0;
                res.Add(sb.ToString());
                sb.Clear();
            }
        }
        res.Add(sb.ToString());
        sb.Clear();
        return res;
    }
    #endregion
}
