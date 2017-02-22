using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
namespace Maticsoft.Common
{
	public class StringPlus
	{
		public static List<string> GetStrArray(string str, char speater, bool toLower)
		{
			List<string> list = new List<string>();
			string[] array = str.Split(new char[]
			{
				speater
			});
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text = array2[i];
				if (!string.IsNullOrEmpty(text) && text != speater.ToString())
				{
					string item = text;
					if (toLower)
					{
						item = text.ToLower();
					}
					list.Add(item);
				}
			}
			return list;
		}
		public static string[] GetStrArray(string str)
		{
			return str.Split(new char[]
			{
				','
			});
		}
		public static string GetArrayStr(List<string> list, string speater)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < list.Count; i++)
			{
				if (i == list.Count - 1)
				{
					stringBuilder.Append(list[i]);
				}
				else
				{
					stringBuilder.Append(list[i]);
					stringBuilder.Append(speater);
				}
			}
			return stringBuilder.ToString();
		}
		public static string GetArrayStr(List<int> list)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < list.Count; i++)
			{
				if (i == list.Count - 1)
				{
					stringBuilder.Append(list[i].ToString());
				}
				else
				{
					stringBuilder.Append(list[i]);
					stringBuilder.Append(",");
				}
			}
			return stringBuilder.ToString();
		}
		public static string GetArrayValueStr(Dictionary<int, int> list)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<int, int> current in list)
			{
				stringBuilder.Append(current.Value + ",");
			}
			if (list.Count > 0)
			{
				return StringPlus.DelLastComma(stringBuilder.ToString());
			}
			return "";
		}
		public static string DelLastComma(string str)
		{
			return str.Substring(0, str.LastIndexOf(","));
		}
		public static string DelLastChar(string str, string strchar)
		{
			return str.Substring(0, str.LastIndexOf(strchar));
		}
		public static string ToSBC(string input)
		{
			char[] array = input.ToCharArray();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == ' ')
				{
					array[i] = '\u3000';
				}
				else
				{
					if (array[i] < '\u007f')
					{
						array[i] += 'ﻠ';
					}
				}
			}
			return new string(array);
		}
		public static string ToDBC(string input)
		{
			char[] array = input.ToCharArray();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == '\u3000')
				{
					array[i] = ' ';
				}
				else
				{
					if (array[i] > '＀' && array[i] < '｟')
					{
						array[i] -= 'ﻠ';
					}
				}
			}
			return new string(array);
		}
		public static List<string> GetSubStringList(string o_str, char sepeater)
		{
			List<string> list = new List<string>();
			string[] array = o_str.Split(new char[]
			{
				sepeater
			});
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text = array2[i];
				if (!string.IsNullOrEmpty(text) && text != sepeater.ToString())
				{
					list.Add(text);
				}
			}
			return list;
		}
		public static string GetCleanStyle(string StrList, string SplitString)
		{
			string result;
			if (StrList == null)
			{
				result = "";
			}
			else
			{
				string text = StrList.Replace(SplitString, "");
				result = text;
			}
			return result;
		}
		public static string GetNewStyle(string StrList, string NewStyle, string SplitString, out string Error)
		{
			string result;
			if (StrList == null)
			{
				result = "";
				Error = "请输入需要划分格式的字符串";
			}
			else
			{
				int length = StrList.Length;
				int length2 = StringPlus.GetCleanStyle(NewStyle, SplitString).Length;
				if (length != length2)
				{
					result = "";
					Error = "样式格式的长度与输入的字符长度不符，请重新输入";
				}
				else
				{
					string text = "";
					for (int i = 0; i < NewStyle.Length; i++)
					{
						if (NewStyle.Substring(i, 1) == SplitString)
						{
							text = text + "," + i;
						}
					}
					if (text != "")
					{
						text = text.Substring(1);
					}
					string[] array = text.Split(new char[]
					{
						','
					});
					string[] array2 = array;
					for (int j = 0; j < array2.Length; j++)
					{
						string s = array2[j];
						StrList = StrList.Insert(int.Parse(s), SplitString);
					}
					result = StrList;
					Error = "";
				}
			}
			return result;
		}
		public static string[] SplitMulti(string str, string splitstr)
		{
			string[] result = null;
			if (str != null && str != "")
			{
				result = new Regex(splitstr).Split(str);
			}
			return result;
		}
		public static string SqlSafeString(string String, bool IsDel)
		{
			if (IsDel)
			{
				String = String.Replace("'", "");
				String = String.Replace("\"", "");
				return String;
			}
			String = String.Replace("'", "&#39;");
			String = String.Replace("\"", "&#34;");
			return String;
		}
	}
}
