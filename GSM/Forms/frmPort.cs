using System;
using System.Windows.Forms;
using System.IO.Ports;

namespace GSM.Forms
{
    public partial class frmPort : Form
    {

        //系统串口数组
        string[] sysPorts = null;
        //未使用的串口数组
        //string[] notUsedPorts=null;
        //使用了的串口数组
        //string[] usedPorts=null;
        private frmMain _fm;
        public frmPort(frmMain fm)
        {
            InitializeComponent();
            _fm = fm;
        }
        //        /// <summary>
        //        /// 通讯串口1
        //        /// </summary>
        //        private string _serialPort;
        //        public string _SerialPort
        //        {
        //        	get{return this._serialPort;}
        //        	set {this._serialPort = value;}
        //        }
        //        /// <summary>
        //        /// 通讯串口回路
        //        /// </summary>
        // 		private string _serialPortBack;
        // 		public string _SerialPortBack
        //        {
        //        	get{return this._serialPortBack;}
        //        	set {this._serialPortBack = value;}
        //        }
        //        private void cmbLoop_SelectedIndexChanged(object sender, EventArgs e)
        //        {
        //
        //        }


        private void frmPort_Load(object sender, EventArgs e)
        { 

            udLunxunTime.Value = Convert.ToInt32(Cls.RWconfig.GetAppSettings("lunxunTimeSpan")) / 1000;
            udofflinetime.Value = Convert.ToInt32(Cls.RWconfig.GetAppSettings("offlineTimeSpan")) / 1000;  
            udordertimespan.Value = Convert.ToInt32(Cls.RWconfig.GetAppSettings("orderTimeSpan"));

            sysPorts = SerialPort.GetPortNames();
            // Display each port name to the console.
            foreach (string port in sysPorts)
            {
                this.cmbPort.Items.Add(port);
                this.cmbPortBack.Items.Add(port);
            }
            this.cmbPort.Text = Cls.RWconfig.GetAppSettings("portName");
            this.cmbPortBack.Text = Cls.RWconfig.GetAppSettings("portNameBack");
            this.cmbRate1.Text = Cls.RWconfig.GetAppSettings("RateMain");
            this.cmbRate2.Text = Cls.RWconfig.GetAppSettings("RateBack");
            //this.txtAlarmNum.Text = Cls.RWconfig.GetAppSettings("AlarmNum");
        }

        private void btnOK_Click(object sender, EventArgs e)
        { 
            try
            {
                if (this.cmbPort.Text == this.cmbPortBack.Text)
                {
                    MessageBox.Show("两个串口不能相同,请重新选择！");
                    return;
                }
                try
                {
                    Cls.RWconfig.SetAppSettings("PortName", this.cmbPort.Text);
                    Cls.RWconfig.SetAppSettings("PortNameBack", this.cmbPortBack.Text);
                   
                    Cls.RWconfig.SetAppSettings("RateMain", this.cmbRate1.Text);
                    Cls.RWconfig.SetAppSettings("RateBack", this.cmbRate2.Text);

                    Cls.RWconfig.SetAppSettings("orderTimeSpan", this.udordertimespan.Value.ToString());
                    Cls.RWconfig.SetAppSettings("lunxunTimeSpan", (this.udLunxunTime.Value * 1000).ToString());
                    Cls.RWconfig.SetAppSettings("offlineTimeSpan", (this.udofflinetime.Value * 1000).ToString());
                }
                catch (Exception ee) { MessageBox.Show("无法写入,请检查配置文件" + ee.ToString()); }

                if (_fm._Comm.IsOpen)
                    _fm._Comm.Close();
                if (_fm._CommBack.IsOpen)
                    _fm._CommBack.Close();
                if (!string.IsNullOrEmpty(this.cmbPort.Text))
                {
                    _fm._Comm.PortName = this.cmbPort.Text;
                    _fm._Comm.BaudRate = int.Parse(this.cmbRate1.Text);
                }
                if (!string.IsNullOrEmpty(this.cmbPortBack.Text))
                {
                    _fm._CommBack.PortName = this.cmbPortBack.Text;
                    _fm._CommBack.BaudRate = int.Parse(this.cmbRate2.Text);
                }
                _fm.Initsp1();
                _fm.Initsp2();
                this.Dispose();
            }
            catch { }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        } 
    }
}
