using System;
using System.Data;
using System.Collections.Generic;
using Maticsoft.Common;
using GoDexData.Model;
namespace GoDexData.BLL
{
	/// <summary>
	/// nodeSet
	/// </summary>
	public partial class nodeSet
	{
		private readonly GoDexData.DAL.nodeSet dal=new GoDexData.DAL.nodeSet();
		public nodeSet()
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
		public long Add(GoDexData.Model.nodeSet model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(GoDexData.Model.nodeSet model)
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
		public GoDexData.Model.nodeSet GetModel(int machineNo)
		{

            return dal.GetModel(machineNo);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
        public GoDexData.Model.nodeSet GetModelByCache(int machineNo)
		{
			
			string CacheKey = "nodeSetModel-" + machineNo;
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
			return (GoDexData.Model.nodeSet)objModel;
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
		public List<GoDexData.Model.nodeSet> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<GoDexData.Model.nodeSet> DataTableToList(DataTable dt)
		{
			List<GoDexData.Model.nodeSet> modelList = new List<GoDexData.Model.nodeSet>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				GoDexData.Model.nodeSet model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new GoDexData.Model.nodeSet();
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
					model.machineType=dt.Rows[n]["machineType"].ToString();
					}
					if(dt.Rows[n]["airflowH_pipe1"]!=null && dt.Rows[n]["airflowH_pipe1"].ToString()!="")
					{
						model.airflowH_pipe1=decimal.Parse(dt.Rows[n]["airflowH_pipe1"].ToString());
					}
					if(dt.Rows[n]["airflowL_pipe1"]!=null && dt.Rows[n]["airflowL_pipe1"].ToString()!="")
					{
						model.airflowL_pipe1=decimal.Parse(dt.Rows[n]["airflowL_pipe1"].ToString());
					}
					if(dt.Rows[n]["airflowH_pipe2"]!=null && dt.Rows[n]["airflowH_pipe2"].ToString()!="")
					{
						model.airflowH_pipe2=decimal.Parse(dt.Rows[n]["airflowH_pipe2"].ToString());
					}
					if(dt.Rows[n]["airflowL_pipe2"]!=null && dt.Rows[n]["airflowL_pipe2"].ToString()!="")
					{
						model.airflowL_pipe2=decimal.Parse(dt.Rows[n]["airflowL_pipe2"].ToString());
					}
					if(dt.Rows[n]["airFlowH_pipe3"]!=null && dt.Rows[n]["airFlowH_pipe3"].ToString()!="")
					{
						model.airFlowH_pipe3=decimal.Parse(dt.Rows[n]["airFlowH_pipe3"].ToString());
					}
					if(dt.Rows[n]["airflowL_pipe3"]!=null && dt.Rows[n]["airflowL_pipe3"].ToString()!="")
					{
						model.airflowL_pipe3=decimal.Parse(dt.Rows[n]["airflowL_pipe3"].ToString());
					}
					if(dt.Rows[n]["ariflowH_pipe4"]!=null && dt.Rows[n]["ariflowH_pipe4"].ToString()!="")
					{
						model.ariflowH_pipe4=decimal.Parse(dt.Rows[n]["ariflowH_pipe4"].ToString());
					}
					if(dt.Rows[n]["airflowL_pipe4"]!=null && dt.Rows[n]["airflowL_pipe4"].ToString()!="")
					{
						model.airflowL_pipe4=decimal.Parse(dt.Rows[n]["airflowL_pipe4"].ToString());
					}
					if(dt.Rows[n]["fireA1_area1"]!=null && dt.Rows[n]["fireA1_area1"].ToString()!="")
					{
						model.fireA1_area1=decimal.Parse(dt.Rows[n]["fireA1_area1"].ToString());
					}
					if(dt.Rows[n]["fireA2_area1"]!=null && dt.Rows[n]["fireA2_area1"].ToString()!="")
					{
						model.fireA2_area1=decimal.Parse(dt.Rows[n]["fireA2_area1"].ToString());
					}
					if(dt.Rows[n]["fireA3_area1"]!=null && dt.Rows[n]["fireA3_area1"].ToString()!="")
					{
						model.fireA3_area1=decimal.Parse(dt.Rows[n]["fireA3_area1"].ToString());
					}
					if(dt.Rows[n]["fireA4_area1"]!=null && dt.Rows[n]["fireA4_area1"].ToString()!="")
					{
						model.fireA4_area1=decimal.Parse(dt.Rows[n]["fireA4_area1"].ToString());
					}
					if(dt.Rows[n]["fireA1_area2"]!=null && dt.Rows[n]["fireA1_area2"].ToString()!="")
					{
						model.fireA1_area2=decimal.Parse(dt.Rows[n]["fireA1_area2"].ToString());
					}
					if(dt.Rows[n]["fireA2_area2"]!=null && dt.Rows[n]["fireA2_area2"].ToString()!="")
					{
						model.fireA2_area2=decimal.Parse(dt.Rows[n]["fireA2_area2"].ToString());
					}
					if(dt.Rows[n]["fireA3_area2"]!=null && dt.Rows[n]["fireA3_area2"].ToString()!="")
					{
						model.fireA3_area2=decimal.Parse(dt.Rows[n]["fireA3_area2"].ToString());
					}
					if(dt.Rows[n]["fireA4_area2"]!=null && dt.Rows[n]["fireA4_area2"].ToString()!="")
					{
						model.fireA4_area2=decimal.Parse(dt.Rows[n]["fireA4_area2"].ToString());
					}
					if(dt.Rows[n]["fireA1_area3"]!=null && dt.Rows[n]["fireA1_area3"].ToString()!="")
					{
						model.fireA1_area3=decimal.Parse(dt.Rows[n]["fireA1_area3"].ToString());
					}
					if(dt.Rows[n]["fireA2_area3"]!=null && dt.Rows[n]["fireA2_area3"].ToString()!="")
					{
						model.fireA2_area3=decimal.Parse(dt.Rows[n]["fireA2_area3"].ToString());
					}
					if(dt.Rows[n]["fireA3_area3"]!=null && dt.Rows[n]["fireA3_area3"].ToString()!="")
					{
						model.fireA3_area3=decimal.Parse(dt.Rows[n]["fireA3_area3"].ToString());
					}
					if(dt.Rows[n]["fireA4_area3"]!=null && dt.Rows[n]["fireA4_area3"].ToString()!="")
					{
						model.fireA4_area3=decimal.Parse(dt.Rows[n]["fireA4_area3"].ToString());
					}
					if(dt.Rows[n]["fireA1_area4"]!=null && dt.Rows[n]["fireA1_area4"].ToString()!="")
					{
						model.fireA1_area4=decimal.Parse(dt.Rows[n]["fireA1_area4"].ToString());
					}
					if(dt.Rows[n]["fireA2_area4"]!=null && dt.Rows[n]["fireA2_area4"].ToString()!="")
					{
						model.fireA2_area4=decimal.Parse(dt.Rows[n]["fireA2_area4"].ToString());
					}
					if(dt.Rows[n]["fireA3_area4"]!=null && dt.Rows[n]["fireA3_area4"].ToString()!="")
					{
						model.fireA3_area4=decimal.Parse(dt.Rows[n]["fireA3_area4"].ToString());
					}
					if(dt.Rows[n]["fireA4_area4"]!=null && dt.Rows[n]["fireA4_area4"].ToString()!="")
					{
						model.fireA4_area4=decimal.Parse(dt.Rows[n]["fireA4_area4"].ToString());
					}
					if(dt.Rows[n]["pumpSpeed"]!=null && dt.Rows[n]["pumpSpeed"].ToString()!="")
					{
						model.pumpSpeed=int.Parse(dt.Rows[n]["pumpSpeed"].ToString());
					}
					if(dt.Rows[n]["enterPwd"]!=null && dt.Rows[n]["enterPwd"].ToString()!="")
					{
					model.enterPwd=dt.Rows[n]["enterPwd"].ToString();
					}
					if(dt.Rows[n]["chartSpeed"]!=null && dt.Rows[n]["chartSpeed"].ToString()!="")
					{
						model.chartSpeed=int.Parse(dt.Rows[n]["chartSpeed"].ToString());
					}
					if(dt.Rows[n]["machineTime"]!=null && dt.Rows[n]["machineTime"].ToString()!="")
					{
						model.machineTime=DateTime.Parse(dt.Rows[n]["machineTime"].ToString());
					}
					if(dt.Rows[n]["sign"]!=null && dt.Rows[n]["sign"].ToString()!="")
					{
					model.sign=dt.Rows[n]["sign"].ToString();
					}
                    if (dt.Rows[n]["a1delay"] != null && dt.Rows[n]["a1delay"].ToString() != "")
                    {
                        model.chartSpeed = int.Parse(dt.Rows[n]["a1delay"].ToString());
                    }
                    if (dt.Rows[n]["a2delay"] != null && dt.Rows[n]["a2delay"].ToString() != "")
                    {
                        model.chartSpeed = int.Parse(dt.Rows[n]["a2delay"].ToString());
                    }
                    if (dt.Rows[n]["a3delay"] != null && dt.Rows[n]["a3delay"].ToString() != "")
                    {
                        model.chartSpeed = int.Parse(dt.Rows[n]["a3delay"].ToString());
                    }
                    if (dt.Rows[n]["a4delay"] != null && dt.Rows[n]["a4delay"].ToString() != "")
                    {
                        model.chartSpeed = int.Parse(dt.Rows[n]["a4delay"].ToString());
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

