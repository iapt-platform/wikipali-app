﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using Imdork.SQLite;
using static SettingManager;

public class DictManager
{
    //懒汉式单例类.在第一次调用的时候实例化自己 
    private DictManager() { }
    private static DictManager manager = null;
    //静态工厂方法 
    public static DictManager Instance()
    {
        if (manager == null)
        {
            manager = new DictManager();
        }
        return manager;
    }

    public DBManager dbManager = DBManager.Instance();
    //词典查词总览限制显示数量
    public const int LIMIT_COUNT = 30;
    //class DicIDInfo
    //{
    //    public string dictname;
    //    public string shortname;
    //    public string description;
    //    public string dest_lang;
    //    public string author;
    //    public string uuid;

    //    public string source;
    //    public string language;
    //}
    #region summary列表查询
    /// <summary>
    /// 搜索匹配到的词汇的数据，返回给DicView,作为List显示
    /// </summary>
    public class MatchedWord
    {
        public string id;
        public string dicID;
        public string word_en;
        public string word;
        public string meaning;
        //反向查找用，查词义的位置
        public float chineseNoteIndex;
    }
    List<MatchedWord> SelectDictLike(DbAccess db, Dictionary<string, MatchedWord> matchedWordDic, string tableName, string inputStr)
    {
        List<MatchedWord> matchedWordList = new List<MatchedWord>();
        var reader = db.SelectDictLike(tableName, inputStr, "word", LIMIT_COUNT);

        //调用SQLite工具  解析对应数据
        Dictionary<string, object>[] pairs = SQLiteTools.GetValues(reader);
        if (pairs != null)
        {
            int length = pairs.Length;
            for (int i = 0; i < length; i++)
            {
                string word = pairs[i]["word"].ToString();
                if (!matchedWordDic.ContainsKey(word))
                {
                    MatchedWord m = new MatchedWord()
                    {
                        id = pairs[i]["id"].ToString(),
                        word = word,
                        meaning = pairs[i]["note"].ToString(),
                        dicID = pairs[i]["dict_id"].ToString(),
                    };
                    matchedWordList.Add(m);
                    matchedWordDic.Add(m.word, m);
                }
            };
        }

        //混入word_en搜索结果
        var readerEn = db.SelectDictLike(tableName, inputStr, "word_en", LIMIT_COUNT);

        //调用SQLite工具  解析对应数据
        Dictionary<string, object>[] pairsEn = SQLiteTools.GetValues(readerEn);
        if (pairsEn != null)
        {
            int length = pairsEn.Length;
            for (int i = 0; i < length; i++)
            {
                string word = pairsEn[i]["word"].ToString();
                if (!matchedWordDic.ContainsKey(word))
                {
                    MatchedWord m = new MatchedWord()
                    {
                        id = pairsEn[i]["id"].ToString(),
                        word = word,
                        meaning = pairsEn[i]["note"].ToString(),
                        dicID = pairsEn[i]["dict_id"].ToString(),
                    };
                    matchedWordList.Add(m);
                    matchedWordDic.Add(m.word, m);
                }
            };
        }
        return matchedWordList;
    }
    void GetDictLikeByLanguage(List<MatchedWord> matchedWordList, Language language, DbAccess db, Dictionary<string, MatchedWord> matchedWordDic, string inputStr)
    {
        string[] dicIDArr = DicLangDic[language];
        for (int i = 0; i < dicIDArr.Length; i++)
        {
            //todo:AddRange性能问题
            matchedWordList.AddRange(SelectDictLike(db, matchedWordDic, dicIDArr[i], inputStr));
            if (matchedWordList.Count >= LIMIT_COUNT)
                break;
        }
    }
    //反向搜索
    List<MatchedWord> SelectDictLikeChinese(DbAccess db, Dictionary<string, MatchedWord> matchedWordDic, string tableName, string inputStr)
    {
        List<MatchedWord> matchedWordList = new List<MatchedWord>();
        List<string> wordList = new List<string>();
        var reader = db.SelectDictLikeChinese(tableName, inputStr, "note", LIMIT_COUNT);
        //调用SQLite工具  解析对应数据
        Dictionary<string, object>[] pairs = SQLiteTools.GetValues(reader);
        if (pairs != null)
        {
            int length = pairs.Length;
            for (int i = 0; i < length; i++)
            {
                string word = pairs[i]["word"].ToString();
                if (!matchedWordDic.ContainsKey(word))
                {
                    MatchedWord m = new MatchedWord()
                    {
                        id = pairs[i]["id"].ToString(),
                        word = word,
                        //反向查找的高亮
                        meaning = pairs[i]["note"].ToString(),
                        dicID = pairs[i]["dict_id"].ToString(),
                    };
                    //减去词头长度(因为note字段含义会把单词拼写放在前面)
                    m.chineseNoteIndex = m.meaning.IndexOf(inputStr) - word.Length;
                    m.meaning = m.meaning.Replace(inputStr, "<color=#5895FF>" + inputStr + "</color>");
                    wordList.Add(m.word);
                    matchedWordList.Add(m);
                    matchedWordDic.Add(m.word, m);
                }
            };
        }
        //查单词出现次数
        if (wordList.Count > 0)
        { 
            var reader2 = db.SelectIn("word_count", "word", wordList.ToArray());
            //调用SQLite工具  解析对应数据
            Dictionary<string, object>[] pairs2 = SQLiteTools.GetValues(reader2);
            if (pairs2 != null)
            {
                int length = pairs2.Length;
                for (int i = 0; i < length; i++)
                {
                    string word = pairs2[i]["word"].ToString();
                    int count = int.Parse(pairs2[i]["count"].ToString());
                    matchedWordDic[word].chineseNoteIndex /= (float)(count + 1);
                }
            }
        }

        return matchedWordList;
    }
    //反向搜索
    void GetDictLikeChinese(List<MatchedWord> matchedWordList, DbAccess db, Dictionary<string, MatchedWord> matchedWordDic, string inputStr)
    {
        string[] dicIDArr = DicLangDic[Language.ZH_CN];
        for (int i = 0; i < dicIDArr.Length; i++)
        {
            //todo:AddRange性能问题
            matchedWordList.AddRange(SelectDictLikeChinese(db, matchedWordDic, dicIDArr[i], inputStr));
            if (matchedWordList.Count >= LIMIT_COUNT)
                break;
        }
    }
    /// <summary>
    /// 根据用户输入搜索数据库，返回显示需要的结果
    /// 逻辑根据选择语言，优先选择筛选的数据库
    /// 结果小于LIMIT_COUNT时加入查找其他数据库
    /// 按照 系统语言->英语->缅语的顺序
    /// </summary>
    public MatchedWord[] MatchWord(string inputStr)
    {
        if (string.IsNullOrEmpty(inputStr))
            return new MatchedWord[0];
        List<MatchedWord> matchedWordList = new List<MatchedWord>();

        dbManager.Getdb(db =>
        {
            //key: word,value: MatchedWord
            Dictionary<string, MatchedWord> matchedWordDic = new Dictionary<string, MatchedWord>();
            GetDictLikeByLanguage(matchedWordList, SettingManager.Instance().language, db, matchedWordDic, inputStr);
            if (matchedWordList.Count < LIMIT_COUNT)
            {
                //查完自己语言的词典，找不到就找英文，再找不到就找缅文
                if (SettingManager.Instance().language == Language.EN)
                {
                    GetDictLikeByLanguage(matchedWordList, Language.MY, db, matchedWordDic, inputStr);

                }
                else if (SettingManager.Instance().language != Language.MY)
                {
                    GetDictLikeByLanguage(matchedWordList, Language.EN, db, matchedWordDic, inputStr);
                    if (matchedWordList.Count < LIMIT_COUNT)
                        GetDictLikeByLanguage(matchedWordList, Language.MY, db, matchedWordDic, inputStr);
                }
            }
            //matchedWordList = SelectDictLike(db, matchedWordDic,"", inputStr);
        }, DBManager.DictDBurl);
        MatchedWord[] resT = SortWordList(matchedWordList.ToArray());
        int length = resT.Length > LIMIT_COUNT ? LIMIT_COUNT : resT.Length;
        //裁剪List到LIMIT_COUNT以下
        MatchedWord[] res = new MatchedWord[length];
        for (int i = 0; i < length; i++)
        {
            res[i] = resT[i];
        }
        return res;
    }
    //根据词义反向查词
    public MatchedWord[] MatchWordChinese(string inputStr)
    {
        if (string.IsNullOrEmpty(inputStr))
            return new MatchedWord[0];
        List<MatchedWord> matchedWordList = new List<MatchedWord>();

        dbManager.Getdb(db =>
        {
            //key: word,value: MatchedWord
            Dictionary<string, MatchedWord> matchedWordDic = new Dictionary<string, MatchedWord>();
            GetDictLikeChinese(matchedWordList, db, matchedWordDic, inputStr);
            //matchedWordList = SelectDictLike(db, matchedWordDic,"", inputStr);
        }, DBManager.DictDBurl);
        //排序方式，词意中，匹配的词越靠前，说明该单词越可能是该意思
        MatchedWord[] resT = SortWordChineseList(matchedWordList.ToArray());
        int length = resT.Length > LIMIT_COUNT ? LIMIT_COUNT : resT.Length;
        //裁剪List到LIMIT_COUNT以下
        MatchedWord[] res = new MatchedWord[length];
        for (int i = 0; i < length; i++)
        {
            res[i] = resT[i];
        }
        return res;
    }
    /// <summary>
    /// 根据word字数从小到大排序
    /// </summary>
    MatchedWord[] SortWordList(MatchedWord[] array)
    {
        if (array.Length == 0)
            return array;

        QuickSort(array, 0, array.Length - 1, false);
        return array;
    }
    //快速排序
    public void QuickSort(MatchedWord[] array, int start, int end, bool isChinese)
    {
        if (start < end)
        {
            int mid = Partition(array, start, end, isChinese);
            QuickSort(array, start, mid - 1, isChinese);
            QuickSort(array, mid + 1, end, isChinese);
        }
    }

    //分治方法 把数组中一个数放置在确定的位置
    public int Partition(MatchedWord[] array, int start, int end, bool isChinese)
    {
        MatchedWord x = array[end];//选取一个判定值(一般选取最后一个)
        int i = start;
        if (isChinese)
        {
            for (int j = start; j < end; j++)
            {
                if (array[j].chineseNoteIndex < x.chineseNoteIndex)
                {
                    //将下标j的值与下标i的值交换 保证i的前面都小于判定值
                    MatchedWord temp = array[j];
                    array[j] = array[i];
                    array[i] = temp;
                    i++;
                }
            }
        }
        else
        {
            for (int j = start; j < end; j++)
            {
                if (array[j].word.Length < x.word.Length)
                {
                    //将下标j的值与下标i的值交换 保证i的前面都小于判定值
                    MatchedWord temp = array[j];
                    array[j] = array[i];
                    array[i] = temp;
                    i++;
                }
            }
        }


        //将下标i的值与判定值交换
        array[end] = array[i];
        array[i] = x;

        return i;
    }
    /// <summary>
    /// 排序方式，词意中，匹配的词越靠前，说明该单词越可能是该意思
    /// 根据关键字在词义的序位来排序
    /// </summary>
    MatchedWord[] SortWordChineseList(MatchedWord[] array)
    {
        if (array.Length == 0)
            return array;
        QuickSort(array, 0, array.Length - 1, true);
        return array;
    }
    #endregion
    #region detail 查词 各个词典
    //todo 改为从数据库Dict index里读取
    //此处排序要按照最全的词典递减排序，减少查询的词典数量
    //汉语词典
    static string[] dicIDArr_ZH = new string[]
     {
            "bh-paper",//巴汉词典 9939
            "bhmf_paper",//巴汉增订 14035
            "syhy_huang_paper",//汉译パーリ语辞典-黃秉榮 5958
            "syhy_li_paper",//汉译パーリ语辞典-李瑩 708
            "bhfxch_paper",//巴汉佛学辞汇 175
            "blyrm_paper",//546
            "mahinda_paper",//393
            "blyzh_paper",//346
            "syhy_miji_paper",//90
            "bysy_paper",//76
            "yfhb_paper",//语法 779
            "pali_root_paper_zh",//词根 1698
            "speed_up_paper_zh",//词根 300
     };
    //英语词典
    static string[] dicIDArr_EN = new string[]
    {
            //"u_hau_sein_pali_roma_paper",//60751  善巧尊者说不需要这个词典
            "concise",//22565
            "pts_papter",//PTS P-E dictionary 16158
            "vir_paper_copy",//13508
            "proper_names_paper",//9074
            "buddhist_paper",//923
            "pali_root_paper",//词根 1698
            "speed_up_paper",//词根 300
            
    };
    //日语
    static string[] dicIDArr_JP = new string[]
{
            "syhy_paper",//13772
            "syzb",//3296
};
    //缅语
    static string[] dicIDArr_MY = new string[]
{
            "pm_grammar1",//巴缅词典???? 157271
            "pmt_paper",//T巴缅词典 157271
            "u_hau_sein_my_papter",//60751
            "pali_roots_paper",//词根 1894
};
    //越南语
    static string[] dicIDArr_VI = new string[]
{
            "pali_vi_paper",//9957
            "abhi_terms_paper",//4788
            "vinaya_terms_paper",//924
};
    //系统语言与优先查询词典
    Dictionary<Language, string[]> DicLangDic = new Dictionary<Language, string[]>
    {
        {Language.ZH_CN,dicIDArr_ZH  },
        {Language.ZH_TW,dicIDArr_ZH  },
        {Language.EN,dicIDArr_EN  },
        {Language.JP,dicIDArr_JP  },
        {Language.MY,dicIDArr_MY  },
        {Language.SI,dicIDArr_EN  },
    };
    List<string[]> AllDictList = new List<string[]> { dicIDArr_ZH, dicIDArr_EN, dicIDArr_JP, dicIDArr_MY, dicIDArr_VI };
    //TODO:改为dicID 在数据库查
    string[] dicIDArr = new string[]
        {
            "bh-paper",//巴汉词典
            "concise",//英文词典
            "syzb"//日语词典
        };
    public class MatchedWordDetail
    {
        public string id;
        public string dicID;
        public string dicName;
        public string word;
        public string meaning;
    }
    void SelectDicDetail(DbAccess db, List<MatchedWordDetail> matchedWordList, string tableName, string word)
    {
        var reader = db.SelectDictSame(tableName, word, "word", 1);

        //调用SQLite工具  解析对应数据
        Dictionary<string, object> pairs = SQLiteTools.GetValue(reader);
        if (pairs != null)
        {

            MatchedWordDetail m = new MatchedWordDetail()
            {
                id = pairs["id"].ToString(),
                word = pairs["word"].ToString(),
                meaning = pairs["note"].ToString(),
                dicID = pairs["dict_id"].ToString(),
            };
            m.meaning = MarkdownText.ReplaceHTMLStyle(m.meaning);
            var readerDic = db.SelectDic(m.dicID);
            Dictionary<string, object> dicPairs = SQLiteTools.GetValue(readerDic);
            m.dicName = dicPairs["dictname"].ToString();
            matchedWordList.Add(m);
        }

    }
    //当前查询的单词
    public string currWord = "";
    //当前所有查询结果
    public MatchedWordDetail[] currMatchedWordDetail;
    //查询每个词典，准确匹配
    public MatchedWordDetail[] MatchWordDetail(string word)
    {
        if (string.IsNullOrEmpty(word))
        {
            currMatchedWordDetail = new MatchedWordDetail[0];
            return currMatchedWordDetail;
        }
        List<MatchedWordDetail> matchedWordList = new List<MatchedWordDetail>();

        dbManager.Getdb(db =>
        {
            //优先查询当前系统语言词典
            string[] langDic = DicLangDic[SettingManager.Instance().language];
            int l = langDic.Length;
            for (int i = 0; i < l; i++)
            {
                SelectDicDetail(db, matchedWordList, langDic[i], word);
            }
            int c = AllDictList.Count;
            //其他语言结果
            for (int i = 0; i < c; i++)
            {
                if (AllDictList[i][0] != langDic[0])
                {
                    int cl = AllDictList[i].Length;
                    for (int j = 0; j < cl; j++)
                        SelectDicDetail(db, matchedWordList, AllDictList[i][j], word);
                }
            }
        }, DBManager.DictDBurl);

        currMatchedWordDetail = matchedWordList.ToArray();
        return currMatchedWordDetail;
    }
    //是否存在该词，只查最全面的巴缅词典
    public bool IsHaveWord(string word)
    {
        if (string.IsNullOrEmpty(word))
            return false;
        bool res = false;
        dbManager.Getdb(db =>
        {
            var reader = db.SelectDictExist(dicIDArr_MY[0], word, "word");

            //调用SQLite工具  解析对应数据
            Dictionary<string, object> pairs = SQLiteTools.GetValue(reader);
            if (pairs != null && pairs.Count > 0)
            {
                res = true;
            }
            //查两个词典太慢了
            //reader = db.SelectDictExist(dicIDArr_EN[1], word, "word");

            ////调用SQLite工具  解析对应数据
            //pairs = SQLiteTools.GetValue(reader);
            //if (pairs != null && pairs.Count > 0)
            //{
            //    res = true;
            //}
        }, DBManager.DictDBurl);


        return res;
    }
    #endregion


    #region 词典分组
    public StarGroupDictView dicStarGroup;
    public class DicGroupInfo
    {
        public int groupID;
        public string groupName;
        public List<string> wordList = new List<string>();
    }
    //所有单词本
    public List<DicGroupInfo> allDicGroup = new List<DicGroupInfo>();
    public int dicGroupCount;
    /// <summary>
    /// 加载所有单词本
    /// </summary>
    public void LoadAllDicGroup()
    {
        int groupCount = PlayerPrefs.GetInt("dicGroupCount");
        string[] dicGroupNameArr = PlayerPrefsX.GetStringArray("dicGroupName");
        allDicGroup.Clear();
        for (int i = 0; i < groupCount; i++)
        {
            DicGroupInfo dg = new DicGroupInfo();
            dg.groupID = i;
            dg.groupName = dicGroupNameArr[i];
            string[] wordArr = PlayerPrefsX.GetStringArray("dic" + i);
            int wl = wordArr.Length;
            for (int j = 0; j < wl; j++)
            {
                dg.wordList.Add(wordArr[j]);
            }
            allDicGroup.Add(dg);
        }
        dicGroupCount = groupCount;
    }
    void ClearDicGroupData()
    {
        PlayerPrefs.DeleteKey("dicGroupName");
        for (int i = 0; i < dicGroupCount; i++)
        {
            PlayerPrefs.DeleteKey("dic" + i);
        }
        //dicGroupCount = 0;
    }
    public void ModifyDicGroup()
    {
        PlayerPrefs.SetInt("dicGroupCount", allDicGroup.Count);
        dicGroupCount = allDicGroup.Count;
        ClearDicGroupData();
        List<string> dicNameList = new List<string>();
        for (int i = 0; i < dicGroupCount; i++)
        {
            dicNameList.Add(allDicGroup[i].groupName);
            PlayerPrefsX.SetStringArray("dic" + i, allDicGroup[i].wordList.ToArray());

        }
        PlayerPrefsX.SetStringArray("dicGroupName", dicNameList.ToArray());
    }
    public void DelGroup(int id)
    {
        allDicGroup.RemoveAt(id);
        int groupCount = allDicGroup.Count;
        dicGroupCount = groupCount;
        for (int i = 0; i < groupCount; i++)
        {
            allDicGroup[i].groupID = i;
        }
        ModifyDicGroup();
    }
    public void AddGroup(string gName)
    {
        DicGroupInfo group = new DicGroupInfo();
        group.groupName = gName;
        group.groupID = dicGroupCount;
        allDicGroup.Add(group);
        int groupCount = allDicGroup.Count;
        dicGroupCount = groupCount;
        ModifyDicGroup();
    }
    public void DelWord(int groupID, string word)
    {
        allDicGroup[groupID].wordList.Remove(word);
        PlayerPrefsX.SetStringArray("dic" + groupID, allDicGroup[groupID].wordList.ToArray());
    }
    public void AddWord(int groupID, string word)
    {
        allDicGroup[groupID].wordList.Add(word);
        PlayerPrefsX.SetStringArray("dic" + groupID, allDicGroup[groupID].wordList.ToArray());
    }
    //改组名
    public void ChangeGroupName(int groupID, string name)
    {
        string[] nameArr = PlayerPrefsX.GetStringArray("dicGroupName");
        nameArr[groupID] = name;
        PlayerPrefsX.SetStringArray("dicGroupName", nameArr);
    }
    /// <summary>
    /// 当前单词是否被收藏
    /// </summary>
    /// <param name="word"></param>
    public void SetWordStar(string word)
    {
        bool isStar = false;
        int l = allDicGroup.Count;
        for (int i = 0; i < l; i++)
        {
            if (allDicGroup[i].wordList.Contains(word))
            {
                isStar = true;
                break;
            }
        }
        dicStarGroup.SetToggleValue(isStar);
    }
    public bool IsContainsWord(int groupId, string word)
    {
        if (allDicGroup[groupId].wordList.Contains(word))
        {
            return true;
        }
        return false;
    }
    #endregion
}
