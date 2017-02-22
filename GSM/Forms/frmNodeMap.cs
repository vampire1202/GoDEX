using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using System.IO;

namespace GSM.Forms
{
    public partial class frmNodeMap :  WeifenLuo.WinFormsUI.Docking.DockContent
    {
        private string _userRole = string.Empty;
        private string _nodeConfigPath = string.Empty;
        private string _zoneMapConfigPath = string.Empty;
        public frmNodeMap(string userRole)
        {
            _userRole = userRole;
            InitializeComponent();
        }

        public static string[] _devicetype = {  "500型", "2000型", "3000型","未设置型号" };
        /// <summary>
        /// NodeID节点ID
        /// </summary>
        private string _nodeID = string.Empty;
        public string _NodeID
        {
            get { return _nodeID; }
            set { this._nodeID = value; }
        }
        ///// <summary>
        ///// node配置文件
        ///// </summary>
        //private string _nodeConfigPath;
        //public string _NodeConfigPath
        //{
        //    get { return _nodeConfigPath; }
        //    set { this._nodeConfigPath = value; }
        //}
        /// <summary>
        /// node区域地图配置文件
        /// </summary>
        //private string _zoneMapConfigPath;
        //public string _ZoneMapConfigPath
        //{
        //    get { return _zoneMapConfigPath; }
        //    set { this._zoneMapConfigPath = value; }
        //}
        //打开区域图片对话框
        OpenFileDialog ofd;
        private void frmNodeMap_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < _devicetype.Length; i++)
            {
                this.cmbNodeType.Items.Add(_devicetype[i].ToString());
            }
            this.cmbNodeZoneID.Items.Add("-");
            for (int i = 1; i <= 100; i++)
            {
                this.cmbZoneID.Items.Add(i.ToString());
                this.cmbNodeZoneID.Items.Add(i.ToString());
            }
            this.cmbZoneID.SelectedIndex = 0;          
            //生成节点树形列表
            getNodeTreeView();
            //读取区域地图列表
            getZoneMapSet();
            //picZoneMap.Controls.Add(new UserControls.NodeSite());
        }

        private void addDetector_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 生成区域地图列表
        /// </summary>
        /// <param name="zoneMapConfigPath">区域地图配置文件路径</param>
        private void getZoneMapSet()
        {
            //this.cmbNodeZoneID.Items.Clear();
            //this.cmbNodeZoneID.Items.Add("-");
            //XmlDocument xd = new XmlDocument();
            //xd.Load(zoneMapConfigPath);
            //XmlNodeList xnl = xd.SelectNodes("/Map/zoneMap");
            //            foreach (XmlNode xn in xnl)
            //            {
            //                this.cmbNodeZoneID.Items.Add(xn.Attributes["ID"].Value);
            //            }
        }
        /// <summary>
        /// 生成节点树形列表
        /// </summary>
        /// <param name="nodeConfigPath">节点配置文件路径</param>

        private void getNodeTreeView()
        {
            this.treeViewNodes.Nodes.Clear();
            for (int i = 0; i < _devicetype.Length; i++)
            {
                TreeNode tnParent = new TreeNode();
                tnParent.Text = _devicetype[i].ToString();
                //tnParent.Name = _devicetype[i].ToString();
                this.treeViewNodes.Nodes.Add(tnParent);
            }

            GoDexData.BLL.nodeInfo bllNi = new GoDexData.BLL.nodeInfo();
            DataSet dsNi = bllNi.GetAllList(); 
            for (int j = 0; j < dsNi.Tables[0].Rows.Count; j++)
            {
                TreeNode tn = new TreeNode();
                tn.Text = dsNi.Tables[0].Rows[j]["machineNo"].ToString();//xnl.Item(j).Attributes["ID"].Value;
                tn.Name = dsNi.Tables[0].Rows[j]["machineNo"].ToString();
                foreach (TreeNode tnchild in this.treeViewNodes.Nodes)
                {
                    if (tnchild.Text == dsNi.Tables[0].Rows[j]["machineModel"].ToString())
                    {
                        tnchild.Nodes.Add(tn);
                    }
                }

            }
            dsNi.Dispose();
            this.treeViewNodes.ExpandAll();
        }

        private void treeViewNodes_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //MessageBox.Show(e.Node.Text);
            //try
            //{
                //XmlDocument xd = new XmlDocument();
                //xd.Load(this._nodeConfigPath);
                //XmlNode xn = xd.SelectSingleNode("/nodeGroup/node[@ID='" + e.Node.Text + "']");
                GoDexData.BLL.nodeInfo bllNi = new GoDexData.BLL.nodeInfo();
                if (!string.IsNullOrEmpty(e.Node.Name))
                {
                    GoDexData.Model.nodeInfo modelNi = bllNi.GetModel(int.Parse(e.Node.Name));
                    if (modelNi != null)
                    {
                        switch (modelNi.machineType)
                        {
                            case 1:
                                this.lblNode.Text = "单管单区";
                                break;
                            case 2:
                                this.lblNode.Text = "四管单区";
                                break;
                            case 3:
                                this.lblNode.Text = "四管四区";
                                break;
                        }
                        this.lblNodeID.Text = e.Node.Text;
                        this.txtNodePos.Text = modelNi.sign;//位置
                        this.cmbNodeType.Text = e.Node.Parent.Text; //machineModel
                        this.cmbNodeZoneID.Text = modelNi.areaMapPath;//区域地图编号
                    }
                    else
                    {
                        this.lblNode.Text = "";
                        this.lblNodeID.Text = "";
                        this.txtNodePos.Text = "";
                        this.cmbNodeType.Text = "未设置型号";
                        this.cmbNodeZoneID.Text = "-";
                    }
                }
            //}
            //catch (Exception ee) { MessageBox.Show(ee.ToString()); }
        }

        void BtnZoneMapFileClick(object sender, EventArgs e)
        {
            ofd = new OpenFileDialog();
            ofd.Filter = "图片文件（*.jpg;*.bmp;*.png;*.gif）|*.jpg;*.bmp;*.png;*.gif";
            ofd.ShowDialog();
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "zoneMap"))
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "zoneMap");
            //--------------打开图片文件
            MemoryStream ms = null;
            try
            {
                if (ofd.FileName != null && File.Exists(ofd.FileName))
                {
                    if (this.picZoneMap.Image != null)
                        this.picZoneMap.Image = null;
                    //读取Image流
                    ms = new MemoryStream();
                    Image fPic = Image.FromFile(ofd.FileName);
                    fPic.Save(ms, ImageFormat.Jpeg);
                    Image sPic = Image.FromStream(ms);
                    fPic.Dispose();

                    this.picZoneMap.Image = sPic;
                    this.picZoneMap.Width = sPic.Width;
                    this.picZoneMap.Height = sPic.Height;
                    ms.Dispose();

                    //XmlDocument xd = new XmlDocument();
                    //xd.Load(this._zoneMapConfigPath);
                    //XmlNode xn = xd.SelectSingleNode("/Map/zoneMap[@zoneID='" + this.cmbZoneID.Text + "']");
                    GoDexData.BLL.areaMap bllam = new GoDexData.BLL.areaMap();
                    GoDexData.Model.areaMap modelam = bllam.GetModel(this.cmbZoneID.SelectedIndex+1);
                    if (modelam != null)
                    {
                        File.Copy(ofd.FileName, AppDomain.CurrentDomain.BaseDirectory + "zoneMap\\zone_" + cmbZoneID.Text + Path.GetExtension(ofd.FileName), true);
                        modelam.areaMapPath = AppDomain.CurrentDomain.BaseDirectory + "zoneMap\\zone_" + cmbZoneID.Text + Path.GetExtension(ofd.FileName);
                        // xn.Attributes["imgPath"].Value = AppDomain.CurrentDomain.BaseDirectory + "zoneMap\\zone_" + cmbZoneID.Text + Path.GetExtension(ofd.FileName);
                        bllam.Update(modelam);
                    }
                    //xd.Save(this._zoneMapConfigPath);
                    else
                    {
                        GoDexData.Model.areaMap modelam1 = new GoDexData.Model.areaMap();
                        File.Copy(ofd.FileName, AppDomain.CurrentDomain.BaseDirectory + "zoneMap\\zone_" + cmbZoneID.Text + Path.GetExtension(ofd.FileName), true);
                        modelam1.areaMapPath = AppDomain.CurrentDomain.BaseDirectory + "zoneMap\\zone_" + cmbZoneID.Text + Path.GetExtension(ofd.FileName);
                        modelam1.areaMapNo = this.cmbZoneID.SelectedIndex + 1;
                        bllam.Add(modelam1);
                    } 
                    ofd.Dispose();
                }
            }
            catch (Exception ee) { MessageBox.Show(ee.ToString()); }
        }

        void CmbZoneIDSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.picZoneMap.Image != null)
                    this.picZoneMap.Image = null;

                //XmlDocument xd = new XmlDocument();
                //xd.Load(this._zoneMapConfigPath);
                //XmlNode xn = xd.SelectSingleNode("/Map/zoneMap[@zoneID='" + this.cmbZoneID.Text + "']");
                GoDexData.BLL.areaMap bllam = new GoDexData.BLL.areaMap();
                GoDexData.Model.areaMap modelam = bllam.GetModel(this.cmbZoneID.SelectedIndex + 1);
                if (modelam != null)
                {
                    MemoryStream ms = null;
                    if (File.Exists(modelam.areaMapPath))
                    {
                        ms = new MemoryStream();
                        Image fImg = Image.FromFile(modelam.areaMapPath);
                        fImg.Save(ms, ImageFormat.Jpeg);
                        fImg.Dispose();
                        this.picZoneMap.Image = Image.FromStream(ms);
                        //        			Bitmap bt = (Bitmap)Image.FromFile(xn.Attributes["imgPath"].Value);
                        //        	        this.picZoneMap.Image = bt;
                        this.picZoneMap.Width = Image.FromStream(ms).Width;
                        this.picZoneMap.Height = Image.FromStream(ms).Height;
                        ms.Dispose();
                        //bt.Dispose();
                    } 
                }
               
            }
            catch (Exception ee) { MessageBox.Show(ee.ToString()); }
        }

        void TreeViewNodesAfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void btnAllMapFile_Click(object sender, EventArgs e)
        {
            ofd = new OpenFileDialog();
            ofd.Filter = "图片文件（*.jpg;*.bmp;*.png;*.gif）|*.jpg;*.bmp;*.png;*.gif";
            ofd.ShowDialog();
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "zoneMap"))
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "zoneMap");
            //--------------打开图片文件
            MemoryStream ms = null;
            try
            {
                if (ofd.FileName != null && File.Exists(ofd.FileName))
                {
                    if (this.picZoneMap.Image != null)
                        this.picZoneMap.Image = null;
                    //读取Image流
                    ms = new MemoryStream();
                    Image fPic = Image.FromFile(ofd.FileName);
                    fPic.Save(ms, ImageFormat.Jpeg);
                    Image sPic = Image.FromStream(ms);
                    fPic.Dispose();

                    this.picZoneMap.Image = sPic;
                    this.picZoneMap.Width = sPic.Width;
                    this.picZoneMap.Height = sPic.Height;
                    ms.Dispose();


                    //XmlDocument xd = new XmlDocument();
                    //xd.Load(this._zoneMapConfigPath);
                    //XmlNode xn = xd.SelectSingleNode("/Map");
                    GoDexData.BLL.worldMap bllWm = new GoDexData.BLL.worldMap();
                    GoDexData.Model.worldMap modelWm = bllWm.GetModel(1);

                    if (modelWm != null)
                    {
                        File.Copy(ofd.FileName, AppDomain.CurrentDomain.BaseDirectory + "zoneMap\\fullMap.jpg", true);
                        modelWm.worldMapPath = AppDomain.CurrentDomain.BaseDirectory + "\\zoneMap\\fullMap.jpg";
                        bllWm.Update(modelWm);
                    }
                    else
                    {
                        File.Copy(ofd.FileName, AppDomain.CurrentDomain.BaseDirectory + "zoneMap\\fullMap.jpg", true);
                        GoDexData.Model.worldMap modelWm1 = new GoDexData.Model.worldMap();
                        modelWm1.worldMapNo = 1;
                        modelWm1.worldMapPath = AppDomain.CurrentDomain.BaseDirectory + "\\zoneMap\\fullMap.jpg";
                        bllWm.Add(modelWm1);
                    } 
                    ofd.Dispose();
                }
            }
            catch (Exception ee) { MessageBox.Show(ee.ToString()); }
        }

        private void btnShowFullMap_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.picZoneMap.Image != null)
                    this.picZoneMap.Image = null; 

                //XmlDocument xd = new XmlDocument();
                //xd.Load(this._zoneMapConfigPath);
                //XmlNode xn = xd.SelectSingleNode("/Map");
                GoDexData.BLL.worldMap bllwm = new GoDexData.BLL.worldMap();
                GoDexData.Model.worldMap modelWm = bllwm.GetModel(1); 

                MemoryStream ms = null;
                if (modelWm != null)
                {
                    if (File.Exists(modelWm.worldMapPath))
                    {
                        ms = new MemoryStream();
                        Image fImg = Image.FromFile(modelWm.worldMapPath);
                        fImg.Save(ms, ImageFormat.Jpeg);
                        fImg.Dispose();
                        this.picZoneMap.Image = Image.FromStream(ms);
                        //        			Bitmap bt = (Bitmap)Image.FromFile(xn.Attributes["imgPath"].Value);
                        //        	        this.picZoneMap.Image = bt;
                        this.picZoneMap.Width = Image.FromStream(ms).Width;
                        this.picZoneMap.Height = Image.FromStream(ms).Height;
                        ms.Dispose();
                        //bt.Dispose();
                    }
                }
            }
            catch (Exception ee) { MessageBox.Show(ee.ToString()); }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(e.Node.Text);
            try
            {
                //XmlDocument xd = new XmlDocument();
                //xd.Load(this._nodeConfigPath);

                if (this.treeViewNodes.SelectedNode == null)
                {  
                      MessageBox.Show("请选择节点"); return; 
                }
                else
                {
                    if (string.IsNullOrEmpty(this.treeViewNodes.SelectedNode.Name))
                    { MessageBox.Show("请选择节点"); return; }
                }

                GoDexData.BLL.nodeInfo bllNi = new GoDexData.BLL.nodeInfo();
                GoDexData.Model.nodeInfo modelNi = bllNi.GetModel(int.Parse(this.treeViewNodes.SelectedNode.Name));  

                //XmlNode xn = xd.SelectSingleNode("/nodeGroup/node[@ID='" + this.treeViewNodes.SelectedNode.Name + "']");
                if (modelNi != null)
                {//<node ID="153" mathineType="单管单驱" mathineXinghao="未知型号" airSpeed="高速" address="" zoneMapID="">
                    modelNi.machineModel = this.cmbNodeType.Text; 
                    modelNi.sign = this.txtNodePos.Text;
                    modelNi.areaMapPath = this.cmbNodeZoneID.SelectedIndex.ToString();//区域地图编号
                    //xn.Attributes["mathineXinghao"].Value = this.cmbNodeType.Text;
                    //xn.Attributes["mathineType"].Value = this.lblNode.Text;
                    //xn.Attributes["address"].Value = this.txtNodePos.Text;
                    //xn.Attributes["zoneMapID"].Value = this.cmbNodeZoneID.Text;                    
                }
                if (bllNi.Update(modelNi))
                {
                    MessageBox.Show("更新成功");
                    getNodeTreeView();
                }
            }
            catch (Exception ee) { MessageBox.Show(ee.ToString()); }
            finally
            {
                this.cmbNodeType.Text = "";
                this.lblNodeID.Text = "";
                this.lblNode.Text = "";
                this.txtNodePos.Text = "";
                this.cmbNodeZoneID.Text = "-";
            }
        }

        private void btnEditNodeSet_Click(object sender, EventArgs e)
        {

        }

        private void treeViewNodes_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //Forms.frmSetNode fsn = new frmSetNode(_userRole,null);
            //fsn.NodeNo = e.Node.Text;
            //fsn.Text = "监控器:" + e.Node.Text + "设备设定";
            //fsn.XmlPath = this._nodeConfigPath;
            //fsn.ShowDialog();
        }

        private void btnShowNodes_Click(object sender, EventArgs e)
        {
            getNodeTreeView();

        }
    }
}
