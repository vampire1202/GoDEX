using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using System.Windows;

namespace GoDexWPFB
{
    class utils
    {

        /// <summary>
        /// 获取局域网内的所有数据库服务器名称
        /// </summary>
        /// <returns>服务器名称数组</returns>
        public static string[] GetLocalSqlServerNamesWithSqlClientFactory()
        {
            DataTable dataSources = SqlClientFactory.Instance.CreateDataSourceEnumerator().GetDataSources();
            DataColumn column2 = dataSources.Columns["ServerName"];
            DataColumn column = dataSources.Columns["InstanceName"];
            DataRowCollection rows = dataSources.Rows;
            string[] array = new string[rows.Count];
            for (int i = 0; i < array.Length; i++)
            {
                string str2 = rows[i][column2] as string;
                string str = rows[i][column] as string;
                if (((str == null) || (str.Length == 0)) || ("MSSQLSERVER" == str))
                {
                    array[i] = str2;
                }
                else
                {
                    array[i] = str2 + @"\" + str;
                }
            }
            Array.Sort<string>(array);

            return array;
        }

       
        /// <summary>
        /// 获取指定IP地址的数据库所有数据库实例名。
        /// </summary>
        /// <param name="ip">指定的 IP 地址。</param>
        /// <param name="username">登录数据库的用户名。</param>
        /// <param name="password">登陆数据库的密码。</param>
        /// <returns>返回包含数据实例名的列表。</returns>
        private ArrayList GetAllDataBase(string server, string username, string password)
        {
            SqlConnection Conn = new SqlConnection(
                    String.Format("Data Source={0};Initial Catalog = master;User ID = {1};PWD = {2}", server, username, password));
            try
            {
                Conn.Open();
                ArrayList DBNameList = new ArrayList();
                DataTable DBNameTable = new DataTable();
                SqlDataAdapter Adapter = new SqlDataAdapter("select name from master..sysdatabases", Conn);
                lock (Adapter)
                {
                    Adapter.Fill(DBNameTable);
                }
                foreach (DataRow row in DBNameTable.Rows)
                {
                    DBNameList.Add(row["name"]);
                }
                return DBNameList;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
                return null;
            }
            finally
            {
                Conn.Close();
            }
        }
    }
}
