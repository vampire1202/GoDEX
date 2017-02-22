using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;
using System.Security.Cryptography;
using System.Timers;
using Microsoft.Win32;
using System.Diagnostics;


namespace GSM
{
    public partial class frmMain : Form
    {
        public SerialPort _Comm;//定义串口
        public SerialPort _CommBack;
        private Forms.frmWarningMap formWaringMap = null;
        private delegate void autoCreateNode(FlowLayoutPanel flp, string nodeID, RichTextBox rt, ListView warningInfo, Byte[] orderItem, string tipInfo, ContextMenuStrip cMenu, string userRole);
        private delegate void askAireFireSet(SerialPort _serialPort, Byte data1, Byte data2, string nodeNo, string tipText, RichTextBox rtb);
        private delegate void askAireFireValue(SerialPort _serialPort, Byte data1, Byte data2, string nodeNo, string tipText, RichTextBox rtb, FlowLayoutPanel flp, int arrIndex);
        Cls.ReadComm.CommReader readerBack;
        //登陆者身份
        public string _userCode = string.Empty;
        public string _userName = string.Empty;
        public string _userRole = string.Empty;
        public int order = 0;
        //定义语音报警
        public Cls.Speach alarmSpeaker = Cls.Speach.instance();
        public System.Timers.Timer lxtimer;
        private WeifenLuo.WinFormsUI.Docking.DockContent dc_Map;
        private WeifenLuo.WinFormsUI.Docking.DockContent dc_FindLog;
        private WeifenLuo.WinFormsUI.Docking.DockContent dc_AddrSet;
        private WeifenLuo.WinFormsUI.Docking.DockContent dc_zoneMap;
        private WeifenLuo.WinFormsUI.Docking.DockContent dc_Curve;

        //ID数组的序号 
        private static int[] m_arrayID = null;
        private int m_arrayIDIndex;
        //判断是否接收成功
        public Boolean isSendSuccess = false;
        //隔离标志
        private int isGeli = 0;
        private int isGeliTemp = 0;
        //是否在线标志
        private bool m_isOnline = false;
        private int m_currentID;
        //发命令参数
        int m_inde = 0;
        int m_SendTimes = 0;
        int m_orderTime = 200;
        string[] m_arrSN = new string[4];
        bool m_Closeing = false;
        bool m_Listening = false;
        Thread threadlx;
        delegate void SetZeroValue(FlowLayoutPanel flp, Cls.Speach sp);
        public frmMain(string userRole, string userCode, string userName)
        {
            InitializeComponent();
            this._userRole = userRole;
            this._userCode = userCode;
            this._userName = userName;
            //布局界面
            //dockPanel元素
            //1、左侧 Legend ，显示 地图上各节点位置的 shape 列表
            WeifenLuo.WinFormsUI.Docking.DockContent dc_nodeTree = new WeifenLuo.WinFormsUI.Docking.DockContent();
            dc_nodeTree.Text = "节点信息";
            dc_nodeTree.Name = "LegendContainer";
            dc_nodeTree.HideOnClose = true;
            dc_nodeTree.CloseButton = false;
            dc_nodeTree.CloseButtonVisible = false;
            dc_nodeTree.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft;
            panel1.Dock = DockStyle.Fill;
            dc_nodeTree.Controls.Add(this.panel1);
            this.treeViewNodes.Dock = DockStyle.Fill;
            dc_nodeTree.Show(this.dockPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockLeftAutoHide);

            //2、底部的日志和报警信息
            WeifenLuo.WinFormsUI.Docking.DockContent dc_Log = new WeifenLuo.WinFormsUI.Docking.DockContent();
            dc_Log.Text = "日志与报警信息";
            dc_Log.Name = "LogContainer";
            dc_Log.HideOnClose = true;
            dc_Log.CloseButton = false;
            dc_Log.CloseButtonVisible = false;
            dc_Log.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom;
            dc_Log.Controls.Add(this.customTabControl1);
            this.customTabControl1.Dock = DockStyle.Fill;
            dc_Log.Show(this.dockPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockBottomAutoHide);

            //3、主页面的节点列表
            WeifenLuo.WinFormsUI.Docking.DockContent dc_NodeList = new WeifenLuo.WinFormsUI.Docking.DockContent();
            dc_NodeList.Text = "节点列表";
            dc_NodeList.Name = "NodeListContainer";
            dc_NodeList.HideOnClose = true;
            dc_NodeList.CloseButton = false;
            dc_NodeList.CloseButtonVisible = false;
            dc_NodeList.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.Document;
            dc_NodeList.Controls.Add(this.flowLayoutPanel1);
            this.flowLayoutPanel1.Dock = DockStyle.Fill;
            dc_NodeList.Show(this.dockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
        }

        #region ICallback Members
        //void MapWinGIS.ICallback.Error(string KeyOfSender, string ErrorMsg)
        //{
        //    MessageBox.Show(ErrorMsg, "Error");
        //}
        //void MapWinGIS.ICallback.Progress(string KeyOfSender, int Percent, string Message)
        //{
        //    Application.DoEvents(); // Allow the interface to refresh
        //}
        #endregion

        /// <summary>
        /// 获取权限功能 
        /// </summary>
        /// <param name="userRole"></param>
        private void getUserRole(string userRole, string userName, string userCode)
        {
            switch (userRole)
            {
                case "Administrator":
                    this.tsmiPortSet.Enabled = true;
                    this.tsmiVoiseSet.Enabled = true;
                    this.tsmiUsers.Enabled = true;

                    this.tsmiItem.Enabled = true;
                    this.tsbtnSetAddress.Enabled = true;
                    this.tsmiUsers.Enabled = true;
                    this.contextMenuStrip1.Enabled = true;
                    this.tsBtnReadHistory.Enabled = true;
                    this.tsmiUserRole.Enabled = true;
                    this.tsslblUserRole.Text = userCode + "(" + userName + ") 身份:生产商";
                    this._userRole = userCode;
                    break;
                case "Agency":
                    this.tsmiPortSet.Enabled = true;
                    this.tsmiVoiseSet.Enabled = true;
                    this.tsmiUsers.Enabled = true;

                    this.tsmiUserRole.Enabled = true;
                    this.tsBtnReadHistory.Enabled = true;
                    this.tsmiUsers.Enabled = true;
                    this.tsbtnSetAddress.Enabled = true;
                    this.tsmiItem.Enabled = true;
                    this.tsbtnSetAddress.Enabled = true;
                    this.contextMenuStrip1.Enabled = true;
                    this.tsslblUserRole.Text = userCode + "(" + userName + ") 身份:代理商";
                    this._userRole = userCode;
                    break;
                case "Manager":
                    this.tsmiPortSet.Enabled = false;
                    this.tsmiVoiseSet.Enabled = false;
                    this.tsmiUsers.Enabled = false;

                    this.tsmiUserRole.Enabled = false;
                    this.tsBtnReadHistory.Enabled = true;
                    this.tsmiUsers.Enabled = false;
                    this.tsbtnSetAddress.Enabled = true;
                    this.tsmiItem.Enabled = true;
                    this.tsbtnSetAddress.Enabled = true;
                    this.contextMenuStrip1.Enabled = true;
                    this.tsslblUserRole.Text = userCode + "(" + userName + ") 身份:管理员";
                    this._userRole = userCode;
                    break;
                case "Users":
                    this.tsmiPortSet.Enabled = false;
                    this.tsmiVoiseSet.Enabled = false;
                    this.tsmiUsers.Enabled = false;

                    this.tsmiItem.Enabled = false;
                    this.tsbtnSetAddress.Enabled = false;
                    this.tsmiUsers.Enabled = false;
                    this.contextMenuStrip1.Enabled = false;
                    this.tsBtnReadHistory.Enabled = false;
                    this.tsslblUserRole.Text = userCode + "(" + userName + ") 身份:操作员";
                    this._userRole = userCode;
                    break;
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            getUserRole(this._userRole, this._userName, this._userCode);
            m_arrayID = arrOrderID();
            try
            {
                if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "Curve"))
                {
                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "Curve");
                }
                string strPortName = string.Empty;
                ////轮询Timer
                //lxtimer = new System.Timers.Timer();
                ////读取轮询时间间隔
                //lxtimer.Elapsed += new ElapsedEventHandler(lxTimeEvent);
                //lxtimer.Enabled = false;

                //初始化语音警报
                alarmSpeaker.Rate = 1;//语速
                alarmSpeaker.Volume = 100;//声音强度

                //通讯串口参数设置
                _Comm = new SerialPort();
                _Comm.BaudRate = int.Parse(Cls.RWconfig.GetAppSettings("RateMain"));
                _Comm.Parity = Parity.None; //Parity为C#预定义枚举型变量
                _Comm.DataBits = 8;
                _Comm.StopBits = StopBits.One; //StopBits为C#预定义的枚举量
                _Comm.RtsEnable = false;
                //_Comm.PortName = Cls.RWconfig.GetAppSettings("PortName");//串口名为COM1
                _Comm.ReadBufferSize = 2048;
                _Comm.WriteBufferSize = 2048;
                _Comm.DtrEnable = true;

                //回路串口
                _CommBack = new SerialPort();
                _CommBack.BaudRate = int.Parse(Cls.RWconfig.GetAppSettings("RateBack"));
                _Comm.Parity = Parity.None; //Parity为C#预定义枚举型变量
                _CommBack.DataBits = 8;
                _CommBack.StopBits = StopBits.One; //StopBits为C#预定义的枚举量
                _CommBack.RtsEnable = false;
                //_CommBack.PortName = Cls.RWconfig.GetAppSettings("PortNameBack");//串口名为COMBack
                _CommBack.DtrEnable = true;
                _CommBack.ReadBufferSize = 2048;
                _CommBack.WriteBufferSize = 2048;

                readerBack = new Cls.ReadComm.CommReader(_CommBack, 100);
                readerBack.Handlers += new GSM.Cls.ReadComm.CommReader.HandleCommData(HandleCommData);

                foreach (string spName in SerialPort.GetPortNames())
                {
                    strPortName += spName + ",";
                }

                if (strPortName.Contains(Cls.RWconfig.GetAppSettings("PortName")) & (!string.IsNullOrEmpty(Cls.RWconfig.GetAppSettings("PortName"))))
                {
                    Initsp1();
                }
                else
                {
                    MessageBox.Show("串口" + Cls.RWconfig.GetAppSettings("PortName") + "不存在");
                }

                if (strPortName.Contains(Cls.RWconfig.GetAppSettings("PortNameBack")) & (!string.IsNullOrEmpty(Cls.RWconfig.GetAppSettings("PortNameBack"))))
                {
                    Initsp2();
                    readerBack.Start();
                }
                else
                {
                    MessageBox.Show("串口" + Cls.RWconfig.GetAppSettings("PortNameBack") + "不存在");
                }
                //初始化警报信息栏
                CreateWarningList();
                //导入节点配置文件,生成节点列表         
                Task t = TaskCreateNodesView();
                //GoDexData.BLL.nodeInfo bllNodeInfo = new GoDexData.BLL.nodeInfo();
                //dsNodeInfo = bllNodeInfo.GetAllList();
                //int count = dsNodeInfo.Tables[0].Rows.Count; 
                //Parallel.For(0, count, (i) => { CreateNodeView(i); });
                CreateNodeTree(this.treeViewNodes);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
        DataSet dsNodeInfo;
        void CreateNodeView(int i)
        {
            this.BeginInvoke(new Action(() =>
            {

                UserControls.Node newNode = new GSM.UserControls.Node();
                newNode.Name = dsNodeInfo.Tables[0].Rows[i]["machineNo"].ToString();
                newNode.NodeNo = dsNodeInfo.Tables[0].Rows[i]["machineNo"].ToString();
                newNode.Tag = "0";
                switch (dsNodeInfo.Tables[0].Rows[i]["machineType"].ToString())
                {
                    case "1":
                        newNode.Addr = "设备类型:单管单区";
                        newNode.lblAire2.Enabled = false;
                        //newNode.lblAire2.Visible = false;
                        newNode.lblAire2.BackColor = Color.Gainsboro;
                        newNode.lblAire3.Enabled = false;
                        //newNode.lblAire3.Visible = false;
                        newNode.lblAire3.BackColor = Color.Gainsboro;
                        newNode.lblAire4.Enabled = false;
                        //newNode.lblAire4.Visible = false;
                        newNode.lblAire4.BackColor = Color.Gainsboro;
                        newNode.lblFire2.Enabled = false;
                        //newNode.lblFire2.Visible = false;
                        newNode.lblFire2.BackColor = Color.Gainsboro;
                        newNode.lblFire3.Enabled = false;
                        //newNode.lblFire3.Visible = false;
                        newNode.lblFire3.BackColor = Color.Gainsboro;
                        newNode.lblFire4.Enabled = false;
                        //newNode.lblFire4.Visible = false;
                        newNode.lblFire4.BackColor = Color.Gainsboro;
                        break;
                    case "2":
                        newNode.Addr = "设备类型:四管单区";
                        newNode.lblFire2.Enabled = false;
                        //newNode.lblFire2.Visible = false;
                        newNode.lblFire2.BackColor = Color.Gainsboro;
                        newNode.lblFire3.Enabled = false;
                        //newNode.lblFire3.Visible = false;
                        newNode.lblFire3.BackColor = Color.Gainsboro;
                        newNode.lblFire4.Enabled = false;
                        //newNode.lblFire4.Visible = false;
                        newNode.lblFire4.BackColor = Color.Gainsboro;
                        break;
                    case "3":
                        newNode.Addr = "设备类型:四管四区";
                        break;
                    default:
                        break;
                }
                newNode.ContextMenuStrip = this.contextMenuStrip1;
                this.flowLayoutPanel1.Controls.Add(newNode);
                this.flowLayoutPanel1.Refresh();
                //this.flowLayoutPanel1.Controls.Add(ni); 
                this.Enabled = true;
                dsNodeInfo.Dispose();
                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                richTextBox1.Focus();
            }));
        }

        async Task TaskCreateNodesView()
        {

            this.toolStripMain.Enabled = false;
            this.dockPanel.Enabled = false;
            GoDexData.BLL.nodeInfo bllNodeInfo = new GoDexData.BLL.nodeInfo();
            DataSet dsNodeInfo = bllNodeInfo.GetAllList();
            int count = dsNodeInfo.Tables[0].Rows.Count;
            for (int i = 0; i < count; i++)
            {
                UserControls.Node newNode = new GSM.UserControls.Node();
                newNode.Name = dsNodeInfo.Tables[0].Rows[i]["machineNo"].ToString();
                newNode.NodeNo = dsNodeInfo.Tables[0].Rows[i]["machineNo"].ToString();
                newNode.Tag = "1";
                switch (dsNodeInfo.Tables[0].Rows[i]["machineType"].ToString())
                {
                    case "1":
                        newNode.Addr = "设备类型:单管单区";
                        newNode.lblAire2.Enabled = false;
                        newNode.lblAire2.BackColor = Color.Gainsboro;
                        newNode.lblAire3.Enabled = false;
                        newNode.lblAire3.BackColor = Color.Gainsboro;
                        newNode.lblAire4.Enabled = false;
                        newNode.lblAire4.BackColor = Color.Gainsboro;
                        newNode.lblFire2.Enabled = false;
                        newNode.lblFire2.BackColor = Color.Gainsboro;
                        newNode.lblFire3.Enabled = false;
                        newNode.lblFire3.BackColor = Color.Gainsboro;
                        newNode.lblFire4.Enabled = false;
                        newNode.lblFire4.BackColor = Color.Gainsboro;
                        break;
                    case "2":
                        newNode.Addr = "设备类型:四管单区";
                        newNode.lblFire2.Enabled = false;
                        newNode.lblFire2.BackColor = Color.Gainsboro;
                        newNode.lblFire3.Enabled = false;
                        newNode.lblFire3.BackColor = Color.Gainsboro;
                        newNode.lblFire4.Enabled = false;
                        newNode.lblFire4.BackColor = Color.Gainsboro;
                        break;
                    case "3":
                        newNode.Addr = "设备类型:四管四区";
                        break;
                    default:
                        break;
                }
                newNode.ContextMenuStrip = this.contextMenuStrip1;
                this.flowLayoutPanel1.Controls.Add(newNode);
                this.flowLayoutPanel1.Refresh();
                Trace.WriteLine("添加Node" + newNode.nodeNo.ToString());
            }
            this.toolStripMain.Enabled = true;
            this.dockPanel.Enabled = true;
            dsNodeInfo.Dispose();
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.Focus();
            await Task.Delay(50).ConfigureAwait(false);
        }


        /// <summary>
        /// 执行轮询以及判断节点是否离线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lxTimeEvent(object sender, ElapsedEventArgs e)
        {
            //开启发送命令线程
            threadlx = new Thread(new ParameterizedThreadStart(sendlx));
            threadlx.IsBackground = true;
            threadlx.Start(m_arrayID);
            threadlx = null;
        }

        private void dlgSetZeroValue(FlowLayoutPanel flp, Cls.Speach sp)
        {
            //lock (this)
            //{
            //轮询前所有节点烟雾值和气流值清零
            foreach (UserControls.Node node in flp.Controls)
            {
                if (node.Tag.ToString() == "0")
                {
                    node.lblOnline.BackColor = Color.Red;
                    node.lblOnline.Text = "离线";
                    sp.AnalyseSpeak("节点" + node.NodeNo + ":通讯故障,请检查线路和设备运行情况!");
                }
                else
                {
                    node.lblOnline.BackColor = Color.LightCyan;
                    node.lblOnline.Text = "在线";
                }
                node.Tag = "0";
                Thread.Sleep(50);
                //node.Refresh();
            }
            //}
        }

        bool isRequireStop = false;
        private void sendlx(object arrID)
        {
            int[] arr = (int[])arrID;
            int count = arr.Length;
            Thread.Sleep(10);
            while (m_arrayIDIndex < count && !isRequireStop)
            {
                m_isOnline = false;
                m_currentID = arr[m_arrayIDIndex];
                if (m_SendTimes == 0)
                    this.BeginInvoke(new askAireFireValue(RequireData), new object[] { this._Comm, Convert.ToByte(0xAB), Convert.ToByte(0x00), arr[m_arrayIDIndex].ToString(), " 烟雾值[" + DateTime.Now.ToString() + "]", this.richTextBox1, this.flowLayoutPanel1, m_arrayIDIndex });
                Thread.Sleep(m_orderTime);
                m_SendTimes++;
                //判断在线标志是否置1，如果是，则继续，如果不是，则重发命令；
                if (m_isOnline)
                {
                    m_arrayIDIndex++;
                    m_SendTimes = 0;
                }
                else if (m_SendTimes == 3)
                {
                    m_arrayIDIndex++;
                    m_SendTimes = 0;
                }
            }
            m_arrayIDIndex = 0;
            Thread.Sleep(100);
            m_inde++;
            //轮询两遍后检测节点是否在线
            if (m_inde == 2)
            {
                this.BeginInvoke(new SetZeroValue(dlgSetZeroValue), new object[] { this.flowLayoutPanel1, this.alarmSpeaker });
                m_inde = 0;
            }
        }



        private void SetNodeStateVoid(FlowLayoutPanel flp, int index)
        {
            Control[] cc = flp.Controls.Find(m_arrayID[index].ToString(), false);
            UserControls.Node _node;
            if (cc.Length > 0)
            {
                _node = (UserControls.Node)cc[0];

                ////判断前一个节点是否回数据
                if (_node.Firevalue1 != 0 || _node.Airevalue1 != 0)
                {
                    _node.lblOnline.BackColor = Color.LightCyan;
                    _node.lblOnline.Text = "在线";
                    _node.lblOnline.Refresh();
                }
                else
                {
                    _node.lblOnline.BackColor = Color.Red;
                    _node.lblOnline.Text = "离线";
                    _node.lblOnline.Refresh();
                }
                _node.Refresh();
            }
        }


        /// <summary>
        /// 根据节点配置文件生成节点列表
        /// </summary>
        /// <param name="xmlPath">节点配置文件路径</param>
        private void CreateNodesView()
        {
            try
            {
                GoDexData.BLL.nodeInfo bllNodeInfo = new GoDexData.BLL.nodeInfo();
                DataSet dsNodeInfo = bllNodeInfo.GetAllList();
                int count = dsNodeInfo.Tables[0].Rows.Count;
                for (int i = 0; i < count; i++)
                {
                    //UserControls.NodeInfo ni = new UserControls.NodeInfo();
                    UserControls.Node newNode = new GSM.UserControls.Node();
                    newNode.Name = dsNodeInfo.Tables[0].Rows[i]["machineNo"].ToString();
                    newNode.NodeNo = dsNodeInfo.Tables[0].Rows[i]["machineNo"].ToString();
                    newNode.Tag = "1";
                    switch (dsNodeInfo.Tables[0].Rows[i]["machineType"].ToString())
                    {
                        case "1":
                            newNode.Addr = "设备类型:单管单区";
                            newNode.lblAire2.Enabled = false;
                            //newNode.lblAire2.Visible = false;
                            newNode.lblAire2.BackColor = Color.Gainsboro;
                            newNode.lblAire3.Enabled = false;
                            //newNode.lblAire3.Visible = false;
                            newNode.lblAire3.BackColor = Color.Gainsboro;
                            newNode.lblAire4.Enabled = false;
                            //newNode.lblAire4.Visible = false;
                            newNode.lblAire4.BackColor = Color.Gainsboro;
                            newNode.lblFire2.Enabled = false;
                            //newNode.lblFire2.Visible = false;
                            newNode.lblFire2.BackColor = Color.Gainsboro;
                            newNode.lblFire3.Enabled = false;
                            //newNode.lblFire3.Visible = false;
                            newNode.lblFire3.BackColor = Color.Gainsboro;
                            newNode.lblFire4.Enabled = false;
                            //newNode.lblFire4.Visible = false;
                            newNode.lblFire4.BackColor = Color.Gainsboro;
                            break;
                        case "2":
                            newNode.Addr = "设备类型:四管单区";
                            newNode.lblFire2.Enabled = false;
                            //newNode.lblFire2.Visible = false;
                            newNode.lblFire2.BackColor = Color.Gainsboro;
                            newNode.lblFire3.Enabled = false;
                            //newNode.lblFire3.Visible = false;
                            newNode.lblFire3.BackColor = Color.Gainsboro;
                            newNode.lblFire4.Enabled = false;
                            //newNode.lblFire4.Visible = false;
                            newNode.lblFire4.BackColor = Color.Gainsboro;
                            break;
                        case "3":
                            newNode.Addr = "设备类型:四管四区";
                            break;
                        default:
                            break;
                    }

                    newNode.ContextMenuStrip = this.contextMenuStrip1;
                    this.flowLayoutPanel1.Controls.Add(newNode);
                    //this.flowLayoutPanel1.Controls.Add(ni);
                }
                dsNodeInfo.Dispose();
                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                richTextBox1.Focus();
            }
            catch (Exception ee)
            {
                //MessageBox.Show("CreateNodesView" + ee.ToString());
                return;
            }
        }

        public static void CreateNodeTree(TreeView tv)
        {
            try
            {
                tv.Nodes.Clear();
                //获取全部节点列表
                GoDexData.BLL.nodeInfo bllNi = new GoDexData.BLL.nodeInfo();
                DataSet ds = bllNi.GetAllList();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    TreeNode tnc = new TreeNode();
                    //获取某节点 设置信息表
                    GoDexData.BLL.nodeSet bllNs = new GoDexData.BLL.nodeSet();
                    //GoDexData.Model.nodeSet modelNs = bllNs.GetModel(int.Parse(ds.Tables[0].Rows[i]["machineNo"].ToString()));
                    DataSet dsNs = bllNs.GetList(" machineNo ='" + ds.Tables[0].Rows[i]["machineNo"].ToString() + "'");
                    switch (ds.Tables[0].Rows[i]["machineType"].ToString())
                    {

                        case "1": //"单管单区":
                            tnc.Text = "设备:" + ds.Tables[0].Rows[i]["machineNo"].ToString() + "-单管单区  位置:" + ds.Tables[0].Rows[i]["sign"].ToString();
                            tnc.Name = ds.Tables[0].Rows[i]["machineNo"].ToString();
                            tnc.Nodes.Add("气流低阈值:" + dsNs.Tables[0].Rows[0]["airflowL_pipe1"].ToString());
                            tnc.Nodes.Add("气流高阈值:" + dsNs.Tables[0].Rows[0]["airflowH_pipe1"].ToString());
                            tnc.Nodes.Add("一级火警阈值:" + dsNs.Tables[0].Rows[0]["fireA1_area1"].ToString());
                            tnc.Nodes.Add("二级火警阈值:" + dsNs.Tables[0].Rows[0]["fireA2_area1"].ToString());
                            tnc.Nodes.Add("三级火警阈值:" + dsNs.Tables[0].Rows[0]["fireA3_area1"].ToString());
                            tnc.Nodes.Add("四级火警阈值:" + dsNs.Tables[0].Rows[0]["fireA4_area1"].ToString());
                            break;

                        case "2":// "四管单区":
                            tnc.Text = "设备:" + ds.Tables[0].Rows[i]["machineNo"].ToString() + "-四管单区  位置:" + ds.Tables[0].Rows[i]["sign"].ToString();
                            tnc.Name = ds.Tables[0].Rows[i]["machineNo"].ToString();
                            for (int j = 1; j <= 4; j++)
                            {
                                tnc.Nodes.Add("管" + j.ToString());
                                tnc.Nodes[j - 1].Nodes.Add("气流高阈值:" + dsNs.Tables[0].Rows[0][2 * j + 1].ToString());
                                tnc.Nodes[j - 1].Nodes.Add("气流低阈值:" + dsNs.Tables[0].Rows[0][2 * j + 2].ToString());
                            }
                            tnc.Nodes.Add("一级火警阈值:" + dsNs.Tables[0].Rows[0]["fireA1_area1"].ToString());
                            tnc.Nodes.Add("二级火警阈值:" + dsNs.Tables[0].Rows[0]["fireA2_area1"].ToString());
                            tnc.Nodes.Add("三级火警阈值:" + dsNs.Tables[0].Rows[0]["fireA3_area1"].ToString());
                            tnc.Nodes.Add("四级火警阈值:" + dsNs.Tables[0].Rows[0]["fireA4_area1"].ToString());
                            break;

                        case "3"://"四管四区":
                            tnc.Text = "设备:" + ds.Tables[0].Rows[i]["machineNo"].ToString() + "-四管四区  位置:" + ds.Tables[0].Rows[i]["sign"].ToString();
                            tnc.Name = ds.Tables[0].Rows[i]["machineNo"].ToString();
                            for (int k = 1; k <= 4; k++)
                            {
                                tnc.Nodes.Add("区" + k.ToString());
                                tnc.Nodes[k - 1].Nodes.Add("气流高阈值:" + dsNs.Tables[0].Rows[0][2 * k + 1].ToString());
                                tnc.Nodes[k - 1].Nodes.Add("气流低阈值:" + dsNs.Tables[0].Rows[0][2 * k + 2].ToString());
                                tnc.Nodes[k - 1].Nodes.Add("一级火警阈值:" + dsNs.Tables[0].Rows[0][3 * k + 8].ToString());
                                tnc.Nodes[k - 1].Nodes.Add("二级火警阈值:" + dsNs.Tables[0].Rows[0][3 * k + 9].ToString());
                                tnc.Nodes[k - 1].Nodes.Add("三级火警阈值:" + dsNs.Tables[0].Rows[0][3 * k + 10].ToString());
                                tnc.Nodes[k - 1].Nodes.Add("四级火警阈值:" + dsNs.Tables[0].Rows[0][3 * k + 11].ToString());
                            }
                            break;
                        default:
                            break;
                    }
                    dsNs.Dispose();
                    tv.Nodes.Add(tnc);
                }
                ds.Dispose();
            }
            catch (Exception ee)
            {
                //MessageBox.Show("CreateNodeTree:" + ee.ToString());
                return;
            }
        }

        public void CreateNodeTree()
        {
            try
            {
                this.treeViewNodes.Nodes.Clear();
                //获取全部节点列表
                GoDexData.BLL.nodeInfo bllNi = new GoDexData.BLL.nodeInfo();
                DataSet ds = bllNi.GetAllList();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    TreeNode tnc = new TreeNode();
                    //获取某节点 设置信息表
                    GoDexData.BLL.nodeSet bllNs = new GoDexData.BLL.nodeSet();
                    //GoDexData.Model.nodeSet modelNs = bllNs.GetModel(int.Parse(ds.Tables[0].Rows[i]["machineNo"].ToString()));
                    DataSet dsNs = bllNs.GetList(" machineNo ='" + ds.Tables[0].Rows[i]["machineNo"].ToString() + "'");
                    switch (ds.Tables[0].Rows[i]["machineType"].ToString())
                    {

                        case "1": //"单管单区":
                            tnc.Text = "设备:" + ds.Tables[0].Rows[i]["machineNo"].ToString() + "-单管单区  位置:" + ds.Tables[0].Rows[i]["sign"].ToString();
                            tnc.Name = ds.Tables[0].Rows[i]["machineNo"].ToString();
                            tnc.Nodes.Add("气流低阈值:" + dsNs.Tables[0].Rows[0]["airflowL_pipe1"].ToString());
                            tnc.Nodes.Add("气流高阈值:" + dsNs.Tables[0].Rows[0]["airflowH_pipe1"].ToString());
                            tnc.Nodes.Add("一级火警阈值:" + dsNs.Tables[0].Rows[0]["fireA1_area1"].ToString());
                            tnc.Nodes.Add("二级火警阈值:" + dsNs.Tables[0].Rows[0]["fireA2_area1"].ToString());
                            tnc.Nodes.Add("三级火警阈值:" + dsNs.Tables[0].Rows[0]["fireA3_area1"].ToString());
                            tnc.Nodes.Add("四级火警阈值:" + dsNs.Tables[0].Rows[0]["fireA4_area1"].ToString());
                            break;

                        case "2":// "四管单区":
                            tnc.Text = "设备:" + ds.Tables[0].Rows[i]["machineNo"].ToString() + "-四管单区  位置:" + ds.Tables[0].Rows[i]["sign"].ToString();
                            tnc.Name = ds.Tables[0].Rows[i]["machineNo"].ToString();
                            for (int j = 1; j <= 4; j++)
                            {
                                tnc.Nodes.Add("管" + j.ToString());
                                tnc.Nodes[j - 1].Nodes.Add("气流高阈值:" + dsNs.Tables[0].Rows[0][2 * j + 1].ToString());
                                tnc.Nodes[j - 1].Nodes.Add("气流低阈值:" + dsNs.Tables[0].Rows[0][2 * j + 2].ToString());
                            }
                            tnc.Nodes.Add("一级火警阈值:" + dsNs.Tables[0].Rows[0]["fireA1_area1"].ToString());
                            tnc.Nodes.Add("二级火警阈值:" + dsNs.Tables[0].Rows[0]["fireA2_area1"].ToString());
                            tnc.Nodes.Add("三级火警阈值:" + dsNs.Tables[0].Rows[0]["fireA3_area1"].ToString());
                            break;

                        case "3"://"四管四区":
                            tnc.Text = "设备:" + ds.Tables[0].Rows[i]["machineNo"].ToString() + "-四管四区  位置:" + ds.Tables[0].Rows[i]["sign"].ToString();
                            tnc.Name = ds.Tables[0].Rows[i]["machineNo"].ToString();
                            for (int k = 1; k <= 4; k++)
                            {
                                tnc.Nodes.Add("区" + k.ToString());
                                tnc.Nodes[k - 1].Nodes.Add("气流高阈值:" + dsNs.Tables[0].Rows[0][2 * k + 1].ToString());
                                tnc.Nodes[k - 1].Nodes.Add("气流低阈值:" + dsNs.Tables[0].Rows[0][2 * k + 2].ToString());
                                tnc.Nodes[k - 1].Nodes.Add("一级火警阈值:" + dsNs.Tables[0].Rows[0][3 * k + 8].ToString());
                                tnc.Nodes[k - 1].Nodes.Add("二级火警阈值:" + dsNs.Tables[0].Rows[0][3 * k + 9].ToString());
                                tnc.Nodes[k - 1].Nodes.Add("三级火警阈值:" + dsNs.Tables[0].Rows[0][3 * k + 10].ToString());
                            }
                            break;
                        default:
                            break;
                    }

                    dsNs.Dispose();
                    this.treeViewNodes.Nodes.Add(tnc);
                }
                ds.Dispose();
            }
            catch (Exception ee)
            {
                //MessageBox.Show("CreateNodeTree" + ee.ToString());
                return;
            }
        }

        private static int[] OrderID()
        {
            GoDexData.BLL.nodeInfo bllNodeInfo = new GoDexData.BLL.nodeInfo();
            DataSet dsni = bllNodeInfo.GetAllList();
            Int32[] arrID = null;
            if (dsni != null)
            {
                arrID = new Int32[dsni.Tables[0].Rows.Count];
                for (int i = 0; i < dsni.Tables[0].Rows.Count; i++)
                {
                    arrID[i] = int.Parse(dsni.Tables[0].Rows[i]["machineNo"].ToString());
                }
                dsni.Dispose();

                for (int j = 0; j < arrID.Length; j++)
                {
                    for (int k = j; k < arrID.Length; k++)
                    {
                        if (arrID[j] > arrID[k])
                        {
                            int temp = arrID[k];
                            arrID[k] = arrID[j];
                            arrID[j] = temp;
                        }
                    }
                }
            }
            return arrID;
        }

        private static int[] arrOrderID()
        {
            GoDexData.BLL.nodeInfo bllNodeInfo = new GoDexData.BLL.nodeInfo();
            DataSet dsni = bllNodeInfo.GetAllList();
            Int32[] arrID = null;
            if (dsni != null)
            {
                arrID = new Int32[dsni.Tables[0].Rows.Count];
                for (int i = 0; i < dsni.Tables[0].Rows.Count; i++)
                {
                    arrID[i] = int.Parse(dsni.Tables[0].Rows[i]["machineNo"].ToString());
                }
                dsni.Dispose();
            }
            return arrID;
        }

        /// <summary>
        /// 串口初始化
        /// </summary>
        public void Initsp1()
        {
            try
            {
                _Comm.PortName = Cls.RWconfig.GetAppSettings("PortName");//串口名为COM1
                if (_Comm.IsOpen)
                    _Comm.Close();
                Thread.Sleep(300);
                _Comm.Open(); //打开串口
                //_Comm.DtrEnable = true;
                //参数设置,略
                //reader.Start();
            }
            catch (Exception ee)
            {
                MessageBox.Show("串口错误!" + ee.ToString());
            }
        }

        public void Initsp2()
        {
            try
            {
                _CommBack.PortName = Cls.RWconfig.GetAppSettings("PortNameBack");//串口名为COM2
                if (_CommBack.IsOpen)
                    _CommBack.Close();
                Thread.Sleep(300);
                _CommBack.Open(); //打开串口
                //_Comm.DtrEnable = true;                
                //参数设置,略
                readerBack.Start();
            }
            catch (Exception ee)
            {
                MessageBox.Show("串口错误!" + ee.ToString());
            }
        }

        private void InitComm(SerialPort _sp, string _name, int _baudrate, int _databits, System.IO.Ports.StopBits _stopbits, System.IO.Ports.Parity _parity)
        {
            if (_sp.IsOpen)
            {
                _sp.Close();
                Thread.Sleep(TimeSpan.FromSeconds(0.3));
            }
            _sp.PortName = _name;
            _sp.BaudRate = _baudrate;
            _sp.DataBits = _databits;
            _sp.StopBits = _stopbits;
            _sp.Parity = _parity;
            _sp.ReadTimeout = 1000;
            _sp.WriteTimeout = 1000;
            _sp.RtsEnable = false;
            _sp.DtrEnable = false;
            _sp.ReceivedBytesThreshold = 5;
            _sp.ReadBufferSize = 2048;
            _sp.WriteBufferSize = 2048;
            // _sp.NewLine = "\r\n";
            try { _sp.Open(); }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        /// <summary>
        /// Listview初始化
        /// </summary>
        private void CreateWarningList()
        {
            listView1.View = View.Details;
            listView1.LabelEdit = false;
            listView1.AllowColumnReorder = false;
            listView1.CheckBoxes = false;
            listView1.FullRowSelect = true;
            listView1.GridLines = true;
            listView1.Sorting = SortOrder.None;
            listView1.Columns.Add("等级", 150, HorizontalAlignment.Left);
            listView1.Columns.Add("编号", 50, HorizontalAlignment.Left);
            listView1.Columns.Add("描述", 300, HorizontalAlignment.Left);
            listView1.Columns.Add("位置", 300, HorizontalAlignment.Left);
            listView1.Columns.Add("时间", 500, HorizontalAlignment.Left);
        }

        bool CheckSN(string[] sn)
        {
            //sn是否为空
            foreach (string s in sn)
            {
                if (string.IsNullOrEmpty(s))
                {
                    return false;
                }
            }
            return true;
        }

        string CreateSN(string[] sn)
        {
            StringBuilder sb = new StringBuilder();
            //sn是否为空
            foreach (string s in sn)
            {
                sb.Append(s);
            }
            return sb.ToString();
        }
        /// <summary>
        /// 实时处理串口所接收的数据
        /// </summary>
        /// <param name="data"></param>
        public void HandleCommData(Byte[] data)
        {
            if (m_Closeing) return;
            string tipInfo = string.Empty;
            string nodeID = Convert.ToString(data[2]);
            string strData = Cls.Method.ByteToString(data);
            string address = string.Empty;
            string serialNo = string.Empty;
            //软件试用版限制数量
            //if (int.Parse(nodeID) > 5)
            //    return;
            //存储的报警上传  
            try
            {
                m_Listening = true;
                this.BeginInvoke(new Action(()=>{
                
               
                      if (data.Length == 9)
                      {
                          tipInfo = "监控节点-" + data[2].ToString() + "-通道" + ((data[3] >> 4) & 0x0F).ToString() + "历史警报-:" + (data[3] & 0x0F).ToString() + "级火警,位置:" + address + ",时间:" + Cls.Method.AscTo16(data[4].ToString()).ToString() + "年" + Cls.Method.AscTo16(data[5].ToString()).ToString() + "月" + Cls.Method.AscTo16(data[6].ToString()).ToString() + "日" + Cls.Method.AscTo16(data[7].ToString()).ToString() + "时" + Cls.Method.AscTo16(data[8].ToString()).ToString() + "分";
                          ShowLog(tipInfo, nodeID);
                      }

                      if (data.Length == 6)
                      {
                          //先判断校验是否成功
                          int c = data[1] ^ data[2] ^ data[3] ^ data[4];

                          if (data[5] == (data[1] ^ data[2] ^ data[3] ^ data[4]))
                          {
                              //添加节点
                              addNode(this.flowLayoutPanel1, nodeID, this.contextMenuStrip1);
                              //创建树形列表
                              CreateNodeTree(this.treeViewNodes);
                              //获取该节点控件
                              Control[] cc = this.flowLayoutPanel1.Controls.Find(nodeID, false);
                              UserControls.Node node = null;
                              if (cc.Length > 0)
                              {
                                  node = cc[0] as UserControls.Node;
                              }

                              //获取节点模型
                              GoDexData.BLL.nodeInfo bllNodeInfo = new GoDexData.BLL.nodeInfo();
                              GoDexData.Model.nodeInfo modelNi = bllNodeInfo.GetModel(int.Parse(nodeID));
                              if (modelNi == null)
                                  return;
                              //获取节点的设置参数模型
                              GoDexData.BLL.nodeSet bllNs = new GoDexData.BLL.nodeSet();
                              GoDexData.Model.nodeSet modelNs = bllNs.GetModel(int.Parse(nodeID));
                              if (modelNs == null)
                                  return;
                              address = modelNi.sign;
                              serialNo = modelNs.sign;

                              //处理串口数据
                              if (data[3] == 0x00 && data[4] == 0x00)//请求登记
                              {
                                  tipInfo = "监控节点-" + nodeID + "-登记成功-位置:" + address;
                                  ShowLog(tipInfo, nodeID);
                              }

                              if (data[3] == 0x00 && data[4] == 0x01)//设备类型:单管单区
                              {
                                  modelNi.machineType = 1;
                                  modelNs.machineType = "1";
                                  bllNodeInfo.Update(modelNi);
                                  bllNs.Update(modelNs);
                                  tipInfo = "上电-监控节点-" + nodeID + "-设备类型:单管单区-位置:" + address;
                                  //存储设备类型
                                  //this.Invoke(new autoCreateNode(CreateSysLog), new object[] { this.flowLayoutPanel1, nodeID, this.richTextBox1, this.listView1, data, tipInfo, this.contextMenuStrip1, this._userRole });
                                  if (cc.Length > 0)
                                  {
                                      //this.Invoke(new autoEditNode(changeNodeState), new object[] { alarmSpeaker, cc[0], this.flowLayoutPanel1, this.contextMenuStrip1, listView1, data, tipInfo });
                                      node.Addr = "设备类型:单管单区";
                                      node.lblAire2.Enabled = false;
                                      node.lblAire2.BackColor = Color.Gainsboro;
                                      node.lblAire3.Enabled = false;
                                      node.lblAire3.BackColor = Color.Gainsboro;
                                      node.lblAire4.Enabled = false;
                                      node.lblAire4.BackColor = Color.Gainsboro;
                                      node.lblFire2.Enabled = false;
                                      node.lblFire2.BackColor = Color.Gainsboro;
                                      node.lblFire3.Enabled = false;
                                      node.lblFire3.BackColor = Color.Gainsboro;
                                      node.lblFire4.Enabled = false;
                                      node.lblFire4.BackColor = Color.Gainsboro;
                                      node.Invalidate();
                                  }
                                  ShowLog(tipInfo, nodeID);
                              }

                              if (data[3] == 0x00 && data[4] == 0x02)//设备类型:单管四区
                              {
                                  tipInfo = "上电-监控节点-" + nodeID + "-设备类型:四管单区-位置:" + address;
                                  modelNi.machineType = 2;
                                  modelNs.machineType = "2";
                                  bllNodeInfo.Update(modelNi);
                                  //存储设备类型
                                  bllNs.Update(modelNs);
                                  //自动改变节点界面
                                  if (cc.Length > 0)
                                  {
                                      node.Addr = "设备类型:四管单区";
                                      node.lblFire2.Enabled = false;
                                      node.lblFire2.BackColor = Color.Gainsboro;
                                      node.lblFire3.Enabled = false;
                                      node.lblFire3.BackColor = Color.Gainsboro;
                                      node.lblFire4.Enabled = false;
                                      node.lblFire4.BackColor = Color.Gainsboro;
                                      node.Invalidate();
                                  }
                                  ShowLog(tipInfo, nodeID);
                              }

                              if (data[3] == 0x00 && data[4] == 0x03)//设备类型:四管四区
                              {
                                  tipInfo = "上电-监控节点-" + nodeID + "-设备类型:四管四区-位置:" + address;
                                  //存储设备类型
                                  modelNi.machineType = 3;
                                  modelNs.machineType = "3";
                                  bllNodeInfo.Update(modelNi);
                                  bllNs.Update(modelNs);
                                  //自动改变节点界面
                                  if (cc.Length > 0)
                                  {
                                      node.Addr = "设备类型:四管四区";
                                      node.Invalidate();
                                  }
                                  ShowLog(tipInfo, nodeID);
                              }


                              //上传编号
                              if (data[3] <= 0x8E && data[3] >= 0x8B)
                              {
                                  switch (data[3])
                                  {
                                      case 0x8E:
                                          m_arrSN[0] = data[4].ToString("X2");
                                          break;
                                      case 0x8D:
                                          m_arrSN[1] = data[4].ToString("X2");
                                          break;
                                      case 0x8C:
                                          m_arrSN[2] = data[4].ToString("X2");
                                          break;
                                      case 0x8B:
                                          m_arrSN[3] = data[4].ToString("X2");
                                          break;
                                  }

                                  if (CheckSN(m_arrSN))
                                  {
                                      string sno = CreateSN(m_arrSN);
                                      if (m_ns != null)
                                          m_ns.txtSerialNo.Text = sno;
                                      modelNs.sign = sno;
                                      bllNs.Update(modelNs);
                                      m_arrSN = new string[4];
                                  }
                              }

                              if (data[3] == 0x00 && data[4] == 0x05)//在线
                              {
                                  tipInfo = "提示-监控节点-" + nodeID + "-在线-位置:" + address;
                                  if (cc.Length > 0)
                                  {
                                      node.lblOnline.BackColor = Color.LightCyan;
                                      node.lblOnline.Text = "在线";
                                      modelNi.softversion = "在线";
                                      bllNs.Update(modelNs);
                                      node.Tag = "1";
                                      node.Refresh();
                                  }
                                  ShowLog(tipInfo, nodeID);
                              }

                              if (data[3] >= 0x01 && data[3] <= 0x05)//气流正常 0表示正常
                              {
                                  tipInfo = "提示-监控节点-" + nodeID + "-通道 [" + data[3].ToString() + "]气流正常-位置:" + address;
                                  switch(data[3])
                                  {
                                      case 1: modelNi.airChl1 = 0;break;
                                      case 2: modelNi.airChl2 = 0; break;
                                      case 3: modelNi.airChl3 = 0; break;
                                      case 4: modelNi.airChl4 = 0; break;
                                  }  
                                  bllNodeInfo.Update(modelNi);
                                  if (cc.Length > 0)
                                  {
                                      NormalStatusOfAirAndFire(data[3], node);
                                  }
                                  ShowLog(tipInfo, nodeID);
                              }

                              if (data[3] >= 0x11 && data[3] <= 0x15)//火警正常 0表示正常
                              {
                                  tipInfo = "提示-监控节点-" + nodeID + "-通道 [" + (data[3] - 0x10).ToString() + "]火警正常-位置:" + address;
                                  if (cc.Length > 0)
                                  {
                                      NormalStatusOfAirAndFire(data[3], node);
                                      modelNi.doordog = 0;
                                      int chl = (data[3] - 0x10);
                                      switch(chl)
                                      {
                                          case 1: modelNi.fireChl1 = 0; break;
                                          case 2: modelNi.fireChl2 = 0; break;
                                          case 3: modelNi.fireChl3 = 0; break;
                                          case 4: modelNi.fireChl4 = 0; break;
                                      }
                                      bllNodeInfo.Update(modelNi);
                                      ChangeFullMapNode(nodeID, 0);
                                  }
                                  ShowLog(tipInfo, nodeID);
                                  return;
                              }

                              if (data[3] >= 0x21 && data[3] <= 0x25)//气流低 -1
                              {
                                  tipInfo = "警告-监控节点-" + nodeID + "-通道[" + (data[3] - 0x20).ToString() + "]气流低,-位置:" + address;
                                  string[] arrTipInfo = tipInfo.Split('-');
                                  AddListItem(arrTipInfo, Color.Black, Color.Yellow);
                                  int chl = data[3] - 0x20;
                                  switch (chl)
                                  {
                                      case 1: modelNi.airChl1 = -1; break;
                                      case 2: modelNi.airChl2 = -1; break;
                                      case 3: modelNi.airChl3 = -1; break;
                                      case 4: modelNi.airChl4 = -1; break;
                                  }  
                                  bllNodeInfo.Update(modelNi);
                                  //弹出警报窗口
                                  createWarn(this._userRole, nodeID, tipInfo, Color.Gold, "通道[" + (data[3] - 0x20).ToString() + "]气流低", this.dockPanel, chl);
                                  if (cc.Length > 0)
                                  {
                                      switch (data[3])
                                      {
                                          case 0x21:
                                              node.lblAire1.BackColor = Color.Gold;
                                              break;
                                          case 0x22:
                                              node.lblAire2.BackColor = Color.Gold;
                                              break;
                                          case 0x23:
                                              node.lblAire3.BackColor = Color.Gold;
                                              break;
                                          case 0x24:
                                              node.lblAire4.BackColor = Color.Gold;
                                              break;
                                      }
                                      alarmSpeaker.Rate = 1;

                                      //语音报警
                                      for (int i = 0; i < int.Parse(Cls.RWconfig.GetAppSettings("AlarmNum")); i++)
                                      {
                                          alarmSpeaker.AnalyseSpeak(Cls.RWconfig.GetAppSettings("AireLow1").ToString() + arrTipInfo[2] + "号," + arrTipInfo[3].ToString() + "," + arrTipInfo[4].ToString() + "," + Cls.RWconfig.GetAppSettings("AireLow2").ToString());
                                      }
                                  }
                                  ShowLog(tipInfo, nodeID);
                              }

                              if (data[3] >= 0x31 && data[3] <= 0x35)//气流高 1
                              {
                                  tipInfo = "警告-监控节点-" + nodeID + "-通道[" + (data[3] - 0x30).ToString() + "]气流高,-位置:" + address;
                                  string[] arrTipInfo = tipInfo.Split('-');
                                  AddListItem(arrTipInfo, Color.Black, Color.Gold);
                                  int chl = data[3] - 0x30;
                                  switch (chl)
                                  {
                                      case 1: modelNi.airChl1 = 1; break;
                                      case 2: modelNi.airChl2 = 1; break;
                                      case 3: modelNi.airChl3 = 1; break;
                                      case 4: modelNi.airChl4 = 1; break;
                                  }  
                                  bllNodeInfo.Update(modelNi);

                                  createWarn(this._userRole, nodeID, tipInfo, Color.DarkOrange, "通道[" + (data[3] - 0x30).ToString() + "] 气流高", this.dockPanel, chl);
                                  if (cc.Length > 0)
                                  {
                                      switch (data[3])
                                      {
                                          case 0x31:
                                              node.lblAire1.BackColor = Color.DarkOrange;
                                              break;
                                          case 0x32:
                                              node.lblAire2.BackColor = Color.DarkOrange;
                                              break;
                                          case 0x33:
                                              node.lblAire3.BackColor = Color.DarkOrange;
                                              break;
                                          case 0x34:
                                              node.lblAire4.BackColor = Color.DarkOrange;
                                              break;
                                      }
                                      alarmSpeaker.Rate = 1;
                                      for (int i = 0; i < int.Parse(Cls.RWconfig.GetAppSettings("AlarmNum")); i++)
                                      {
                                          alarmSpeaker.AnalyseSpeak(Cls.RWconfig.GetAppSettings("AireHigh1").ToString() + arrTipInfo[2] + "号," + arrTipInfo[3].ToString() + "," + arrTipInfo[4].ToString() + "," + Cls.RWconfig.GetAppSettings("AireHigh2").ToString());
                                      }
                                  }
                                  ShowLog(tipInfo, nodeID);
                              }

                              if (data[3] == 0x1B)
                              {
                                  switch (data[4])
                                  {
                                      //电源故障，主电掉电
                                      case 0x01:
                                          tipInfo = "警告-监控节点-" + nodeID + "-电源故障:主电掉电-位置:" + address;
                                          string[] arrTipInfo = tipInfo.Split('-');
                                          AddListItem(arrTipInfo, Color.Black, Color.Gray);
                                          alarmSpeaker.Rate = 1;
                                          //语音报警
                                          for (int i = 0; i < int.Parse(Cls.RWconfig.GetAppSettings("AlarmNum")); i++)
                                          {
                                              alarmSpeaker.AnalyseSpeak(tipInfo);
                                          }
                                          ShowLog(tipInfo, nodeID);
                                          break;
                                      //备电欠压
                                      case 0x02:
                                          tipInfo = "警告-监控节点-" + nodeID + "-电源故障:备电欠压-位置:" + address;
                                          arrTipInfo = tipInfo.Split('-');
                                          AddListItem(arrTipInfo, Color.Black, Color.Gray);
                                          alarmSpeaker.Rate = 1;
                                          //语音报警
                                          for (int i = 0; i < int.Parse(Cls.RWconfig.GetAppSettings("AlarmNum")); i++)
                                          {
                                              alarmSpeaker.AnalyseSpeak(tipInfo);
                                          }
                                          ShowLog(tipInfo, nodeID);
                                          break;
                                  }
                              }


                              if (data[3] >= 0x41 && data[3] < 0x45)
                              {
                                  tipInfo = "一级火警警报-监控节点-" + nodeID + "-通道[" + (data[3] - 0x40).ToString() + "]1级火警,-位置:" + address;
                                  string[] arrTipInfo = tipInfo.Split('-');
                                  AddListItem(arrTipInfo, Color.Black, Color.Pink);
                                  alarmSpeaker.Rate = 1;
                                  //语音报警
                                  for (int i = 0; i < int.Parse(Cls.RWconfig.GetAppSettings("AlarmNum")); i++)
                                  {
                                      alarmSpeaker.AnalyseSpeak(Cls.RWconfig.GetAppSettings("Fire11").ToString() + arrTipInfo[2] + "号," + arrTipInfo[3].ToString() + "," + Cls.RWconfig.GetAppSettings("Fire12").ToString() + "," + arrTipInfo[4].ToString() + "," + Cls.RWconfig.GetAppSettings("Fire13").ToString());
                                  }
                                  int chl = data[3] - 0x40;
                                  createWarn(this._userRole, nodeID, tipInfo, Color.Pink, "通道[" + (data[3] - 0x40).ToString() + "] 一级火警", this.dockPanel, chl);
                                  modelNi.doordog = 1;
                                  switch (chl)
                                  {
                                      case 1: modelNi.fireChl1 = 1; break;
                                      case 2: modelNi.fireChl2 = 1; break;
                                      case 3: modelNi.fireChl3 = 1; break;
                                      case 4: modelNi.fireChl4 = 1; break;
                                  }
                                  bllNodeInfo.Update(modelNi);
                                  ChangeFullMapNode(nodeID, 1);
                                  if (cc.Length > 0)
                                  {
                                      switch (data[3])
                                      {
                                          case 0x41:
                                              node.lblFire1.BackColor = Color.Pink;
                                              break;
                                          case 0x42:
                                              node.lblFire2.BackColor = Color.Pink;
                                              break;
                                          case 0x43:
                                              node.lblFire3.BackColor = Color.Pink;
                                              break;
                                          case 0x44:
                                              node.lblFire4.BackColor = Color.Pink;
                                              break;
                                      }
                                  }
                                  ShowLog(tipInfo, nodeID);
                              }

                              if (data[3] >= 0x51 && data[3] < 0x55)
                              {
                                  tipInfo = "二级火警警报-监控节点-" + nodeID + "-通道[" + (data[3] - 0x50).ToString() + "]2级火警,-位置:" + address;
                                  string[] arrTipInfo = tipInfo.Split('-');
                                  AddListItem(arrTipInfo, Color.Black, Color.LightCoral); 
                                  switch (data[3])
                                  {
                                      case 0x51:
                                          node.lblFire1.BackColor = Color.LightCoral;
                                          break;
                                      case 0x52:
                                          node.lblFire2.BackColor = Color.LightCoral;
                                          break;
                                      case 0x53:
                                          node.lblFire3.BackColor = Color.LightCoral;
                                          break;
                                      case 0x54:
                                          node.lblFire4.BackColor = Color.LightCoral;
                                          break;
                                  }
                                  alarmSpeaker.Rate = 1;
                                  for (int i = 0; i < int.Parse(Cls.RWconfig.GetAppSettings("AlarmNum")); i++)
                                  {
                                      alarmSpeaker.AnalyseSpeak(Cls.RWconfig.GetAppSettings("Fire21").ToString() + arrTipInfo[2] + "号," + arrTipInfo[3].ToString() + "," + Cls.RWconfig.GetAppSettings("Fire22").ToString() + "," + arrTipInfo[4].ToString() + "," + Cls.RWconfig.GetAppSettings("Fire23").ToString());
                                  }
                                  int chl = (data[3] - 0x50);
                                  switch (chl)
                                  {
                                      case 1: modelNi.fireChl1 = 2; break;
                                      case 2: modelNi.fireChl2 = 2; break;
                                      case 3: modelNi.fireChl3 = 2; break;
                                      case 4: modelNi.fireChl4 = 2; break;
                                  }
                                  modelNi.doordog = 2;
                                  bllNodeInfo.Update(modelNi);
                                  ChangeFullMapNode(nodeID, 2);

                                  createWarn(this._userRole, nodeID, tipInfo, Color.LightCoral, "通道[" + (data[3] - 0x50).ToString() + "] 二级火警", this.dockPanel, chl);

                                  ShowLog(tipInfo, nodeID);
                              }

                              if (data[3] >= 0x61 && data[3] < 0x65)
                              {
                                  tipInfo = "三级火警警报-监控节点-" + nodeID + "-通道[" + (data[3] - 0x60).ToString() + "]3级火警,-位置:" + address;
                                  string[] arrTipInfo = tipInfo.Split('-');
                                  AddListItem(arrTipInfo, Color.White, Color.Red);
                                  
                                  for (int i = 0; i < int.Parse(Cls.RWconfig.GetAppSettings("AlarmNum")); i++)
                                  {
                                      alarmSpeaker.AnalyseSpeak(Cls.RWconfig.GetAppSettings("Fire31").ToString() + arrTipInfo[2] + "号," + arrTipInfo[3].ToString() + "," + Cls.RWconfig.GetAppSettings("Fire32").ToString() + "," + arrTipInfo[4].ToString() + "," + Cls.RWconfig.GetAppSettings("Fire33").ToString());
                                  }
                                  int chl = data[3] - 0x60;
                                  alarmSpeaker.Rate = 1;
                                  modelNi.doordog = 3;
                                  switch (chl)
                                  {
                                      case 1: modelNi.fireChl1 =3; break;
                                      case 2: modelNi.fireChl2 = 3; break;
                                      case 3: modelNi.fireChl3 = 3; break;
                                      case 4: modelNi.fireChl4 = 3; break;
                                  }
                                  bllNodeInfo.Update(modelNi);
                                  ChangeFullMapNode(nodeID, 3);

                                  createWarn(this._userRole, nodeID, tipInfo, Color.Red, "通道[" + (data[3] - 0x60).ToString() + "] 三级火警", this.dockPanel, chl);
                                  if (cc.Length > 0)
                                  {
                                      switch (data[3])
                                      {
                                          case 0x61:
                                              node.lblFire1.BackColor = Color.Red;
                                              break;
                                          case 0x62:
                                              node.lblFire2.BackColor = Color.Red;
                                              break;
                                          case 0x63:
                                              node.lblFire3.BackColor = Color.Red;
                                              break;
                                          case 0x64:
                                              node.lblFire4.BackColor = Color.Red;
                                              break;
                                      }
                                  }
                                  ShowLog(tipInfo, nodeID);
                              }

                              //通道4 4级火警
                              if (data[3] == 0x45)
                              {
                                  tipInfo = "四级火警警报-监控节点-" + nodeID + "-通道[4]4级火警,-位置:" + address;
                                 
                                  string[] arrTipInfo = tipInfo.Split('-');
                                  AddListItem(arrTipInfo, Color.Black, Color.Purple);
                                  for (int i = 0; i < int.Parse(Cls.RWconfig.GetAppSettings("AlarmNum")); i++)
                                  {
                                      alarmSpeaker.AnalyseSpeak(Cls.RWconfig.GetAppSettings("Fire41").ToString() + arrTipInfo[2] + "号," + arrTipInfo[3].ToString() + "," + Cls.RWconfig.GetAppSettings("Fire42").ToString() + "," + arrTipInfo[4].ToString() + "," + Cls.RWconfig.GetAppSettings("Fire43").ToString());
                                  }
                                 
                                  modelNi.fireChl4 = 4; 
                                  modelNi.doordog = 4;
                                  bllNodeInfo.Update(modelNi);
                                  ChangeFullMapNode(nodeID, 4);

                                  createWarn(this._userRole, nodeID, tipInfo, Color.Purple, "通道[4] 四级火警", this.dockPanel, 4);
                                  if (cc.Length > 0)
                                  {
                                      node.lblFire4.BackColor = Color.Purple;
                                  }
                                  ShowLog(tipInfo, nodeID);
                              }

                              //四级火警 ch3
                              if (data[3] == 0x55)
                              {
                                  tipInfo = "四级火警警报-监控节点-" + nodeID + "-通道[3]4级火警,-位置:" + address;
                                  modelNi.doordog = 4;
                                  modelNi.fireChl3 = 4; 
                                  bllNodeInfo.Update(modelNi);
                                  ChangeFullMapNode(nodeID, 4);
                                  string[] arrTipInfo = tipInfo.Split('-');
                                  AddListItem(arrTipInfo, Color.Black, Color.Purple);
                                  alarmSpeaker.Rate = 1;
                                  for (int i = 0; i < int.Parse(Cls.RWconfig.GetAppSettings("AlarmNum")); i++)
                                  {
                                      alarmSpeaker.AnalyseSpeak(Cls.RWconfig.GetAppSettings("Fire41").ToString() + arrTipInfo[2] + "号," + arrTipInfo[3].ToString() + "," + Cls.RWconfig.GetAppSettings("Fire42").ToString() + "," + arrTipInfo[4].ToString() + "," + Cls.RWconfig.GetAppSettings("Fire43").ToString());
                                  }
                                  createWarn(this._userRole, nodeID, tipInfo, Color.Purple, "通道[3],四级火警", this.dockPanel, 4);
                                  if (cc.Length > 0)
                                  {
                                      node.lblFire3.BackColor = Color.Purple;
                                  }
                                  ShowLog(tipInfo, nodeID);
                              }

                              //四级火警 ch2
                              if (data[3] == 0x60)
                              {
                                  tipInfo = "四级火警警报-监控节点-" + nodeID + "-通道[2]4级火警,-位置:" + address;
                                  modelNi.doordog = 4;
                                  modelNi.fireChl2 = 4; 
                                  bllNodeInfo.Update(modelNi);
                                  ChangeFullMapNode(nodeID, 4);
                                  string[] arrTipInfo = tipInfo.Split('-');
                                  AddListItem(arrTipInfo, Color.Black, Color.Purple);
                                  createWarn(this._userRole, nodeID, tipInfo, Color.Purple, "通道[2],四级火警", this.dockPanel, 2);
                                  alarmSpeaker.Rate = 1;
                                  for (int i = 0; i < int.Parse(Cls.RWconfig.GetAppSettings("AlarmNum")); i++)
                                  {
                                      alarmSpeaker.AnalyseSpeak(Cls.RWconfig.GetAppSettings("Fire41").ToString() + arrTipInfo[2] + "号," + arrTipInfo[3].ToString() + "," + Cls.RWconfig.GetAppSettings("Fire42").ToString() + "," + arrTipInfo[4].ToString() + "," + Cls.RWconfig.GetAppSettings("Fire43").ToString());
                                  }
                                  if (cc.Length > 0)
                                  {
                                      node.lblFire2.BackColor = Color.Purple;
                                  }
                                  ShowLog(tipInfo, nodeID);
                              }


                              //四级火警 ch1
                              if (data[3] == 0x65)
                              {
                                  tipInfo = "四级火警警报-监控节点-" + nodeID + "-通道[1]4级火警,-位置:" + address;
                                  modelNi.doordog = 4;
                                  modelNi.fireChl1 = 4; 
                                  bllNodeInfo.Update(modelNi);
                                  string[] arrTipInfo = tipInfo.Split('-');
                                  AddListItem(arrTipInfo, Color.Black, Color.Purple);
                                  //this.Invoke(new autoCreateNode(CreateSysLog), new object[] { this.flowLayoutPanel1, nodeID, this.richTextBox1, this.listView1, data, tipInfo, this.contextMenuStrip1, this._userRole });
                                  createWarn(this._userRole, nodeID, tipInfo, Color.Purple, "通道[1],四级火警", this.dockPanel, 1);
                                  alarmSpeaker.Rate = 1;
                                  for (int i = 0; i < int.Parse(Cls.RWconfig.GetAppSettings("AlarmNum")); i++)
                                  {
                                      alarmSpeaker.AnalyseSpeak(Cls.RWconfig.GetAppSettings("Fire41").ToString() + arrTipInfo[2] + "号," + arrTipInfo[3].ToString() + "," + Cls.RWconfig.GetAppSettings("Fire42").ToString() + "," + arrTipInfo[4].ToString() + "," + Cls.RWconfig.GetAppSettings("Fire43").ToString());
                                  }
                                  if (cc.Length > 0)
                                  {
                                      node.lblFire1.BackColor = Color.Purple;
                                  }
                                  ShowLog(tipInfo, nodeID);
                              }

                              if (data[3] >= 0x71 && data[3] <= 0x75)//气流故障 2 Gold
                              {
                                  switch (data[3])
                                  {
                                      case 0x71:
                                          node.lblAire1.BackColor = Color.Gold;
                                          break;
                                      case 0x72:
                                          node.lblAire2.BackColor = Color.Gold;
                                          break;
                                      case 0x73:
                                          node.lblAire3.BackColor = Color.Gold;
                                          break;
                                      case 0x74:
                                          node.lblAire4.BackColor = Color.Gold;
                                          break;
                                  }
                                  node.Refresh();

                                  int chl = data[3] - 0x70;
                                  switch (chl)
                                  {
                                      case 1: modelNi.airChl1 = 2; break;
                                      case 2: modelNi.airChl2 = 2; break;
                                      case 3: modelNi.airChl3 = 2; break;
                                      case 4: modelNi.airChl4 = 2; break;
                                  }
                                  bllNodeInfo.Update(modelNi);
                                  tipInfo = "故障-监控节点-" + nodeID + "-通道[" + (data[3] - 0x70).ToString() + "]气流故障,-位置:" + address;
                                  ShowLog(tipInfo, nodeID); 
                                  createWarn(this._userRole, nodeID, tipInfo, Color.Gold, "通道[" + (data[3] - 0x70).ToString() + "] 气流故障", this.dockPanel, chl);
                                  
                              }

                              if (data[3] >= 0x76 && data[3] <= 0x7A)//烟雾故障 -1 Gold
                              {
                                  switch (data[3])
                                  {
                                      case 0x76:
                                          node.lblFire1.BackColor = Color.Gold;
                                          break;
                                      case 0x77:
                                          node.lblFire2.BackColor = Color.Gold;
                                          break;
                                      case 0x78:
                                          node.lblFire3.BackColor = Color.Gold;
                                          break;
                                      case 0x79:
                                          node.lblFire4.BackColor = Color.Gold;
                                          break;
                                  }
                                  tipInfo = "故障-监控节点-" + nodeID + "-通道[" + (data[3] - 0x75).ToString() + "]烟雾故障,-位置:" + address;
                                  ShowLog(tipInfo, nodeID);
                                  int chl = data[3] - 0x75;
                                  switch (chl)
                                  {
                                      case 1: modelNi.fireChl1 = -1; break;
                                      case 2: modelNi.fireChl2 = -1; break;
                                      case 3: modelNi.fireChl3 = -1; break;
                                      case 4: modelNi.fireChl4 = -1; break;
                                  }
                                  createWarn(this._userRole, nodeID, tipInfo, Color.Gold, "通道[" + (data[3] - 0x75).ToString() + "] 烟雾故障,烟雾值为" + data[4].ToString(), this.dockPanel, chl);
                                  alarmSpeaker.Rate = 1;
                                  for (int i = 0; i < int.Parse(Cls.RWconfig.GetAppSettings("AlarmNum")); i++)
                                  {
                                      alarmSpeaker.AnalyseSpeak(tipInfo);
                                  }
                              }

                              if (data[3] == 0x80)
                              {
                                  string tip = string.Empty;
                                  //记住上一次隔离状态
                                  isGeliTemp = isGeli;
                                  //判断节点状态
                                  if ((data[4] & 0x01) == 0x01)
                                  {
                                      tip += "告警锁定;";
                                      modelNs.isLock = 1;
                                  }
                                  else
                                  {
                                      tip += "告警未锁定;";
                                      modelNs.isLock = 0;
                                  }

                                  if (((data[4] >> 1) & 0x01) == 0x01)
                                  {
                                      tip += "静音;";
                                      modelNs.isMute = 1;
                                  }
                                  else
                                  {
                                      tip += "未静音;";
                                      modelNs.isMute = 0;
                                  }

                                  if (((data[4] >> 2) & 0x01) == 0x01)
                                  {
                                      tip += "隔离;";
                                      isGeli = 1;
                                      modelNs.isSeparate = 1;
                                  }
                                  else
                                  {
                                      isGeli = 0;
                                      tip += "未隔离;";
                                      modelNs.isSeparate = 0;
                                  }

                                  if (((data[4] >> 3) & 0x01) == 0x01)
                                  {
                                      tip += "继电器极性反转;";
                                      modelNs.isReverse = 1;
                                  }
                                  else
                                  {
                                      modelNs.isReverse = 0;
                                      tip += "继电器极性正常;";
                                  }
                                  bllNs.Update(modelNs);

                                  tipInfo = "提示:-监控节点-" + nodeID + "-" + tip + "-位置:" + address;
                                  ShowLog(tipInfo, nodeID);
                                  if (isGeliTemp != isGeli)
                                  {
                                      createWarn(this._userRole, nodeID, tipInfo, Color.DeepSkyBlue, "节点状态", this.dockPanel, 0);
                                  }
                              }

                              if (data[3] == 0x81)
                              {
                                  tipInfo = "提示:-监控节点-" + nodeID + "-一级火警告警延时值:" + data[4].ToString() + "秒-位置" + address;
                                  ShowLog(tipInfo, nodeID);
                              }
                              if (data[3] == 0x82)
                              {
                                  tipInfo = "提示:-监控节点-" + nodeID + "-二级火警告警延时值:" + data[4].ToString() + "秒-位置" + address;
                                  ShowLog(tipInfo, nodeID);
                              }
                              if (data[3] == 0x83)
                              {
                                  tipInfo = "提示:-监控节点-" + nodeID + "-三级火警告警延时值:" + data[4].ToString() + "秒-位置" + address;
                                  ShowLog(tipInfo, nodeID);
                              }
                              if (data[3] == 0x84)
                              {
                                  tipInfo = "提示:-监控节点-" + nodeID + "-四级火警告警延时值:" + data[4].ToString() + "秒-位置" + address;
                                  ShowLog(tipInfo, nodeID);
                              }


                              if (data[3] == 0xAD)
                              {
                                  tipInfo = "提示-监控节点-" + nodeID + "-的烟雾浓度值-位置:" + address;
                                  ShowLog(tipInfo, nodeID);
                              }

                              //----------------------------------------------------

                              if (data[3] >= 0x06 && data[3] <= 0x0A)
                              {
                                  if (m_currentID.ToString() == nodeID)
                                      m_isOnline = true;
                                  tipInfo = "当前气流值-监控节点-" + nodeID + "-通道[" + (data[3] - 0x05).ToString() + "]值:" + data[4].ToString() + "-位置:" + address;
                                  ShowLog(tipInfo, nodeID);
                                  if (cc.Length > 0)
                                  {
                                      switch (data[3])
                                      {
                                          case 0x06:
                                              node.Airevalue1 = data[4];
                                              int airlow = int.Parse(modelNs.airflowL_pipe1.Value.ToString());//int.Parse(xn.ChildNodes[3].Attributes["ch1"].Value);
                                              int airhigh = int.Parse(modelNs.airflowH_pipe1.Value.ToString());//int.Parse(xn.ChildNodes[4].Attributes["ch1"].Value); 
                                              int value = data[4];
                                              //创建或插入烟雾值曲线数据
                                              Cls.Method.createAirCurveTxt(AppDomain.CurrentDomain.BaseDirectory + "Curve\\" + node.NodeNo + "_air_ch1_" + DateTime.Now.ToString("yyyyMMdd") + ".lin", airlow, airhigh, value);
                                              break;
                                          case 0x07:
                                              node.Airevalue2 = data[4];
                                              node.Airevalue1 = data[4];
                                              airlow = int.Parse(modelNs.airflowL_pipe2.Value.ToString());//int.Parse(xn.ChildNodes[3].Attributes["ch1"].Value);
                                              airhigh = int.Parse(modelNs.airflowH_pipe2.Value.ToString());//int.Parse(xn.ChildNodes[4].Attributes["ch1"].Value);
                                              value = data[4];
                                              //创建或插入烟雾值曲线数据
                                              Cls.Method.createAirCurveTxt(AppDomain.CurrentDomain.BaseDirectory + "Curve\\" + node.NodeNo + "_air_ch2_" + DateTime.Now.ToString("yyyyMMdd") + ".lin", airlow, airhigh, value);
                                              break;
                                          case 0x08: //当前气流值三管 
                                              node.Airevalue3 = data[4];
                                              airlow = int.Parse(modelNs.airflowL_pipe3.Value.ToString());//int.Parse(xn.ChildNodes[3].Attributes["ch1"].Value);
                                              airhigh = int.Parse(modelNs.airFlowH_pipe3.Value.ToString());//int.Parse(xn.ChildNodes[4].Attributes["ch1"].Value); 
                                              value = data[4];
                                              //创建或插入烟雾值曲线数据
                                              Cls.Method.createAirCurveTxt(AppDomain.CurrentDomain.BaseDirectory + "Curve\\" + node.NodeNo + "_air_ch3_" + DateTime.Now.ToString("yyyyMMdd") + ".lin", airlow, airhigh, value);
                                              break;
                                          case 0x09: //当前气流值四管 
                                              node.Airevalue4 = data[4];
                                              airlow = int.Parse(modelNs.airflowL_pipe4.Value.ToString());
                                              airhigh = int.Parse(modelNs.airflowL_pipe4.Value.ToString());
                                              value = data[4];
                                              //创建或插入烟雾值曲线数据
                                              Cls.Method.createAirCurveTxt(AppDomain.CurrentDomain.BaseDirectory + "Curve\\" + node.NodeNo + "_air_ch4_" + DateTime.Now.ToString("yyyyMMdd") + ".lin", airlow, airhigh, value);
                                              break;
                                      }
                                  }
                              }

                              if (data[3] >= 0x16 && data[3] <= 0x1A)
                              {
                                  if (m_currentID.ToString() == nodeID)
                                      m_isOnline = true;
                                  tipInfo = "当前烟雾值-监控节点-" + nodeID + "-通道[" + (data[3] - 0x15).ToString() + "]值:" + data[4].ToString() + "-位置:" + address;
                                  ShowLog(tipInfo, nodeID);
                                  //有烟雾值表示在线
                                  node.Tag = "1";
                                  //this.Invoke(new autoCreateNode(CreateSysLog), new object[] { this.flowLayoutPanel1, nodeID, this.richTextBox1, this.listView1, data, tipInfo, this.contextMenuStrip1, this._userRole });
                                  if (cc.Length > 0)
                                  {
                                      switch (data[3])
                                      {
                                          case 0x16://当前烟雾值一通道 
                                              int a1fire = int.Parse(modelNs.fireA1_area1.Value.ToString());
                                              int a2fire = int.Parse(modelNs.fireA2_area1.Value.ToString());
                                              int a3fire = int.Parse(modelNs.fireA3_area1.Value.ToString());
                                              int a4fire = int.Parse(modelNs.fireA4_area1.Value.ToString());
                                              int value = data[4];
                                              DateTime time = DateTime.Now;
                                              //创建或插入烟雾值曲线数据
                                              Cls.Method.createFireCurveTxt(AppDomain.CurrentDomain.BaseDirectory + "Curve\\" + node.NodeNo + "_fire_ch1_" + DateTime.Now.ToString("yyyyMMdd") + ".lin", a1fire, a2fire, a3fire, a4fire, value);
                                              node.Firevalue1 = data[4];
                                              break;
                                          case 0x17://当前烟雾值二通道 
                                              a1fire = int.Parse(modelNs.fireA1_area2.Value.ToString());
                                              a2fire = int.Parse(modelNs.fireA2_area2.Value.ToString());
                                              a3fire = int.Parse(modelNs.fireA3_area2.Value.ToString());
                                              a4fire = int.Parse(modelNs.fireA4_area2.Value.ToString());
                                              value = data[4];
                                              time = DateTime.Now;
                                              //创建或插入曲线数据
                                              Cls.Method.createFireCurveTxt(AppDomain.CurrentDomain.BaseDirectory + "Curve\\" + node.NodeNo + "_fire_ch2_" + DateTime.Now.ToString("yyyyMMdd") + ".lin", a1fire, a2fire, a3fire, a4fire, value);

                                              node.Firevalue2 = data[4];
                                              break;
                                          case 0x18://当前烟雾值三通道
                                              a1fire = int.Parse(modelNs.fireA1_area3.Value.ToString());
                                              a2fire = int.Parse(modelNs.fireA2_area3.Value.ToString());
                                              a3fire = int.Parse(modelNs.fireA3_area3.Value.ToString());
                                              a4fire = int.Parse(modelNs.fireA4_area3.Value.ToString());
                                              value = data[4];
                                              time = DateTime.Now;
                                              //创建或插入曲线数据
                                              Cls.Method.createFireCurveTxt(AppDomain.CurrentDomain.BaseDirectory + "Curve\\" + node.NodeNo + "_fire_ch3_" + DateTime.Now.ToString("yyyyMMdd") + ".lin", a1fire, a2fire, a3fire, a4fire, value);
                                              node.Firevalue3 = data[4];
                                              break;
                                          case 0x19://当前烟雾值四通道

                                              a1fire = int.Parse(modelNs.fireA1_area4.Value.ToString());
                                              a2fire = int.Parse(modelNs.fireA2_area4.Value.ToString());
                                              a3fire = int.Parse(modelNs.fireA3_area4.Value.ToString());
                                              a4fire = int.Parse(modelNs.fireA4_area4.Value.ToString());
                                              value = data[4];
                                              time = DateTime.Now;
                                              //创建或插入曲线数据
                                              Cls.Method.createFireCurveTxt(AppDomain.CurrentDomain.BaseDirectory + "Curve\\" + node.NodeNo + "_fire_ch4_" + DateTime.Now.ToString("yyyyMMdd") + ".lin", a1fire, a2fire, a3fire, a4fire, value);
                                              node.Firevalue4 = data[4];
                                              break;
                                      }
                                  }
                              }

                              if (data[3] >= 0x26 && data[3] <= 0x2A)
                              {
                                  tipInfo = "气流低告警门限值-监控节点-" + nodeID + "-通道[" + (data[3] - 0x25).ToString() + "]值:" + data[4].ToString() + "-位置:" + address;
                                  ShowLog(tipInfo, nodeID);
                                  switch (data[3])
                                  {
                                      case 0x26:
                                          modelNs.airflowL_pipe1 = data[4];
                                          break;
                                      case 0x27:
                                          modelNs.airflowL_pipe2 = data[4];
                                          break;
                                      case 0x28:
                                          modelNs.airflowL_pipe3 = data[4];
                                          break;
                                      case 0x29:
                                          modelNs.airflowL_pipe4 = data[4];
                                          break;
                                  }
                                  bllNs.Update(modelNs);
                              }

                              if (data[3] >= 0x36 && data[3] <= 0x3A)
                              {
                                  tipInfo = "气流高告警门限值-监控节点-" + nodeID + "-通道[" + (data[3] - 0x35).ToString() + "]值:" + data[4] + "-位置:" + address;
                                  ShowLog(tipInfo, nodeID);
                                  switch (data[3])
                                  {
                                      case 0x36:
                                          modelNs.airflowH_pipe1 = data[4];
                                          break;
                                      case 0x37:
                                          modelNs.airflowH_pipe2 = data[4];
                                          break;
                                      case 0x38:
                                          modelNs.airFlowH_pipe3 = data[4];
                                          break;
                                      case 0x39:
                                          modelNs.ariflowH_pipe4 = data[4];
                                          break;
                                  }
                                  bllNs.Update(modelNs);
                              }

                              if (data[3] >= 0x46 && data[3] <= 0x4A)
                              {
                                  tipInfo = "一级火警门限值-监控节点-" + nodeID + "-通道[" + (data[3] - 0x45).ToString() + "]值:" + data[4].ToString() + "-位置:" + address;
                                  ShowLog(tipInfo, nodeID);
                                  switch (data[3])
                                  {
                                      case 0x46:
                                          modelNs.fireA1_area1 = data[4];
                                          break;
                                      case 0x47:
                                          modelNs.fireA1_area2 = data[4];
                                          break;
                                      case 0x48:
                                          modelNs.fireA1_area3 = data[4];
                                          break;
                                      case 0x49:
                                          modelNs.fireA1_area4 = data[4];
                                          break;
                                  }
                                  bllNs.Update(modelNs);
                              }

                              if (data[3] >= 0x56 && data[3] <= 0x5A)
                              {
                                  tipInfo = "二级火警门限值-监控节点-" + nodeID + "-通道[" + (data[3] - 0x55).ToString() + "]值:" + data[4].ToString() + "-位置:" + address;
                                  ShowLog(tipInfo, nodeID);
                                  switch (data[3])
                                  {
                                      case 0x56:
                                          modelNs.fireA2_area1 = data[4];
                                          break;
                                      case 0x57:
                                          modelNs.fireA2_area2 = data[4];
                                          break;
                                      case 0x58:
                                          modelNs.fireA2_area3 = data[4];
                                          break;
                                      case 0x59:
                                          modelNs.fireA2_area4 = data[4];
                                          break;
                                  }
                                  bllNs.Update(modelNs);
                              }
                              if (data[3] >= 0x66 && data[3] <= 0x69)
                              {
                                  tipInfo = "三级火警门限值-监控节点-" + nodeID + "-通道[" + (data[3] - 0x65).ToString() + "]值:" + data[4].ToString() + "-位置:" + address;
                                  ShowLog(tipInfo, nodeID);
                                  switch (data[3])
                                  {
                                      case 0x66:
                                          modelNs.fireA3_area1 = data[4];
                                          break;
                                      case 0x67:
                                          modelNs.fireA3_area2 = data[4];
                                          break;
                                      case 0x68:
                                          modelNs.fireA3_area3 = data[4];
                                          break;
                                      case 0x69:
                                          modelNs.fireA3_area4 = data[4];
                                          break;
                                  }
                                  bllNs.Update(modelNs);
                              }

                              if (data[3] >= 0x6A && data[3] <= 0x6D)
                              {
                                  tipInfo = "四级火警门限值-监控节点-" + nodeID + "-通道[" + (data[3] - 0x69).ToString() + "]值:" + data[4].ToString() + "-位置:" + address;
                                  ShowLog(tipInfo, nodeID);
                                  switch (data[3])
                                  {
                                      case 0x6A:
                                          modelNs.fireA4_area1 = data[4];
                                          break;
                                      case 0x6B:
                                          modelNs.fireA4_area2 = data[4];
                                          break;
                                      case 0x6C:
                                          modelNs.fireA4_area3 = data[4];
                                          break;
                                      case 0x6D:
                                          modelNs.fireA4_area4 = data[4];
                                          break;
                                  }
                                  bllNs.Update(modelNs);
                              }


                              if (data[3] >= 0x90 && data[3] <= 0x95)
                              {

                                  if (data[3] == 0x90)
                                      tipInfo = "提示-监控节点-" + nodeID + "-年:-位置:" + address + data[4].ToString();
                                  if (data[3] == 0x91)
                                      tipInfo = "提示-监控节点-" + nodeID + "-月:-位置:" + address + data[4].ToString();
                                  if (data[3] == 0x92)
                                      tipInfo = "提示-监控节点-" + nodeID + "-日:-位置:" + address + data[4].ToString();
                                  if (data[3] == 0x93)
                                      tipInfo = "提示-监控节点-" + nodeID + "-时:-位置:" + address + data[4].ToString();
                                  if (data[3] == 0x94)
                                      tipInfo = "提示-监控节点-" + nodeID + "-分:-位置:" + address + data[4].ToString();
                                  if (data[3] == 0x95)
                                      tipInfo = "提示-监控节点-" + nodeID + "-秒:-位置:" + address + data[4].ToString();
                                  ShowLog(tipInfo, nodeID);
                              }
                          }
                      }
                }));
            }
            catch (Exception ee)
            {
                MessageBox.Show("handleData:" + ee.ToString());
                return;
            }
            finally { m_Listening = false; }
        }

        void ChangeFullMapNode(string id, int firegrade)
        {
            if (dc_Map != null)
            {
                Forms.frmFullMap ffm = dc_Map as Forms.frmFullMap;
                Control[] ffmc = ffm.picZoneMap.Controls.Find(id, false);
                if (ffmc.Length > 0)
                {
                    UserControls.NodeSite ns = ffmc[0] as UserControls.NodeSite;
                    switch (firegrade)
                    {
                        case 0:
                            ns.lblNo.BackColor = Color.Transparent;
                            break;
                        case 1:
                            ns.lblNo.BackColor = Color.Pink;
                            break;
                        case 2:
                            ns.lblNo.BackColor = Color.LightCoral;
                            break;
                        case 3:
                            ns.lblNo.BackColor = Color.Red;
                            break;
                        case 4:
                            ns.lblNo.BackColor = Color.Purple;
                            break;
                            break;
                    }

                    ns.Refresh();
                }
            }
        }

        private void AddListItem(string[] arrTipInfo, Color forcolor, Color backcolor)
        {
            ListViewItem item1 = new ListViewItem(arrTipInfo[0], 0);
            item1.SubItems.Add(arrTipInfo[2].ToString());
            item1.SubItems.Add(arrTipInfo[3].ToString());
            item1.SubItems.Add(arrTipInfo[4].ToString());
            item1.SubItems.Add(DateTime.Now.ToString());
            item1.ForeColor = forcolor;
            item1.BackColor = backcolor;
            listView1.Items.AddRange(new ListViewItem[] { item1 });
        }

        private void ShowLog(string tipInfo, string id)
        {
            if (!string.IsNullOrEmpty(tipInfo))
            {
                this.richTextBox1.AppendText(tipInfo + " [" + DateTime.Now.ToString() + "]\r\n");
                writeLogData(int.Parse(id), tipInfo, this._userRole, DateTime.Now, "-");
                //Cls.Method.writeLog(tipInfo + "[" + DateTime.Now.ToString() + "]");
            }
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.Focus();
        }

        //气流正常改变节点状态
        private void NormalStatusOfAirAndFire(byte data, UserControls.Node node)
        {
            if (data == 0x01)
            {
                //气流正常，根据节点类型改变节点界面
                switch (node.Addr)
                {
                    case "设备类型:单管单区":
                        node.lblAire1.BackColor = Color.LightCyan;
                        break;
                    case "设备类型:四管单区":
                        node.lblAire1.BackColor = Color.LightCyan;
                        break;
                    case "设备类型:四管四区":
                        node.lblAire1.BackColor = Color.LightCyan;
                        break;
                }
                return;
            }

            if (data == 0x02)
            {
                //气流正常，根据节点类型改变节点界面
                switch (node.Addr)
                {
                    case "设备类型:单管单区":
                        node.lblAire2.BackColor = Color.Gainsboro;
                        break;
                    case "设备类型:四管单区":
                        node.lblAire2.BackColor = Color.LightCyan;
                        break;
                    case "设备类型:四管四区":
                        node.lblAire2.BackColor = Color.LightCyan;
                        break;
                }
                return;
            }

            if (data == 0x03)
            {
                //气流正常，根据节点类型改变节点界面
                switch (node.Addr)
                {
                    case "设备类型:单管单区":
                        node.lblAire3.BackColor = Color.Gainsboro;
                        break;
                    case "设备类型:四管单区":
                        node.lblAire3.BackColor = Color.LightCyan;
                        break;
                    case "设备类型:四管四区":
                        node.lblAire3.BackColor = Color.LightCyan;
                        break;
                }
                return;
            }

            if (data == 0x04)
            {
                //气流正常，根据节点类型改变节点界面
                switch (node.Addr)
                {
                    case "设备类型:单管单区":
                        node.lblAire4.BackColor = Color.Gainsboro;
                        break;
                    case "设备类型:四管单区":
                        node.lblAire4.BackColor = Color.LightCyan;
                        break;
                    case "设备类型:四管四区":
                        node.lblAire4.BackColor = Color.LightCyan;
                        break;
                }
                return;
            }

            if (data == 0x11)
            {
                //火警正常，根据节点类型改变节点界面
                switch (node.Addr)
                {
                    case "设备类型:单管单区":
                        node.lblFire1.BackColor = Color.LightCyan;
                        break;
                    case "设备类型:四管单区":
                        node.lblFire1.BackColor = Color.LightCyan;
                        break;
                    case "设备类型:四管四区":
                        node.lblFire1.BackColor = Color.LightCyan;
                        break;
                }
                return;
            }

            if (data == 0x12)
            {
                //火警正常，根据节点类型改变节点界面
                switch (node.Addr)
                {
                    case "设备类型:单管单区":
                        node.lblFire2.BackColor = Color.Gainsboro;
                        break;
                    case "设备类型:四管单区":
                        node.lblFire2.BackColor = Color.Gainsboro;
                        break;
                    case "设备类型:四管四区":
                        node.lblFire2.BackColor = Color.LightCyan;
                        break;
                }
                return;
            }
            if (data == 0x13)
            {
                //火警正常，根据节点类型改变节点界面
                switch (node.Addr)
                {
                    case "设备类型:单管单区":
                        node.lblFire3.BackColor = Color.Gainsboro;
                        break;
                    case "设备类型:四管单区":
                        node.lblFire3.BackColor = Color.Gainsboro;
                        break;
                    case "设备类型:四管四区":
                        node.lblFire3.BackColor = Color.LightCyan;
                        break;
                }
                return;
            }
            if (data == 0x14)
            {
                //火警正常，根据节点类型改变节点界面
                switch (node.Addr)
                {
                    case "设备类型:单管单区":
                        node.lblFire4.BackColor = Color.Gainsboro;
                        break;
                    case "设备类型:四管单区":
                        node.lblFire4.BackColor = Color.Gainsboro;
                        break;
                    case "设备类型:四管四区":
                        node.lblFire4.BackColor = Color.LightCyan;
                        break;
                }
                return;
            }

        }

        /// <summary>
        /// 创建警告窗口 的 委托的方法
        /// </summary>
        /// <param name="userRole"></param>
        /// <param name="zoneMapConfigPath"></param>
        /// <param name="nodeConfigPath"></param>
        /// <param name="nodeID"></param>
        /// <param name="warningMsg"></param>
        /// <param name="nodeStatusColor"></param>
        private void createWarn(string userRole, string nodeID, string warningMsg, Color nodeStatusColor, string discribWarn, WeifenLuo.WinFormsUI.Docking.DockPanel dp, int chl)
        {
            formWaringMap = new GSM.Forms.frmWarningMap(userRole, nodeID, warningMsg, nodeStatusColor, chl);
            formWaringMap.CloseButton = true;
            formWaringMap.CloseButtonVisible = true;
            formWaringMap.HideOnClose = false;
            formWaringMap.Text = "节点:" + nodeID + discribWarn;
            formWaringMap.Show(dp, WeifenLuo.WinFormsUI.Docking.DockState.Document);
            GoDexData.Model.warn mwarn = new GoDexData.Model.warn();
            mwarn.machineNo = Convert.ToInt32( nodeID);
            mwarn.userName = userRole;
            mwarn.warnDiscrib = discribWarn;
            mwarn.warnLeval = warningMsg;
            mwarn.warnDateTime = DateTime.Now;
            new GoDexData.BLL.warn().Add(mwarn);
        }

        private static void addNode(FlowLayoutPanel flp, string nodeID, ContextMenuStrip cms)
        {
            try
            {
                //string ss = string.Empty;
                //foreach (GSM.UserControls.Node nd in flp.Controls)
                //{
                //    ss += "$" + nd.NodeNo + "$";
                //}
                Control[] c = flp.Controls.Find(nodeID, false);
                if (c.Length == 0)
                {
                    //软件试用版限制数量
                    if (int.Parse(nodeID) > 0 && int.Parse(nodeID) < 255)
                    {
                        //刷新树形列表
                        //CreateNodeTree(tv);
                        //Cls.Method.InsertNode(xmlpath, nodeID);  
                        //修改为数据库形式
                        //如果界面上无节点则创建 ,一个基本数据表，一个设置表
                        GoDexData.BLL.nodeInfo bllNodeInfo = new GoDexData.BLL.nodeInfo();
                        GoDexData.Model.nodeInfo modelNodeInfo;

                        if (!bllNodeInfo.Exists(int.Parse(nodeID)))
                        {
                            modelNodeInfo = new GoDexData.Model.nodeInfo();
                            modelNodeInfo.machineNo = int.Parse(nodeID);
                            modelNodeInfo.softversion = "离线";
                            modelNodeInfo.worldXY = "0,0";
                            modelNodeInfo.worldXY_1 = "0,0";
                            modelNodeInfo.worldXY_2 = "0,0";
                            modelNodeInfo.worldXY_3 = "0,0";
                            modelNodeInfo.worldXY_4 = "0,0";
                            modelNodeInfo.areaXY = "0,0";
                            modelNodeInfo.areaXY_1 = "0,0";
                            modelNodeInfo.areaXY_2 = "0,0";
                            modelNodeInfo.areaXY_3 = "0,0";
                            modelNodeInfo.areaXY_4 = "0,0";
                            modelNodeInfo.machineModel = "未设置型号";
                            modelNodeInfo.machineType = 3;
                            modelNodeInfo.lvwangdate = Convert.ToDateTime("2015-01-01");
                            modelNodeInfo.fireChl1 = 0;
                            modelNodeInfo.fireChl2 = 0;
                            modelNodeInfo.fireChl3 = 0;
                            modelNodeInfo.fireChl4 = 0;
                            modelNodeInfo.airChl1 = 0;
                            modelNodeInfo.airChl2 = 0;
                            modelNodeInfo.airChl3 = 0;
                            modelNodeInfo.airChl4 = 0;
                            bllNodeInfo.Add(modelNodeInfo);
                        }

                        GoDexData.BLL.nodeSet bllNodeSet = new GoDexData.BLL.nodeSet();
                        GoDexData.Model.nodeSet modelNodeSet;
                        if (!bllNodeSet.Exists(int.Parse(nodeID)))
                        {
                            modelNodeSet = new GoDexData.Model.nodeSet();
                            modelNodeSet.a1delay = 1;
                            modelNodeSet.a2delay = 1;
                            modelNodeSet.a3delay = 1;
                            modelNodeSet.a4delay = 1;
                            modelNodeSet.airflowH_pipe1 = 1;
                            modelNodeSet.airflowH_pipe2 = 1;
                            modelNodeSet.airFlowH_pipe3 = 1;
                            modelNodeSet.airflowL_pipe1 = 1;
                            modelNodeSet.airflowL_pipe2 = 1;
                            modelNodeSet.airflowL_pipe3 = 1;
                            modelNodeSet.airflowL_pipe4 = 1;
                            modelNodeSet.ariflowH_pipe4 = 1;
                            modelNodeSet.fireA1_area1 = 1;
                            modelNodeSet.fireA1_area2 = 1;
                            modelNodeSet.fireA1_area3 = 1;
                            modelNodeSet.fireA1_area4 = 1;
                            modelNodeSet.fireA2_area1 = 1;
                            modelNodeSet.fireA2_area2 = 1;
                            modelNodeSet.fireA2_area3 = 1;
                            modelNodeSet.fireA2_area4 = 1;
                            modelNodeSet.fireA3_area1 = 1;
                            modelNodeSet.fireA3_area2 = 1;
                            modelNodeSet.fireA3_area3 = 1;
                            modelNodeSet.fireA3_area4 = 1;
                            modelNodeSet.fireA4_area1 = 1;
                            modelNodeSet.fireA4_area2 = 1;
                            modelNodeSet.fireA4_area3 = 1;
                            modelNodeSet.fireA4_area4 = 1;
                            modelNodeSet.pumpSpeed = 1;
                            //节点状态
                            modelNodeSet.isLock = 1;
                            modelNodeSet.isSeparate = 0;
                            modelNodeSet.isMute = 0;
                            modelNodeSet.isReverse = 0;
                            modelNodeSet.machineType = "3";
                            modelNodeSet.machineNo = int.Parse(nodeID);
                            bllNodeSet.Add(modelNodeSet);
                        }

                        //在界面上添加节点控件 
                        modelNodeInfo = bllNodeInfo.GetModel(int.Parse(nodeID));
                        UserControls.Node newNode = new GSM.UserControls.Node();
                        newNode.Name = nodeID;
                        newNode.NodeNo = nodeID;
                        switch (modelNodeInfo.machineType)
                        {
                            case 1:
                                newNode.Addr = "设备类型:单管单区";
                                break;
                            case 2:
                                newNode.Addr = "设备类型:四管单区";
                                break;
                            case 3:
                                newNode.Addr = "设备类型:四管四区";
                                break;
                            default:
                                newNode.Addr = "设备类型:未知";
                                break;
                        }
                        newNode.ContextMenuStrip = cms;
                        newNode.Refresh();

                        m_arrayID = OrderID();

                        flp.Controls.Add(newNode);
                        //设置插入节点的位置                 
                        for (int i = 0; i < m_arrayID.Length; i++)
                        {
                            if (m_arrayID[i] == int.Parse(nodeID))
                            {
                                flp.Controls.SetChildIndex(newNode, i);
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show("addNode" + ee.ToString());
                return;
            }
        }

        /// <summary>
        /// 添加节点控件
        /// </summary>
        /// <param name="flp"></param>
        /// <param name="nodeID"></param>
        /// <param name="cms"></param>
        private static void addNode1(FlowLayoutPanel flp, string nodeID, ContextMenuStrip cms, int[] arrID)
        {
            try
            {
                string ss = string.Empty;
                foreach (GSM.UserControls.Node nd in flp.Controls)
                {
                    ss += "$" + nd.NodeNo + "$";
                }

                if (!ss.Contains("$" + nodeID + "$"))
                {
                    //软件试用版限制数量
                    if (int.Parse(nodeID) > 0 && int.Parse(nodeID) < 255)
                    {
                        //刷新树形列表
                        //CreateNodeTree(tv);

                        //Cls.Method.InsertNode(xmlpath, nodeID);  
                        //修改为数据库形式
                        //如果界面上无节点则创建 ,一个基本数据表，一个设置表
                        GoDexData.BLL.nodeInfo bllNodeInfo = new GoDexData.BLL.nodeInfo();
                        GoDexData.Model.nodeInfo modelNodeInfo;

                        if (!bllNodeInfo.Exists(int.Parse(nodeID)))
                        {
                            modelNodeInfo = new GoDexData.Model.nodeInfo();
                            modelNodeInfo.machineNo = int.Parse(nodeID);
                            modelNodeInfo.softversion = "离线";
                            modelNodeInfo.worldXY = "0,0";
                            modelNodeInfo.worldXY_1 = "0,0";
                            modelNodeInfo.worldXY_2 = "0,0";
                            modelNodeInfo.worldXY_3 = "0,0";
                            modelNodeInfo.worldXY_4 = "0,0";
                            modelNodeInfo.areaXY = "0,0";
                            modelNodeInfo.areaXY_1 = "0,0";
                            modelNodeInfo.areaXY_2 = "0,0";
                            modelNodeInfo.areaXY_3 = "0,0";
                            modelNodeInfo.areaXY_4 = "0,0";
                            modelNodeInfo.machineModel = "未设置型号";
                            bllNodeInfo.Add(modelNodeInfo);
                        }

                        GoDexData.BLL.nodeSet bllNodeSet = new GoDexData.BLL.nodeSet();
                        GoDexData.Model.nodeSet modelNodeSet;
                        if (!bllNodeSet.Exists(int.Parse(nodeID)))
                        {
                            modelNodeSet = new GoDexData.Model.nodeSet();
                            modelNodeSet.a1delay = 1;
                            modelNodeSet.a2delay = 1;
                            modelNodeSet.a3delay = 1;
                            modelNodeSet.a4delay = 1;
                            modelNodeSet.airflowH_pipe1 = 1;
                            modelNodeSet.airflowH_pipe2 = 1;
                            modelNodeSet.airFlowH_pipe3 = 1;
                            modelNodeSet.airflowL_pipe1 = 1;
                            modelNodeSet.airflowL_pipe2 = 1;
                            modelNodeSet.airflowL_pipe3 = 1;
                            modelNodeSet.airflowL_pipe4 = 1;
                            modelNodeSet.ariflowH_pipe4 = 1;
                            modelNodeSet.fireA1_area1 = 1;
                            modelNodeSet.fireA1_area2 = 1;
                            modelNodeSet.fireA1_area3 = 1;
                            modelNodeSet.fireA1_area4 = 1;
                            modelNodeSet.fireA2_area1 = 1;
                            modelNodeSet.fireA2_area2 = 1;
                            modelNodeSet.fireA2_area3 = 1;
                            modelNodeSet.fireA2_area4 = 1;
                            modelNodeSet.fireA3_area1 = 1;
                            modelNodeSet.fireA3_area2 = 1;
                            modelNodeSet.fireA3_area3 = 1;
                            modelNodeSet.fireA3_area4 = 1;
                            modelNodeSet.fireA4_area1 = 1;
                            modelNodeSet.fireA4_area2 = 1;
                            modelNodeSet.fireA4_area3 = 1;
                            modelNodeSet.fireA4_area4 = 1;
                            modelNodeSet.pumpSpeed = 1;
                            //节点状态
                            modelNodeSet.isLock = 1;
                            modelNodeSet.isSeparate = 0;
                            modelNodeSet.isMute = 0;
                            modelNodeSet.isReverse = 0;

                            modelNodeSet.machineNo = int.Parse(nodeID);
                            bllNodeSet.Add(modelNodeSet);
                        }

                        //在界面上添加节点控件 
                        modelNodeInfo = bllNodeInfo.GetModel(int.Parse(nodeID));
                        UserControls.Node newNode = new GSM.UserControls.Node();
                        newNode.Name = nodeID;
                        newNode.NodeNo = nodeID;
                        switch (modelNodeInfo.machineType)
                        {
                            case 1:
                                newNode.Addr = "设备类型:单管单区";
                                break;
                            case 2:
                                newNode.Addr = "设备类型:四管单区";
                                break;
                            case 3:
                                newNode.Addr = "设备类型:四管四区";
                                break;
                            default:
                                newNode.Addr = "设备类型:未知";
                                break;
                        }
                        newNode.ContextMenuStrip = cms;
                        newNode.Refresh();

                        arrID = OrderID();
                        flp.Controls.Add(newNode);
                        //设置插入节点的位置                 
                        for (int i = 0; i < arrID.Length; i++)
                        {
                            if (arrID[i] == int.Parse(nodeID))
                            {
                                flp.Controls.SetChildIndex(newNode, i);
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                //MessageBox.Show("addNode" + ee.ToString());
                return;
            }
        }

        /// <summary>
        /// 自动创建节点,显示接收的命令串,自动生成警报信息,自动记录日志。自动填充 警报列表
        /// </summary>
        /// <param name="flp">主界面容纳节点集合的控件</param>
        /// <param name="nodeID">节点ID</param>
        /// <param name="rt">用于显示提示信息的Richtextbox</param>
        /// <param name="listWarning">用于显示警报信息的listview</param>
        /// <param name="data">所接收的数据字符串数组</param>
        /// <param name="tipInfo">提示或警报的描述信息</param>
        /// <param name="cMenu">节点的右键菜单</param>
        //private static void CreateSysLog(FlowLayoutPanel flp, string nodeID, RichTextBox rt, ListView listWarning, Byte[] data, string tipInfo, ContextMenuStrip cMenu, string userRole)
        //{
        //    try
        //    {
        //        GoDexData.BLL.nodeInfo bllNi = new GoDexData.BLL.nodeInfo();
        //        GoDexData.Model.nodeInfo modelNi = bllNi.GetModel(int.Parse(nodeID));

        //        GoDexData.BLL.nodeSet bllNs = new GoDexData.BLL.nodeSet();
        //        GoDexData.Model.nodeSet modelNs = bllNs.GetModel(int.Parse(nodeID));

        //        if (data.Length == 9)
        //            goto LabelSysLog;

        //        if (data[5] == (data[1] ^ data[2] ^ data[3] ^ data[4]))
        //        {
        //            addNode(flp, nodeID, cMenu);
        //            //if (data[3] == 0x00 && data[4] == 0x00)//设备类型:单管单区
        //            //{
        //            //    goto LabelSysLog;
        //            //}

        //            if (data[3] == 0x00 && data[4] == 0x01)//设备类型:单管单区
        //            {
        //                modelNi.machineType = 1;
        //                modelNs.machineType = "1";
        //                goto LabelSysLog;
        //            }

        //            if (data[3] == 0x00 && data[4] == 0x02)//设备类型:四管单区
        //            {
        //                modelNi.machineType = 2;
        //                modelNs.machineType = "2";
        //                goto LabelSysLog;
        //            }

        //            if (data[3] == 0x00 && data[4] == 0x03)//设备类型:四管四区
        //            {
        //                modelNi.machineType = 3;
        //                modelNs.machineType = "3";
        //                goto LabelSysLog;
        //            }


        //            if (data[3] == 0x1B && data[4] == 0x01) //主电掉电
        //            {
        //                string[] arrTipInfo = tipInfo.Split('-');
        //                ListViewItem item1 = new ListViewItem(arrTipInfo[0], 0);
        //                item1.SubItems.Add(arrTipInfo[2].ToString());
        //                item1.SubItems.Add(arrTipInfo[3].ToString());
        //                item1.SubItems.Add(arrTipInfo[4].ToString());
        //                item1.SubItems.Add(DateTime.Now.ToString());
        //                item1.ForeColor = Color.Black;
        //                item1.BackColor = Color.Gray;
        //                listWarning.Items.AddRange(new ListViewItem[] { item1 });
        //                goto LabelSysLog;
        //            }

        //            if (data[3] == 0x1B && data[4] == 0x02) //备电欠压
        //            {
        //                string[] arrTipInfo = tipInfo.Split('-');
        //                ListViewItem item1 = new ListViewItem(arrTipInfo[0], 0);
        //                item1.SubItems.Add(arrTipInfo[2].ToString());
        //                item1.SubItems.Add(arrTipInfo[3].ToString());
        //                item1.SubItems.Add(arrTipInfo[4].ToString());
        //                item1.SubItems.Add(DateTime.Now.ToString());
        //                item1.ForeColor = Color.Black;
        //                item1.BackColor = Color.Gray;
        //                listWarning.Items.AddRange(new ListViewItem[] { item1 });
        //                goto LabelSysLog;
        //            }

        //            if (data[3] >= 0x21 && data[3] <= 0x25) //气流低
        //            {
        //                string[] arrTipInfo = tipInfo.Split('-');
        //                ListViewItem item1 = new ListViewItem(arrTipInfo[0], 0);
        //                item1.SubItems.Add(arrTipInfo[2].ToString());
        //                item1.SubItems.Add(arrTipInfo[3].ToString());
        //                item1.SubItems.Add(arrTipInfo[4].ToString());
        //                item1.SubItems.Add(DateTime.Now.ToString());
        //                item1.ForeColor = Color.Black;
        //                item1.BackColor = Color.Yellow;
        //                listWarning.Items.AddRange(new ListViewItem[] { item1 });
        //                goto LabelSysLog;
        //            }

        //            if (data[3] >= 0x31 && data[3] <= 0x35) //气流高
        //            {
        //                string[] arrTipInfo = tipInfo.Split('-');
        //                ListViewItem item1 = new ListViewItem(arrTipInfo[0], 0);
        //                item1.SubItems.Add(arrTipInfo[2].ToString());
        //                item1.SubItems.Add(arrTipInfo[3].ToString());
        //                item1.SubItems.Add(arrTipInfo[4].ToString());
        //                item1.SubItems.Add(DateTime.Now.ToString());
        //                item1.ForeColor = Color.Black;
        //                item1.BackColor = Color.Gold;
        //                listWarning.Items.AddRange(new ListViewItem[] { item1 });
        //                goto LabelSysLog;
        //            }

        //            if (data[3] >= 0x41 && data[3] < 0x45)//A1级火警
        //            {
        //                string[] arrTipInfo = tipInfo.Split('-');
        //                ListViewItem item1 = new ListViewItem(arrTipInfo[0], 0);
        //                item1.SubItems.Add(arrTipInfo[2].ToString());
        //                item1.SubItems.Add(arrTipInfo[3].ToString());
        //                item1.SubItems.Add(arrTipInfo[4].ToString());
        //                item1.SubItems.Add(DateTime.Now.ToString());
        //                item1.ForeColor = Color.Black;
        //                item1.BackColor = Color.Pink;
        //                listWarning.Items.AddRange(new ListViewItem[] { item1 });
        //                goto LabelSysLog;
        //            }

        //            if (data[3] >= 0x51 && data[3] < 0x55)//A2级火警
        //            {
        //                string[] arrTipInfo = tipInfo.Split('-');
        //                ListViewItem item1 = new ListViewItem(arrTipInfo[0], 0);
        //                item1.SubItems.Add(arrTipInfo[2].ToString());
        //                item1.SubItems.Add(arrTipInfo[3].ToString());
        //                item1.SubItems.Add(arrTipInfo[4].ToString());
        //                item1.SubItems.Add(DateTime.Now.ToString());
        //                item1.ForeColor = Color.Black;
        //                item1.BackColor = Color.LightCoral;
        //                listWarning.Items.AddRange(new ListViewItem[] { item1 });
        //                goto LabelSysLog;
        //            }

        //            if (data[3] >= 0x61 && data[3] < 0x65)//A3级火警
        //            {
        //                string[] arrTipInfo = tipInfo.Split('-');
        //                ListViewItem item1 = new ListViewItem(arrTipInfo[0], 0);
        //                item1.SubItems.Add(arrTipInfo[2].ToString());
        //                item1.SubItems.Add(arrTipInfo[3].ToString());
        //                item1.SubItems.Add(arrTipInfo[4].ToString());
        //                item1.SubItems.Add(DateTime.Now.ToString());
        //                item1.ForeColor = Color.White;
        //                item1.BackColor = Color.Red;
        //                listWarning.Items.AddRange(new ListViewItem[] { item1 });
        //                goto LabelSysLog;
        //            }


        //            if (data[3] == 0x45 || data[3] == 0x55 || data[3] == 0x60 || data[3] == 0x65)//4级火警
        //            {
        //                string[] arrTipInfo = tipInfo.Split('-');
        //                ListViewItem item1 = new ListViewItem(arrTipInfo[0], 0);
        //                item1.SubItems.Add(arrTipInfo[2].ToString());
        //                item1.SubItems.Add(arrTipInfo[3].ToString());
        //                item1.SubItems.Add(arrTipInfo[4].ToString());
        //                item1.SubItems.Add(DateTime.Now.ToString());
        //                item1.ForeColor = Color.Black;
        //                item1.BackColor = Color.Purple;
        //                listWarning.Items.AddRange(new ListViewItem[] { item1 });
        //                goto LabelSysLog;
        //            }


        //            if (data[3] >= 0x26 && data[3] <= 0x2A)//存取气流低告警门限值
        //            {
        //                switch (data[3])
        //                {
        //                    case 0x26:
        //                        modelNs.airflowL_pipe1 = data[4];
        //                        break;
        //                    case 0x27:
        //                        modelNs.airflowL_pipe2 = data[4];
        //                        break;
        //                    case 0x28:
        //                        modelNs.airflowL_pipe3 = data[4];
        //                        break;
        //                    case 0x29:
        //                        modelNs.airflowL_pipe4 = data[4];
        //                        break;
        //                }
        //                goto LabelSysLog;
        //            }

        //            if (data[3] >= 0x36 && data[3] <= 0x3A)//存取气流高告警门限值
        //            {
        //                switch (data[3])
        //                {
        //                    case 0x36:
        //                        modelNs.airflowH_pipe1 = data[4];
        //                        break;
        //                    case 0x37:
        //                        modelNs.airflowH_pipe2 = data[4];
        //                        break;
        //                    case 0x38:
        //                        modelNs.airFlowH_pipe3 = data[4];
        //                        break;
        //                    case 0x39:
        //                        modelNs.ariflowH_pipe4 = data[4];
        //                        break;
        //                }
        //                goto LabelSysLog;
        //            }

        //            if (data[3] >= 0x46 && data[3] <= 0x4A)//存取A1告警门限值
        //            {
        //                switch (data[3])
        //                {
        //                    case 0x46:
        //                        modelNs.fireA1_area1 = data[4];
        //                        break;
        //                    case 0x47:
        //                        modelNs.fireA1_area2 = data[4];
        //                        break;
        //                    case 0x48:
        //                        modelNs.fireA1_area3 = data[4];
        //                        break;
        //                    case 0x49:
        //                        modelNs.fireA1_area4 = data[4];
        //                        break;
        //                }
        //                goto LabelSysLog;
        //            }

        //            if (data[3] >= 0x56 && data[3] <= 0x5A)//存取A2告警门限值
        //            {
        //                switch (data[3])
        //                {
        //                    case 0x56:
        //                        modelNs.fireA2_area1 = data[4];
        //                        break;
        //                    case 0x57:
        //                        modelNs.fireA2_area2 = data[4];
        //                        break;
        //                    case 0x58:
        //                        modelNs.fireA2_area3 = data[4];
        //                        break;
        //                    case 0x59:
        //                        modelNs.fireA2_area4 = data[4];
        //                        break;
        //                }
        //                goto LabelSysLog;
        //            }

        //            if (data[3] >= 0x66 && data[3] <= 0x69)//存取A3告警门限值
        //            {
        //                switch (data[3])
        //                {
        //                    case 0x66:
        //                        modelNs.fireA3_area1 = data[4];
        //                        break;
        //                    case 0x67:
        //                        modelNs.fireA3_area2 = data[4];
        //                        break;
        //                    case 0x68:
        //                        modelNs.fireA3_area3 = data[4];
        //                        break;
        //                    case 0x69:
        //                        modelNs.fireA3_area4 = data[4];
        //                        break;
        //                }
        //                goto LabelSysLog;
        //            }

        //            if (data[3] >= 0x6A && data[3] <= 0x6D)//存取A4告警门限值
        //            {
        //                switch (data[3])
        //                {
        //                    case 0x6A:
        //                        modelNs.fireA4_area1 = data[4];
        //                        break;
        //                    case 0x6B:
        //                        modelNs.fireA4_area2 = data[4];
        //                        break;
        //                    case 0x6C:
        //                        modelNs.fireA4_area3 = data[4];
        //                        break;
        //                    case 0x6D:
        //                        modelNs.fireA4_area4 = data[4];
        //                        break;
        //                }
        //                goto LabelSysLog;
        //            }

        //            //获取节点 状态及火警延时值
        //            if (data[3] >= 0x81 && data[3] <= 0x84)
        //            {
        //                switch (data[3])
        //                {
        //                    case 0x81:
        //                        modelNs.a1delay = data[4];
        //                        break;
        //                    case 0x82:
        //                        modelNs.a2delay = data[4];
        //                        break;
        //                    case 0x83:
        //                        modelNs.a3delay = data[4];
        //                        break;
        //                    case 0x84:
        //                        modelNs.a4delay = data[4];
        //                        break;
        //                }
        //                goto LabelSysLog;
        //            }

        //            if (data[3] >= 0x90 && data[3] <= 0x95)//存储年月日时分秒
        //            {
        //                //XmlDocument xd = new XmlDocument();
        //                //xd.Load(xmlPath);
        //                //XmlElement xnChl = (XmlElement)xd.SelectSingleNode("/nodeGroup/node[@ID='" + nodeID + "']");	//选择ID为nodeID的node节点

        //                //foreach (XmlElement xe in xnChl)
        //                //{
        //                //    if (xe.Name == "dateTime")
        //                //        xe.SetAttribute("date" + (data[3] - 0x89).ToString(), data[4].ToString());
        //                //}
        //                //xd.Save(xmlPath); 

        //                switch (data[3])
        //                {
        //                    case 0x90:
        //                        break;
        //                    case 0x91:
        //                        break;
        //                    case 0x92:
        //                        break;
        //                    case 0x93:
        //                        break;
        //                    case 0x94:
        //                        break;
        //                    case 0x95:
        //                        break;
        //                    case 0x96:
        //                        break;
        //                }

        //                goto LabelSysLog;
        //            }
        //        }

        //    LabelSysLog:
        //        bllNi.Update(modelNi);
        //        bllNs.Update(modelNs);
        //        if (!string.IsNullOrEmpty(tipInfo))
        //        {
        //            rt.AppendText(tipInfo + " [" + DateTime.Now.ToString() + "]\r\n");
        //        }
        //        writeLogData(int.Parse(nodeID), tipInfo, userRole, DateTime.Now, "-");
        //        //Cls.Method.writeLog(tipInfo + "[" + DateTime.Now.ToString() + "]");
        //        rt.SelectionStart = rt.Text.Length;
        //        rt.Focus();
        //    }
        //    catch (Exception ee)
        //    {

        //        //MessageBox.Show("CreateSysLog"+ee.ToString());
        //        return;
        //    }
        //}

        public static void writeLogData(int nodeID, string tipInfo, string userRole, DateTime acDateTime, string sign)
        {
            try
            {
                GoDexData.BLL.log bllLog = new GoDexData.BLL.log();
                GoDexData.Model.log modelLog = new GoDexData.Model.log();
                modelLog.machineNo = nodeID;
                modelLog.action = tipInfo;
                modelLog.userName = userRole;
                modelLog.acDateTime = DateTime.Now;
                bllLog.Add(modelLog);
            }
            catch { }
        }

        private string[] getNodeArray(FlowLayoutPanel flp)
        {
            string[] nodeArray = null;
            int i = 0;
            foreach (UserControls.Node node in flp.Controls)
            {
                nodeArray[i] = node.NodeNo;
                i++;
            }
            return nodeArray;
        }
        /// <summary>
        /// 改变节点外观状态。正常: 预警: 火警1: 火警2:和显示 烟雾浓度 气流 值。此委托包含节点控件,可以控制节点控件上的元素。
        /// </summary>
        /// <param name="speaker">语音报警器</param>
        /// <param name="node">节点控件</param>
        /// <param name="warningList">主界面listview报警信号列表,自动添加</param>
        /// <param name="data">所接收到的数据字符串数组</param>
        /// <param name="tipInfo">报警或提示信息字符串,以‘,’隔</param>
        private static void changeNodeState(Cls.Speach speaker, UserControls.Node node, FlowLayoutPanel flp, ContextMenuStrip cms, ListView warningList, Byte[] data, string tipInfo)
        {
            try
            {
                string[] strTipInfo = tipInfo.Split('-');
                if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "Curve"))
                {
                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "Curve");
                }
                if (data[5] == (data[1] ^ data[2] ^ data[3] ^ data[4]))
                {
                    addNode(flp, node.NodeNo, cms);
                    GoDexData.BLL.nodeSet bllNs = new GoDexData.BLL.nodeSet();
                    GoDexData.Model.nodeSet modelNs = bllNs.GetModel(int.Parse(node.NodeNo));
                    node.Tag = "1";
                    if (data[3] == 0x00 && data[4] == 0x01)//设备类型:单管单区
                    {
                        node.Addr = "设备类型:单管单区";
                        node.lblAire2.Enabled = false;
                        node.lblAire2.BackColor = Color.Gainsboro;
                        node.lblAire3.Enabled = false;
                        node.lblAire3.BackColor = Color.Gainsboro;
                        node.lblAire4.Enabled = false;
                        node.lblAire4.BackColor = Color.Gainsboro;

                        node.lblFire2.Enabled = false;
                        node.lblFire2.BackColor = Color.Gainsboro;
                        node.lblFire3.Enabled = false;
                        node.lblFire3.BackColor = Color.Gainsboro;
                        node.lblFire4.Enabled = false;
                        node.lblFire4.BackColor = Color.Gainsboro;
                        node.Refresh();
                        return;
                    }

                    if (data[3] == 0x00 && data[4] == 0x02)//设备类型:四管单区
                    {
                        node.Addr = "设备类型:四管单区";
                        node.lblFire2.Enabled = false;
                        node.lblFire2.BackColor = Color.Gainsboro;
                        node.lblFire3.Enabled = false;
                        node.lblFire3.BackColor = Color.Gainsboro;
                        node.lblFire4.Enabled = false;
                        node.lblFire4.BackColor = Color.Gainsboro;
                        node.Refresh();
                        return;
                    }

                    if (data[3] == 0x00 && data[4] == 0x03)//设备类型:四管四区
                    {
                        node.Addr = "设备类型:四管四区";
                        node.Refresh();
                        return;
                    }

                    if (data[3] == 0x00 && data[4] == 0x05)//设备在线
                    {
                        node.lblOnline.BackColor = Color.LightCyan;
                        node.lblOnline.Text = "在线";
                        return;
                    }


                    if (data[3] == 0x01)
                    {
                        //气流正常，根据节点类型改变节点界面
                        switch (node.Addr)
                        {
                            case "设备类型:单管单区":
                                node.lblAire1.BackColor = Color.LightCyan;
                                break;
                            case "设备类型:四管单区":
                                node.lblAire1.BackColor = Color.LightCyan;
                                break;
                            case "设备类型:四管四区":
                                node.lblAire1.BackColor = Color.LightCyan;
                                break;
                        }
                        return;
                    }

                    if (data[3] == 0x02)
                    {
                        //气流正常，根据节点类型改变节点界面
                        switch (node.Addr)
                        {
                            case "设备类型:单管单区":
                                node.lblAire2.BackColor = Color.Gainsboro;
                                break;
                            case "设备类型:四管单区":
                                node.lblAire2.BackColor = Color.LightCyan;
                                break;
                            case "设备类型:四管四区":
                                node.lblAire2.BackColor = Color.LightCyan;
                                break;
                        }
                        return;
                    }

                    if (data[3] == 0x03)
                    {
                        //气流正常，根据节点类型改变节点界面
                        switch (node.Addr)
                        {
                            case "设备类型:单管单区":
                                node.lblAire3.BackColor = Color.Gainsboro;
                                break;
                            case "设备类型:四管单区":
                                node.lblAire3.BackColor = Color.LightCyan;
                                break;
                            case "设备类型:四管四区":
                                node.lblAire3.BackColor = Color.LightCyan;
                                break;
                        }
                        return;
                    }

                    if (data[3] == 0x04)
                    {
                        //气流正常，根据节点类型改变节点界面
                        switch (node.Addr)
                        {
                            case "设备类型:单管单区":
                                node.lblAire4.BackColor = Color.Gainsboro;
                                break;
                            case "设备类型:四管单区":
                                node.lblAire4.BackColor = Color.LightCyan;
                                break;
                            case "设备类型:四管四区":
                                node.lblAire4.BackColor = Color.LightCyan;
                                break;
                        }
                        return;
                    }

                    if (data[3] == 0x11)
                    {
                        //火警正常，根据节点类型改变节点界面
                        switch (node.Addr)
                        {
                            case "设备类型:单管单区":
                                node.lblFire1.BackColor = Color.LightCyan;
                                break;
                            case "设备类型:四管单区":
                                node.lblFire1.BackColor = Color.LightCyan;
                                break;
                            case "设备类型:四管四区":
                                node.lblFire1.BackColor = Color.LightCyan;
                                break;
                        }
                        return;
                    }

                    if (data[3] == 0x12)
                    {
                        //火警正常，根据节点类型改变节点界面
                        switch (node.Addr)
                        {
                            case "设备类型:单管单区":
                                node.lblFire2.BackColor = Color.Gainsboro;
                                break;
                            case "设备类型:四管单区":
                                node.lblFire2.BackColor = Color.Gainsboro;
                                break;
                            case "设备类型:四管四区":
                                node.lblFire2.BackColor = Color.LightCyan;
                                break;
                        }
                        return;
                    }
                    if (data[3] == 0x13)
                    {
                        //火警正常，根据节点类型改变节点界面
                        switch (node.Addr)
                        {
                            case "设备类型:单管单区":
                                node.lblFire3.BackColor = Color.Gainsboro;
                                break;
                            case "设备类型:四管单区":
                                node.lblFire3.BackColor = Color.Gainsboro;
                                break;
                            case "设备类型:四管四区":
                                node.lblFire3.BackColor = Color.LightCyan;
                                break;
                        }
                        return;
                    }
                    if (data[3] == 0x14)
                    {
                        //火警正常，根据节点类型改变节点界面
                        switch (node.Addr)
                        {
                            case "设备类型:单管单区":
                                node.lblFire4.BackColor = Color.Gainsboro;
                                break;
                            case "设备类型:四管单区":
                                node.lblFire4.BackColor = Color.Gainsboro;
                                break;
                            case "设备类型:四管四区":
                                node.lblFire4.BackColor = Color.LightCyan;
                                break;
                        }
                        return;
                    }

                    if (data[3] == 0x06) //当前气流值 一管
                    {
                        node.Airevalue1 = data[4];
                        int airlow = int.Parse(modelNs.airflowL_pipe1.Value.ToString());//int.Parse(xn.ChildNodes[3].Attributes["ch1"].Value);
                        int airhigh = int.Parse(modelNs.airflowH_pipe1.Value.ToString());//int.Parse(xn.ChildNodes[4].Attributes["ch1"].Value); 
                        int value = data[4];
                        //创建或插入烟雾值曲线数据
                        Cls.Method.createAirCurveTxt(AppDomain.CurrentDomain.BaseDirectory + "Curve\\" + node.NodeNo + "_air_ch1_" + DateTime.Now.ToString("yyyyMMdd") + ".lin", airlow, airhigh, value);
                        return;
                    }

                    if (data[3] == 0x07) //当前气流值二管
                    {
                        node.Airevalue2 = data[4];
                        node.Airevalue1 = data[4];
                        int airlow = int.Parse(modelNs.airflowL_pipe2.Value.ToString());//int.Parse(xn.ChildNodes[3].Attributes["ch1"].Value);
                        int airhigh = int.Parse(modelNs.airflowH_pipe2.Value.ToString());//int.Parse(xn.ChildNodes[4].Attributes["ch1"].Value);
                        int value = data[4];
                        //创建或插入烟雾值曲线数据
                        Cls.Method.createAirCurveTxt(AppDomain.CurrentDomain.BaseDirectory + "Curve\\" + node.NodeNo + "_air_ch2_" + DateTime.Now.ToString("yyyyMMdd") + ".lin", airlow, airhigh, value);
                        return;
                    }
                    if (data[3] == 0x08) //当前气流值三管
                    {
                        node.Airevalue3 = data[4];
                        int airlow = int.Parse(modelNs.airflowL_pipe3.Value.ToString());//int.Parse(xn.ChildNodes[3].Attributes["ch1"].Value);
                        int airhigh = int.Parse(modelNs.airFlowH_pipe3.Value.ToString());//int.Parse(xn.ChildNodes[4].Attributes["ch1"].Value); 
                        int value = data[4];
                        //创建或插入烟雾值曲线数据
                        Cls.Method.createAirCurveTxt(AppDomain.CurrentDomain.BaseDirectory + "Curve\\" + node.NodeNo + "_air_ch3_" + DateTime.Now.ToString("yyyyMMdd") + ".lin", airlow, airhigh, value);
                        return;
                    }
                    if (data[3] == 0x09) //当前气流值四管
                    {
                        node.Airevalue4 = data[4];
                        int airlow = int.Parse(modelNs.airflowL_pipe4.Value.ToString());
                        int airhigh = int.Parse(modelNs.airflowL_pipe4.Value.ToString());
                        int value = data[4];
                        //创建或插入烟雾值曲线数据
                        Cls.Method.createAirCurveTxt(AppDomain.CurrentDomain.BaseDirectory + "Curve\\" + node.NodeNo + "_air_ch4_" + DateTime.Now.ToString("yyyyMMdd") + ".lin", airlow, airhigh, value);
                        return;
                    }

                    if (data[3] == 0x16)//当前烟雾值一通道
                    {

                        int a1fire = int.Parse(modelNs.fireA1_area1.Value.ToString());
                        int a2fire = int.Parse(modelNs.fireA2_area1.Value.ToString());
                        int a3fire = int.Parse(modelNs.fireA3_area1.Value.ToString());
                        int a4fire = int.Parse(modelNs.fireA4_area1.Value.ToString());
                        int value = data[4];
                        DateTime time = DateTime.Now;
                        //创建或插入烟雾值曲线数据
                        Cls.Method.createFireCurveTxt(AppDomain.CurrentDomain.BaseDirectory + "Curve\\" + node.NodeNo + "_fire_ch1_" + DateTime.Now.ToString("yyyyMMdd") + ".lin", a1fire, a2fire, a3fire, a4fire, value);
                        node.Firevalue1 = data[4];
                        return;
                    }

                    if (data[3] == 0x17)//当前烟雾值二通道
                    {
                        int a1fire = int.Parse(modelNs.fireA1_area2.Value.ToString());
                        int a2fire = int.Parse(modelNs.fireA2_area2.Value.ToString());
                        int a3fire = int.Parse(modelNs.fireA3_area2.Value.ToString());
                        int a4fire = int.Parse(modelNs.fireA4_area2.Value.ToString());
                        int value = data[4];
                        DateTime time = DateTime.Now;
                        //创建或插入曲线数据
                        Cls.Method.createFireCurveTxt(AppDomain.CurrentDomain.BaseDirectory + "Curve\\" + node.NodeNo + "_fire_ch2_" + DateTime.Now.ToString("yyyyMMdd") + ".lin", a1fire, a2fire, a3fire, a4fire, value);

                        node.Firevalue2 = data[4];
                        return;
                    }
                    if (data[3] == 0x18)//当前烟雾值三通道
                    {
                        int a1fire = int.Parse(modelNs.fireA1_area3.Value.ToString());
                        int a2fire = int.Parse(modelNs.fireA2_area3.Value.ToString());
                        int a3fire = int.Parse(modelNs.fireA3_area3.Value.ToString());
                        int a4fire = int.Parse(modelNs.fireA4_area3.Value.ToString());
                        int value = data[4];
                        DateTime time = DateTime.Now;
                        //创建或插入曲线数据
                        Cls.Method.createFireCurveTxt(AppDomain.CurrentDomain.BaseDirectory + "Curve\\" + node.NodeNo + "_fire_ch3_" + DateTime.Now.ToString("yyyyMMdd") + ".lin", a1fire, a2fire, a3fire, a4fire, value);
                        node.Firevalue3 = data[4];
                        return;
                    }
                    if (data[3] == 0x19)//当前烟雾值四通道
                    {
                        int a1fire = int.Parse(modelNs.fireA1_area4.Value.ToString());
                        int a2fire = int.Parse(modelNs.fireA2_area4.Value.ToString());
                        int a3fire = int.Parse(modelNs.fireA3_area4.Value.ToString());
                        int a4fire = int.Parse(modelNs.fireA4_area4.Value.ToString());
                        int value = data[4];
                        DateTime time = DateTime.Now;
                        //创建或插入曲线数据
                        Cls.Method.createFireCurveTxt(AppDomain.CurrentDomain.BaseDirectory + "Curve\\" + node.NodeNo + "_fire_ch4_" + DateTime.Now.ToString("yyyyMMdd") + ".lin", a1fire, a2fire, a3fire, a4fire, value);
                        node.Firevalue4 = data[4];
                        return;
                    }
                    if (data[3] == 0x1A)//当前烟雾值五通道
                    {
                        node.Firevalue5 = data[4];
                        return;
                    }

                    if (data[3] >= 0x21 && data[3] <= 0x25)//上行通道气流低
                    {
                        switch (data[3])
                        {
                            case 0x21:
                                node.lblAire1.BackColor = Color.Gold;
                                break;
                            case 0x22:
                                node.lblAire2.BackColor = Color.Gold;
                                break;
                            case 0x23:
                                node.lblAire3.BackColor = Color.Gold;
                                break;
                            case 0x24:
                                node.lblAire4.BackColor = Color.Gold;
                                break;
                        }
                        speaker.Rate = 1;

                        //语音报警
                        for (int i = 0; i < int.Parse(Cls.RWconfig.GetAppSettings("AlarmNum")); i++)
                        {
                            speaker.AnalyseSpeak(Cls.RWconfig.GetAppSettings("AireLow1").ToString() + strTipInfo[2] + "号," + strTipInfo[3].ToString() + "," + strTipInfo[4].ToString() + "," + Cls.RWconfig.GetAppSettings("AireLow2").ToString());
                        }

                        return;
                    }

                    if (data[3] == 0x1B)
                    {
                        speaker.Rate = 1;
                        //语音报警
                        for (int i = 0; i < int.Parse(Cls.RWconfig.GetAppSettings("AlarmNum")); i++)
                        {
                            speaker.AnalyseSpeak(tipInfo);
                        }
                        return;
                    }

                    if (data[3] >= 0x31 && data[3] <= 0x35)//上行通道气流高
                    {
                        switch (data[3])
                        {
                            case 0x31:
                                node.lblAire1.BackColor = Color.DarkOrange;
                                break;
                            case 0x32:
                                node.lblAire2.BackColor = Color.DarkOrange;
                                break;
                            case 0x33:
                                node.lblAire3.BackColor = Color.DarkOrange;
                                break;
                            case 0x34:
                                node.lblAire4.BackColor = Color.DarkOrange;
                                break;
                        }
                        speaker.Rate = 1;
                        //string[] strTipInfo = tipInfo.Split('-');
                        for (int i = 0; i < int.Parse(Cls.RWconfig.GetAppSettings("AlarmNum")); i++)
                        {
                            speaker.AnalyseSpeak(Cls.RWconfig.GetAppSettings("AireHigh1").ToString() + strTipInfo[2] + "号," + strTipInfo[3].ToString() + "," + strTipInfo[4].ToString() + "," + Cls.RWconfig.GetAppSettings("AireHigh2").ToString());
                        }

                        return;
                    }

                    if (data[3] >= 0x41 && data[3] < 0x45)//上行A1级火警
                    {
                        speaker.Rate = 1;
                        //SendData(_Comm, null, 0xBB, 0x00, node.NodeNo);//报警回执 
                        switch (data[3])
                        {
                            case 0x41:
                                node.lblFire1.BackColor = Color.Pink;
                                break;
                            case 0x42:
                                node.lblFire2.BackColor = Color.Pink;
                                break;
                            case 0x43:
                                node.lblFire3.BackColor = Color.Pink;
                                break;
                            case 0x44:
                                node.lblFire4.BackColor = Color.Pink;
                                break;

                        }
                        //语音报警
                        for (int i = 0; i < int.Parse(Cls.RWconfig.GetAppSettings("AlarmNum")); i++)
                        {
                            speaker.AnalyseSpeak(Cls.RWconfig.GetAppSettings("Fire11").ToString() + strTipInfo[2] + "号," + strTipInfo[3].ToString() + "," + Cls.RWconfig.GetAppSettings("Fire12").ToString() + "," + strTipInfo[4].ToString() + "," + Cls.RWconfig.GetAppSettings("Fire13").ToString());
                        }

                        return;
                    }

                    //通道4的四级火警
                    if (data[3] == 0x45)
                    {
                        node.lblFire4.BackColor = Color.Purple;
                        //语音报警
                        for (int i = 0; i < int.Parse(Cls.RWconfig.GetAppSettings("AlarmNum")); i++)
                        {
                            speaker.AnalyseSpeak(Cls.RWconfig.GetAppSettings("Fire41").ToString() + strTipInfo[2] + "号," + strTipInfo[3].ToString() + "," + Cls.RWconfig.GetAppSettings("Fire42").ToString() + "," + strTipInfo[4].ToString() + "," + Cls.RWconfig.GetAppSettings("Fire43").ToString());
                        }
                        return;
                    }

                    if (data[3] >= 0x51 && data[3] < 0x55)//上行A2级火警
                    {
                        speaker.Rate = 1;
                        switch (data[3])
                        {
                            case 0x51:
                                node.lblFire1.BackColor = Color.LightCoral;
                                break;
                            case 0x52:
                                node.lblFire2.BackColor = Color.LightCoral;
                                break;
                            case 0x53:
                                node.lblFire3.BackColor = Color.LightCoral;
                                break;
                            case 0x54:
                                node.lblFire4.BackColor = Color.LightCoral;
                                break;
                        }
                        //node.lblFireValue.Text = data[4].ToString();
                        //报警回执
                        //SendData(_Comm, null, 0xBB, 0x00, node.NodeNo);
                        //语音报警
                        for (int i = 0; i < int.Parse(Cls.RWconfig.GetAppSettings("AlarmNum")); i++)
                        {
                            speaker.AnalyseSpeak(Cls.RWconfig.GetAppSettings("Fire21").ToString() + strTipInfo[2] + "号," + strTipInfo[3].ToString() + "," + Cls.RWconfig.GetAppSettings("Fire22").ToString() + "," + strTipInfo[4].ToString() + "," + Cls.RWconfig.GetAppSettings("Fire23").ToString());
                        }

                        return;
                    }

                    if (data[3] == 0x55)//上行通道3 四级火警
                    {
                        speaker.Rate = 1;
                        node.lblFire3.BackColor = Color.Purple;
                        //node.lblFireValue.Text = data[4].ToString();
                        //报警回执
                        //SendData(_Comm, null, 0xBB, 0x00, node.NodeNo);
                        //语音报警
                        for (int i = 0; i < int.Parse(Cls.RWconfig.GetAppSettings("AlarmNum")); i++)
                        {
                            speaker.AnalyseSpeak(Cls.RWconfig.GetAppSettings("Fire41").ToString() + strTipInfo[2] + "号," + strTipInfo[3].ToString() + "," + Cls.RWconfig.GetAppSettings("Fire42").ToString() + "," + strTipInfo[4].ToString() + "," + Cls.RWconfig.GetAppSettings("Fire43").ToString());
                        }
                        return;
                    }

                    if (data[3] == 0x60)//上行通道2 四级火警
                    {
                        speaker.Rate = 1;
                        node.lblFire2.BackColor = Color.Purple;
                        //node.lblFireValue.Text = data[4].ToString();
                        //报警回执
                        //SendData(_Comm, null, 0xBB, 0x00, node.NodeNo);
                        //语音报警
                        for (int i = 0; i < int.Parse(Cls.RWconfig.GetAppSettings("AlarmNum")); i++)
                        {
                            speaker.AnalyseSpeak(Cls.RWconfig.GetAppSettings("Fire41").ToString() + strTipInfo[2] + "号," + strTipInfo[3].ToString() + "," + Cls.RWconfig.GetAppSettings("Fire42").ToString() + "," + strTipInfo[4].ToString() + "," + Cls.RWconfig.GetAppSettings("Fire43").ToString());
                        }
                        return;
                    }


                    if (data[3] >= 0x61 && data[3] <= 0x64)//上行A3级火警
                    {
                        speaker.Rate = 1;
                        switch (data[3])
                        {
                            case 0x61:
                                node.lblFire1.BackColor = Color.Red;
                                break;
                            case 0x62:
                                node.lblFire2.BackColor = Color.Red;
                                break;
                            case 0x63:
                                node.lblFire3.BackColor = Color.Red;
                                break;
                            case 0x64:
                                node.lblFire4.BackColor = Color.Red;
                                break;
                        }
                        //node.lblFireValue.Text = data[4].ToString();
                        //报警回执
                        //SendData(_Comm, null, 0xBB, 0x00, node.NodeNo);
                        //语音报警
                        for (int i = 0; i < int.Parse(Cls.RWconfig.GetAppSettings("AlarmNum")); i++)
                        {
                            speaker.AnalyseSpeak(Cls.RWconfig.GetAppSettings("Fire31").ToString() + strTipInfo[2] + "号," + strTipInfo[3].ToString() + "," + Cls.RWconfig.GetAppSettings("Fire32").ToString() + "," + strTipInfo[4].ToString() + "," + Cls.RWconfig.GetAppSettings("Fire33").ToString());
                        }

                        return;
                    }

                    if (data[3] == 0x65)//ch1 上行A4级火警
                    {
                        speaker.Rate = 1;
                        node.lblFire1.BackColor = Color.Purple;
                        for (int i = 0; i < int.Parse(Cls.RWconfig.GetAppSettings("AlarmNum")); i++)
                        {
                            speaker.AnalyseSpeak(Cls.RWconfig.GetAppSettings("Fire41").ToString() + strTipInfo[2] + "号," + strTipInfo[3].ToString() + "," + Cls.RWconfig.GetAppSettings("Fire42").ToString() + "," + strTipInfo[4].ToString() + "," + Cls.RWconfig.GetAppSettings("Fire43").ToString());
                        }

                        return;
                    }

                    if (data[3] >= 0x76 && data[3] <= 0x7A)
                    {
                        speaker.Rate = 1;
                        for (int i = 0; i < int.Parse(Cls.RWconfig.GetAppSettings("AlarmNum")); i++)
                        {
                            speaker.AnalyseSpeak(tipInfo);
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                //MessageBox.Show("changeNodeState" + ee.ToString());
                return;
            }
        }

        private void hideTool_Click(object sender, EventArgs e)
        {
            this.hideTool.Checked = !this.hideTool.Checked;
        }

        private void hideTool_CheckedChanged(object sender, EventArgs e)
        {
            if (this.hideTool.Checked == true)
                this.toolStripMain.Visible = true;
            else
                this.toolStripMain.Visible = false;
        }

        void ToolStripButton3Click(object sender, EventArgs e)
        {
            Forms.frmArea fa = new GSM.Forms.frmArea();
            fa.ShowDialog();
        }

        void ToolStripButton5Click(object sender, EventArgs e)
        {
            //4、系统日志
            if (dc_FindLog == null)
            {
                dc_FindLog = new Forms.frmUserLog();
                dc_FindLog.Text = "系统日志";
                dc_FindLog.Name = "MapContainer";
                dc_FindLog.HideOnClose = false;
                dc_FindLog.CloseButton = true;
                dc_FindLog.CloseButtonVisible = true;
                dc_FindLog.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.Document;
                dc_FindLog.Show(this.dockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
            }
            else
            {
                dc_FindLog.Dispose();
                dc_FindLog = new Forms.frmUserLog();
                dc_FindLog.Text = "系统日志";
                dc_FindLog.Name = "MapContainer";
                dc_FindLog.HideOnClose = false;
                dc_FindLog.CloseButton = true;
                dc_FindLog.CloseButtonVisible = true;
                dc_FindLog.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.Document;
                dc_FindLog.Show(this.dockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
            }


            //Forms.frmUserLog log = new GSM.Forms.frmUserLog();
            //log.NodeConfigPath = this.nodeConfigPath;
            //log.ShowDialog();
        }


        private void Port_Click(object sender, EventArgs e)
        {
            Forms.frmPort fp = new GSM.Forms.frmPort(this);
            fp.ShowDialog();
            //int loopNums = int.Parse( Cls.RWconfig.GetAppSettings("loopNum").ToString());
            //CreateLoopPanel();
        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            //地图配置
            if (dc_AddrSet == null)
            {
                dc_AddrSet = new Forms.frmNodeMap(_userRole);
                dc_AddrSet.Text = "地理位置配置";
                dc_AddrSet.Name = "nodemap";
                dc_AddrSet.HideOnClose = true;
                dc_AddrSet.CloseButton = true;
                dc_AddrSet.CloseButtonVisible = true;
                dc_AddrSet.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.Document;
                dc_AddrSet.Show(this.dockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
            }
            else
            {
                dc_AddrSet.Dispose();
                dc_AddrSet = new Forms.frmNodeMap(_userRole);
                dc_AddrSet.Text = "地理位置配置";
                dc_AddrSet.Name = "nodemap";
                dc_AddrSet.HideOnClose = true;
                dc_AddrSet.CloseButton = true;
                dc_AddrSet.CloseButtonVisible = true;
                dc_AddrSet.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.Document;
                dc_AddrSet.Show(this.dockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
            }
            //Forms.frmNodeMap fnm = new GSM.Forms.frmNodeMap(_userRole);
            //fnm._NodeConfigPath = this.nodeConfigPath;
            //fnm._ZoneMapConfigPath = this.nodeMapConfigPath;
            //fnm.ShowDialog();
        }

        public void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                //Cls.Method.writeLog(this.tsslblUserRole.Text + "登出" + "[" + DateTime.Now.ToString() + "]");
                if (DialogResult.No == MessageBox.Show("您确定要退出吗?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    e.Cancel = true;
                    return;
                }
                if (_Comm.IsOpen)
                {
                    m_Closeing = true;
                    while (m_Listening) Application.DoEvents();
                    _Comm.Close();
                    while (_Comm.IsOpen) ;
                    m_Closeing = false;
                }

                if (_CommBack.IsOpen)
                {
                    m_Closeing = true;
                    while (m_Listening) Application.DoEvents();
                    _CommBack.Close();
                    while (_CommBack.IsOpen) ;
                    m_Closeing = false;
                }

                readerBack.Stop();
                Application.ExitThread();
                this.Dispose();
            }
            catch (Exception ee)
            {
                MessageBox.Show("软件操作异常" + ee.ToString());
            }
        }
        /// <summary>
        /// 创建节点配置文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void newFile_Click(object sender, EventArgs e)
        //{
        //    SaveFileDialog sf = new SaveFileDialog();
        //    int loopNum = int.Parse(Cls.RWconfig.GetAppSettings("loopNum").ToString());
        //    sf.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
        //    sf.Filter = "节点配置文件(*.nodexml) |*.nodexml";
        //    sf.RestoreDirectory = true;
        //    sf.ShowDialog();
        //    if (sf.FileName != string.Empty)
        //    {
        //        Cls.Method.CreateXmlFile(sf.FileName);
        //        Cls.Method.CreateMapXmlFile(Path.GetFileNameWithoutExtension(sf.FileName) + ".mapxml");
        //        Cls.RWconfig.SetAppSettings("nodeConfig", Path.GetFullPath(sf.FileName));
        //        Cls.RWconfig.SetAppSettings("nodeConfigMap", Path.GetFullPath(sf.FileName).Replace(".nodexml", ".mapxml"));
        //        this.Text = "早期预警系统-" + sf.FileName;
        //        this.flowLayoutPanel1.Controls.Clear();
        //    }
        //}

        ////打开节点配置文件
        //private void openFile_Click(object sender, EventArgs e)
        //{
        //    OpenFileDialog of = new OpenFileDialog();
        //    of.Filter = "节点配置文件(*.nodexml) |*.nodexml";
        //    of.ShowDialog();
        //    if (File.Exists(of.FileName))
        //    {
        //        Cls.RWconfig.SetAppSettings("nodeConfig", Path.GetFullPath(of.FileName));
        //        Cls.RWconfig.SetAppSettings("nodeConfigMap", Path.GetFullPath(of.FileName).Replace(".nodexml", ".mapxml"));
        //        this.Text = "早期预警系统-" + of.FileName;
        //        this.nodeConfigPath = of.FileName;
        //        this.nodeMapConfigPath = of.FileName.Replace(".nodexml", ".mapxml");
        //        CreateNodesView();
        //    }
        //}

        private void menuItemMobile_Click(object sender, EventArgs e)
        {
            Forms.frmOtherSet fos = new GSM.Forms.frmOtherSet();
            fos.ShowDialog();
        }

        private void readNodeCfg(string cfgFilePath)
        {

        }

        /// <summary>
        /// //串口发送数据字符串
        /// </summary>
        /// <param name="sendData1">第一个数据位</param>
        /// <param name="sendData2">第二个数据位</param>
        /// <param name="nodeID">节点ID</param>
        /// <returns>返回 整条命令的 16进制 字符串</returns>
        public static Byte[] SendData(SerialPort _Comm, SerialPort _CommBack, Byte sendData1, Byte sendData2, string nodeID)
        {
            Byte[] sData = new Byte[6];
            try
            {
                sData[0] = 0xFE;
                sData[1] = 0x04;
                sData[2] = Convert.ToByte(int.Parse(nodeID));
                sData[3] = sendData1;
                sData[4] = sendData2;
                sData[5] = Convert.ToByte(sData[1] ^ sData[2] ^ sData[3] ^ sData[4]);
                string[] sysPorts = SerialPort.GetPortNames();

                if (_Comm != null)
                {
                    foreach (string portName in sysPorts)
                    {
                        if (_Comm.PortName == portName)
                        {
                            _Comm.Write(sData, 0, 6);
                            //MessageBox.Show("发送成功");
                            break;
                        }
                    }
                }

                if (_CommBack != null)
                {
                    foreach (string portName in sysPorts)
                    {
                        if (_CommBack.PortName == portName)
                        {
                            _CommBack.Write(sData, 0, 6);
                            //MessageBox.Show("发送成功");
                            break;
                        }
                    }
                }
            }
            catch (Exception ee)
            { //MessageBox.Show(ee.ToString()); 
            }
            return sData;
        }

        //public static void initPort(SerialPort _port)
        //{
        //    try
        //    {
        //        _port.PortName = Cls.RWconfig.GetAppSettings("PortName");//串口名为COM1
        //        if (_port.IsOpen)
        //            _port.Close();
        //        _port.Open(); //打开串口
        //    }
        //    catch (Exception ee)
        //    {
        //        MessageBox.Show("串口错误!" + ee.ToString());
        //    }
        //}

        /// <summary>
        /// 节点复位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItemReset_Click(object sender, EventArgs e)
        {
            //this.alarmSpeaker.Stop();
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            ToolStrip ts = tsmi.Owner;
            UserControls.Node node = (UserControls.Node)((ts as ContextMenuStrip).SourceControl);

            ////气流
            //node.panelAire1.BackColor = Color.LightCyan;
            //node.panelAire2.BackColor = Color.LightCyan;
            //node.panelAire3.BackColor = Color.LightCyan;
            //node.panelAire4.BackColor = Color.LightCyan;
            ////火警
            //node.panelFire1.BackColor = Color.LightCyan;
            //node.panelFire2.BackColor = Color.LightCyan;
            //node.panelFire3.BackColor = Color.LightCyan;
            //node.panelFire4.BackColor = Color.LightCyan;
            if (node != null)
            {
                switch (node.lblAddr.Text)
                {
                    case "设备类型:单管单区":
                        node.lblAire2.Enabled = false;
                        node.lblAire2.BackColor = Color.Gainsboro;
                        node.lblAire3.Enabled = false;
                        node.lblAire3.BackColor = Color.Gainsboro;
                        node.lblAire4.Enabled = false;
                        node.lblAire4.BackColor = Color.Gainsboro;

                        node.lblFire2.Enabled = false;
                        node.lblFire2.BackColor = Color.Gainsboro;
                        node.lblFire3.Enabled = false;
                        node.lblFire3.BackColor = Color.Gainsboro;
                        node.lblFire4.Enabled = false;
                        node.lblFire4.BackColor = Color.Gainsboro;
                        break;
                    case "设备类型:四管单区":
                        node.lblFire2.Enabled = false;
                        node.lblFire2.BackColor = Color.Gainsboro;
                        node.lblFire3.Enabled = false;
                        node.lblFire3.BackColor = Color.Gainsboro;
                        node.lblFire4.Enabled = false;
                        node.lblFire4.BackColor = Color.Gainsboro;
                        break;
                    case "设备类型:四管四区":
                        break;
                    default:
                        break;
                }
                switch (this._userRole)
                {
                    case "Administrator":
                    case "Agency":
                    case "Manager":
                        SendData(this._Comm, null, 0xA1, 0x00, node.NodeNo);
                        //this.richTextBox1.AppendText( "命令: " + Cls.Method.ByteToString(SendData(this._Comm, null, 0xA1, 0x00, node.NodeNo)) + " \r\n";
                        break;

                    default:
                        this.richTextBox1.AppendText("-监控节点-" + node.NodeNo + " 复位![" + DateTime.Now.ToString() + "] \r\n");
                        SendData(this._Comm, null, 0xA1, 0x00, node.NodeNo);
                        break;
                }

                //Cls.Method.writeLog("-监控节点- " + node.NodeNo + " 复位![" + DateTime.Now.ToString() + "]");
                Cls.Method.writeLogData(int.Parse(node.NodeNo), "复位", this._userRole, DateTime.Now, "-");
                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                richTextBox1.Focus();
            }
        }

        /// <summary>
        /// 节点学习
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MenuItemStudyClick(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            ToolStrip ts = tsmi.Owner;
            UserControls.Node node = (UserControls.Node)((ts as ContextMenuStrip).SourceControl);
            if (node != null)
            {
                switch (node.lblAddr.Text)
                {
                    case "设备类型:单管单区":
                        node.lblAire2.Enabled = false;
                        node.lblAire2.BackColor = Color.Gainsboro;
                        node.lblAire3.Enabled = false;
                        node.lblAire3.BackColor = Color.Gainsboro;
                        node.lblAire4.Enabled = false;
                        node.lblAire4.BackColor = Color.Gainsboro;

                        node.lblFire2.Enabled = false;
                        node.lblFire2.BackColor = Color.Gainsboro;
                        node.lblFire3.Enabled = false;
                        node.lblFire3.BackColor = Color.Gainsboro;
                        node.lblFire4.Enabled = false;
                        node.lblFire4.BackColor = Color.Gainsboro;
                        break;
                    case "设备类型:四管单区":
                        node.lblFire2.Enabled = false;
                        node.lblFire2.BackColor = Color.Gainsboro;
                        node.lblFire3.Enabled = false;
                        node.lblFire3.BackColor = Color.Gainsboro;
                        node.lblFire4.Enabled = false;
                        node.lblFire4.BackColor = Color.Gainsboro;
                        break;
                    case "设备类型:四管四区":
                        break;
                    default:
                        break;
                }

                Thread.Sleep(10);
                //node.label1.Text = "正常 预警 火警1 火警2";
                //SendData(this._Comm,null,0xA2, 0x00, node.NodeNo);
                switch (this._userRole)
                {
                    case "Administrator":
                    case "Agency":
                    case "Manager":
                        SendData(this._Comm, null, 0xA2, 0x00, node.NodeNo);
                        //this.richTextBox1.AppendText( "命令: " + Cls.Method.ByteToString(SendData(this._Comm, null, 0xA2, 0x00, node.NodeNo)) + " \r\n";
                        break;
                    default:
                        SendData(this._Comm, null, 0xA2, 0x00, node.NodeNo);
                        break;
                }
                this.richTextBox1.AppendText("-监控节点-" + node.NodeNo + " 开始学习![" + DateTime.Now.ToString() + "] \r\n");
                //Cls.Method.writeLog("-监控节点-" + node.NodeNo + " 开始学习![" + DateTime.Now.ToString() + "]");
                Cls.Method.writeLogData(int.Parse(node.NodeNo), "开始学习!", this._userRole, DateTime.Now, "-");

                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                richTextBox1.Focus();
            }
        }

        void ToolStripButtonResetClick(object sender, EventArgs e)
        {
            foreach (UserControls.Node node in this.flowLayoutPanel1.Controls)
            {
                if (node.IsCheck == true)
                {
                    SendData(this._Comm, null, 0xA1, 0x00, node.NodeNo);//诊断复位
                }
            }
        }

        void ToolStripButtonFixClick(object sender, EventArgs e)
        {
            foreach (UserControls.Node node in this.flowLayoutPanel1.Controls)
            {
                if (node.IsCheck == true)
                {
                    SendData(this._Comm, null, 0xA1, 0x00, node.NodeNo);//诊断复位
                }
            }
        }

        bool m_isLunXun = false;
        CancellationTokenSource m_ctsLunXun;
        Stopwatch m_watchLunxun = new Stopwatch();



        void ToolStripButtonScanClick(object sender, EventArgs e)
        {

            m_isLunXun = !m_isLunXun;
            if (m_isLunXun)
            {
                this.tsbtnScan.Text = "停止巡检";
                this.Port.Enabled = false;
                this.tsbtnScan.BackColor = Color.Red;
                m_ctsLunXun = new CancellationTokenSource();
                //轮询读取数据
                Task.Run(() => { SendLunXunOrder(m_ctsLunXun.Token); });
                //轮询检测离线
                Task.Run(() => { CheckOnlineTimer(m_ctsLunXun.Token); });
            }
            else
            {
                m_ctsLunXun.Cancel();
                m_isLunXun = false;
                this.tsbtnScan.Text = "启动巡检";
                this.Port.Enabled = true;
                this.tsbtnScan.BackColor = Color.LightCyan;
            }
        }

        void SendLunXunOrder(CancellationToken ct)
        {
            m_orderTime = GetOrderTimeSpan();
            m_arrayID = arrOrderID();
            int lunxunTimeSpan = GetLunxunTimeSpan();
            while (m_isLunXun)
            {
                for (int i = 0; i < m_arrayID.Length; i++)
                {
                    if (ct.IsCancellationRequested)
                        break;
                    else
                    {
                        byte[] b = SendData(_Comm, null, Convert.ToByte(0xAB), Convert.ToByte(0x00), m_arrayID[i].ToString());
                        StringBuilder sb = new StringBuilder();
                        foreach (byte bb in b)
                            sb.Append(bb.ToString("X2") + " ");
                        Trace.WriteLine("发送命令:" + sb.ToString() + "-" + DateTime.Now.ToString());
                        Thread.Sleep(m_orderTime);
                    }
                }
                Thread.Sleep(lunxunTimeSpan);
            }
        }

        void CheckOnlineTimer(CancellationToken ct)
        {
            int offlineSpan = GetOfflineTimeSpan();
            while (m_isLunXun)
            {
                CheckOnline(ct);
                Thread.Sleep(offlineSpan);
            }
        }

        void CheckOnline(CancellationToken ct)
        {
            GoDexData.BLL.nodeInfo bllNodeInfo = new GoDexData.BLL.nodeInfo();
            DataSet dsni = bllNodeInfo.GetAllList();
            int count = dsni.Tables[0].Rows.Count;
            //如何避免集合改变，此处抛出bug 
            bool warnoffline = false;
            for (int i = 0; i < count; i++)
            {
                if (ct.IsCancellationRequested)
                    break;
                else
                {
                    string node = dsni.Tables[0].Rows[i]["machineNo"].ToString();
                    GoDexData.Model.nodeInfo ni = bllNodeInfo.GetModel(Convert.ToInt32(node));
                    this.BeginInvoke(new Action(() =>
                    {
                        //根据ID查找该控件的值，如果有值，则表示在线
                        Control[] c = this.flowLayoutPanel1.Controls.Find(node, false);
                        if (c.Length > 0)
                        {
                            UserControls.Node n = (UserControls.Node)c[0];
                            if (n.Tag.ToString() == "0")
                            {
                                n.lblOnline.Text = "离线";
                                n.lblOnline.BackColor = Color.Red;
                                n.Refresh();                                
                                ni.softversion = "离线"; 
                                //Trace.WriteLine("ID:" + m_arrayID[i].ToString() + "-Tag:" + n.Tag.ToString() + n.lblOnline.Text);
                                Cls.Method.writeLogData(Convert.ToInt32(n.NodeNo), "离线", this._userRole, DateTime.Now, "离线,请检查线路和设备！");
                                warnoffline = true;
                            }
                            else
                            {
                                n.lblOnline.Text = "在线";
                                n.lblOnline.BackColor = Color.LightCyan;
                                ni.softversion = "在线"; 
                                n.Refresh();
                                //Trace.WriteLine("ID:" + m_arrayID[i].ToString() + "-Tag:" + n.Tag.ToString() + n.lblOnline.Text);
                            }
                            bllNodeInfo.Update(ni);
                            n.Tag = "0";
                        }
                    }));
                }
                Thread.Sleep(100);
            }
            if (warnoffline)
                alarmSpeaker.AnalyseSpeak("警告-节点离线-请检查线路和设备");
        }

        int GetOrderTimeSpan()
        {
            return string.IsNullOrEmpty(Cls.RWconfig.GetAppSettings("orderTimeSpan")) ? 100 : Convert.ToInt32(Cls.RWconfig.GetAppSettings("orderTimeSpan"));
        }

        int GetLunxunTimeSpan()
        {
            return int.Parse(Cls.RWconfig.GetAppSettings("lunxunTimeSpan")) < 10000 ? 10000 : int.Parse(Cls.RWconfig.GetAppSettings("lunxunTimeSpan"));
        }

        int GetOfflineTimeSpan()
        {
            return int.Parse(Cls.RWconfig.GetAppSettings("offlineTimeSpan")) < 10000 ? 10000 : int.Parse(Cls.RWconfig.GetAppSettings("offlineTimeSpan"));
        }

        //地点配置
        void SiteToolStripMenuItemClick(object sender, EventArgs e)
        {
            //foreach (UserControls.Node node in this.flowLayoutPanel1.Controls)
            //{
            //    if (node.IsCheck == true)
            //    {
            //        Forms.frmSetNode ns = new GSM.Forms.frmSetNode();
            //       node.Text = "监控器:" + node.NodeNo + "设备设定";
            //        ns.NodeNo = node.NodeNo;
            //        ns.SerialPort = this._Comm;
            //        ns.XmlPath = this.nodeConfigPath;
            //        ns.ShowDialog();
            //    }
            //}
        }

        private void toolAbout_Click(object sender, EventArgs e)
        {
            new Forms.AboutBox().ShowDialog();
        }

        private void machineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (UserControls.Node node in this.flowLayoutPanel1.Controls)
            {
                if (node.IsCheck == true)
                {
                    Forms.frmSetNode ns = new GSM.Forms.frmSetNode(_userRole, this);
                    ns.Text = "监控器:" + node.NodeNo + "设备设定";
                    ns.NodeNo = ns.NodeNo;
                    ns.RichTextBox = this.richTextBox1;
                    ns.SerialPort = this._Comm;
                    //ns.XmlPath = this.nodeConfigPath;
                    ns.ShowDialog();
                }
            }
        }
        Forms.frmSetNode m_ns;
        //节点设置
        private void MenuItemSet_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            ToolStrip ts = tsmi.Owner;
            UserControls.Node node = (UserControls.Node)((ts as ContextMenuStrip).SourceControl);
            if (node != null)
            {
                m_ns = new GSM.Forms.frmSetNode(_userRole, this);
                m_ns.Text = "监控器:" + node.NodeNo + "设备设定";
                m_ns._machineType = node.lblAddr.Text.Replace("设备类型:", "");
                m_ns.NodeNo = node.NodeNo;
                m_ns.RichTextBox = this.richTextBox1;
                m_ns.SerialPort = this._Comm;
                m_ns.txtSerialNo.Text = "";
                switch (_userRole)
                {
                    case "Administrator":
                        m_ns.btnReadSerialNo.Enabled = true;
                        m_ns.btnSetSerialNo.Enabled = true;
                        m_ns.txtSerialNo.Enabled = true;
                        m_ns.groupBoxOther.Enabled = true;
                        break;
                    case "Agency":
                        m_ns.btnReadSerialNo.Enabled = true;
                        m_ns.btnSetSerialNo.Enabled = false;
                        m_ns.txtSerialNo.ReadOnly = true;
                        m_ns.groupBoxOther.Enabled = true;
                        break;
                    case "Manager":
                        m_ns.btnReadSerialNo.Enabled = false;
                        m_ns.btnSetSerialNo.Enabled = false;
                        m_ns.txtSerialNo.Enabled = false;
                        m_ns.groupBoxOther.Enabled = false;
                        break;
                    case "Users":
                        m_ns.btnReadSerialNo.Enabled = false;
                        m_ns.btnSetSerialNo.Enabled = false;
                        m_ns.txtSerialNo.Enabled = false;
                        m_ns.groupBoxOther.Enabled = false;
                        break;
                }
                //ns.SerialPortBack = this._CommBack;
                //ns.XmlPath = this.nodeConfigPath;
                m_ns.ShowDialog();
            }
        }

        //请求上传烟雾值
        private void askToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            ToolStrip ts = tsmi.Owner;
            UserControls.Node node = (UserControls.Node)((ts as ContextMenuStrip).SourceControl);
            if (node != null)
            {
                SendData(this._Comm, null, 0xAD, 0x00, node.NodeNo);
            }
        }

        //请求上传气流值 4通道
        void ToolStripMenuItem6Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            ToolStrip ts = tsmi.Owner;
            UserControls.Node node = (UserControls.Node)((ts as ContextMenuStrip).SourceControl);
            if (node != null)
            {
                SendData(this._Comm, null, 0xAC, 0x00, node.NodeNo);
            }
        }

        private void MenuItemPositon_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            ToolStrip ts = tsmi.Owner;
            UserControls.Node node = (UserControls.Node)((ts as ContextMenuStrip).SourceControl);
            if (node != null)
            {
                //Forms.frmZoneMap fnm = new GSM.Forms.frmZoneMap();
                //fnm._NodeConfigPath = this.nodeConfigPath;
                //fnm._ZoneMapConfigPath = this.nodeMapConfigPath;
                //fnm._NodeID = node.NodeNo;
                //fnm._NodeStatusColor = node.panelFire1.BackColor;
                //fnm.ShowDialog(); 

                string zoneMapID = string.Empty;
                //XmlDocument xdN = new XmlDocument();
                //xdN.Load(this.nodeConfigPath);
                ////读取节点的区域地图ID
                //XmlNode xnN = xdN.SelectSingleNode("/nodeGroup/node[@ID='" + node.NodeNo + "']");
                //if (xnN != null)
                //    zoneMapID = xnN.Attributes["zoneMapID"].Value;


                GoDexData.BLL.nodeInfo bllNi = new GoDexData.BLL.nodeInfo();
                GoDexData.Model.nodeInfo modelNi = bllNi.GetModel(int.Parse(node.NodeNo));
                string areamapID = modelNi.areaMapPath;//存储areaMapID 
                if (!string.IsNullOrEmpty(areamapID))
                {
                    GoDexData.BLL.areaMap bllAreaMap = new GoDexData.BLL.areaMap();
                    GoDexData.Model.areaMap modelAreaMap = bllAreaMap.GetModel(int.Parse(areamapID));

                    if (modelAreaMap != null)
                    {
                        if (File.Exists(modelAreaMap.areaMapPath))
                        {
                            //区域地图   
                            if (dc_zoneMap == null)
                            {
                                dc_zoneMap = new Forms.frmZoneMap(node.NodeNo);
                                dc_zoneMap.Text = "区域地图-" + node.NodeNo;
                                dc_zoneMap.Name = "MapContainer";
                                dc_zoneMap.HideOnClose = true;
                                dc_zoneMap.CloseButton = true;
                                dc_zoneMap.CloseButtonVisible = true;
                                dc_zoneMap.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.Document;
                                dc_zoneMap.Show(this.dockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
                            }
                            else
                            {
                                dc_zoneMap.Dispose();
                                dc_zoneMap = new Forms.frmZoneMap(node.NodeNo);
                                dc_zoneMap.Text = "区域地图-" + node.NodeNo;
                                dc_zoneMap.Name = "MapContainer";
                                dc_zoneMap.HideOnClose = true;
                                dc_zoneMap.CloseButton = true;
                                dc_zoneMap.CloseButtonVisible = true;
                                dc_zoneMap.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.Document;
                                dc_zoneMap.Show(this.dockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
                            }
                        }
                    }

                }
                else
                {
                    MessageBox.Show("未设置区域图,请您设置!");
                    toolStripButton11_Click(sender, e);
                    return;
                }
            }
        }



        void MenuItemStopClick(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            ToolStrip ts = tsmi.Owner;
            UserControls.Node node = (UserControls.Node)((ts as ContextMenuStrip).SourceControl);

            if (node != null)
            {
                switch (node.lblAddr.Text)
                {
                    case "设备类型:单管单区":
                        node.lblAire2.Enabled = false;
                        node.lblAire2.BackColor = Color.Gainsboro;
                        node.lblAire3.Enabled = false;
                        node.lblAire3.BackColor = Color.Gainsboro;
                        node.lblAire4.Enabled = false;
                        node.lblAire4.BackColor = Color.Gainsboro;

                        node.lblFire2.Enabled = false;
                        node.lblFire2.BackColor = Color.Gainsboro;
                        node.lblFire3.Enabled = false;
                        node.lblFire3.BackColor = Color.Gainsboro;
                        node.lblFire4.Enabled = false;
                        node.lblFire4.BackColor = Color.Gainsboro;
                        break;
                    case "设备类型:四管单区":
                        node.lblFire2.Enabled = false;
                        node.lblFire2.BackColor = Color.Gainsboro;
                        node.lblFire3.Enabled = false;
                        node.lblFire3.BackColor = Color.Gainsboro;
                        node.lblFire4.Enabled = false;
                        node.lblFire4.BackColor = Color.Gainsboro;
                        break;
                    case "设备类型:四管四区":
                        break;
                    default:
                        break;
                }
                switch (this._userRole)
                {
                    case "Administrator":
                    case "Agency":
                    case "Manager":
                        SendData(this._Comm, null, 0xAC, 0x00, node.NodeNo);
                        //this.richTextBox1.AppendText( "命令: " + Cls.Method.ByteToString(SendData(this._Comm, null, 0xAC, 0x00, node.NodeNo)) + " \r\n";
                        break;
                    default:
                        SendData(this._Comm, null, 0xAC, 0x00, node.NodeNo);
                        break;
                }
                //Thread.Sleep(200);
                //if (isSuccess)
                //    isSuccess = false;
                //else
                //{
                //    this.richTextBox1.AppendText( "命令重发: " + Cls.Method.ByteToString(SendData(this._Comm, null, 0xA1, 0x00, node.NodeNo)) + " \r\n";
                //}
                this.richTextBox1.AppendText("-监控节点-" + node.NodeNo + " 停运![" + DateTime.Now.ToString() + "] \r\n");
                //Cls.Method.writeLog("-监控节点- " + node.NodeNo + " 停运![" + DateTime.Now.ToString() + "]");
                writeLogData(int.Parse(node.NodeNo), "停运", this._userRole, DateTime.Now, "-");
                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                richTextBox1.Focus();
            }
        }

        void MenuItemSeperateClick(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            ToolStrip ts = tsmi.Owner;
            UserControls.Node node = (UserControls.Node)((ts as ContextMenuStrip).SourceControl);
            if (node != null)
            {
                switch (node.lblAddr.Text)
                {
                    case "设备类型:单管单区":
                        node.lblAire2.Enabled = false;
                        node.lblAire2.BackColor = Color.Gainsboro;
                        node.lblAire3.Enabled = false;
                        node.lblAire3.BackColor = Color.Gainsboro;
                        node.lblAire4.Enabled = false;
                        node.lblAire4.BackColor = Color.Gainsboro;

                        node.lblFire2.Enabled = false;
                        node.lblFire2.BackColor = Color.Gainsboro;
                        node.lblFire3.Enabled = false;
                        node.lblFire3.BackColor = Color.Gainsboro;
                        node.lblFire4.Enabled = false;
                        node.lblFire4.BackColor = Color.Gainsboro;
                        break;
                    case "设备类型:四管单区":
                        node.lblFire2.Enabled = false;
                        node.lblFire2.BackColor = Color.Gainsboro;
                        node.lblFire3.Enabled = false;
                        node.lblFire3.BackColor = Color.Gainsboro;
                        node.lblFire4.Enabled = false;
                        node.lblFire4.BackColor = Color.Gainsboro;
                        break;
                    case "设备类型:四管四区":
                        break;
                    default:
                        break;
                }
                switch (this._userRole)
                {
                    case "Administrator":
                    case "Agency":
                    case "Manager":
                        SendData(this._Comm, null, 0xA3, 0x00, node.NodeNo);
                        //this.richTextBox1.AppendText( "命令: " + Cls.Method.ByteToString(SendData(this._Comm, null, 0xA3, 0x00, node.NodeNo)) + " \r\n";
                        break;
                    default:
                        SendData(this._Comm, null, 0xA3, 0x00, node.NodeNo);
                        break;
                }

                this.richTextBox1.AppendText("-监控节点-" + node.NodeNo + " 隔离![" + DateTime.Now.ToString() + "] \r\n");
                //Cls.Method.writeLog("-监控节点- " + node.NodeNo + " 隔离![" + DateTime.Now.ToString() + "]");
                writeLogData(int.Parse(node.NodeNo), "隔离", this._userRole, DateTime.Now, "-");
                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                richTextBox1.Focus();
            }

        }

        void MenuItemAddClick(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            ToolStrip ts = tsmi.Owner;
            UserControls.Node node = (UserControls.Node)((ts as ContextMenuStrip).SourceControl);
            if (node != null)
            {
                switch (node.lblAddr.Text)
                {
                    case "设备类型:单管单区":
                        node.lblAire2.Enabled = false;
                        node.lblAire2.BackColor = Color.Gainsboro;
                        node.lblAire3.Enabled = false;
                        node.lblAire3.BackColor = Color.Gainsboro;
                        node.lblAire4.Enabled = false;
                        node.lblAire4.BackColor = Color.Gainsboro;

                        node.lblFire2.Enabled = false;
                        node.lblFire2.BackColor = Color.Gainsboro;
                        node.lblFire3.Enabled = false;
                        node.lblFire3.BackColor = Color.Gainsboro;
                        node.lblFire4.Enabled = false;
                        node.lblFire4.BackColor = Color.Gainsboro;
                        break;
                    case "设备类型:四管单区":
                        node.lblFire2.Enabled = false;
                        node.lblFire2.BackColor = Color.Gainsboro;
                        node.lblFire3.Enabled = false;
                        node.lblFire3.BackColor = Color.Gainsboro;
                        node.lblFire4.Enabled = false;
                        node.lblFire4.BackColor = Color.Gainsboro;
                        break;
                    case "设备类型:四管四区":
                        break;
                    default:
                        break;
                }
                switch (this._userRole)
                {
                    case "Administrator":
                    case "Agency":
                    case "Manager":
                        SendData(this._Comm, null, 0xA5, 0x00, node.NodeNo);
                        //this.richTextBox1.AppendText( "命令: " + Cls.Method.ByteToString(SendData(this._Comm, null, 0xA5, 0x00, node.NodeNo)) + " \r\n";
                        break;
                    default:

                        SendData(this._Comm, null, 0xA5, 0x00, node.NodeNo);
                        break;
                }
                //Thread.Sleep(200);
                //if (isSuccess)
                //    isSuccess = false;
                //else
                //{
                //    this.richTextBox1.AppendText( "命令重发: " + Cls.Method.ByteToString(SendData(this._Comm, null, 0xA1, 0x00, node.NodeNo)) + " \r\n";
                //}
                this.richTextBox1.AppendText("-监控节点-" + node.NodeNo + " 解除隔离![" + DateTime.Now.ToString() + "] \r\n");
                //Cls.Method.writeLog("-监控节点- " + node.NodeNo + " 解除隔离![" + DateTime.Now.ToString() + "]");
                writeLogData(int.Parse(node.NodeNo), "解除隔离", this._userRole, DateTime.Now, "-");
                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                richTextBox1.Focus();
            }
        }

        void MenuItemMuteClick(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            ToolStrip ts = tsmi.Owner;
            UserControls.Node node = (UserControls.Node)((ts as ContextMenuStrip).SourceControl);
            if (node != null)
            {
                switch (node.lblAddr.Text)
                {
                    case "设备类型:单管单区":
                        node.lblAire2.Enabled = false;
                        node.lblAire2.BackColor = Color.Gainsboro;
                        node.lblAire3.Enabled = false;
                        node.lblAire3.BackColor = Color.Gainsboro;
                        node.lblAire4.Enabled = false;
                        node.lblAire4.BackColor = Color.Gainsboro;
                        node.lblFire2.Enabled = false;
                        node.lblFire2.BackColor = Color.Gainsboro;
                        node.lblFire3.Enabled = false;
                        node.lblFire3.BackColor = Color.Gainsboro;
                        node.lblFire4.Enabled = false;
                        node.lblFire4.BackColor = Color.Gainsboro;
                        break;
                    case "设备类型:四管单区":
                        node.lblFire2.Enabled = false;
                        node.lblFire2.BackColor = Color.Gainsboro;
                        node.lblFire3.Enabled = false;
                        node.lblFire3.BackColor = Color.Gainsboro;
                        node.lblFire4.Enabled = false;
                        node.lblFire4.BackColor = Color.Gainsboro;
                        break;
                    case "设备类型:四管四区":
                        break;
                    default:
                        break;
                }
                switch (this._userRole)
                {
                    case "Administrator":
                    case "Agency":
                    case "Manager":
                        SendData(this._Comm, null, 0xA4, 0x00, node.NodeNo);
                        //this.richTextBox1.AppendText( "命令: " + Cls.Method.ByteToString(SendData(this._Comm, null, 0xA4, 0x00, node.NodeNo)) + " \r\n";
                        break;
                    default:
                        SendData(this._Comm, null, 0xA4, 0x00, node.NodeNo);
                        break;
                }

                this.richTextBox1.AppendText("-监控节点-" + node.NodeNo + " 静音![" + DateTime.Now.ToString() + "] \r\n");
                //Cls.Method.writeLog("-监控节点- " + node.NodeNo + " 静音![" + DateTime.Now.ToString() + "]");
                writeLogData(int.Parse(node.NodeNo), "静音", this._userRole, DateTime.Now, "-");
                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                richTextBox1.Focus();
            }

        }

        private void allMap_Click(object sender, EventArgs e)
        {
            if (dc_Map == null)
            {
                dc_Map = new Forms.frmFullMap();
                Forms.frmFullMap f = dc_Map as Forms.frmFullMap;
                dc_Map.Text = "全局地图";
                dc_Map.Name = "MapContainer";
                dc_Map.HideOnClose = true;
                dc_Map.CloseButton = true;
                dc_Map.CloseButtonVisible = true;
                dc_Map.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.Document;
                dc_Map.Show(this.dockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
            }
            else
            {
                dc_Map.Dispose();
                dc_Map = new Forms.frmFullMap();
                dc_Map.Text = "全局地图";
                dc_Map.Name = "MapContainer";
                dc_Map.HideOnClose = true;
                dc_Map.CloseButton = true;
                dc_Map.CloseButtonVisible = true;
                dc_Map.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.Document;
                dc_Map.Show(this.dockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
            }
            Forms.frmFullMap frmfmap = dc_Map as Forms.frmFullMap;
            if (_userRole == "Users")
                frmfmap.toolStrip1.Enabled = false;
        }

        void TsmiCurveClick(object sender, EventArgs e)
        {

        }

        void TsmiClearClick(object sender, EventArgs e)
        {
            this.listView1.Items.Clear();
            this.richTextBox1.Text = "";
        }

        void ToolStripMenuItem5Click(object sender, EventArgs e)
        {
            new Forms.frmWarning().ShowDialog();
        }

        //删除节点
        private void tsmiDelete_Click(object sender, EventArgs e)
        {
            try
            {
                //XmlDocument xd = new XmlDocument();
                //xd.Load(this.nodeConfigPath);
                ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
                ToolStrip ts = tsmi.Owner;
                UserControls.Node node = (UserControls.Node)((ts as ContextMenuStrip).SourceControl);
                //XmlNode xn = xd.SelectSingleNode("/nodeGroup/node[@ID='" + node.NodeNo + "']"); 
                if (node != null)
                {
                    if (DialogResult.OK == MessageBox.Show("确认删除该节点?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
                    {
                        GoDexData.BLL.nodeInfo bllNi = new GoDexData.BLL.nodeInfo();
                        GoDexData.BLL.nodeSet bllNs = new GoDexData.BLL.nodeSet();
                        bllNi.Delete(int.Parse(node.NodeNo));
                        bllNs.Delete(int.Parse(node.NodeNo));
                        node.Dispose();
                        CreateNodeTree();
                    }
                }
            }
            catch (Exception ee) { MessageBox.Show(ee.ToString()); }
        }

        void MenuItemLogClick(object sender, EventArgs e)
        {

        }

        private void orderTsmi_Click(object sender, EventArgs e)
        {
            //CreateNodesView(this.nodeConfigPath);
        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            this.alarmSpeaker.Stop();
        }

        //委托的方法
        private void RequireData(SerialPort serialPort, Byte data1, Byte data2, string nodeNo, string tipText, RichTextBox rtb, FlowLayoutPanel flp, int arrindex)
        {

            //先将此节点烟雾值清0
            Control[] cc = flp.Controls.Find(nodeNo, false);
            UserControls.Node _node;
            if (cc.Length > 0)
            {
                _node = (UserControls.Node)cc[0];
                _node.Firevalue1 = 0;
                _node.Airevalue1 = 0;
                SendData(serialPort, null, data1, data2, _node.NodeNo);
                rtb.AppendText("请求节点: " + _node.NodeNo + " " + tipText + "\r\n");
                rtb.SelectionStart = rtb.Text.Length;
                rtb.Focus();
                rtb.Refresh();
            }
        }

        private void SendAskData(SerialPort serialPort, Byte data1, Byte data2, string nodeNo, string tipText, RichTextBox rtb)
        {
            SendData(serialPort, null, data1, data2, nodeNo);
            rtb.AppendText("请求节点: " + nodeNo + " " + tipText + "\r\n");
            rtb.SelectionStart = this.richTextBox1.Text.Length;
            rtb.Focus();
        }

        //请求的线程
        Thread askdata;
        //线程的方法
        void _askdata1(object o)
        {
            try
            {
                UserControls.Node node = (UserControls.Node)o;
                if (this.InvokeRequired)
                {
                    this.Invoke(new askAireFireSet(SendAskData), new object[] { this._Comm, Convert.ToByte(0xB6), Convert.ToByte(0x01), node.NodeNo, "通道1气流低阈值", this.richTextBox1 });
                    Thread.Sleep(100);
                    this.Invoke(new askAireFireSet(SendAskData), new object[] { this._Comm, Convert.ToByte(0xB7), Convert.ToByte(0x01), node.NodeNo, "通道1气流高阈值", this.richTextBox1 });
                    Thread.Sleep(100);
                    this.Invoke(new askAireFireSet(SendAskData), new object[] { this._Comm, Convert.ToByte(0xB8), Convert.ToByte(0x01), node.NodeNo, "通道1一级火警阈值", this.richTextBox1 });
                    Thread.Sleep(100);
                    this.Invoke(new askAireFireSet(SendAskData), new object[] { this._Comm, Convert.ToByte(0xB9), Convert.ToByte(0x01), node.NodeNo, "通道1二级火警阈值", this.richTextBox1 });
                    Thread.Sleep(100);
                    this.Invoke(new askAireFireSet(SendAskData), new object[] { this._Comm, Convert.ToByte(0xBA), Convert.ToByte(0x01), node.NodeNo, "通道1三级火警阈值", this.richTextBox1 });
                    Thread.Sleep(100);
                    this.Invoke(new askAireFireSet(SendAskData), new object[] { this._Comm, Convert.ToByte(0xBD), Convert.ToByte(0x01), node.NodeNo, "通道1四级火警阈值", this.richTextBox1 });
                }
                askdata = null;
            }
            catch (Exception ee) { throw ee; }
        }

        void _askdata2(object o)
        {
            try
            {
                UserControls.Node node = (UserControls.Node)o;
                if (this.InvokeRequired)
                {
                    this.Invoke(new askAireFireSet(SendAskData), new object[] { this._Comm, Convert.ToByte(0xB6), Convert.ToByte(0x02), node.NodeNo, "通道2气流低阈值", this.richTextBox1 });
                    Thread.Sleep(100);
                    this.Invoke(new askAireFireSet(SendAskData), new object[] { this._Comm, Convert.ToByte(0xB7), Convert.ToByte(0x02), node.NodeNo, "通道2气流高阈值", this.richTextBox1 });
                    Thread.Sleep(100);
                    this.Invoke(new askAireFireSet(SendAskData), new object[] { this._Comm, Convert.ToByte(0xB8), Convert.ToByte(0x02), node.NodeNo, "通道2一级火警阈值", this.richTextBox1 });
                    Thread.Sleep(100);
                    this.Invoke(new askAireFireSet(SendAskData), new object[] { this._Comm, Convert.ToByte(0xB9), Convert.ToByte(0x02), node.NodeNo, "通道2二级火警阈值", this.richTextBox1 });
                    Thread.Sleep(100);
                    this.Invoke(new askAireFireSet(SendAskData), new object[] { this._Comm, Convert.ToByte(0xBA), Convert.ToByte(0x02), node.NodeNo, "通道2三级火警阈值", this.richTextBox1 });
                    Thread.Sleep(100);
                    this.Invoke(new askAireFireSet(SendAskData), new object[] { this._Comm, Convert.ToByte(0xBD), Convert.ToByte(0x02), node.NodeNo, "通道2四级火警阈值", this.richTextBox1 });
                }
                askdata = null;
            }
            catch { }
        }

        void _askdata3(object o)
        {
            try
            {
                UserControls.Node node = (UserControls.Node)o;
                if (this.InvokeRequired)
                {
                    this.Invoke(new askAireFireSet(SendAskData), new object[] { this._Comm, Convert.ToByte(0xB6), Convert.ToByte(0x03), node.NodeNo, "通道3气流低阈值", this.richTextBox1 });
                    Thread.Sleep(100);
                    this.Invoke(new askAireFireSet(SendAskData), new object[] { this._Comm, Convert.ToByte(0xB7), Convert.ToByte(0x03), node.NodeNo, "通道3气流高阈值", this.richTextBox1 });
                    Thread.Sleep(100);
                    this.Invoke(new askAireFireSet(SendAskData), new object[] { this._Comm, Convert.ToByte(0xB8), Convert.ToByte(0x03), node.NodeNo, "通道3一级火警阈值", this.richTextBox1 });
                    Thread.Sleep(100);
                    this.Invoke(new askAireFireSet(SendAskData), new object[] { this._Comm, Convert.ToByte(0xB9), Convert.ToByte(0x03), node.NodeNo, "通道3二级火警阈值", this.richTextBox1 });
                    Thread.Sleep(100);
                    this.Invoke(new askAireFireSet(SendAskData), new object[] { this._Comm, Convert.ToByte(0xBA), Convert.ToByte(0x03), node.NodeNo, "通道3三级火警阈值", this.richTextBox1 });
                    Thread.Sleep(100);
                    this.Invoke(new askAireFireSet(SendAskData), new object[] { this._Comm, Convert.ToByte(0xBD), Convert.ToByte(0x03), node.NodeNo, "通道3四级火警阈值", this.richTextBox1 });
                }
                askdata = null;
            }
            catch { }
        }

        void _askdata4(object o)
        {
            try
            {
                UserControls.Node node = (UserControls.Node)o;
                if (this.InvokeRequired)
                {
                    this.Invoke(new askAireFireSet(SendAskData), new object[] { this._Comm, Convert.ToByte(0xB6), Convert.ToByte(0x04), node.NodeNo, "通道4气流低阈值", this.richTextBox1 });
                    Thread.Sleep(100);
                    this.Invoke(new askAireFireSet(SendAskData), new object[] { this._Comm, Convert.ToByte(0xB7), Convert.ToByte(0x04), node.NodeNo, "通道4气流高阈值", this.richTextBox1 });
                    Thread.Sleep(100);
                    this.Invoke(new askAireFireSet(SendAskData), new object[] { this._Comm, Convert.ToByte(0xB8), Convert.ToByte(0x04), node.NodeNo, "通道4一级火警阈值", this.richTextBox1 });
                    Thread.Sleep(100);
                    this.Invoke(new askAireFireSet(SendAskData), new object[] { this._Comm, Convert.ToByte(0xB9), Convert.ToByte(0x04), node.NodeNo, "通道4二级火警阈值", this.richTextBox1 });
                    Thread.Sleep(100);
                    this.Invoke(new askAireFireSet(SendAskData), new object[] { this._Comm, Convert.ToByte(0xBA), Convert.ToByte(0x04), node.NodeNo, "通道4三级火警阈值", this.richTextBox1 });
                    Thread.Sleep(100);
                    this.Invoke(new askAireFireSet(SendAskData), new object[] { this._Comm, Convert.ToByte(0xBD), Convert.ToByte(0x04), node.NodeNo, "通道4四级火警阈值", this.richTextBox1 });
                }
                askdata = null;
            }
            catch { }
        }

        void _askdata5(object o)
        {
            try
            {
                UserControls.Node node = (UserControls.Node)o;
                if (this.InvokeRequired)
                {
                    this.Invoke(new askAireFireSet(SendAskData), new object[] { this._Comm, Convert.ToByte(0xA7), Convert.ToByte(0x05), node.NodeNo, "A1火警延时报警值", this.richTextBox1 });
                    Thread.Sleep(100);
                    this.Invoke(new askAireFireSet(SendAskData), new object[] { this._Comm, Convert.ToByte(0xA7), Convert.ToByte(0x06), node.NodeNo, "A2火警延时报警值", this.richTextBox1 });
                    Thread.Sleep(100);
                    this.Invoke(new askAireFireSet(SendAskData), new object[] { this._Comm, Convert.ToByte(0xA7), Convert.ToByte(0x07), node.NodeNo, "A3火警延时报警值", this.richTextBox1 });
                    Thread.Sleep(100);
                    this.Invoke(new askAireFireSet(SendAskData), new object[] { this._Comm, Convert.ToByte(0xA7), Convert.ToByte(0x08), node.NodeNo, "A4火警延时报警值", this.richTextBox1 });
                }
                askdata = null;
            }
            catch { }
        }

        //请求单管单区
        private void tsmiAskSet1_Click(object sender, EventArgs e)
        {
            try
            {
                ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
                ToolStrip ts = tsmi.Owner;
                UserControls.Node node = (UserControls.Node)((ts as ContextMenuStrip).SourceControl);
                if (node != null)
                {
                    if (askdata == null)
                    {
                        askdata = new Thread(new ParameterizedThreadStart(_askdata1));
                        askdata.Start(node);
                    }
                }
            }
            catch { }
        }

        private void tsmiAskSet2_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            ToolStrip ts = tsmi.Owner;
            UserControls.Node node = (UserControls.Node)((ts as ContextMenuStrip).SourceControl);
            if (node != null)
            {
                if (node.Addr != "设备类型:单管单区")
                {
                    if (askdata == null)
                    {
                        askdata = new Thread(new ParameterizedThreadStart(_askdata2));
                        askdata.Start(node);
                    }
                }
                else
                {
                    MessageBox.Show("单管单区设备无此通道");
                }
            }
        }

        private void tsmiAskSet3_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            ToolStrip ts = tsmi.Owner;
            UserControls.Node node = (UserControls.Node)((ts as ContextMenuStrip).SourceControl);
            if (node != null)
            {
                if (node.Addr != "设备类型:单管单区")
                {
                    if (askdata == null)
                    {
                        askdata = new Thread(new ParameterizedThreadStart(_askdata3));
                        askdata.Start(node);
                    }
                }
                else
                {
                    MessageBox.Show("单管单区设备无此通道");
                }
            }
        }

        private void tsmiAskSet4_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            ToolStrip ts = tsmi.Owner;
            UserControls.Node node = (UserControls.Node)((ts as ContextMenuStrip).SourceControl);
            if (node != null)
            {
                if (node.Addr != "设备类型:单管单区")
                {
                    if (askdata == null)
                    {
                        askdata = new Thread(new ParameterizedThreadStart(_askdata4));
                        askdata.Start(node);
                    }
                }
                else
                {
                    MessageBox.Show("单管单区设备无此通道");
                }
            }
        }

        private void tsmiUserRole_Click(object sender, EventArgs e)
        {
            new Forms.frmSetUserPwd().ShowDialog();
        }

        /*
        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string tag = (string)e.ClickedItem.Tag;
            switch (tag)
            {
                case "Add":
                    AddLayer();
                    break;
                case "Remove":
                    legend.Layers.Remove(legend.SelectedLayer);
                    legend.Refresh();
                    break;
                case "Clear":
                    legend.Layers.Clear();
                    break;
                case "ZoomIn":
                    axMap.CursorMode = MapWinGIS.tkCursorMode.cmZoomIn;
                    break;
                case "ZoomOut":
                    axMap.CursorMode = MapWinGIS.tkCursorMode.cmZoomOut;
                    break;
                case "Pan":
                    axMap.CursorMode = MapWinGIS.tkCursorMode.cmPan;
                    break;
                case "FullExtents":
                    axMap.ZoomToMaxExtents();
                    break;
            }
        }
         
         */

        /*
        private void AddLayer()
        {
            MapWinGIS.Shapefile shpfile;
            MapWinGIS.Grid grid;
            MapWinGIS.GridColorScheme gridScheme = null;
            MapWinGIS.Image image = null;
            MapWinGIS.Utils utils;
            OpenFileDialog openDlg = new OpenFileDialog();
            int handle;
            string ext;

            //initialize dialog
            openDlg.Filter = "Supported Formats|*.shp;*.bgd;*asc;*.jpg;*.png;*.bmp;*.map;*.gif;*.jp2;*.sid;*.tif|Image Files(*.jpg;*.png;*.bmp;*.map;*.gif;*.jp2;*.sid;*.tif)|*.jpg;*.png;*.bmp;*.map;*.gif;*.jp2;*.sid;*.tif|Shapefile (*.shp)|*.shp|Binary Grids (*.bgd)|*.bgd|ASCII Grids (*.asc)|*.asc";
            openDlg.CheckFileExists = true;

            if (openDlg.ShowDialog(this) == DialogResult.OK)
            {
                //get the extension of the file
                ext = System.IO.Path.GetExtension(openDlg.FileName);

                if (ext == ".bgd" || ext == ".asc")
                {
                    utils = new MapWinGIS.UtilsClass();
                    gridScheme = new MapWinGIS.GridColorScheme();
                    grid = new MapWinGIS.GridClass();

                    //open the grid
                    grid.Open(openDlg.FileName, MapWinGIS.GridDataType.UnknownDataType, true, MapWinGIS.GridFileType.UseExtension, this);

                    //create a coloring scheme for the image
                    gridScheme.UsePredefined(System.Convert.ToDouble(grid.Minimum), System.Convert.ToDouble(grid.Maximum), MapWinGIS.PredefinedColorScheme.SummerMountains);

                    //convert the grid to a image
                    image = utils.GridToImage(grid, gridScheme, this);

                    //add the image to the legend and map
                    handle = legend.Layers.Add(image, true);

                    if (legend.Layers.IsValidHandle(handle))
                    {
                        //set the layer name
                        legend.Map.set_LayerName(handle, System.IO.Path.GetFileNameWithoutExtension(grid.Filename));

                        //set's the legend layer type, this displays a default icon in the legend (line shapefile, point shapefile,polygon shapefile,grid,image)
                        legend.Layers.ItemByHandle(handle).Type = MapWindow.Interfaces.eLayerType.Grid;

                        //set coloring scheme
                        //when applying a coloring scheme to a shapfile use axMap1.ApplyLegendColors(ShapefileColorScheme)
                        //when applying a coloring scheme for a grid or image use axMap1.SetImageLayerColorScheme(handle,GridColorScheme);
                        axMap.SetImageLayerColorScheme(legend.SelectedLayer, gridScheme);
                        legend.Layers.ItemByHandle(legend.SelectedLayer).Refresh();
                    }
                    //close the grid
                    grid.Close();
                }
                else if (ext == ".shp")
                {
                    shpfile = new MapWinGIS.ShapefileClass();

                    //open the shapefile
                    shpfile.Open(openDlg.FileName, this);

                    //add the shapefile to the map and legend
                    handle = legend.Layers.Add(shpfile, true);

                    //set the layer name
                    legend.Map.set_LayerName(handle, System.IO.Path.GetFileNameWithoutExtension(shpfile.Filename));
                }
                else if (ext == ".jpg" || ext == ".png" || ext == ".bmp" || ext == ".map" || ext == ".gif" || ext == ".jp2" || ext == ".sid" || ext == ".tif")
                {
                    image = new MapWinGIS.Image();
                    image.Open(openDlg.FileName, MapWinGIS.ImageType.USE_FILE_EXTENSION, true, this);
                    handle = legend.Layers.Add(image, true);
                    legend.Map.set_LayerName(handle, System.IO.Path.GetFileNameWithoutExtension(image.Filename));
                }

            }

        }
        */
        private void tsbtnLogout_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == MessageBox.Show("确定注销当前用户?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
            {
                this._userRole = string.Empty;
                this.tsslblUserRole.Text = string.Empty;
                TsmiClearClick(sender, e);
                this.Dispose();
                new frmLoginIn().ShowDialog();
            }
        }

        private void tsmiExit_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == MessageBox.Show("确认退出软件?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
            {
                this.frmMain_FormClosing(sender, new FormClosingEventArgs(CloseReason.UserClosing, true));
            }
        }

        private void MenuItemChart_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            ToolStrip ts = tsmi.Owner;
            UserControls.Node node = (UserControls.Node)((ts as ContextMenuStrip).SourceControl);
            //曲线
            if (node != null)
            {
                if (dc_Curve == null)
                {
                    dc_Curve = new ShowCurve.frmCurveA(node.NodeNo, node.lblAddr.Text); // new Forms.frmCurve(node.NodeNo,node.lblAddr.Text);//传递节点ID，会显示当日的火警和气流曲线；
                    dc_Curve.Text = "节点-" + node.NodeNo + "-曲线";
                    dc_Curve.Name = "Curve";
                    dc_Curve.HideOnClose = true;
                    dc_Curve.CloseButton = true;
                    dc_Curve.CloseButtonVisible = true;
                    dc_Curve.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.Document;
                    dc_Curve.Show(this.dockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
                }
                else
                {
                    dc_Curve.Dispose();
                    dc_Curve = new ShowCurve.frmCurveA(node.NodeNo, node.lblAddr.Text); //传递节点ID，会显示当日的火警和气流曲线；
                    dc_Curve.Text = "节点-" + node.NodeNo + "-曲线";
                    dc_Curve.Name = "Curve";
                    dc_Curve.HideOnClose = true;
                    dc_Curve.CloseButton = true;
                    dc_Curve.CloseButtonVisible = true;
                    dc_Curve.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.Document;
                    dc_Curve.Show(this.dockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
                }
            }
        }

        //注册
        private void ToolStripMenuItemReg_Click(object sender, EventArgs e)
        {
            Cls.DES des = new Cls.DES();
            string _key = Convert.ToString(Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\" + "{" + Cls.SoftRegister.getMd5("Go-Dex") + "}", Cls.SoftRegister.getMd5("md5Key"), ""));
            string publicKey = "<RSAKeyValue><Modulus>pl4q/wNkWPs7RLPYknv0CkFjoAUJosIaFcBWCN7x9g8M9f/l2aj6XDe8ehN6iCPb9ksvsQPS5t5lA1sDE3o/fxvZGuttN7DYva0Xv8x+0/mXflVCEOnnbkQ2iDEqFDr9EVowrTW9hSBkyNRSfQWojO+JFtNqOJfGesctc+pKo9s=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
            string regCode = des.DecryptString(Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\" + "{" + Cls.SoftRegister.getMd5("Go-Dex") + "}", Cls.SoftRegister.getMd5("Lisence"), "").ToString(), _key);
            string machineCode = des.DecryptString(Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\" + "{" + Cls.SoftRegister.getMd5("Go-Dex") + "}", Cls.SoftRegister.getMd5("machineCode"), "").ToString(), _key);
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(publicKey);
                RSAPKCS1SignatureDeformatter fRsapacs = new RSAPKCS1SignatureDeformatter(rsa);
                fRsapacs.SetHashAlgorithm("SHA1");
                //key是注册码
                byte[] key = Convert.FromBase64String(regCode);
                SHA1Managed sha = new SHA1Managed();
                //name为 需加密的字符串
                byte[] name = sha.ComputeHash(ASCIIEncoding.ASCII.GetBytes(machineCode));
                //判断是否成功
                if (fRsapacs.VerifySignature(name, key))
                {
                    MessageBox.Show("该产品已注册，谢谢您的使用！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    frmReg form = new frmReg();
                    form.ShowDialog();
                    return;
                }
            }
        }

        void TsmiChangeClick(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            ToolStrip ts = tsmi.Owner;
            UserControls.Node node = (UserControls.Node)((ts as ContextMenuStrip).SourceControl);
            if (node != null)
            {
                switch (node.lblAddr.Text)
                {
                    case "设备类型:单管单区":
                        node.lblAire2.Enabled = false;
                        node.lblAire2.BackColor = Color.Gainsboro;
                        node.lblAire3.Enabled = false;
                        node.lblAire3.BackColor = Color.Gainsboro;
                        node.lblAire4.Enabled = false;
                        node.lblAire4.BackColor = Color.Gainsboro;

                        node.lblFire2.Enabled = false;
                        node.lblFire2.BackColor = Color.Gainsboro;
                        node.lblFire3.Enabled = false;
                        node.lblFire3.BackColor = Color.Gainsboro;
                        node.lblFire4.Enabled = false;
                        node.lblFire4.BackColor = Color.Gainsboro;
                        break;
                    case "设备类型:四管单区":
                        node.lblFire2.Enabled = false;
                        node.lblFire2.BackColor = Color.Gainsboro;
                        node.lblFire3.Enabled = false;
                        node.lblFire3.BackColor = Color.Gainsboro;
                        node.lblFire4.Enabled = false;
                        node.lblFire4.BackColor = Color.Gainsboro;
                        break;
                    case "设备类型:四管四区":
                        break;
                    default:
                        break;
                }

                Thread.Sleep(10);
                //node.label1.Text = "正常 预警 火警1 火警2";
                //SendData(this._Comm,null,0xA2, 0x00, node.NodeNo);
                switch (this._userRole)
                {
                    case "Administrator":
                    case "Agency":
                    case "Manager":
                        SendData(this._Comm, null, 0xA7, 0x00, node.NodeNo);
                        //this.richTextBox1.AppendText( "命令: " + Cls.Method.ByteToString(SendData(this._Comm, null, 0xA7, 0x00, node.NodeNo)) + " \r\n";
                        break;
                    default:
                        SendData(this._Comm, null, 0xA7, 0x00, node.NodeNo);
                        break;
                }
                this.richTextBox1.AppendText("-监控节点-" + node.NodeNo + " 继电器极性反转![" + DateTime.Now.ToString() + "] \r\n");
                //Cls.Method.writeLog("-监控节点-" + node.NodeNo + " 继电器极性反转![" + DateTime.Now.ToString() + "]");
                Cls.Method.writeLogData(int.Parse(node.NodeNo), "继电器极性反转!", this._userRole, DateTime.Now, "-");
                //if (isSuccess)
                //    isSuccess = false;
                //else
                //{
                //    this.richTextBox1.AppendText( "命令重发: " + Cls.Method.ByteToString(SendData(this._Comm, null, 0xA2, 0x00, node.NodeNo)) + " \r\n";
                //}
                //Thread.Sleep(500);
                //SendData(this._Comm,null,0xAD,0x00,node.NodeNo);//请求上传烟雾浓度值
                //Thread.Sleep(500);
                //SendData(this._Comm,null,0xAC,0x00,node.NodeNo);//请求上传气流AD值
                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                richTextBox1.Focus();
            }
        }

        private void menuItemCutMute_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            ToolStrip ts = tsmi.Owner;
            UserControls.Node node = (UserControls.Node)((ts as ContextMenuStrip).SourceControl);
            //SendData(this._Comm, null, 0xA3, 0x00, node.NodeNo);

            //this.alarmSpeaker.Stop();
            //ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            //ToolStrip ts = tsmi.Owner;
            //UserControls.Node node = (UserControls.Node)((ts as ContextMenuStrip).SourceControl);

            ////气流
            //node.lblAire1.BackColor = Color.LightCyan;
            //node.lblAire2.BackColor = Color.LightCyan;
            //node.lblAire3.BackColor = Color.LightCyan;
            //node.lblAire4.BackColor = Color.LightCyan;
            ////火警
            //node.lblFire1.BackColor = Color.LightCyan;
            //node.lblFire2.BackColor = Color.LightCyan;
            //node.lblFire3.BackColor = Color.LightCyan;
            //node.lblFire4.BackColor = Color.LightCyan;
            if (node != null)
            {
                switch (node.lblAddr.Text)
                {
                    case "设备类型:单管单区":
                        node.lblAire2.Enabled = false;
                        node.lblAire2.BackColor = Color.Gainsboro;
                        node.lblAire3.Enabled = false;
                        node.lblAire3.BackColor = Color.Gainsboro;
                        node.lblAire4.Enabled = false;
                        node.lblAire4.BackColor = Color.Gainsboro;

                        node.lblFire2.Enabled = false;
                        node.lblFire2.BackColor = Color.Gainsboro;
                        node.lblFire3.Enabled = false;
                        node.lblFire3.BackColor = Color.Gainsboro;
                        node.lblFire4.Enabled = false;
                        node.lblFire4.BackColor = Color.Gainsboro;
                        break;
                    case "设备类型:四管单区":
                        node.lblFire2.Enabled = false;
                        node.lblFire2.BackColor = Color.Gainsboro;
                        node.lblFire3.Enabled = false;
                        node.lblFire3.BackColor = Color.Gainsboro;
                        node.lblFire4.Enabled = false;
                        node.lblFire4.BackColor = Color.Gainsboro;
                        break;
                    case "设备类型:四管四区":
                        break;
                    default:
                        break;
                }
                switch (this._userRole)
                {
                    case "Administrator":
                    case "Agency":
                    case "Manager":
                        SendData(this._Comm, null, 0xA6, 0x00, node.NodeNo);
                        //this.richTextBox1.AppendText( "命令: " + Cls.Method.ByteToString(SendData(this._Comm, null, 0xA6, 0x00, node.NodeNo)) + " \r\n";
                        break;
                    default:
                        SendData(this._Comm, null, 0xA6, 0x00, node.NodeNo);
                        break;
                }
                //Thread.Sleep(200);
                //if (isSuccess)
                //    isSuccess = false;
                //else
                //{
                //    this.richTextBox1.AppendText( "命令重发: " + Cls.Method.ByteToString(SendData(this._Comm, null, 0xA1, 0x00, node.NodeNo)) + " \r\n";
                //}
                this.richTextBox1.AppendText("-监控节点-" + node.NodeNo + " 解除静音![" + DateTime.Now.ToString() + "] \r\n");
                //Cls.Method.writeLog("-监控节点- " + node.NodeNo + " 解除静音![" + DateTime.Now.ToString() + "]");
                Cls.Method.writeLogData(int.Parse(node.NodeNo), "解除静音", this._userRole, DateTime.Now, "-");
                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                richTextBox1.Focus();
            }
        }

        private void tsmiLockFire_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            ToolStrip ts = tsmi.Owner;
            UserControls.Node node = (UserControls.Node)((ts as ContextMenuStrip).SourceControl);
            if (node != null)
            {
                switch (node.lblAddr.Text)
                {
                    case "设备类型:单管单区":
                        node.lblAire2.Enabled = false;
                        node.lblAire2.BackColor = Color.Gainsboro;
                        node.lblAire3.Enabled = false;
                        node.lblAire3.BackColor = Color.Gainsboro;
                        node.lblAire4.Enabled = false;
                        node.lblAire4.BackColor = Color.Gainsboro;

                        node.lblFire2.Enabled = false;
                        node.lblFire2.BackColor = Color.Gainsboro;
                        node.lblFire3.Enabled = false;
                        node.lblFire3.BackColor = Color.Gainsboro;
                        node.lblFire4.Enabled = false;
                        node.lblFire4.BackColor = Color.Gainsboro;
                        break;
                    case "设备类型:四管单区":
                        node.lblFire2.Enabled = false;
                        node.lblFire2.BackColor = Color.Gainsboro;
                        node.lblFire3.Enabled = false;
                        node.lblFire3.BackColor = Color.Gainsboro;
                        node.lblFire4.Enabled = false;
                        node.lblFire4.BackColor = Color.Gainsboro;
                        break;
                    case "设备类型:四管四区":
                        break;
                    default:
                        break;
                }

                Thread.Sleep(10);
                //node.label1.Text = "正常 预警 火警1 火警2";
                //SendData(this._Comm,null,0xA2, 0x00, node.NodeNo);
                switch (this._userRole)
                {
                    case "Administrator":
                    case "Agency":
                    case "Manager":
                        SendData(this._Comm, null, 0xA7, 0x01, node.NodeNo);
                        //this.richTextBox1.AppendText( "命令: " + Cls.Method.ByteToString(SendData(this._Comm, null, 0xA7, 0x01, node.NodeNo)) + " \r\n";
                        break;
                    default:
                        SendData(this._Comm, null, 0xA7, 0x01, node.NodeNo);
                        break;
                }
                this.richTextBox1.AppendText("-监控节点-" + node.NodeNo + " 火警锁定![" + DateTime.Now.ToString() + "] \r\n");
                //Cls.Method.writeLog("-监控节点-" + node.NodeNo + " 火警锁定![" + DateTime.Now.ToString() + "]");
                Cls.Method.writeLogData(int.Parse(node.NodeNo), "火警锁定", this._userRole, DateTime.Now, "-");
                //if (isSuccess)
                //    isSuccess = false;
                //else
                //{
                //    this.richTextBox1.AppendText( "命令重发: " + Cls.Method.ByteToString(SendData(this._Comm, null, 0xA2, 0x00, node.NodeNo)) + " \r\n";
                //}
                //Thread.Sleep(500);
                //SendData(this._Comm,null,0xAD,0x00,node.NodeNo);//请求上传烟雾浓度值
                //Thread.Sleep(500);
                //SendData(this._Comm,null,0xAC,0x00,node.NodeNo);//请求上传气流AD值
                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                richTextBox1.Focus();
            }
        }

        private void tsmiUnlockFire_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            ToolStrip ts = tsmi.Owner;
            UserControls.Node node = (UserControls.Node)((ts as ContextMenuStrip).SourceControl);
            if (node != null)
            {
                switch (node.lblAddr.Text)
                {
                    case "设备类型:单管单区":
                        node.lblAire2.Enabled = false;
                        node.lblAire2.BackColor = Color.Gainsboro;
                        node.lblAire3.Enabled = false;
                        node.lblAire3.BackColor = Color.Gainsboro;
                        node.lblAire4.Enabled = false;
                        node.lblAire4.BackColor = Color.Gainsboro;

                        node.lblFire2.Enabled = false;
                        node.lblFire2.BackColor = Color.Gainsboro;
                        node.lblFire3.Enabled = false;
                        node.lblFire3.BackColor = Color.Gainsboro;
                        node.lblFire4.Enabled = false;
                        node.lblFire4.BackColor = Color.Gainsboro;
                        break;
                    case "设备类型:四管单区":
                        node.lblFire2.Enabled = false;
                        node.lblFire2.BackColor = Color.Gainsboro;
                        node.lblFire3.Enabled = false;
                        node.lblFire3.BackColor = Color.Gainsboro;
                        node.lblFire4.Enabled = false;
                        node.lblFire4.BackColor = Color.Gainsboro;
                        break;
                    case "设备类型:四管四区":
                        break;
                    default:
                        break;
                }

                Thread.Sleep(10);
                //node.label1.Text = "正常 预警 火警1 火警2";
                //SendData(this._Comm,null,0xA2, 0x00, node.NodeNo);
                switch (this._userRole)
                {
                    case "Administrator":
                    case "Agency":
                    case "Manager":
                        SendData(this._Comm, null, 0xA7, 0x02, node.NodeNo);
                        //this.richTextBox1.AppendText( "命令: " + Cls.Method.ByteToString(SendData(this._Comm, null, 0xA7, 0x02, node.NodeNo)) + " \r\n";
                        break;
                    default:
                        SendData(this._Comm, null, 0xA7, 0x02, node.NodeNo);
                        break;
                }
                this.richTextBox1.AppendText("-监控节点-" + node.NodeNo + " 火警不锁定![" + DateTime.Now.ToString() + "] \r\n");
                //Cls.Method.writeLog("-监控节点-" + node.NodeNo + " 火警不锁定![" + DateTime.Now.ToString() + "]");
                Cls.Method.writeLogData(int.Parse(node.NodeNo), "火警不锁定", this._userRole, DateTime.Now, "-");
                //if (isSuccess)
                //    isSuccess = false;
                //else
                //{
                //    this.richTextBox1.AppendText( "命令重发: " + Cls.Method.ByteToString(SendData(this._Comm, null, 0xA2, 0x00, node.NodeNo)) + " \r\n";
                //}
                //Thread.Sleep(500);
                //SendData(this._Comm,null,0xAD,0x00,node.NodeNo);//请求上传烟雾浓度值
                //Thread.Sleep(500);
                //SendData(this._Comm,null,0xAC,0x00,node.NodeNo);//请求上传气流AD值
                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                richTextBox1.Focus();
            }
        }

        private void tsmiTest_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            ToolStrip ts = tsmi.Owner;
            UserControls.Node node = (UserControls.Node)((ts as ContextMenuStrip).SourceControl);
            if (node != null)
            {
                switch (node.lblAddr.Text)
                {
                    case "设备类型:单管单区":
                        node.lblAire2.Enabled = false;
                        node.lblAire2.BackColor = Color.Gainsboro;
                        node.lblAire3.Enabled = false;
                        node.lblAire3.BackColor = Color.Gainsboro;
                        node.lblAire4.Enabled = false;
                        node.lblAire4.BackColor = Color.Gainsboro;

                        node.lblFire2.Enabled = false;
                        node.lblFire2.BackColor = Color.Gainsboro;
                        node.lblFire3.Enabled = false;
                        node.lblFire3.BackColor = Color.Gainsboro;
                        node.lblFire4.Enabled = false;
                        node.lblFire4.BackColor = Color.Gainsboro;
                        break;
                    case "设备类型:四管单区":
                        node.lblFire2.Enabled = false;
                        node.lblFire2.BackColor = Color.Gainsboro;
                        node.lblFire3.Enabled = false;
                        node.lblFire3.BackColor = Color.Gainsboro;
                        node.lblFire4.Enabled = false;
                        node.lblFire4.BackColor = Color.Gainsboro;
                        break;
                    case "设备类型:四管四区":
                        break;
                    default:
                        break;
                }

                Thread.Sleep(10);
                //node.label1.Text = "正常 预警 火警1 火警2";
                //SendData(this._Comm,null,0xA2, 0x00, node.NodeNo);
                switch (this._userRole)
                {
                    case "Administrator":
                    case "Agency":
                    case "Manager":
                        SendData(this._Comm, null, 0xA7, 0x03, node.NodeNo);
                        //this.richTextBox1.AppendText( "命令: " + Cls.Method.ByteToString(SendData(this._Comm, null, 0xA7, 0x03, node.NodeNo)) + " \r\n";
                        break;
                    default:
                        SendData(this._Comm, null, 0xA7, 0x03, node.NodeNo);
                        break;
                }
                this.richTextBox1.AppendText("-监控节点-" + node.NodeNo + " 测试......![" + DateTime.Now.ToString() + "] \r\n");
                //Cls.Method.writeLog("-监控节点-" + node.NodeNo + " 测试......![" + DateTime.Now.ToString() + "]");
                Cls.Method.writeLogData(int.Parse(node.NodeNo), "测试", this._userRole, DateTime.Now, "-");
                //if (isSuccess)
                //    isSuccess = false;
                //else
                //{
                //    this.richTextBox1.AppendText( "命令重发: " + Cls.Method.ByteToString(SendData(this._Comm, null, 0xA2, 0x00, node.NodeNo)) + " \r\n";
                //}
                //Thread.Sleep(500);
                //SendData(this._Comm,null,0xAD,0x00,node.NodeNo);//请求上传烟雾浓度值
                //Thread.Sleep(500);
                //SendData(this._Comm,null,0xAC,0x00,node.NodeNo);//请求上传气流AD值
                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                richTextBox1.Focus();
            }
        }

        private void tsmiAskStatus_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            ToolStrip ts = tsmi.Owner;
            UserControls.Node node = (UserControls.Node)((ts as ContextMenuStrip).SourceControl);
            if (node != null)
            {
                switch (node.lblAddr.Text)
                {
                    case "设备类型:单管单区":
                        node.lblAire2.Enabled = false;
                        node.lblAire2.BackColor = Color.Gainsboro;
                        node.lblAire3.Enabled = false;
                        node.lblAire3.BackColor = Color.Gainsboro;
                        node.lblAire4.Enabled = false;
                        node.lblAire4.BackColor = Color.Gainsboro;

                        node.lblFire2.Enabled = false;
                        node.lblFire2.BackColor = Color.Gainsboro;
                        node.lblFire3.Enabled = false;
                        node.lblFire3.BackColor = Color.Gainsboro;
                        node.lblFire4.Enabled = false;
                        node.lblFire4.BackColor = Color.Gainsboro;
                        break;
                    case "设备类型:四管单区":
                        node.lblFire2.Enabled = false;
                        node.lblFire2.BackColor = Color.Gainsboro;
                        node.lblFire3.Enabled = false;
                        node.lblFire3.BackColor = Color.Gainsboro;
                        node.lblFire4.Enabled = false;
                        node.lblFire4.BackColor = Color.Gainsboro;
                        break;
                    case "设备类型:四管四区":
                        break;
                    default:
                        break;
                }
            }

            Thread.Sleep(10);
            //node.label1.Text = "正常 预警 火警1 火警2";
            //SendData(this._Comm,null,0xA2, 0x00, node.NodeNo);
            switch (this._userRole)
            {
                case "Administrator":
                case "Agency":
                case "Manager":
                    SendData(this._Comm, null, 0xA7, 0x04, node.NodeNo);
                    //this.richTextBox1.AppendText( "命令: " + Cls.Method.ByteToString(SendData(this._Comm, null, 0xA7, 0x04, node.NodeNo)) + " \r\n";
                    break;
                default:
                    SendData(this._Comm, null, 0xA7, 0x04, node.NodeNo);
                    break;
            }
            this.richTextBox1.AppendText("-请求上传监控节点-" + node.NodeNo + "-的状态![" + DateTime.Now.ToString() + "] \r\n");
            //Cls.Method.writeLog("-请求上传监控节点-" + node.NodeNo + "-的状态![" + DateTime.Now.ToString() + "]");
            Cls.Method.writeLogData(int.Parse(node.NodeNo), "请求上传状态", this._userRole, DateTime.Now, "-");
            //if (isSuccess)
            //    isSuccess = false;
            //else
            //{
            //    this.richTextBox1.AppendText( "命令重发: " + Cls.Method.ByteToString(SendData(this._Comm, null, 0xA2, 0x00, node.NodeNo)) + " \r\n";
            //}
            //Thread.Sleep(500);
            //SendData(this._Comm,null,0xAD,0x00,node.NodeNo);//请求上传烟雾浓度值
            //Thread.Sleep(500);
            //SendData(this._Comm,null,0xAC,0x00,node.NodeNo);//请求上传气流AD值
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.Focus();
        }

        private void tsmiAskFireTime_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            ToolStrip ts = tsmi.Owner;
            UserControls.Node node = (UserControls.Node)((ts as ContextMenuStrip).SourceControl);
            if (node != null)
            {
                if (askdata == null)
                {
                    askdata = new Thread(new ParameterizedThreadStart(_askdata5));
                    askdata.Start(node);
                }
            }
        }

        private void tsBtnReadHistory_Click(object sender, EventArgs e)
        {
            Forms.frmSetNode ns = new GSM.Forms.frmSetNode(_userRole, this);
            foreach (GSM.UserControls.Node nd in this.flowLayoutPanel1.Controls)
            {
                if (nd.IsCheck == true)
                {
                    SendData(this._Comm, null, 0xE0, 0x00, nd.NodeNo);
                }
            }
        }

        private void tsBtnShowCurve_Click(object sender, EventArgs e)
        {
            Forms.frmSetNode ns = new GSM.Forms.frmSetNode(_userRole, this);
            foreach (GSM.UserControls.Node nd in this.flowLayoutPanel1.Controls)
            {
                if (nd.IsCheck == true)
                {
                    //曲线
                    if (dc_Curve == null)
                    {
                        dc_Curve = new ShowCurve.frmCurveA(nd.NodeNo, nd.lblAddr.Text); // new Forms.frmCurve(node.NodeNo,node.lblAddr.Text);//传递节点ID，会显示当日的火警和气流曲线；
                        dc_Curve.Text = "节点-" + nd.NodeNo + "-曲线";
                        dc_Curve.Name = "Curve";
                        dc_Curve.HideOnClose = true;
                        dc_Curve.CloseButton = true;
                        dc_Curve.CloseButtonVisible = true;
                        dc_Curve.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.Document;
                        dc_Curve.Show(this.dockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
                    }
                    else
                    {
                        dc_Curve.Dispose();
                        dc_Curve = new ShowCurve.frmCurveA(nd.NodeNo, nd.lblAddr.Text); //传递节点ID，会显示当日的火警和气流曲线；
                        dc_Curve.Text = "节点-" + nd.NodeNo + "-曲线";
                        dc_Curve.Name = "Curve";
                        dc_Curve.HideOnClose = true;
                        dc_Curve.CloseButton = true;
                        dc_Curve.CloseButtonVisible = true;
                        dc_Curve.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.Document;
                        dc_Curve.Show(this.dockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
                    }
                }
            }
        }

        private void btnRefreshNode_Click(object sender, EventArgs e)
        {
            CreateNodeTree(this.treeViewNodes);
        }

        private void tsmiPortSet_Click(object sender, EventArgs e)
        {
            Forms.frmPort fp = new GSM.Forms.frmPort(this);
            fp.ShowDialog();
        }

        private void tsmiVoiseSet_Click(object sender, EventArgs e)
        {
            new Forms.frmWarning().ShowDialog();
        }

        private void tsmiUsers_Click(object sender, EventArgs e)
        {
            Forms.frmSetUserPwd fsup = new Forms.frmSetUserPwd();
            fsup._UserRole = _userRole;
            fsup.ShowDialog();
        }

        private void tsmiReg_Click(object sender, EventArgs e)
        {
            //Cls.DES des = new Cls.DES();
            //string _key = Convert.ToString(Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\" + "{" + Cls.SoftRegister.getMd5("Go-Dex") + "}", Cls.SoftRegister.getMd5("md5Key"), ""));
            //string publicKey = "<RSAKeyValue><Modulus>pl4q/wNkWPs7RLPYknv0CkFjoAUJosIaFcBWCN7x9g8M9f/l2aj6XDe8ehN6iCPb9ksvsQPS5t5lA1sDE3o/fxvZGuttN7DYva0Xv8x+0/mXflVCEOnnbkQ2iDEqFDr9EVowrTW9hSBkyNRSfQWojO+JFtNqOJfGesctc+pKo9s=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
            //string regCode = des.DecryptString(Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\" + "{" + Cls.SoftRegister.getMd5("Go-Dex") + "}", Cls.SoftRegister.getMd5("Lisence"), "").ToString(), _key);
            //string machineCode = des.DecryptString(Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\" + "{" + Cls.SoftRegister.getMd5("Go-Dex") + "}", Cls.SoftRegister.getMd5("machineCode"), "").ToString(), _key);
            //using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            //{
            //    rsa.FromXmlString(publicKey);
            //    RSAPKCS1SignatureDeformatter fRsapacs = new RSAPKCS1SignatureDeformatter(rsa);
            //    fRsapacs.SetHashAlgorithm("SHA1");
            //    //key是注册码
            //    byte[] key = Convert.FromBase64String(regCode);
            //    SHA1Managed sha = new SHA1Managed();
            //    //name为 需加密的字符串
            //    byte[] name = sha.ComputeHash(ASCIIEncoding.ASCII.GetBytes(machineCode));
            //    //判断是否成功
            //    if (fRsapacs.VerifySignature(name, key))
            //    {
            //        MessageBox.Show("该产品已注册，谢谢您的使用！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        return;
            //    }
            //    else
            //    {
            //        frmReg form = new frmReg();
            //        form.ShowDialog();
            //        return;
            //    }
            //}
            //检测注册信息
            Cls.Register reg = new GSM.Cls.Register();
            string[] msg = null;
            reg.ReadAdditonalLicenseInformation(out msg);
            string message = string.Empty;
            foreach (string s in msg)
            {
                message += s + "\r\n";
            }

            MessageBox.Show(this, message, "注册信息", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void tsmiExit_Click_1(object sender, EventArgs e)
        {
            if (DialogResult.OK == MessageBox.Show("确认退出?", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
            {
                //Cls.Method.writeLog(this.tsslblUserRole.Text + "登出" + "[" + DateTime.Now.ToString() + "]");
                _Comm.Close();
                Thread.Sleep(500);
                _CommBack.Close();
                Thread.Sleep(500);
                //reader.Stop();
                //Thread.Sleep(500);
                readerBack.Stop();
                Application.ExitThread();
                this.Dispose();
            }
        }

        private void tsbtnSerial_Click(object sender, EventArgs e)
        {
            ToolStripSplitButton tsdd = (ToolStripSplitButton)sender;
            tsdd.ShowDropDown();
        }

        private void tsbtnAbout_Click(object sender, EventArgs e)
        {
            new Forms.AboutBox().ShowDialog();
        }

        private void tsbtnLogout_Click_1(object sender, EventArgs e)
        {
            if (DialogResult.OK == MessageBox.Show("确定注销当前用户?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
            {
                this._userRole = string.Empty;
                this.tsslblUserRole.Text = string.Empty;
                TsmiClearClick(sender, e);

                _Comm.Close();
                Thread.Sleep(500);
                _CommBack.Close();
                Thread.Sleep(500);
                //reader.Stop();
                //Thread.Sleep(500);
                readerBack.Stop();

                this.Hide();
                new frmLoginIn().ShowDialog();
            }
        }

        private void tsbtnSerial_ButtonClick(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

            GoDexData.BLL.nodeInfo bni = new GoDexData.BLL.nodeInfo();
            for (int i = 20; i < 255; i++)
            {
                GoDexData.Model.nodeInfo ni = new GoDexData.Model.nodeInfo();
                ni.machineNo = i;
                ni.machineType = i % 4;
                bni.Add(ni);
            }
        }

    }
}
