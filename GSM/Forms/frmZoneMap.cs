using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using System.Drawing.Imaging;

namespace GSM.Forms
{
    public partial class frmZoneMap : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        public frmZoneMap(string nodeID)
        {
            InitializeComponent();
            //this._zoneMapConfigPath = zoneMapConfigPath;
            //this._nodeConfigPath = nodeConfigPath;
            this._nodeID = nodeID;
        }
        ///// <summary>
        ///// 地图配置文件路径
        ///// </summary>
        //private string _zoneMapConfigPath;
        //public string _ZoneMapConfigPath
        //{
        //    get { return this._zoneMapConfigPath; }
        //    set { this._zoneMapConfigPath = value; }
        //}
        ///// <summary>
        ///// 节点配置文件路径
        ///// </summary>
        //private string _nodeConfigPath;
        //public string _NodeConfigPath
        //{
        //    get { return this._nodeConfigPath; }
        //    set { this._nodeConfigPath = value; }
        //}
        /// <summary>
        /// 节点ID
        /// </summary>
        private string _nodeID;
        public string _NodeID
        {
        	get{return _nodeID;}
        	set {this._nodeID = value;}
        }
        /// <summary>
        /// 地图上节点颜色,取决节点的状态
        /// </summary>
        public Color _NodeStatusColor
        {
            get { return this.picZoneMap.BackColor; }
            set { this.picZoneMap.BackColor = value; }
        }

        private  MemoryStream ms = null;
        Point[] pArray = new Point[5];

        private void frmZoneMap_Load(object sender, EventArgs e)
        { 
            readFullMap();
            //读取节点信息
            //readFullNode(_nodeConfigPath);
            
        }
        
        //读取区域地图文件
         private void readFullMap()
        {
            try
            {
            	string zoneMapID = string.Empty;
                if (this.picZoneMap.Image != null)
                    this.picZoneMap.Image = null; 
  
                this.picZoneMap.Controls.Clear();
                
                
                GoDexData.BLL.nodeInfo bllNi = new GoDexData.BLL.nodeInfo();
                GoDexData.Model.nodeInfo modelNi = bllNi.GetModel(int.Parse(this._nodeID));
                zoneMapID = modelNi.areaMapPath; 
                if (!string.IsNullOrEmpty(zoneMapID))
                {
                    GoDexData.BLL.areaMap bllAm = new GoDexData.BLL.areaMap();
                    GoDexData.Model.areaMap modelAm = bllAm.GetModel(int.Parse(zoneMapID));

                    //读取此区域地图ID的节点集

                    //读取区域地图文件
                    //XmlDocument xd = new XmlDocument();
                    //xd.Load(xmlMapPath);
                    //XmlNode xn = xd.SelectSingleNode("/Map/zoneMap[@zoneID='"+ zoneMapID +"']" );

                    if (modelAm != null)
                    {
                        if (File.Exists(modelAm.areaMapPath))
                        {
                            ms = new MemoryStream();
                            Image fImg = Image.FromFile(modelAm.areaMapPath);
                            if (fImg == null)
                            {
                                Bitmap bm = new Bitmap(this.picZoneMap.Width, this.picZoneMap.Height);
                                fImg = bm;
                            }
                            fImg.Save(ms, ImageFormat.Jpeg);
                            fImg.Dispose();
                            this.picZoneMap.Image = Image.FromStream(ms);
                            this.picZoneMap.Width = Image.FromStream(ms).Width;
                            this.picZoneMap.Height = Image.FromStream(ms).Height;
                            //ms.Dispose();
                        }
                    }
                    readFullNode(zoneMapID);
                } 
            }
            catch (Exception ee) {MessageBox.Show(ee.ToString()); }
        }

        private void readFullNode(string zoneMapID)
        {
            //try
            //{
                string tipinfo = string.Empty;
               
                GoDexData.BLL.nodeInfo bllNi = new GoDexData.BLL.nodeInfo();
                DataSet dsNi = bllNi.GetList(" areaMapPath='" + zoneMapID + "'");
                if (dsNi.Tables[0].Rows.Count == 0)
                { return; }

                foreach (DataRow dr in dsNi.Tables[0].Rows)
                {
                    UserControls.NodeSite ns = new GSM.UserControls.NodeSite();
                    //ns._FullXY = new Point(int.Parse(xn.Attributes["allPosX"].Value), int.Parse(xn.Attributes["allPosY"].Value));
                    ns.Name = dr["machineNo"].ToString();
                    ns.lblNo.Text = dr["machineNo"].ToString();
                    ns.lblNo.BackColor = Color.White;
                    ns.lblNo.ForeColor = Color.Black;
                    if (this._nodeID == dr["machineNo"].ToString())
                        ns.pbStatus.BackColor = this._NodeStatusColor;
                    ns.AutoSize = false; 
                    if (dr["areaXY"].ToString() != null)
                    {
                        string[] strXY = dr["areaXY"].ToString().Split(',');
                        ns.Left = int.Parse(strXY[0]);
                        ns.Top = int.Parse(strXY[1]);
                    }
                    else
                    {
                        ns.Left =0;
                        ns.Top = 0;
                    }
                    pArray[0] = ns.Location;
                    if (this.picZoneMap.Image == null)
                    {
                        MessageBox.Show("未设置区域地图,请设置");
                        return;
                    }
                    Graphics g = Graphics.FromImage(this.picZoneMap.Image);
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                    if (dr["machineType"].ToString() == "3")
                    {
                        UC.NodeChild nc1 = new GSM.UC.NodeChild();
                        this.picZoneMap.Controls.Add(nc1);
                        nc1._ID = dr["machineNo"].ToString();
                        nc1.Name = dr["machineNo"].ToString();
                        nc1.lblNodeChild.ForeColor = Color.Black;
                        nc1.lblNodeChild.BackColor = Color.White;
                        nc1.lblNodeChild.Text = dr["machineNo"].ToString() + "-1区";
                        nc1._Area = 1;
                        nc1.BringToFront(); 
                        string[] strXY1 = dr["areaXY_1"].ToString().Split(',');  
                        nc1.Left = int.Parse(strXY1[0]);
                        nc1.Top = int.Parse(strXY1[1]);
                        nc1.MouseUp += new MouseEventHandler(nc1_LocationChanged);
                        Cls.ControlMover.Init(nc1);
                        //Cls.ControlMover.Init(nc1);
                        pArray[1] = nc1.Location;
                        g.DrawLine(Pens.Silver, pArray[0].X + 15, pArray[0].Y + 25, pArray[1].X + 15, pArray[1].Y + 25);


                        UC.NodeChild nc2 = new GSM.UC.NodeChild();
                        this.picZoneMap.Controls.Add(nc2);
                        nc2._ID = dr["machineNo"].ToString();
                        nc2.Name = dr["machineNo"].ToString();
                        nc2.lblNodeChild.ForeColor = Color.Black;
                        nc2.lblNodeChild.BackColor = Color.White;
                        nc2._Area = 2;
                        nc2.lblNodeChild.Text = dr["machineNo"].ToString() + "-2区";
                        nc2.BringToFront();
                        string[] strXY2 = dr["areaXY_2"].ToString().Split(',');
                        nc2.Left = int.Parse(strXY2[0]);
                        nc2.Top = int.Parse(strXY2[1]);

                        nc2.MouseUp += new MouseEventHandler(nc1_LocationChanged);
                        //Cls.ControlMover.Init(nc2);
                        Cls.ControlMover.Init(nc2);
                        pArray[2] = nc2.Location;
                        g.DrawLine(Pens.Silver, pArray[0].X + 15, pArray[0].Y + 25, pArray[2].X + 15, pArray[2].Y + 25);

                        UC.NodeChild nc3 = new GSM.UC.NodeChild();
                        this.picZoneMap.Controls.Add(nc3);
                        nc3._Area = 3;
                        nc3._ID = dr["machineNo"].ToString();
                        nc3.Name = dr["machineNo"].ToString();
                        nc3.lblNodeChild.ForeColor = Color.Black;
                        nc3.lblNodeChild.BackColor = Color.White;
                        nc3.BringToFront();
                        nc3.lblNodeChild.Text = dr["machineNo"].ToString() + "-3区";

                        string[] strXY3 = dr["areaXY_3"].ToString().Split(',');
                        nc3.Left = int.Parse(strXY3[0]);
                        nc3.Top = int.Parse(strXY3[1]);

                        nc3.MouseUp += new MouseEventHandler(nc1_LocationChanged);
                        //Cls.ControlMover.Init(nc3);
                        Cls.ControlMover.Init(nc3);
                        pArray[3] = nc3.Location;
                        g.DrawLine(Pens.Silver, pArray[0].X + 15, pArray[0].Y + 25, pArray[3].X + 15, pArray[3].Y + 25);

                        UC.NodeChild nc4 = new GSM.UC.NodeChild();
                        this.picZoneMap.Controls.Add(nc4);
                        nc4._ID = dr["machineNo"].ToString();
                        nc4.lblNodeChild.ForeColor = Color.Black;
                        nc4.lblNodeChild.BackColor = Color.White;
                        nc4._Area = 4;
                        nc4.Name = dr["machineNo"].ToString();
                        nc4.BringToFront();
                        nc4.lblNodeChild.Text = dr["machineNo"].ToString() + "-4区";

                        string[] strXY4 = dr["areaXY_4"].ToString().Split(',');
                        nc4.Left = int.Parse(strXY4[0]);
                        nc4.Top = int.Parse(strXY4[1]);

                        nc4.MouseUp += new MouseEventHandler(nc1_LocationChanged);
                        //Cls.ControlMover.Init(nc4);
                        Cls.ControlMover.Init(nc4);
                        //nc4.MouseDown += new MouseEventHandler(nc1_MouseDown);
                        pArray[4] = nc4.Location;
                        g.DrawLine(Pens.Silver, pArray[0].X + 15, pArray[0].Y + 25, pArray[4].X + 15, pArray[4].Y + 25); 
                    }
 
                    ns.BringToFront();
                    Cls.ControlMover.Init(ns);
                    //Cls.ControlMover.Init(ns);
                    ns.MouseUp += new MouseEventHandler(ns_LocationChanged);
                    
                    this.picZoneMap.Controls.Add(ns);
                }

                dsNi.Dispose();
            //}
            //catch (Exception ee) { 
            //    //MessageBox.Show(ee.ToString());
            //}
        }
        
        void ns_LocationChanged(object sender, EventArgs e)
        {
            UserControls.NodeSite ns = (UserControls.NodeSite)sender;
            try
            {
                //XmlDocument xd = new XmlDocument();
                //xd.Load(this._nodeConfigPath);
                //XmlNode xnl = xd.SelectSingleNode("/nodeGroup/node[@ID='" + ns._CurID + "']");

                GoDexData.BLL.nodeInfo bllNi = new GoDexData.BLL.nodeInfo();
                GoDexData.Model.nodeInfo modelNi = bllNi.GetModel(int.Parse(ns.Name));

                if (modelNi != null)
                {
                    modelNi.areaXY = ns.Left.ToString() + "," + ns.Top.ToString(); 
                    //if (modelNi.machineType.ToString() == "3")
                    //{    
                    //    pArray[0] = ns.Location;

                    //    string[] strXY1 = modelNi.areaXY_1.ToString().Split(',');
                    //    pArray[1].X = int.Parse(strXY1[0]);
                    //    pArray[1].Y = int.Parse(strXY1[1]);

                    //    string[] strXY2 = modelNi.areaXY_2.ToString().Split(',');
                    //    pArray[2].X = int.Parse(strXY2[0]);
                    //    pArray[2].Y = int.Parse(strXY2[1]);

                    //    string[] strXY3 = modelNi.areaXY_3.ToString().Split(',');
                    //    pArray[3].X = int.Parse(strXY3[0]);
                    //    pArray[3].Y = int.Parse(strXY3[1]);

                    //    string[] strXY4 = modelNi.areaXY_4.ToString().Split(',');
                    //    pArray[4].X = int.Parse(strXY4[0]);
                    //    pArray[4].Y = int.Parse(strXY4[1]); 

                    //    this.picZoneMap.Image = Image.FromStream(ms);
                    //    Graphics g = Graphics.FromImage(this.picZoneMap.Image);
                    //    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    //    g.DrawLine(Pens.Silver, pArray[0].X + 15, pArray[0].Y + 25, pArray[1].X + 15, pArray[1].Y + 25);
                    //    g.DrawLine(Pens.Silver, pArray[0].X + 15, pArray[0].Y + 25, pArray[2].X + 15, pArray[2].Y + 25);
                    //    g.DrawLine(Pens.Silver, pArray[0].X + 15, pArray[0].Y + 25, pArray[3].X + 15, pArray[3].Y + 25);
                    //    g.DrawLine(Pens.Silver, pArray[0].X + 15, pArray[0].Y + 25, pArray[4].X + 15, pArray[4].Y + 25); 
                    //    this.picZoneMap.Refresh();
                    //    g.Dispose();
                    //}
                }
                bllNi.Update(modelNi);
                readFullMap();
            }
            catch (Exception ee) {
                // MessageBox.Show(ee.ToString());
            }
            //throw new NotImplementedException();
        }

        void nc1_LocationChanged(object sender, EventArgs e)
        {
            UC.NodeChild nc = (UC.NodeChild)sender;
            //try
            //{
                //XmlDocument xd = new XmlDocument();
                //xd.Load(this._nodeConfigPath);
                //XmlNode xnl = xd.SelectSingleNode("/nodeGroup/node[@ID='" + nc._ID + "']");

                GoDexData.BLL.nodeInfo bllNi = new GoDexData.BLL.nodeInfo();
                GoDexData.Model.nodeInfo modelNi = bllNi.GetModel(int.Parse(nc.Name)); 

                if (modelNi != null)
                {
                    switch (nc._Area)
                    {
                        case 1:
                            modelNi.areaXY_1 = nc.Left.ToString() + "," + nc.Top.ToString();
                            break;
                        case 2:
                            modelNi.areaXY_2 = nc.Left.ToString() + "," + nc.Top.ToString();
                            break;
                        case 3:
                            modelNi.areaXY_3 = nc.Left.ToString() + "," + nc.Top.ToString();
                            break;
                        case 4:
                            modelNi.areaXY_4 = nc.Left.ToString() + "," + nc.Top.ToString();
                            break;
                    }

                    //if (modelNi.machineType.ToString() == "3")
                    //{
                    //    string[] strXY = modelNi.areaXY.ToString().Split(','); 
                    //    pArray[0] = new Point(int.Parse(strXY[0]), int.Parse(strXY[1]));
                    //    pArray[nc._Area] = nc.Location;

                    //    string[] strXY1 = modelNi.areaXY_1.ToString().Split(',');
                    //    pArray[1].X = int.Parse(strXY1[0]);
                    //    pArray[1].Y = int.Parse(strXY1[1]);

                    //    string[] strXY2 = modelNi.areaXY_2.ToString().Split(',');
                    //    pArray[2].X = int.Parse(strXY2[0]);
                    //    pArray[2].Y = int.Parse(strXY2[1]);

                    //    string[] strXY3 = modelNi.areaXY_3.ToString().Split(',');
                    //    pArray[3].X = int.Parse(strXY3[0]);
                    //    pArray[3].Y = int.Parse(strXY3[1]);

                    //    string[] strXY4 = modelNi.areaXY_4.ToString().Split(',');
                    //    pArray[4].X = int.Parse(strXY4[0]);
                    //    pArray[4].Y = int.Parse(strXY4[1]); 

                    //    this.picZoneMap.Image = Image.FromStream(ms);
                    //    Graphics g = Graphics.FromImage(this.picZoneMap.Image);
                    //    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    //    g.DrawLine(Pens.Silver, pArray[0].X + 15, pArray[0].Y + 25, pArray[1].X + 15, pArray[1].Y + 25);
                    //    g.DrawLine(Pens.Silver, pArray[0].X + 15, pArray[0].Y + 25, pArray[2].X + 15, pArray[2].Y + 25);
                    //    g.DrawLine(Pens.Silver, pArray[0].X + 15, pArray[0].Y + 25, pArray[3].X + 15, pArray[3].Y + 25);
                    //    g.DrawLine(Pens.Silver, pArray[0].X + 15, pArray[0].Y + 25, pArray[4].X + 15, pArray[4].Y + 25); 
                    //    this.picZoneMap.Refresh();
                    //    g.Dispose();
                    //}
                }
                bllNi.Update(modelNi);
                readFullMap();
            //}
            //catch (Exception ee) { //MessageBox.Show(ee.ToString()); 
            //}
        }

        private void tsbtnEdit_Click(object sender, EventArgs e)
        {
            this.tsbtnLock.Enabled = true;
            this.picZoneMap.Enabled = true;
            this.tsbtnEdit.Enabled = false;
        }

        private void tsbtnLock_Click(object sender, EventArgs e)
        {
            this.tsbtnLock.Enabled = false;
            this.tsbtnEdit.Enabled = true;
            this.picZoneMap.Enabled = false;
        }

        
    }
}
