using UnityEngine;
using System;
using System.Collections;
using Mono.Data.Sqlite;

namespace Imdork.SQLite
{
    /// <summary>
    /// SQLite数据库操作类
    /// </summary>
    public class DbAccess : IDisposable
    {
        private SqliteConnection conn; // SQLite连接
        private SqliteCommand cmd; // SQLite命令
        private SqliteDataReader reader;
        public DbAccess(string connectionString)
        {
            OpenDB(connectionString);
        }
        public DbAccess() { }

        /// <summary>
        /// 打开数据库
        /// </summary>
        /// <param name="connectionString"></param>
        public void OpenDB(string connectionString)
        {
            try
            {
                conn = new SqliteConnection(connectionString);
                conn.Open();
                Debug.Log("Connected to db,连接数据库成功！");
            }
            catch (Exception e)
            {
                string temp1 = e.ToString();
                Debug.Log(temp1);
            }
        }
        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public void CloseSqlConnection()
        {
            if (cmd != null) { cmd.Dispose(); cmd = null; }
            if (reader != null) { reader.Dispose(); reader = null; }
            if (conn != null) { conn.Close(); conn = null; }
            Debug.Log("Disconnected from db.关闭数据库！");
        }

        /// <summary>
        /// 回收资源
        /// </summary>
        public void Dispose()
        {
            CloseSqlConnection();
        }

        /// <summary>
        /// 执行SQL语句 用于Update/Insert/Delete
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sqlQuery)
        {
            Debug.Log("ExecuteNonQuery:: " + sqlQuery);
            cmd = conn.CreateCommand();
            cmd.CommandText = sqlQuery;
            int rows = cmd.ExecuteNonQuery();
            return rows;
        }



        #region 更新数据

        /// <summary>
        /// 更新数据 param tableName=表名  selectkey=查找字段（主键) selectvalue=查找内容 cols=更新字段 colsvalues=更新内容
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="selectKeys"></param>
        /// <param name="selectValues"></param>
        /// <param name="cols"></param>
        /// <param name="colsValues"></param>
        /// <returns></returns>
        public int UpdateIntoSpecific(string tableName, string[] selectKeys, string[] selectValues, string[] cols, string[] colsValues)
        {
            string query = "UPDATE " + tableName + " SET " + cols[0] + " = " + "'" + colsValues[0] + "'";

            for (int i = 1; i < colsValues.Length; ++i)
            {
                query += ", " + cols[i] + " =" + "'" + colsValues[i] + "'";
            }

            query += " WHERE " + selectKeys[0] + " = " + "'" + selectValues[0] + "' ";
            for (int i = 1; i < selectKeys.Length; ++i)
            {
                query += " AND " + selectKeys[i] + " = " + "'" + selectValues[i] + "' ";
            }
            return ExecuteNonQuery(query);
        }
        /// <summary>
        /// 更新数据 param tableName=表名  selectkey=查找字段（主键) operation=判断的符号 selectvalue=查找内容 cols=更新字段 colsvalues=更新内容
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="selectKeys"></param>
        /// <param name="operation"></param>
        /// <param name="selectValues"></param>
        /// <param name="cols"></param>
        /// <param name="colsValues"></param>
        /// <returns></returns>
        public int UpdateIntoSpecific(string tableName, string[] selectKeys, string[] operation, string[] selectValues, string[] cols, string[] colsValues)
        {
            string query = "UPDATE " + tableName + " SET " + cols[0] + " = " + "'" + colsValues[0] + "'";

            for (int i = 1; i < colsValues.Length; ++i)
            {
                query += ", " + cols[i] + " =" + "'" + colsValues[i] + "'";
            }

            query += " WHERE " + selectKeys[0] + " " + operation[0] + " " + "'" + selectValues[0] + "' ";
            for (int i = 1; i < selectKeys.Length; ++i)
            {
                query += " AND " + selectKeys[i] + " " + operation[i] + " " + "'" + selectValues[i] + "' ";
            }
            return ExecuteNonQuery(query);
        }
        #endregion

        #region 插入数据	

        #region 插入部分数据
        /// <summary>
        /// 插入部分数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="cols">字段名</param>
        /// <param name="values">具体数值</param>
        /// <returns></returns>
        public int InsertIntoSpecific(string tableName, string[] cols, string[] values)
        {
            if (cols.Length != values.Length)
            {
                throw new Exception("columns.Length != colType.Length");
            }

            string query = "INSERT INTO " + tableName + " (" + cols[0];
            for (int i = 1; i < cols.Length; ++i)
            {
                query += ", " + cols[i];
            }

            query += ") VALUES (" + "'" + values[0] + "'";
            for (int i = 1; i < values.Length; ++i)
            {
                query += ", " + "'" + values[i] + "'";
            }

            query += ")";
            return ExecuteNonQuery(query);
        }

        #endregion 

        #region 插入一行数据

        /// <summary>
        /// 插入一行数据 param tableName=表名 values=插入数据内容
        /// </summary>
        public int InsertInto(string tableName, string[] values)
        {
            string query = "INSERT INTO " + tableName + " VALUES (" + string.Format("'{0}'", values[0]);
            for (int i = 1; i < values.Length; ++i)
            {
                query += ", " + string.Format("'{0}'", values[i]);
            }
            query += ")";
            return ExecuteNonQuery(query);
        }

        #endregion

        #endregion

        #region 删除表

        #region 根据条件删除表
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="cols">字段</param>
        /// <param name="colsValues">字段值</param>
        /// <returns></returns>
        public int Delete(string tableName, string[] cols, string[] colsValues)
        {
            string query = "DELETE FROM " + tableName + " WHERE " + cols[0] + " = " + "'" + colsValues[0] + "'";

            for (int i = 1; i < colsValues.Length; ++i)
            {
                query += " and " + cols[i] + " = " + "'" + colsValues[i] + "'";
            }
            return ExecuteNonQuery(query);
        }
        /// <summary>
        /// 删除表
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="cols"></param>
        /// <param name="operation"></param>
        /// <param name="colsValues"></param>
        /// <returns></returns>
        public int Delete(string tableName, string[] cols, string[] operation, string[] colsValues)
        {
            string query = "DELETE FROM " + tableName + " WHERE " + cols[0] + " " + operation[0] + " " + "'" + colsValues[0] + "'";

            for (int i = 1; i < colsValues.Length; ++i)
            {
                query += " and " + cols[i] + " " + operation[i] + " " + "'" + colsValues[i] + "'";
            }
            return ExecuteNonQuery(query);
        }
        #endregion

        /// <summary> 
        /// 删除表中全部数据
        /// </summary>
        public int DeleteContents(string tableName)
        {
            string query = "DELETE FROM " + tableName;
            return ExecuteNonQuery(query);
        }

        #endregion

        #region 创建表

        /// <summary>
        /// 创建表 param name=表名 col=字段名 colType=字段类型
        /// </summary>
        public int CreateTable(string name, string[] col, string[] colType)
        {
            if (col.Length != colType.Length)
            {
                throw new SqliteException("columns.Length != colType.Length");
            }
            string query = "CREATE TABLE " + name + " (" + col[0] + " " + colType[0];
            for (int i = 1; i < col.Length; ++i)
            {
                query += ", " + col[i] + " " + colType[i];
            }
            query += ")";
            return ExecuteNonQuery(query);
        }

        #endregion

        #region 查询表

        #region 按条件查询全部数据
        /// <summary>
        /// 按条件查询全部数据 param tableName=表名  col=查找字段 operation=运算符 values=内容
        /// </summary>
        public SqliteDataReader SelectsWhere(string tableName, string[] col, string[] operation, string[] values)
        {
            if (col.Length != operation.Length || operation.Length != values.Length)
            {
                throw new SqliteException("col.Length != operation.Length != values.Length");
            }
            string query = "SELECT *";

            query += " FROM " + tableName + " WHERE " + col[0] + operation[0] + "'" + values[0] + "' ";
            for (int i = 1; i < col.Length; ++i)
            {
                query += " AND " + col[i] + operation[i] + "'" + values[i] + "' ";
            }
            return ExecuteQuery(query);
        }
        /// <summary>
        /// 按条件查询全部数据 param tableName=表名 col=查找字段  values=内容
        /// </summary>
        public SqliteDataReader SelectsWhere(string tableName, string[] col, string[] values)
        {
            if (col.Length != values.Length)
            {
                throw new SqliteException("col.Length != values.Length");
            }
            string query = "SELECT *";
            query += " FROM " + tableName + " WHERE " + col[0] + "=" + "'" + values[0] + "' ";
            for (int i = 1; i < col.Length; ++i)
            {
                query += " AND " + col[i] + "=" + "'" + values[i] + "' ";
            }
            return ExecuteQuery(query);
        }
        #endregion

        #region 按条件查询数据
        /// <summary>
        /// 按条件查询数据 param tableName=表名 items=查询字段 col=查找字段 operation=运算符 values=内容
        /// </summary>
        public SqliteDataReader SelectWhere(string tableName, string[] items, string[] col, string[] operation, string[] values)
        {
            if (col.Length != operation.Length || operation.Length != values.Length)
            {
                throw new SqliteException("col.Length != operation.Length != values.Length");
            }
            string query = "SELECT " + items[0];
            for (int i = 1; i < items.Length; ++i)
            {
                query += ", " + items[i];
            }
            query += " FROM " + tableName + " WHERE " + col[0] + operation[0] + "'" + values[0] + "' ";
            for (int i = 1; i < col.Length; ++i)
            {
                query += " AND " + col[i] + operation[i] + "'" + values[i] + "' ";
            }
            return ExecuteQuery(query);
        }
        /// <summary>
        /// 按条件查询数据 param tableName=表名 items=查询字段 col=查找字段  values=内容
        /// </summary>
        public SqliteDataReader SelectWhere(string tableName, string[] items, string[] col, string[] values)
        {
            if (col.Length != values.Length)
            {
                throw new SqliteException("col.Length != values.Length");
            }
            string query = "SELECT " + items[0];
            for (int i = 1; i < items.Length; ++i)
            {
                query += ", " + items[i];
            }
            query += " FROM " + tableName + " WHERE " + col[0] + "=" + "'" + values[0] + "' ";
            for (int i = 1; i < col.Length; ++i)
            {
                query += " AND " + col[i] + "=" + "'" + values[i] + "' ";
            }
            return ExecuteQuery(query);
        }
        #endregion

        #region 按条件查询 单个字段数据

        /// <summary>
        /// 查询表
        /// </summary>
        public SqliteDataReader Select(string tableName, string col, string operation, string values)
        {
            string query = "SELECT * FROM " + tableName + " WHERE " + col + " " + operation + " " + string.Format("'{0}'", values);
            return ExecuteQuery(query);
        }
        /// <summary>
        /// 查询表
        /// </summary>
        public SqliteDataReader Select(string tableName, string col, string values)
        {
            string query = "SELECT * FROM " + tableName + " WHERE " + col + " = " + string.Format("'{0}'", values);
            return ExecuteQuery(query);
        }
        /// <summary>
        /// 查询所有数据
        /// </summary>
        public SqliteDataReader Select(string tableName)
        {
            string query = "SELECT * FROM " + "'" + tableName + "'";
            return ExecuteQuery(query);
        }
        #region 功能性扩展
        /// <summary>
        /// 模糊查询，查词用
        /// </summary>
        public SqliteDataReader SelectDictLike(string tableName, string inputStr, string word, int limit)
        {
            string query = "SELECT `id`,`word`,`note`,`dict_id` FROM " + "'" + tableName + "'" + " WHERE " + word + " LIKE " + "'" + inputStr + "%' limit " + limit.ToString();
            return ExecuteQuery(query);
        }
        /// <summary>
        /// 匹配查询，查词用
        /// </summary>
        public SqliteDataReader SelectDictSame(string tableName, string inputStr, string word, int limit)
        {
            string query = "SELECT `id`,`word`,`note`,`dict_id` FROM " + "'" + tableName + "'" + " WHERE " + word + " = " + "'" + inputStr + "' limit " + limit.ToString();
            return ExecuteQuery(query);
        }
        /// <summary>
        /// 匹配查询词典名
        /// </summary>
        public SqliteDataReader SelectDic(string uuid, string id = "uuid", string tableName = "dict")
        {
            string query = "SELECT `dictname` FROM " + "'" + tableName + "'" + " WHERE " + id + " = " + "'" + uuid + "'";
            return ExecuteQuery(query);
        }
        /// <summary>
        /// 匹配查询,通用
        /// </summary>
        public SqliteDataReader SelectSame(string tableName, string inputStr, string word, int limit, params string[] selects)
        {
            string select = "";
            int length = selects.Length;
            for (int i = 0; i < length; i++)
            {
                if (i > 0)
                    select += ",";
                //select += string.Format("`{0}`", selects[i]);
                select += "`";
                select += selects[i];
                select += "`";
            }
            string query = "SELECT " + select + " FROM " + "'" + tableName + "'" + " WHERE " + word + " = " + "'" + inputStr + "' limit " + limit.ToString();
            return ExecuteQuery(query);
        }
        /// <summary>
        /// 匹配查询,文章用
        /// </summary>
        public SqliteDataReader SelectArticleTag(string[] tagIDArr)
        {
            string select = string.Join("','", tagIDArr);
            select = "('" + select + "')";
            //string query = "SELECT `id` FROM " + "`" + tableName + "`" + " WHERE " + word + " IN(" + select + ");";// "' limit " + limit.ToString();
            //string query = "SELECT `id` FROM `tag` WHERE `name` IN (`sutta`,`dīghanikāya`,`sīlakkhandhavagga`)";// "' limit " + limit.ToString();
            //string query = "SELECT anchor_id,count(*) AS co FROM tag_map WHERE tag_id IN  (SELECT id FROM tag WHERE name IN " + select + ") GROUP BY anchor_id ORDER BY co DESC";
            string query = "SELECT anchor_id,count(*) AS co FROM tag_map WHERE tag_id IN  (SELECT id FROM tag WHERE name IN " + select + ") GROUP BY anchor_id ORDER BY co DESC";// ORDER BY co DESC";
            return ExecuteQuery(query);
        }
        public SqliteDataReader SelectArticle(string[] tagIDArr)
        {
            string select = string.Join("','", tagIDArr);
            select = "('" + select + "')";
            //string query = "SELECT `id` FROM " + "`" + tableName + "`" + " WHERE " + word + " IN(" + select + ");";// "' limit " + limit.ToString();
            //string query = "SELECT `id` FROM `tag` WHERE `name` IN (`sutta`,`dīghanikāya`,`sīlakkhandhavagga`)";// "' limit " + limit.ToString();
            string query = "SELECT * FROM pali_text WHERE id IN " + select+ "  ORDER BY level ASC , paragraph ASC";// "' limit " + limit.ToString();
            return ExecuteQuery(query);
        }
        public SqliteDataReader SelectChapter(int[] bookIDArr)
        {
            string select = string.Join("','", bookIDArr);
            select = "('" + select + "')";
            string query = "SELECT * FROM chapter WHERE book IN " + select+ "  ORDER BY book ASC , paragraph ASC, progress DESC";// "' limit " + limit.ToString();
            //string query = "SELECT * FROM chapter WHERE book = 9";// "' limit " + limit.ToString();
            return ExecuteQuery(query);
        }
        /// <summary>
        /// 匹配查询,通用
        /// </summary>
        public SqliteDataReader SelectIn(string tableName, string word, string[] inStr)
        {
            string select = "";
            int length = inStr.Length;
            for (int i = 0; i < length; i++)
            {
                if (i > 0)
                    select += ",";
                //select += string.Format("`{0}`", selects[i]);
                select += "`";
                select += inStr[i];
                select += "`";
            }
            //string query = "SELECT `id` FROM " + "`" + tableName + "`" + " WHERE " + word + " IN(" + select + ");";// "' limit " + limit.ToString();
            //string query = "SELECT `id` FROM `tag` WHERE `name` IN (`sutta`,`dīghanikāya`,`sīlakkhandhavagga`)";// "' limit " + limit.ToString();
            string query = "SELECT id FROM tag WHERE name = 'sutta' OR name = 'dīghanikāya' OR name = 'sīlakkhandhavagga'";// "' limit " + limit.ToString();
            return ExecuteQuery(query);
        }
        #endregion
        #endregion

        #region  升序查询/降序查询/查询表行数/查询表全部数据
        /// <summary>
        /// 升序查询
        /// </summary>
        public SqliteDataReader SelectOrderASC(string tableName, string col)
        {
            string query = "SELECT * FROM " + tableName + " ORDER BY " + col + " ASC";
            return ExecuteQuery(query);
        }
        /// <summary>
        /// 降序查询
        /// </summary>
        public SqliteDataReader SelectOrderDESC(string tableName, string col)
        {
            string query = "SELECT * FROM " + tableName + " ORDER BY " + col + " DESC";
            return ExecuteQuery(query);
        }
        /// <summary>
        /// 查询表行数
        /// </summary>
        public SqliteDataReader SelectCount(string tableName)
        {
            string query = "SELECT COUNT(*) FROM " + tableName;
            return ExecuteQuery(query);
        }
        /// <summary>
        /// 查询表中全部数据 param tableName=表名 
        /// </summary>
        public SqliteDataReader ReadFullTable(string tableName)
        {
            string query = "SELECT * FROM " + tableName;
            return ExecuteQuery(query);
        }
        #endregion

        /// <summary>
        /// 执行SQL语句 用于SelectWhere查询语句
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        public SqliteDataReader ExecuteQuery(string sqlQuery)
        {
            Debug.Log("ExecuteQuery:: " + sqlQuery);
            cmd = conn.CreateCommand();
            cmd.CommandText = sqlQuery;
            reader = cmd.ExecuteReader();
            return reader;
        }

        #endregion


    }
}