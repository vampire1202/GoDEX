using System;
using System.Data;
using System.Collections.Generic;
//using Maticsoft.Common;
using GoDexData.Model;
namespace GoDexData.BLL
{
	/// <summary>
	/// nodeSetOther
	/// </summary>
	public partial class nodeSetOther
	{
		private readonly GoDexData.DAL.nodeSetOther dal=new GoDexData.DAL.nodeSetOther();
		public nodeSetOther()
		{}
		#region  Method

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(GoDexData.Model.nodeSetOther model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(GoDexData.Model.nodeSetOther model)
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
		public GoDexData.Model.nodeSetOther GetModel()
		{
			//该表无主键信息，请自定义主键/条件字段
			return dal.GetModel();
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public GoDexData.Model.nodeSetOther GetModelByCache()
		{
			//该表无主键信息，请自定义主键/条件字段
			string CacheKey = "nodeSetOtherModel-" ;
			object objModel = Maticsoft.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel();
					if (objModel != null)
					{
						int ModelCache = Maticsoft.Common.ConfigHelper.GetConfigInt("ModelCache");
						Maticsoft.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (GoDexData.Model.nodeSetOther)objModel;
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
		public List<GoDexData.Model.nodeSetOther> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<GoDexData.Model.nodeSetOther> DataTableToList(DataTable dt)
		{
			List<GoDexData.Model.nodeSetOther> modelList = new List<GoDexData.Model.nodeSetOther>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				GoDexData.Model.nodeSetOther model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new GoDexData.Model.nodeSetOther();
					if(dt.Rows[n]["ID"]!=null && dt.Rows[n]["ID"].ToString()!="")
					{
						model.ID=long.Parse(dt.Rows[n]["ID"].ToString());
					}
					if(dt.Rows[n]["machineNo"]!=null && dt.Rows[n]["machineNo"].ToString()!="")
					{
						model.machineNo=int.Parse(dt.Rows[n]["machineNo"].ToString());
					}
					if(dt.Rows[n]["remote_isolate"]!=null && dt.Rows[n]["remote_isolate"].ToString()!="")
					{
						if((dt.Rows[n]["remote_isolate"].ToString()=="1")||(dt.Rows[n]["remote_isolate"].ToString().ToLower()=="true"))
						{
						model.remote_isolate=true;
						}
						else
						{
							model.remote_isolate=false;
						}
					}
					if(dt.Rows[n]["remote_reset"]!=null && dt.Rows[n]["remote_reset"].ToString()!="")
					{
						if((dt.Rows[n]["remote_reset"].ToString()=="1")||(dt.Rows[n]["remote_reset"].ToString().ToLower()=="true"))
						{
						model.remote_reset=true;
						}
						else
						{
							model.remote_reset=false;
						}
					}
					if(dt.Rows[n]["remote_daynight"]!=null && dt.Rows[n]["remote_daynight"].ToString()!="")
					{
						if((dt.Rows[n]["remote_daynight"].ToString()=="1")||(dt.Rows[n]["remote_daynight"].ToString().ToLower()=="true"))
						{
						model.remote_daynight=true;
						}
						else
						{
							model.remote_daynight=false;
						}
					}
					if(dt.Rows[n]["program_isolate"]!=null && dt.Rows[n]["program_isolate"].ToString()!="")
					{
						if((dt.Rows[n]["program_isolate"].ToString()=="1")||(dt.Rows[n]["program_isolate"].ToString().ToLower()=="true"))
						{
						model.program_isolate=true;
						}
						else
						{
							model.program_isolate=false;
						}
					}
					if(dt.Rows[n]["lockWarn"]!=null && dt.Rows[n]["lockWarn"].ToString()!="")
					{
						if((dt.Rows[n]["lockWarn"].ToString()=="1")||(dt.Rows[n]["lockWarn"].ToString().ToLower()=="true"))
						{
						model.lockWarn=true;
						}
						else
						{
							model.lockWarn=false;
						}
					}
					if(dt.Rows[n]["lockFault"]!=null && dt.Rows[n]["lockFault"].ToString()!="")
					{
						if((dt.Rows[n]["lockFault"].ToString()=="1")||(dt.Rows[n]["lockFault"].ToString().ToLower()=="true"))
						{
						model.lockFault=true;
						}
						else
						{
							model.lockFault=false;
						}
					}
					if(dt.Rows[n]["stepWarn"]!=null && dt.Rows[n]["stepWarn"].ToString()!="")
					{
						if((dt.Rows[n]["stepWarn"].ToString()=="1")||(dt.Rows[n]["stepWarn"].ToString().ToLower()=="true"))
						{
						model.stepWarn=true;
						}
						else
						{
							model.stepWarn=false;
						}
					}
					if(dt.Rows[n]["autoEnergy"]!=null && dt.Rows[n]["autoEnergy"].ToString()!="")
					{
						if((dt.Rows[n]["autoEnergy"].ToString()=="1")||(dt.Rows[n]["autoEnergy"].ToString().ToLower()=="true"))
						{
						model.autoEnergy=true;
						}
						else
						{
							model.autoEnergy=false;
						}
					}
					if(dt.Rows[n]["checkMainPower"]!=null && dt.Rows[n]["checkMainPower"].ToString()!="")
					{
						if((dt.Rows[n]["checkMainPower"].ToString()=="1")||(dt.Rows[n]["checkMainPower"].ToString().ToLower()=="true"))
						{
						model.checkMainPower=true;
						}
						else
						{
							model.checkMainPower=false;
						}
					}
					if(dt.Rows[n]["checkCell"]!=null && dt.Rows[n]["checkCell"].ToString()!="")
					{
						if((dt.Rows[n]["checkCell"].ToString()=="1")||(dt.Rows[n]["checkCell"].ToString().ToLower()=="true"))
						{
						model.checkCell=true;
						}
						else
						{
							model.checkCell=false;
						}
					}
					if(dt.Rows[n]["useResetButton"]!=null && dt.Rows[n]["useResetButton"].ToString()!="")
					{
						if((dt.Rows[n]["useResetButton"].ToString()=="1")||(dt.Rows[n]["useResetButton"].ToString().ToLower()=="true"))
						{
						model.useResetButton=true;
						}
						else
						{
							model.useResetButton=false;
						}
					}
					if(dt.Rows[n]["useTestButton"]!=null && dt.Rows[n]["useTestButton"].ToString()!="")
					{
						if((dt.Rows[n]["useTestButton"].ToString()=="1")||(dt.Rows[n]["useTestButton"].ToString().ToLower()=="true"))
						{
						model.useTestButton=true;
						}
						else
						{
							model.useTestButton=false;
						}
					}
					if(dt.Rows[n]["useIsolateButton"]!=null && dt.Rows[n]["useIsolateButton"].ToString()!="")
					{
						if((dt.Rows[n]["useIsolateButton"].ToString()=="1")||(dt.Rows[n]["useIsolateButton"].ToString().ToLower()=="true"))
						{
						model.useIsolateButton=true;
						}
						else
						{
							model.useIsolateButton=false;
						}
					}
					if(dt.Rows[n]["closeDaynight"]!=null && dt.Rows[n]["closeDaynight"].ToString()!="")
					{
						if((dt.Rows[n]["closeDaynight"].ToString()=="1")||(dt.Rows[n]["closeDaynight"].ToString().ToLower()=="true"))
						{
						model.closeDaynight=true;
						}
						else
						{
							model.closeDaynight=false;
						}
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

