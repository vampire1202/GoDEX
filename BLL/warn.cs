using System;
using System.Data;
using System.Collections.Generic;
using Maticsoft.Common;
using GoDexData.Model;
namespace GoDexData.BLL
{
	/// <summary>
	/// warn
	/// </summary>
	public partial class warn
	{
		private readonly GoDexData.DAL.warn dal=new GoDexData.DAL.warn();
		public warn()
		{}
		#region  Method

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public long Add(GoDexData.Model.warn model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(GoDexData.Model.warn model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(long ID)
		{
			
			return dal.Delete(ID);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string IDlist )
		{
			return dal.DeleteList(IDlist );
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public GoDexData.Model.warn GetModel(long ID)
		{
			
			return dal.GetModel(ID);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public GoDexData.Model.warn GetModelByCache(long ID)
		{
			
			string CacheKey = "warnModel-" + ID;
			object objModel = Maticsoft.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(ID);
					if (objModel != null)
					{
						int ModelCache = Maticsoft.Common.ConfigHelper.GetConfigInt("ModelCache");
						Maticsoft.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (GoDexData.Model.warn)objModel;
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
		public List<GoDexData.Model.warn> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<GoDexData.Model.warn> DataTableToList(DataTable dt)
		{
			List<GoDexData.Model.warn> modelList = new List<GoDexData.Model.warn>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				GoDexData.Model.warn model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new GoDexData.Model.warn();
					if(dt.Rows[n]["ID"]!=null && dt.Rows[n]["ID"].ToString()!="")
					{
						model.ID=long.Parse(dt.Rows[n]["ID"].ToString());
					}
					if(dt.Rows[n]["machineNo"]!=null && dt.Rows[n]["machineNo"].ToString()!="")
					{
						model.machineNo=int.Parse(dt.Rows[n]["machineNo"].ToString());
					}
					if(dt.Rows[n]["warnLeval"]!=null && dt.Rows[n]["warnLeval"].ToString()!="")
					{
					model.warnLeval=dt.Rows[n]["warnLeval"].ToString();
					}
					if(dt.Rows[n]["warnDiscrib"]!=null && dt.Rows[n]["warnDiscrib"].ToString()!="")
					{
					model.warnDiscrib=dt.Rows[n]["warnDiscrib"].ToString();
					}
					if(dt.Rows[n]["userName"]!=null && dt.Rows[n]["userName"].ToString()!="")
					{
					model.userName=dt.Rows[n]["userName"].ToString();
					}
					if(dt.Rows[n]["warnDateTime"]!=null && dt.Rows[n]["warnDateTime"].ToString()!="")
					{
						model.warnDateTime=DateTime.Parse(dt.Rows[n]["warnDateTime"].ToString());
					}
					if(dt.Rows[n]["sign"]!=null && dt.Rows[n]["sign"].ToString()!="")
					{
					model.sign=dt.Rows[n]["sign"].ToString();
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

