using Imdork.SQLite;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using static SettingManager;

public class ArticleManager
{
    //懒汉式单例类.在第一次调用的时候实例化自己 
    private ArticleManager() { }
    private static ArticleManager manager = null;
    //静态工厂方法 
    public static ArticleManager Instance()
    {
        if (manager == null)
        {
            manager = new ArticleManager();
        }
        return manager;
    }
    public DBManager dbManager = DBManager.Instance();

    /// <summary>
    /// 读取本地文件夹中的Json文件
    /// <param name="jsonName">json文件名或文件名路径</param>
    /// </summary>
    string ReadJsonFromStreamingAssetsPath(string jsonName)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(jsonName);
        return textAsset.text;
    }
    #region 读取Json目录树
    const string defualtJsonFilePath = "defualt";
    const string cscd4JsonFilePath = "cscd";
    public string ReadDefualtJson()
    {
        return ReadJsonFromStreamingAssetsPath("Json/PalicanonCategory/" + defualtJsonFilePath);
    }
    public string ReadCSCDJson()
    {
        return ReadJsonFromStreamingAssetsPath("Json/PalicanonCategory/" + cscd4JsonFilePath);
    }
    #endregion
    #region 读取Json目录翻译
    Dictionary<Language, string> languageTSPath = new Dictionary<Language, string>()
    {
        //{ "pali","default"},
        { Language.ZH_CN,"zh-cn"},
        { Language.ZH_TW,"zh-tw"},
        { Language.EN,"en"},
        { Language.MY,"my"},
        { Language.SI,"si"},
    };

    public string ReadCurrLanguageBookJson()
    {
        return ReadJsonFromStreamingAssetsPath("Json/book_index/a/" + languageTSPath[SettingManager.Instance().language]);
    }
    public string ReadPaliBookJson()
    {
        return ReadJsonFromStreamingAssetsPath("Json/book_index/a/default");
    }
    #endregion
    #region 读取数据库句子与释义
    public class BookDBData
    {
        public int id;
        public string toc;
        public int level;
        public int paragraph;//段落数
        public int chapter_len;//章节paragraph长度
        public int parent;//是父的paragraph?
        //public string translateName;
    }
    /// <summary>
    /// 输入Tag，返回book数据
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public (List<BookDBData>, List<int>) GetBooksFromTags(List<string> tag)
    {
        int length = tag.Count;
        List<BookDBData> bookList = new List<BookDBData>();
        List<int> bookIDList = new List<int>();
        if (tag == null || length == 0)
            return (bookList, bookIDList);
        List<string> anchorIDList = new List<string>();
        dbManager.Getdb(db =>
        {
            //查找tagID
            var readerTag = db.SelectArticleTag(tag.ToArray());
            Dictionary<string, object>[] tagPairs = SQLiteTools.GetValues(readerTag);
            //因为有orderby 减少循环次数
            bool isCoCountMatch = false;
            string tagCount = length.ToString();
            if (tagPairs != null)
            {
                int tagLength = tagPairs.Length;
                for (int t = 0; t < tagLength; t++)
                {
                    //取所有tag获取到的结果的交集
                    string anchorID = tagPairs[t]["anchor_id"].ToString();
                    string co = tagPairs[t]["co"].ToString();
                    if (co == tagCount)
                    {
                        isCoCountMatch = true;
                        anchorIDList.Add(anchorID);
                    }
                    else
                    {
                        if (isCoCountMatch)
                            break;
                    }
                };
            }

            var readerPali = db.SelectArticle(anchorIDList.ToArray());
            Dictionary<string, object>[] paliPairs = SQLiteTools.GetValues(readerPali);
            if (paliPairs != null)
            {
                int paliLength = paliPairs.Length;
                for (int p = 0; p < paliLength; p++)
                {
                    BookDBData book = new BookDBData()
                    {
                        id = int.Parse(paliPairs[p]["book"].ToString()),
                        paragraph = int.Parse(paliPairs[p]["paragraph"].ToString()),
                        level = int.Parse(paliPairs[p]["level"].ToString()),
                        toc = paliPairs[p]["toc"].ToString(),
                        chapter_len = int.Parse(paliPairs[p]["chapter_len"].ToString()),
                        parent = int.Parse(paliPairs[p]["parent"].ToString()),
                    };
                    bookList.Add(book);
                    if (!bookIDList.Contains(book.id))
                        bookIDList.Add(book.id);
                }
            }

        }, DBManager.SentenceDBurl);
        return (bookList, bookIDList);
    }
    /// <summary>
    /// 输入BookID，返回指定pargraph范围内的level>2&&<100的book数据
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public List<BookDBData> GetBookChildrenFromID(int bookID, int minPargraph, int maxPargraph)
    {

        List<BookDBData> bookList = new List<BookDBData>();

        dbManager.Getdb(db =>
        {
            var readerPali = db.SelectArticleChildren(bookID.ToString(), minPargraph.ToString(), maxPargraph.ToString());
            Dictionary<string, object>[] paliPairs = SQLiteTools.GetValues(readerPali);
            if (paliPairs != null)
            {
                int paliLength = paliPairs.Length;
                for (int p = 0; p < paliLength; p++)
                {
                    BookDBData book = new BookDBData()
                    {
                        id = int.Parse(paliPairs[p]["book"].ToString()),
                        paragraph = int.Parse(paliPairs[p]["paragraph"].ToString()),
                        level = int.Parse(paliPairs[p]["level"].ToString()),
                        toc = paliPairs[p]["toc"].ToString(),
                        chapter_len = int.Parse(paliPairs[p]["chapter_len"].ToString()),
                        parent = int.Parse(paliPairs[p]["parent"].ToString()),
                    };
                    bookList.Add(book);
                }
            }

        }, DBManager.SentenceDBurl);
        return bookList;
    }
    public class ChapterDBData
    {
        public string id;
        public int bookID;
        public int paragraph;//段落数
        public string language;
        public string title;
        public string channel_id;
        public float progress;
        //public Date
        //public string translateName;
    }
    /// <summary>
    /// 输入bookID List，返回chapter数据
    /// </summary>
    public List<ChapterDBData> GetChaptersFromBookIDs(List<int> bookIDList)
    {
        List<ChapterDBData> cList = new List<ChapterDBData>();
        if (bookIDList == null || bookIDList.Count == 0)
            return cList;
        dbManager.Getdb(db =>
        {

            var readerPali = db.SelectChapter(bookIDList.ToArray());
            Dictionary<string, object>[] paliPairs = SQLiteTools.GetValues(readerPali);
            if (paliPairs != null)
            {
                int paliLength = paliPairs.Length;
                for (int p = 0; p < paliLength; p++)
                {

                    string title = "";
                    if (paliPairs[p].ContainsKey("title"))
                        title = paliPairs[p]["title"].ToString();


                    //int.Parse(paliPairs[p]["book"].ToString());
                    //int.Parse(paliPairs[p]["paragraph"].ToString());
                    string language = "pali";
                    if (paliPairs[p].ContainsKey("language"))
                        language = paliPairs[p]["language"].ToString();
                    //paliPairs[p]["language"].ToString();
                    //paliPairs[p]["channel_id"].ToString();
                    //float.Parse(paliPairs[p]["progress"].ToString());

                    ChapterDBData c = new ChapterDBData()
                    {
                        id = paliPairs[p]["id"].ToString(),
                        bookID = int.Parse(paliPairs[p]["book"].ToString()),
                        paragraph = int.Parse(paliPairs[p]["paragraph"].ToString()),
                        language = language,
                        title = title,
                        channel_id = paliPairs[p]["channel_id"].ToString(),
                        progress = float.Parse(paliPairs[p]["progress"].ToString()),
                    };
                    cList.Add(c);
                }
            }

        }, DBManager.SentenceDBurl);
        return cList;
    }
    public List<ChapterDBData> GetChaptersFromBookID(int bookID)
    {
        List<ChapterDBData> cList = new List<ChapterDBData>();
        dbManager.Getdb(db =>
        {

            var readerPali = db.SelectChapter(bookID);
            Dictionary<string, object>[] paliPairs = SQLiteTools.GetValues(readerPali);
            if (paliPairs != null)
            {
                int paliLength = paliPairs.Length;
                for (int p = 0; p < paliLength; p++)
                {

                    string title = "";
                    if (paliPairs[p].ContainsKey("title"))
                        title = paliPairs[p]["title"].ToString();


                    //int.Parse(paliPairs[p]["book"].ToString());
                    //int.Parse(paliPairs[p]["paragraph"].ToString());
                    string language = "pali";
                    if (paliPairs[p].ContainsKey("language"))
                        language = paliPairs[p]["language"].ToString();
                    //paliPairs[p]["language"].ToString();
                    //paliPairs[p]["channel_id"].ToString();
                    //float.Parse(paliPairs[p]["progress"].ToString());

                    ChapterDBData c = new ChapterDBData()
                    {
                        id = paliPairs[p]["id"].ToString(),
                        bookID = int.Parse(paliPairs[p]["book"].ToString()),
                        paragraph = int.Parse(paliPairs[p]["paragraph"].ToString()),
                        language = language,
                        title = title,
                        channel_id = paliPairs[p]["channel_id"].ToString(),
                        progress = float.Parse(paliPairs[p]["progress"].ToString()),
                    };
                    cList.Add(c);
                }
            }

        }, DBManager.SentenceDBurl);
        return cList;
    }
    #endregion
}
