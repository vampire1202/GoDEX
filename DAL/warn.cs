using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Maticsoft.DBUtility;//Please add references
namespace GoDexData.DAL
{
	/// <summary>
	/// 数据访问类:warn
	/// </summary>
	public partial class warn
	{
		public warn()
		{}
		#region  Method



		/// <summary>
		/// 增加一条数据
		/// </summary>
		public long Add(GoDexData.Model.warn model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_warn(");
			strSql.Append("machineNo,warnLeval,warnDiscrib,userName,warnDateTime,sign)");
			strSql.Append(" values (");
			strSql.Append("@machineNo,@warnLeval,@warnDiscrib,@userName,@warnDateTime,@sign)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@machineNo", SqlDbType.Int,4),
					new SqlParameter("@warnLeval", SqlDbType.VarChar,50),
					new SqlParameter("@warnDiscrib", SqlDbType.VarChar,500),
					new SqlParameter("@userName", SqlDbType.VarChar,50),
					new SqlParameter("@warnDateTime", SqlDbType.DateTime),
					new SqlParameter("@sign", SqlDbType.NChar,10)};
			parameters[0].Value = model.machineNo;
			parameters[1].Value = model.warnLeval;
			parameters[2].Value = model.warnDiscrib;
			parameters[3].Value = model.userName;
			parameters[4].Value = model.warnDateTime;
			parameters[5].Value = model.sign;

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
		public bool Update(GoDexData.Model.warn model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_warn set ");
			strSql.Append("machineNo=@machineNo,");
			strSql.Append("warnLeval=@warnLeval,");
			strSql.Append("warnDiscrib=@warnDiscrib,");
			strSql.Append("userName=@userName,");
			strSql.Append("warnDateTime=@warnDateTime,");
			strSql.Append("sign=@sign");
			strSql.Append(" where ID=@ID");
			SqlParameter[] parameters = {
					new SqlParameter("@machineNo", SqlDbType.Int,4),
					new SqlParameter("@warnLeval", SqlDbType.VarChar,50),
					new SqlParameter("@warnDiscrib", SqlDbType.VarChar,500),
					new SqlParameter("@userName", SqlDbType.VarChar,50),
					new SqlParameter("@warnDateTime", SqlDbType.DateTime),
					new SqlParameter("@sign", SqlDbType.NChar,10),
					new SqlParameter("@ID", SqlDbType.BigInt,8)};
			parameters[0].Value = model.machineNo;
			parameters[1].Value = model.warnLeval;
			parameters[2].Value = model.warnDiscrib;
			parameters[3].Value = model.userName;
			parameters[4].Value = model.warnDateTime;
			parameters[5].Value = model.sign;
			parameters[6].Value = model.ID;

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
			strSql.Append("delete from tb_warn ");
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
		/// 批量删除数据
		/// </summary>
		public bool DeleteList(string IDlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_warn ");
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
		public GoDexData.Model.warn GetModel(long ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,machineNo,warnLeval,warnDiscrib,userName,warnDateTime,sign from tb_warn ");
			strSql.Append(" where ID=@ID");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.BigInt)
};
			parameters[0].Value = ID;

			GoDexData.Model.warn model=new GoDexData.Model.warn();
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
				if(ds.Tables[0].Rows[0]["warnLeval"]!=null && ds.Tables[0].Rows[0]["warnLeval"].ToString()!="")
				{
					model.warnLeval=ds.Tables[0].Rows[0]["warnLeval"].ToString();
				}
				if(ds.Tables[0].Rows[0]["warnDiscrib"]!=null && ds.Tables[0].Rows[0]["warnDiscrib"].ToString()!="")
				{
					model.warnDiscrib=ds.Tables[0].Rows[0]["warnDiscrib"].ToString();
				}
				if(ds.Tables[0].Rows[0]["userName"]!=null && ds.Tables[0].Rows[0]["userName"].ToString()!="")
				{
					model.userName=ds.Tables[0].Rows[0]["userName"].ToString();
				}
				if(ds.Tables[0].Rows[0]["warnDateTime"]!=null && ds.Tables[0].Rows[0]["warnDateTime"].ToString()!="")
				{
					model.warnDateTime=DateTime.Parse(ds.Tables[0].Rows[0]["warnDateTime"].ToString());
				}
				if(ds.Tables[0].Rows[0]["sign"]!=null && ds.Tables[0].Rows[0]["sign"].ToString()!="")
				{
					model.sign=ds.Tables[0].Rows[0]["sign"].ToString();
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
			strSql.Append("select ID,machineNo,warnLeval,warnDiscrib,userName,warnDateTime,sign ");
			strSql.Append(" FROM tb_warn ");
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
			strSql.Append(" ID,machineNo,warnLeval,warnDiscrib,userName,warnDateTime,sign ");
			strSql.Append(" FROM tb_warn ");
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
			parameters[0].Value = "tb_warn";
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

