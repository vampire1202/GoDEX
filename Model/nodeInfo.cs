using System;
namespace GoDexData.Model
{
	/// <summary>
	/// nodeInfo:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class nodeInfo
	{
		public nodeInfo()
		{}
		#region Model
		private long _id;
		private int _machineno;
		private int? _machinetype;
		private string _softversion;
		private int? _doordog;
		private string _runtime;
		private string _worldmappath;
		private string _areamappath;
		private string _worldxy;
		private string _worldxy_1;
		private string _worldxy_2;
		private string _worldxy_3;
		private string _worldxy_4;
		private string _areaxy;
		private string _areaxy_1;
		private string _areaxy_2;
		private string _areaxy_3;
		private string _areaxy_4;
		private string _sign;
        private string _machineModel;
        private DateTime _lvwangdate;

        private int _fireChl1;
        public int fireChl1
        {
            get { return _fireChl1; }
            set { _fireChl1 = value; }
        }

        private int _fireChl2;
        public int fireChl2
        {
            get { return _fireChl2; }
            set { _fireChl2 = value; }
        }

        private int _fireChl3;
        public int fireChl3
        {
            get { return _fireChl3; }
            set { _fireChl3 = value; }
        }

        private int _fireChl4;
        public int fireChl4
        {
            get { return _fireChl4; }
            set { _fireChl4 = value; }
        }

        private int _airChl1;

        public int airChl1
        {
            get { return _airChl1; }
            set { _airChl1 = value; }
        }
        private int _airChl2;

        public int airChl2
        {
            get { return _airChl2; }
            set { _airChl2 = value; }
        }
        private int _airChl3;

        public int airChl3
        {
            get { return _airChl3; }
            set { _airChl3 = value; }
        }
        private int _airChl4;

        public int airChl4
        {
            get { return _airChl4; }
            set { _airChl4 = value; }
        }


		/// <summary>
		/// 
		/// </summary>
		public long ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 设备编号
		/// </summary>
		public int machineNo
		{
			set{ _machineno=value;}
			get{return _machineno;}
		}
		/// <summary>
		/// 设备类型
		/// </summary>
		public int? machineType
		{
			set{ _machinetype=value;}
			get{return _machinetype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string softversion
		{
			set{ _softversion=value;}
			get{return _softversion;}
		}
		/// <summary>
		/// 看门狗计数
		/// </summary>
		public int? doordog
		{
			set{ _doordog=value;}
			get{return _doordog;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string runtime
		{
			set{ _runtime=value;}
			get{return _runtime;}
		}
		/// <summary>
		/// 世界地图
		/// </summary>
		public string worldMapPath
		{
			set{ _worldmappath=value;}
			get{return _worldmappath;}
		}
		/// <summary>
		/// 区域地图
		/// </summary>
		public string areaMapPath
		{
			set{ _areamappath=value;}
			get{return _areamappath;}
		}
		/// <summary>
		/// 世界地图坐标(x,y)
		/// </summary>
		public string worldXY
		{
			set{ _worldxy=value;}
			get{return _worldxy;}
		}
		/// <summary>
		/// 世界地图坐标(x,y)_1
		/// </summary>
		public string worldXY_1
		{
			set{ _worldxy_1=value;}
			get{return _worldxy_1;}
		}
		/// <summary>
		/// 世界地图坐标(x,y)_2
		/// </summary>
		public string worldXY_2
		{
			set{ _worldxy_2=value;}
			get{return _worldxy_2;}
		}
		/// <summary>
		/// 世界地图坐标(x,y)_3
		/// </summary>
		public string worldXY_3
		{
			set{ _worldxy_3=value;}
			get{return _worldxy_3;}
		}
		/// <summary>
		/// 世界地图坐标(x,y)_4
		/// </summary>
		public string worldXY_4
		{
			set{ _worldxy_4=value;}
			get{return _worldxy_4;}
		}
		/// <summary>
		/// 区域地图坐标(x,y)
		/// </summary>
		public string areaXY
		{
			set{ _areaxy=value;}
			get{return _areaxy;}
		}
		/// <summary>
		/// 区域 1 地图坐标(x,y)
		/// </summary>
		public string areaXY_1
		{
			set{ _areaxy_1=value;}
			get{return _areaxy_1;}
		}
		/// <summary>
		/// 区域 2 地图坐标(x,y)
		/// </summary>
		public string areaXY_2
		{
			set{ _areaxy_2=value;}
			get{return _areaxy_2;}
		}
		/// <summary>
		/// 区域 3 地图坐标(x,y)
		/// </summary>
		public string areaXY_3
		{
			set{ _areaxy_3=value;}
			get{return _areaxy_3;}
		}
		/// <summary>
		/// 区域 4 地图坐标(x,y)
		/// </summary>
		public string areaXY_4
		{
			set{ _areaxy_4=value;}
			get{return _areaxy_4;}
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
        public string machineModel
        {
            set { _machineModel = value; }
            get { return _machineModel; }
        }

        public DateTime lvwangdate
        {
            set { _lvwangdate = value; }
            get { return _lvwangdate; }
        }

		#endregion Model

	}
}

