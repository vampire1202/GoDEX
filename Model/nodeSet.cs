using System;
namespace GoDexData.Model
{
	/// <summary>
	/// nodeSet:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class nodeSet
	{
		public nodeSet()
		{}
		#region Model
		private long _id;
		private int _machineno;
		private string _machinetype;
		private decimal? _airflowh_pipe1;
		private decimal? _airflowl_pipe1;
		private decimal? _airflowh_pipe2;
		private decimal? _airflowl_pipe2;
		private decimal? _airflowh_pipe3;
		private decimal? _airflowl_pipe3;
		private decimal? _ariflowh_pipe4;
		private decimal? _airflowl_pipe4;
		private decimal? _firea1_area1;
		private decimal? _firea2_area1;
		private decimal? _firea3_area1;
		private decimal? _firea4_area1;
		private decimal? _firea1_area2;
		private decimal? _firea2_area2;
		private decimal? _firea3_area2;
		private decimal? _firea4_area2;
		private decimal? _firea1_area3;
		private decimal? _firea2_area3;
		private decimal? _firea3_area3;
		private decimal? _firea4_area3;
		private decimal? _firea1_area4;
		private decimal? _firea2_area4;
		private decimal? _firea3_area4;
		private decimal? _firea4_area4;
		private int? _pumpspeed;
		private string _enterpwd;
		private int? _chartspeed;
		private DateTime? _machinetime;
		private string _sign;
        private int? _a1delay;
        private int? _a2delay;
        private int? _a3delay;
        private int? _a4delay;
        private int? _isLock;
        private int? _isSeparate;
        private int? _isMute;
        private int? _isReverse;
        //isLock,isSeparate,isMute,isReverse
		/// <summary>
		/// 
		/// </summary>
		public long ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int machineNo
		{
			set{ _machineno=value;}
			get{return _machineno;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string machineType
		{
			set{ _machinetype=value;}
			get{return _machinetype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? airflowH_pipe1
		{
			set{ _airflowh_pipe1=value;}
			get{return _airflowh_pipe1;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? airflowL_pipe1
		{
			set{ _airflowl_pipe1=value;}
			get{return _airflowl_pipe1;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? airflowH_pipe2
		{
			set{ _airflowh_pipe2=value;}
			get{return _airflowh_pipe2;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? airflowL_pipe2
		{
			set{ _airflowl_pipe2=value;}
			get{return _airflowl_pipe2;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? airFlowH_pipe3
		{
			set{ _airflowh_pipe3=value;}
			get{return _airflowh_pipe3;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? airflowL_pipe3
		{
			set{ _airflowl_pipe3=value;}
			get{return _airflowl_pipe3;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? ariflowH_pipe4
		{
			set{ _ariflowh_pipe4=value;}
			get{return _ariflowh_pipe4;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? airflowL_pipe4
		{
			set{ _airflowl_pipe4=value;}
			get{return _airflowl_pipe4;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? fireA1_area1
		{
			set{ _firea1_area1=value;}
			get{return _firea1_area1;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? fireA2_area1
		{
			set{ _firea2_area1=value;}
			get{return _firea2_area1;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? fireA3_area1
		{
			set{ _firea3_area1=value;}
			get{return _firea3_area1;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? fireA4_area1
		{
			set{ _firea4_area1=value;}
			get{return _firea4_area1;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? fireA1_area2
		{
			set{ _firea1_area2=value;}
			get{return _firea1_area2;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? fireA2_area2
		{
			set{ _firea2_area2=value;}
			get{return _firea2_area2;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? fireA3_area2
		{
			set{ _firea3_area2=value;}
			get{return _firea3_area2;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? fireA4_area2
		{
			set{ _firea4_area2=value;}
			get{return _firea4_area2;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? fireA1_area3
		{
			set{ _firea1_area3=value;}
			get{return _firea1_area3;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? fireA2_area3
		{
			set{ _firea2_area3=value;}
			get{return _firea2_area3;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? fireA3_area3
		{
			set{ _firea3_area3=value;}
			get{return _firea3_area3;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? fireA4_area3
		{
			set{ _firea4_area3=value;}
			get{return _firea4_area3;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? fireA1_area4
		{
			set{ _firea1_area4=value;}
			get{return _firea1_area4;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? fireA2_area4
		{
			set{ _firea2_area4=value;}
			get{return _firea2_area4;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? fireA3_area4
		{
			set{ _firea3_area4=value;}
			get{return _firea3_area4;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? fireA4_area4
		{
			set{ _firea4_area4=value;}
			get{return _firea4_area4;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? pumpSpeed
		{
			set{ _pumpspeed=value;}
			get{return _pumpspeed;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string enterPwd
		{
			set{ _enterpwd=value;}
			get{return _enterpwd;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? chartSpeed
		{
			set{ _chartspeed=value;}
			get{return _chartspeed;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? machineTime
		{
			set{ _machinetime=value;}
			get{return _machinetime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string sign
		{
			set{ _sign=value;}
			get{return _sign;}
		}

        /// <summary>
        /// 
        /// </summary>
        public int? a1delay
        {
            set { _a1delay = value; }
            get { return _a1delay; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? a2delay
        {
            set { _a2delay = value; }
            get { return _a2delay; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? a3delay
        {
            set { _a3delay = value; }
            get { return _a3delay; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? a4delay
        {
            set { _a4delay = value; }
            get { return _a4delay; }
        } 

        /// <summary>
        /// 
        /// </summary>
        public int? isLock
        {
            set { _isLock = value; }
            get { return _isLock; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? isSeparate
        {
            set { _isSeparate = value; }
            get { return _isSeparate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? isMute
        {
            set { _isMute = value; }
            get { return _isMute; }
        }
        /// 
        /// </summary>
        public int? isReverse
        {
            set { _isReverse = value; }
            get { return _isReverse; }
        }


      
		#endregion Model

	}
}

