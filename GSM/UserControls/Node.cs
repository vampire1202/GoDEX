using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using System.Threading;
using System.Runtime.InteropServices;
using System.IO.Ports;


namespace GSM.UserControls
{
    public partial class Node : UserControl
    {
        //public delegate void deleReadScop(); //读取节点实时数据的委托函数
        //static bool _continue;//读取串口标志
        /// <summary>
        /// 通讯0串口
        /// </summary>
        private SerialPort _serialPort;//串口定义
        public SerialPort SerialPort
        {
            get { return this._serialPort; }
            set { this._serialPort = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        private string _operateInfo;
        public string OperateInfo
        {
            get { return this._operateInfo; }
            set { this._operateInfo = value; }
        }
        /// <summary>
        /// 位置
        /// </summary>
        public string Addr
        {
        	get { return this.lblAddr.Text; }
            set { this.lblAddr.Text = value; }
        }
        /// <summary>
        /// 火警值1
        /// </summary>
        public int Firevalue1
        {
        	get{return int.Parse(this.lblFireValue1.Text);}
        	set{this.lblFireValue1.Text = value.ToString();}
        }
                /// <summary>
        /// 火警值2
        /// </summary>
        public int Firevalue2
        {
        	get{return int.Parse(this.lblFireValue2.Text);}
        	set{this.lblFireValue2.Text = value.ToString();}
        }
                /// <summary>
        /// 火警值3
        /// </summary>
        public int Firevalue3
        {
        	get{return int.Parse(this.lblFireValue3.Text);}
        	set{this.lblFireValue3.Text = value.ToString();}
        }
                /// <summary>
        /// 火警值4
        /// </summary>
        public int Firevalue4
        {
        	get{return int.Parse(this.lblFireValue4.Text);}
        	set{this.lblFireValue4.Text = value.ToString();}
        }
                /// <summary>
        /// 火警值5
        /// </summary>
        public int Firevalue5
        {
        	get{return int.Parse(this.lblFireValue5.Text);}
        	set{this.lblFireValue5.Text = value.ToString();}
        }
        
        /// <summary>
        /// 通道1气流值
        /// </summary>
        public int Airevalue1
        {
        	get{return int.Parse(this.lblAirValue1.Text);}
        	set{this.lblAirValue1.Text = value.ToString();}
        }
          /// <summary>
        /// 通道2气流值
        /// </summary>
        public int Airevalue2
        {
        	get{return int.Parse(this.lblAirValue2.Text);}
        	set{this.lblAirValue2.Text = value.ToString();}
        }
             /// <summary>
        /// 通道3气流值
        /// </summary>
        public int Airevalue3
        {
        	get{return int.Parse(this.lblAirValue3.Text);}
        	set{this.lblAirValue3.Text = value.ToString();}
        }
             /// <summary>
        /// 通道4气流值
        /// </summary>
        public int Airevalue4
        {
        	get{return int.Parse(this.lblAirValue4.Text);}
        	set{this.lblAirValue4.Text = value.ToString();}
        }
        
        /// <summary>
        /// 通道5气流值
        /// </summary>
        public int Airevalue5
        {
        	get{return int.Parse(this.lblAirValue5.Text);}
        	set{this.lblAirValue5.Text = value.ToString();}
        }
        
        public Node()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.FixedHeight | ControlStyles.FixedWidth, true);
            InitializeComponent();            
        }

        /// <summary>
        /// 监测器编号
        /// </summary>       
        public string NodeNo
        {
            get { return this.nodeNo.Text; }
            set { this.nodeNo.Text = value; }
        }
        /// <summary>
        /// 是否选择
        /// </summary> 
        [Category("开关状态")]
        [Description("是否被选中")]
        volatile bool isCheck = false;       
        public bool IsCheck
        {
            set
            {
               
                isCheck = value;
            }
            get { return isCheck; }
        }  

        protected override void OnClick(EventArgs e)
        {
            this.isCheck = true; 
            a1Panel_Click(this, e);          
        } 

        private void a1Panel_Click(object sender, EventArgs e)
        {
            foreach (GSM.UserControls.Node nd in this.Parent.Controls)
            {
                nd.IsCheck = false;
                nd.BackColor = Color.FromArgb(181, 219, 242);
                //nd.BorderStyle = BorderStyle.None;
            }
            this.IsCheck = true;
            this.BackColor = Color.DeepSkyBlue; 
        }  
    }
}
