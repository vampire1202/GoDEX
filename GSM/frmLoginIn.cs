using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Runtime.InteropServices;
using Microsoft.Win32; 
using System.Security.Cryptography;
using System.Threading;
using System.ServiceProcess;
namespace GSM
{
    public partial class frmLoginIn : Form
    { 
        private string appUserXmlPath = AppDomain.CurrentDomain.BaseDirectory + "godexApp.userxml";

        //注册模块
        private Cls.DES des;
        private string _key = string.Empty;
        private string _md5useTimes = string.Empty;
        private string _md5machineCode = string.Empty;
        private string _md5Lisence = string.Empty;
        private string _md5md5Key = string.Empty;
        private string _md5wegoPacsnkj = string.Empty;

        private string _userRole = string.Empty;
        private string _userCode = string.Empty;
        private string _userName = string.Empty;


        public frmLoginIn()
        {
            InitializeComponent(); 
        } 
        
        private void gbtnOk_Click(object sender, EventArgs e)
        {
            //解密xml文件
            Cls.XmlHelper.DecryptXml(appUserXmlPath);
            XmlDocument xd = new XmlDocument();
            xd.Load(appUserXmlPath);

            XmlNode xn = xd.SelectSingleNode("/root/user[@name='" + this.cmbUserCode.Text + "']");
            try
            {
                string pwd = Cls.DESS.Decode(xn.Attributes["password"].Value);
                if (this.txtPwd.Text == pwd)
                {
                    switch (xn.Attributes["role"].Value)
                    {
                        case "Administrator":
                            _userCode = xn.Attributes["code"].Value;
                            _userName = xn.Attributes["name"].Value;
                            _userRole = "Administrator"; 
                            break;
                        case "Agency":
                            _userCode = xn.Attributes["code"].Value;
                            _userName = xn.Attributes["name"].Value;
                            _userRole = "Agency";  
                            break;
                        case "Manager":
                            _userCode = xn.Attributes["code"].Value;
                            _userName = xn.Attributes["name"].Value;
                            _userRole = "Manager";  
                            break;
                        case "Users":
                            _userCode = xn.Attributes["code"].Value;
                            _userName = xn.Attributes["name"].Value;
                            _userRole = "Users";
                            break;
                    }
                    //Cls.Method.writeLog(this.cmbUserCode.Text + "登入[" + DateTime.Now.ToString()+"]");   
                    this.Hide();
                    frmMain fMain = new frmMain(_userRole, _userCode, _userName);
                    fMain.ShowDialog();
                }
                else
                {
                    MessageBox.Show("密码不正确,请重新输入");
                    return;
                }
                Cls.XmlHelper.EncryptXml(appUserXmlPath, "root");
            }
            catch (Exception ee)
            { MessageBox.Show(ee.ToString()); }            
        }
        ///// <summary>
        ///// 获取权限功能 
        ///// </summary>
        ///// <param name="userRole"></param>
        //private void getUserRole(string userRole)
        //{
        //    switch (userRole)
        //    {
        //        case "SystemManager":
        //            _frmMain.tsmiItem.Enabled = true;
        //            _frmMain.tsbtnSetAddress.Enabled = true;
        //            _frmMain.tsmiUsers.Enabled = true;
        //            _frmMain.contextMenuStrip1.Enabled = true;
        //            _frmMain.tsBtnReadHistory.Enabled = true;
        //            _frmMain.tsmiUserRole.Enabled = true;
        //            break;
        //        case "CommonUser":
        //            _frmMain.tsmiUserRole.Enabled = false;
        //            _frmMain.tsBtnReadHistory.Enabled = false;
        //            _frmMain.tsmiUsers.Enabled = false;
        //            _frmMain.tsbtnSetAddress.Enabled = false;
        //            _frmMain.tsmiItem.Enabled = true;
        //            _frmMain.tsbtnSetAddress.Enabled = true;
        //            _frmMain.contextMenuStrip1.Enabled = true;
        //            break;
        //        case "Observer": 
        //            _frmMain.tsmiItem.Enabled = false;  
        //            _frmMain.tsbtnSetAddress.Enabled = false;
        //            _frmMain.tsmiUsers.Enabled = false;  
        //            _frmMain.contextMenuStrip1.Enabled = false;
        //            _frmMain.tsBtnReadHistory.Enabled = false;
        //            break;
        //    }
        //}
     

        private void frmLoginIn_Load(object sender, EventArgs e)
        {
            InstallDB();
            try
            {
                if (!File.Exists(appUserXmlPath))
                {
                    Cls.Method.CreateUserXmlFile(AppDomain.CurrentDomain.BaseDirectory + "godexApp.userxml");
                    //加密用户xml文件
                    Cls.XmlHelper.EncryptXml(appUserXmlPath, "root");
                }
                this.cmbUserCode.Items.Clear();
                //解密xml文件
                Cls.XmlHelper.DecryptXml(appUserXmlPath);
                //读取用户名称
                XmlDocument xd = new XmlDocument();
                xd.Load(appUserXmlPath);
                XmlNode xn = xd.SelectSingleNode("/root");
                for (int i = 0; i < xn.ChildNodes.Count; i++)
                {
                    this.cmbUserCode.Items.Add(xn.ChildNodes[i].Attributes["name"].Value);
                }
                if (this.cmbUserCode.Items.Count > 0)
                    this.cmbUserCode.SelectedIndex = 0;
                //加密用户xml文件
                Cls.XmlHelper.EncryptXml(appUserXmlPath, "root");

                //检测注册信息
                Cls.Register reg = new GSM.Cls.Register();
                //读取注册信息
                string hidpath = AppDomain.CurrentDomain.BaseDirectory + "HardwareID.txt";
                string HID = string.Empty;
                if (!File.Exists(hidpath))
                {
                    string path = AppDomain.CurrentDomain.BaseDirectory + "HID.exe";
                    if (File.Exists(path))
                    {
                        System.Diagnostics.Process.Start(path);
                        Thread.Sleep(3000);
                        HID = File.ReadAllText(hidpath, Encoding.Default);
                    }
                    else
                    {
                        MessageBox.Show("文件缺失,请重新安装本软件系统!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Application.ExitThread();
                        Application.Exit();
                        return;
                    }
                }
                else
                {
                    HID = File.ReadAllText(hidpath, Encoding.Default);
                }
            }
            catch (Exception ee) 
            { 
                MessageBox.Show(ee.Message.ToString());
                this.Dispose();
            }   
     
        }
        void InstallDB()
        {
            string server = "MSSQL$SQLExpress";// this.Context.Parameters["server"];
            string dbname = "db_Go_Dex_CN"; //this.Context.Parameters["dbname"];
            //string user = this.Context.Parameters["user"];
            //string pwd = this.Context.Parameters["pwd"];
            string targetdir = AppDomain.CurrentDomain.BaseDirectory;
            if (!Directory.Exists(targetdir + "Data"))
                Directory.CreateDirectory(targetdir + "Data");

            if (!File.Exists(targetdir + "Data\\db_Go_Dex.MDF"))
            {
                File.Copy(targetdir + "db_Go_Dex.MDF", targetdir + "Data\\db_Go_Dex.MDF");
                File.Copy(targetdir + "db_Go_Dex_Log.LDF", targetdir + "Data\\db_Go_Dex_Log.LDF");
            }
            else
                return;
             
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
                string sql = "SELECT name FROM master.dbo.sysdatabases WHERE name = N'" + dbname + "'";//--- dbname是数据库名称
                SqlCommand com = new SqlCommand(sql, conn);
                string m = com.ExecuteNonQuery().ToString(); 
                if (m.Trim() == string.Empty || m.ToLower() != dbname)//数据库中原来没有数据库
                { 
                    SqlCommand Cmd = new SqlCommand("sp_attach_db", conn); 
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
                    Cmd.ExecuteNonQuery();
                    System.Windows.Forms.MessageBox.Show("安装数据库成功!");
                }
                else
                {
                    //System.Windows.Forms.MessageBox.Show("数据库已存在!");
                }
                conn.Close();
            }
            catch (Exception ee)
            {
                //MessageBox.Show(ee.Message.ToString());
                //System.Windows.Forms.Application.Exit();
                return;
            }
        }
        

        //注册表
        private void CreateRegKey()
        {
            RegistryKey local = Registry.LocalMachine;
            RegistryKey software = local.OpenSubKey("SOFTWARE", true);
            string[] subkeys = software.GetSubKeyNames();
            bool found = false;
            //生成md5串
            string code = "{" + _md5wegoPacsnkj + "}";
            foreach (string subkey in subkeys)
            {
                if (subkey == code)
                {
                    found = true;
                    //检测注册
                    checkRegCode();
                    break;
                }
            }
            //如果未找到则创建
            if (!found)
            {
                software.CreateSubKey(code.ToString());
                _key = des.GenerateKey().ToString();
                //创建使用次数
                RegistryKey wegoPacsnkj = software.OpenSubKey(code.ToString(), true); //该项必须已存在 
                wegoPacsnkj.SetValue(_md5useTimes, des.EncryptString("0", _key), RegistryValueKind.String);
                wegoPacsnkj.SetValue(_md5machineCode, des.EncryptString("pl4q/wNkWPs7RLPYknv0CkFjoAUJosIaFcBWCN7x9g8M9FjoAUJosIaFcBWCN7x9g8M9", _key), RegistryValueKind.String);
                wegoPacsnkj.SetValue(_md5Lisence, des.EncryptString("pl4q/wNkWPs7RLPYknv0CkFjoAUJosIaFcBWCN7x9g8M9FjoAUJosIaFcBWCN7x9g8M9", _key), RegistryValueKind.String);
                //md5key 存储使用次数的md5key
                wegoPacsnkj.SetValue(_md5md5Key, _key, RegistryValueKind.String);
                checkRegCode();
            }
        }

        //检测注册是否成功
        public void checkRegCode()
        {
            _key = Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\" + "{" + _md5wegoPacsnkj + "}", _md5md5Key, "").ToString();
            string publicKey = "<RSAKeyValue><Modulus>pl4q/wNkWPs7RLPYknv0CkFjoAUJosIaFcBWCN7x9g8M9f/l2aj6XDe8ehN6iCPb9ksvsQPS5t5lA1sDE3o/fxvZGuttN7DYva0Xv8x+0/mXflVCEOnnbkQ2iDEqFDr9EVowrTW9hSBkyNRSfQWojO+JFtNqOJfGesctc+pKo9s=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
            string regCode = des.DecryptString(Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\" + "{" + _md5wegoPacsnkj + "}", _md5Lisence, "").ToString(), _key);
            string machineCode = des.DecryptString(Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\" + "{" + _md5wegoPacsnkj + "}", _md5machineCode, "").ToString(), _key);
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(publicKey);
                RSAPKCS1SignatureDeformatter fRsapacs = new RSAPKCS1SignatureDeformatter(rsa);
                fRsapacs.SetHashAlgorithm("SHA1");
                //key是注册码
                byte[] key = Convert.FromBase64String(regCode);
                SHA1Managed sha = new SHA1Managed();
                //name为 需加密的字符串
                byte[] name = sha.ComputeHash(ASCIIEncoding.ASCII.GetBytes(machineCode));
                //判断是否成功
                if (fRsapacs.VerifySignature(name, key))
                {
                    return;
                }
                else
                {
                    //thCheckRegist();
                    MessageBox.Show("软件未注册，请联系注册");
                    new frmReg().ShowDialog(); 
                }
            }
        }

        /// <summary>
        /// 验证试用次数，若已注册，则跳过检测次数。
        /// </summary>
        private void thCheckRegist()
        {
            Int32 tLong = 0;
            string useTime = des.EncryptString("0", _key);
            //EncryptString加密
            try
            {
                MessageBox.Show("您现在使用的是试用版，该软件可以免费试用10次！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                useTime = Convert.ToString(Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\" + "{" + _md5wegoPacsnkj + "}", _md5useTimes, useTime));
                MessageBox.Show("感谢您已使用了" + des.DecryptString(useTime, _key) + "次", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\" + "{" + _md5wegoPacsnkj + "}", "useTimes", des.EncryptString("0", _key), RegistryValueKind.String);
                MessageBox.Show("欢迎您使用本软件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            useTime = Convert.ToString(Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\" + "{" + _md5wegoPacsnkj + "}", _md5useTimes, des.EncryptString("0", _key)));
            tLong = Convert.ToInt32(des.DecryptString(useTime, _key));
            if (tLong < 10)
            {
                int Times = tLong + 1;
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\" + "{" + _md5wegoPacsnkj + "}", _md5useTimes, des.EncryptString(Times.ToString(), _key));
            }
            else
            {
                MessageBox.Show("试用次数已到", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Hide();
                new frmReg().ShowDialog();
            }
        }


        private void gbtnCancel_Click(object sender, EventArgs e)
        { 
            Application.ExitThread();   
            //this.Dispose(); 
        }

        private void cmbUserCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                gbtnOk_Click(sender, new EventArgs());

            if (e.KeyChar == (char)Keys.Escape)
                gbtnCancel_Click(sender, new EventArgs());
        }

        private void frmLoginIn_FormClosing(object sender, FormClosingEventArgs e)
        {
            //this.Dispose();
            //_frmMain.frmMain_FormClosing(sender, e);
        }
    }
}
