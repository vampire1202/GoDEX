using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Maticsoft.DBUtility;//Please add references
namespace GoDexData.DAL
{
	/// <summary>
	/// 数据访问类:areaMap
	/// </summary>
	public partial class areaMap
	{
		public areaMap()
		{}
		#region  Method

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("areaMapNo", "tb_areaMap"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int areaMapNo)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_areaMap");
			strSql.Append(" where areaMapNo=@areaMapNo ");
			SqlParameter[] parameters = {
					new SqlParameter("@areaMapNo", SqlDbType.Int,4)};
			parameters[0].Value = areaMapNo;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(GoDexData.Model.areaMap model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_areaMap(");
			strSql.Append("areaMapNo,areaMapPath)");
			strSql.Append(" values (");
			strSql.Append("@areaMapNo,@areaMapPath)");
			SqlParameter[] parameters = {
					new SqlParameter("@areaMapNo", SqlDbType.Int,4),
					new SqlParameter("@areaMapPath", SqlDbType.VarChar,500)};
			parameters[0].Value = model.areaMapNo;
			parameters[1].Value = model.areaMapPath;

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
		public bool Update(GoDexData.Model.areaMap model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_areaMap set "); 
			strSql.Append("areaMapPath=@areaMapPath");
			strSql.Append(" where areaMapNo=@areaMapNo ");
			SqlParameter[] parameters = { 
					new SqlParameter("@areaMapPath", SqlDbType.VarChar,500),
					new SqlParameter("@areaMapNo", SqlDbType.Int,4)}; 
			parameters[0].Value = model.areaMapPath;
			parameters[1].Value = model.areaMapNo;

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
		public bool Delete(int areaMapNo)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_areaMap ");
			strSql.Append(" where areaMapNo=@areaMapNo ");
			SqlParameter[] parameters = {
					new SqlParameter("@areaMapNo", SqlDbType.Int,4)};
			parameters[0].Value = areaMapNo;

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
		public bool DeleteList(string areaMapNolist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_areaMap ");
			strSql.Append(" where areaMapNo in ("+areaMapNolist + ")  ");
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
		public GoDexData.Model.areaMap GetModel(int areaMapNo)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,areaMapNo,areaMapPath from tb_areaMap ");
			strSql.Append(" where areaMapNo=@areaMapNo ");
			SqlParameter[] parameters = {
					new SqlParameter("@areaMapNo", SqlDbType.Int,4)};
			parameters[0].Value = areaMapNo;

			GoDexData.Model.areaMap model=new GoDexData.Model.areaMap();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["ID"]!=null && ds.Tables[0].Rows[0]["ID"].ToString()!="")
				{
					model.ID=long.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
				}
				if(ds.Tables[0].Rows[0]["areaMapNo"]!=null && ds.Tables[0].Rows[0]["areaMapNo"].ToString()!="")
				{
					model.areaMapNo=int.Parse(ds.Tables[0].Rows[0]["areaMapNo"].ToString());
				}
				if(ds.Tables[0].Rows[0]["areaMapPath"]!=null && ds.Tables[0].Rows[0]["areaMapPath"].ToString()!="")
				{
					model.areaMapPath=ds.Tables[0].Rows[0]["areaMapPath"].ToString();
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
			strSql.Append("select ID,areaMapNo,areaMapPath ");
			strSql.Append(" FROM tb_areaMap ");
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
			strSql.Append(" ID,areaMapNo,areaMapPath ");
			strSql.Append(" FROM tb_areaMap ");
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
			parameters[0].Value = "tb_areaMap";
			parameters[1].Value = "areaMapNo";
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

