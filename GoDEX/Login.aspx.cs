using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using GoDexData.Model;
namespace GoDex
{
    public partial class Login : System.Web.UI.Page
    {
 
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
            
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            User u = CheckUser(this.txtUser.Text, this.txtPassword.Text);
            if (u != null)
            {
                Session["user"] = u.UserName ;
                Session["role"] = u.Role;
                Session["pwd"] = u.Password;
                Server.Transfer("Home.aspx");
                Response.Write("<script language='javascript'>window.location='Home.aspx'</script>");
            }
            else
                Response.Write("<script languag='javascript'>alert('用户名或密码不正确')</script>");
        }

        protected void btnSet_Click(object sender, EventArgs e)
        {
            Response.Write("<script language='javascript'>window.location='DataServerSet.aspx'</script>");
        }

        public User CheckUser(string cUserName, string cPassword)
        {          
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
                    user.UserID = Convert.ToInt32(reader["userID"]);
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
            catch (Exception ee)
            {
                Response.Write("<script languag='javascript'>alert('"+ ee.Message.ToString() +"')</script>");
                return null;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}