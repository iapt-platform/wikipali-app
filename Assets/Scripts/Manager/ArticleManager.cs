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
        { Language.JP,"en"},
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
    #region 读取数据库目录树
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
                    string toc = "";
                    if (paliPairs[p].ContainsKey("toc"))
                        toc = paliPairs[p]["toc"].ToString();
                    BookDBData book = new BookDBData()
                    {
                        id = int.Parse(paliPairs[p]["book"].ToString()),
                        paragraph = int.Parse(paliPairs[p]["paragraph"].ToString()),
                        level = int.Parse(paliPairs[p]["level"].ToString()),
                        toc = toc,
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
                    string toc = "";
                    if (paliPairs[p].ContainsKey("toc"))
                        toc = paliPairs[p]["toc"].ToString();

                    BookDBData book = new BookDBData()
                    {
                        id = int.Parse(paliPairs[p]["book"].ToString()),
                        paragraph = int.Parse(paliPairs[p]["paragraph"].ToString()),
                        level = int.Parse(paliPairs[p]["level"].ToString()),
                        toc = toc,
                        chapter_len = int.Parse(paliPairs[p]["chapter_len"].ToString()),
                        parent = int.Parse(paliPairs[p]["parent"].ToString()),
                    };
                    bookList.Add(book);
                }
            }

        }, DBManager.SentenceDBurl);
        return bookList;
    }
    /// <summary>
    /// 输入BookID，pargraph返回指定book数据
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public BookDBData GetBookChildrenFromID(int bookID, int pargraph)
    {
        BookDBData res = null;
        dbManager.Getdb(db =>
        {
            var readerPali = db.SelectArticle(bookID, pargraph);
            Dictionary<string, object> paliPair = SQLiteTools.GetValue(readerPali);
            if (paliPair != null)
            {
                string toc = "";
                if (paliPair.ContainsKey("toc"))
                    toc = paliPair["toc"].ToString();
                BookDBData book = new BookDBData()
                {
                    id = int.Parse(paliPair["book"].ToString()),
                    paragraph = int.Parse(paliPair["paragraph"].ToString()),
                    level = int.Parse(paliPair["level"].ToString()),
                    toc = toc,
                    chapter_len = int.Parse(paliPair["chapter_len"].ToString()),
                    parent = int.Parse(paliPair["parent"].ToString()),
                };
                res = book;
            }

        }, DBManager.SentenceDBurl);
        return res;
    }

    public class ChannelChapterDBData
    {
        public string channel_id;
        public string name;
        public Language language;
        public string summary;
    }
    public class ChapterDBData
    {
        public string id;
        public int bookID;
        public int paragraph;//段落数
        public string language;
        public string title;
        public string channel_id;
        public ChannelChapterDBData channelData;
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
    /// <summary>
    /// 返回channel数据，key : channelID ,value:channelData
    /// </summary>
    public Dictionary<string, ChannelChapterDBData> GetChannelDataByIDs(List<string> channelIDList)
    {
        Dictionary<string, ChannelChapterDBData> data = new Dictionary<string, ChannelChapterDBData>();
        if (channelIDList == null || channelIDList.Count == 0)
            return data;
        dbManager.Getdb(db =>
        {

            var readerPali = db.SelectChannel(channelIDList.ToArray());
            Dictionary<string, object>[] paliPairs = SQLiteTools.GetValues(readerPali);
            if (paliPairs != null)
            {
                int paliLength = paliPairs.Length;
                for (int p = 0; p < paliLength; p++)
                {

                    //?????默认为null的是中文？
                    Language l = Language.ZH_CN;
                    if (paliPairs[p].ContainsKey("language"))
                    {
                        string language = paliPairs[p]["language"].ToString();
                        switch (language)
                        {
                            case "zh":
                            case "zh-cn":
                                l = Language.ZH_CN;
                                break;
                            case "zh-tw":
                                l = Language.ZH_TW;
                                break;
                            case "en":
                            case "jp":
                                l = Language.EN;
                                break;
                            case "my":
                                l = Language.MY;
                                break;
                            case "si":
                                l = Language.MY;
                                break;
                        }
                    }

                    string summary = "";
                    if (paliPairs[p].ContainsKey("summary"))
                        summary = paliPairs[p]["summary"].ToString();
                    string name = "";
                    if (paliPairs[p].ContainsKey("name"))
                        name = paliPairs[p]["name"].ToString();

                    ChannelChapterDBData c = new ChannelChapterDBData()
                    {
                        channel_id = paliPairs[p]["id"].ToString(),
                        name = name,
                        language = l,
                        summary = summary,
                    };
                    data.Add(c.channel_id, c);
                }
            }
        }, DBManager.SentenceDBurl);
        return data;
    }
    /// <summary>
    /// 返回channel数据，key : channelID ,value:channelData
    /// </summary>
    public ChannelChapterDBData GetChannelDataByID(string channelID)
    {
        ChannelChapterDBData data = null;

        dbManager.Getdb(db =>
        {

            var readerPali = db.SelectChannel(channelID);
            Dictionary<string, object> paliPair = SQLiteTools.GetValue(readerPali);
            if (paliPair != null)
            {

                //?????默认为null的是中文？
                Language l = Language.ZH_CN;
                if (paliPair.ContainsKey("language"))
                {
                    string language = paliPair["language"].ToString();
                    switch (language)
                    {
                        case "zh":
                        case "zh-cn":
                            l = Language.ZH_CN;
                            break;
                        case "zh-tw":
                            l = Language.ZH_TW;
                            break;
                        case "en":
                        case "jp":
                            l = Language.EN;
                            break;
                        case "my":
                            l = Language.MY;
                            break;
                        case "si":
                            l = Language.MY;
                            break;
                    }
                }

                string summary = "";
                if (paliPair.ContainsKey("summary"))
                    summary = paliPair["summary"].ToString();
                string name = "";
                if (paliPair.ContainsKey("name"))
                    name = paliPair["name"].ToString();

                ChannelChapterDBData c = new ChannelChapterDBData()
                {
                    channel_id = paliPair["id"].ToString(),
                    name = name,
                    language = l,
                    summary = summary,
                };
                data = c;
            }

        }, DBManager.SentenceDBurl);
        return data;
    }
    #endregion
    #region 读取数据库句子与释义
    public class SentenceDBData
    {
        //public string id;
        public int bookID;
        public int paragraph;
        public int word_start;
        public int word_end;
        public string content;
    }
    public List<SentenceDBData> GetPaliSentenceByBookParagraph(int bookID, int min, int max)
    {
        List<SentenceDBData> res = new List<SentenceDBData>();
        dbManager.Getdb(db =>
        {

            var readerPali = db.SelectSentence(bookID, min.ToString(), max.ToString());
            Dictionary<string, object>[] paliPairs = SQLiteTools.GetValues(readerPali);
            if (paliPairs != null)
            {
                int paliLength = paliPairs.Length;
                for (int p = 0; p < paliLength; p++)
                {
                    string content = "";
                    if (paliPairs[p].ContainsKey("content"))
                        content = paliPairs[p]["content"].ToString();
                    SentenceDBData s = new SentenceDBData()
                    {
                        //id = paliPairs[p]["id"].ToString(),
                        bookID = int.Parse(paliPairs[p]["book"].ToString()),
                        paragraph = int.Parse(paliPairs[p]["paragraph"].ToString()),
                        word_start = int.Parse(paliPairs[p]["word_start"].ToString()),
                        word_end = int.Parse(paliPairs[p]["word_end"].ToString()),
                        content = content,

                    };
                    res.Add(s);
                }
            }
        }, DBManager.SentenceDBurl);
        return res;
    }
    public List<SentenceDBData> GetPaliSentenceTranslationByBookParagraph(int bookID, int min, int max, string channel)
    {
        List<SentenceDBData> res = new List<SentenceDBData>();
        dbManager.Getdb(db =>
        {

            var readerPali = db.SelectSentenceTranslation(bookID, min.ToString(), max.ToString(), channel);
            Dictionary<string, object>[] paliPairs = SQLiteTools.GetValues(readerPali);
            if (paliPairs != null)
            {
                int paliLength = paliPairs.Length;
                for (int p = 0; p < paliLength; p++)
                {
                    string content = "";
                    if (paliPairs[p].ContainsKey("content"))
                        content = paliPairs[p]["content"].ToString();
                    SentenceDBData s = new SentenceDBData()
                    {
                        //id = paliPairs[p]["id"].ToString(),
                        bookID = int.Parse(paliPairs[p]["book"].ToString()),
                        paragraph = int.Parse(paliPairs[p]["paragraph"].ToString()),
                        word_start = int.Parse(paliPairs[p]["word_start"].ToString()),
                        word_end = int.Parse(paliPairs[p]["word_end"].ToString()),
                        content = content,
                    };
                    res.Add(s);
                }
            }
        }, DBManager.SentenceDBurl);
        return res;
    }
    #endregion

    #region 文章收藏
    public class ArticleGroupInfo
    {
        public int groupID;
        public string groupName;
        public List<string> bookTitleList;
        public List<int> bookIDList;
        public List<string> channelIDList;//channel ID为空是pali原文
    }

    //所有单词本
    public List<ArticleGroupInfo> allArticleGroup = new List<ArticleGroupInfo>();
    public int articleGroupCount;
    /// <summary>
    /// 加载所有收藏
    /// </summary>
    public void LoadAllArticleGroup()
    {
        int groupCount = PlayerPrefs.GetInt("articleGroupCount");
        string[] dicGroupNameArr = PlayerPrefsX.GetStringArray("articleGroupName");
        allArticleGroup.Clear();
        for (int i = 0; i < groupCount; i++)
        {
            ArticleGroupInfo dg = new ArticleGroupInfo();
            dg.groupID = i;
            dg.groupName = dicGroupNameArr[i];
            string[] articleTitleArr = PlayerPrefsX.GetStringArray("articleTitle" + i);
            int[] bookIDArr = PlayerPrefsX.GetIntArray("bookID" + i);
            string[] channelIDArr = PlayerPrefsX.GetStringArray("channelID" + i);
            int wl = articleTitleArr.Length;
            for (int j = 0; j < wl; j++)
            {
                dg.bookTitleList.Add(articleTitleArr[j]);
                dg.bookIDList.Add(bookIDArr[j]);
                dg.channelIDList.Add(channelIDArr[j]);
            }
            allArticleGroup.Add(dg);
        }
        articleGroupCount = groupCount;
    }
    void ClearArticleGroupData()
    {
        PlayerPrefs.DeleteKey("articleGroupName");
        for (int i = 0; i < articleGroupCount; i++)
        {
            PlayerPrefs.DeleteKey("articleTitle" + i);
            PlayerPrefs.DeleteKey("bookID" + i);
            PlayerPrefs.DeleteKey("channelID" + i);
        }
        //dicGroupCount = 0;
    }
    public void ModifyArticleGroup()
    {
        PlayerPrefs.SetInt("articleGroupCount", allArticleGroup.Count);
        articleGroupCount = allArticleGroup.Count;
        ClearArticleGroupData();
        List<string> dicNameList = new List<string>();
        for (int i = 0; i < articleGroupCount; i++)
        {
            dicNameList.Add(allArticleGroup[i].groupName);
            PlayerPrefsX.SetStringArray("articleTitle" + i, allArticleGroup[i].bookTitleList.ToArray());
            PlayerPrefsX.SetIntArray("bookID" + i, allArticleGroup[i].bookIDList.ToArray());
            PlayerPrefsX.SetStringArray("channelID" + i, allArticleGroup[i].channelIDList.ToArray());

        }
        PlayerPrefsX.SetStringArray("articleGroupName", dicNameList.ToArray());
    }
    public void DelGroup(int id)
    {
        allArticleGroup.RemoveAt(id);
        int groupCount = allArticleGroup.Count;
        articleGroupCount = groupCount;
        for (int i = 0; i < groupCount; i++)
        {
            allArticleGroup[i].groupID = i;
        }
        ModifyArticleGroup();
    }
    public void AddGroup(string gName)
    {
        ArticleGroupInfo group = new ArticleGroupInfo();
        group.groupName = gName;
        group.groupID = articleGroupCount;
        allArticleGroup.Add(group);
        int groupCount = allArticleGroup.Count;
        articleGroupCount = groupCount;
        ModifyArticleGroup();
    }
    public void DelArticle(int groupID, string articleTitle)//,int bookID,string channelID)
    {
        //todo 以文章标题查找是否唯一？？？？？？可能会出现误删bug
        int index = allArticleGroup[groupID].bookTitleList.IndexOf(articleTitle);
        allArticleGroup[groupID].bookTitleList.RemoveAt(index);
        allArticleGroup[groupID].bookIDList.RemoveAt(index);
        allArticleGroup[groupID].channelIDList.RemoveAt(index);
        PlayerPrefsX.SetStringArray("articleTitle" + groupID, allArticleGroup[groupID].bookTitleList.ToArray());
        PlayerPrefsX.SetIntArray("bookID" + groupID, allArticleGroup[groupID].bookIDList.ToArray());
        PlayerPrefsX.SetStringArray("channelID" + groupID, allArticleGroup[groupID].channelIDList.ToArray());
    }
    public void AddArticle(int groupID, string articleTitle, int bookID, string channelID)
    {
        allArticleGroup[groupID].bookTitleList.Add(articleTitle);
        allArticleGroup[groupID].bookIDList.Add(bookID);
        allArticleGroup[groupID].channelIDList.Add(channelID);
        PlayerPrefsX.SetStringArray("articleTitle" + groupID, allArticleGroup[groupID].bookTitleList.ToArray());
        PlayerPrefsX.SetIntArray("bookID" + groupID, allArticleGroup[groupID].bookIDList.ToArray());
        PlayerPrefsX.SetStringArray("channelID" + groupID, allArticleGroup[groupID].channelIDList.ToArray());
    }
    //改组名
    public void ChangeGroupName(int groupID, string name)
    {
        string[] nameArr = PlayerPrefsX.GetStringArray("articleGroupName");
        nameArr[groupID] = name;
        PlayerPrefsX.SetStringArray("articleGroupName", nameArr);
    }
    public StarGroupArticleView articleStarGroup;
    /// <summary>
    /// 当前单词是否被收藏
    /// </summary>
    /// <param name="word"></param>
    public void SetArticleStar(string articleTitle, int bookID, string channelID)
    {
        bool isStar = false;
        int l = allArticleGroup.Count;
        for (int i = 0; i < l; i++)
        {
            if (allArticleGroup[i].bookTitleList.Contains(articleTitle))
            {
                for (int j = 0; j < allArticleGroup[i].bookTitleList.Count; j++)
                {
                    if (allArticleGroup[i].bookTitleList[j] == articleTitle &&
                       allArticleGroup[i].bookIDList[j] == bookID &&
                       allArticleGroup[i].channelIDList[j] == channelID)
                    {
                        isStar = true;
                        break;
                    }
                }
            }
        }
        articleStarGroup.SetToggleValue(isStar);
    }

    public bool IsContainsWord(int groupId, string articleTitle, int bookID, string channelID)
    {
        for (int j = 0; j < allArticleGroup[groupId].bookTitleList.Count; j++)
        {
            if (allArticleGroup[groupId].bookTitleList[j] == articleTitle &&
               allArticleGroup[groupId].bookIDList[j] == bookID &&
               allArticleGroup[groupId].channelIDList[j] == channelID)
            {
                return true;
            }
        }
        return false;
    }
    #endregion
}
