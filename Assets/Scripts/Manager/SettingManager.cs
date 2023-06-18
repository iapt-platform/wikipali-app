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

    public enum PaliSpeakVoiceGender
    {
        Male = 0,
        Female = 1,
    }
    public PaliSpeakVoiceGender GetPaliVoiceGender()
    {
        return (PaliSpeakVoiceGender)PlayerPrefs.GetInt("PaliSpeakVoiceGender");
    }
    public void SetPaliVoiceGender(PaliSpeakVoiceGender gender)
    {
        PlayerPrefs.SetInt("PaliSpeakVoiceGender", (int)gender);
    }
}
