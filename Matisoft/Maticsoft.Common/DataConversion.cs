using System;
using System.Data;
using System.Data.OleDb;
using System.Text;
namespace Maticsoft.Common
{
	public class DataConversion
	{
		private const string ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=Excel 8.0;";
		public string DataTableToExcel(DataTable dt, string excelPath)
		{
			if (dt == null)
			{
				return "DataTable不能为空";
			}
			dt.TableName = "Sheet1";
			int count = dt.Rows.Count;
			int count2 = dt.Columns.Count;
			if (count == 0)
			{
				return "没有数据";
			}
			StringBuilder stringBuilder = new StringBuilder();
			string connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=Excel 8.0;", excelPath);
			stringBuilder.Append("CREATE TABLE ");
			stringBuilder.Append(dt.TableName + " ( ");
			for (int i = 0; i < count2; i++)
			{
				if (i < count2 - 1)
				{
					stringBuilder.Append(string.Format("{0} nvarchar,", dt.Columns[i].ColumnName));
				}
				else
				{
					stringBuilder.Append(string.Format("{0} nvarchar)", dt.Columns[i].ColumnName));
				}
			}
			string result;
			using (OleDbConnection oleDbConnection = new OleDbConnection(connectionString))
			{
				OleDbCommand oleDbCommand = new OleDbCommand();
				oleDbCommand.Connection = oleDbConnection;
				oleDbCommand.CommandText = stringBuilder.ToString();
				try
				{
					oleDbConnection.Open();
					oleDbCommand.ExecuteNonQuery();
				}
				catch (Exception ex)
				{
					result = "在Excel中创建表失败，错误信息：" + ex.Message;
					return result;
				}
				stringBuilder.Remove(0, stringBuilder.Length);
				stringBuilder.Append("INSERT INTO ");
				stringBuilder.Append(dt.TableName + " ( ");
				for (int j = 0; j < count2; j++)
				{
					if (j < count2 - 1)
					{
						stringBuilder.Append(dt.Columns[j].ColumnName + ",");
					}
					else
					{
						stringBuilder.Append(dt.Columns[j].ColumnName + ") values (");
					}
				}
				for (int k = 0; k < count2; k++)
				{
					if (k < count2 - 1)
					{
						stringBuilder.Append("@" + dt.Columns[k].ColumnName + ",");
					}
					else
					{
						stringBuilder.Append("@" + dt.Columns[k].ColumnName + ")");
					}
				}
				oleDbCommand.CommandText = stringBuilder.ToString();
				OleDbParameterCollection parameters = oleDbCommand.Parameters;
				for (int l = 0; l < count2; l++)
				{
					parameters.Add(new OleDbParameter("@" + dt.Columns[l].ColumnName, OleDbType.VarChar));
				}
				foreach (DataRow dataRow in dt.Rows)
				{
					for (int m = 0; m < parameters.Count; m++)
					{
						parameters[m].Value = dataRow[m];
					}
					oleDbCommand.ExecuteNonQuery();
				}
				result = "数据已成功导入Excel";
			}
			return result;
		}
		public DataSet ExcelToDS(string Path)
		{
			string text = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Path + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\";";
			OleDbConnection oleDbConnection = new OleDbConnection(text);
			oleDbConnection.Open();
			string selectCommandText = "select * from [Sheet1$]";
			OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(selectCommandText, text);
			DataSet dataSet = new DataSet();
			oleDbDataAdapter.Fill(dataSet, "table1");
			oleDbConnection.Close();
			return dataSet;
		}
	}
}
