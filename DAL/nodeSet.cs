using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Maticsoft.DBUtility;//Please add references
namespace GoDexData.DAL
{
	/// <summary>
	/// 数据访问类:nodeSet
	/// </summary>
	public partial class nodeSet
	{
		public nodeSet()
		{}
		#region  Method

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("machineNo", "tb_nodeSet"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int machineNo)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_nodeSet");
			strSql.Append(" where machineNo=@machineNo ");
			SqlParameter[] parameters = {
					new SqlParameter("@machineNo", SqlDbType.Int,4)};
			parameters[0].Value = machineNo;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public long Add(GoDexData.Model.nodeSet model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_nodeSet(");
            strSql.Append("machineNo,machineType,airflowH_pipe1,airflowL_pipe1,airflowH_pipe2,airflowL_pipe2,airFlowH_pipe3,airflowL_pipe3,ariflowH_pipe4,airflowL_pipe4,fireA1_area1,fireA2_area1,fireA3_area1,fireA4_area1,fireA1_area2,fireA2_area2,fireA3_area2,fireA4_area2,fireA1_area3,fireA2_area3,fireA3_area3,fireA4_area3,fireA1_area4,fireA2_area4,fireA3_area4,fireA4_area4,pumpSpeed,enterPwd,chartSpeed,machineTime,sign,a1delay,a2delay,a3delay,a4delay,isLock,isSeparate,isMute,isReverse)");
			strSql.Append(" values (");
            strSql.Append("@machineNo,@machineType,@airflowH_pipe1,@airflowL_pipe1,@airflowH_pipe2,@airflowL_pipe2,@airFlowH_pipe3,@airflowL_pipe3,@ariflowH_pipe4,@airflowL_pipe4,@fireA1_area1,@fireA2_area1,@fireA3_area1,@fireA4_area1,@fireA1_area2,@fireA2_area2,@fireA3_area2,@fireA4_area2,@fireA1_area3,@fireA2_area3,@fireA3_area3,@fireA4_area3,@fireA1_area4,@fireA2_area4,@fireA3_area4,@fireA4_area4,@pumpSpeed,@enterPwd,@chartSpeed,@machineTime,@sign,@a1delay,@a2delay,@a3delay,@a4delay,@isLock,@isSeparate,@isMute,@isReverse)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@machineNo", SqlDbType.Int,4),
					new SqlParameter("@machineType", SqlDbType.VarChar,50),
					new SqlParameter("@airflowH_pipe1", SqlDbType.Float,8),
					new SqlParameter("@airflowL_pipe1", SqlDbType.Float,8),
					new SqlParameter("@airflowH_pipe2", SqlDbType.Float,8),
					new SqlParameter("@airflowL_pipe2", SqlDbType.Float,8),
					new SqlParameter("@airFlowH_pipe3", SqlDbType.Float,8),
					new SqlParameter("@airflowL_pipe3", SqlDbType.Float,8),
					new SqlParameter("@ariflowH_pipe4", SqlDbType.Float,8),
					new SqlParameter("@airflowL_pipe4", SqlDbType.Float,8),
					new SqlParameter("@fireA1_area1", SqlDbType.Float,8),
					new SqlParameter("@fireA2_area1", SqlDbType.Float,8),
					new SqlParameter("@fireA3_area1", SqlDbType.Float,8),
					new SqlParameter("@fireA4_area1", SqlDbType.Float,8),
					new SqlParameter("@fireA1_area2", SqlDbType.Float,8),
					new SqlParameter("@fireA2_area2", SqlDbType.Float,8),
					new SqlParameter("@fireA3_area2", SqlDbType.Float,8),
					new SqlParameter("@fireA4_area2", SqlDbType.Float,8),
					new SqlParameter("@fireA1_area3", SqlDbType.Float,8),
					new SqlParameter("@fireA2_area3", SqlDbType.Float,8),
					new SqlParameter("@fireA3_area3", SqlDbType.Float,8),
					new SqlParameter("@fireA4_area3", SqlDbType.Float,8),
					new SqlParameter("@fireA1_area4", SqlDbType.Float,8),
					new SqlParameter("@fireA2_area4", SqlDbType.Float,8),
					new SqlParameter("@fireA3_area4", SqlDbType.Float,8),
					new SqlParameter("@fireA4_area4", SqlDbType.Float,8),
					new SqlParameter("@pumpSpeed", SqlDbType.Int,4),
					new SqlParameter("@enterPwd", SqlDbType.VarChar,50),
					new SqlParameter("@chartSpeed", SqlDbType.Int,4),
					new SqlParameter("@machineTime", SqlDbType.DateTime),
					new SqlParameter("@sign", SqlDbType.VarChar,500),
                                        new SqlParameter("@a1delay", SqlDbType.Int,4),
                                        new SqlParameter("@a2delay", SqlDbType.Int,4),
                                        new SqlParameter("@a3delay", SqlDbType.Int,4),
                                        new SqlParameter("@a4delay", SqlDbType.Int,4),
                                        new SqlParameter("@isLock",SqlDbType.Int,4),
                                         new SqlParameter("@isSeparate",SqlDbType.Int,4),
                                          new SqlParameter("@isMute",SqlDbType.Int,4),
                                           new SqlParameter("@isReverse",SqlDbType.Int,4)
                                        };
			parameters[0].Value = model.machineNo;
			parameters[1].Value = model.machineType;
			parameters[2].Value = model.airflowH_pipe1;
			parameters[3].Value = model.airflowL_pipe1;
			parameters[4].Value = model.airflowH_pipe2;
			parameters[5].Value = model.airflowL_pipe2;
			parameters[6].Value = model.airFlowH_pipe3;
			parameters[7].Value = model.airflowL_pipe3;
			parameters[8].Value = model.ariflowH_pipe4;
			parameters[9].Value = model.airflowL_pipe4;
			parameters[10].Value = model.fireA1_area1;
			parameters[11].Value = model.fireA2_area1;
			parameters[12].Value = model.fireA3_area1;
			parameters[13].Value = model.fireA4_area1;
			parameters[14].Value = model.fireA1_area2;
			parameters[15].Value = model.fireA2_area2;
			parameters[16].Value = model.fireA3_area2;
			parameters[17].Value = model.fireA4_area2;
			parameters[18].Value = model.fireA1_area3;
			parameters[19].Value = model.fireA2_area3;
			parameters[20].Value = model.fireA3_area3;
			parameters[21].Value = model.fireA4_area3;
			parameters[22].Value = model.fireA1_area4;
			parameters[23].Value = model.fireA2_area4;
			parameters[24].Value = model.fireA3_area4;
			parameters[25].Value = model.fireA4_area4;
			parameters[26].Value = model.pumpSpeed;
			parameters[27].Value = model.enterPwd;
			parameters[28].Value = model.chartSpeed;
			parameters[29].Value = model.machineTime;
			parameters[30].Value = model.sign;
            parameters[31].Value = model.a1delay;
            parameters[32].Value = model.a2delay;
            parameters[33].Value = model.a3delay;
            parameters[34].Value = model.a4delay;
            parameters[35].Value = model.isLock;
            parameters[36].Value = model.isSeparate;
            parameters[37].Value = model.isMute;
            parameters[38].Value = model.isReverse;

			object obj = DbHelperSQL.GetSingle(strSql.ToString(),parameters);
			if (obj == null)
			{
				return 0;
			}
			else
			{
				return Convert.ToInt64(obj);
			}
		}
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(GoDexData.Model.nodeSet model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_nodeSet set ");
			strSql.Append("machineType=@machineType,");
			strSql.Append("airflowH_pipe1=@airflowH_pipe1,");
			strSql.Append("airflowL_pipe1=@airflowL_pipe1,");
			strSql.Append("airflowH_pipe2=@airflowH_pipe2,");
			strSql.Append("airflowL_pipe2=@airflowL_pipe2,");
			strSql.Append("airFlowH_pipe3=@airFlowH_pipe3,");
			strSql.Append("airflowL_pipe3=@airflowL_pipe3,");
			strSql.Append("ariflowH_pipe4=@ariflowH_pipe4,");
			strSql.Append("airflowL_pipe4=@airflowL_pipe4,");
			strSql.Append("fireA1_area1=@fireA1_area1,");
			strSql.Append("fireA2_area1=@fireA2_area1,");
			strSql.Append("fireA3_area1=@fireA3_area1,");
			strSql.Append("fireA4_area1=@fireA4_area1,");
			strSql.Append("fireA1_area2=@fireA1_area2,");
			strSql.Append("fireA2_area2=@fireA2_area2,");
			strSql.Append("fireA3_area2=@fireA3_area2,");
			strSql.Append("fireA4_area2=@fireA4_area2,");
			strSql.Append("fireA1_area3=@fireA1_area3,");
			strSql.Append("fireA2_area3=@fireA2_area3,");
			strSql.Append("fireA3_area3=@fireA3_area3,");
			strSql.Append("fireA4_area3=@fireA4_area3,");
			strSql.Append("fireA1_area4=@fireA1_area4,");
			strSql.Append("fireA2_area4=@fireA2_area4,");
			strSql.Append("fireA3_area4=@fireA3_area4,");
			strSql.Append("fireA4_area4=@fireA4_area4,");
			strSql.Append("pumpSpeed=@pumpSpeed,");
			strSql.Append("enterPwd=@enterPwd,");
			strSql.Append("chartSpeed=@chartSpeed,");
			strSql.Append("machineTime=@machineTime,");
			strSql.Append("sign=@sign,");
            strSql.Append("a1delay=@a1delay,");
            strSql.Append("a2delay=@a2delay,");
            strSql.Append("a3delay=@a3delay,");
            strSql.Append("a4delay=@a4delay,");
            strSql.Append("isLock=@isLock,");
            strSql.Append("isSeparate=@isSeparate,");
            strSql.Append("isMute=@isMute,");
            strSql.Append("isReverse=@isReverse");  

			strSql.Append(" where machineNo=@machineNo");
			SqlParameter[] parameters = {
					new SqlParameter("@machineType", SqlDbType.VarChar,50),
					new SqlParameter("@airflowH_pipe1", SqlDbType.Float,8),
					new SqlParameter("@airflowL_pipe1", SqlDbType.Float,8),
					new SqlParameter("@airflowH_pipe2", SqlDbType.Float,8),
					new SqlParameter("@airflowL_pipe2", SqlDbType.Float,8),
					new SqlParameter("@airFlowH_pipe3", SqlDbType.Float,8),
					new SqlParameter("@airflowL_pipe3", SqlDbType.Float,8),
					new SqlParameter("@ariflowH_pipe4", SqlDbType.Float,8),
					new SqlParameter("@airflowL_pipe4", SqlDbType.Float,8),
					new SqlParameter("@fireA1_area1", SqlDbType.Float,8),
					new SqlParameter("@fireA2_area1", SqlDbType.Float,8),
					new SqlParameter("@fireA3_area1", SqlDbType.Float,8),
					new SqlParameter("@fireA4_area1", SqlDbType.Float,8),
					new SqlParameter("@fireA1_area2", SqlDbType.Float,8),
					new SqlParameter("@fireA2_area2", SqlDbType.Float,8),
					new SqlParameter("@fireA3_area2", SqlDbType.Float,8),
					new SqlParameter("@fireA4_area2", SqlDbType.Float,8),
					new SqlParameter("@fireA1_area3", SqlDbType.Float,8),
					new SqlParameter("@fireA2_area3", SqlDbType.Float,8),
					new SqlParameter("@fireA3_area3", SqlDbType.Float,8),
					new SqlParameter("@fireA4_area3", SqlDbType.Float,8),
					new SqlParameter("@fireA1_area4", SqlDbType.Float,8),
					new SqlParameter("@fireA2_area4", SqlDbType.Float,8),
					new SqlParameter("@fireA3_area4", SqlDbType.Float,8),
					new SqlParameter("@fireA4_area4", SqlDbType.Float,8),
					new SqlParameter("@pumpSpeed", SqlDbType.Int,4),
					new SqlParameter("@enterPwd", SqlDbType.VarChar,50),
					new SqlParameter("@chartSpeed", SqlDbType.Int,4),
					new SqlParameter("@machineTime", SqlDbType.DateTime),
					new SqlParameter("@sign", SqlDbType.VarChar,500),
                    new SqlParameter("@a1delay", SqlDbType.Int,4),
                    new SqlParameter("@a2delay", SqlDbType.Int,4),
                    new SqlParameter("@a3delay", SqlDbType.Int,4),
                    new SqlParameter("@a4delay", SqlDbType.Int,4), 

                    new SqlParameter("@isLock", SqlDbType.Int,4),
                    new SqlParameter("@isSeparate", SqlDbType.Int,4),
                    new SqlParameter("@isMute", SqlDbType.Int,4),
                    new SqlParameter("@isReverse", SqlDbType.Int,4),  

					new SqlParameter("@machineNo", SqlDbType.Int,4)};

			parameters[0].Value = model.machineType;
			parameters[1].Value = model.airflowH_pipe1;
			parameters[2].Value = model.airflowL_pipe1;
			parameters[3].Value = model.airflowH_pipe2;
			parameters[4].Value = model.airflowL_pipe2;
			parameters[5].Value = model.airFlowH_pipe3;
			parameters[6].Value = model.airflowL_pipe3;
			parameters[7].Value = model.ariflowH_pipe4;
			parameters[8].Value = model.airflowL_pipe4;
			parameters[9].Value = model.fireA1_area1;
			parameters[10].Value = model.fireA2_area1;
			parameters[11].Value = model.fireA3_area1;
			parameters[12].Value = model.fireA4_area1;
			parameters[13].Value = model.fireA1_area2;
			parameters[14].Value = model.fireA2_area2;
			parameters[15].Value = model.fireA3_area2;
			parameters[16].Value = model.fireA4_area2;
			parameters[17].Value = model.fireA1_area3;
			parameters[18].Value = model.fireA2_area3;
			parameters[19].Value = model.fireA3_area3;
			parameters[20].Value = model.fireA4_area3;
			parameters[21].Value = model.fireA1_area4;
			parameters[22].Value = model.fireA2_area4;
			parameters[23].Value = model.fireA3_area4;
			parameters[24].Value = model.fireA4_area4;
			parameters[25].Value = model.pumpSpeed;
			parameters[26].Value = model.enterPwd;
			parameters[27].Value = model.chartSpeed;
			parameters[28].Value = model.machineTime;
			parameters[29].Value = model.sign; 
            parameters[30].Value = model.a1delay;
            parameters[31].Value = model.a2delay;
            parameters[32].Value = model.a3delay;
            parameters[33].Value = model.a4delay; 
			//parameters[34].Value = model.ID;

            parameters[34].Value = model.isLock;
            parameters[35].Value = model.isSeparate;
            parameters[36].Value = model.isMute;
            parameters[37].Value = model.isReverse;


			parameters[38].Value = model.machineNo;

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
		public bool Delete(long ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_nodeSet ");
			strSql.Append(" where ID=@ID");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.BigInt)
};
			parameters[0].Value = ID;

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
		public bool Delete(int machineNo)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_nodeSet ");
			strSql.Append(" where machineNo=@machineNo ");
			SqlParameter[] parameters = {
					new SqlParameter("@machineNo", SqlDbType.Int,4)};
			parameters[0].Value = machineNo;

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
		/// 批量删除数据
		/// </summary>
		public bool DeleteList(string IDlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_nodeSet ");
			strSql.Append(" where ID in ("+IDlist + ")  ");
			int rows=DbHelperSQL.ExecuteSql(strSql.ToString());
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
		public GoDexData.Model.nodeSet GetModel(int machineNo)
		{
			
			StringBuilder strSql=new StringBuilder();
            strSql.Append("select  top 1 ID,machineNo,machineType,airflowH_pipe1,airflowL_pipe1,airflowH_pipe2,airflowL_pipe2,airFlowH_pipe3,airflowL_pipe3,ariflowH_pipe4,airflowL_pipe4,fireA1_area1,fireA2_area1,fireA3_area1,fireA4_area1,fireA1_area2,fireA2_area2,fireA3_area2,fireA4_area2,fireA1_area3,fireA2_area3,fireA3_area3,fireA4_area3,fireA1_area4,fireA2_area4,fireA3_area4,fireA4_area4,pumpSpeed,enterPwd,chartSpeed,machineTime,sign,a1delay,a2delay,a3delay,a4delay,isLock,isSeparate,isMute,isReverse from tb_nodeSet ");
            strSql.Append(" where machineNo=@machineNo");
			SqlParameter[] parameters = {
					new SqlParameter("@machineNo", SqlDbType.Int)
};
            parameters[0].Value = machineNo;

			GoDexData.Model.nodeSet model=new GoDexData.Model.nodeSet();
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
				if(ds.Tables[0].Rows[0]["machineType"]!=null && ds.Tables[0].Rows[0]["machineType"].ToString()!="")
				{
					model.machineType=ds.Tables[0].Rows[0]["machineType"].ToString();
				}
				if(ds.Tables[0].Rows[0]["airflowH_pipe1"]!=null && ds.Tables[0].Rows[0]["airflowH_pipe1"].ToString()!="")
				{
					model.airflowH_pipe1=decimal.Parse(ds.Tables[0].Rows[0]["airflowH_pipe1"].ToString());
				}
				if(ds.Tables[0].Rows[0]["airflowL_pipe1"]!=null && ds.Tables[0].Rows[0]["airflowL_pipe1"].ToString()!="")
				{
					model.airflowL_pipe1=decimal.Parse(ds.Tables[0].Rows[0]["airflowL_pipe1"].ToString());
				}
				if(ds.Tables[0].Rows[0]["airflowH_pipe2"]!=null && ds.Tables[0].Rows[0]["airflowH_pipe2"].ToString()!="")
				{
					model.airflowH_pipe2=decimal.Parse(ds.Tables[0].Rows[0]["airflowH_pipe2"].ToString());
				}
				if(ds.Tables[0].Rows[0]["airflowL_pipe2"]!=null && ds.Tables[0].Rows[0]["airflowL_pipe2"].ToString()!="")
				{
					model.airflowL_pipe2=decimal.Parse(ds.Tables[0].Rows[0]["airflowL_pipe2"].ToString());
				}
				if(ds.Tables[0].Rows[0]["airFlowH_pipe3"]!=null && ds.Tables[0].Rows[0]["airFlowH_pipe3"].ToString()!="")
				{
					model.airFlowH_pipe3=decimal.Parse(ds.Tables[0].Rows[0]["airFlowH_pipe3"].ToString());
				}
				if(ds.Tables[0].Rows[0]["airflowL_pipe3"]!=null && ds.Tables[0].Rows[0]["airflowL_pipe3"].ToString()!="")
				{
					model.airflowL_pipe3=decimal.Parse(ds.Tables[0].Rows[0]["airflowL_pipe3"].ToString());
				}
				if(ds.Tables[0].Rows[0]["ariflowH_pipe4"]!=null && ds.Tables[0].Rows[0]["ariflowH_pipe4"].ToString()!="")
				{
					model.ariflowH_pipe4=decimal.Parse(ds.Tables[0].Rows[0]["ariflowH_pipe4"].ToString());
				}
				if(ds.Tables[0].Rows[0]["airflowL_pipe4"]!=null && ds.Tables[0].Rows[0]["airflowL_pipe4"].ToString()!="")
				{
					model.airflowL_pipe4=decimal.Parse(ds.Tables[0].Rows[0]["airflowL_pipe4"].ToString());
				}
				if(ds.Tables[0].Rows[0]["fireA1_area1"]!=null && ds.Tables[0].Rows[0]["fireA1_area1"].ToString()!="")
				{
					model.fireA1_area1=decimal.Parse(ds.Tables[0].Rows[0]["fireA1_area1"].ToString());
				}
				if(ds.Tables[0].Rows[0]["fireA2_area1"]!=null && ds.Tables[0].Rows[0]["fireA2_area1"].ToString()!="")
				{
					model.fireA2_area1=decimal.Parse(ds.Tables[0].Rows[0]["fireA2_area1"].ToString());
				}
				if(ds.Tables[0].Rows[0]["fireA3_area1"]!=null && ds.Tables[0].Rows[0]["fireA3_area1"].ToString()!="")
				{
					model.fireA3_area1=decimal.Parse(ds.Tables[0].Rows[0]["fireA3_area1"].ToString());
				}
				if(ds.Tables[0].Rows[0]["fireA4_area1"]!=null && ds.Tables[0].Rows[0]["fireA4_area1"].ToString()!="")
				{
					model.fireA4_area1=decimal.Parse(ds.Tables[0].Rows[0]["fireA4_area1"].ToString());
				}
				if(ds.Tables[0].Rows[0]["fireA1_area2"]!=null && ds.Tables[0].Rows[0]["fireA1_area2"].ToString()!="")
				{
					model.fireA1_area2=decimal.Parse(ds.Tables[0].Rows[0]["fireA1_area2"].ToString());
				}
				if(ds.Tables[0].Rows[0]["fireA2_area2"]!=null && ds.Tables[0].Rows[0]["fireA2_area2"].ToString()!="")
				{
					model.fireA2_area2=decimal.Parse(ds.Tables[0].Rows[0]["fireA2_area2"].ToString());
				}
				if(ds.Tables[0].Rows[0]["fireA3_area2"]!=null && ds.Tables[0].Rows[0]["fireA3_area2"].ToString()!="")
				{
					model.fireA3_area2=decimal.Parse(ds.Tables[0].Rows[0]["fireA3_area2"].ToString());
				}
				if(ds.Tables[0].Rows[0]["fireA4_area2"]!=null && ds.Tables[0].Rows[0]["fireA4_area2"].ToString()!="")
				{
					model.fireA4_area2=decimal.Parse(ds.Tables[0].Rows[0]["fireA4_area2"].ToString());
				}
				if(ds.Tables[0].Rows[0]["fireA1_area3"]!=null && ds.Tables[0].Rows[0]["fireA1_area3"].ToString()!="")
				{
					model.fireA1_area3=decimal.Parse(ds.Tables[0].Rows[0]["fireA1_area3"].ToString());
				}
				if(ds.Tables[0].Rows[0]["fireA2_area3"]!=null && ds.Tables[0].Rows[0]["fireA2_area3"].ToString()!="")
				{
					model.fireA2_area3=decimal.Parse(ds.Tables[0].Rows[0]["fireA2_area3"].ToString());
				}
				if(ds.Tables[0].Rows[0]["fireA3_area3"]!=null && ds.Tables[0].Rows[0]["fireA3_area3"].ToString()!="")
				{
					model.fireA3_area3=decimal.Parse(ds.Tables[0].Rows[0]["fireA3_area3"].ToString());
				}
				if(ds.Tables[0].Rows[0]["fireA4_area3"]!=null && ds.Tables[0].Rows[0]["fireA4_area3"].ToString()!="")
				{
					model.fireA4_area3=decimal.Parse(ds.Tables[0].Rows[0]["fireA4_area3"].ToString());
				}
				if(ds.Tables[0].Rows[0]["fireA1_area4"]!=null && ds.Tables[0].Rows[0]["fireA1_area4"].ToString()!="")
				{
					model.fireA1_area4=decimal.Parse(ds.Tables[0].Rows[0]["fireA1_area4"].ToString());
				}
				if(ds.Tables[0].Rows[0]["fireA2_area4"]!=null && ds.Tables[0].Rows[0]["fireA2_area4"].ToString()!="")
				{
					model.fireA2_area4=decimal.Parse(ds.Tables[0].Rows[0]["fireA2_area4"].ToString());
				}
				if(ds.Tables[0].Rows[0]["fireA3_area4"]!=null && ds.Tables[0].Rows[0]["fireA3_area4"].ToString()!="")
				{
					model.fireA3_area4=decimal.Parse(ds.Tables[0].Rows[0]["fireA3_area4"].ToString());
				}
				if(ds.Tables[0].Rows[0]["fireA4_area4"]!=null && ds.Tables[0].Rows[0]["fireA4_area4"].ToString()!="")
				{
					model.fireA4_area4=decimal.Parse(ds.Tables[0].Rows[0]["fireA4_area4"].ToString());
				}
				if(ds.Tables[0].Rows[0]["pumpSpeed"]!=null && ds.Tables[0].Rows[0]["pumpSpeed"].ToString()!="")
				{
					model.pumpSpeed=int.Parse(ds.Tables[0].Rows[0]["pumpSpeed"].ToString());
				}
				if(ds.Tables[0].Rows[0]["enterPwd"]!=null && ds.Tables[0].Rows[0]["enterPwd"].ToString()!="")
				{
					model.enterPwd=ds.Tables[0].Rows[0]["enterPwd"].ToString();
				}
				if(ds.Tables[0].Rows[0]["chartSpeed"]!=null && ds.Tables[0].Rows[0]["chartSpeed"].ToString()!="")
				{
					model.chartSpeed=int.Parse(ds.Tables[0].Rows[0]["chartSpeed"].ToString());
				}
				if(ds.Tables[0].Rows[0]["machineTime"]!=null && ds.Tables[0].Rows[0]["machineTime"].ToString()!="")
				{
					model.machineTime=DateTime.Parse(ds.Tables[0].Rows[0]["machineTime"].ToString());
				}
				if(ds.Tables[0].Rows[0]["sign"]!=null && ds.Tables[0].Rows[0]["sign"].ToString()!="")
				{
					model.sign=ds.Tables[0].Rows[0]["sign"].ToString();
				}
                if (ds.Tables[0].Rows[0]["a1delay"] != null && ds.Tables[0].Rows[0]["a1delay"].ToString() != "")
                {
                    model.a1delay = int.Parse(ds.Tables[0].Rows[0]["a1delay"].ToString());
                }
                if (ds.Tables[0].Rows[0]["a2delay"] != null && ds.Tables[0].Rows[0]["a2delay"].ToString() != "")
                {
                    model.a2delay = int.Parse(ds.Tables[0].Rows[0]["a2delay"].ToString());
                }
                if (ds.Tables[0].Rows[0]["a3delay"] != null && ds.Tables[0].Rows[0]["a3delay"].ToString() != "")
                {
                    model.a3delay = int.Parse(ds.Tables[0].Rows[0]["a3delay"].ToString());
                }
                if (ds.Tables[0].Rows[0]["a4delay"] != null && ds.Tables[0].Rows[0]["a4delay"].ToString() != "")
                {
                    model.a4delay = int.Parse(ds.Tables[0].Rows[0]["a4delay"].ToString());
                }

                if (ds.Tables[0].Rows[0]["isLock"] != null && ds.Tables[0].Rows[0]["isLock"].ToString() != "")
                {
                    model.isLock = int.Parse(ds.Tables[0].Rows[0]["isLock"].ToString());
                }
                if (ds.Tables[0].Rows[0]["isSeparate"] != null && ds.Tables[0].Rows[0]["isSeparate"].ToString() != "")
                {
                    model.isSeparate = int.Parse(ds.Tables[0].Rows[0]["isSeparate"].ToString());
                }
                if (ds.Tables[0].Rows[0]["isMute"] != null && ds.Tables[0].Rows[0]["isMute"].ToString() != "")
                {
                    model.isMute = int.Parse(ds.Tables[0].Rows[0]["isMute"].ToString());
                }
                if (ds.Tables[0].Rows[0]["isReverse"] != null && ds.Tables[0].Rows[0]["isReverse"].ToString() != "")
                {
                    model.isReverse = int.Parse(ds.Tables[0].Rows[0]["isReverse"].ToString());
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
            strSql.Append("select ID,machineNo,machineType,airflowH_pipe1,airflowL_pipe1,airflowH_pipe2,airflowL_pipe2,airFlowH_pipe3,airflowL_pipe3,ariflowH_pipe4,airflowL_pipe4,fireA1_area1,fireA2_area1,fireA3_area1,fireA4_area1,fireA1_area2,fireA2_area2,fireA3_area2,fireA4_area2,fireA1_area3,fireA2_area3,fireA3_area3,fireA4_area3,fireA1_area4,fireA2_area4,fireA3_area4,fireA4_area4,pumpSpeed,enterPwd,chartSpeed,machineTime,sign,a1delay,a2delay,a3delay,a4delay,isLock,isSeparate,isMute,isReverse ");
			strSql.Append(" FROM tb_nodeSet ");
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
            strSql.Append(" ID,machineNo,machineType,airflowH_pipe1,airflowL_pipe1,airflowH_pipe2,airflowL_pipe2,airFlowH_pipe3,airflowL_pipe3,ariflowH_pipe4,airflowL_pipe4,fireA1_area1,fireA2_area1,fireA3_area1,fireA4_area1,fireA1_area2,fireA2_area2,fireA3_area2,fireA4_area2,fireA1_area3,fireA2_area3,fireA3_area3,fireA4_area3,fireA1_area4,fireA2_area4,fireA3_area4,fireA4_area4,pumpSpeed,enterPwd,chartSpeed,machineTime,sign,a1delay,a2delay,a3delay,a4delay,isLock,isSeparate,isMute,isReverse ");
			strSql.Append(" FROM tb_nodeSet ");
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
			parameters[0].Value = "tb_nodeSet";
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

