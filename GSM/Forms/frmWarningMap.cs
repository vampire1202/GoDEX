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
    public partial class frmWarningMap : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        public frmWarningMap(string user, string nodeID, string warningMsg, Color nodeStatusColor, int chl)
        {
            _user = user;
            //_zoneMapConfigPath = zoneMapConfigPath;
            //_nodeConfigPath = nodeConfigPath;
            _nodeID = nodeID;
            _chl = chl;
            _warningMsg = warningMsg;
            _nodeStatusColor = nodeStatusColor;
            InitializeComponent();
        }

        private string _user;
        public string _User
        {
            get { return this._user; }
            set { this._user = value; }
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
            get { return _nodeID; }
            set { this._nodeID = value; }
        }
        /// <summary>
        /// 报警信息
        /// </summary>
        private string _warningMsg;
        public string _WarningMsg
        {
            get { return _warningMsg; }
            set { _warningMsg = value; }
        }

        /// <summary>
        /// 报警信息
        /// </summary>
        private int _chl;
        public int _Chl
        {
            get { return _chl; }
            set { _chl = value; }
        }

        /// <summary>
        /// 地图上节点颜色,取决节点的状态
        /// </summary>
        private Color _nodeStatusColor;
        public Color _NodeStatusColor
        {
            get { return _nodeStatusColor; }
            set { _nodeStatusColor = value; }
        }

        private void frmZoneMap_Load(object sender, EventArgs e)
        {
            //this.panel1.Width = this.splitContainer1.Panel1.Width;
            this.panel1.AutoScroll = true;
            //读取地图信息
            readFullMap();
            this.lblWarningMsg.Text = _warningMsg;
            this.lblWarningMsg.BackColor = _nodeStatusColor;
            //switch (_nodeStatusColor.Name)
            //{
            //    case "Gold":
            //    case "DarkOrange":
            //        this.lblWarningMsg.BackColor = Color.Gold;
            //        break;
            //    case "Pink":
            //    case "LightCoral":
            //    case "Red":
            //    case "Purple":
            //        this.lblWarningMsg.BackColor = Color.LightCoral;
            //        break;
            //    case "DeepSkyBlue":
            //        this.lblWarningMsg.BackColor = Color.DeepSkyBlue;
            //        break;
            //    default:
            //        break;
            //}
        }
        MemoryStream ms = null;
        Point[] pArray = new Point[5];

        //读取区域地图文件
        private void readFullMap()
        {
            try
            {
                string zoneMapID = string.Empty;
                if (this.picZoneMap.Image != null)
                    this.picZoneMap.Image = null;
                //XmlDocument xdN = new XmlDocument();
                //xdN.Load(xmlNodePath);
                //读取节点的区域地图ID
                //XmlNode xnN = xdN.SelectSingleNode("/nodeGroup/node[@ID='"+ this._nodeID +"']");
                //if(xnN != null)
                //    zoneMapID = xnN.Attributes["zoneMapID"].Value;                                                   	

                //读取区域地图文件
                //XmlDocument xd = new XmlDocument();
                //xd.Load(xmlMapPath);
                //XmlNode xn = xd.SelectSingleNode("/Map/zoneMap[@zoneID='"+ zoneMapID +"']" );
                GoDexData.BLL.nodeInfo bllni = new GoDexData.BLL.nodeInfo();
                GoDexData.Model.nodeInfo modelni = bllni.GetModel(int.Parse(this._nodeID));
                zoneMapID = modelni.areaMapPath;

                if (!string.IsNullOrEmpty(zoneMapID))
                {
                    GoDexData.BLL.areaMap bllam = new GoDexData.BLL.areaMap();
                    GoDexData.Model.areaMap modelam = bllam.GetModel(int.Parse(zoneMapID));
                    if (modelam != null)
                    {
                        if (File.Exists(modelam.areaMapPath))
                        {
                            ms = new MemoryStream();
                            Image fImg = Image.FromFile(modelam.areaMapPath);
                            fImg.Save(ms, ImageFormat.Jpeg);
                            fImg.Dispose();
                            this.picZoneMap.Image = Image.FromStream(ms);
                            this.picZoneMap.Width = Image.FromStream(ms).Width;
                            this.picZoneMap.Height = Image.FromStream(ms).Height;
                            ms.Dispose();
                            //bt.Dispose();
                        }
                    }
                    //读取此区域地图ID的节点集
                    readFullNode(zoneMapID);
                }


            }
            catch (Exception ee)
            { //MessageBox.Show(ee.ToString());
            }
        }

        private void readFullNode(string zoneMapID)
        {
            try
            {
                string tipinfo = string.Empty;
                //XmlDocument xd = new XmlDocument();
                //xd.Load(this._nodeConfigPath);
                //XmlNodeList xnl = xd.SelectNodes("/nodeGroup/node[@zoneMapID='"+ zoneMapID + "']");

                GoDexData.BLL.nodeInfo bllni = new GoDexData.BLL.nodeInfo();
                DataSet dsni = bllni.GetList(" areaMapPath='" + zoneMapID + "'");

                foreach (DataRow dr in dsni.Tables[0].Rows)
                {
                    UserControls.NodeSite ns = new GSM.UserControls.NodeSite();
                    //ns._FullXY = new Point(int.Parse(xn.Attributes["allPosX"].Value), int.Parse(xn.Attributes["allPosY"].Value));
                    ns.Name = dr["machineNo"].ToString();
                    ns.lblNo.Text = dr["machineNo"].ToString();

                    if (this._nodeID == dr["machineNo"].ToString())
                        ns.pbStatus.BackColor = this._NodeStatusColor;

                    ns.AutoSize = false;
                    ns._MathineType = dr["machineType"].ToString();
                    ns.Refresh();
                    if (dr["areaXY"] != null)
                    {
                        string[] posXY = dr["areaXY"].ToString().Split(',');
                        ns.Left = int.Parse(posXY[0]);
                        ns.Top = int.Parse(posXY[1]);
                    }
                    else
                    {
                        ns.Left = 0;
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
                        nc1.lblNodeChild.Text = dr["machineNo"].ToString() + "-1区";
                        nc1._Area = 1;
                        nc1.BringToFront();
                        if (dr["areaXY_1"].ToString() != null)
                        {
                            string[] strXY = dr["areaXY_1"].ToString().Split(',');
                            nc1.Left = int.Parse(strXY[0]);
                            nc1.Top = int.Parse(strXY[1]);
                        }
                        else
                        {
                            nc1.Left = 0;
                            nc1.Top = 0;
                        }

                        //nc1.Left = int.Parse(xn.ChildNodes[6].Attributes["zoneMapX"].Value);
                        //nc1.Top = int.Parse(xn.ChildNodes[6].Attributes["zoneMapY"].Value);
                        // nc1.LocationChanged += new EventHandler(nc1_LocationChanged);
                        pArray[1] = nc1.Location;
                        g.DrawLine(Pens.DarkGray, pArray[0].X + 15, pArray[0].Y + 25, pArray[1].X + 15, pArray[1].Y + 25);

                        UC.NodeChild nc2 = new GSM.UC.NodeChild();
                        this.picZoneMap.Controls.Add(nc2);
                        nc2._ID = dr["machineNo"].ToString();
                        nc2._Area = 2;
                        nc2.lblNodeChild.Text = dr["machineNo"].ToString() + "-2区";
                        nc2.BringToFront();
                        if (dr["areaXY_2"].ToString() != null)
                        {
                            string[] strXY = dr["areaXY_2"].ToString().Split(',');
                            nc2.Left = int.Parse(strXY[0]);
                            nc2.Top = int.Parse(strXY[1]);
                        }
                        else
                        {
                            nc2.Left = 0;
                            nc2.Top = 0;
                        }
                        //nc2.Left = int.Parse(xn.ChildNodes[7].Attributes["zoneMapX"].Value);
                        //nc2.Top = int.Parse(xn.ChildNodes[7].Attributes["zoneMapY"].Value);
                        // nc2.LocationChanged += new EventHandler(nc1_LocationChanged);
                        pArray[2] = nc2.Location;
                        g.DrawLine(Pens.DarkGray, pArray[0].X + 15, pArray[0].Y + 25, pArray[2].X + 15, pArray[2].Y + 25);

                        UC.NodeChild nc3 = new GSM.UC.NodeChild();
                        this.picZoneMap.Controls.Add(nc3);
                        nc3._Area = 3;
                        nc3._ID = dr["machineNo"].ToString();
                        nc3.BringToFront();
                        nc3.lblNodeChild.Text = dr["machineNo"].ToString() + "-3区";
                        //nc3.Left = int.Parse(xn.ChildNodes[8].Attributes["zoneMapX"].Value);
                        //nc3.Top = int.Parse(xn.ChildNodes[8].Attributes["zoneMapY"].Value);
                        // nc3.LocationChanged += new EventHandler(nc1_LocationChanged);
                        if (dr["areaXY_3"].ToString() != null)
                        {
                            string[] strXY = dr["areaXY_3"].ToString().Split(',');
                            nc3.Left = int.Parse(strXY[0]);
                            nc3.Top = int.Parse(strXY[1]);
                        }
                        else
                        {
                            nc3.Left = 0;
                            nc3.Top = 0;
                        }
                        pArray[3] = nc3.Location;
                        g.DrawLine(Pens.DarkGray, pArray[0].X + 15, pArray[0].Y + 25, pArray[3].X + 15, pArray[3].Y + 25);

                        UC.NodeChild nc4 = new GSM.UC.NodeChild();
                        this.picZoneMap.Controls.Add(nc4);
                        nc4._ID = dr["machineNo"].ToString();
                        nc4._Area = 4;
                        nc4.BringToFront();
                        nc4.lblNodeChild.Text = dr["machineNo"].ToString() + "-4区";
                        //nc4.Left = int.Parse(xn.ChildNodes[9].Attributes["zoneMapX"].Value);
                        //nc4.Top = int.Parse(xn.ChildNodes[9].Attributes["zoneMapY"].Value);
                        //nc4.LocationChanged += new EventHandler(nc1_LocationChanged);
                        if (dr["areaXY_4"].ToString() != null)
                        {
                            string[] strXY = dr["areaXY_4"].ToString().Split(',');
                            nc4.Left = int.Parse(strXY[0]);
                            nc4.Top = int.Parse(strXY[1]);
                        }
                        else
                        {
                            nc4.Left = 0;
                            nc4.Top = 0;
                        }

                        pArray[4] = nc4.Location;
                        g.DrawLine(Pens.DarkGray, pArray[0].X + 15, pArray[0].Y + 25, pArray[4].X + 15, pArray[4].Y + 25);
                        switch (_chl)
                        {
                            case 0:
                                ns.pbStatus.BackColor = this._NodeStatusColor;
                                break;
                            case 1:
                                nc1.BackColor = this._nodeStatusColor;
                                break;
                            case 2:
                                nc2.BackColor = this._nodeStatusColor;
                                break;
                            case 3:
                                nc3.BackColor = this._nodeStatusColor;
                                break;
                            case 4:
                                nc4.BackColor = this._nodeStatusColor;
                                break;
                        }
                    }


                    ns.BringToFront();
                    //ns.LocationChanged += new EventHandler(ns_LocationChanged);                    
                    this.picZoneMap.Controls.Add(ns);
                }
            }
            catch (Exception ee) { MessageBox.Show(ee.ToString()); }
        }

        //void ns_LocationChanged(object sender, EventArgs e)
        //{
        //    UserControls.NodeSite ns = (UserControls.NodeSite)sender;
        //    try
        //    {
        //        XmlDocument xd = new XmlDocument();
        //        xd.Load(this._nodeConfigPath);
        //        XmlNode xnl = xd.SelectSingleNode("/nodeGroup/node[@ID='" + ns._CurID + "']");

        //        this.picZoneMap.Image = Image.FromStream(ms);
        //        Graphics g = Graphics.FromImage(this.picZoneMap.Image);
        //        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        //        if (xnl != null)
        //        {
        //            xnl.Attributes["zoneMapX"].Value = (ns.Left).ToString();
        //            xnl.Attributes["zoneMapY"].Value = (ns.Top).ToString();
        //            pArray[0] = ns.Location;
        //            g.DrawLine(Pens.Blue, pArray[0].X + 15, pArray[0].Y + 25, pArray[1].X + 15, pArray[1].Y + 25);
        //            g.DrawLine(Pens.Blue, pArray[0].X + 15, pArray[0].Y + 25, pArray[2].X + 15, pArray[2].Y + 25);
        //            g.DrawLine(Pens.Blue, pArray[0].X + 15, pArray[0].Y + 25, pArray[3].X + 15, pArray[3].Y + 25);
        //            g.DrawLine(Pens.Blue, pArray[0].X + 15, pArray[0].Y + 25, pArray[4].X + 15, pArray[4].Y + 25);
        //        }
        //        this.picZoneMap.Refresh();
        //        g.Dispose();
        //        xd.Save(this._nodeConfigPath);
        //    }
        //    catch (Exception ee) { MessageBox.Show(ee.ToString()); }
        //    //throw new NotImplementedException();
        //}

        //void nc1_LocationChanged(object sender, EventArgs e)
        //{
        //    UC.NodeChild nc = (UC.NodeChild)sender;
        //    try
        //    {
        //        XmlDocument xd = new XmlDocument();
        //        xd.Load(this._nodeConfigPath);
        //        XmlNode xnl = xd.SelectSingleNode("/nodeGroup/node[@ID='" + nc._ID + "']");
        //        this.picZoneMap.Image = Image.FromStream(ms);
        //        Graphics g = Graphics.FromImage(this.picZoneMap.Image);
        //        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        //        if (xnl != null)
        //        {
        //            xnl.ChildNodes[5 + nc._Area].Attributes["zoneMapX"].Value = (nc.Left).ToString();
        //            xnl.ChildNodes[5 + nc._Area].Attributes["zoneMapY"].Value = (nc.Top).ToString();
        //            pArray[nc._Area] = nc.Location;
        //            g.DrawLine(Pens.Blue, pArray[0].X + 15, pArray[0].Y + 25, pArray[1].X + 15, pArray[1].Y + 25);
        //            g.DrawLine(Pens.Blue, pArray[0].X + 15, pArray[0].Y + 25, pArray[2].X + 15, pArray[2].Y + 25);
        //            g.DrawLine(Pens.Blue, pArray[0].X + 15, pArray[0].Y + 25, pArray[3].X + 15, pArray[3].Y + 25);
        //            g.DrawLine(Pens.Blue, pArray[0].X + 15, pArray[0].Y + 25, pArray[4].X + 15, pArray[4].Y + 25);
        //        }
        //        this.picZoneMap.Refresh();
        //        g.Dispose();
        //        xd.Save(this._nodeConfigPath);
        //    }
        //    catch (Exception ee) { MessageBox.Show(ee.ToString()); }
        //}

        private void btnOk_Click(object sender, EventArgs e)
        {
            //Cls.Method.writeLog(_user + "确认报警:" + this.lblWarningMsg.Text+"[" + DateTime.Now.ToString() + "]");
            //Cls.Method.writeLog(_user + "确认描述:" + this.txtWarningMsg.Text + "[" + DateTime.Now.ToString() + "]");
            Cls.Method.writeLogData(int.Parse(this._nodeID), "确认报警:" + this.lblWarningMsg.Text, this._user, DateTime.Now, "-");
            Cls.Method.writeLogData(int.Parse(this._nodeID), "确认描述:" + this.txtWarningMsg.Text, this._user, DateTime.Now, "-");
            this.Dispose();
        }
        private void frmWarningMap_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Cls.Method.writeLog(_user + "取消报警:" + this.lblWarningMsg.Text + "[" + DateTime.Now.ToString() + "]");
            Cls.Method.writeLogData(int.Parse(this._nodeID), "取消报警:" + this.lblWarningMsg.Text, this._user, DateTime.Now, "-");
            this.Dispose();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //Cls.Method.writeLog(_user + "取消报警:" + this.lblWarningMsg.Text + "[" + DateTime.Now.ToString() + "]");
            Cls.Method.writeLogData(int.Parse(this._nodeID), "取消报警:" + this.lblWarningMsg.Text, this._user, DateTime.Now, "-");
            this.Dispose();
        }

        private void splitContainer1_Resize(object sender, EventArgs e)
        {
            this.panel1.Width = this.splitContainer1.Panel1.Width;
            this.panel1.Height = this.splitContainer1.Panel1.Height;
        }
    }
}
