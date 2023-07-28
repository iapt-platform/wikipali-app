using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager
{
    //懒汉式单例类.在第一次调用的时候实例化自己 
    private SettingManager() { }
    private static SettingManager manager = null;
    //静态工厂方法 
    public static SettingManager Instance()
    {
        if (manager == null)
        {
            manager = new SettingManager();
        }
        return manager;
    }
    //软件语言
    public enum Language
    {
        ZH_CN,
        ZH_TW,
        EN,
        JP,
        MY,
        SI
    }
    public Language language = Language.ZH_CN;
    public void InitGame()
    {
        //在此处无法解压缩？？？？？？？
        //??????CalendarManager.Instance().StartLocation();

        //覆盖更新判断版本号重新解压压缩包
        if (PlayerPrefs.HasKey("saveVersion"))
        {
            //版本号不相等默认覆盖更新过
            //覆盖更新过就重新解压安装包
            if (PlayerPrefs.GetString("saveVersion") != Application.version)
            {
                PlayerPrefs.SetString("saveVersion", Application.version);
                PlayerPrefs.SetInt("isUnZiped", 0);
            }
        }
        else
        {
            PlayerPrefs.SetString("saveVersion", Application.version);

        }
        //语速初始化
        if (!PlayerPrefs.HasKey("PaliSpeakVoiceSpeed"))
            PlayerPrefs.SetInt("PaliSpeakVoiceSpeed", -20);
        //初始化单词本
        if (!PlayerPrefs.HasKey("dicGroupCount"))
        {
            //?????默认是1？应该是0
            PlayerPrefs.SetInt("dicGroupCount", 1);
            PlayerPrefs.SetString("dicGroupName", "默认单词本");
            PlayerPrefs.SetString("dic0", "");
        }
        //PlayerPrefs.DeleteKey("articleGroupCount");
        if (!PlayerPrefs.HasKey("articleGroupCount"))
        {
            PlayerPrefs.SetInt("articleGroupCount", 1);
            PlayerPrefs.SetString("articleGroupName", "默认收藏");
            PlayerPrefs.SetString("articleTitle0", "");
            PlayerPrefs.SetString("bookID0", "0");
            PlayerPrefs.SetString("bookParagraph0", "0");
            PlayerPrefs.SetString("bookChapterLen0", "0");
            PlayerPrefs.SetString("channelID0", "");
            PlayerPrefs.SetString("channelName0", "");
        }
        if (!PlayerPrefs.HasKey("CalType"))
        {
            SettingManager.Instance().SetCalType(1);
        }
        if (!PlayerPrefs.HasKey("TransContent"))
        {
            SettingManager.Instance().SetTransContent(1);
        }
        //todo 目前是只在最开始解压数据库，要添加用户误删数据库的情况，判断数据库是否Exist，压缩包是否Exist，不Exist提示重新安装
        //是否已解压数据库
        //PlayerPrefs.SetInt("isUnZiped", 0);
        int isUnZipedDB = PlayerPrefs.GetInt("isUnZiped", 0);
        if (isUnZipedDB == 0)
        {
            ZipManager.Instance().UnZipDB();
        }
        CalendarManager.Instance().StartLocation();
        //加载单词本
        DictManager.Instance().LoadAllDicGroup();
        //加载文章收藏
        ArticleManager.Instance().LoadAllArticleGroup();
        //??????没执行到这里，有报错？？？
        //CalendarManager.Instance().StartLocation();

    }
    public void UnZipFin()
    {
        Debug.LogError("解压缩结束");
        PlayerPrefs.SetInt("isUnZiped", 1);

    }
    //设置日历类型
    public void SetCalType(int isMM)
    {
        PlayerPrefs.SetInt("CalType", isMM);
    }
    //获取日历类型
    public int GetCalType()//isMM
    {
        return PlayerPrefs.GetInt("CalType");
    }
    //设置pali不朗读括号内容
    public void SetPaliRemoveBracket(int boolean)
    {
        PlayerPrefs.SetInt("PaliRemoveBracket", boolean);
    }
    //获取pali不朗读括号内容
    public int GetPaliRemoveBracket()
    {
        return PlayerPrefs.GetInt("PaliRemoveBracket");
    }
    //设置译文显示pali原文
    public void SetTransContent(int boolean)
    {
        PlayerPrefs.SetInt("TransContent", boolean);
    }
    //获取译文显示pali原文
    public int GetTransContent()//isMM
    {
        return PlayerPrefs.GetInt("TransContent");
    }
    #region 打开先前的浏览内容
    //设置打开先前的浏览内容
    public void SetOpenLast(int boolean)
    {
        PlayerPrefs.SetInt("OpenLast", boolean);
    }
    //获取打开先前的浏览内容
    public int GetOpenLast()
    {
        return PlayerPrefs.GetInt("OpenLast");
    }
    //0:词典
    //1:圣典
    public void SetOpenLastType(int olType)
    {
        PlayerPrefs.SetInt("OpenLastType", olType);
    }
    public int GetOpenLastType()
    {
        return PlayerPrefs.GetInt("OpenLastType");
    }
    public void SetOpenLastDicWord(string word)
    {
        PlayerPrefs.SetString("OpenLastDicWord", word);
    }
    public string GetOpenLastDicWord()
    {
        return PlayerPrefs.GetString("OpenLastDicWord");
    }
    //组合bookId, bookParagraph, bookChapterLen, channelID
    public void SetOpenLastDicArticle(string article)
    {
        PlayerPrefs.SetString("OpenLastDicArticle", article);
    }
    public string GetOpenLastDicArticle()
    {
        return PlayerPrefs.GetString("OpenLastDicArticle");
    }

    public void SaveOpenLastWord(string word)
    {
        if (GetOpenLast() == 1)
        {
            SetOpenLastType(0);
            SetOpenLastDicWord(word);
        }
    }
    public void SaveOpenLastArticle(int bookID, int bookParagraph, int bookChapterLen, string channelId)
    {
        if (GetOpenLast() == 1)
        {
            SetOpenLastType(1);
            string save = bookID + "," + bookParagraph + "," + bookChapterLen + "," + channelId;
            SetOpenLastDicArticle(save);
        }
    }
    public void OpenLast()
    {
        if (GetOpenLast() == 1)
        {
            if (GetOpenLastType() == 0)//词典
            {
                string w = GetOpenLastDicWord();
                GameManager.Instance().ShowDicWord(w);
            }
            else//文章
            {
                string a = GetOpenLastDicArticle();
                string[] split = a.Split(',');
                if (split.Length > 3)
                    GameManager.Instance().ShowArticle(int.Parse(split[0]), int.Parse(split[1]), int.Parse(split[2]), split[3]);
            }
        }
    }

    #endregion
    #region 朗读选项
    public enum PaliSpeakVoiceGender
    {
        Male = 0,
        Female = 1,
    }
    public enum PaliSpeakVoiceType
    {
        Telugu = 0,//泰卢固语
        Myanmar = 1,//缅甸语
        //Sinhala = 2,//僧伽罗语
    }
    public static List<int> PaliSpeakVoiceSpeedList = new List<int> { 0, -10, -20, -30, -40 };
    public enum PaliSpeakVoiceSpeed
    {
        _0 = 0,
        _10 = -10,
        _20 = -20,
        _30 = -30,
        _40 = -40,
    }
    public PaliSpeakVoiceGender GetPaliVoiceGender()
    {
        return (PaliSpeakVoiceGender)PlayerPrefs.GetInt("PaliSpeakVoiceGender");
    }
    public void SetPaliVoiceGender(PaliSpeakVoiceGender gender)
    {
        PlayerPrefs.SetInt("PaliSpeakVoiceGender", (int)gender);
        StarGroupArticleView.currVoiceArticle = "";
        StarGroupDictView.currVoiceWord = "";
        SpeechManager.Instance().ClearContentList();
    }
    public string GetPaliVoiceGenderName()
    {
        PaliSpeakVoiceGender gender = (PaliSpeakVoiceGender)PlayerPrefs.GetInt("PaliSpeakVoiceGender");
        if (gender == PaliSpeakVoiceGender.Male)
        {
            return "        男声";
        }
        else
        {
            return "        女声";
        }
    }
    public PaliSpeakVoiceType GetPaliVoiceType()
    {
        return (PaliSpeakVoiceType)PlayerPrefs.GetInt("PaliSpeakVoiceType");
    }
    public void SetPaliVoiceType(PaliSpeakVoiceType vt)
    {
        PlayerPrefs.SetInt("PaliSpeakVoiceType", (int)vt);
        StarGroupArticleView.currVoiceArticle = "";
        StarGroupDictView.currVoiceWord = "";
        SpeechManager.Instance().ClearContentList();
    }


    public string GetPaliVoiceTypeName()
    {
        PaliSpeakVoiceType t = (PaliSpeakVoiceType)PlayerPrefs.GetInt("PaliSpeakVoiceType");
        if (t == PaliSpeakVoiceType.Telugu)
        {
            return "        印度风格";
        }
        else if (t == PaliSpeakVoiceType.Myanmar)
        {
            return "        缅甸风格";
        }
        //else if (t == PaliSpeakVoiceType.Sinhala)
        //{
        //    return "        斯里兰卡风格";
        //}
        return "";
    }

    public PaliSpeakVoiceSpeed GetPaliVoiceSpeed()
    {
        return (PaliSpeakVoiceSpeed)PlayerPrefs.GetInt("PaliSpeakVoiceSpeed");
    }
    public void SetPaliVoiceSpeed(PaliSpeakVoiceSpeed vt)
    {
        PlayerPrefs.SetInt("PaliSpeakVoiceSpeed", (int)vt);
        StarGroupArticleView.currVoiceArticle = "";
        StarGroupDictView.currVoiceWord = "";
        SpeechManager.Instance().ClearContentList();
    }


    public string GetPaliVoiceSpeedName()
    {
        PaliSpeakVoiceSpeed t = (PaliSpeakVoiceSpeed)PlayerPrefs.GetInt("PaliSpeakVoiceSpeed");
        if (t == PaliSpeakVoiceSpeed._0)
        {
            return "        0";
        }
        else if (t == PaliSpeakVoiceSpeed._10)
        {
            return "        -10%";
        }
        else if (t == PaliSpeakVoiceSpeed._20)
        {
            return "        -20%";
        }
        else if (t == PaliSpeakVoiceSpeed._30)
        {
            return "        -30%";
        }
        else if (t == PaliSpeakVoiceSpeed._40)
        {
            return "        -40%";
        }
        return "";
    }
    #endregion
}
