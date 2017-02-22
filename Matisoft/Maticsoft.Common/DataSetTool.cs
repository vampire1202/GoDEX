using System;
using System.Data;
namespace Maticsoft.Common
{
	public class DataSetTool
	{
		public static DataRowCollection GetDataSetRows(DataSet ds)
		{
			if (ds.Tables.Count > 0)
			{
				return ds.Tables[0].Rows;
			}
			return null;
		}
	}
}
