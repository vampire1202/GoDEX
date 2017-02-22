using System;
namespace GoDexData.Model
{
	/// <summary>
	/// areaMap:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class areaMap
	{
		public areaMap()
		{}
		#region Model
		private long? _id;
		private int _areamapno;
		private string _areamappath;
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
		public int areaMapNo
		{
			set{ _areamapno=value;}
			get{return _areamapno;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string areaMapPath
		{
			set{ _areamappath=value;}
			get{return _areamappath;}
		}
		#endregion Model

	}
}

