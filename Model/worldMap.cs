using System;
namespace GoDexData.Model
{
	/// <summary>
	/// worldMap:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class worldMap
	{
		public worldMap()
		{}
		#region Model
		private int? _id;
		private int? _worldmapno;
		private string _worldmappath;
		/// <summary>
		/// 
		/// </summary>
		public int? ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? worldMapNo
		{
			set{ _worldmapno=value;}
			get{return _worldmapno;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string worldMapPath
		{
			set{ _worldmappath=value;}
			get{return _worldmappath;}
		}
		#endregion Model

	}
}

