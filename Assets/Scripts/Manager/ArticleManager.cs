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
        public int parent;//是父的paragraph?
        //public string translateName;
    }
    /// <summary>
    /// 输入Tag，返回book数据
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public List<BookDBData> GetBooksFromTags(List<string> tag)
    {
        int length = tag.Count;
        List<BookDBData> bookList = new List<BookDBData>();
        if (tag == null || length == 0)
            return bookList;
        List<string> anchorIDList = new List<string>();
        dbManager.Getdb(db =>
        {
            List<string> tempList = new List<string>();

            //查找tagID
            var readerTag = db.SelectArticleTag(tag.ToArray());
            Dictionary<string, object>[] tagPairs = SQLiteTools.GetValues(readerTag);
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
                        //isCoCountMatch = true;
                        anchorIDList.Add(anchorID);
                    }
                    //else
                    //{
                    //    if (isCoCountMatch)
                    //        break;
                    //}
                };
            }


            //length = anchorIDList.Count;

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
                        parent = int.Parse(paliPairs[p]["parent"].ToString()),
                    };
                    bookList.Add(book);
                }
            }

        }, DBManager.SentenceDBurl);
        return bookList;
    }
    #endregion
}
