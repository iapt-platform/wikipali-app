using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
/// <summary>
/// CSV批量导入数据库
/// </summary>
public class CSV2SQLiteDBs
{
    static DBManager dbManager = DBManager.Instance();
    //public struct DictItem
    //{
    //    public int id;
    //    public string lang;
    //    public int sn;
    //    public string word_en;
    //    public string word;
    //    public string word2;//废弃，首字母大写的pali
    //    public string note;
    //    //public string dict_id;//
    //}
    public struct DictItem
    {
        public string id;
        public string lang;
        //public string sn;
        public string word_en;
        public string word;
        public string word2;//废弃，首字母大写的pali
        public string note;
        //public string dict_id;//
    }
    public struct DictIndexItem
    {
        public string uuid;
        public string dictname;
        public string shortname;
        public string description;
        public string dest_lang;
    }
    //去掉chapter文件title变量 换行
    [MenuItem("Assets/Tools/CSV2SQLiteDB")]
    public static void CSV2SQLiteDB()
    {
        //支持多选
        string[] guids = Selection.assetGUIDs;
        string assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);//通过GUID获取路径
        //string assetPathCSV = AssetDatabase.GUIDToAssetPath(guids[0]);//通过GUID获取路径
        //string assetPathINI = AssetDatabase.GUIDToAssetPath(guids[1]);//通过GUID获取路径
        DirectoryInfo folder = new DirectoryInfo(assetPath);
        FileInfo assetCSV = folder.GetFiles("*.csv")[0];
        FileInfo assetINI = folder.GetFiles("*.ini")[0];

        string fileNameWithoutExtension = folder.Name;// 没有扩展名的文件名
        List<DictItem> dList = ReadCSV(assetCSV.FullName);
        DictIndexItem dIndex = ReadIni(assetINI.FullName);
        Create(fileNameWithoutExtension, dList, dIndex.uuid);
        InsertDictIndex(dIndex);
        Debug.LogError("创建结束");
    }
    //文件夹含有多个csv的情况
    [MenuItem("Assets/Tools/CSV2SQLiteDBMutiCSV")]
    public static void CSV2SQLiteDBMutiCSV()
    {
        //支持多选
        string[] guids = Selection.assetGUIDs;
        string assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);//通过GUID获取路径
        //string assetPathCSV = AssetDatabase.GUIDToAssetPath(guids[0]);//通过GUID获取路径
        //string assetPathINI = AssetDatabase.GUIDToAssetPath(guids[1]);//通过GUID获取路径
        DirectoryInfo folder = new DirectoryInfo(assetPath);
        FileInfo[] assetCSVs = folder.GetFiles("*.csv");
        string[] csvPath = new string[assetCSVs.Length];
        for (int i = 0; i < assetCSVs.Length; i++)
        {
            csvPath[i] = assetCSVs[i].FullName;
        }
        FileInfo assetINI = folder.GetFiles("*.ini")[0];

        string fileNameWithoutExtension = folder.Name;// 没有扩展名的文件名
        List<DictItem> dList = ReadMutiCSV(csvPath);
        DictIndexItem dIndex = ReadIni(assetINI.FullName);
        Create(fileNameWithoutExtension, dList, dIndex.uuid);
        InsertDictIndex(dIndex);
        Debug.LogError("创建结束");
    }
    //创建新词典
    static void Create(string tableName, List<DictItem> dicItems, string dicID)
    {
        tableName = tableName.Replace("-", "_");
        tableName = tableName.Replace(" ", "_");

        dbManager.Getdb(db => db.CreateTable(tableName, new[] { "id", /*"lang","sn", */ "word_en", "word", "note", "dict_id" },
        new[] { "INTEGER", /*"STRING", "INTEGER",*/ "STRING", "STRING", "STRING", "STRING"/*, "DATETIME"*/ }), DBManager.DictDBurl);
        int l = dicItems.Count;
        for (int i = 0; i < l; i++)
        {
            //string note = dicItems[i].note;
            //note = dicItems[i].note.Replace("'", "\"");
            //if (dicItems[i].id == "65525")
            //{
            //    Debug.LogError(dicItems[i].id);
            //    Debug.LogError(dicItems[i].lang);
            //    Debug.LogError(dicItems[i].sn);
            //    Debug.LogError(dicItems[i].word_en);
            //    Debug.LogError(dicItems[i].word);
            //    Debug.LogError(dicItems[i].note);
            //    note = dicItems[i].note.Replace("'", "");
            //    Debug.LogError(dicID);

            //}
            dbManager.Getdb(db => db.InsertIntoSpecific(tableName, new[] { "id", /*"lang", "sn",*/ "word_en", "word", "note", "dict_id" }
            , new[] { dicItems[i].id, /*dicItems[i].lang, dicItems[i].sn,*/ dicItems[i].word_en,
                dicItems[i].word, dicItems[i].note, dicID })
            , DBManager.DictDBurl);

        }

    }
    //词典Index表里新增数据
    static void InsertDictIndex(DictIndexItem index)
    {
        dbManager.Getdb(db => db.InsertIntoSpecific("Dict", new[] { "uuid", "dictname", "shortname", "description", "dest_lang" }
        , new[] { index.uuid, index.dictname, index.shortname, index.description, index.dest_lang })
        , DBManager.DictDBurl);
    }
    static List<DictItem> ReadCSV(string assetPath)
    {
        string[] textTxt = File.ReadAllLines(assetPath);
        List<DictItem> res = new List<DictItem>();
        int l = textTxt.Length;
        for (int i = 1; i < l; i++)
        {
            string[] splitFields = textTxt[i].Split(new Char[] { ',' }, 7);
            DictItem item = new DictItem
            {
                id = splitFields[0],
                lang = splitFields[1],
                //sn = splitFields[2],
                word_en = splitFields[3].Replace("'", "＇"),//数据库对'符号报错
                word = splitFields[4].Replace("'", "＇"),//数据库对'符号报错
                word2 = splitFields[5],
                note = splitFields[6].Replace("'", "＇"),//数据库对'符号报错
            };
            res.Add(item);
        }
        return res;
    }
    static List<DictItem> ReadMutiCSV(string[] assetPath)
    {
        List<DictItem> res = new List<DictItem>();
        for (int j = 0; j < assetPath.Length; j++)
        {
            string[] textTxt = File.ReadAllLines(assetPath[j]);
            int l = textTxt.Length;
            for (int i = 1; i < l; i++)
            {
                string[] splitFields = textTxt[i].Split(new Char[] { ',' }, 7);
                DictItem item = new DictItem
                {
                    id = splitFields[0],
                    lang = splitFields[1],
                    //sn = splitFields[2],
                    word_en = splitFields[3].Replace("'", "＇"),//数据库对'符号报错
                    word = splitFields[4].Replace("'", "＇"),//数据库对'符号报错
                    word2 = splitFields[5],
                    note = splitFields[6].Replace("'", "＇"),//数据库对'符号报错
                };
                res.Add(item);
            }
        }
        return res;
    }
    static DictIndexItem ReadIni(string assetPath)
    {
        string[] textTxt = File.ReadAllLines(assetPath);
        DictIndexItem res = new DictIndexItem();
        int l = textTxt.Length;
        for (int i = 1; i < l; i++)
        {
            string[] splitFields = textTxt[i].Split(new Char[] { '=' });

            if (splitFields[0].Contains("dictname"))
            {
                string dictname = splitFields[1].Replace(" \"", "");
                dictname = dictname.Replace("\"", "");
                res.dictname = dictname;
            }
            else if (splitFields[0].Contains("shortname"))
            {
                string shortname = splitFields[1].Replace(" \"", "");
                shortname = shortname.Replace("\"", "");
                res.shortname = shortname;
            }
            else if (splitFields[0].Contains("description"))
            {
                string description = splitFields[1].Replace(" \"", "");
                description = description.Replace("\"", "");
                res.description = description.Replace("'", "＇");//数据库对'符号报错;
            }
            else if (splitFields[0].Contains("dest_lang"))
            {
                string dest_lang = splitFields[1].Replace(" \"", "");
                dest_lang = dest_lang.Replace("\"", "");
                res.dest_lang = dest_lang;
            }
            else if (splitFields[0].Contains("uuid"))
            {
                string uuid = splitFields[1].Replace(" \"", "");
                uuid = uuid.Replace("\"", "");
                res.uuid = uuid;
            }

        }
        return res;
    }
}
