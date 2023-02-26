using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;

namespace Imdork.SQLite
{
    /// <summary>
    /// SQLite访问类
    /// </summary>
    public class SQLiteHelper
    {
        /// <summary>
        /// 填写StreamingAssets中数据库路径文件
        /// </summary>
        /// <param name="path">数据库路径</param>
        public SQLiteHelper(string path)
        {
            //调用更新路径方法
            UpdatePath(path);
        }
        /// <summary>
        /// 更新路径的方法
        /// </summary>
        /// <param name="path"></param>
        public void UpdatePath(string path)
        {
            //给路径添加.db后缀 并赋值给数据库路径
            datebasePath = string.Format("{0}.db", path);
            //获取路径名称 加后缀db 并赋值给数据库名称
            datebaseName = Path.GetFileName(datebasePath);
        }

        /// <summary>
        /// 数据库名称
        /// </summary>
        string datebaseName;

        /// <summary>
        /// 数据库路径
        /// </summary>
        string datebasePath;

        /// <summary>
        /// 打开数据库
        /// </summary>
        public DbAccess Open()
        {
            string url = "URI = file:" + appDBPath();
            var db = new DbAccess();
            db.OpenDB(url);
            return db;
        }


        /// <summary>
        /// 根据平台读取对应StreamingAssets路径
        /// </summary>
        /// <returns></returns>
        private string appDBPath()
        {

            string path;

#if UNITY_EDITOR
            path = Application.streamingAssetsPath + "/" + datebasePath;
#elif UNITY_ANDROID
            Debug.LogError("找数据库路径");

            // 沙盘路径
            path = CommonTool.CopyAndroidPathToPersistent(datebasePath);
#else
       path = Application.streamingAssetsPath + "/" + datebasePath;
#endif
            return path;
        }

        /// <summary>
        /// 删除数据库 整个db文件删除掉
        /// </summary>
        public void Delete()
        {
            //删除文件方法
            File.Delete(appDBPath());
        }

        /// <summary>
        /// 查询db文件是否存在
        /// </summary>
        /// <returns></returns>
        public bool Find()
        {
            //查询文件的方法
            return File.Exists(appDBPath());
        }

    }
}