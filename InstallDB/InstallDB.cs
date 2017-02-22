using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace InstallDB
{
    [RunInstaller(true)]
    public partial class InstallDB : Installer
    {
        public InstallDB()
        {
            InitializeComponent();
        }
       
        public override void Install(System.Collections.IDictionary mySavedState)
        {
            string server = "MSSQL$SQLExpress";// this.Context.Parameters["server"];
            string dbname = "db_Go_Dex_CN"; //this.Context.Parameters["dbname"];
            //string user = this.Context.Parameters["user"];
            //string pwd = this.Context.Parameters["pwd"];
            string targetdir = this.Context.Parameters["targetdir"];
            if (!Directory.Exists(targetdir + "Data"))
                Directory.CreateDirectory(targetdir + "Data");

            if (!File.Exists(targetdir + "Data\\db_Go_Dex.MDF"))
            {
                File.Copy(targetdir + "db_Go_Dex.MDF", targetdir + "Data\\db_Go_Dex.MDF");
                File.Copy(targetdir + "db_Go_Dex_Log.LDF", targetdir + "Data\\db_Go_Dex_Log.LDF");
            }else
                return;

            base.Install(mySavedState);
            try
            {//“/dbname=[CUSTOMTEXTA1] /server=[CUSTOMTEXTA2] /user=[CUSTOMTEXTA3] /pwd=[CUSTOMTEXTA4] /targetdir="[TARGETDIR]\"”。 

               
                System.ServiceProcess.ServiceController service = new System.ServiceProcess.ServiceController(server);//--服务名称

                if (service.Status == ServiceControllerStatus.Stopped)
                {
                    service.Start();
                }
                string connStr = "Data Source=" + System.Windows.Forms.SystemInformation.ComputerName + "\\SQLExpress;Integrated Security=True";//SQLSERVER 身份验证是windows方式时使用,我这里是这种方式,所以前面所输入的用户名和密码没用.
                //connStr += "password=" + pwd + ";user id=" + user + ";data COLOR: green;";//SQLSERVER 身份验证是混合安全方式时使用
                SqlConnection conn = new SqlConnection(connStr);
                conn.Open();
                conn.ChangeDatabase("master");
                string sql = "SELECT name FROM master.dbo.sysdatabases WHERE name = N'"+ dbname + "'";//--- dbname是数据库名称
                SqlCommand com = new SqlCommand(sql, conn);
                string m = com.ExecuteNonQuery().ToString();
                //System.Windows.Forms.MessageBox.Show("m:" + m);
                //System.Windows.Forms.MessageBox.Show("查询数据库后" + m);
                if (m.Trim() == string.Empty || m.ToLower() != dbname)//数据库中原来没有数据库
                {
                    //System.Windows.Forms.MessageBox.Show("建立命令对象前");
                    SqlCommand Cmd = new SqlCommand("sp_attach_db", conn);
                    //System.Windows.Forms.MessageBox.Show("建立命令对象后");
                    Cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter _dbName = new SqlParameter();
                    _dbName = Cmd.Parameters.Add("@dbname", SqlDbType.NVarChar, 20);
                    _dbName.Value = dbname;
                    SqlParameter mdfPath = new SqlParameter();
                    mdfPath = Cmd.Parameters.Add("@filename1", SqlDbType.NVarChar, 255);
                    mdfPath.Value = targetdir + "Data\\db_Go_Dex.MDF";
                    SqlParameter ldfPath = new SqlParameter();
                    ldfPath = Cmd.Parameters.Add("@filename2", SqlDbType.NVarChar, 255);
                    ldfPath.Value = targetdir + "Data\\db_Go_Dex_Log.LDF";
                    //System.Windows.Forms.MessageBox.Show("附加数据库前" + mdfPath.Value);
                    Cmd.ExecuteNonQuery();
                    System.Windows.Forms.MessageBox.Show("安装数据库成功!");
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("数据库已存在!");
                }
                conn.Close();
            }
            catch (Exception e)
            { 
                System.Windows.Forms.MessageBox.Show("数据库已存在，请继续!","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
                //System.Windows.Forms.Application.Exit();
                return;
            }
            //Console.WriteLine("The Install method of 'MyInstallerSample' has been called");
        }

        public override void Uninstall(System.Collections.IDictionary savedState)
        {
            base.Uninstall(savedState);            
        }
        
        protected override void OnBeforeUninstall(System.Collections.IDictionary savedState)
        {
            base.OnBeforeUninstall(savedState);
        }
    }
}