using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using Imdork.SQLite;

public class DictManager : MonoBehaviour
{
    public DBManager dbManager;
    class DicIDInfo
    {
        public string dictname;
        public string shortname;
        public string description;
        public string dest_lang;
        public string author;
        public string uuid;

        public string source;
        public string language;
    }
    //key:table name,value:词典信息
    Dictionary<string, DicIDInfo> dicInfoArr = new Dictionary<string, DicIDInfo>
        {
            { "bh-paper",new DicIDInfo{
            dictname = "《巴汉词典》,《巴汉词典》Mahāñāṇo Bhikkhu编著",
            shortname = "巴汉",
            description = "《巴汉词典》,《巴汉词典》Mahāñāṇo Bhikkhu编著",
            dest_lang = "zh-hans",
            author = "原著：中译：Mahāñāṇo Bhikkhu尊者",
            uuid = "f364d3dc-b611-471b-9a4f-531286b8c2c3",

            source = "_PAPER_",
            language = "zh"} }
        };
    /// <summary>
    /// 搜索匹配到的词汇的数据，返回给DicView,作为List显示
    /// </summary>
    public class MatchedWord
    {
        public string id;
        public string dicID;
        public string word;
        public string meaning;
    }
    /// <summary>
    /// 根据用户输入搜索数据库，返回显示需要的结果
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public MatchedWord[] MatchWord(string inputStr)
    {
        if (string.IsNullOrEmpty(inputStr))
            return new MatchedWord[0];
        List<MatchedWord> matchedWordList = new List<MatchedWord>();
        dbManager.Getdb(db =>
        {
            //查询该账号UID 大于50  对应的 LoginTime 和 User 对应字段数据
            var reader = db.SelectLike("bh-paper", inputStr);

            //调用SQLite工具  解析对应数据
            Dictionary<string, object>[] pairs = SQLiteTools.GetValues(reader);
            int length = pairs.Length;
            for (int i = 0; i < length; i++)
            {
                MatchedWord m = new MatchedWord()
                {
                    id = pairs[i]["id"].ToString(),
                    word = pairs[i]["word"].ToString(),
                    meaning = pairs[i]["note"].ToString(),
                };

                matchedWordList.Add(m);

            };
        });
        return matchedWordList.ToArray();
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
