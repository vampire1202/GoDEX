using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Maticsoft.DBUtility;//Please add references
namespace GoDexData.DAL
{
	/// <summary>
	/// 数据访问类:log
	/// </summary>
	public partial class log
	{
		public log()
		{}
		#region  Method

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(long ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_log");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.BigInt,8)};
			parameters[0].Value = ID;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(GoDexData.Model.log model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_log(");
			strSql.Append("machineNo,action,userName,acDateTime,sign)");
			strSql.Append(" values (");
			strSql.Append("@machineNo,@action,@userName,@acDateTime,@sign)");
			SqlParameter[] parameters = { 
					new SqlParameter("@machineNo", SqlDbType.Int,4),
					new SqlParameter("@action", SqlDbType.VarChar,500),
					new SqlParameter("@userName", SqlDbType.VarChar,100),
					new SqlParameter("@acDateTime", SqlDbType.DateTime),
					new SqlParameter("@sign", SqlDbType.VarChar,200)};
			//parameters[0].Value = model.ID;
			parameters[0].Value = model.machineNo;
			parameters[1].Value = model.action;
			parameters[2].Value = model.userName;
			parameters[3].Value = model.acDateTime;
			parameters[4].Value = model.sign;

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
		public bool Update(GoDexData.Model.log model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_log set ");
			strSql.Append("machineNo=@machineNo,");
			strSql.Append("action=@action,");
			strSql.Append("userName=@userName,");
			strSql.Append("acDateTime=@acDateTime,");
			strSql.Append("sign=@sign");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@machineNo", SqlDbType.Int,4),
					new SqlParameter("@action", SqlDbType.VarChar,500),
					new SqlParameter("@userName", SqlDbType.VarChar,100),
					new SqlParameter("@acDateTime", SqlDbType.DateTime),
					new SqlParameter("@sign", SqlDbType.VarChar,200),
					new SqlParameter("@ID", SqlDbType.BigInt,8)};
			parameters[0].Value = model.machineNo;
			parameters[1].Value = model.action;
			parameters[2].Value = model.userName;
			parameters[3].Value = model.acDateTime;
			parameters[4].Value = model.sign;
			parameters[5].Value = model.ID;

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
			strSql.Append("delete from tb_log ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.BigInt,8)};
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
			strSql.Append("delete from tb_log ");
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
		public GoDexData.Model.log GetModel(long ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,machineNo,action,userName,acDateTime,sign from tb_log ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.BigInt,8)};
			parameters[0].Value = ID;

			GoDexData.Model.log model=new GoDexData.Model.log();
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
				if(ds.Tables[0].Rows[0]["action"]!=null && ds.Tables[0].Rows[0]["action"].ToString()!="")
				{
					model.action=ds.Tables[0].Rows[0]["action"].ToString();
				}
				if(ds.Tables[0].Rows[0]["userName"]!=null && ds.Tables[0].Rows[0]["userName"].ToString()!="")
				{
					model.userName=ds.Tables[0].Rows[0]["userName"].ToString();
				}
				if(ds.Tables[0].Rows[0]["acDateTime"]!=null && ds.Tables[0].Rows[0]["acDateTime"].ToString()!="")
				{
					model.acDateTime=DateTime.Parse(ds.Tables[0].Rows[0]["acDateTime"].ToString());
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
            strSql.Append("select  machineNo as 节点编号,action as 操作内容,userName as 操作用户,CONVERT(varchar(100),acDateTime, 20) as 时间 ");
			strSql.Append(" FROM tb_log ");
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
			strSql.Append(" ID,machineNo,action,userName,acDateTime,sign ");
			strSql.Append(" FROM tb_log ");
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
			parameters[0].Value = "tb_log";
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

