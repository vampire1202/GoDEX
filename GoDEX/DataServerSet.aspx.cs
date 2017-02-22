using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace GoDex
{
    public partial class DataServerSet : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string[] arr = utils.SqlLocator.GetServers();
                if (arr != null)
                {
                    foreach (string s in utils.SqlLocator.GetServers())
                    {
                        this.cmbServer.Items.Add(s);
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string server = this.cmbServer.Text;
            string username = this.txtUser.Text;
            string password = this.txtPassword.Text;
            string connstr = String.Format("Data Source={0}\\SQLExpress;Initial Catalog=db_Go_Dex_CN;User ID={1};Password={2};Integrated Security=false;", server, username, password);
            SqlConnection Conn = new SqlConnection(connstr);
            try
            {
                Conn.Open();
                //RWconfig.SetAppSettings("ConnectionString", connstr);
                System.Configuration.ConfigurationManager.AppSettings.Set("ConnectionString", connstr);
                Response.Write("<script languag='javascript'>alert('测试成功,请返回登录!')</script>");
            }
            catch (Exception ee)
            {
                Response.Write("<script languag='javascript'>alert('" + ee.Message.ToString() + "')</script>");
                return;
            }
            finally
            {
                Conn.Close();
            }
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Response.Write("<script language='javascript'>window.location='Login.aspx'</script>");
        }
    }
}