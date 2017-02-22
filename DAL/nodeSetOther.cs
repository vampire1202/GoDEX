using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Maticsoft.DBUtility;//Please add references
namespace GoDexData.DAL
{
	/// <summary>
	/// 数据访问类:nodeSetOther
	/// </summary>
	public partial class nodeSetOther
	{
		public nodeSetOther()
		{}
		#region  Method



		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(GoDexData.Model.nodeSetOther model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_nodeSetOther(");
			strSql.Append("ID,machineNo,remote_isolate,remote_reset,remote_daynight,program_isolate,lockWarn,lockFault,stepWarn,autoEnergy,checkMainPower,checkCell,useResetButton,useTestButton,useIsolateButton,closeDaynight)");
			strSql.Append(" values (");
			strSql.Append("@ID,@machineNo,@remote_isolate,@remote_reset,@remote_daynight,@program_isolate,@lockWarn,@lockFault,@stepWarn,@autoEnergy,@checkMainPower,@checkCell,@useResetButton,@useTestButton,@useIsolateButton,@closeDaynight)");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.BigInt,8),
					new SqlParameter("@machineNo", SqlDbType.Int,4),
					new SqlParameter("@remote_isolate", SqlDbType.Bit,1),
					new SqlParameter("@remote_reset", SqlDbType.Bit,1),
					new SqlParameter("@remote_daynight", SqlDbType.Bit,1),
					new SqlParameter("@program_isolate", SqlDbType.Bit,1),
					new SqlParameter("@lockWarn", SqlDbType.Bit,1),
					new SqlParameter("@lockFault", SqlDbType.Bit,1),
					new SqlParameter("@stepWarn", SqlDbType.Bit,1),
					new SqlParameter("@autoEnergy", SqlDbType.Bit,1),
					new SqlParameter("@checkMainPower", SqlDbType.Bit,1),
					new SqlParameter("@checkCell", SqlDbType.Bit,1),
					new SqlParameter("@useResetButton", SqlDbType.Bit,1),
					new SqlParameter("@useTestButton", SqlDbType.Bit,1),
					new SqlParameter("@useIsolateButton", SqlDbType.Bit,1),
					new SqlParameter("@closeDaynight", SqlDbType.Bit,1)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.machineNo;
			parameters[2].Value = model.remote_isolate;
			parameters[3].Value = model.remote_reset;
			parameters[4].Value = model.remote_daynight;
			parameters[5].Value = model.program_isolate;
			parameters[6].Value = model.lockWarn;
			parameters[7].Value = model.lockFault;
			parameters[8].Value = model.stepWarn;
			parameters[9].Value = model.autoEnergy;
			parameters[10].Value = model.checkMainPower;
			parameters[11].Value = model.checkCell;
			parameters[12].Value = model.useResetButton;
			parameters[13].Value = model.useTestButton;
			parameters[14].Value = model.useIsolateButton;
			parameters[15].Value = model.closeDaynight;

			int rows=DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(GoDexData.Model.nodeSetOther model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_nodeSetOther set ");
			strSql.Append("ID=@ID,");
			strSql.Append("machineNo=@machineNo,");
			strSql.Append("remote_isolate=@remote_isolate,");
			strSql.Append("remote_reset=@remote_reset,");
			strSql.Append("remote_daynight=@remote_daynight,");
			strSql.Append("program_isolate=@program_isolate,");
			strSql.Append("lockWarn=@lockWarn,");
			strSql.Append("lockFault=@lockFault,");
			strSql.Append("stepWarn=@stepWarn,");
			strSql.Append("autoEnergy=@autoEnergy,");
			strSql.Append("checkMainPower=@checkMainPower,");
			strSql.Append("checkCell=@checkCell,");
			strSql.Append("useResetButton=@useResetButton,");
			strSql.Append("useTestButton=@useTestButton,");
			strSql.Append("useIsolateButton=@useIsolateButton,");
			strSql.Append("closeDaynight=@closeDaynight");
			strSql.Append(" where ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.BigInt,8),
					new SqlParameter("@machineNo", SqlDbType.Int,4),
					new SqlParameter("@remote_isolate", SqlDbType.Bit,1),
					new SqlParameter("@remote_reset", SqlDbType.Bit,1),
					new SqlParameter("@remote_daynight", SqlDbType.Bit,1),
					new SqlParameter("@program_isolate", SqlDbType.Bit,1),
					new SqlParameter("@lockWarn", SqlDbType.Bit,1),
					new SqlParameter("@lockFault", SqlDbType.Bit,1),
					new SqlParameter("@stepWarn", SqlDbType.Bit,1),
					new SqlParameter("@autoEnergy", SqlDbType.Bit,1),
					new SqlParameter("@checkMainPower", SqlDbType.Bit,1),
					new SqlParameter("@checkCell", SqlDbType.Bit,1),
					new SqlParameter("@useResetButton", SqlDbType.Bit,1),
					new SqlParameter("@useTestButton", SqlDbType.Bit,1),
					new SqlParameter("@useIsolateButton", SqlDbType.Bit,1),
					new SqlParameter("@closeDaynight", SqlDbType.Bit,1)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.machineNo;
			parameters[2].Value = model.remote_isolate;
			parameters[3].Value = model.remote_reset;
			parameters[4].Value = model.remote_daynight;
			parameters[5].Value = model.program_isolate;
			parameters[6].Value = model.lockWarn;
			parameters[7].Value = model.lockFault;
			parameters[8].Value = model.stepWarn;
			parameters[9].Value = model.autoEnergy;
			parameters[10].Value = model.checkMainPower;
			parameters[11].Value = model.checkCell;
			parameters[12].Value = model.useResetButton;
			parameters[13].Value = model.useTestButton;
			parameters[14].Value = model.useIsolateButton;
			parameters[15].Value = model.closeDaynight;

			int rows=DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete()
		{
			//该表无主键信息，请自定义主键/条件字段
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_nodeSetOther ");
			strSql.Append(" where ");
			SqlParameter[] parameters = {
};

			int rows=DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public GoDexData.Model.nodeSetOther GetModel()
		{
			//该表无主键信息，请自定义主键/条件字段
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,machineNo,remote_isolate,remote_reset,remote_daynight,program_isolate,lockWarn,lockFault,stepWarn,autoEnergy,checkMainPower,checkCell,useResetButton,useTestButton,useIsolateButton,closeDaynight from tb_nodeSetOther ");
			strSql.Append(" where ");
			SqlParameter[] parameters = {
};

			GoDexData.Model.nodeSetOther model=new GoDexData.Model.nodeSetOther();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["ID"]!=null && ds.Tables[0].Rows[0]["ID"].ToString()!="")
				{
					model.ID=long.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
				}
				if(ds.Tables[0].Rows[0]["machineNo"]!=null && ds.Tables[0].Rows[0]["machineNo"].ToString()!="")
				{
					model.machineNo=int.Parse(ds.Tables[0].Rows[0]["machineNo"].ToString());
				}
				if(ds.Tables[0].Rows[0]["remote_isolate"]!=null && ds.Tables[0].Rows[0]["remote_isolate"].ToString()!="")
				{
					if((ds.Tables[0].Rows[0]["remote_isolate"].ToString()=="1")||(ds.Tables[0].Rows[0]["remote_isolate"].ToString().ToLower()=="true"))
					{
						model.remote_isolate=true;
					}
					else
					{
						model.remote_isolate=false;
					}
				}
				if(ds.Tables[0].Rows[0]["remote_reset"]!=null && ds.Tables[0].Rows[0]["remote_reset"].ToString()!="")
				{
					if((ds.Tables[0].Rows[0]["remote_reset"].ToString()=="1")||(ds.Tables[0].Rows[0]["remote_reset"].ToString().ToLower()=="true"))
					{
						model.remote_reset=true;
					}
					else
					{
						model.remote_reset=false;
					}
				}
				if(ds.Tables[0].Rows[0]["remote_daynight"]!=null && ds.Tables[0].Rows[0]["remote_daynight"].ToString()!="")
				{
					if((ds.Tables[0].Rows[0]["remote_daynight"].ToString()=="1")||(ds.Tables[0].Rows[0]["remote_daynight"].ToString().ToLower()=="true"))
					{
						model.remote_daynight=true;
					}
					else
					{
						model.remote_daynight=false;
					}
				}
				if(ds.Tables[0].Rows[0]["program_isolate"]!=null && ds.Tables[0].Rows[0]["program_isolate"].ToString()!="")
				{
					if((ds.Tables[0].Rows[0]["program_isolate"].ToString()=="1")||(ds.Tables[0].Rows[0]["program_isolate"].ToString().ToLower()=="true"))
					{
						model.program_isolate=true;
					}
					else
					{
						model.program_isolate=false;
					}
				}
				if(ds.Tables[0].Rows[0]["lockWarn"]!=null && ds.Tables[0].Rows[0]["lockWarn"].ToString()!="")
				{
					if((ds.Tables[0].Rows[0]["lockWarn"].ToString()=="1")||(ds.Tables[0].Rows[0]["lockWarn"].ToString().ToLower()=="true"))
					{
						model.lockWarn=true;
					}
					else
					{
						model.lockWarn=false;
					}
				}
				if(ds.Tables[0].Rows[0]["lockFault"]!=null && ds.Tables[0].Rows[0]["lockFault"].ToString()!="")
				{
					if((ds.Tables[0].Rows[0]["lockFault"].ToString()=="1")||(ds.Tables[0].Rows[0]["lockFault"].ToString().ToLower()=="true"))
					{
						model.lockFault=true;
					}
					else
					{
						model.lockFault=false;
					}
				}
				if(ds.Tables[0].Rows[0]["stepWarn"]!=null && ds.Tables[0].Rows[0]["stepWarn"].ToString()!="")
				{
					if((ds.Tables[0].Rows[0]["stepWarn"].ToString()=="1")||(ds.Tables[0].Rows[0]["stepWarn"].ToString().ToLower()=="true"))
					{
						model.stepWarn=true;
					}
					else
					{
						model.stepWarn=false;
					}
				}
				if(ds.Tables[0].Rows[0]["autoEnergy"]!=null && ds.Tables[0].Rows[0]["autoEnergy"].ToString()!="")
				{
					if((ds.Tables[0].Rows[0]["autoEnergy"].ToString()=="1")||(ds.Tables[0].Rows[0]["autoEnergy"].ToString().ToLower()=="true"))
					{
						model.autoEnergy=true;
					}
					else
					{
						model.autoEnergy=false;
					}
				}
				if(ds.Tables[0].Rows[0]["checkMainPower"]!=null && ds.Tables[0].Rows[0]["checkMainPower"].ToString()!="")
				{
					if((ds.Tables[0].Rows[0]["checkMainPower"].ToString()=="1")||(ds.Tables[0].Rows[0]["checkMainPower"].ToString().ToLower()=="true"))
					{
						model.checkMainPower=true;
					}
					else
					{
						model.checkMainPower=false;
					}
				}
				if(ds.Tables[0].Rows[0]["checkCell"]!=null && ds.Tables[0].Rows[0]["checkCell"].ToString()!="")
				{
					if((ds.Tables[0].Rows[0]["checkCell"].ToString()=="1")||(ds.Tables[0].Rows[0]["checkCell"].ToString().ToLower()=="true"))
					{
						model.checkCell=true;
					}
					else
					{
						model.checkCell=false;
					}
				}
				if(ds.Tables[0].Rows[0]["useResetButton"]!=null && ds.Tables[0].Rows[0]["useResetButton"].ToString()!="")
				{
					if((ds.Tables[0].Rows[0]["useResetButton"].ToString()=="1")||(ds.Tables[0].Rows[0]["useResetButton"].ToString().ToLower()=="true"))
					{
						model.useResetButton=true;
					}
					else
					{
						model.useResetButton=false;
					}
				}
				if(ds.Tables[0].Rows[0]["useTestButton"]!=null && ds.Tables[0].Rows[0]["useTestButton"].ToString()!="")
				{
					if((ds.Tables[0].Rows[0]["useTestButton"].ToString()=="1")||(ds.Tables[0].Rows[0]["useTestButton"].ToString().ToLower()=="true"))
					{
						model.useTestButton=true;
					}
					else
					{
						model.useTestButton=false;
					}
				}
				if(ds.Tables[0].Rows[0]["useIsolateButton"]!=null && ds.Tables[0].Rows[0]["useIsolateButton"].ToString()!="")
				{
					if((ds.Tables[0].Rows[0]["useIsolateButton"].ToString()=="1")||(ds.Tables[0].Rows[0]["useIsolateButton"].ToString().ToLower()=="true"))
					{
						model.useIsolateButton=true;
					}
					else
					{
						model.useIsolateButton=false;
					}
				}
				if(ds.Tables[0].Rows[0]["closeDaynight"]!=null && ds.Tables[0].Rows[0]["closeDaynight"].ToString()!="")
				{
					if((ds.Tables[0].Rows[0]["closeDaynight"].ToString()=="1")||(ds.Tables[0].Rows[0]["closeDaynight"].ToString().ToLower()=="true"))
					{
						model.closeDaynight=true;
					}
					else
					{
						model.closeDaynight=false;
					}
				}
				return model;
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select ID,machineNo,remote_isolate,remote_reset,remote_daynight,program_isolate,lockWarn,lockFault,stepWarn,autoEnergy,checkMainPower,checkCell,useResetButton,useTestButton,useIsolateButton,closeDaynight ");
			strSql.Append(" FROM tb_nodeSetOther ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return DbHelperSQL.Query(strSql.ToString());
		}

		/// <summary>
		/// 获得前几行数据
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select ");
			if(Top>0)
			{
				strSql.Append(" top "+Top.ToString());
			}
			strSql.Append(" ID,machineNo,remote_isolate,remote_reset,remote_daynight,program_isolate,lockWarn,lockFault,stepWarn,autoEnergy,checkMainPower,checkCell,useResetButton,useTestButton,useIsolateButton,closeDaynight ");
			strSql.Append(" FROM tb_nodeSetOther ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + filedOrder);
			return DbHelperSQL.Query(strSql.ToString());
		}

		/*
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 255),
					new SqlParameter("@fldName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@IsReCount", SqlDbType.Bit),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
					};
			parameters[0].Value = "tb_nodeSetOther";
			parameters[1].Value = "ID";
			parameters[2].Value = PageSize;
			parameters[3].Value = PageIndex;
			parameters[4].Value = 0;
			parameters[5].Value = 0;
			parameters[6].Value = strWhere;	
			return DbHelperSQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
		}*/

		#endregion  Method
	}
}

