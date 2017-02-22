using System;
using System.Data;
using System.Collections.Generic;
using Maticsoft.Common;
using GoDexData.Model;
namespace GoDexData.BLL
{
	/// <summary>
	/// areaMap
	/// </summary>
	public partial class areaMap
	{
		private readonly GoDexData.DAL.areaMap dal=new GoDexData.DAL.areaMap();
		public areaMap()
		{}
		#region  Method

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
			return dal.GetMaxId();
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int areaMapNo)
		{
			return dal.Exists(areaMapNo);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(GoDexData.Model.areaMap model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(GoDexData.Model.areaMap model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int areaMapNo)
		{
			
			return dal.Delete(areaMapNo);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string areaMapNolist )
		{
			return dal.DeleteList(areaMapNolist );
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public GoDexData.Model.areaMap GetModel(int areaMapNo)
		{
			
			return dal.GetModel(areaMapNo);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public GoDexData.Model.areaMap GetModelByCache(int areaMapNo)
		{
			
			string CacheKey = "areaMapModel-" + areaMapNo;
			object objModel = Maticsoft.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(areaMapNo);
					if (objModel != null)
					{
						int ModelCache = Maticsoft.Common.ConfigHelper.GetConfigInt("ModelCache");
						Maticsoft.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (GoDexData.Model.areaMap)objModel;
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
		public List<GoDexData.Model.areaMap> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<GoDexData.Model.areaMap> DataTableToList(DataTable dt)
		{
			List<GoDexData.Model.areaMap> modelList = new List<GoDexData.Model.areaMap>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				GoDexData.Model.areaMap model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new GoDexData.Model.areaMap();
					if(dt.Rows[n]["ID"]!=null && dt.Rows[n]["ID"].ToString()!="")
					{
						model.ID=long.Parse(dt.Rows[n]["ID"].ToString());
					}
					if(dt.Rows[n]["areaMapNo"]!=null && dt.Rows[n]["areaMapNo"].ToString()!="")
					{
						model.areaMapNo=int.Parse(dt.Rows[n]["areaMapNo"].ToString());
					}
					if(dt.Rows[n]["areaMapPath"]!=null && dt.Rows[n]["areaMapPath"].ToString()!="")
					{
					model.areaMapPath=dt.Rows[n]["areaMapPath"].ToString();
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

