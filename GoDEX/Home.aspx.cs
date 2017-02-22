using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;


namespace GoDex
{
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                SetNavigateStyle();
                GetInfo(); 
            }
        }
        void SetNavigateStyle()
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "当前位置:  首页";
            HyperLink hlMonitor = (HyperLink)Page.Master.FindControl("hrefMonitor");
            hlMonitor.Attributes.CssStyle.Value = null;
            HyperLink hlHome = (HyperLink)Page.Master.FindControl("hrefHome");
            hlHome.Attributes.CssStyle.Value = "background:url(../images/navbg.png) no-repeat;";
            HyperLink hlNodes = (HyperLink)Page.Master.FindControl("hrefNodes");
            hlNodes.Attributes.CssStyle.Value = null;
            HyperLink hlReport = (HyperLink)Page.Master.FindControl("hrefReport");
            hlReport.Attributes.CssStyle.Value = null;
        }

        void GetInfo()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("客户端计算机名：" + Request.UserHostName + "\r\n");
            sb.Append("客户端IP：" + Request.UserHostAddress + "\r\n");
            sb.Append("浏览器：" + Request.Browser.Browser + "\r\n");
            sb.Append("浏览器版本：" + Request.Browser.Version + "\r\n");
            sb.Append("浏览器类型：" + Request.Browser.Type + "\r\n");
            sb.Append("客户端操作系统：" + Request.Browser.Platform + "\r\n");
            sb.Append("是否支持Java：" + Request.Browser.JavaApplets + "\r\n");
            sb.Append("是否支持框架网页：" + Request.Browser.Frames + "\r\n");
            sb.Append("是否支持Cookie：" + Request.Browser.Cookies + "\r\n");
            sb.Append("客户端.NET Framework版本：" + Request.Browser.ClrVersion + "\r\n");
            sb.Append("JScript版本：" + Request.Browser.JScriptVersion + "\r\n");
            sb.Append("请求的虚拟路径：" + Request.Path + "\r\n");
            //sb.Append("title：" + Request.He );  
            for (int i = 0; i < Request.Headers.Count; i++)
            {
                sb.Append(Request.Headers.Keys[i] + ":" + Request.Headers[Request.Headers.Keys[i]] + "\r\n");
            }
            sb.Append("请求的物理路径：" + Request.PhysicalPath + "\r\n");
            sb.Append("浏览器类型和版本：" + Request.ServerVariables["HTTP_USER_AGENT"] + "\r\n");
            sb.Append("用户的IP地址：" + Request.ServerVariables["REMOTE_ADDR"] + "\r\n");
            sb.Append("请求的方法：" + Request.ServerVariables["REQUEST_METHOD"] + "\r\n");
            sb.Append("服务器的IP地址：" + Request.ServerVariables["LOCAL_ADDR"] + "\r\n");
            //this.txtInfo.Text = sb.ToString();
        }
    }
}