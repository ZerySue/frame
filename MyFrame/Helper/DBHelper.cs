using MyFrame.LogManager;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MyFrame.Helper
{
    public class DBHelper
    {
        private volatile static DBHelper _instance = null;
        private static readonly object _lockHelper = new object();
        public static string connectionString = ConfigurationManager.AppSettings["ConnectionString"];
        Log log = null;
        private DBHelper()
        {
            log = Log.CreateInstance();
        }
        /// <summary>
        /// 单例模式实现数据库访问
        /// </summary>
        /// <returns></returns>
        public static DBHelper CreateInstance()
        {
            if (_instance == null)
            {
                lock (_lockHelper)
                {
                    if (_instance == null)
                    {
                        _instance = new DBHelper();
                    }
                }
            }
            return _instance;
        }
        /// <summary>
        /// 执行简单sql
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int ExecuteSql(string sql)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    try
                    {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch(SqlException ex)
                    {
                        connection.Close();
                     
                        log.WriteLog(ex.Message.ToString());
                        return 0;
                    }
                }
            }
        }
        /// <summary>
        /// 执行事务
        /// </summary>
        /// <param name="SQLStringList"></param>
        /// <returns></returns>
        public  int ExecuteSql(List<String> SQLStringList)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                SqlTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    int count = 0;
                    for (int n = 0; n < SQLStringList.Count; n++)
                    {
                        string strsql = SQLStringList[n];
                        if (strsql.Trim().Length > 1)
                        {
                            cmd.CommandText = strsql;
                            count += cmd.ExecuteNonQuery();
                        }
                    }
                    tx.Commit();
                    return count;
                }
                catch (Exception ex)
                {
                    tx.Rollback();
                    log.WriteLog(ex.Message.ToString());
                    return 0;
                }
            }
        }
        /// <summary>
        /// 获取datatable数据  带参数 防止sql注入
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string sql, params SqlParameter[] cmdParms)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                PrepareCommand(cmd, con, null, sql, cmdParms);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        log.WriteLog(ex.Message.ToString());
                    }
                    if (ds != null)
                    {
                        return ds.Tables[0];
                    } 
                } 
            }
            return null;
        }
        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public  int ExecuteSql(string SQLString, params SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        int rows = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        return rows;
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        throw e;
                    }
                }
            }
        }
         /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。  插入数据 可以返回最新数据 id等信息
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public object GetSingle(string SQLString, params SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        object obj = cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        log.WriteLog(e.Message);
                        throw e;
                    }
                }
            }
        }
        private void PrepareCommand(SqlCommand cmd,SqlConnection conn,SqlTransaction trans,string cmdText,SqlParameter[] cmdParms)
        {
            if(conn.State!=ConnectionState.Open)
            {
                conn.Open();
            }
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if(trans!=null)
            {
                cmd.Transaction = trans;
            }
            cmd.CommandType = CommandType.Text;
            if(cmdParms!=null)
            {
                foreach(SqlParameter parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
        }

    }
}