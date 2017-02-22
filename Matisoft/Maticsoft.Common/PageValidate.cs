using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;
namespace Maticsoft.Common
{
	public class PageValidate
	{
		private static Regex RegPhone = new Regex("^[0-9]+[-]?[0-9]+[-]?[0-9]$");
		private static Regex RegNumber = new Regex("^[0-9]+$");
		private static Regex RegNumberSign = new Regex("^[+-]?[0-9]+$");
		private static Regex RegDecimal = new Regex("^[0-9]+[.]?[0-9]+$");
		private static Regex RegDecimalSign = new Regex("^[+-]?[0-9]+[.]?[0-9]+$");
		private static Regex RegEmail = new Regex("^[\\w-]+@[\\w-]+\\.(com|net|cn|org|edu|mil|tv|biz|info)$");
		private static Regex RegCHZN = new Regex("[一-龥]");
		public static bool IsPhone(string inputData)
		{
			Match match = PageValidate.RegPhone.Match(inputData);
			return match.Success;
		}
		public static string FetchInputDigit(HttpRequest req, string inputKey, int maxLen)
		{
			string text = string.Empty;
			if (inputKey != null && inputKey != string.Empty)
			{
				text = req.QueryString[inputKey];
				if (text == null)
				{
					text = req.Form[inputKey];
				}
				if (text != null)
				{
					text = PageValidate.SqlText(text, maxLen);
					if (!PageValidate.IsNumber(text))
					{
						text = string.Empty;
					}
				}
			}
			if (text == null)
			{
				text = string.Empty;
			}
			return text;
		}
		public static bool IsNumber(string inputData)
		{
			Match match = PageValidate.RegNumber.Match(inputData);
			return match.Success;
		}
		public static bool IsNumberSign(string inputData)
		{
			Match match = PageValidate.RegNumberSign.Match(inputData);
			return match.Success;
		}
		public static bool IsDecimal(string inputData)
		{
			Match match = PageValidate.RegDecimal.Match(inputData);
			return match.Success;
		}
		public static bool IsDecimalSign(string inputData)
		{
			Match match = PageValidate.RegDecimalSign.Match(inputData);
			return match.Success;
		}
		public static bool IsHasCHZN(string inputData)
		{
			Match match = PageValidate.RegCHZN.Match(inputData);
			return match.Success;
		}
		public static bool IsEmail(string inputData)
		{
			Match match = PageValidate.RegEmail.Match(inputData);
			return match.Success;
		}
		public static bool IsDateTime(string str)
		{
			bool result;
			try
			{
				if (!string.IsNullOrEmpty(str))
				{
					DateTime.Parse(str);
					result = true;
				}
				else
				{
					result = false;
				}
			}
			catch
			{
				result = false;
			}
			return result;
		}
		public static bool CheckIDCard(string Id)
		{
			if (Id.Length == 18)
			{
				return PageValidate.CheckIDCard18(Id);
			}
			return Id.Length == 15 && PageValidate.CheckIDCard15(Id);
		}
		private static bool CheckIDCard18(string Id)
		{
			long num = 0L;
			if (!long.TryParse(Id.Remove(17), out num) || (double)num < Math.Pow(10.0, 16.0) || !long.TryParse(Id.Replace('x', '0').Replace('X', '0'), out num))
			{
				return false;
			}
			string text = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
			if (text.IndexOf(Id.Remove(2)) == -1)
			{
				return false;
			}
			string s = Id.Substring(6, 8).Insert(6, "-").Insert(4, "-");
			DateTime dateTime = default(DateTime);
			if (!DateTime.TryParse(s, out dateTime))
			{
				return false;
			}
			string[] array = "1,0,x,9,8,7,6,5,4,3,2".Split(new char[]
			{
				','
			});
			string[] array2 = "7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2".Split(new char[]
			{
				','
			});
			char[] array3 = Id.Remove(17).ToCharArray();
			int num2 = 0;
			for (int i = 0; i < 17; i++)
			{
				num2 += int.Parse(array2[i]) * int.Parse(array3[i].ToString());
			}
			int num3 = -1;
			Math.DivRem(num2, 11, out num3);
			return !(array[num3] != Id.Substring(17, 1).ToLower());
		}
		private static bool CheckIDCard15(string Id)
		{
			long num = 0L;
			if (!long.TryParse(Id, out num) || (double)num < Math.Pow(10.0, 14.0))
			{
				return false;
			}
			string text = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
			if (text.IndexOf(Id.Remove(2)) == -1)
			{
				return false;
			}
			string s = Id.Substring(6, 6).Insert(4, "-").Insert(2, "-");
			DateTime dateTime = default(DateTime);
			return DateTime.TryParse(s, out dateTime);
		}
		public static string SqlText(string sqlInput, int maxLength)
		{
			if (sqlInput != null && sqlInput != string.Empty)
			{
				sqlInput = sqlInput.Trim();
				if (sqlInput.Length > maxLength)
				{
					sqlInput = sqlInput.Substring(0, maxLength);
				}
			}
			return sqlInput;
		}
		public static string HtmlEncode(string inputData)
		{
			return HttpUtility.HtmlEncode(inputData);
		}
		public static void SetLabel(Label lbl, string txtInput)
		{
			lbl.Text = PageValidate.HtmlEncode(txtInput);
		}
		public static void SetLabel(Label lbl, object inputObj)
		{
			PageValidate.SetLabel(lbl, inputObj.ToString());
		}
		public static string InputText(string inputString, int maxLength)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (inputString != null && inputString != string.Empty)
			{
				inputString = inputString.Trim();
				if (inputString.Length > maxLength)
				{
					inputString = inputString.Substring(0, maxLength);
				}
				for (int i = 0; i < inputString.Length; i++)
				{
					char c = inputString[i];
					if (c != '"')
					{
						switch (c)
						{
						case '<':
							stringBuilder.Append("&lt;");
							goto IL_97;
						case '>':
							stringBuilder.Append("&gt;");
							goto IL_97;
						}
						stringBuilder.Append(inputString[i]);
					}
					else
					{
						stringBuilder.Append("&quot;");
					}
					IL_97:;
				}
				stringBuilder.Replace("'", " ");
			}
			return stringBuilder.ToString();
		}
		public static string Encode(string str)
		{
			str = str.Replace("&", "&amp;");
			str = str.Replace("'", "''");
			str = str.Replace("\"", "&quot;");
			str = str.Replace(" ", "&nbsp;");
			str = str.Replace("<", "&lt;");
			str = str.Replace(">", "&gt;");
			str = str.Replace("\n", "<br>");
			return str;
		}
		public static string Decode(string str)
		{
			str = str.Replace("<br>", "\n");
			str = str.Replace("&gt;", ">");
			str = str.Replace("&lt;", "<");
			str = str.Replace("&nbsp;", " ");
			str = str.Replace("&quot;", "\"");
			return str;
		}
		public static string SqlTextClear(string sqlText)
		{
			if (sqlText == null)
			{
				return null;
			}
			if (sqlText == "")
			{
				return "";
			}
			sqlText = sqlText.Replace(",", "");
			sqlText = sqlText.Replace("<", "");
			sqlText = sqlText.Replace(">", "");
			sqlText = sqlText.Replace("--", "");
			sqlText = sqlText.Replace("'", "");
			sqlText = sqlText.Replace("\"", "");
			sqlText = sqlText.Replace("=", "");
			sqlText = sqlText.Replace("%", "");
			sqlText = sqlText.Replace(" ", "");
			return sqlText;
		}
		public static bool isContainSameChar(string strInput)
		{
			string charInput = string.Empty;
			if (!string.IsNullOrEmpty(strInput))
			{
				charInput = strInput.Substring(0, 1);
			}
			return PageValidate.isContainSameChar(strInput, charInput, strInput.Length);
		}
		public static bool isContainSameChar(string strInput, string charInput, int lenInput)
		{
			if (string.IsNullOrEmpty(charInput))
			{
				return false;
			}
			Regex regex = new Regex(string.Format("^([{0}])+$", charInput));
			Match match = regex.Match(strInput);
			return match.Success;
		}
		public static bool isContainSpecChar(string strInput)
		{
			string[] array = new string[]
			{
				"123456",
				"654321"
			};
			bool result = false;
			for (int i = 0; i < array.Length; i++)
			{
				if (strInput == array[i])
				{
					result = true;
					break;
				}
			}
			return result;
		}
	}
}
