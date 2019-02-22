using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
///******************
///数据库访问处理层
///*******************
namespace SqlLiteHelper
{
    public class DBService
    {
        /// <summary>
        /// 定义数据库连接串
        /// </summary>
        private static SQLiteConnection _objConnection;

        public static SQLiteConnection Connection
        {
            get
            {
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["sqlite"].ConnectionString;
                if (_objConnection == null)
                {
                    _objConnection = new SQLiteConnection(connectionString);
                }
                if (_objConnection == null)
                {
                    _objConnection.Open();
                }
                else if (_objConnection.State == System.Data.ConnectionState.Closed)
                {
                    _objConnection.Open();
                }
                else if (_objConnection.State == System.Data.ConnectionState.Broken)
                {
                    _objConnection.Close();
                    _objConnection.Open();
                }
                return _objConnection;
            }
        }

        /// <summary>
        /// 执行SQL语句，返回错误结果
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static string ExecuteSql(string SQLString, ref int returnValue)
        {
            using (SQLiteCommand cmd = new SQLiteCommand(SQLString, (SQLiteConnection)Connection))
            {
                try
                {
                    int rows = cmd.ExecuteNonQuery();
                    //cmd.Parameters
                    returnValue = rows;
                    return string.Empty;
                }
                catch (Exception e)
                {
                    return ErrorMessage();
                }
            }
        }


        /// <summary>
        /// 执行SQL语句，返回错误结果
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <param name="vlist" >参数列表</param>
        /// <returns>影响的记录数</returns>
        public static string ExecuteSql(string SQLString, ref int returnValue, List<SQLiteParameter> vlist)
        {
            using (SQLiteCommand cmd = new SQLiteCommand(SQLString, (SQLiteConnection)Connection))
            {
                try
                {
                    cmd.Parameters.AddRange(vlist.ToArray());
                    int rows = cmd.ExecuteNonQuery();                    
                    returnValue = rows;
                    return string.Empty;
                }
                catch (Exception e)
                {
                    return ErrorMessage();
                }
            }
        }

        /// <summary>
        /// 直线查询，不需要返回查询结果时使用
        /// </summary>
        /// <param name="SQLString"></param>
        /// <returns></returns>
        public static string ExecuteSql(string SQLString)
        {
            int nRet = 0;
            return ExecuteSql(SQLString, ref nRet);
        }

        public static string ExecuteSql(string SQLString, List<SQLiteParameter> vlist)
        {
            int nRet = 0;
            return ExecuteSql(SQLString, ref nRet, vlist);
        }

        /// <summary>
        /// 执行SQL语句，返回第一行第一列
        /// (不建议使用)
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns></returns>
        public static string ExecuteScalarSql(string SQLString, ref object returnValue)
        {
            using (SQLiteCommand cmd = new SQLiteCommand(SQLString, Connection))
            {
                try
                {
                    returnValue = cmd.ExecuteScalar();
                    return string.Empty;
                }
                catch (Exception e)
                {
                    return ErrorMessage();
                }
            }
        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public static string Query(string SQLString, string strTableName, ref DataSet ds)
        {
            try
            {
                using (SQLiteDataAdapter sda = new SQLiteDataAdapter(SQLString, Connection))
                {
                    sda.Fill(ds, strTableName);
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                return ErrorMessage();
            }
        }
        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public static string Query(string SQLString, ref DataSet ds)
        {
            try
            {
                using (SQLiteDataAdapter sda = new SQLiteDataAdapter(SQLString, Connection))
                {
                    sda.Fill(ds, "Test");
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                return ErrorMessage();
            }
        }
        /// <summary>
        /// 操作事务
        /// </summary>
        /// <param name="listSql"></param>
        /// <returns></returns>
        public static string ExecuteTran(List<string> listSql)
        {
            string strRet = string.Empty;
            using (SQLiteTransaction trans = Connection.BeginTransaction())
            {
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = Connection;
                cmd.Transaction = trans;
                try
                {
                    foreach (string value in listSql)
                    {
                        cmd.CommandText = value;
                        int vals = cmd.ExecuteNonQuery();
                    }
                    trans.Commit();
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    strRet = e.Message;
                }
            }
            return strRet;
        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public static string Query(string SQLString, SQLiteTransaction tr, ref DataSet ds)
        {
            try
            {
                SQLiteDataAdapter sda = new SQLiteDataAdapter(SQLString, Connection);
                sda.SelectCommand.CommandType = CommandType.Text;
                //下面这句话一定要写，否则就会报如标题的问题
                sda.SelectCommand.Transaction = tr;
                sda.Fill(ds);//不加红色部分的话执行到这里就有错误
                return string.Empty;
            }
            catch (Exception ex)
            {
                return ErrorMessage();
            }
        }

        /// <summary>
        /// 小事物
        /// </summary>
        /// <param name="listSql"></param>
        /// <returns></returns>
        public static string ExecuteList(List<string> listSql, SQLiteTransaction tr)
        {
            string strRet = string.Empty;
            SQLiteCommand cmd = new SQLiteCommand();
            cmd.Connection = Connection;
            cmd.Transaction = tr;
            try
            {
                foreach (string value in listSql)
                {
                    cmd.CommandText = value;
                    int vals = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                strRet = ErrorMessage();
            }

            return strRet;
        }

        /// <summary>
        /// 获取sql错误信息
        /// </summary>
        /// <returns></returns>
        private static string ErrorMessage()
        {
            string sValue = string.Empty;
            //bool bResult = SysParameters.vLanguageList.TryGetValue("msgSqlError", out sValue);
            return sValue;
        }

        /// <summary>
        /// 批量执行SQL，没有事物，执行不通过的只是记录结果，然后继续执行
        /// </summary>
        /// <param name="listSql"></param>
        /// <returns></returns>
        public static string ExecuteList(List<string> listSql)
        {
            string strRet = string.Empty;
            SQLiteCommand cmd = new SQLiteCommand();
            cmd.Connection = Connection;

            foreach (string value in listSql)
            {
                try
                {
                    cmd.CommandText = value.Replace("\r\n", " ");
                    int vals = cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    strRet = e.Message;
                }
            }


            return strRet;
        }
    }
}
