using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Configuration;

namespace GoDexWPFB
{
    /// <summary>
    /// DataSet.xaml 的交互逻辑
    /// </summary>
    public partial class SetServer : UserControl
    {
        public SetServer()
        {
            InitializeComponent();
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            Login l = new Login();
            this.Content = l;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Progress<double> d = new Progress<double>();
            d.ProgressChanged += d_ProgressChanged;
            Task t = ReadServers(d);
        }

        void d_ProgressChanged(object sender, double e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
                {
                    this.pBar.Value = e;
                }));
            //throw new NotImplementedException();
        }

        async Task ReadServers(IProgress<double> t)
        {
            cmbSqlServer.IsEnabled = false;
            btnSave.IsEnabled = false;
            btnReturn.IsEnabled = false;
            txtTip.Text = "正在获取数据库服务器列表......";
            await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);  
            t.Report(30);
            string[] servers = utils.GetLocalSqlServerNamesWithSqlClientFactory();
            t.Report(80);
            Dispatcher.BeginInvoke(new Action(() =>
            { 
                foreach (string s in servers)
                {
                    cmbSqlServer.Items.Add(s);
                }               
                cmbSqlServer.IsEnabled = true;
                btnSave.IsEnabled = true;
                btnReturn.IsEnabled = true;
                txtTip.Text = "请选择数据库服务器:";
            }));  
            t.Report(100);
        }


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string server = this.cmbSqlServer.Text;
            string username = this.saUser.Text;
            string password = this.txtPassword.Password;
            //Data Source=(local)\SQLExpress;Initial Catalog=db_Go_Dex_CN;User Id=sa;Password=Password1;Integrated Security=false;
            string connstr = String.Format("Data Source={0}\\SQLExpress;Initial Catalog=db_Go_Dex_CN;User ID={1};Password={2};Integrated Security=false;", server, username, password);
            SqlConnection Conn = new SqlConnection(connstr);
            try
            {
                Conn.Open();
                System.Configuration.ConfigurationManager.AppSettings.Set("ConnectionString", connstr); 
                //RWconfig.SetAppSettings("ConnectionString", connstr);
                MessageBox.Show("测试成功,请返回登录!");
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message.ToString() + "\r\n测试不通过,不能保存,请确认以下信息：\r\n1.确认 SQLBrower 服务已经运行;\r\n2.确认 SQLServer 开启了Tcp/Ip协议;\r\n3.确认数据库已经附加到服务器;");
                return;
            }
            finally
            {
                Conn.Close();
            }
        }
    }
}
