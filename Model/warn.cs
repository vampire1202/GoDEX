using System;
namespace GoDexData.Model
{
	/// <summary>
	/// warn:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class warn
	{
		public warn()
		{}
		#region Model
		private long _id;
		private int? _machineno;
		private string _warnleval;
		private string _warndiscrib;
		private string _username;
		private DateTime? _warndatetime;
		private string _sign;
		/// <summary>
		/// 
		/// </summary>
		public long ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 机器编号
		/// </summary>
		public int? machineNo
		{
			set{ _machineno=value;}
			get{return _machineno;}
		}
		/// <summary>
		/// 警报等级
		/// </summary>
		public string warnLeval
		{
			set{ _warnleval=value;}
			get{return _warnleval;}
		}
		/// <summary>
		/// 警报描述
		/// </summary>
		public string warnDiscrib
		{
			set{ _warndiscrib=value;}
			get{return _warndiscrib;}
		}
		/// <summary>
		/// 确认用户
		/// </summary>
		public string userName
		{
			set{ _username=value;}
			get{return _username;}
		}
		/// <summary>
		/// 确认时间
		/// </summary>
		public DateTime? warnDateTime
		{
			set{ _warndatetime=value;}
			get{return _warndatetime;}
		}
		/// <summary>
		/// 备注
		/// </summary>
		public string sign
		{
			set{ _sign=value;}
			get{return _sign;}
		}
		#endregion Model

	}
}

