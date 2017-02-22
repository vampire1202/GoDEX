using System;
namespace GoDexData.Model
{
	/// <summary>
	/// nodeSetOther:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class nodeSetOther
	{
		public nodeSetOther()
		{}
		#region Model
		private long? _id;
		private int? _machineno;
		private bool? _remote_isolate;
		private bool? _remote_reset;
		private bool? _remote_daynight;
		private bool? _program_isolate;
		private bool? _lockwarn;
		private bool? _lockfault;
		private bool? _stepwarn;
		private bool? _autoenergy;
		private bool? _checkmainpower;
		private bool? _checkcell;
		private bool? _useresetbutton;
		private bool? _usetestbutton;
		private bool? _useisolatebutton;
		private bool? _closedaynight;
		/// <summary>
		/// 
		/// </summary>
		public long? ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? machineNo
		{
			set{ _machineno=value;}
			get{return _machineno;}
		}
		/// <summary>
		/// 是否远端隔离
		/// </summary>
		public bool? remote_isolate
		{
			set{ _remote_isolate=value;}
			get{return _remote_isolate;}
		}
		/// <summary>
		/// 是否远端复位
		/// </summary>
		public bool? remote_reset
		{
			set{ _remote_reset=value;}
			get{return _remote_reset;}
		}
		/// <summary>
		/// 是否远端日夜
		/// </summary>
		public bool? remote_daynight
		{
			set{ _remote_daynight=value;}
			get{return _remote_daynight;}
		}
		/// <summary>
		/// 是否程序隔离
		/// </summary>
		public bool? program_isolate
		{
			set{ _program_isolate=value;}
			get{return _program_isolate;}
		}
		/// <summary>
		/// 是否锁存警报
		/// </summary>
		public bool? lockWarn
		{
			set{ _lockwarn=value;}
			get{return _lockwarn;}
		}
		/// <summary>
		/// 是否锁存故障
		/// </summary>
		public bool? lockFault
		{
			set{ _lockfault=value;}
			get{return _lockfault;}
		}
		/// <summary>
		/// 阶段式报警
		/// </summary>
		public bool? stepWarn
		{
			set{ _stepwarn=value;}
			get{return _stepwarn;}
		}
		/// <summary>
		/// 自动节能
		/// </summary>
		public bool? autoEnergy
		{
			set{ _autoenergy=value;}
			get{return _autoenergy;}
		}
		/// <summary>
		/// 主电源检查
		/// </summary>
		public bool? checkMainPower
		{
			set{ _checkmainpower=value;}
			get{return _checkmainpower;}
		}
		/// <summary>
		/// 检查电池
		/// </summary>
		public bool? checkCell
		{
			set{ _checkcell=value;}
			get{return _checkcell;}
		}
		/// <summary>
		/// 启用复位键
		/// </summary>
		public bool? useResetButton
		{
			set{ _useresetbutton=value;}
			get{return _useresetbutton;}
		}
		/// <summary>
		/// 启用测试键
		/// </summary>
		public bool? useTestButton
		{
			set{ _usetestbutton=value;}
			get{return _usetestbutton;}
		}
		/// <summary>
		/// 启用隔离键
		/// </summary>
		public bool? useIsolateButton
		{
			set{ _useisolatebutton=value;}
			get{return _useisolatebutton;}
		}
		/// <summary>
		/// 关闭日夜切换
		/// </summary>
		public bool? closeDaynight
		{
			set{ _closedaynight=value;}
			get{return _closedaynight;}
		}
		#endregion Model

	}
}

