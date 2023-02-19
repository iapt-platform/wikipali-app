using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using System.IO;
using System;

public class Test2 : MonoBehaviour
{
    //文本中每行的内容
    ArrayList infoall;
    //皮肤资源，这里用于显示中文
    public GUISkin skin;
    void Start()
    {

        //删除文件
        DeleteFile(Application.persistentDataPath, "FileName.txt");

        //创建文件，共写入3次数据
        CreateFile(Application.persistentDataPath, "FileName.txt", "???????????????????????????????????????????????????");
       // CreateFile(Application.persistentDataPath, "FileName.txt", "???????????????????????????????????????????????????");
        //CreateFile(Application.persistentDataPath, "FileName.txt", "???????????????????????????????????????????????????");
        //得到文本中每一行的内容
        infoall = LoadFile(Application.persistentDataPath, "FileName.txt");

    }

    /**
    * path：文件创建目录
    * name：文件的名称
    *  info：写入的内容
    */
    void CreateFile(string path, string name, string info)
    {
        //文件流信息
        StreamWriter sw;
        FileInfo t = new FileInfo(path + "//" + name);
        if (!t.Exists)
        {
            //如果此文件不存在则创建
            sw = t.CreateText();
        }
        else
        {
            //如果此文件存在则打开
            sw = t.AppendText();
        }
        //以行的形式写入信息
        sw.WriteLine(info);
        //关闭流
        sw.Close();
        //销毁流
        sw.Dispose();
    }

    /**
     * path：读取文件的路径
     * name：读取文件的名称
     */
    ArrayList LoadFile(string path, string name)
    {
        //使用流的形式读取
        StreamReader sr = null;
        try
        {
            sr = File.OpenText(path + "//" + name);
        }
        catch (Exception e)
        {
            //路径与名称未找到文件则直接返回空
            return null;
        }
        string line;
        ArrayList arrlist = new ArrayList();
        while ((line = sr.ReadLine()) != null)
        {
            //一行一行的读取
            //将每一行的内容存入数组链表容器中
            arrlist.Add(line);
        }
        //关闭流
        sr.Close();
        //销毁流
        sr.Dispose();
        //将数组链表容器返回
        return arrlist;
    }

    /**
     * path：删除文件的路径
     * name：删除文件的名称
     */

    void DeleteFile(string path, string name)
    {
        File.Delete(path + "//" + name);

    }
    public DBManager dbManager = DBManager.Instance();

    void OnGUI()
    {
        GUIStyle myStyle = new GUIStyle();
        myStyle.fontSize = 30;
        //定义一个GUIStyle的对象

        //用新的皮肤资源，显示中文
        GUI.skin = skin;
        //读取文件中的所有内容
        foreach (string str in infoall)
        {
            //绘制在屏幕当中，哇咔咔！
            GUILayout.Label(str, myStyle);
            GUILayout.Label(Application.persistentDataPath, myStyle);
            GUILayout.Label(Application.streamingAssetsPath, myStyle);
            GUILayout.Label(Application.dataPath, myStyle);
        }
        if (GUI.Button(new Rect(Screen.width / 2 - 210, 640, 140, 80), "文件名persistentDataPath"))
        {
            GetDirectoryFile(Application.persistentDataPath);
        }
        if (GUI.Button(new Rect(Screen.width / 2 + 70, 640, 140, 80), "文件名streamingAssetsPath"))
        {
            GetDirectoryFile(Application.streamingAssetsPath);
        }
        if (GUI.Button(new Rect(Screen.width / 2 - 210, 840, 140, 80), "文件名dataPath"))
        {
            GetDirectoryFile(Application.dataPath);
        }
        if (GUI.Button(new Rect(Screen.width / 2 + 70, 840, 140, 80), "测试数据库"))
        {
            dbManager.Getdb(db =>
            {

            }, DBManager.DictDBurl);
        }
        if (GUI.Button(new Rect(Screen.width / 2 - 210, 1040, 140, 80), "文件名persistent拷贝后位置"))
        {
            GetDirectoryFile(Application.persistentDataPath+"/DB");
        }
    }
    /// <summary>
    /// 获得文件下的所有文件名
    /// </summary>
    /// <param name="path">路径</param>

    private void GetDirectoryFile(string path)
    {
        if (Directory.Exists(path))
        {
            DirectoryInfo direction = new DirectoryInfo(path);
            FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Name.EndsWith(".meta"))
                {
                    continue;
                }
                //Debug.Log("Name : " + files[i].Name);//文件名
                Debug.LogError("FullName : " + files[i].FullName);//根目录下的文件的目录
                //Debug.Log("DirectoryName : " + files[i].DirectoryName);//根目录

            }
        }
    }
}