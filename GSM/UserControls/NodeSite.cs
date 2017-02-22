/*
 * Created by SharpDevelop.
 * User: Vampire
 * Date: 2010/11/19
 * Time: 20:35
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
namespace GSM.UserControls
{
	/// <summary>
	/// Description of NodeSite.
	/// </summary>
	public partial class NodeSite : UserControl
	{
        //[DllImport("user32.dll")]
        //public static extern bool ReleaseCapture();
        //[DllImport("user32.dll")]
        //public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        //public const int WM_SYSCOMMAND = 0x0112;
        //public const int SC_MOVE = 0xF010;
        //public const int HTCAPTION = 0x0002;		
        ///// <summary>
        ///// 前一个节点ID
        ///// </summary>
        //private string _preID;
        //public string _PreID
        //{
        //    get{return this._preID;}
        //    set{this._preID = value;}
        //}
		
        ///// <summary>
        ///// 后一个节点ID
        ///// </summary>
        //private string _nextID;
        //public string _NextID
        //{
        //    get{return this._nextID;}
        //    set{this._nextID = value;}
        //}
		
        /// <summary>
        /// 当前节点ID
        /// </summary>
		private string _curID;
		public string _CurID
		{
			get{return this._curID;}
			set{this._curID=value;}
		}

        /// <summary>
        /// 是否在线
        /// </summary>
        private Boolean _onLine;
        public Boolean _OnLine
        {
            get { return this._onLine; }
            set { this._onLine = value; }
        }

       
        /// <summary>
        /// 区域图坐标
        /// </summary>
        private Point _zoneXY;
        public Point _ZoneXY
        {
            get { return this._zoneXY; }
            set { this._zoneXY = value; }
        }
       
        /// <summary>
        /// 全局图坐标
        /// </summary>
        private Point _fullXY;
        public Point _FullXY
        {
            get { return this._fullXY; }
            set { this._fullXY = value; }
        }

       
        /// <summary>
        /// 气流阈值低Ch1
        /// </summary>
        private int _airLowCh1;
        public int _AirLowCh1
        {
            get { return this._airLowCh1; }
            set { _airLowCh1 = value; }
        }
        /// <summary>
        /// 气流阈值低Ch2
        /// </summary>
        private int _airLowCh2;
        public int _AirLowCh2
        {
            get { return this._airLowCh2; }
            set { _airLowCh2 = value; }
        }
        /// <summary>
        /// 气流阈值低Ch3
        /// </summary>
        private int _airLowCh3;
        public int _AirLowCh3
        {
            get { return this._airLowCh3; }
            set { _airLowCh3 = value; }
        }
        /// <summary>
        /// 气流阈值低Ch4
        /// </summary>
        private int _airLowCh4;
        public int _AirLowCh4
        {
            get { return this._airLowCh4; }
            set { _airLowCh4 = value; }
        }
        /// <summary>
        /// 气流阈值低Ch1
        /// </summary>
        private int _airLowCh5;
        public int _AirLowCh5
        {
            get { return this._airLowCh5; }
            set { _airLowCh5 = value; }
        }

        /// <summary>
        ///气流阈值高Ch1 
        /// </summary>
        private int _airHighCh1;
        public int _AirHighCh1
        {
            get { return this._airHighCh1; }
            set { _airHighCh1 = value; }
        }
        /// <summary>
        ///气流阈值高Ch2 
        /// </summary>
        private int _airHighCh2;
        public int _AirHighCh2
        {
            get { return this._airHighCh2; }
            set { _airHighCh2 = value; }
        }
        /// <summary>
        ///气流阈值高Ch3 
        /// </summary>
        private int _airHighCh3;
        public int _AirHighCh3
        {
            get { return this._airHighCh3; }
            set { _airHighCh3 = value; }
        }
        /// <summary>
        ///气流阈值高Ch1 
        /// </summary>
        private int _airHighCh4;
        public int _AirHighCh4
        {
            get { return this._airHighCh4; }
            set { _airHighCh4 = value; }
        }
        /// <summary>
        ///气流阈值高Ch1 
        /// </summary>
        private int _airHighCh5;
        public int _AirHighCh5
        {
            get { return this._airHighCh5; }
            set { _airHighCh5 = value; }
        }
      
        /// <summary>
        ///A1火警Ch1 
        /// </summary>
        private int _a1FireCh1;
        public int _A1FireCh1
        {
            get { return this._a1FireCh1; }
            set { _a1FireCh1 = value; }
        }

        /// <summary>
        ///A1火警Ch2 
        /// </summary>
        private int _a1FireCh2;
        public int _A1FireCh2
        {
            get { return this._a1FireCh2; }
            set { _a1FireCh2 = value; }
        }
        /// <summary>
        ///A1火警Ch3 
        /// </summary>
        private int _a1FireCh3;
        public int _A1FireCh3
        {
            get { return this._a1FireCh3; }
            set { _a1FireCh3 = value; }
        }
        /// <summary>
        ///A1火警Ch4 
        /// </summary>
        private int _a1FireCh4;
        public int _A1FireCh4
        {
            get { return this._a1FireCh4; }
            set { _a1FireCh4 = value; }
        }
        /// <summary>
        ///A1火警Ch5 
        /// </summary>
        private int _a1FireCh5;
        public int _A1FireCh5
        {
            get { return this._a1FireCh5; }
            set { _a1FireCh5 = value; }
        }

        
        /// <summary>
        ///A2火警Ch1 
        /// </summary>
        private int _a2FireCh1;
        public int _A2FireCh1
        {
            get { return this._a2FireCh1; }
            set { _a2FireCh1 = value; }
        }
        /// <summary>
        ///A2火警Ch2 
        /// </summary>
        private int _a2FireCh2;
        public int _A2FireCh2
        {
            get { return this._a2FireCh2; }
            set { _a2FireCh2 = value; }
        }
        /// <summary>
        ///A2火警Ch3 
        /// </summary>
        private int _a2FireCh3;
        public int _A2FireCh3
        {
            get { return this._a2FireCh3; }
            set { _a2FireCh3= value; }
        }
        /// <summary>
        ///A2火警Ch4 
        /// </summary>
        private int _a2FireCh4;
        public int _A2FireCh4
        {
            get { return this._a2FireCh4; }
            set { _a2FireCh4 = value; }
        }
        /// <summary>
        ///A2火警Ch5 
        /// </summary>
        private int _a2FireCh5;
        public int _A2FireCh5
        {
            get { return this._a2FireCh5; }
            set { _a2FireCh5= value; }
        }


        /// <summary>
        /// A3火警Ch1
        /// </summary>
        private int _a3FireCh1;
        public int _A3FireCh1
        {
            get { return this._a3FireCh1; }
            set { _a3FireCh1 = value; }
        }
        /// <summary>
        /// A3火警Ch2
        /// </summary>
        private int _a3FireCh2;
        public int _A3FireCh2
        {
            get { return this._a3FireCh2; }
            set { _a3FireCh2 = value; }
        }
        /// <summary>
        /// A3火警Ch3
        /// </summary>
        private int _a3FireCh3;
        public int _A3FireCh3
        {
            get { return this._a3FireCh3; }
            set { _a3FireCh3 = value; }
        }
        /// <summary>
        /// A3火警Ch4
        /// </summary>
        private int _a3FireCh4;
        public int _A3FireCh4
        {
            get { return this._a3FireCh4; }
            set { _a3FireCh4 = value; }
        }
        /// <summary>
        /// A3火警Ch5
        /// </summary>
        private int _a3FireCh5;
        public int _A3FireCh5
        {
            get { return this._a3FireCh5; }
            set { _a3FireCh5 = value; }
        }
        
                
        /// <summary>
        ///位置 
        /// </summary>
        private string _addr;
        public string _Addr
        {
            get { return _addr; }
            set { this._addr = value; }
        }
		
        /// <summary>
        /// 设备类型
        /// </summary>
        private string _mathineType;
        public string _MathineType
        {
            get { return this._mathineType; }
            set { this._mathineType = value; }
        }	
	
        /// <summary>
        /// mathineXinghao设备型号
        /// </summary>

        private string _mathineXinghao;
        public string _MathineXinghao
        {
            get { return this._mathineXinghao; }
            set { this._mathineXinghao = value; }
        }	

        /// <summary>
        /// airSpeed风机转速
        /// </summary>
        private string _airSpeed;
        public string _AirSpeed
        {
            get { return this._airSpeed; }
            set { this._airSpeed = value; }
        }

        /// <summary>
        /// 提示语
        /// </summary>
        private string _toolTipInfo;
        public string _ToolTipInfo
        {
            get { return this._toolTipInfo; }
            set { this._toolTipInfo = value; }
        }

		public NodeSite()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//            
			InitializeComponent();			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}		

        private void NodeSite_Load(object sender, EventArgs e)
        {
            //toolTip.ToolTipTitle = "ID: " + this._curID + " 属性";
        }
		
		void LblNoMouseDown(object sender, MouseEventArgs e)
		{
            base.OnMouseDown(e);                          
            //ReleaseCapture();
            //SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
		}

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            base.OnMouseDown(e);
            //ReleaseCapture();
            //SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }

        private void lblNo_Click(object sender, EventArgs e)
        {
            base.OnClick(e);
        }
   }
}
