using Maticsoft.DBUtility;
using System;
using System.Collections;
using System.Data.SqlClient;
namespace Maticsoft.Common
{
	public class Pagination
	{
		private static DbHelperSQLP sqlhelp = new DbHelperSQLP(PubConstant.GetConnectionString("ConnectionStringValueOptima"));
		public static string getPageParameterHidden(ArrayList al)
		{
			string text = "";
			for (int i = 0; i < al.Count; i++)
			{
				try
				{
					string[] array = (string[])al[i];
					string text2 = text;
					text = string.Concat(new string[]
					{
						text2,
						"<input type='hidden' name='",
						array[0],
						"' value='",
						array[1],
						"'>"
					});
				}
				catch
				{
				}
			}
			return text;
		}
		public static int PageCount(int ReCount, int PageSize)
		{
			if (ReCount % PageSize <= 0)
			{
				return ReCount / PageSize;
			}
			return ReCount / PageSize + 1;
		}
		public static string PageInfo(int IndexPage, int ReCount, int PageSize, string PageParameter, int Att)
		{
			return Pagination.PageInfo(IndexPage, ReCount, PageSize, PageParameter, Att, true);
		}
		public static string PageInfo(int IndexPage, int ReCount, int PageSize, string PageParameter, int Att, bool showbutton)
		{
			string result = "";
			if (Att == Config.si_CN)
			{
				return Pagination.PageInfo_CN(IndexPage, ReCount, PageSize, PageParameter, showbutton);
			}
			if (Att == Config.si_EN)
			{
				result = Pagination.PageInfo_EN(IndexPage, ReCount, PageSize, PageParameter, showbutton);
			}
			return result;
		}
		public static string PageInfo_CN(int IndexPage, int ReCount, int PageSize, string PageParameter)
		{
			return Pagination.PageInfo_CN(IndexPage, ReCount, PageSize, PageParameter, true);
		}
		public static string PageInfo_CN(int IndexPage, int ReCount, int PageSize, string PageParameter, bool showbutton)
		{
			int num = Pagination.PageCount(ReCount, PageSize);
			int num2 = 10;
			int num3 = 1;
			if (num >= num2 && IndexPage > 6)
			{
				num3 = IndexPage - 5;
				if (num >= num2 && num3 > num - (num2 - 1))
				{
					num3 = num - (num2 - 1);
				}
			}
			int num4 = num3 + (num2 - 1);
			if (num4 > num)
			{
				num4 = num;
			}
			string text = "<table border='0' cellpadding='0' style='border-collapse: collapse' width='100%' height='20'>";
			object obj = text + "<tr><td align='right'>";
			text = string.Concat(new object[]
			{
				obj,
				"<b>",
				ReCount,
				"</b>items&nbsp;Pagesize:<b>",
				PageSize,
				"</b>&nbsp;<b>",
				num,
				"</b>Page&nbsp;Page:<b>",
				IndexPage,
				"</b>/<b>",
				num,
				"</b>&nbsp;&nbsp;&nbsp;&nbsp;分页:"
			});
			if (IndexPage > 1)
			{
				object obj2 = text + "<a href='?Page=1&" + PageParameter + "' title=First><FONT face=webdings>9</FONT></a>";
				text = string.Concat(new object[]
				{
					obj2,
					"<a href='?Page=",
					IndexPage - 1,
					"&",
					PageParameter,
					"' title=Previous><FONT face=webdings>7</FONT></a>"
				});
			}
			for (int i = num3; i <= num4; i++)
			{
				if (i == IndexPage)
				{
					object obj3 = text;
					text = string.Concat(new object[]
					{
						obj3,
						" <b><font color='red'>",
						i,
						"</font></b> "
					});
				}
				else
				{
					object obj4 = text;
					text = string.Concat(new object[]
					{
						obj4,
						" <a href='?Page=",
						i,
						"&",
						PageParameter,
						"' title=Page",
						i,
						">",
						i,
						"</a> "
					});
				}
			}
			if (IndexPage < num)
			{
				object obj5 = text;
				object obj6 = string.Concat(new object[]
				{
					obj5,
					"<a href='?Page=",
					IndexPage + 1,
					"&",
					PageParameter,
					"' title=Next><FONT face=webdings>8</FONT></a>"
				});
				text = string.Concat(new object[]
				{
					obj6,
					"<a href='?Page=",
					num,
					"&",
					PageParameter,
					"' title=LastPage><FONT face=webdings>:</FONT></a>"
				});
			}
			text += "&nbsp;</td>";
			if (showbutton)
			{
				text = text + "<td><form style='line-height: 150%; margin-top: 0; margin-bottom: 0' name='Select_Page' action='?' mothed='get'><input type='text' name='Page' size='2'><input type='submit' value='GO' name='B1'>" + Pagination.getPageParameterHidden(Pagination.splitPageParameter(PageParameter)) + "</form></td>";
			}
			return text + "</tr></table>";
		}
		public static string PageInfo_EN(int IndexPage, int ReCount, int PageSize, string PageParameter)
		{
			return Pagination.PageInfo_EN(IndexPage, ReCount, PageSize, PageParameter, true);
		}
		public static string PageInfo_EN(int IndexPage, int ReCount, int PageSize, string PageParameter, bool showbutton)
		{
			int num = Pagination.PageCount(ReCount, PageSize);
			int num2 = 10;
			int num3 = 1;
			if (num >= num2 && IndexPage > 6)
			{
				num3 = IndexPage - 5;
				if (num >= num2 && num3 > num - (num2 - 1))
				{
					num3 = num - (num2 - 1);
				}
			}
			int num4 = num3 + (num2 - 1);
			if (num4 > num)
			{
				num4 = num;
			}
			string text = "<table border='0' cellpadding='0' style='border-collapse: collapse' width='100%' height='20'>";
			object obj = text + "<tr><td align='right'>";
			text = string.Concat(new object[]
			{
				obj,
				"",
				ReCount,
				" records&nbsp;",
				PageSize,
				"/page pages&nbsp;<b>",
				IndexPage,
				"</b>/<b>",
				num,
				"</b>&nbsp;&nbsp;&nbsp;&nbsp;page:"
			});
			if (IndexPage > 1)
			{
				object obj2 = text + "<a href='?Page=1&" + PageParameter + "' title='Page 1'><FONT face=webdings>9</FONT></a>";
				text = string.Concat(new object[]
				{
					obj2,
					"<a href='?Page=",
					IndexPage - 1,
					"&",
					PageParameter,
					"' title='Prev'><FONT face=webdings>7</FONT></a>"
				});
			}
			for (int i = num3; i <= num4; i++)
			{
				if (i == IndexPage)
				{
					object obj3 = text;
					text = string.Concat(new object[]
					{
						obj3,
						" <b><font color='red'>",
						i,
						"</font></b> "
					});
				}
				else
				{
					object obj4 = text;
					text = string.Concat(new object[]
					{
						obj4,
						" <a href='?Page=",
						i,
						"&",
						PageParameter,
						"' title='Page ",
						i,
						"'>",
						i,
						"</a> "
					});
				}
			}
			if (IndexPage < num)
			{
				object obj5 = text;
				object obj6 = string.Concat(new object[]
				{
					obj5,
					"<a href='?Page=",
					IndexPage + 1,
					"&",
					PageParameter,
					"' title='Next'><FONT face=webdings>8</FONT></a>"
				});
				text = string.Concat(new object[]
				{
					obj6,
					"<a href='?Page=",
					num,
					"&",
					PageParameter,
					"' title='Page ",
					num,
					"'><FONT face=webdings>:</FONT></a>"
				});
			}
			text += "&nbsp;</td>";
			if (showbutton)
			{
				text = text + "<td><form style='line-height: 150%; margin-top: 0; margin-bottom: 0' name='Select_Page' action='?' mothed='get'><input type='text' name='Page' size='2'><input type='submit' value='Go' name='B1'>" + Pagination.getPageParameterHidden(Pagination.splitPageParameter(PageParameter)) + "</form></td>";
			}
			return text + "</tr></table>";
		}
		public static int ReCordCount(SqlCommand cmd)
		{
			string text = "select count(*) as total " + cmd.CommandText;
			return Convert.ToInt32(Pagination.sqlhelp.Query(text).Tables[0].Rows[0]["total"]);
		}
		public static int ReCordCount(string sql)
		{
			sql = "select count(*) as total " + sql;
			return Convert.ToInt32(Pagination.sqlhelp.Query(sql).Tables[0].Rows[0]["total"]);
		}
		public static ArrayList splitPageParameter(string PageParameter)
		{
			ArrayList arrayList = new ArrayList();
			if (PageParameter != null && PageParameter != "")
			{
				string[] array = StringPlus.SplitMulti(PageParameter, "&");
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] != null && array[i] != "")
					{
						string[] array2 = StringPlus.SplitMulti(array[i], "=");
						if (array2 != null && array2.Length >= 2)
						{
							arrayList.Add(array2);
						}
					}
				}
			}
			return arrayList;
		}
	}
}
