using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Xml;
using System.Xml.XPath;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace GSM.Forms
{
    public partial class frmSetNode : Form
    {
        private string _userRole = string.Empty;
        public string _machineType = string.Empty;

        frmMain _fMain = null;
        public frmSetNode(string userRole, frmMain fMain)
        {
            InitializeComponent();
            _fMain = fMain;
            _userRole = userRole;
        }
        /// <summary>
        /// 节点地址
        /// </summary>
        private string nodeNo;
        public string NodeNo
        {
            get { return nodeNo; }
            set { this.nodeNo = value; }
        }
        /// <summary>
        /// 串口
        /// </summary>
        private SerialPort _serialPort;
        public SerialPort SerialPort
        {
            get { return this._serialPort; }
            set { this._serialPort = value; }
        }

        private RichTextBox richtextbox;
        public RichTextBox RichTextBox
        {
            get { return this.richtextbox; }
            set { this.richtextbox = value; }
        }


        /// <summary>
        /// 串口
        /// </summary>
        //        private SerialPort _serialPortBack;
        //        public SerialPort SerialPortBack
        //        {
        //            get { return this._serialPortBack; }
        //            set { this._serialPortBack = value; }
        //        } 


        /// <summary>
        /// //串口发送数据字符串
        /// </summary>
        /// <param name="sendData1">第一个数据位</param>
        /// <param name="sendData2">第二个数据位</param>
        /// <param name="nodeID">节点ID</param>
        /// <returns>返回 整条命令的 16进制 字符串</returns>
        public Byte[] SendData(Byte sendData1, Byte sendData2, string nodeID)
        {
            Byte[] sendData = new Byte[6];
            try
            {
                sendData[0] = 0xFE;
                sendData[1] = 0x04;
                sendData[2] = Convert.ToByte(int.Parse(nodeID));
                sendData[3] = sendData1;
                sendData[4] = sendData2;
                sendData[5] = Convert.ToByte(Convert.ToInt32(sendData[1]) ^ Convert.ToInt32(sendData[2]) ^ Convert.ToInt32(sendData[3]) ^ Convert.ToInt32(sendData[4]));
                if (this.SerialPort != null)
                    this.SerialPort.Write(sendData, 0, 6);
                Thread.Sleep(50); 
                StringBuilder sb = new StringBuilder();
                foreach(byte b in sendData)
                {
                    sb.Append(b.ToString("X2")+" ");
                }
                Trace.WriteLine(sb.ToString());
            }
            catch { }
            return sendData;
        }

        private void frmSetNode_Load(object sender, EventArgs e)
        {
            //string userRole = Cls.RWconfig.GetAppSettings("CurrentUser");
            this.cmbAireSpeed.Items.Add("低速");
            this.cmbAireSpeed.Items.Add("中速");
            this.cmbAireSpeed.Items.Add("高速");

            this.dateSet.Value = DateTime.Now.Date;
            this.timeSet.Value = DateTime.Now.ToLocalTime();
            readFireAndAire();
            setMachineType(_machineType);
            getUserRole(_userRole);

        }

        private void setMachineType(string machineType)
        {
            switch (machineType)
            {
                case "单管单区":
                    this.guanlu1.Enabled = true;
                    this.guanlu2.Enabled = false;
                    this.guanlu3.Enabled = false;
                    this.guanlu4.Enabled = false;
                    this.tongdao1.Enabled = true;
                    this.tongdao2.Enabled = false;
                    this.tongdao3.Enabled = false;
                    this.tongdao4.Enabled = false;
                    break;
                case "四管单区":
                    this.guanlu1.Enabled = true;
                    this.guanlu2.Enabled = true;
                    this.guanlu3.Enabled = true;
                    this.guanlu4.Enabled = true;
                    this.tongdao1.Enabled = true;
                    this.tongdao2.Enabled = false;
                    this.tongdao3.Enabled = false;
                    this.tongdao4.Enabled = false;
                    break;
                case "四管四区":
                    this.guanlu1.Enabled = true;
                    this.guanlu2.Enabled = true;
                    this.guanlu3.Enabled = true;
                    this.guanlu4.Enabled = true;
                    this.tongdao1.Enabled = true;
                    this.tongdao2.Enabled = true;
                    this.tongdao3.Enabled = true;
                    this.tongdao4.Enabled = true;
                    break;
                default:
                    this.guanlu1.Enabled = false;
                    this.guanlu2.Enabled = false;
                    this.guanlu3.Enabled = false;
                    this.guanlu4.Enabled = false;
                    this.tongdao1.Enabled = false;
                    this.tongdao2.Enabled = false;
                    this.tongdao3.Enabled = false;
                    this.tongdao4.Enabled = false;
                    break;
            }
        }
        private void getUserRole(string _userRole)
        {
            switch (_userRole)
            {
                case "SystemManager":
                case "CommonUser":
                    //this.groupBoxAire.Enabled = false;
                    //this.groupBoxFire.Enabled = false;
                    //this.groupBoxTime.Enabled = false;
                    break;
                case "Observer":
                    foreach (GroupBox gb in this.Controls)
                    {
                        gb.Enabled = false;
                    }
                    break;
            }
        }


        /// <summary>
        /// 读取火警阈值和气流警报值
        /// </summary>
        private void readFireAndAire()
        {
            try
            {
                GoDexData.BLL.nodeInfo bllNi = new GoDexData.BLL.nodeInfo();
                GoDexData.Model.nodeInfo modelNi = bllNi.GetModel(int.Parse(this.nodeNo));
                if(modelNi!=null)
                {
                    this.txtModel.Text = modelNi.machineModel;
                    GetPercent(modelNi);
                }

                GoDexData.BLL.nodeSet bllNs = new GoDexData.BLL.nodeSet();
                GoDexData.Model.nodeSet modelNs = bllNs.GetModel(int.Parse(this.nodeNo));

                if (modelNs != null)
                {
                    //读取抽气泵转速
                    this.cmbAireSpeed.SelectedIndex = int.Parse(modelNs.pumpSpeed.ToString()); // xn.Attributes["airSpeed"].Value.ToString();

                    this.fireA1Ch1.Value = modelNs.fireA1_area1.Value;// int.Parse(xn["a1fire"].Attributes["ch1"].Value);
                    this.fireA1Ch2.Value = modelNs.fireA1_area2.Value;//int.Parse(xn["a1fire"].Attributes["ch2"].Value);
                    this.fireA1Ch3.Value = modelNs.fireA1_area3.Value;//int.Parse(xn["a1fire"].Attributes["ch3"].Value);
                    this.fireA1Ch4.Value = modelNs.fireA1_area4.Value;//int.Parse(xn["a1fire"].Attributes["ch4"].Value); 

                    this.fireA2Ch1.Value = modelNs.fireA2_area1.Value;//int.Parse(xn["a2fire"].Attributes["ch1"].Value);
                    this.fireA2Ch2.Value = modelNs.fireA2_area2.Value;// int.Parse(xn["a2fire"].Attributes["ch2"].Value);
                    this.fireA2Ch3.Value = modelNs.fireA2_area3.Value;//int.Parse(xn["a2fire"].Attributes["ch3"].Value);
                    this.fireA2Ch4.Value = modelNs.fireA2_area4.Value;//int.Parse(xn["a2fire"].Attributes["ch4"].Value); 

                    this.fireA3Ch1.Value = modelNs.fireA3_area1.Value;//int.Parse(xn["a3fire"].Attributes["ch1"].Value);
                    this.fireA3Ch2.Value = modelNs.fireA3_area2.Value;//int.Parse(xn["a3fire"].Attributes["ch2"].Value);
                    this.fireA3Ch3.Value = modelNs.fireA3_area3.Value;//int.Parse(xn["a3fire"].Attributes["ch3"].Value);
                    this.fireA3Ch4.Value = modelNs.fireA3_area4.Value;//int.Parse(xn["a3fire"].Attributes["ch4"].Value); 

                    this.fireA4Ch1.Value = modelNs.fireA4_area1.Value;//int.Parse(xn["a4fire"].Attributes["ch1"].Value);
                    this.fireA4Ch2.Value = modelNs.fireA4_area2.Value;//int.Parse(xn["a4fire"].Attributes["ch2"].Value);
                    this.fireA4Ch3.Value = modelNs.fireA4_area3.Value;// int.Parse(xn["a4fire"].Attributes["ch3"].Value);
                    this.fireA4Ch4.Value = modelNs.fireA4_area4.Value;//int.Parse(xn["a4fire"].Attributes["ch4"].Value); 

                    this.airLowCh1.Value = modelNs.airflowL_pipe1.Value;//int.Parse(xn["airlow"].Attributes["ch1"].Value);
                    this.airLowCh2.Value = modelNs.airflowL_pipe2.Value;// int.Parse(xn["airlow"].Attributes["ch2"].Value);
                    this.airLowCh3.Value = modelNs.airflowL_pipe3.Value;//int.Parse(xn["airlow"].Attributes["ch3"].Value);
                    this.airLowCh4.Value = modelNs.airflowL_pipe4.Value;//int.Parse(xn["airlow"].Attributes["ch4"].Value); 

                    this.airHighCh1.Value = modelNs.airflowH_pipe1.Value;//int.Parse(xn["airhigh"].Attributes["ch1"].Value);
                    this.airHighCh2.Value = modelNs.airflowH_pipe2.Value;//int.Parse(xn["airhigh"].Attributes["ch2"].Value);
                    this.airHighCh3.Value = modelNs.airFlowH_pipe3.Value;//int.Parse(xn["airhigh"].Attributes["ch3"].Value);
                    this.airHighCh4.Value = modelNs.ariflowH_pipe4.Value;// int.Parse(xn["airhigh"].Attributes["ch4"].Value); 
                    //火警延迟时间
                    this.numA1.Value = decimal.Parse(modelNs.a1delay.Value.ToString());// int.Parse(xn["fireTime"].Attributes["A1Time"].Value);
                    this.numA2.Value = decimal.Parse(modelNs.a2delay.Value.ToString());// int.Parse(xn["fireTime"].Attributes["A2Time"].Value);
                    this.numA3.Value = decimal.Parse(modelNs.a3delay.Value.ToString());// int.Parse(xn["fireTime"].Attributes["A3Time"].Value);
                    this.numA4.Value = decimal.Parse(modelNs.a4delay.Value.ToString());// int.Parse(xn["fireTime"].Attributes["A4Time"].Value);

                    //this.txtSerialNo.Text = modelNs.sign;
                    
                    //取消节点状态
                    //if (modelNs.isLock == 1)
                    //    this.chkLock.Checked = true;
                    //else
                    //    this.chkLock.Checked = false;
                    //if (modelNs.isSeparate == 1)
                    //    this.chkSeperate.Checked = true;
                    //else
                    //    this.chkSeperate.Checked = false;
                    //if (modelNs.isMute == 1)
                    //    this.chkMute.Checked = true;
                    //else
                    //    this.chkMute.Checked = false;

                    //if (modelNs.isReverse == 1)
                    //    this.chkReverse.Checked = true;
                    //else
                    //    this.chkReverse.Checked = false;

                }
            }
            catch (Exception ee)
            { //MessageBox.Show(ee.ToString());
            }

        }

        void GetPercent(GoDexData.Model.nodeInfo modelNi)
        {
            if (modelNi.lvwangdate != null)
            {
                this.dtplvwangdate.Value = modelNi.lvwangdate;
                TimeSpan alldays = new TimeSpan((modelNi.lvwangdate.AddYears(2) - modelNi.lvwangdate).Days, 0, 0, 0);
                TimeSpan relaydays = new TimeSpan((modelNi.lvwangdate.AddYears(2) - DateTime.Now).Days, 0, 0, 0);
                double perc = ((double)relaydays.Days / (double)alldays.Days) * 100.0;
                this.lblPer.Text = perc.ToString("f2")+"%";
                this.progressBar1.Value = (int)perc;
            }
        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {

        }

        //private string OctToHex(int value)
        //{
        //    if (value < 16)
        //    {
        //        switch (value)
        //        {
        //            case 15: return "F";
        //            case 14: return "E";
        //            case 13: return "D";
        //            case 12: return "C";
        //            case 11: return "B";
        //            case 10: return "A";
        //            default: return value.ToString();
        //        }
        //    }

        //    string cur = OctToHex(value / 16) + OctToHex(value % 16);
        //    return cur;
        //}

        //创建委托
        delegate void delegateMethod(Byte data1, Byte data2, string nodeNo, string airfire, string chanel, string value, string tiptext);//SendData(0xBB, 0x01, nodeNo);
        //委托用的方法
        public void sendSetData(Byte _data1, Byte _data2, string _nodeNo, string _airfire, string _chanel, string _value, string tiptext)
        {
            SendData(_data1, _data2, _nodeNo);
            //richtextbox.AppendText( "命令:" + Cls.Method.ByteToString(SendData(_data1, _data2, _nodeNo)) + "\r\n";
            richtextbox.AppendText(tiptext + "-监控节点-" + _nodeNo + "-[" + _airfire + "]-通道" + _chanel + ":" + _value + "\r\n");
            richtextbox.SelectionStart = richtextbox.Text.Length;
            richtextbox.Focus();
        }

        private void _sendSetAireData()
        {
            BeginInvoke(new Action(() =>
                 {
                     string msgInfo = string.Empty;
                     GoDexData.BLL.nodeSet bllNs = new GoDexData.BLL.nodeSet();
                     GoDexData.Model.nodeSet modelNs = bllNs.GetModel(int.Parse(this.nodeNo));

                     if (DialogResult.OK == MessageBox.Show("确认设置该值?", "设置气流阈值", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
                     {
                         if (modelNs == null)
                             msgInfo += "该节点为空! \r\n";
                         if (string.IsNullOrEmpty(msgInfo))
                         {
                             //气流低门限值
                             if (modelNs.airflowL_pipe1 != this.airLowCh1.Value)
                             {
                                 if (this.airLowCh1.Value > this.airHighCh1.Value)
                                 {
                                     MessageBox.Show("确保通道1气流低警告值小于气流高警告值");
                                     return;
                                 }
                                 sendSetData(Convert.ToByte(0xC1), Convert.ToByte(this.airLowCh1.Value), this.nodeNo, "airlow", "ch1", this.airLowCh1.Value.ToString(), "气流阈值设置");
                                 modelNs.airflowL_pipe1 = this.airLowCh1.Value;
                                 Thread.Sleep(3000);
                             }

                             if (modelNs.airflowL_pipe2 != this.airLowCh2.Value)
                             {
                                 if (this.airLowCh2.Value > this.airHighCh2.Value)
                                 {
                                     MessageBox.Show("确保通道2气流低警告值小于气流高警告值");
                                     return;
                                 }
                                 sendSetData(Convert.ToByte(0xC2), Convert.ToByte(this.airLowCh2.Value), this.nodeNo, "airlow", "ch2", this.airLowCh2.Value.ToString(), "气流阈值设置");
                                 modelNs.airflowL_pipe2 = this.airLowCh2.Value;
                                 Thread.Sleep(3000);
                             }

                             if (modelNs.airflowL_pipe3 != this.airLowCh3.Value)
                             {
                                 if (this.airLowCh3.Value > this.airHighCh3.Value)
                                 {
                                     MessageBox.Show("确保通道3气流低警告值小于气流高警告值");
                                     return;
                                 }
                                 sendSetData(Convert.ToByte(0xC3), Convert.ToByte(this.airLowCh3.Value), this.nodeNo, "airlow", "ch3", this.airLowCh3.Value.ToString(), "气流阈值设置");
                                 modelNs.airflowL_pipe3 = this.airLowCh3.Value;
                                 Thread.Sleep(3000);
                             }

                             if (modelNs.airflowL_pipe4 != this.airLowCh4.Value)
                             {
                                 if (this.airLowCh4.Value > this.airHighCh4.Value)
                                 {
                                     MessageBox.Show("确保通道4气流低警告值小于气流高警告值");
                                     return;
                                 }
                                 sendSetData(Convert.ToByte(0xC4), Convert.ToByte(this.airLowCh4.Value), this.nodeNo, "airlow", "ch4", this.airLowCh4.Value.ToString(), "气流阈值设置");
                                 modelNs.airflowL_pipe4 = this.airLowCh4.Value;
                                 Thread.Sleep(3000);
                             }

                             if (modelNs.airflowH_pipe1 != this.airHighCh1.Value)
                             {
                                 if (this.airLowCh1.Value > this.airHighCh1.Value)
                                 {
                                     MessageBox.Show("确保通道1气流低警告值小于气流高警告值");
                                     return;
                                 }
                                 sendSetData(Convert.ToByte(0xC6), Convert.ToByte(this.airHighCh1.Value), this.nodeNo, "airhigh", "ch1", this.airHighCh1.Value.ToString(), "气流阈值设置");
                                 modelNs.airflowH_pipe1 = this.airHighCh1.Value;
                                 Thread.Sleep(3000);
                             }

                             if (modelNs.airflowH_pipe2 != this.airHighCh2.Value)
                             {
                                 if (this.airLowCh2.Value > this.airHighCh2.Value)
                                 {
                                     MessageBox.Show("确保通道2气流低警告值小于气流高警告值");
                                     return;
                                 }
                                 sendSetData(Convert.ToByte(0xC7), Convert.ToByte(this.airHighCh2.Value), this.nodeNo, "airhigh", "ch2", this.airHighCh2.Value.ToString(), "气流阈值设置");
                                 modelNs.airflowH_pipe2 = this.airHighCh2.Value;
                                 Thread.Sleep(3000);
                             }

                             if (modelNs.airFlowH_pipe3 != this.airHighCh3.Value)
                             {
                                 if (this.airLowCh3.Value > this.airHighCh3.Value)
                                 {
                                     MessageBox.Show("确保通道3气流低警告值小于气流高警告值");
                                     return;
                                 }
                                 sendSetData(Convert.ToByte(0xC8), Convert.ToByte(this.airHighCh3.Value), this.nodeNo, "airhigh", "ch3", this.airHighCh3.Value.ToString(), "气流阈值设置");
                                 modelNs.airFlowH_pipe3 = this.airHighCh3.Value;
                                 Thread.Sleep(3000);
                             }

                             if (modelNs.ariflowH_pipe4 != this.airHighCh4.Value)
                             {
                                 if (this.airLowCh1.Value > this.airHighCh1.Value)
                                 {
                                     MessageBox.Show("确保通道4气流低警告值小于气流高警告值");
                                     return;
                                 }
                                 sendSetData(Convert.ToByte(0xC9), Convert.ToByte(this.airHighCh4.Value), this.nodeNo, "airhigh", "ch4", this.airHighCh4.Value.ToString(), "气流阈值设置");
                                 modelNs.ariflowH_pipe4 = this.airHighCh4.Value;
                                 Thread.Sleep(3000);
                             }

                             //Cls.Method.writeLog("气流阈值设置-监控节点-" + nodeNo + "-气流[低]阈值- 通道1:" + this.airLowCh1.Value.ToString() + " 通道2:" + this.airLowCh2.Value.ToString() + " 通道3:" + this.airLowCh3.Value.ToString() + " 通道4:" + this.airLowCh4.Value.ToString() + ",[" + DateTime.Now.ToString() + "]");
                             //Cls.Method.writeLog("气流阈值设置-监控节点-" + nodeNo + "-气流[高]阈值- 通道1:" + this.airHighCh1.Value.ToString() + " 通道2:" + this.airHighCh2.Value.ToString() + " 通道3:" + this.airHighCh3.Value.ToString() + " 通道4:" + this.airHighCh4.Value.ToString() + ",[" + DateTime.Now.ToString() + "]");
                             Cls.Method.writeLogData(int.Parse(this.nodeNo), "气流阈值设置[低]- 通道1:" + this.airLowCh1.Value.ToString() + " 通道2:" + this.airLowCh2.Value.ToString() + " 通道3:" + this.airLowCh3.Value.ToString() + " 通道4:" + this.airLowCh4.Value.ToString() + ",[" + DateTime.Now.ToString() + "]", this._userRole, DateTime.Now, "-");
                             Cls.Method.writeLogData(int.Parse(this.nodeNo), "气流阈值设置[高]- 通道1:" + this.airHighCh1.Value.ToString() + " 通道2:" + this.airHighCh2.Value.ToString() + " 通道3:" + this.airHighCh3.Value.ToString() + " 通道4:" + this.airHighCh4.Value.ToString() + ",[" + DateTime.Now.ToString() + "]", this._userRole, DateTime.Now, "-");
                             ///////////////////////////// 
                             richtextbox.SelectionStart = richtextbox.Text.Length;
                             richtextbox.Focus();
                             if (bllNs.Update(modelNs))
                             {
                                 MessageBox.Show("设置气流阈值成功!");
                             }
                         }
                         else
                         {
                             MessageBox.Show(msgInfo, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                         }
                     }
                     setAire = null;
                 }));
        }

        Thread setAire;
        private void btnSetAire_Click(object sender, EventArgs e)
        {
            if (setAire == null)
            {
                setAire = new Thread(new ThreadStart(_sendSetAireData));
                setAire.Start();
            }
        }

        Thread setFire;
        void _sendSetFireData()
        {
            BeginInvoke(new Action(() =>
                {
                    if (DialogResult.OK == MessageBox.Show("确认设置该值?", "设置火警阈值", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
                    {
                        //设定火警阈值1 通道 123级火警
                        string msgInfo1 = string.Empty;
                        if (this.fireA1Ch1.Value > this.fireA2Ch1.Value)
                            msgInfo1 = "确保[1]级警报阈值小于[2]级警报阈值! \r\n";
                        if (this.fireA2Ch1.Value > this.fireA3Ch1.Value)
                            msgInfo1 += "确保[2]级警报阈值小于[3]级警报阈值! \r\n";
                        if (this.fireA1Ch1.Value > this.fireA3Ch1.Value)
                            msgInfo1 += "确保[1]级警报阈值小于[3]级警报阈值! \r\n";
                        if (this.fireA3Ch1.Value > this.fireA4Ch1.Value)
                            msgInfo1 += "确保[3]级警报阈值小于[4]级警报阈值! \r\n";

                        if (!string.IsNullOrEmpty(msgInfo1))
                        {
                            MessageBox.Show(msgInfo1);
                            return;
                        }

                        GoDexData.BLL.nodeSet bllNs = new GoDexData.BLL.nodeSet();
                        GoDexData.Model.nodeSet modelNs = bllNs.GetModel(int.Parse(this.nodeNo));

                        if (string.IsNullOrEmpty(msgInfo1))
                        {

                            if (modelNs.fireA1_area1 != this.fireA1Ch1.Value)
                            {
                                sendSetData(Convert.ToByte(0xD1), Convert.ToByte(this.fireA1Ch1.Value), this.nodeNo, "a1fire", "ch1", this.fireA1Ch1.Value.ToString(), "火警阈值设置");
                                //Cls.Method.writeLog("火警阈值设置-节点-" + nodeNo + "-一级火警阈值-通道 1-" + this.fireA1Ch1.Value + ",[" + DateTime.Now.ToString() + "]");
                                Cls.Method.writeLogData(int.Parse(this.nodeNo), "通道1 一级火警阈值设置:" + this.fireA1Ch1.Value, this._userRole, DateTime.Now, "-");
                                modelNs.fireA1_area1 = this.fireA1Ch1.Value;
                                Thread.Sleep(3000);
                            }
                            if (modelNs.fireA2_area1 != this.fireA2Ch1.Value)
                            {

                                sendSetData(Convert.ToByte(0xD6), Convert.ToByte(this.fireA2Ch1.Value), this.nodeNo, "a2fire", "ch1", this.fireA2Ch1.Value.ToString(), "火警阈值设置");
                                //Cls.Method.writeLog("火警阈值设置-节点-" + nodeNo + "-二级火警阈值-通道 1-" + this.fireA2Ch1.Value + ",[" + DateTime.Now.ToString() + "]");
                                Cls.Method.writeLogData(int.Parse(this.nodeNo), "通道1 二级火警阈值设置:" + this.fireA2Ch1.Value, this._userRole, DateTime.Now, "-");
                                modelNs.fireA2_area1 = this.fireA2Ch1.Value;
                                Thread.Sleep(3000);
                            }

                            if (modelNs.fireA3_area1 != this.fireA3Ch1.Value)
                            {
                                sendSetData(Convert.ToByte(0xDB), Convert.ToByte(this.fireA3Ch1.Value), this.nodeNo, "a3fire", "ch1", this.fireA3Ch1.Value.ToString(), "火警阈值设置");
                                //Cls.Method.writeLog("火警阈值设置-节点-" + nodeNo + "-三级火警阈值-通道 1-" + this.fireA3Ch1.Value + ",[" + DateTime.Now.ToString() + "]");
                                Cls.Method.writeLogData(int.Parse(this.nodeNo), "通道1 三级火警阈值设置:" + this.fireA3Ch1.Value, this._userRole, DateTime.Now, "-");
                                modelNs.fireA3_area1 = this.fireA3Ch1.Value;
                                Thread.Sleep(3000);
                            }

                            if (modelNs.fireA4_area1 != this.fireA4Ch1.Value)
                            {
                                sendSetData(Convert.ToByte(0xDF), Convert.ToByte(this.fireA4Ch1.Value), this.nodeNo, "a4fire", "ch1", this.fireA4Ch1.Value.ToString(), "火警阈值设置");
                                //Cls.Method.writeLog("火警阈值设置-节点-" + nodeNo + "-四级火警阈值-通道 1-" + this.fireA4Ch1.Value + ",[" + DateTime.Now.ToString() + "]");
                                Cls.Method.writeLogData(int.Parse(this.nodeNo), "通道1 四级火警阈值设置:" + this.fireA4Ch1.Value, this._userRole, DateTime.Now, "-");
                                modelNs.fireA4_area1 = this.fireA4Ch1.Value;
                                Thread.Sleep(3000);
                            }

                            if (modelNs.a1delay != int.Parse(this.numA1.Value.ToString()))
                            {
                                sendSetData(Convert.ToByte(0xE1), Convert.ToByte(this.numA1.Value), this.nodeNo, "fireTime", "A1Time", this.numA1.Value.ToString(), "一级火警延迟设置" + numA1.Value.ToString() + "秒");
                                Cls.Method.writeLogData(int.Parse(this.nodeNo), "一级火警延迟设置:" + this.numA1.Value + " 秒", this._userRole, DateTime.Now, "-");
                                modelNs.a1delay = int.Parse(this.numA1.Value.ToString());
                                Thread.Sleep(3000);
                            }

                            if (modelNs.a2delay != int.Parse(this.numA2.Value.ToString()))
                            {
                                sendSetData(Convert.ToByte(0xE2), Convert.ToByte(this.numA2.Value), this.nodeNo, "fireTime", "A2Time", this.numA2.Value.ToString(), "二级火警延迟设置" + numA2.Value.ToString() + "秒");
                                Cls.Method.writeLogData(int.Parse(this.nodeNo), "二级火警延迟设置：" + this.numA2.Value + " 秒", this._userRole, DateTime.Now, "-");
                                modelNs.a2delay = int.Parse(this.numA2.Value.ToString());
                                Thread.Sleep(3000);
                            }

                            if (modelNs.a3delay != int.Parse(this.numA3.Value.ToString()))
                            {
                                sendSetData(Convert.ToByte(0xE3), Convert.ToByte(this.numA3.Value), this.nodeNo, "fireTime", "A3Time", this.numA3.Value.ToString(), "三级火警延迟设置" + numA3.Value.ToString() + "秒");
                                Cls.Method.writeLogData(int.Parse(this.nodeNo), "三级火警延迟设置:" + this.numA3.Value + " 秒", this._userRole, DateTime.Now, "-");
                                modelNs.a3delay = int.Parse(this.numA3.Value.ToString());
                                Thread.Sleep(3000);
                            }

                            if (modelNs.a4delay != int.Parse(this.numA4.Value.ToString()))
                            {
                                sendSetData(Convert.ToByte(0xE4), Convert.ToByte(this.numA4.Value), this.nodeNo, "fireTime", "A4Time", this.numA4.Value.ToString(), "四级火警延迟设置" + numA4.Value.ToString() + "秒");
                                Cls.Method.writeLogData(int.Parse(this.nodeNo), "四级火警延迟设置:" + this.numA4.Value + " 秒", this._userRole, DateTime.Now, "-");
                                modelNs.a4delay = int.Parse(this.numA4.Value.ToString());
                                Thread.Sleep(3000);
                            }
                        }


                        //设定火警阈值 2通道 123级火警
                        string msgInfo2 = string.Empty;
                        if (this.fireA1Ch2.Value > this.fireA2Ch2.Value)
                            msgInfo2 = "确保[1]级警报阈值小于[2]级警报阈值! \r\n";
                        if (this.fireA2Ch2.Value > this.fireA3Ch2.Value)
                            msgInfo2 += "确保[2]级警报阈值小于[3]级警报阈值! \r\n";
                        if (this.fireA1Ch2.Value > this.fireA3Ch2.Value)
                            msgInfo2 += "确保[1]级警报阈值小于[3]级警报阈值! \r\n";
                        if (string.IsNullOrEmpty(msgInfo2))
                        {
                            if (modelNs.fireA1_area2 != this.fireA1Ch2.Value)
                            {
                                sendSetData(Convert.ToByte(0xD2), Convert.ToByte(this.fireA1Ch2.Value), this.nodeNo, "a1fire", "ch2", this.fireA1Ch2.Value.ToString(), "火警阈值设置");
                                //Cls.Method.writeLog("火警阈值设置-节点-" + nodeNo + "-一级火警阈值-通道 2-" + this.fireA1Ch2.Value + ",[" + DateTime.Now.ToString() + "]");
                                Cls.Method.writeLogData(int.Parse(this.nodeNo), " 通道 2 一级火警阈值设置:" + this.fireA1Ch2.Value, this._userRole, DateTime.Now, "-");
                                modelNs.fireA1_area2 = this.fireA1Ch2.Value;
                                Thread.Sleep(3000);
                            }

                            if (modelNs.fireA2_area2 != this.fireA2Ch2.Value)
                            {
                                sendSetData(Convert.ToByte(0xD7), Convert.ToByte(this.fireA2Ch2.Value), this.nodeNo, "a2fire", "ch2", this.fireA2Ch2.Value.ToString(), "火警阈值设置");
                                //Cls.Method.writeLog("火警阈值设置-节点-" + nodeNo + "-二级火警阈值-通道 2-" + this.fireA2Ch2.Value + ",[" + DateTime.Now.ToString() + "]");
                                Cls.Method.writeLogData(int.Parse(this.nodeNo), " 通道 2 二级火警阈值设置:" + this.fireA2Ch2.Value, this._userRole, DateTime.Now, "-");
                                modelNs.fireA2_area2 = this.fireA2Ch2.Value;
                                Thread.Sleep(3000);
                            }
                            if (modelNs.fireA3_area2 != this.fireA3Ch2.Value)
                            {
                                sendSetData(Convert.ToByte(0xDC), Convert.ToByte(this.fireA3Ch2.Value), this.nodeNo, "a3fire", "ch2", this.fireA3Ch2.Value.ToString(), "火警阈值设置");
                                //Cls.Method.writeLog("火警阈值设置-节点-" + nodeNo + "-三级火警阈值-通道 2-" + this.fireA3Ch2.Value + ",[" + DateTime.Now.ToString() + "]");
                                Cls.Method.writeLogData(int.Parse(this.nodeNo), " 通道 2 三级火警阈值设置:" + this.fireA3Ch2.Value, this._userRole, DateTime.Now, "-");
                                modelNs.fireA3_area2 = this.fireA3Ch2.Value;
                                Thread.Sleep(3000);
                            }

                            if (modelNs.fireA4_area2 != this.fireA4Ch2.Value)
                            {
                                sendSetData(Convert.ToByte(0xCC), Convert.ToByte(this.fireA4Ch2.Value), this.nodeNo, "a4fire", "ch2", this.fireA4Ch2.Value.ToString(), "火警阈值设置");
                                //Cls.Method.writeLog("火警阈值设置-节点-" + nodeNo + "-三级火警阈值-通道 2-" + this.fireA3Ch2.Value + ",[" + DateTime.Now.ToString() + "]");
                                Cls.Method.writeLogData(int.Parse(this.nodeNo), " 通道 2 四级火警阈值设置:" + this.fireA4Ch2.Value, this._userRole, DateTime.Now, "-");
                                modelNs.fireA4_area2 = this.fireA4Ch2.Value;
                                Thread.Sleep(3000);
                            }

                        }

                        //设定火警阈值3通道 123级火警
                        string msgInfo3 = string.Empty;
                        if (this.fireA1Ch3.Value > this.fireA2Ch3.Value)
                            msgInfo3 = "确保[1]级警报阈值小于[2]级警报阈值! \r\n";
                        if (this.fireA2Ch3.Value > this.fireA3Ch3.Value)
                            msgInfo3 += "确保[2]级警报阈值小于[3]级警报阈值! \r\n";
                        if (this.fireA1Ch3.Value > this.fireA3Ch3.Value)
                            msgInfo3 += "确保[1]级警报阈值小于[3]级警报阈值! \r\n";
                        if (string.IsNullOrEmpty(msgInfo3))
                        {
                            if (modelNs.fireA1_area3 != this.fireA1Ch3.Value)
                            {
                                sendSetData(Convert.ToByte(0xD3), Convert.ToByte(this.fireA1Ch3.Value), this.nodeNo, "a1fire", "ch3", this.fireA1Ch3.Value.ToString(), "火警阈值设置");
                                //Cls.Method.writeLog("火警阈值设置-节点-" + nodeNo + "-一级火警阈值-通道 3-" + this.fireA1Ch3.Value + ",[" + DateTime.Now.ToString() + "]");
                                Cls.Method.writeLogData(int.Parse(this.nodeNo), " 通道 3 一级火警阈值设置:" + this.fireA1Ch3.Value, this._userRole, DateTime.Now, "-");
                                modelNs.fireA1_area3 = this.fireA1Ch3.Value;
                                Thread.Sleep(3000);
                            }
                            if (modelNs.fireA2_area3 != this.fireA2Ch3.Value)
                            {
                                sendSetData(Convert.ToByte(0xD8), Convert.ToByte(this.fireA2Ch3.Value), this.nodeNo, "a2fire", "ch3", this.fireA2Ch3.Value.ToString(), "火警阈值设置");
                                //Cls.Method.writeLog("火警阈值设置-节点-" + nodeNo + "-二级火警阈值-通道 3-" + this.fireA2Ch3.Value + ",[" + DateTime.Now.ToString() + "]");
                                Cls.Method.writeLogData(int.Parse(this.nodeNo), " 通道 3 二级火警阈值设置:" + this.fireA2Ch3.Value, this._userRole, DateTime.Now, "-");
                                modelNs.fireA2_area3 = this.fireA2Ch3.Value;
                                Thread.Sleep(3000);
                            }
                            if (modelNs.fireA3_area3 != this.fireA3Ch3.Value)
                            {
                                sendSetData(Convert.ToByte(0xDD), Convert.ToByte(this.fireA3Ch3.Value), this.nodeNo, "a3fire", "ch3", this.fireA3Ch3.Value.ToString(), "火警阈值设置");
                                // Cls.Method.writeLog("火警阈值设置-节点-" + nodeNo + "-三级火警阈值-通道 3-" + this.fireA3Ch3.Value + ",[" + DateTime.Now.ToString() + "]");
                                Cls.Method.writeLogData(int.Parse(this.nodeNo), " 通道 3 三级火警阈值设置:" + this.fireA3Ch3.Value, this._userRole, DateTime.Now, "-");
                                modelNs.fireA3_area3 = this.fireA3Ch3.Value;
                                Thread.Sleep(3000);
                            }

                            if (modelNs.fireA4_area3 != this.fireA4Ch3.Value)
                            {
                                sendSetData(Convert.ToByte(0xCD), Convert.ToByte(this.fireA4Ch3.Value), this.nodeNo, "a4fire", "ch3", this.fireA4Ch3.Value.ToString(), "火警阈值设置");
                                //Cls.Method.writeLog("火警阈值设置-节点-" + nodeNo + "-三级火警阈值-通道 2-" + this.fireA3Ch2.Value + ",[" + DateTime.Now.ToString() + "]");
                                Cls.Method.writeLogData(int.Parse(this.nodeNo), " 通道 3 四级火警阈值设置:" + this.fireA4Ch3.Value, this._userRole, DateTime.Now, "-");
                                modelNs.fireA4_area3 = this.fireA4Ch3.Value;
                                Thread.Sleep(3000);
                            }

                        }

                        //设定火警阈值4通道123级火警
                        string msgInfo4 = string.Empty;
                        if (this.fireA1Ch4.Value > this.fireA2Ch4.Value)
                            msgInfo4 = "确保[1]级警报阈值小于[2]级警报阈值! \r\n";
                        if (this.fireA2Ch4.Value > this.fireA3Ch4.Value)
                            msgInfo4 += "确保[2]级警报阈值小于[3]级警报阈值! \r\n";
                        if (this.fireA1Ch4.Value > this.fireA3Ch4.Value)
                            msgInfo4 += "确保[1]级警报阈值小于[3]级警报阈值! \r\n";
                        if (string.IsNullOrEmpty(msgInfo4))
                        {
                            if (modelNs.fireA1_area4 != this.fireA1Ch4.Value)
                            {
                                sendSetData(Convert.ToByte(0xD4), Convert.ToByte(this.fireA1Ch4.Value), this.nodeNo, "a1fire", "ch4", this.fireA1Ch4.Value.ToString(), "火警阈值设置");
                                //Cls.Method.writeLog("火警阈值设置-节点-" + nodeNo + "-一级火警阈值-通道 4-" + this.fireA1Ch4.Value + ",[" + DateTime.Now.ToString() + "]");
                                Cls.Method.writeLogData(int.Parse(this.nodeNo), " 通道 4 一级火警阈值设置:" + this.fireA1Ch4.Value, this._userRole, DateTime.Now, "-");
                                modelNs.fireA1_area4 = this.fireA1Ch4.Value;
                                Thread.Sleep(3000);
                            }

                            if (modelNs.fireA2_area4 != this.fireA2Ch4.Value)
                            {
                                sendSetData(Convert.ToByte(0xD9), Convert.ToByte(this.fireA2Ch4.Value), this.nodeNo, "a2fire", "ch4", this.fireA2Ch4.Value.ToString(), "火警阈值设置");
                                //Cls.Method.writeLog("火警阈值设置-节点-" + nodeNo + "-二级火警阈值-通道 4-" + this.fireA2Ch4.Value + ",[" + DateTime.Now.ToString() + "]");
                                Cls.Method.writeLogData(int.Parse(this.nodeNo), " 通道 4 二级火警阈值设置:" + this.fireA2Ch4.Value, this._userRole, DateTime.Now, "-");
                                modelNs.fireA2_area4 = this.fireA2Ch4.Value;
                                Thread.Sleep(3000);
                            }
                            if (modelNs.fireA3_area4 != this.fireA3Ch4.Value)
                            {
                                sendSetData(Convert.ToByte(0xDE), Convert.ToByte(this.fireA3Ch4.Value), this.nodeNo, "a3fire", "ch4", this.fireA3Ch4.Value.ToString(), "火警阈值设置");
                                //Cls.Method.writeLog("火警阈值设置-节点-" + nodeNo + "-三级火警阈值-通道 4-" + this.fireA3Ch4.Value + ",[" + DateTime.Now.ToString() + "]");
                                Cls.Method.writeLogData(int.Parse(this.nodeNo), " 通道 4 三级火警阈值设置:" + this.fireA3Ch4.Value, this._userRole, DateTime.Now, "-");
                                modelNs.fireA3_area4 = this.fireA3Ch4.Value;
                                Thread.Sleep(3000);
                            }

                            if (modelNs.fireA4_area4 != this.fireA4Ch4.Value)
                            {
                                sendSetData(Convert.ToByte(0xCE), Convert.ToByte(this.fireA4Ch4.Value), this.nodeNo, "a4fire", "ch4", this.fireA4Ch4.Value.ToString(), "火警阈值设置");
                                //Cls.Method.writeLog("火警阈值设置-节点-" + nodeNo + "-三级火警阈值-通道 2-" + this.fireA3Ch2.Value + ",[" + DateTime.Now.ToString() + "]");
                                Cls.Method.writeLogData(int.Parse(this.nodeNo), " 通道 4 四级火警阈值设置:" + this.fireA4Ch4.Value, this._userRole, DateTime.Now, "-");
                                modelNs.fireA4_area4 = this.fireA4Ch4.Value;
                                Thread.Sleep(3000);
                            }

                        }
                        if (bllNs.Update(modelNs))
                        {
                            MessageBox.Show("设置火警阈值成功!");
                        }
                    }
                    setFire = null;
                }));
        }

        private void btnSetFire_Click(object sender, EventArgs e)
        {
            if (setFire == null)
            {
                setFire = new Thread(new ThreadStart(_sendSetFireData));
                setFire.Start();
            }
        }

        void BtnAirSpeedClick(object sender, EventArgs e)
        {
            // Cls.Method.changeNodeAttribute(this._xmlPath, nodeNo, "airSpeed", this.cmbAireSpeed.Text);
            //设置抽气泵转速
            GoDexData.BLL.nodeSet bllNs = new GoDexData.BLL.nodeSet();
            GoDexData.Model.nodeSet modelNs = bllNs.GetModel(int.Parse(this.nodeNo));
            modelNs.pumpSpeed = this.cmbAireSpeed.SelectedIndex;
            bllNs.Update(modelNs);
            switch (this.cmbAireSpeed.SelectedIndex)
            {
                case 0:
                    SendData(0xBB, 0x01, nodeNo);
                    richtextbox.AppendText("抽气泵转速设置-监控节点-" + nodeNo + ", " + this.cmbAireSpeed.Text + "," + DateTime.Now.ToString() + "\r\n");
                    //Cls.Method.writeLog("抽气泵转速设置-监控节点-" + nodeNo + ", " + this.cmbAireSpeed.Text + ",[" + DateTime.Now.ToString() +"]");
                    Cls.Method.writeLogData(int.Parse(nodeNo), "抽气泵转速设置 " + this.cmbAireSpeed.Text, _fMain._userRole, DateTime.Now, "-");
                    break;
                case 1:
                    SendData(0xBB, 0x02, nodeNo);
                    richtextbox.AppendText("抽气泵转速设置-监控节点-" + nodeNo + ", " + this.cmbAireSpeed.Text + "," + DateTime.Now.ToString() + "\r\n");
                    //Cls.Method.writeLog("抽气泵转速设置-监控节点-" + nodeNo + ", " + this.cmbAireSpeed.Text + ",[" + DateTime.Now.ToString()+"]");
                    Cls.Method.writeLogData(int.Parse(nodeNo), "抽气泵转速设置 " + this.cmbAireSpeed.Text, _fMain._userRole, DateTime.Now, "-");
                    break;
                case 2:
                    SendData(0xBB, 0x03, nodeNo);
                    richtextbox.AppendText("抽气泵转速设置-监控节点-" + nodeNo + ", " + this.cmbAireSpeed.Text + "," + DateTime.Now.ToString() + "\r\n");
                    //Cls.Method.writeLog("抽气泵转速设置-监控节点-" + nodeNo + ", " + this.cmbAireSpeed.Text + ",[" + DateTime.Now.ToString()+"]");
                    Cls.Method.writeLogData(int.Parse(nodeNo), "抽气泵转速设置 " + this.cmbAireSpeed.Text, _fMain._userRole, DateTime.Now, "-");
                    break;

                //case 3:
                //    SendData(0xBB, 0x03, nodeNo);
                //    richtextbox.AppendText( "抽气泵转速设置-监控节点-" + nodeNo + ", " + this.cmbAireSpeed.Text + "," + DateTime.Now.ToString() + "\r\n";
                //    //Cls.Method.writeLog("抽气泵转速设置-监控节点-" + nodeNo + ", " + this.cmbAireSpeed.Text + ",[" + DateTime.Now.ToString() + "]");
                //    Cls.Method.writeLogData(int.Parse(nodeNo), "抽气泵转速设置 " + this.cmbAireSpeed.Text, _fMain._userRole, DateTime.Now, "-"); 
                //    break;
                //case 4:
                //    SendData(0xBB, 0x04, nodeNo);
                //    richtextbox.AppendText( "抽气泵转速设置-监控节点-" + nodeNo + ", " + this.cmbAireSpeed.Text + "," + DateTime.Now.ToString() + "\r\n";
                //    //Cls.Method.writeLog("抽气泵转速设置-监控节点-" + nodeNo + ", " + this.cmbAireSpeed.Text + ",[" + DateTime.Now.ToString() + "]");
                //    Cls.Method.writeLogData(int.Parse(nodeNo), "抽气泵转速设置 " + this.cmbAireSpeed.Text, _fMain._userRole, DateTime.Now, "-"); 
                //    break;
                //case 5:
                //    SendData(0xBB, 0x05, nodeNo);
                //    richtextbox.AppendText( "抽气泵转速设置-监控节点-" + nodeNo + ", " + this.cmbAireSpeed.Text + "," + DateTime.Now.ToString() + "\r\n";
                //    //Cls.Method.writeLog("抽气泵转速设置-监控节点-" + nodeNo + ", " + this.cmbAireSpeed.Text + ",[" + DateTime.Now.ToString() + "]");
                //    Cls.Method.writeLogData(int.Parse(nodeNo), "抽气泵转速设置 " + this.cmbAireSpeed.Text, _fMain._userRole, DateTime.Now, "-");
                //    break;

                //case 6:
                //    SendData(0xBB, 0x06, nodeNo);
                //    richtextbox.AppendText( "抽气泵转速设置-监控节点-" + nodeNo + ", " + this.cmbAireSpeed.Text + "," + DateTime.Now.ToString() + "\r\n";
                //    //Cls.Method.writeLog("抽气泵转速设置-监控节点-" + nodeNo + ", " + this.cmbAireSpeed.Text + ",[" + DateTime.Now.ToString() + "]");
                //    Cls.Method.writeLogData(int.Parse(nodeNo), "抽气泵转速设置 " + this.cmbAireSpeed.Text, _fMain._userRole, DateTime.Now, "-");
                //    break;
                //case 7:
                //    SendData(0xBB, 0x07, nodeNo);
                //    richtextbox.AppendText( "抽气泵转速设置-监控节点-" + nodeNo + ", " + this.cmbAireSpeed.Text + "," + DateTime.Now.ToString() + "\r\n";
                //    //Cls.Method.writeLog("抽气泵转速设置-监控节点-" + nodeNo + ", " + this.cmbAireSpeed.Text + ",[" + DateTime.Now.ToString() + "]");
                //    Cls.Method.writeLogData(int.Parse(nodeNo), "抽气泵转速设置 " + this.cmbAireSpeed.Text, _fMain._userRole, DateTime.Now, "-");
                //    break;
                //case 8:
                //    SendData(0xBB, 0x08, nodeNo);
                //    richtextbox.AppendText( "抽气泵转速设置-监控节点-" + nodeNo + ", " + this.cmbAireSpeed.Text + "," + DateTime.Now.ToString() + "\r\n";
                //    //Cls.Method.writeLog("抽气泵转速设置-监控节点-" + nodeNo + ", " + this.cmbAireSpeed.Text + ",[" + DateTime.Now.ToString() + "]");
                //    Cls.Method.writeLogData(int.Parse(nodeNo), "抽气泵转速设置 " + this.cmbAireSpeed.Text, _fMain._userRole, DateTime.Now, "-");
                //    break;
                //case 9:
                //    SendData(0xBB, 0x09, nodeNo);
                //    richtextbox.AppendText( "抽气泵转速设置-监控节点-" + nodeNo + ", " + this.cmbAireSpeed.Text + "," + DateTime.Now.ToString() + "\r\n";
                //    //Cls.Method.writeLog("抽气泵转速设置-监控节点-" + nodeNo + ", " + this.cmbAireSpeed.Text + ",[" + DateTime.Now.ToString() + "]");
                //    Cls.Method.writeLogData(int.Parse(nodeNo), "抽气泵转速设置 " + this.cmbAireSpeed.Text, _fMain._userRole, DateTime.Now, "-");
                //    break;
            }
            richtextbox.Update();
        }

        void Button2Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        Thread setTime;
        void _sendSetTimeData()
        {
            //SendData(0xB0, bYear, this.nodeNo);
            //richtextbox.AppendText( "设置设备时间-监控节点-" + nodeNo + "-年:" + year + DateTime.Now.ToString() + "\r\n";
            //Thread.Sleep(1000);
            if (this.richtextbox.InvokeRequired)
            {
                string year = this.dateSet.Value.Year.ToString().Substring(2, 2);
                byte bYear = Convert.ToByte(TenTo16(year));
                this.richtextbox.Invoke(new delegateMethod(sendSetData), new object[] { Convert.ToByte(0xB0), bYear, this.nodeNo, null, null, "所有,年:20" + year, "设置设备时间-年:" + year });
                //Cls.Method.writeLog("设置设备时间-监控节点-" + nodeNo + "-年:" + year + "[" + DateTime.Now.ToString() + "]");
                Cls.Method.writeLogData(int.Parse(this.nodeNo), "设置设备时间 年:" + year, this._userRole, DateTime.Now, "-");
                Thread.Sleep(1000);

                string month = this.dateSet.Value.Month.ToString();
                byte bMonth = Convert.ToByte(TenTo16(month));
                this.richtextbox.Invoke(new delegateMethod(sendSetData), new object[] { Convert.ToByte(0xB1), bMonth, this.nodeNo, null, null, "所有,月:" + month, "设置设备时间-月:" + month });
                //SendData(0xB1, bMonth, this.nodeNo);
                //richtextbox.AppendText( "设置设备时间-监控节点-" + nodeNo + "-月:" + month + DateTime.Now.ToString() + "\r\n";
                //Cls.Method.writeLog("设置设备时间-监控节点-" + nodeNo + "-月:" + month + "[" + DateTime.Now.ToString() + "]");
                Cls.Method.writeLogData(int.Parse(this.nodeNo), "设置设备时间 月:" + month, this._userRole, DateTime.Now, "-");
                Thread.Sleep(1000);

                string day = this.dateSet.Value.Day.ToString();
                byte bDay = Convert.ToByte(TenTo16(day));
                this.richtextbox.Invoke(new delegateMethod(sendSetData), new object[] { Convert.ToByte(0xB2), bDay, this.nodeNo, null, null, "所有,日:" + day, "设置设备时间-日:" + day });
                //SendData(0xB2, bDay, this.nodeNo);
                //richtextbox.AppendText( "设置设备时间-监控节点-" + nodeNo + "-日:" + day + DateTime.Now.ToString() + "\r\n";
                //Cls.Method.writeLog("设置设备时间-监控节点-" + nodeNo + "-日:" + day + "[" + DateTime.Now.ToString() + "]");
                Cls.Method.writeLogData(int.Parse(this.nodeNo), "设置设备时间 日:" + day, this._userRole, DateTime.Now, "-");
                Thread.Sleep(1000);

                string hour = this.timeSet.Value.Hour.ToString();
                byte bHour = Convert.ToByte(TenTo16(hour));
                this.richtextbox.Invoke(new delegateMethod(sendSetData), new object[] { Convert.ToByte(0xB3), bHour, this.nodeNo, null, null, "所有,时:" + hour, "设置设备时间-时:" + hour });
                //SendData(0xB3, bHour, this.nodeNo);
                //richtextbox.AppendText( "设置设备时间-监控节点-" + nodeNo + "-时:" + hour + DateTime.Now.ToString() + "\r\n";
                //Cls.Method.writeLog("设置设备时间-监控节点-" + nodeNo + "-时:" + hour + "[" + DateTime.Now.ToString() + "]");
                Cls.Method.writeLogData(int.Parse(this.nodeNo), "设置设备时间 时:" + hour, this._userRole, DateTime.Now, "-");
                Thread.Sleep(1000);

                string min = this.timeSet.Value.Minute.ToString();
                byte bMin = Convert.ToByte(TenTo16(min));
                this.richtextbox.Invoke(new delegateMethod(sendSetData), new object[] { Convert.ToByte(0xB4), bMin, this.nodeNo, null, null, "所有,分:" + min, "设置设备时间-分:" + min });
                //SendData(0xB4, bMin, this.nodeNo);
                //richtextbox.AppendText( "设置设备时间-监控节点-" + nodeNo + "-分:" + min + DateTime.Now.ToString() + "\r\n";
                //Cls.Method.writeLog("设置设备时间-监控节点-" + nodeNo + "-分:" + min + "[" + DateTime.Now.ToString() + "]");
                Cls.Method.writeLogData(int.Parse(this.nodeNo), "设置设备时间 分:" + min, this._userRole, DateTime.Now, "-");
                Thread.Sleep(1000);

                string sec = this.timeSet.Value.Second.ToString();
                byte bSec = Convert.ToByte(TenTo16(sec));
                this.richtextbox.Invoke(new delegateMethod(sendSetData), new object[] { Convert.ToByte(0xB5), bSec, this.nodeNo, null, null, "所有,秒:" + sec, "设置设备时间-秒:" + sec });
                //SendData(0xB5, bSec, this.nodeNo);
                //richtextbox.AppendText( "设置设备时间-监控节点-" + nodeNo + "-秒:" + sec + DateTime.Now.ToString() + "\r\n";
                //Cls.Method.writeLog("设置设备时间-监控节点-" + nodeNo + "-秒:" + sec + "[" + DateTime.Now.ToString() + "]");
                Cls.Method.writeLogData(int.Parse(this.nodeNo), "设置设备时间 秒:" + sec, this._userRole, DateTime.Now, "-");
                Thread.Sleep(1000);
                MessageBox.Show("设置设备时间成功");
            }
            setTime = null;
        }

        private void setDateTime1_Click(object sender, EventArgs e)
        {
            //if (setTime == null)
            //{
            //    setTime = new Thread(new ThreadStart(_sendSetTimeData));
            //    setTime.Start();
            //}
        }

        //特殊转换
        private double TenTo16(string strNumber)
        {
            double int16 = 0;
            int j = strNumber.Length;
            for (int i = 0; i < j; i++)
            {
                int16 += Convert.ToDouble(strNumber.Substring(i, 1)) * (System.Math.Pow(16, j - i - 1));
            }
            return int16;
        }

        private void setDateTime_Click(object sender, EventArgs e)
        {
            if (setTime == null)
            {
                setTime = new Thread(new ThreadStart(_sendSetTimeData));
                setTime.Start();
            }
        }

        private void frmSetNode_FormClosing(object sender, FormClosingEventArgs e)
        {
            _fMain.CreateNodeTree();
        }

        private void chkLock_CheckedChanged(object sender, EventArgs e)
        {
            //GoDexData.BLL.nodeSet bllNs = new GoDexData.BLL.nodeSet();
            //GoDexData.Model.nodeSet modelNs = bllNs.GetModel(int.Parse(this.nodeNo));

            //if (chkLock.Checked)
            //{
            //    SendData(0xA7, 0x01, this.nodeNo);
            //    modelNs.isLock = 1;
            //}
            //else
            //{
            //    SendData(0xA7, 0x02, this.nodeNo);
            //    modelNs.isLock = 0;
            //}
            //if (
            //bllNs.Update(modelNs)) { } 
        }

        private void chkSeperate_CheckedChanged(object sender, EventArgs e)
        {
            //GoDexData.BLL.nodeSet bllNs = new GoDexData.BLL.nodeSet();
            //GoDexData.Model.nodeSet modelNs = bllNs.GetModel(int.Parse(this.nodeNo));
            //if (chkSeperate.Checked)
            //{
            //    SendData(0xA3, 0x00, this.nodeNo);
            //    modelNs.isSeparate = 1;
            //}
            //else
            //{
            //    SendData(0xA5, 0x00, this.nodeNo);
            //    modelNs.isSeparate = 0;
            //}
            //if (bllNs.Update(modelNs)) { }
        }

        private void chkMute_CheckedChanged(object sender, EventArgs e)
        {
            //GoDexData.BLL.nodeSet bllNs = new GoDexData.BLL.nodeSet();
            //GoDexData.Model.nodeSet modelNs = bllNs.GetModel(int.Parse(this.nodeNo));
            //if (chkMute.Checked)
            //{
            //    SendData(0xA4, 0x00, this.nodeNo);
            //    modelNs.isMute = 1;

            //}
            //else
            //{
            //    SendData(0xA6, 0x00, this.nodeNo);
            //    modelNs.isMute = 0;
            //}

            //if (bllNs.Update(modelNs))
            //{ }
        }

        private void chkReverse_CheckedChanged(object sender, EventArgs e)
        {
            //GoDexData.BLL.nodeSet bllNs = new GoDexData.BLL.nodeSet();
            //GoDexData.Model.nodeSet modelNs = bllNs.GetModel(int.Parse(this.nodeNo));
            //SendData(0xA7, 0x00, this.nodeNo);
            //modelNs.isReverse = Convert.ToInt16(chkReverse.Checked);
            //if (bllNs.Update(modelNs)) { }
        }

        private void chkLock_Click(object sender, EventArgs e)
        {

        }

        private void btnSetSerialNo_Click(object sender, EventArgs e)
        {
            Task.Run(() => {
                askSerialNo();
            });
        }

        void askSerialNo()
        {
            //举例1：下传的设备编号：L234A6C8F
            //计算机下传FE 04 ID E6 8F 校验    设备回执上传FE 04 ID 8B 8F 校验
            //计算机下传FE 04 ID E7 6C 校验    设备回执上传FE 04 ID 8C 6C 校验
            //计算机下传FE 04 ID E8 4A 校验    设备回执上传FE 04 ID 8D 4A 校验
            //计算机下传FE 04 ID E9 23 校验    设备回执上传FE 04 ID 8E 23 校验
            string sNo = this.txtSerialNo.Text;
            if(string.IsNullOrEmpty(sNo))
            {
                MessageBox.Show("请输入序列号！");
                return;
            }  
            if(sNo.Length<8)
            {
                MessageBox.Show("序列号长度不满8位！");
                return;
            } 

            if(!CheckSNo(sNo))
            {
               MessageBox.Show("请输入正确的编号,允许 数字 0～9 , 大写字母 A～F !");
               return;
            }

            long b1 = Cls.Method.strTo16(sNo.Substring(0, 2));
            long b2 = Cls.Method.strTo16(sNo.Substring(2, 2));
            long b3 = Cls.Method.strTo16(sNo.Substring(4, 2));
            long b4 = Cls.Method.strTo16(sNo.Substring(6, 2));

            SendData(0xE9,Convert.ToByte(b1) , nodeNo);
            Thread.Sleep(200);
            SendData(0xE8, Convert.ToByte(b2), nodeNo);
            Thread.Sleep(200);
            SendData(0xE7, Convert.ToByte(b3), nodeNo);
            Thread.Sleep(200);
            SendData(0xE6, Convert.ToByte(b4), nodeNo);
            Thread.Sleep(200);
            Trace.WriteLine("设置节点:"+ nodeNo +",编号"+ sNo);
        }

        bool CheckSNo(string input)
        {
            int[] s = new int[]{ 48,49,50,51,52,53,54,55,56,57,65 ,66 ,67,68,69,70};
            bool success=false;
            foreach(byte b in input)
            { 
                if (!s.Contains(b))
                { 
                    success= false;
                }
                else
                    success= true;
            }
            return success;
        }

        private void btnReadSerialNo_Click(object sender, EventArgs e)
        {
            SendData(0xBE, 0x00, nodeNo);
            readFireAndAire();
        }

        private void dtplvwangdate_ValueChanged(object sender, EventArgs e)
        {
            //如果设置的时间比当前时间 大于两年或小于两年 都不允许
            TimeSpan oldvalue = new TimeSpan((dtplvwangdate.Value.AddYears(2) - DateTime.Now).Days,0,0,0);  
            if(oldvalue.TotalDays<0)
            {
                MessageBox.Show("请设定比当前日期小于两年内的日期");
                dtplvwangdate.Value = DateTime.Now;
                return;
            }
            if(dtplvwangdate.Value>DateTime.Now)
            { 
                MessageBox.Show("请设定不大于当前日期的日期");
                dtplvwangdate.Value = DateTime.Now;
                return;
            }

            GoDexData.BLL.nodeInfo bllNi = new GoDexData.BLL.nodeInfo();
            GoDexData.Model.nodeInfo modelNi = bllNi.GetModel(int.Parse(this.nodeNo));
            if (modelNi != null)
            {
                modelNi.lvwangdate = dtplvwangdate.Value;
                if(bllNi.Update(modelNi))
                    GetPercent(modelNi);
            } 
        } 
    }
}
