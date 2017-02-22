using System;
using System.Configuration;
using System.IO;
using System.Web;
using System.Xml;
namespace Maticsoft.Common
{
	public class PopConfig
	{
		private static PopConfig _popConfig;
		private string ConfigFile
		{
			get
			{
				string text = ConfigurationManager.AppSettings["PopConfigPath"];
				if (string.IsNullOrEmpty(text) || text.Trim().Length == 0)
				{
					text = HttpContext.Current.Request.MapPath("/Config/PopSetting.config");
				}
				else
				{
					if (!Path.IsPathRooted(text))
					{
						text = HttpContext.Current.Request.MapPath(Path.Combine(text, "PopSetting.config"));
					}
					else
					{
						text = Path.Combine(text, "PopSetting.config");
					}
				}
				return text;
			}
		}
		public PopSetting PopSetting
		{
			get
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(this.ConfigFile);
				return new PopSetting
				{
					Server = xmlDocument.DocumentElement.SelectSingleNode("Server").InnerText,
					Port = Convert.ToInt32(xmlDocument.DocumentElement.SelectSingleNode("Port").InnerText),
					UseSSL = Convert.ToBoolean(xmlDocument.DocumentElement.SelectSingleNode("UseSSL").InnerText),
					UserName = xmlDocument.DocumentElement.SelectSingleNode("User").InnerText,
					Password = xmlDocument.DocumentElement.SelectSingleNode("Password").InnerText
				};
			}
		}
		public static PopConfig Create()
		{
			if (PopConfig._popConfig == null)
			{
				PopConfig._popConfig = new PopConfig();
			}
			return PopConfig._popConfig;
		}
	}
}
