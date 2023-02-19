using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using static ArticleController;

public class CommonTool
{
    //返回翻译标题
    //todo：不同语言 标题不同
    public static string GetBookTranslateName(Book book)
    {
        if (string.IsNullOrEmpty(book.translateName))
            return book.toc;
        else
            return book.translateName;
    }


    public static string CopyAndroidPathToPersistent(string datebasePath)
    {

        string path;


        path = Application.persistentDataPath + "/" + datebasePath;

        //如果查找该文件路径
        if (File.Exists(path))
        {
            Debug.LogError("找到了！" + path);
            //返回该数据库路径
            return path;
        }
        //Debug.LogError("1111111111wwww");
        //Debug.LogError("复制数据库路径" + "jar:file://" + Application.dataPath + "!/assets/" + datebasePath);

        // jar:file:是安卓手机路径的意思  
        // Application.dataPath + "!/assets/"   即  Application.dataPath/StreamingAssets  
        var request = UnityWebRequest.Get("jar:file://" + Application.dataPath + "!/assets/" + datebasePath);
        request.SendWebRequest(); //读取数据
        while (!request.downloadHandler.isDone) { }
        // 因为安卓中streamingAssetsPath路径下的文件权限是只读，所以获取之后把他拷贝到沙盘路径中
        File.WriteAllBytes(path, request.downloadHandler.data);
        //Debug.LogError("复制完了" + path);
        //Debug.LogError("22222222222wwww");

        return path;
    }
}
