//数据库管理类
using Imdork.SQLite;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBManager
{
    //懒汉式单例类.在第一次调用的时候实例化自己 
    private DBManager() { }
    private static DBManager manager = null;
    //静态工厂方法 
    public static DBManager Instance()
    {
        if (manager == null)
        {
            manager = new DBManager();
        }
        return manager;
    }

    public static string DictDBurl = "DB/Dict";
    public static string SentenceDBurl = "DB/Sentence";
    public SQLiteHelper dbHelper;

    public void Getdb(Action<DbAccess> action,string dburl)
    {
        //Path数据库文件，一定是StreamingAssets文件夹下 填写的路径文件不需要填写.db后缀
        //创建数据库读取类
        SQLiteHelper helper = new SQLiteHelper(dburl);
        //打开数据库 存储数据库操作类
        using (var db = helper.Open())
        {
            //调用数据库委托
            action(db);
        }
        /*
         因为每次使用数据 添/删/改/查 都需要使用完后Close掉 
         重复代码，写无数次太麻烦 因为数据库操作类 继承了IDisposable接口 所以，
         using会自动关闭数据库连接，我们无需手动关闭数据库
         */

    }

    void OnApplicationQuit()
    {

    }
    private void DBConnect(string url)
    {
      //  dbHelper = DbAccess(url);
    }
    private void DbAccess(string url)
    {

    }
    // Update is called once per frame
    void Update()
    {

    }
}
