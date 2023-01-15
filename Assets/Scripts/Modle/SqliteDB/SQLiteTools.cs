using System.Collections;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using System;

namespace Imdork.SQLite
{
    /*
     SqliteDataReader 类使用方法：
     
     bool Read()方法 查看查询出来数据 有没有可读字段，如果有返回 true 否则反之 false

     int GetOrdinal(string Name)方法  传递数据库字段名 返回对应序列号
 
     bool IsDBNull(int i)方法  接收字段名称序列号 判断该序列号是否为空 是空为true 反之为false

     object GetValue(int i)方法  接收字段名称序列号 返回对应字段数据

     FieldCount属性 获取该行字段数量

     string GetName(int i)方法 参数传递行索引 返回对应字段名称
     
     */
    /*
     SQLite类型 对应  C#数据类型   
     TINYINT          Byte   -->byte
     INT              Int32  -->int
     INTEGER          Int64  -->long
     SINGLE           Single -->float
     DECIMAL          Decimal -->decimal

     BIT              Boolean -->bool  
     BOOLEAN          Boolean -->bool
     注意：bool类型 只能存储 0-1 到数据库中 (0)false (1) true

     DOUBLE           Double -->double (首选)
     REAL             Double -->double 
        
     NVARCHAR         String -->string 
     STRING           String -->string  (首选)
     TEXT             String -->string  文本字符串，存储使用的编码方式为UTF-8、UTF-16BE、UTF-16LE
     
     TIME             DateTime
     DATETIME         DateTime （首选）
     生成时间字符串代码：
     DateTime.Now.ToString("s");
     
     */
    /*
     SQLite 数据类型	C# 数据类型	 
     BIGINT	             Int64	 
     BIGUINT	         UInt64	 
     BINARY	             Binary	 
     BIT	             Boolean	            首选
     BLOB	             Binary	                首选
     BOOL	             Boolean	 
     BOOLEAN	         Boolean	 
     CHAR	             AnsiStringFixedLength	首选
     CLOB	             String	 
     COUNTER	         Int64	 
     CURRENCY	         Decimal	 
     DATE	             DateTime	 
     DATETIME	         DateTime	            首选
     DECIMAL	         Decimal	            首选
     DOUBLE	             Double	 
     FLOAT	             Double	 
     GENERAL	         Binary	 
     GUID	             Guid	 
     IDENTITY	         Int64	 
     IMAGE	             Binary	 
     INT	             Int32	                首选
     INT8	             SByte	 
     INT16	             Int16	 
     INT32	             Int32	 
     INT64	             Int64	 
     INTEGER	         Int64	                首选
     INTEGER8	         SByte	 
     INTEGER16	         Int16	 
     INTEGER32	         Int32	 
     INTEGER64	         Int64	 
     LOGICAL	         Boolean	 
     LONG	             Int64	 
     LONGCHAR	         String	 
     LONGTEXT	         String	 
     LONGVARCHAR	     String	 
     MEMO	             String	 
     MONEY	             Decimal	 
     NCHAR	             StringFixedLength	    首选
     NOTE	             String	 
     NTEXT	             String	 
     NUMBER	             Decimal	 
     NUMERIC	         Decimal	 
     NVARCHAR	         String	                首选
     OLEOBJECT	         Binary	 
     RAW	             Binary	 
     REAL	             Double	                首选
     SINGLE	             Single	                首选
     SMALLDATE	         DateTime	 
     SMALLINT	         Int16	                首选
     SMALLUINT	         UInt16	                首选
     STRING	             String	 
     TEXT	             String	 
     TIME	             DateTime	 
     TIMESTAMP	         DateTime	 
     TINYINT	         Byte	                首选
     TINYSINT	         SByte	                首选
     UINT	             UInt32	                首选
     UINT8	             Byte	 
     UINT16	             UInt16	 
     UINT32	             UInt32	 
     UINT64	             UInt64	 
     ULONG	             UInt64	 
     UNIQUEIDENTIFIER	 Guid	                首选
     UNSIGNEDINTEGER	 UInt64	                首选
     UNSIGNEDINTEGER8	 Byte	 
     UNSIGNEDINTEGER16	 UInt16	 
     UNSIGNEDINTEGER32	 UInt32	 
     UNSIGNEDINTEGER64	 UInt64	 
     VARBINARY	         Binary	 
     VARCHAR	         AnsiString	            首选
     VARCHAR2	         AnsiString	 
     YESNO	             Boolean
     */
    /// <summary>
    /// SQLite 解析数据工具类
    /// </summary>
    public class SQLiteTools
    {
        /// <summary>
        /// 获取单行数据库 字段名称 对应数据对象 方法内会调用Read()
        /// </summary>
        /// <param name="reader">数据库读取器</param>
        /// <returns></returns>
        public static Dictionary<string, object> GetValue(SqliteDataReader reader)
        {
            if (!reader.Read())
            {
                return null;
            }
            return ReadValue(reader);
        }

        /// <summary>
        /// 获取多行数据库 字段名称 对应数据对象   方法内会调用Read()
        /// </summary>
        /// <param name="reader">数据库读取器</param>
        /// <returns></returns>
        public static Dictionary<string, object>[] GetValues(SqliteDataReader reader)
        {
            if (!reader.Read())
            {
                return null;
            }

            var Line = new List<Dictionary<string, object>>();
            do
            {
                Line.Add(ReadValue(reader));

            } while (reader.Read());

            return Line.ToArray();
        }
     
        /// <summary>
        /// 读取该行数据方法
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static Dictionary<string, object> ReadValue(SqliteDataReader reader)
        {
            //初始化字典的方法
            var Line = new Dictionary<string, object>();

            //遍历当前行中列数
            for (int i = 0; i < reader.FieldCount; i++)
            {
                //获取该字段名称
                string colName = reader.GetName(i);

                //判断该字段数据是否为null
                if(IsDBNull(reader,colName))
                {
                    //暂停循环
                    continue;
                }

                //获取数据的方法
                object value = GetValue(reader, colName);

                //存储到字典中
                Line.Add(colName, value);
            }

            //返回字典
            return Line;
        }

        /// <summary>
        /// 判断该字段是否为空  需要手动调用Read()方法
        /// </summary>
        /// <param name="reader">数据库读取器</param>
        /// <param name="colName">字段名称</param>
        /// <returns></returns>
        public static bool IsDBNull(SqliteDataReader reader, string colName)
        {
            int colOrdinal = reader.GetOrdinal(colName);
            return reader.IsDBNull(colOrdinal);
        }

        /// <summary>
        /// 获取数据的方法  需要手动调用Read()方法
        /// </summary>
        /// <param name="reader">数据库读取器</param>
        /// <param name="colName">字段名称</param>
        /// <returns></returns>
        public static object GetValue(SqliteDataReader reader, string colName)
        {
            int colOrdinal = reader.GetOrdinal(colName);
            return reader.GetValue(colOrdinal);
        }

        /// <summary>
        /// 获取数据的方法  自定义类型获取 需要手动调用Read()
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader">数据读取类</param>
        /// <param name="colName">字段名称</param>
        /// <param name="action">获取对应类型数据</param>
        /// <returns></returns>
        public static T GetValue<T>(SqliteDataReader reader,string colName,Func<int,T> action)
        {
            int colOrdinal = reader.GetOrdinal(colName);
            return action(colOrdinal);
        }

        /// <summary>
        /// 读取的方法 需要读取下一行数据时调用 （如果是第一次读取数据 还是读取第一行数据）
        /// </summary>
        /// <param name="reader">数据库读取类</param>
        /// <returns></returns>
        public static bool Read(SqliteDataReader reader)
        {
            return reader.Read();
        }
         
    }
     
}