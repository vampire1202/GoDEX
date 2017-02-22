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
    public partial class frmFullAreaMap : Form
    {
        public frmFullAreaMap()
        {
            InitializeComponent();

            WeifenLuo.WinFormsUI.Docking.DockContent dcMap = new WeifenLuo.WinFormsUI.Docking.DockContent();
            dcMap.Text = "地图浏览";
            dcMap.Name = "MapContainer";
            dcMap.HideOnClose = true;
            dcMap.CloseButton = false;
            dcMap.CloseButtonVisible = true;
            dcMap.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.Document;
            //dcMap.Controls.Add(this.axMap1);
            //axMap1.Dock = DockStyle.Fill;
            dcMap.Show(this.dockPanel1, WeifenLuo.WinFormsUI.Docking.DockState.Document);

        }

        //地图原始尺寸
        public System.Drawing.Size _fullMapSize;
        //地图缩放比例
        public double _zoneValue;
        //地图控件
        public System.Drawing.Size _fullNodeSize;
        private string _zoneMapConfigPath;
        public string _ZoneMapConfigPath
        {
            get { return this._zoneMapConfigPath; }
            set { this._zoneMapConfigPath = value; }
        }

        private string _nodeConfigPath;
        public string _NodeConfigPath
        {
            get { return this._nodeConfigPath; }
            set { this._nodeConfigPath = value; }
        }

        //#region ICallback Members
        //void MapWinGIS.ICallback.Error(string KeyOfSender, string ErrorMsg)
        //{
        //    MessageBox.Show(ErrorMsg, "Error");
        //}
        //void MapWinGIS.ICallback.Progress(string KeyOfSender, int Percent, string Message)
        //{
        //    Application.DoEvents(); // Allow the interface to refresh
        //}
        //#endregion
        
        private void frmFullMap_Load(object sender, EventArgs e)
        {
            //读取地图信息
            readFullMap(_zoneMapConfigPath);
            //读取节点信息
            readFullNode(_nodeConfigPath);

           


            //frmTool ftool = new frmTool();
            //ftool.Show(this.dockPanel1,WeifenLuo.WinFormsUI.Docking.DockState.DockTop);
            //this.mapToolStrip1.AxMap = this.axMap1;
            //this.legend1.Map = (MapWinGIS.Map)this.axMap1.GetOcx();
            //legend1.Map.set_LayerName(0, System.IO.Path.GetFileNameWithoutExtension("0"));
            //legend1.Map.set_LayerName(pointHandle, System.IO.Path.GetFileNameWithoutExtension(pointShape.Filename));

            //_fullMapSize = new Size(this.picZoneMap.Size.Width, this.picZoneMap.Size.Height);
            //_fullNodeSize = new Size(40, 36);
            //_zoneValue = 1;
            //this.axMap1.MouseWheelSpeed = 0.0005;
            //this.axMap1.ZoomPercent = 100;
        }

        private void picZoneMap_MouseWheel(object sender, MouseEventArgs e)
        {
            //System.Drawing.Size picSize = picZoneMap.Size;
            //try
            //{
            //    if (e.Delta < 0 && this.picZoneMap.Width > (_fullMapSize.Width / 5))
            //    {
            //        this.picZoneMap.Width = this.picZoneMap.Width * 9 / 10;
            //        this.picZoneMap.Height = this.picZoneMap.Height * 9 / 10;
            //        _zoneValue = _fullMapSize.Width / this.picZoneMap.Width;
            //        foreach (UserControls.NodeSite ns in this.picZoneMap.Controls)
            //        {
            //            ns.Left = ns.Left * 9 / 10;
            //            ns.Top = ns.Top * 9 / 10;
            //            ns.Width = ns.Width * 9 / 10;
            //            ns.Height = ns.Height * 9 / 10;
            //        }
            //    }

            //    if(e.Delta > 0 && this.picZoneMap.Width <(_fullMapSize.Width *5))
            //    {
            //        this.picZoneMap.Width = this.picZoneMap.Width * 11 / 10;
            //        this.picZoneMap.Height = this.picZoneMap.Height * 11 / 10;
            //        _zoneValue = _fullMapSize.Width / this.picZoneMap.Width;
            //        foreach (UserControls.NodeSite ns in this.picZoneMap.Controls)
            //        {
            //            if (ns.Width > 0)
            //            {
            //                ns.Left = ns.Left * 11 / 10;
            //                ns.Top = ns.Top * 11 / 10;
            //                ns.Width = ns.Width * 11 / 10;
            //                ns.Height = ns.Height * 11 / 10;
            //            }
            //        }
            //    }
            //}
            //catch { }
        }

        private void readFullMap(string xmlMapPath)
        {
            try
            {
                //if (this.picZoneMap.Image != null)
                //    this.picZoneMap.Image = null;
                XmlDocument xd = new XmlDocument();
                xd.Load(xmlMapPath);
                XmlNode xn = xd.SelectSingleNode("/Map");
                //MemoryStream ms = null;
                if (xn != null)
                {
                    if (File.Exists(xn.Attributes["path"].Value))
                    {
                        //ms = new MemoryStream();
                        //Image fImg = Image.FromFile(xn.Attributes["path"].Value);
                        //fImg.Save(ms, ImageFormat.Jpeg);
                        //fImg.Dispose();
                        //this.picZoneMap.Image = Image.FromStream(ms);
                        ////MapWinGIS.Image gisImg = new MapWinGIS.Image();
                        ////gisImg.Open(xn.Attributes["path"].Value, MapWinGIS.ImageType.USE_FILE_EXTENSION, true, this);
                        ////this.axMap1.AddLayer(gisImg, true);

                        //        			Bitmap bt = (Bitmap)Image.FromFile(xn.Attributes["imgPath"].Value);
                        //        	        this.picZoneMap.Image = bt;
                        //this.picZoneMap.Width = Image.FromStream(ms).Width;
                        //this.picZoneMap.Height = Image.FromStream(ms).Height;
                        //ms.Dispose();
                        //bt.Dispose();
                    }
                }
            }
            catch (Exception ee) { MessageBox.Show(ee.ToString()); }
        }

        private void readFullNode(string xmlPath)
        {
            try
            {
                string tipinfo = string.Empty;
                XmlDocument xd = new XmlDocument();
                xd.Load(this._nodeConfigPath);
                XmlNodeList xnl = xd.SelectNodes("/nodeGroup/node");
                for(int i=0;i<xnl.Count;i++)
                {
                    UserControls.NodeSite ns = new GSM.UserControls.NodeSite();
                    //ns._FullXY = new Point(int.Parse(xn.Attributes["allPosX"].Value), int.Parse(xn.Attributes["allPosY"].Value));
                    ns.Name = xnl[i].Attributes["ID"].Value;
                    ns.lblNo.Text = xnl[i].Attributes["ID"].Value;
                    ns.AutoSize = false;
                    ns._A1FireCh1 = int.Parse(xnl[i].ChildNodes[0].Attributes["ch1"].Value);
                    ns._A1FireCh2 = int.Parse(xnl[i].ChildNodes[0].Attributes["ch2"].Value);
                    ns._A1FireCh3 = int.Parse(xnl[i].ChildNodes[0].Attributes["ch3"].Value);
                    ns._A1FireCh4 = int.Parse(xnl[i].ChildNodes[0].Attributes["ch4"].Value);
                    ns._A1FireCh5 = int.Parse(xnl[i].ChildNodes[0].Attributes["ch5"].Value);

                    ns._A2FireCh1 = int.Parse(xnl[i].ChildNodes[1].Attributes["ch1"].Value);
                    ns._A2FireCh2 = int.Parse(xnl[i].ChildNodes[1].Attributes["ch2"].Value);
                    ns._A2FireCh3 = int.Parse(xnl[i].ChildNodes[1].Attributes["ch3"].Value);
                    ns._A2FireCh4 = int.Parse(xnl[i].ChildNodes[1].Attributes["ch4"].Value);
                    ns._A2FireCh5 = int.Parse(xnl[i].ChildNodes[1].Attributes["ch5"].Value);

                    ns._A3FireCh1 = int.Parse(xnl[i].ChildNodes[2].Attributes["ch1"].Value);
                    ns._A3FireCh2 = int.Parse(xnl[i].ChildNodes[2].Attributes["ch2"].Value);
                    ns._A3FireCh3 = int.Parse(xnl[i].ChildNodes[2].Attributes["ch3"].Value);
                    ns._A3FireCh4 = int.Parse(xnl[i].ChildNodes[2].Attributes["ch4"].Value);
                    ns._A3FireCh5 = int.Parse(xnl[i].ChildNodes[2].Attributes["ch5"].Value);

                    ns._AirLowCh1 = int.Parse(xnl[i].ChildNodes[3].Attributes["ch1"].Value);
                    ns._AirLowCh2 = int.Parse(xnl[i].ChildNodes[3].Attributes["ch2"].Value);
                    ns._AirLowCh3 = int.Parse(xnl[i].ChildNodes[3].Attributes["ch3"].Value);
                    ns._AirLowCh4 = int.Parse(xnl[i].ChildNodes[3].Attributes["ch4"].Value);
                    ns._AirLowCh5 = int.Parse(xnl[i].ChildNodes[3].Attributes["ch5"].Value);

                    ns._AirHighCh1 = int.Parse(xnl[i].ChildNodes[4].Attributes["ch1"].Value);
                    ns._AirHighCh2 = int.Parse(xnl[i].ChildNodes[4].Attributes["ch2"].Value);
                    ns._AirHighCh3 = int.Parse(xnl[i].ChildNodes[4].Attributes["ch3"].Value);
                    ns._AirHighCh4 = int.Parse(xnl[i].ChildNodes[4].Attributes["ch4"].Value);
                    ns._AirHighCh5 = int.Parse(xnl[i].ChildNodes[4].Attributes["ch5"].Value);

                    ns._MathineType = xnl[i].Attributes["mathineType"].Value;
                    ns._Addr = xnl[i].Attributes["address"].Value;
                    ns._MathineXinghao = xnl[i].Attributes["mathineXinghao"].Value;
                    ns._AirSpeed = xnl[i].Attributes["airSpeed"].Value;
                    ns._CurID = xnl[i].Attributes["ID"].Value;
                    ns.Left = int.Parse(xnl[i].Attributes["fullMapX"].Value);
                    ns.Top = int.Parse(xnl[i].Attributes["fullMapY"].Value);
                    //tipinfo = "ID:" + ns._CurID + "\r\n";
                    tipinfo = "设备类型:" + ns._MathineType + "\r\n";
                    tipinfo += "设备型号:" + ns._MathineXinghao + "\r\n";
                    tipinfo += "风机转速:" + ns._AirSpeed + "\r\n";
                    tipinfo += "地理位置:" + ns._Addr + "\r\n";
                    tipinfo += "\r\n";
                    tipinfo += "A1级火警阈值通道1:" + ns._A1FireCh1 + "\r\n";
                    tipinfo += "A1级火警阈值通道2:" + ns._A1FireCh2 + "\r\n";
                    tipinfo += "A1级火警阈值通道3:" + ns._A1FireCh3 + "\r\n";
                    tipinfo += "A1级火警阈值通道4:" + ns._A1FireCh4 + "\r\n";
                    tipinfo += "A1级火警阈值通道5:" + ns._A1FireCh5 + "\r\n";
                    tipinfo += "\r\n";
                    tipinfo += "A2级火警阈值通道1:" + ns._A2FireCh1 + "\r\n";
                    tipinfo += "A2级火警阈值通道2:" + ns._A2FireCh2 + "\r\n";
                    tipinfo += "A2级火警阈值通道3:" + ns._A2FireCh3 + "\r\n";
                    tipinfo += "A2级火警阈值通道4:" + ns._A2FireCh4 + "\r\n";
                    tipinfo += "A2级火警阈值通道5:" + ns._A2FireCh5 + "\r\n";
                    tipinfo += "\r\n";
                    tipinfo += "A3级火警阈值通道1:" + ns._A3FireCh1 + "\r\n";
                    tipinfo += "A3级火警阈值通道2:" + ns._A3FireCh2 + "\r\n";
                    tipinfo += "A3级火警阈值通道3:" + ns._A3FireCh3 + "\r\n";
                    tipinfo += "A3级火警阈值通道4:" + ns._A3FireCh4 + "\r\n";
                    tipinfo += "A3级火警阈值通道5:" + ns._A3FireCh5 + "\r\n";
                    tipinfo += "\r\n";
                    tipinfo += "气流低阈值通道1:" + ns._AirLowCh1 + "\r\n";
                    tipinfo += "气流低阈值通道2:" + ns._AirLowCh2 + "\r\n";
                    tipinfo += "气流低阈值通道3:" + ns._AirLowCh3 + "\r\n";
                    tipinfo += "气流低阈值通道4:" + ns._AirLowCh4 + "\r\n";
                    tipinfo += "气流低阈值通道5:" + ns._AirLowCh5 + "\r\n";
                    tipinfo += "\r\n";
                    tipinfo += "气流高阈值通道1:" + ns._AirHighCh1 + "\r\n";
                    tipinfo += "气流高阈值通道2:" + ns._AirHighCh2 + "\r\n";
                    tipinfo += "气流高阈值通道3:" + ns._AirHighCh3 + "\r\n";
                    tipinfo += "气流高阈值通道4:" + ns._AirHighCh4 + "\r\n";
                    tipinfo += "气流高阈值通道5:" + ns._AirHighCh5 + "\r\n";
                    tipinfo += "\r\n";

                    //ns._ToolTipInfo = tipinfo;
                    //ns.toolTip.SetToolTip(ns, tipinfo);
                    ns.toolTip.SetToolTip(ns.lblNo, tipinfo);
                    ns.toolTip.SetToolTip(ns.pbStatus, tipinfo);
                    ns.BringToFront();
                    ns.LocationChanged += new EventHandler(ns_LocationChanged);
                                      
                    //this.axMap1.AddLayer(lbl, true);
                    //this.picZoneMap.Controls.Add(ns);
                }
            }
            catch (Exception ee) { MessageBox.Show(ee.ToString()); }
        }

        void ns_LocationChanged(object sender, EventArgs e)
        {
            UserControls.NodeSite ns = (UserControls.NodeSite)sender;
            try
            {
                XmlDocument xd = new XmlDocument();
                xd.Load(this._nodeConfigPath);
                XmlNode xnl = xd.SelectSingleNode("/nodeGroup/node[@ID='" + ns._CurID + "']");
                if (xnl != null)
                {
                    xnl.Attributes["fullMapX"].Value = (ns.Left * _zoneValue).ToString();
                    xnl.Attributes["fullMapY"].Value = (ns.Top * _zoneValue).ToString();
                }
                xd.Save(this._nodeConfigPath);
            }
            catch (Exception ee) { MessageBox.Show(ee.ToString()); }
            //throw new NotImplementedException();
        }

        private void picZoneMap_MouseMove(object sender, MouseEventArgs e)
        {
            this.toolStripStatusLabel2.Text = e.X.ToString();
            this.toolStripStatusLabel4.Text = e.Y.ToString();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void tsbSave_Click(object sender, EventArgs e)
        {

        }
    }
}
