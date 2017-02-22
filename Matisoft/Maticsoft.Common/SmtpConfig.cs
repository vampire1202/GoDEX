using System;
using System.Configuration;
using System.IO;
using System.Web;
using System.Xml;
namespace Maticsoft.Common
{
	public class SmtpConfig
	{
		private static SmtpConfig _smtpConfig;
		private string ConfigFile
		{
			get
			{
				string text = ConfigurationManager.AppSettings["SmtpConfigPath"];
				if (string.IsNullOrEmpty(text) || text.Trim().Length == 0)
				{
					text = HttpContext.Current.Request.MapPath("/Config/SmtpSetting.config");
				}
				else
				{
					if (!Path.IsPathRooted(text))
					{
						text = HttpContext.Current.Request.MapPath(Path.Combine(text, "SmtpSetting.config"));
					}
					else
					{
						text = Path.Combine(text, "SmtpSetting.config");
					}
				}
				return text;
			}
		}
		public SmtpSetting SmtpSetting
		{
			get
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(this.ConfigFile);
				return new SmtpSetting
				{
					Server = xmlDocument.DocumentElement.SelectSingleNode("Server").InnerText,
					Authentication = Convert.ToBoolean(xmlDocument.DocumentElement.SelectSingleNode("Authentication").InnerText),
					UserName = xmlDocument.DocumentElement.SelectSingleNode("User").InnerText,
					Password = xmlDocument.DocumentElement.SelectSingleNode("Password").InnerText,
					Sender = xmlDocument.DocumentElement.SelectSingleNode("Sender").InnerText
				};
			}
		}
		private SmtpConfig()
		{
		}
		public static SmtpConfig Create()
		{
			if (SmtpConfig._smtpConfig == null)
			{
				SmtpConfig._smtpConfig = new SmtpConfig();
			}
			return SmtpConfig._smtpConfig;
		}
	}
}
