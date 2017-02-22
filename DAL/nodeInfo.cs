using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Maticsoft.DBUtility;//Please add references
namespace GoDexData.DAL
{
	/// <summary>
	/// 数据访问类:nodeInfo
	/// </summary>
	public partial class nodeInfo
	{
		public nodeInfo()
		{}
		#region  Method

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("machineNo", "tb_nodeInfo"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int machineNo)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_nodeInfo");
			strSql.Append(" where machineNo=@machineNo ");
			SqlParameter[] parameters = {
					new SqlParameter("@machineNo", SqlDbType.Int,4)};
			parameters[0].Value = machineNo;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public long Add(GoDexData.Model.nodeInfo model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_nodeInfo(");
			strSql.Append("machineNo,machineType,softversion,doordog,runtime,worldMapPath,areaMapPath,worldXY,worldXY_1,worldXY_2,worldXY_3,worldXY_4,areaXY,areaXY_1,areaXY_2,areaXY_3,areaXY_4,sign,machineModel,lvwangdate,fireChl1,fireChl2,fireChl3,fireChl4,airChl1,airChl2,airChl3,airChl4)");
			strSql.Append(" values (");
            strSql.Append("@machineNo,@machineType,@softversion,@doordog,@runtime,@worldMapPath,@areaMapPath,@worldXY,@worldXY_1,@worldXY_2,@worldXY_3,@worldXY_4,@areaXY,@areaXY_1,@areaXY_2,@areaXY_3,@areaXY_4,@sign,@machineModel,@lvwangdate,@fireChl1,@fireChl2,@fireChl3,@fireChl4,@airChl1,@airChl2,@airChl3,@airChl4)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@machineNo", SqlDbType.Int,4),
					new SqlParameter("@machineType", SqlDbType.Int,4),
					new SqlParameter("@softversion", SqlDbType.VarChar,50),
					new SqlParameter("@doordog", SqlDbType.Int,4),
					new SqlParameter("@runtime", SqlDbType.VarChar,50),
					new SqlParameter("@worldMapPath", SqlDbType.VarChar,300),
					new SqlParameter("@areaMapPath", SqlDbType.VarChar,300),
					new SqlParameter("@worldXY", SqlDbType.VarChar,50),
					new SqlParameter("@worldXY_1", SqlDbType.VarChar,50),
					new SqlParameter("@worldXY_2", SqlDbType.VarChar,50),
					new SqlParameter("@worldXY_3", SqlDbType.VarChar,50),
					new SqlParameter("@worldXY_4", SqlDbType.VarChar,50),
					new SqlParameter("@areaXY", SqlDbType.VarChar,50),
					new SqlParameter("@areaXY_1", SqlDbType.VarChar,50),
					new SqlParameter("@areaXY_2", SqlDbType.VarChar,50),
					new SqlParameter("@areaXY_3", SqlDbType.VarChar,50),
					new SqlParameter("@areaXY_4", SqlDbType.VarChar,50),
					new SqlParameter("@sign", SqlDbType.VarChar,500),
                    new SqlParameter("@machineModel", SqlDbType.VarChar,100),
                    new SqlParameter("@lvwangdate",SqlDbType.DateTime),
                    new SqlParameter("@fireChl1", SqlDbType.Int,4),
                    new SqlParameter("@fireChl2", SqlDbType.Int,4),
                    new SqlParameter("@fireChl3", SqlDbType.Int,4),
                    new SqlParameter("@fireChl4", SqlDbType.Int,4),
                    new SqlParameter("@airChl1", SqlDbType.Int,4),
                    new SqlParameter("@airChl2", SqlDbType.Int,4),
                    new SqlParameter("@airChl3", SqlDbType.Int,4),
                    new SqlParameter("@airChl4", SqlDbType.Int,4)
                                        };
			parameters[0].Value = model.machineNo;
			parameters[1].Value = model.machineType;
			parameters[2].Value = model.softversion;
			parameters[3].Value = model.doordog;
			parameters[4].Value = model.runtime;
			parameters[5].Value = model.worldMapPath;
			parameters[6].Value = model.areaMapPath;
			parameters[7].Value = model.worldXY;
			parameters[8].Value = model.worldXY_1;
			parameters[9].Value = model.worldXY_2;
			parameters[10].Value = model.worldXY_3;
			parameters[11].Value = model.worldXY_4;
			parameters[12].Value = model.areaXY;
			parameters[13].Value = model.areaXY_1;
			parameters[14].Value = model.areaXY_2;
			parameters[15].Value = model.areaXY_3;
			parameters[16].Value = model.areaXY_4;
			parameters[17].Value = model.sign;
            parameters[18].Value = model.machineModel;
            parameters[19].Value = model.lvwangdate;
            parameters[20].Value = model.fireChl1;
            parameters[21].Value = model.fireChl2;
            parameters[22].Value = model.fireChl3;
            parameters[23].Value = model.fireChl4;
            parameters[24].Value = model.airChl1;
            parameters[25].Value = model.airChl2;
            parameters[26].Value = model.airChl3;
            parameters[27].Value = model.airChl4;

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
		public bool Update(GoDexData.Model.nodeInfo model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_nodeInfo set ");
			strSql.Append("machineType=@machineType,");
			strSql.Append("softversion=@softversion,");
			strSql.Append("doordog=@doordog,");
			strSql.Append("runtime=@runtime,");
			strSql.Append("worldMapPath=@worldMapPath,");
			strSql.Append("areaMapPath=@areaMapPath,");
			strSql.Append("worldXY=@worldXY,");
			strSql.Append("worldXY_1=@worldXY_1,");
			strSql.Append("worldXY_2=@worldXY_2,");
			strSql.Append("worldXY_3=@worldXY_3,");
			strSql.Append("worldXY_4=@worldXY_4,");
			strSql.Append("areaXY=@areaXY,");
			strSql.Append("areaXY_1=@areaXY_1,");
			strSql.Append("areaXY_2=@areaXY_2,");
			strSql.Append("areaXY_3=@areaXY_3,");
			strSql.Append("areaXY_4=@areaXY_4,");
			strSql.Append("sign=@sign,");
            strSql.Append("machineModel=@machineModel,");
            strSql.Append("lvwangdate=@lvwangdate,");
            strSql.Append("fireChl1=@fireChl1,");
            strSql.Append("fireChl2=@fireChl2,");
            strSql.Append("fireChl3=@fireChl3,");
            strSql.Append("fireChl4=@fireChl4,");
            strSql.Append("airChl1=@airChl1,");
            strSql.Append("airChl2=@airChl2,");
            strSql.Append("airChl3=@airChl3,");
            strSql.Append("airChl4=@airChl4 ");


			strSql.Append(" where machineNo=@machineNo");
			SqlParameter[] parameters = {
					new SqlParameter("@machineType", SqlDbType.Int,4),
					new SqlParameter("@softversion", SqlDbType.VarChar,50),
					new SqlParameter("@doordog", SqlDbType.Int,4),
					new SqlParameter("@runtime", SqlDbType.VarChar,50),
					new SqlParameter("@worldMapPath", SqlDbType.VarChar,300),
					new SqlParameter("@areaMapPath", SqlDbType.VarChar,300),
					new SqlParameter("@worldXY", SqlDbType.VarChar,50),
					new SqlParameter("@worldXY_1", SqlDbType.VarChar,50),
					new SqlParameter("@worldXY_2", SqlDbType.VarChar,50),
					new SqlParameter("@worldXY_3", SqlDbType.VarChar,50),
					new SqlParameter("@worldXY_4", SqlDbType.VarChar,50),
					new SqlParameter("@areaXY", SqlDbType.VarChar,50),
					new SqlParameter("@areaXY_1", SqlDbType.VarChar,50),
					new SqlParameter("@areaXY_2", SqlDbType.VarChar,50),
					new SqlParameter("@areaXY_3", SqlDbType.VarChar,50),
					new SqlParameter("@areaXY_4", SqlDbType.VarChar,50),
					new SqlParameter("@sign", SqlDbType.VarChar,500), 
                    new SqlParameter("@machineModel", SqlDbType.VarChar,100), 
                    new SqlParameter("@lvwangdate",SqlDbType.DateTime),
                    new SqlParameter("@fireChl1", SqlDbType.Int,4),
                    new SqlParameter("@fireChl2", SqlDbType.Int,4),
                    new SqlParameter("@fireChl3", SqlDbType.Int,4),
                    new SqlParameter("@fireChl4", SqlDbType.Int,4),
                    new SqlParameter("@airChl1", SqlDbType.Int,4),
                    new SqlParameter("@airChl2", SqlDbType.Int,4),
                    new SqlParameter("@airChl3", SqlDbType.Int,4),
                    new SqlParameter("@airChl4", SqlDbType.Int,4),
                    new SqlParameter("@machineNo", SqlDbType.Int,4)
                                        };
			parameters[0].Value = model.machineType;
			parameters[1].Value = model.softversion;
			parameters[2].Value = model.doordog;
			parameters[3].Value = model.runtime;
			parameters[4].Value = model.worldMapPath;
			parameters[5].Value = model.areaMapPath;
			parameters[6].Value = model.worldXY;
			parameters[7].Value = model.worldXY_1;
			parameters[8].Value = model.worldXY_2;
			parameters[9].Value = model.worldXY_3;
			parameters[10].Value = model.worldXY_4;
			parameters[11].Value = model.areaXY;
			parameters[12].Value = model.areaXY_1;
			parameters[13].Value = model.areaXY_2;
			parameters[14].Value = model.areaXY_3;
			parameters[15].Value = model.areaXY_4;
			parameters[16].Value = model.sign;
            parameters[17].Value = model.machineModel;
            parameters[18].Value = model.lvwangdate;
            parameters[19].Value = model.fireChl1;
            parameters[20].Value = model.fireChl2;
            parameters[21].Value = model.fireChl3;
            parameters[22].Value = model.fireChl4;
            parameters[23].Value = model.airChl1;
            parameters[24].Value = model.airChl2;
            parameters[25].Value = model.airChl3;
            parameters[26].Value = model.airChl4;

			parameters[27].Value = model.machineNo;

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
			strSql.Append("delete from tb_nodeInfo ");
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
			strSql.Append("delete from tb_nodeInfo ");
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
			strSql.Append("delete from tb_nodeInfo ");
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
		public GoDexData.Model.nodeInfo GetModel(int machineNo)
		{
			
			StringBuilder strSql=new StringBuilder();
            strSql.Append("select  top 1 ID,machineNo,machineType,softversion,doordog,runtime,worldMapPath,areaMapPath,worldXY,worldXY_1,worldXY_2,worldXY_3,worldXY_4,areaXY,areaXY_1,areaXY_2,areaXY_3,areaXY_4,sign,machineModel,lvwangdate,fireChl1,fireChl2,fireChl3,fireChl4,airChl1,airChl2,airChl3,airChl4 from tb_nodeInfo ");
			strSql.Append(" where machineNo=@machineNo");
			SqlParameter[] parameters = {
					new SqlParameter("@machineNo", SqlDbType.Int)
};
			parameters[0].Value = machineNo;

			GoDexData.Model.nodeInfo model=new GoDexData.Model.nodeInfo();
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
					model.machineType=int.Parse(ds.Tables[0].Rows[0]["machineType"].ToString());
				}
				if(ds.Tables[0].Rows[0]["softversion"]!=null && ds.Tables[0].Rows[0]["softversion"].ToString()!="")
				{
					model.softversion=ds.Tables[0].Rows[0]["softversion"].ToString();
				}
				if(ds.Tables[0].Rows[0]["doordog"]!=null && ds.Tables[0].Rows[0]["doordog"].ToString()!="")
				{
					model.doordog=int.Parse(ds.Tables[0].Rows[0]["doordog"].ToString());
				}
				if(ds.Tables[0].Rows[0]["runtime"]!=null && ds.Tables[0].Rows[0]["runtime"].ToString()!="")
				{
					model.runtime=ds.Tables[0].Rows[0]["runtime"].ToString();
				}
				if(ds.Tables[0].Rows[0]["worldMapPath"]!=null && ds.Tables[0].Rows[0]["worldMapPath"].ToString()!="")
				{
					model.worldMapPath=ds.Tables[0].Rows[0]["worldMapPath"].ToString();
				}
				if(ds.Tables[0].Rows[0]["areaMapPath"]!=null && ds.Tables[0].Rows[0]["areaMapPath"].ToString()!="")
				{
					model.areaMapPath=ds.Tables[0].Rows[0]["areaMapPath"].ToString();
				}
				if(ds.Tables[0].Rows[0]["worldXY"]!=null && ds.Tables[0].Rows[0]["worldXY"].ToString()!="")
				{
					model.worldXY=ds.Tables[0].Rows[0]["worldXY"].ToString();
				}
				if(ds.Tables[0].Rows[0]["worldXY_1"]!=null && ds.Tables[0].Rows[0]["worldXY_1"].ToString()!="")
				{
					model.worldXY_1=ds.Tables[0].Rows[0]["worldXY_1"].ToString();
				}
				if(ds.Tables[0].Rows[0]["worldXY_2"]!=null && ds.Tables[0].Rows[0]["worldXY_2"].ToString()!="")
				{
					model.worldXY_2=ds.Tables[0].Rows[0]["worldXY_2"].ToString();
				}
				if(ds.Tables[0].Rows[0]["worldXY_3"]!=null && ds.Tables[0].Rows[0]["worldXY_3"].ToString()!="")
				{
					model.worldXY_3=ds.Tables[0].Rows[0]["worldXY_3"].ToString();
				}
				if(ds.Tables[0].Rows[0]["worldXY_4"]!=null && ds.Tables[0].Rows[0]["worldXY_4"].ToString()!="")
				{
					model.worldXY_4=ds.Tables[0].Rows[0]["worldXY_4"].ToString();
				}
				if(ds.Tables[0].Rows[0]["areaXY"]!=null && ds.Tables[0].Rows[0]["areaXY"].ToString()!="")
				{
					model.areaXY=ds.Tables[0].Rows[0]["areaXY"].ToString();
				}
				if(ds.Tables[0].Rows[0]["areaXY_1"]!=null && ds.Tables[0].Rows[0]["areaXY_1"].ToString()!="")
				{
					model.areaXY_1=ds.Tables[0].Rows[0]["areaXY_1"].ToString();
				}
				if(ds.Tables[0].Rows[0]["areaXY_2"]!=null && ds.Tables[0].Rows[0]["areaXY_2"].ToString()!="")
				{
					model.areaXY_2=ds.Tables[0].Rows[0]["areaXY_2"].ToString();
				}
				if(ds.Tables[0].Rows[0]["areaXY_3"]!=null && ds.Tables[0].Rows[0]["areaXY_3"].ToString()!="")
				{
					model.areaXY_3=ds.Tables[0].Rows[0]["areaXY_3"].ToString();
				}
				if(ds.Tables[0].Rows[0]["areaXY_4"]!=null && ds.Tables[0].Rows[0]["areaXY_4"].ToString()!="")
				{
					model.areaXY_4=ds.Tables[0].Rows[0]["areaXY_4"].ToString();
				}
				if(ds.Tables[0].Rows[0]["sign"]!=null && ds.Tables[0].Rows[0]["sign"].ToString()!="")
				{
					model.sign=ds.Tables[0].Rows[0]["sign"].ToString();
				}
                if (ds.Tables[0].Rows[0]["machineModel"] != null && ds.Tables[0].Rows[0]["machineModel"].ToString() != "")
                {
                    model.machineModel = ds.Tables[0].Rows[0]["machineModel"].ToString();
                }
                if (ds.Tables[0].Rows[0]["lvwangdate"] != null && ds.Tables[0].Rows[0]["lvwangdate"].ToString() != "")
                {
                    model.lvwangdate = Convert.ToDateTime( ds.Tables[0].Rows[0]["lvwangdate"].ToString());
                }

                if (ds.Tables[0].Rows[0]["fireChl1"] != null && ds.Tables[0].Rows[0]["fireChl1"].ToString() != "")
                {
                    model.fireChl1 = int.Parse(ds.Tables[0].Rows[0]["fireChl1"].ToString());
                }
                if (ds.Tables[0].Rows[0]["fireChl2"] != null && ds.Tables[0].Rows[0]["fireChl2"].ToString() != "")
                {
                    model.fireChl2 = int.Parse(ds.Tables[0].Rows[0]["fireChl2"].ToString());
                }
                if (ds.Tables[0].Rows[0]["fireChl3"] != null && ds.Tables[0].Rows[0]["fireChl3"].ToString() != "")
                {
                    model.fireChl3 = int.Parse(ds.Tables[0].Rows[0]["fireChl3"].ToString());
                }
                if (ds.Tables[0].Rows[0]["fireChl4"] != null && ds.Tables[0].Rows[0]["fireChl4"].ToString() != "")
                {
                    model.fireChl4 = int.Parse(ds.Tables[0].Rows[0]["fireChl4"].ToString());
                }

                if (ds.Tables[0].Rows[0]["airChl1"] != null && ds.Tables[0].Rows[0]["airChl1"].ToString() != "")
                {
                    model.airChl1 = int.Parse(ds.Tables[0].Rows[0]["airChl1"].ToString());
                }
                if (ds.Tables[0].Rows[0]["airChl2"] != null && ds.Tables[0].Rows[0]["airChl2"].ToString() != "")
                {
                    model.airChl2 = int.Parse(ds.Tables[0].Rows[0]["airChl2"].ToString());
                }
                if (ds.Tables[0].Rows[0]["airChl3"] != null && ds.Tables[0].Rows[0]["airChl3"].ToString() != "")
                {
                    model.airChl3 = int.Parse(ds.Tables[0].Rows[0]["airChl3"].ToString());
                }
                if (ds.Tables[0].Rows[0]["airChl4"] != null && ds.Tables[0].Rows[0]["airChl4"].ToString() != "")
                {
                    model.airChl4 = int.Parse(ds.Tables[0].Rows[0]["airChl4"].ToString());
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
            strSql.Append("select ID,machineNo,machineType,softversion,doordog,runtime,worldMapPath,areaMapPath,worldXY,worldXY_1,worldXY_2,worldXY_3,worldXY_4,areaXY,areaXY_1,areaXY_2,areaXY_3,areaXY_4,sign,machineModel,lvwangdate,fireChl1,fireChl2,fireChl3,fireChl4,airChl1,airChl2,airChl3,airChl4 ");
			strSql.Append(" FROM tb_nodeInfo");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
            strSql.Append(" order by machineNo ");
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
            strSql.Append(" ID,machineNo,machineType,softversion,doordog,runtime,worldMapPath,areaMapPath,worldXY,worldXY_1,worldXY_2,worldXY_3,worldXY_4,areaXY,areaXY_1,areaXY_2,areaXY_3,areaXY_4,sign,machineModel,lvwangdate,fireChl1,fireChl2,fireChl3,fireChl4,airChl1,airChl2,airChl3,airChl4 ");
			strSql.Append(" FROM tb_nodeInfo ");
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
			parameters[0].Value = "tb_nodeInfo";
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

