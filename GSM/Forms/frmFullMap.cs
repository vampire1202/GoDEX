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
    public partial class frmFullMap : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        public frmFullMap()
        {
            InitializeComponent(); 
        }

        //地图原始尺寸
        public System.Drawing.Size _fullMapSize;
        //地图缩放比例
        public double _zoneValue;
        //地图控件
        public System.Drawing.Size _fullNodeSize;

        Point[] pArray = new Point[5];

        private string userRole;
        public string _UserRole
        {
            get { return userRole; }
            set { userRole = value; }
        }


        public void frmFullMap_Load(object sender, EventArgs e)
        { 
            //读取地图信息
            readFullMap(this.picZoneMap);
            //读取节点信息
            readFullNode();
            _fullMapSize = new Size(this.picZoneMap.Size.Width, this.picZoneMap.Size.Height);
            _fullNodeSize = new Size(40, 36);
            _zoneValue = 1;
        }

        //private void picZoneMap_MouseWheel(object sender, MouseEventArgs e)
        //{
        //    System.Drawing.Size picSize = picZoneMap.Size;
        //    try
        //    {
        //        if (e.Delta < 0 && this.picZoneMap.Width > (_fullMapSize.Width / 5))
        //        {
        //            this.picZoneMap.Width = this.picZoneMap.Width * 9 / 10;
        //            this.picZoneMap.Height = this.picZoneMap.Height * 9 / 10;
        //            _zoneValue = _fullMapSize.Width / this.picZoneMap.Width;
        //            foreach (UserControls.NodeSite ns in this.picZoneMap.Controls)
        //            {
        //                ns.Left = ns.Left * 9 / 10;                        
        //                ns.Top = ns.Top * 9 / 10;


        //                //ns.Width = ns.Width * 9 / 10;
        //                //ns.Height = ns.Height * 9 / 10;
        //            }
        //        }

        //        if (e.Delta > 0 && this.picZoneMap.Width < (_fullMapSize.Width))
        //        {
        //            this.picZoneMap.Width = this.picZoneMap.Width * 11 / 10;
        //            this.picZoneMap.Height = this.picZoneMap.Height * 11 / 10;
        //            _zoneValue = _fullMapSize.Width / this.picZoneMap.Width;
        //            foreach (UserControls.NodeSite ns in this.picZoneMap.Controls)
        //            {
        //                if (ns.Width > 0)
        //                {
        //                    ns.Left = ns.Left * 11 / 10;
        //                    ns.Top = ns.Top * 11 / 10;
        //                    //ns.Width = ns.Width * 11 / 10;
        //                    //ns.Height = ns.Height * 11 / 10;
        //                }
        //            }
        //        }
        //    }
        //    catch { }
        //}
        System.IO.MemoryStream mstream = null;
        public void readFullMap( PictureBox picBox)
        {

            //try
            //{
                if (picBox.Image != null)
                    picBox.Image = null;

                //XmlDocument xd = new XmlDocument();
                //xd.Load(xmlMapPath);
                //XmlNode xn = xd.SelectSingleNode("/Map");
                GoDexData.BLL.worldMap bllwm = new GoDexData.BLL.worldMap();
                GoDexData.Model.worldMap modelwm = bllwm.GetModel(1);
                if (modelwm != null)
                {
                    if (File.Exists(modelwm.worldMapPath))
                    {
                        this.mstream = new MemoryStream();
                        Image fImg = Image.FromFile(modelwm.worldMapPath);
                        if (fImg == null)
                        {
                            Bitmap bm = new Bitmap(this.picZoneMap.Width, this.picZoneMap.Height);
                            fImg = bm;
                        }
                        fImg.Save(mstream, ImageFormat.Bmp);
                        fImg.Dispose();
                        picBox.Image = Image.FromStream(mstream);
                        //        			Bitmap bt = (Bitmap)Image.FromFile(xn.Attributes["imgPath"].Value);
                        //        	        this.picZoneMap.Image = bt;
                        picBox.Width = Image.FromStream(mstream).Width;
                        picBox.Height = Image.FromStream(mstream).Height;
                        //ms.Dispose();
                        //bt.Dispose();
                    }
                    else
                    {
                        MessageBox.Show("全局地图不存在");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("未设置全局地图");
                    return;
                }
            //}
            //catch (Exception ee) { MessageBox.Show(ee.ToString()); }
        }

        private void readFullNode()
        {
            try
            {
                this.picZoneMap.Controls.Clear();
             
                string tipinfo = string.Empty;
                GoDexData.BLL.nodeInfo bllNi = new GoDexData.BLL.nodeInfo();
                DataSet dsNi = bllNi.GetAllList();
                foreach (DataRow dr in dsNi.Tables[0].Rows)
                {
                    UserControls.NodeSite ns = new GSM.UserControls.NodeSite(); 
                    //ns._FullXY = new Point(int.Parse(xn.Attributes["allPosX"].Value), int.Parse(xn.Attributes["allPosY"].Value));
                    ns.Name = dr["machineNo"].ToString(); // xn.Attributes["ID"].Value;
                    ns.lblNo.Text = dr["machineNo"].ToString();//xn.Attributes["ID"].Value;
                    ns.lblNo.ForeColor = Color.Black;

                    switch(dr["doordog"].ToString())
                    {  
                        case null:
                        case "0":
                            ns.lblNo.BackColor = Color.Transparent;
                            break; 
                        case "1":
                            ns.lblNo.BackColor = Color.Pink;
                            break;
                        case "2":
                            ns.lblNo.BackColor = Color.LightCoral;
                            break;
                        case "3":
                            ns.lblNo.BackColor = Color.Red;
                            break;
                        case "4":
                            ns.lblNo.BackColor = Color.Purple;
                            break; 
                    } 

                    ns.BringToFront();
                    if (dr["worldXY"] != null)
                    {
                        string[] strXY = dr["worldXY"].ToString().Split(',');
                        ns.Left = int.Parse(strXY[0]);
                        ns.Top = int.Parse(strXY[1]);
                    }
                    else
                    {
                        ns.Left = 0;
                        ns.Top = 0;
                    } 

                    this.picZoneMap.Controls.Add(ns);
                    Cls.ControlMover.Init(ns);
                    ns.MouseUp += new MouseEventHandler(ns_LocationChanged);
                    //ns.LocationChanged += new EventHandler(ns_LocationChanged); 
                    pArray[0] = ns.Location; 

                    ns._MathineType = dr["machineType"].ToString();
                    //if (this.picZoneMap.Image == null)
                    //{
                    //    Bitmap bm = new Bitmap(this.picZoneMap.Width, this.picZoneMap.Height);
                    //    this.picZoneMap.Image = bm;
                    //    return;
                    //}

                    Graphics g = Graphics.FromImage(this.picZoneMap.Image);
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;


                    //if (ns._MathineType == "3")
                    //{
                    //    UC.NodeChild nc1 = new GSM.UC.NodeChild();
                    //    this.picZoneMap.Controls.Add(nc1);
                    //    nc1._ID = dr["machineNo"].ToString() ;
                    //    nc1.lblNodeChild.Text = dr["machineNo"].ToString() + "-1区";
                    //    nc1.lblNodeChild.BackColor = Color.White;
                    //    nc1.lblNodeChild.ForeColor = Color.Black;
                    //    nc1._Area = 1;
                    //    nc1.Name = dr["machineNo"].ToString();
                    //    nc1.BringToFront();
                    //    if (dr["worldXY_1"].ToString() != null)
                    //    {
                    //        string[] strXY = dr["worldXY_1"].ToString().Split(',');
                    //        nc1.Left = int.Parse(strXY[0]);
                    //        nc1.Top = int.Parse(strXY[1]);
                    //    }
                    //    else
                    //    {
                    //        nc1.Left = 0;
                    //        nc1.Top = 0;
                    //    } 
                    //    pArray[1] = nc1.Location;
                    //    nc1.MouseUp += new MouseEventHandler(nc1_LocationChanged);
                    //    Cls.ControlMover.Init(nc1);
                    //    g.DrawLine(Pens.Silver, pArray[0].X + 15, pArray[0].Y + 25, pArray[1].X + 15, pArray[1].Y + 25);


                    //    UC.NodeChild nc2 = new GSM.UC.NodeChild();
                    //    this.picZoneMap.Controls.Add(nc2);
                    //    nc2._ID = dr["machineNo"].ToString();
                    //    nc2.lblNodeChild.Text = dr["machineNo"].ToString() + "-2区";
                    //    nc2.lblNodeChild.BackColor = Color.White;
                    //    nc2.lblNodeChild.ForeColor = Color.Black;
                    //    nc2._Area = 2;
                    //    nc2.Name = dr["machineNo"].ToString();
                    //    nc2.BringToFront();
                    //    if (dr["worldXY_2"].ToString() != null)
                    //    {
                    //        string[] strXY = dr["worldXY_2"].ToString().Split(',');
                    //        nc2.Left = int.Parse(strXY[0]);
                    //        nc2.Top = int.Parse(strXY[1]);
                    //    }
                    //    else
                    //    {
                    //        nc2.Left = 0;
                    //        nc2.Top = 0;
                    //    } 
                    //    pArray[2] = nc2.Location;
                    //    nc2.MouseUp += new MouseEventHandler(nc1_LocationChanged);
                    //    Cls.ControlMover.Init(nc2);
                    //    g.DrawLine(Pens.Silver, pArray[0].X + 15, pArray[0].Y + 25, pArray[2].X + 15, pArray[2].Y + 25);

                    //    UC.NodeChild nc3 = new GSM.UC.NodeChild();
                    //    this.picZoneMap.Controls.Add(nc3);
                    //    nc3._ID = dr["machineNo"].ToString();
                    //    nc3.lblNodeChild.Text = dr["machineNo"].ToString() + "-3区";
                    //    nc3.lblNodeChild.BackColor = Color.White;
                    //    nc3.lblNodeChild.ForeColor = Color.Black;
                    //    nc3._Area = 3;
                    //    nc3.Name = dr["machineNo"].ToString();
                    //    nc3.BringToFront();
                    //    if (dr["worldXY_3"].ToString() != null)
                    //    {
                    //        string[] strXY = dr["worldXY_3"].ToString().Split(',');
                    //        nc3.Left = int.Parse(strXY[0]);
                    //        nc3.Top = int.Parse(strXY[1]);
                    //    }
                    //    else
                    //    {
                    //        nc3.Left = 0;
                    //        nc3.Top = 0;
                    //    } 
                    //    pArray[3] = nc3.Location;
                    //    nc3.MouseUp += new MouseEventHandler(nc1_LocationChanged);
                    //    Cls.ControlMover.Init(nc3);
                    //    g.DrawLine(Pens.Silver, pArray[0].X + 15, pArray[0].Y + 25, pArray[3].X + 15, pArray[3].Y + 25);

                    //    UC.NodeChild nc4 = new GSM.UC.NodeChild();
                    //    this.picZoneMap.Controls.Add(nc4);
                    //    nc4._ID = dr["machineNo"].ToString();
                    //    nc4.lblNodeChild.Text = dr["machineNo"].ToString() + "-4区";
                    //    nc4.lblNodeChild.BackColor = Color.White;
                    //    nc4.lblNodeChild.ForeColor = Color.Black;
                    //    nc4._Area = 4;
                    //    nc4.Name = dr["machineNo"].ToString();
                    //    nc4.BringToFront();
                    //    if (dr["worldXY_4"].ToString() != null)
                    //    {
                    //        string[] strXY = dr["worldXY_4"].ToString().Split(',');
                    //        nc4.Left = int.Parse(strXY[0]);
                    //        nc4.Top = int.Parse(strXY[1]);
                    //    }
                    //    else
                    //    {
                    //        nc4.Left = 0;
                    //        nc4.Top = 0;
                    //    } 
                    //    pArray[4] = nc4.Location;
                    //    nc4.MouseUp += new MouseEventHandler(nc1_LocationChanged);
                    //    Cls.ControlMover.Init(nc4);
                    //    g.DrawLine(Pens.Silver, pArray[0].X + 15, pArray[0].Y + 25, pArray[4].X + 15, pArray[4].Y + 25);
                    //}

                    this.picZoneMap.Refresh();
                    //ns._A1FireCh1 = int.Parse(xn.ChildNodes[0].Attributes["ch1"].Value);
                    //ns._A1FireCh2 = int.Parse(xn.ChildNodes[0].Attributes["ch2"].Value);
                    //ns._A1FireCh3 = int.Parse(xn.ChildNodes[0].Attributes["ch3"].Value);
                    //ns._A1FireCh4 = int.Parse(xn.ChildNodes[0].Attributes["ch4"].Value);
                    ////ns._A1FireCh5 = int.Parse(xn.ChildNodes[0].Attributes["ch5"].Value);

                    //ns._A2FireCh1 = int.Parse(xn.ChildNodes[1].Attributes["ch1"].Value);
                    //ns._A2FireCh2 = int.Parse(xn.ChildNodes[1].Attributes["ch2"].Value);
                    //ns._A2FireCh3 = int.Parse(xn.ChildNodes[1].Attributes["ch3"].Value);
                    //ns._A2FireCh4 = int.Parse(xn.ChildNodes[1].Attributes["ch4"].Value);
                    ////ns._A2FireCh5 = int.Parse(xn.ChildNodes[1].Attributes["ch5"].Value);

                    //ns._A3FireCh1 = int.Parse(xn.ChildNodes[2].Attributes["ch1"].Value);
                    //ns._A3FireCh2 = int.Parse(xn.ChildNodes[2].Attributes["ch2"].Value);
                    //ns._A3FireCh3 = int.Parse(xn.ChildNodes[2].Attributes["ch3"].Value);
                    //ns._A3FireCh4 = int.Parse(xn.ChildNodes[2].Attributes["ch4"].Value);
                    ////ns._A3FireCh5 = int.Parse(xn.ChildNodes[2].Attributes["ch5"].Value);

                    //ns._AirLowCh1 = int.Parse(xn.ChildNodes[3].Attributes["ch1"].Value);
                    //ns._AirLowCh2 = int.Parse(xn.ChildNodes[3].Attributes["ch2"].Value);
                    //ns._AirLowCh3 = int.Parse(xn.ChildNodes[3].Attributes["ch3"].Value);
                    //ns._AirLowCh4 = int.Parse(xn.ChildNodes[3].Attributes["ch4"].Value);
                    ////ns._AirLowCh5 = int.Parse(xn.ChildNodes[3].Attributes["ch5"].Value);

                    //ns._AirHighCh1 = int.Parse(xn.ChildNodes[4].Attributes["ch1"].Value);
                    //ns._AirHighCh2 = int.Parse(xn.ChildNodes[4].Attributes["ch2"].Value);
                    //ns._AirHighCh3 = int.Parse(xn.ChildNodes[4].Attributes["ch3"].Value);
                    //ns._AirHighCh4 = int.Parse(xn.ChildNodes[4].Attributes["ch4"].Value);
                    //ns._AirHighCh5 = int.Parse(xn.ChildNodes[4].Attributes["ch5"].Value);

                    //tipinfo = "ID:" + ns._CurID + "\r\n";
                    //tipinfo = "设备类型:" + ns._MathineType + "\r\n";
                    //tipinfo += "设备型号:" + ns._MathineXinghao + "\r\n";
                    //tipinfo += "风机转速:" + ns._AirSpeed + "\r\n";
                    //tipinfo += "地理位置:" + ns._Addr + "\r\n";
                    //tipinfo += "\r\n";
                    //tipinfo += "A1级火警阈值通道1:" + ns._A1FireCh1 + "\r\n";
                    //tipinfo += "A1级火警阈值通道2:" + ns._A1FireCh2 + "\r\n";
                    //tipinfo += "A1级火警阈值通道3:" + ns._A1FireCh3 + "\r\n";
                    //tipinfo += "A1级火警阈值通道4:" + ns._A1FireCh4 + "\r\n";
                    ////tipinfo += "A1级火警阈值通道5:" + ns._A1FireCh5 + "\r\n";
                    //tipinfo += "\r\n";
                    //tipinfo += "A2级火警阈值通道1:" + ns._A2FireCh1 + "\r\n";
                    //tipinfo += "A2级火警阈值通道2:" + ns._A2FireCh2 + "\r\n";
                    //tipinfo += "A2级火警阈值通道3:" + ns._A2FireCh3 + "\r\n";
                    //tipinfo += "A2级火警阈值通道4:" + ns._A2FireCh4 + "\r\n";
                    ////tipinfo += "A2级火警阈值通道5:" + ns._A2FireCh5 + "\r\n";
                    //tipinfo += "\r\n";
                    //tipinfo += "A3级火警阈值通道1:" + ns._A3FireCh1 + "\r\n";
                    //tipinfo += "A3级火警阈值通道2:" + ns._A3FireCh2 + "\r\n";
                    //tipinfo += "A3级火警阈值通道3:" + ns._A3FireCh3 + "\r\n";
                    //tipinfo += "A3级火警阈值通道4:" + ns._A3FireCh4 + "\r\n";
                    ////tipinfo += "A3级火警阈值通道5:" + ns._A3FireCh5 + "\r\n";
                    //tipinfo += "\r\n";
                    //tipinfo += "气流低阈值通道1:" + ns._AirLowCh1 + "\r\n";
                    //tipinfo += "气流低阈值通道2:" + ns._AirLowCh2 + "\r\n";
                    //tipinfo += "气流低阈值通道3:" + ns._AirLowCh3 + "\r\n";
                    //tipinfo += "气流低阈值通道4:" + ns._AirLowCh4 + "\r\n";
                    ////tipinfo += "气流低阈值通道5:" + ns._AirLowCh5 + "\r\n";
                    //tipinfo += "\r\n";
                    //tipinfo += "气流高阈值通道1:" + ns._AirHighCh1 + "\r\n";
                    //tipinfo += "气流高阈值通道2:" + ns._AirHighCh2 + "\r\n";
                    //tipinfo += "气流高阈值通道3:" + ns._AirHighCh3 + "\r\n";
                    //tipinfo += "气流高阈值通道4:" + ns._AirHighCh4 + "\r\n";
                    ////tipinfo += "气流高阈值通道5:" + ns._AirHighCh5 + "\r\n";
                    //tipinfo += "\r\n";

                    //ns._ToolTipInfo = tipinfo;
                    //ns.toolTip.SetToolTip(ns, tipinfo);
                    //ns.toolTip.SetToolTip(ns.lblNo, tipinfo);
                    //ns.toolTip.SetToolTip(ns.pbStatus, tipinfo);

                }
            }
            catch (Exception ee) {  }
        }

        void nc1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                UC.NodeChild nc = sender as UC.NodeChild;
                Cls.Win32API.ReleaseCapture();
                //Cls.Win32API.SendMessage(nc.Handle, Cls.Win32API.WM_SYSCOMMAND, Cls.Win32API.SC_MOVE + Cls.Win32API.HTCAPTION, 0); 
                Cls.Win32API.SendMessage(nc.Handle, 161, 2, 0);
                Cls.Win32API.SendMessage(nc.Handle, 0x0202, 0, 0);
            }
            //throw new NotImplementedException();
        }

        void ns_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                UserControls.NodeSite n = sender as UserControls.NodeSite;
                Cls.Win32API.ReleaseCapture();
                //Cls.Win32API.SendMessage(n.Handle, Cls.Win32API.WM_SYSCOMMAND, Cls.Win32API.SC_MOVE + Cls.Win32API.HTCAPTION, 0);
                Cls.Win32API.SendMessage(n.Handle, 161, 2, 0);
                Cls.Win32API.SendMessage(n.Handle, 0x0202, 0, 0);
            }
            //throw new NotImplementedException();
        }

        void nc1_LocationChanged(object sender, EventArgs e)
        {
            UC.NodeChild nc = (UC.NodeChild)sender;
            //try
            //{
                GoDexData.BLL.nodeInfo bllNi = new GoDexData.BLL.nodeInfo();
                GoDexData.Model.nodeInfo modelNi = bllNi.GetModel(int.Parse(nc.Name));
                if (modelNi != null)
                {  
                    switch (nc._Area)
                    {
                        case 1:
                            modelNi.worldXY_1 = nc.Left.ToString() + "," + nc.Top.ToString();
                            break;
                        case 2:
                            modelNi.worldXY_2 = nc.Left.ToString() + "," + nc.Top.ToString();
                            break;
                        case 3:
                            modelNi.worldXY_3 = nc.Left.ToString() + "," + nc.Top.ToString();
                            break;
                        case 4:
                            modelNi.worldXY_4 = nc.Left.ToString() + "," + nc.Top.ToString();
                            break;
                    }

                    //if (modelNi.machineType == 3)
                    //{   
                    //    pArray[nc._Area] = nc.Location; 
                    //    pArray[0] = new Point(int.Parse(strPos[0]), int.Parse(strPos[1]));  
                    //    this.picZoneMap.Image = Image.FromStream(mstream);
                    //    Graphics g = Graphics.FromImage(this.picZoneMap.Image);
                    //    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    //    g.DrawLine(Pens.Silver, pArray[0].X + 15, pArray[0].Y + 25, pArray[1].X + 15, pArray[1].Y + 25);
                    //    g.DrawLine(Pens.Silver, pArray[0].X + 15, pArray[0].Y + 25, pArray[2].X + 15, pArray[2].Y + 25);
                    //    g.DrawLine(Pens.Silver, pArray[0].X + 15, pArray[0].Y + 25, pArray[3].X + 15, pArray[3].Y + 25);
                    //    g.DrawLine(Pens.Silver, pArray[0].X + 15, pArray[0].Y + 25, pArray[4].X + 15, pArray[4].Y + 25);
                    //    this.picZoneMap.Refresh();
                    //    g.Dispose();
                    //}
                    //modelNi.worldXY = pArray[0].X.ToString() + "," + pArray[0].Y.ToString();
                    //modelNi.worldXY_1 = pArray[1].X.ToString() + "," + pArray[1].Y.ToString();
                    //modelNi.worldXY_2 = pArray[2].X.ToString() + "," + pArray[2].Y.ToString();
                    //modelNi.worldXY_3 = pArray[3].X.ToString() + "," + pArray[3].Y.ToString();
                    //modelNi.worldXY_4 = pArray[4].X.ToString() + "," + pArray[4].Y.ToString(); 
                    if (bllNi.Update(modelNi))
                    {
                        readFullMap(picZoneMap);
                        readFullNode();
                    }
                }
               
            //}
            //catch (Exception ee) {// MessageBox.Show(ee.ToString());
            //}
        }

        void ns_LocationChanged(object sender, EventArgs e)
        {
            UserControls.NodeSite ns = (UserControls.NodeSite)sender;
            try
            {  
                GoDexData.BLL.nodeInfo bllNi = new GoDexData.BLL.nodeInfo();
                GoDexData.Model.nodeInfo modelNi = bllNi.GetModel(int.Parse(ns.Name));
                if (modelNi != null)
                {   
                    //xnl.Attributes["fullMapX"].Value = (ns.Left).ToString();
                    //xnl.Attributes["fullMapY"].Value = (ns.Top).ToString();
                    modelNi.worldXY = ns.Left.ToString() + "," + ns.Top.ToString();

                    //if (modelNi.machineType == 3)
                    //{
                    //    pArray[0] = ns.Location;
                    //    this.picZoneMap.Image = Image.FromStream(mstream);
                    //    Graphics g = Graphics.FromImage(this.picZoneMap.Image);
                    //    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    //    g.DrawLine(Pens.Silver, pArray[0].X + 15, pArray[0].Y + 25, pArray[1].X + 15, pArray[1].Y + 25);
                    //    g.DrawLine(Pens.Silver, pArray[0].X + 15, pArray[0].Y + 25, pArray[2].X + 15, pArray[2].Y + 25);
                    //    g.DrawLine(Pens.Silver, pArray[0].X + 15, pArray[0].Y + 25, pArray[3].X + 15, pArray[3].Y + 25);
                    //    g.DrawLine(Pens.Silver, pArray[0].X + 15, pArray[0].Y + 25, pArray[4].X + 15, pArray[4].Y + 25);
                    //    this.picZoneMap.Refresh(); g.Dispose();
                    //}
                    if (bllNi.Update(modelNi))
                    {
                        readFullMap(picZoneMap);
                        readFullNode();
                    }
                }
                
            }
            catch (Exception ee) { //MessageBox.Show(ee.ToString());
            }
            //throw new NotImplementedException();
        }


        private void picZoneMap_MouseMove(object sender, MouseEventArgs e)
        {
            this.toolStripStatusLabel2.Text = e.X.ToString();
            this.toolStripStatusLabel4.Text = e.Y.ToString();
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
