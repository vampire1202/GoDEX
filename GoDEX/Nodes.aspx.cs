using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GoDex
{
    public partial class ManageNodes : System.Web.UI.Page
    {
        public static string nodesList = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetNavigateStyle();
                //tvNodes.Nodes.Clear();
                //tvNodes.Nodes.Add(new TreeNode("主机节点"));
                ReadNodes();
            }
        }

        void ReadNodes()
        {
            GoDexData.BLL.nodeInfo bllNi = new GoDexData.BLL.nodeInfo();
            List<GoDexData.Model.nodeInfo> lstNi = bllNi.GetModelList("");
            nodesList = string.Empty;
            foreach (GoDexData.Model.nodeInfo o in lstNi)
            {
                nodesList += @"<li><cite></cite><a href='NodeInfo.aspx?machineNo=" + o.machineNo.ToString() + "' target='nodeiframe'>"+ o.machineNo.ToString() 
                    +"</a><i></i></li>"; 
            }
        }

        void SetNavigateStyle()
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "当前位置:  节点信息";
            HyperLink hlMonitor = (HyperLink)Page.Master.FindControl("hrefMonitor");
            hlMonitor.Attributes.CssStyle.Value = null;
            HyperLink hlHome = (HyperLink)Page.Master.FindControl("hrefHome");
            hlHome.Attributes.CssStyle.Value = null;
            HyperLink hlNodes = (HyperLink)Page.Master.FindControl("hrefNodes");
            hlNodes.Attributes.CssStyle.Value = "background:url(../images/navbg.png) no-repeat;";
            HyperLink hlReport = (HyperLink)Page.Master.FindControl("hrefReport");
            hlReport.Attributes.CssStyle.Value = null;
        }
    }
}