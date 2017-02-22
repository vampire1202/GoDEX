using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GoDEX
{
    public partial class NodeInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string machinNo = Request.QueryString["machineNo"];
                int mNo = 0;
                if (!string.IsNullOrEmpty(machinNo))
                    mNo = Convert.ToInt32(machinNo);
                this.lblMachineNo.Text = machinNo;
                GoDexData.Model.nodeInfo modNode = new GoDexData.BLL.nodeInfo().GetModel(mNo);
                if (modNode != null)
                {
                    switch (modNode.machineType)
                    {
                        case 1:
                            lblMachineType.Text = "单管单区";
                            break;
                        case 2:
                            lblMachineType.Text = "四管单区";
                            break;
                        case 3:
                            lblMachineType.Text = "四管四区";
                            break;
                        default:
                            lblMachineType.Text = "未知";
                            break;
                    }
                    lblAddress.Text = modNode.sign;
                    lblLvwangDate.Text = modNode.lvwangdate.ToLongDateString(); 
                }

            }
        }
    }
}