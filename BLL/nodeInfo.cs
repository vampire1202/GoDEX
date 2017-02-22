using System;
using System.Data;
using System.Collections.Generic;
//using Maticsoft.Common;
using GoDexData.Model;
namespace GoDexData.BLL
{
	/// <summary>
	/// nodeInfo
	/// </summary>
	public partial class nodeInfo
	{
		private readonly GoDexData.DAL.nodeInfo dal=new GoDexData.DAL.nodeInfo();
		public nodeInfo()
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
		public bool Exists(int machineNo)
		{
			return dal.Exists(machineNo);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public long Add(GoDexData.Model.nodeInfo model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(GoDexData.Model.nodeInfo model)
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
		public bool Delete(int machineNo)
		{
			
			return dal.Delete(machineNo);
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
		public GoDexData.Model.nodeInfo GetModel(int machineNo)
		{
			
			return dal.GetModel(machineNo);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public GoDexData.Model.nodeInfo GetModelByCache(int machineNo)
		{
			
			string CacheKey = "nodeInfoModel-" + machineNo;
			object objModel = Maticsoft.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(machineNo);
					if (objModel != null)
					{
						int ModelCache = Maticsoft.Common.ConfigHelper.GetConfigInt("ModelCache");
						Maticsoft.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (GoDexData.Model.nodeInfo)objModel;
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
		public List<GoDexData.Model.nodeInfo> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<GoDexData.Model.nodeInfo> DataTableToList(DataTable dt)
		{
			List<GoDexData.Model.nodeInfo> modelList = new List<GoDexData.Model.nodeInfo>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				GoDexData.Model.nodeInfo model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new GoDexData.Model.nodeInfo();
					if(dt.Rows[n]["ID"]!=null && dt.Rows[n]["ID"].ToString()!="")
					{
						model.ID=long.Parse(dt.Rows[n]["ID"].ToString());
					}
					if(dt.Rows[n]["machineNo"]!=null && dt.Rows[n]["machineNo"].ToString()!="")
					{
						model.machineNo=int.Parse(dt.Rows[n]["machineNo"].ToString());
					}
					if(dt.Rows[n]["machineType"]!=null && dt.Rows[n]["machineType"].ToString()!="")
					{
						model.machineType=int.Parse(dt.Rows[n]["machineType"].ToString());
					}
					if(dt.Rows[n]["softversion"]!=null && dt.Rows[n]["softversion"].ToString()!="")
					{
					model.softversion=dt.Rows[n]["softversion"].ToString();
					}
					if(dt.Rows[n]["doordog"]!=null && dt.Rows[n]["doordog"].ToString()!="")
					{
						model.doordog=int.Parse(dt.Rows[n]["doordog"].ToString());
					}
					if(dt.Rows[n]["runtime"]!=null && dt.Rows[n]["runtime"].ToString()!="")
					{
					model.runtime=dt.Rows[n]["runtime"].ToString();
					}
					if(dt.Rows[n]["worldMapPath"]!=null && dt.Rows[n]["worldMapPath"].ToString()!="")
					{
					model.worldMapPath=dt.Rows[n]["worldMapPath"].ToString();
					}
					if(dt.Rows[n]["areaMapPath"]!=null && dt.Rows[n]["areaMapPath"].ToString()!="")
					{
					model.areaMapPath=dt.Rows[n]["areaMapPath"].ToString();
					}
					if(dt.Rows[n]["worldXY"]!=null && dt.Rows[n]["worldXY"].ToString()!="")
					{
					model.worldXY=dt.Rows[n]["worldXY"].ToString();
					}
					if(dt.Rows[n]["worldXY_1"]!=null && dt.Rows[n]["worldXY_1"].ToString()!="")
					{
					model.worldXY_1=dt.Rows[n]["worldXY_1"].ToString();
					}
					if(dt.Rows[n]["worldXY_2"]!=null && dt.Rows[n]["worldXY_2"].ToString()!="")
					{
					model.worldXY_2=dt.Rows[n]["worldXY_2"].ToString();
					}
					if(dt.Rows[n]["worldXY_3"]!=null && dt.Rows[n]["worldXY_3"].ToString()!="")
					{
					model.worldXY_3=dt.Rows[n]["worldXY_3"].ToString();
					}
					if(dt.Rows[n]["worldXY_4"]!=null && dt.Rows[n]["worldXY_4"].ToString()!="")
					{
					model.worldXY_4=dt.Rows[n]["worldXY_4"].ToString();
					}
					if(dt.Rows[n]["areaXY"]!=null && dt.Rows[n]["areaXY"].ToString()!="")
					{
					model.areaXY=dt.Rows[n]["areaXY"].ToString();
					}
					if(dt.Rows[n]["areaXY_1"]!=null && dt.Rows[n]["areaXY_1"].ToString()!="")
					{
					model.areaXY_1=dt.Rows[n]["areaXY_1"].ToString();
					}
					if(dt.Rows[n]["areaXY_2"]!=null && dt.Rows[n]["areaXY_2"].ToString()!="")
					{
					model.areaXY_2=dt.Rows[n]["areaXY_2"].ToString();
					}
					if(dt.Rows[n]["areaXY_3"]!=null && dt.Rows[n]["areaXY_3"].ToString()!="")
					{
					model.areaXY_3=dt.Rows[n]["areaXY_3"].ToString();
					}
					if(dt.Rows[n]["areaXY_4"]!=null && dt.Rows[n]["areaXY_4"].ToString()!="")
					{
					model.areaXY_4=dt.Rows[n]["areaXY_4"].ToString();
					}
					if(dt.Rows[n]["sign"]!=null && dt.Rows[n]["sign"].ToString()!="")
					{
					model.sign=dt.Rows[n]["sign"].ToString();
					}

                    if (dt.Rows[n]["fireChl1"] != null && dt.Rows[n]["fireChl1"].ToString() != "")
                    {
                        model.fireChl1 = int.Parse(dt.Rows[n]["fireChl1"].ToString());
                    }
                    if (dt.Rows[n]["fireChl2"] != null && dt.Rows[n]["fireChl2"].ToString() != "")
                    {
                        model.fireChl2 = int.Parse(dt.Rows[n]["fireChl2"].ToString());
                    }
                    if (dt.Rows[n]["fireChl3"] != null && dt.Rows[n]["fireChl3"].ToString() != "")
                    {
                        model.fireChl3 = int.Parse(dt.Rows[n]["fireChl3"].ToString());
                    }
                    if (dt.Rows[n]["fireChl4"] != null && dt.Rows[n]["fireChl4"].ToString() != "")
                    {
                        model.fireChl4 = int.Parse(dt.Rows[n]["fireChl4"].ToString());
                    }

                    if (dt.Rows[n]["airChl1"] != null && dt.Rows[n]["airChl1"].ToString() != "")
                    {
                        model.airChl1 = int.Parse(dt.Rows[n]["airChl1"].ToString());
                    }

                    if (dt.Rows[n]["airChl2"] != null && dt.Rows[n]["airChl2"].ToString() != "")
                    {
                        model.airChl2 = int.Parse(dt.Rows[n]["airChl2"].ToString());
                    }
                    if (dt.Rows[n]["airChl3"] != null && dt.Rows[n]["airChl3"].ToString() != "")
                    {
                        model.airChl3 = int.Parse(dt.Rows[n]["airChl3"].ToString());
                    }
                    if (dt.Rows[n]["airChl4"] != null && dt.Rows[n]["airChl4"].ToString() != "")
                    {
                        model.airChl4 = int.Parse(dt.Rows[n]["airChl4"].ToString());
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

