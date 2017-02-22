using System;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
namespace Maticsoft.Common
{
	public class UrlOper
	{
		private static Encoding encoding = Encoding.UTF8;
		public static string Base64Encrypt(string sourthUrl)
		{
			string s = HttpUtility.UrlEncode(sourthUrl);
			return Convert.ToBase64String(UrlOper.encoding.GetBytes(s));
		}
		public static string Base64Decrypt(string eStr)
		{
			if (!UrlOper.IsBase64(eStr))
			{
				return eStr;
			}
			byte[] bytes = Convert.FromBase64String(eStr);
			string @string = UrlOper.encoding.GetString(bytes);
			return HttpUtility.UrlDecode(@string);
		}
		public static bool IsBase64(string eStr)
		{
			return eStr.Length % 4 == 0 && Regex.IsMatch(eStr, "^[A-Z0-9/+=]*$", RegexOptions.IgnoreCase);
		}
		public static string AddParam(string url, string paramName, string value)
		{
			Uri uri = new Uri(url);
			if (string.IsNullOrEmpty(uri.Query))
			{
				string str = HttpContext.Current.Server.UrlEncode(value);
				return url + ("?" + paramName + "=" + str);
			}
			string str2 = HttpContext.Current.Server.UrlEncode(value);
			return url + ("&" + paramName + "=" + str2);
		}
		public static string UpdateParam(string url, string paramName, string value)
		{
			string text = paramName + "=";
			int num = url.IndexOf(text) + text.Length;
			int num2 = url.IndexOf("&", num);
			if (num2 == -1)
			{
				url = url.Remove(num, url.Length - num);
				url += value;
				return url;
			}
			url = url.Remove(num, num2 - num);
			url = url.Insert(num, value);
			return url;
		}
		public static void GetDomain(string fromUrl, out string domain, out string subDomain)
		{
			domain = "";
			subDomain = "";
			try
			{
				if (fromUrl.IndexOf("的名片") > -1)
				{
					subDomain = fromUrl;
					domain = "名片";
				}
				else
				{
					UriBuilder uriBuilder = new UriBuilder(fromUrl);
					fromUrl = uriBuilder.ToString();
					Uri uri = new Uri(fromUrl);
					if (uri.IsWellFormedOriginalString())
					{
						if (uri.IsFile)
						{
							string text;
							domain = (text = "客户端本地文件路径");
							subDomain = text;
						}
						else
						{
							string text2 = uri.Authority;
							string[] array = uri.Authority.Split(new char[]
							{
								'.'
							});
							if (array.Length == 2)
							{
								text2 = "www." + text2;
							}
							int num = text2.IndexOf('.', 0);
							domain = text2.Substring(num + 1, text2.Length - num - 1).Replace("comhttp", "com");
							subDomain = text2.Replace("comhttp", "com");
							if (array.Length < 2)
							{
								domain = "不明路径";
								subDomain = "不明路径";
							}
						}
					}
					else
					{
						if (uri.IsFile)
						{
							string text3;
							domain = (text3 = "客户端本地文件路径");
							subDomain = text3;
						}
						else
						{
							string text4;
							domain = (text4 = "不明路径");
							subDomain = text4;
						}
					}
				}
			}
			catch
			{
				string text5;
				domain = (text5 = "不明路径");
				subDomain = text5;
			}
		}
		public static void ParseUrl(string url, out string baseUrl, out NameValueCollection nvc)
		{
			if (url == null)
			{
				throw new ArgumentNullException("url");
			}
			nvc = new NameValueCollection();
			baseUrl = "";
			if (url == "")
			{
				return;
			}
			int num = url.IndexOf('?');
			if (num == -1)
			{
				baseUrl = url;
				return;
			}
			baseUrl = url.Substring(0, num);
			if (num == url.Length - 1)
			{
				return;
			}
			string input = url.Substring(num + 1);
			Regex regex = new Regex("(^|&)?(\\w+)=([^&]+)(&|$)?", RegexOptions.Compiled);
			MatchCollection matchCollection = regex.Matches(input);
			foreach (Match match in matchCollection)
			{
				nvc.Add(match.Result("$2").ToLower(), match.Result("$3"));
			}
		}
	}
}
