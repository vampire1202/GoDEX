using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Drawing;
using System.Data.SqlClient;
namespace GoDex
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ReadNodes();
                SetNavigateStyle();
                SqlDependency sqlD = new SqlDependency();
                sqlD.OnChange += sqlD_OnChange;
            }
        }

        void sqlD_OnChange(object sender, SqlNotificationEventArgs e)
        {
            //throw new NotImplementedException();
        }

        


        void ReadNodes()
        {
            GoDexData.BLL.nodeInfo bllNi = new GoDexData.BLL.nodeInfo();
            List<GoDexData.Model.nodeInfo> lstNi = bllNi.GetModelList(""); 
            foreach (GoDexData.Model.nodeInfo o in lstNi)
            {
                //查找Panel中是否有该控件 
                UControl.Node c = (UControl.Node)Page.LoadControl("UControl\\Node.ascx");
                c.ID = o.machineNo.ToString();  //定义唯一标示 
                c.lblMachineNo.Text = o.machineNo.ToString();
                c.lblOnline.Text = o.softversion;
              
                if (o.softversion == "在线")//#FF0000FF
                    c.lblOnline.ForeColor = Color.Green;//#FF008000绿色
                else
                    c.lblOnline.ForeColor = Color.Red;//#FF008000绿色 

                //火警信息
                switch (o.fireChl1)
                { 
                    //<!--一级火警Pink 
                    //二级火警LightCoral 
                    //三级火警Red 
                    //四级火警DarkRed 
                    //正常LightCyan 
                    //烟雾故障 -1 Gold
                    case -1: c.fire1.BackColor = Color.Gold; break;
                    case 0: c.fire1.BackColor = Color.LightCyan; break;
                    case 1: c.fire1.BackColor = Color.Pink; break;
                    case 2: c.fire1.BackColor = Color.LightCoral; break;
                    case 3: c.fire1.BackColor = Color.Red; break;
                    case 4: c.fire1.BackColor = Color.DarkRed; break; 
                }
                //火警信息
                switch (o.fireChl2)
                {
                    //<!--一级火警Pink 
                    //二级火警LightCoral 
                    //三级火警Red 
                    //四级火警DarkRed 
                    //正常LightCyan 
                    //烟雾故障 -1 Gold
                    case -1: c.fire2.BackColor = Color.Gold; break;
                    case 0: c.fire2.BackColor = Color.LightCyan; break;
                    case 1: c.fire2.BackColor = Color.Pink; break;
                    case 2: c.fire2.BackColor = Color.LightCoral; break;
                    case 3: c.fire2.BackColor = Color.Red; break;
                    case 4: c.fire2.BackColor = Color.DarkRed; break;
                }

                //火警信息
                switch (o.fireChl3)
                {
                    //<!--一级火警Pink 
                    //二级火警LightCoral 
                    //三级火警Red 
                    //四级火警DarkRed 
                    //正常LightCyan 
                    //烟雾故障 -1 Gold
                    case -1: c.fire3.BackColor = Color.Gold; break;
                    case 0: c.fire3.BackColor = Color.LightCyan; break;
                    case 1: c.fire3.BackColor = Color.Pink; break;
                    case 2: c.fire3.BackColor = Color.LightCoral; break;
                    case 3: c.fire3.BackColor = Color.Red; break;
                    case 4: c.fire3.BackColor = Color.DarkRed; break;
                }
                //火警信息
                switch (o.fireChl4)
                {
                    //<!--一级火警Pink 
                    //二级火警LightCoral 
                    //三级火警Red 
                    //四级火警DarkRed 
                    //正常LightCyan 
                    //烟雾故障 -1 Gold
                    case -1: c.fire4.BackColor = Color.Gold; break;
                    case 0: c.fire4.BackColor = Color.LightCyan; break;
                    case 1: c.fire4.BackColor = Color.Pink; break;
                    case 2: c.fire4.BackColor = Color.LightCoral; break;
                    case 3: c.fire4.BackColor = Color.Red; break;
                    case 4: c.fire4.BackColor = Color.DarkRed; break;
                }
                 
                //气流低 yellow #FFFFFF00
                //气流高 DarkOrange  #FFFFD700
                //正常LightCyan #FF E0 FF FF 
                //气流故障 Gold
                switch (o.airChl1)
                {                   
                    case -1: c.air1.BackColor = Color.Yellow; break;
                    case 0: c.air1.BackColor = Color.LightCyan; break;
                    case 1: c.air1.BackColor = Color.DarkOrange; break;
                    case 2: c.air1.BackColor = Color.Gold; break;
                }
                switch (o.airChl2)
                {
                    case -1: c.air2.BackColor = Color.Yellow; break;
                    case 0: c.air2.BackColor = Color.LightCyan; break;
                    case 1: c.air2.BackColor = Color.DarkOrange; break;
                    case 2: c.air2.BackColor = Color.Gold; break;
                }
                switch (o.airChl3)
                {
                    case -1: c.air3.BackColor = Color.Yellow; break;
                    case 0: c.air3.BackColor = Color.LightCyan; break;
                    case 1: c.air3.BackColor = Color.DarkOrange; break;
                    case 2: c.air3.BackColor = Color.Gold; break;
                }

                switch (o.airChl4)
                {
                    case -1: c.air4.BackColor = Color.Yellow; break;
                    case 0: c.air4.BackColor = Color.LightCyan; break;
                    case 1: c.air4.BackColor = Color.DarkOrange; break;
                    case 2: c.air4.BackColor = Color.Gold; break;
                }

                switch (o.machineType)
                {
                    case 1:
                        c.lblMachineType.Text = "设备型号:单管单区";
                        break;
                    case 2:
                        c.lblMachineType.Text = "设备型号:四管单区";
                        break;
                    case 3:
                        c.lblMachineType.Text = "设备型号:四管四区";
                        break;
                    default:
                        c.lblMachineType.Text = "设备型号:未知";
                        break;
                }
                this.palNodes.Controls.Add(c);
            }
        }

        void SetNavigateStyle()
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "当前位置:  节点监控";
            HyperLink hlMonitor = (HyperLink)Page.Master.FindControl("hrefMonitor");
            hlMonitor.Attributes.CssStyle.Value = "background:url(../images/navbg.png) no-repeat;";
            HyperLink hlHome = (HyperLink)Page.Master.FindControl("hrefHome");
            hlHome.Attributes.CssStyle.Value = null;
            HyperLink hlNodes = (HyperLink)Page.Master.FindControl("hrefNodes");
            hlNodes.Attributes.CssStyle.Value = null;
            HyperLink hlReport = (HyperLink)Page.Master.FindControl("hrefReport");
            hlReport.Attributes.CssStyle.Value = null;
        }
    }
}