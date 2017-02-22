using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Maticsoft.DBUtility;//Please add references
namespace GoDexData.DAL
{
	/// <summary>
	/// 数据访问类:worldMap
	/// </summary>
	public partial class worldMap
	{
		public worldMap()
		{}
		#region  Method



		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(GoDexData.Model.worldMap model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_worldMap(");
			strSql.Append("worldMapNo,worldMapPath)");
			strSql.Append(" values (");
			strSql.Append("@worldMapNo,@worldMapPath)");
			SqlParameter[] parameters = {
					new SqlParameter("@worldMapNo", SqlDbType.Int,4),
					new SqlParameter("@worldMapPath", SqlDbType.VarChar,500)};
			parameters[0].Value = model.worldMapNo;
			parameters[1].Value = model.worldMapPath;

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
		public bool Update(GoDexData.Model.worldMap model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_worldMap set "); 
			strSql.Append("worldMapPath=@worldMapPath");
			strSql.Append(" where ");
            strSql.Append("worldMapNo=@worldMapNo");
			SqlParameter[] parameters = { 
					new SqlParameter("@worldMapNo", SqlDbType.Int,4),
					new SqlParameter("@worldMapPath", SqlDbType.VarChar,500)}; 
			parameters[0].Value = model.worldMapNo;
			parameters[1].Value = model.worldMapPath;

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
			strSql.Append("delete from tb_worldMap ");
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
		public GoDexData.Model.worldMap GetModel(int worldMapNo)
		{
			//该表无主键信息，请自定义主键/条件字段
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select top 1 ID,worldMapNo,worldMapPath from tb_worldMap ");
			strSql.Append(" where ");
            strSql.Append(" worldMapNo=@worldMapNo ");
            SqlParameter[] parameters = {new SqlParameter("@worldMapNo", SqlDbType.Int)
};
            parameters[0].Value = worldMapNo;

			GoDexData.Model.worldMap model=new GoDexData.Model.worldMap();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["ID"]!=null && ds.Tables[0].Rows[0]["ID"].ToString()!="")
				{
					model.ID=int.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
				}
				if(ds.Tables[0].Rows[0]["worldMapNo"]!=null && ds.Tables[0].Rows[0]["worldMapNo"].ToString()!="")
				{
					model.worldMapNo=int.Parse(ds.Tables[0].Rows[0]["worldMapNo"].ToString());
				}
				if(ds.Tables[0].Rows[0]["worldMapPath"]!=null && ds.Tables[0].Rows[0]["worldMapPath"].ToString()!="")
				{
					model.worldMapPath=ds.Tables[0].Rows[0]["worldMapPath"].ToString();
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
			strSql.Append("select ID,worldMapNo,worldMapPath ");
			strSql.Append(" FROM tb_worldMap ");
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
			strSql.Append(" ID,worldMapNo,worldMapPath ");
			strSql.Append(" FROM tb_worldMap ");
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
			parameters[0].Value = "tb_worldMap";
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

