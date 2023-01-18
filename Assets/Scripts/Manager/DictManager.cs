using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using Imdork.SQLite;

public class DictManager : MonoBehaviour
{
    public DBManager dbManager;
    //词典查词总览限制显示数量
    const int LIMIT_COUNT = 30;
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
            //TODO:是否需要混入word_en搜索结果？目前只搜索word，模糊查询也有word_en的效果
            var reader = db.SelectLike("bh-paper", inputStr,"word", LIMIT_COUNT);

            //调用SQLite工具  解析对应数据
            Dictionary<string, object>[] pairs = SQLiteTools.GetValues(reader);
            if (pairs != null)
            {
                int length = pairs.Length;
                for (int i = 0; i < length; i++)
                {
                    MatchedWord m = new MatchedWord()
                    {
                        id = pairs[i]["id"].ToString(),
                        word = pairs[i]["word"].ToString(),
                        meaning = pairs[i]["note"].ToString(),
                        dicID = pairs[i]["dict_id"].ToString(),
                    };

                    matchedWordList.Add(m);

                };
            }
        });
        return SortWordList(matchedWordList.ToArray());
    }
    /// <summary>
    /// 根据word字数从小到大排序
    /// </summary>
    MatchedWord[] SortWordList(MatchedWord[] array)
    {
        if (array.Length == 0)
            return array;

        QuickSort(array, 0, array.Length - 1);
        return array;
    }
    //快速排序
    public void QuickSort(MatchedWord[] array, int start, int end)
    {
        if (start < end)
        {
            int mid = Partition(array, start, end);
            QuickSort(array, start, mid - 1);
            QuickSort(array, mid + 1, end);
        }
    }

    //分治方法 把数组中一个数放置在确定的位置
    public int Partition(MatchedWord[] array, int start, int end)
    {
        MatchedWord x = array[end];//选取一个判定值(一般选取最后一个)
        int i = start;
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

        //将下标i的值与判定值交换
        array[end] = array[i];
        array[i] = x;

        return i;
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
