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
using System.Data;
using System.Data.SqlClient;
using GoDexData.Model;
using Maticsoft.DBUtility;

namespace GoDexWPFB
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : UserControl
    {
        public Login()
        {
            InitializeComponent();
        }
       

        public List< User> QueryUser()
        {
            //连接数据库
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString());
            try
            {
                conn.Open();
                string strSql = "SELECT * FROM tb_user";
                List<GoDexData.Model.User> lstUser = new List<User>();
                DataSet ds = new DataSet();
                SqlDataAdapter s = new SqlDataAdapter(strSql, conn);
                s.Fill(ds);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    User u = new User(dr["userName"].ToString(), dr["password"].ToString());
                    u.Role = dr["role"].ToString();
                    u.UserID = Convert.ToInt32(dr["userID"].ToString());
                    lstUser.Add(u);
                }
                return lstUser;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        public User CheckUser(string cUserName, string cPassword)
        {
            //连接数据库
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString());
            SqlCommand cmd = new SqlCommand("select * from tb_user where userName='" + cUserName + "'", conn);
            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    User user = new User((string)reader["userName"],
                        (string)reader["password"]);
                    user.UserID =Convert.ToInt32(reader["userID"]);
                    user.UserName = (string)reader["userName"];
                    user.Role = (string)reader["role"];
                    user.Password = (string)reader["password"];

                    if (user.Password == cPassword)
                    {
                        return user;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
                catch(Exception ee)
            {
                MessageBox.Show(ee.Message.ToString());
                return null; 
            }
            finally
            {
                conn.Close();
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            //List<GoDexData.Model.User> lstUser = QueryUser(); 
            User u = CheckUser(this.txtUserName.Text, this.txtPassword.Password);
            if (u != null)
            {
                MainPage mp = new MainPage();
                mp.txtUserName.Text = u.UserName;
                mp.txtRole.Text = u.Role;
                this.Content = mp;
            }
            else
                MessageBox.Show("用户名或密码不正确!");
        }

        private void ShowDataSet_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(Maticsoft.DBUtility.PubConstant.ConnectionString);
            SetServer ss = new SetServer();
            this.Content = ss;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        } 
    }
}
