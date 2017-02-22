using System;
namespace GoDexData.Model
{
	/// <summary>
	/// log:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class log
	{
		public log()
		{}
		#region Model
		private long _id;
		private int? _machineno;
		private string _action;
		private string _username;
		private DateTime? _acdatetime;
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
		/// 设备编号
		/// </summary>
		public int? machineNo
		{
			set{ _machineno=value;}
			get{return _machineno;}
		}
		/// <summary>
		/// 动作描述
		/// </summary>
		public string action
		{
			set{ _action=value;}
			get{return _action;}
		}
		/// <summary>
		/// 登陆用户
		/// </summary>
		public string userName
		{
			set{ _username=value;}
			get{return _username;}
		}
		/// <summary>
		/// 动作时间
		/// </summary>
		public DateTime? acDateTime
		{
			set{ _acdatetime=value;}
			get{return _acdatetime;}
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

