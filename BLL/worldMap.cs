using System;
using System.Data;
using System.Collections.Generic;
//using Maticsoft.Common;
using GoDexData.Model;
namespace GoDexData.BLL
{
	/// <summary>
	/// worldMap
	/// </summary>
	public partial class worldMap
	{
		private readonly GoDexData.DAL.worldMap dal=new GoDexData.DAL.worldMap();
		public worldMap()
		{}
		#region  Method

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(GoDexData.Model.worldMap model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(GoDexData.Model.worldMap model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete()
		{
			//该表无主键信息，请自定义主键/条件字段
			return dal.Delete();
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public GoDexData.Model.worldMap GetModel(int worldMapNo)
		{
			//该表无主键信息，请自定义主键/条件字段
            return dal.GetModel(worldMapNo);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public GoDexData.Model.worldMap GetModelByCache(int worldMapNo)
		{
			//该表无主键信息，请自定义主键/条件字段
			string CacheKey = "worldMapModel-" ;
			object objModel = Maticsoft.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(worldMapNo);
					if (objModel != null)
					{
						int ModelCache = Maticsoft.Common.ConfigHelper.GetConfigInt("ModelCache");
						Maticsoft.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (GoDexData.Model.worldMap)objModel;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			return dal.GetList(strWhere);
		}
		/// <summary>
		/// 获得前几行数据
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			return dal.GetList(Top,strWhere,filedOrder);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<GoDexData.Model.worldMap> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<GoDexData.Model.worldMap> DataTableToList(DataTable dt)
		{
			List<GoDexData.Model.worldMap> modelList = new List<GoDexData.Model.worldMap>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				GoDexData.Model.worldMap model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new GoDexData.Model.worldMap();
					if(dt.Rows[n]["ID"]!=null && dt.Rows[n]["ID"].ToString()!="")
					{
						model.ID=int.Parse(dt.Rows[n]["ID"].ToString());
					}
					if(dt.Rows[n]["worldMapNo"]!=null && dt.Rows[n]["worldMapNo"].ToString()!="")
					{
						model.worldMapNo=int.Parse(dt.Rows[n]["worldMapNo"].ToString());
					}
					if(dt.Rows[n]["worldMapPath"]!=null && dt.Rows[n]["worldMapPath"].ToString()!="")
					{
					model.worldMapPath=dt.Rows[n]["worldMapPath"].ToString();
					}
					modelList.Add(model);
				}
			}
			return modelList;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetAllList()
		{
			return GetList("");
		}

		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		//public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		//{
			//return dal.GetList(PageSize,PageIndex,strWhere);
		//}

		#endregion  Method
	}
}

