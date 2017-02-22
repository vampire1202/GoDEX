using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GoDex
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                if (Session["user"] == null || Session["user"] == string.Empty)
                {
                    Response.Write("<script>alert('请重新登录');</script>");
                    Response.Write("<script language='javascript'>window.location='Login.aspx'</script>");
                    return;
                } 
                this.lblUserName.Text = Session["user"].ToString();
                this.lblRole.Text = Session["role"].ToString(); 
            }
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Session["user"] = null;
            Session["role"] = null;
            Response.Write("<script language='javascript'>window.location='Login.aspx'</script>");
        }
    }
}