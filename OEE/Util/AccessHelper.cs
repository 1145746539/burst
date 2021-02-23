using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace OEE.Util
{
    class AccessHelper
    {
        private static AccessHelper accHelper = null;
        private AccessHelper() { }
        private static string connStr = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0}\\Database\\AccessDatabase.accdb;Persist Security Info=False;Jet OLEDB:Database Password=huahua.2020",
            Application.StartupPath);

        public static AccessHelper Instance()
        {
            if (accHelper == null)
            {
                accHelper = new AccessHelper();
            }
            return accHelper;
        }

        /// <summary>
        /// 执行增加、删除、修改指令
        /// </summary>
        /// <param name="sql">增加、删除、修改的sql语句</param>
        /// <param name="param">sql语句的参数</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql, params OleDbParameter[] param)
        {
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connStr))
                {
                    using (OleDbCommand cmd = new OleDbCommand(sql, conn))
                    {
                        if (param != null)
                        {
                            cmd.Parameters.AddRange(param);
                        }
                        conn.Open();
                        return (cmd.ExecuteNonQuery());
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
            
        }

        /// <summary>
        /// 执行增加、删除、修改指令
        /// </summary>
        /// <param name="sql">增加、删除、修改的sql语句</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql)
        {
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connStr))
                {
                    using (OleDbCommand cmd = new OleDbCommand(sql, conn))
                    {
                        conn.Open();
                        return (cmd.ExecuteNonQuery());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }

        }

        /// <summary>
        /// 执行查询指令，获取返回datatable
        /// </summary>
        /// <param name="sql">查询sql语句</param>
        /// <param name="param">sql语句的参数</param>
        /// <returns></returns>
        public DataTable ExecuteDatable(string sql, params OleDbParameter[] param)
        {
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connStr))
                {
                    using (OleDbCommand cmd = new OleDbCommand(sql, conn))
                    {
                        if (param != null)
                        {
                            cmd.Parameters.AddRange(param);
                        }
                        DataTable dt = new DataTable();
                        OleDbDataAdapter sda = new OleDbDataAdapter(cmd);
                        sda.Fill(dt);
                        return (dt);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }


        /// <summary>
        /// 执行查询指令，获取返回datatable
        /// </summary>
        /// <param name="sql">查询sql语句</param>
        /// <param name="param">sql语句的参数</param>
        /// <returns></returns>
        public DataTable ExecuteDatable(string sql)
        {
            try
            {

                using (OleDbConnection conn = new OleDbConnection(connStr))
                {
                    using (OleDbCommand cmd = new OleDbCommand(sql, conn))
                    {
                        DataTable dt = new DataTable();
                        OleDbDataAdapter sda = new OleDbDataAdapter(cmd);
                        sda.Fill(dt);
                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }

        /// <summary>
        /// 将DataTable转为实体对象
        /// </summary>
        /// <typeparam name="T"> 目标类型 </typeparam>
        /// <param name="sourceDT">原DT</param>
        /// <returns>转换后的实体列表</returns>
        public List<T> GetEntityFromDataTable<T>(DataTable sourceDT) where T : class  //数据库列名与属性保持一致
        {
            List<T> list = new List<T>();            // 获取需要转换的目标类型     
            Type type = typeof(T);
            try
            {
                foreach (DataRow dRow in sourceDT.Rows)
                {
                    // 实体化目标类型对象                
                    object obj = Activator.CreateInstance(type);
                    foreach (var prop in type.GetProperties())
                    {
                        if (prop.Name != "sTime")
                        {
                            // 给目标类型对象的各个属性值赋值                    
                            prop.SetValue(obj, dRow[prop.Name], null);
                        }

                    }
                    list.Add(obj as T);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return list;
        }


    }

}
