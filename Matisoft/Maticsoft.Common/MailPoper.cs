using Maticsoft.Common.Mail;
using System;
using System.Collections.Generic;
namespace Maticsoft.Common
{
	public class MailPoper
	{
		public static List<MailMessageEx> Receive()
		{
			PopSetting popSetting = PopConfig.Create().PopSetting;
			return MailPoper.Receive(popSetting.Server, popSetting.Port, popSetting.UseSSL, popSetting.UserName, popSetting.Password);
		}
		public static List<MailMessageEx> Receive(string hostname, int port, bool useSsl, string username, string password)
		{
			List<MailMessageEx> result;
			using (Pop3Client pop3Client = new Pop3Client(hostname, port, useSsl, username, password))
			{
				pop3Client.Trace += new Action<string>(Console.WriteLine);
				pop3Client.Authenticate();
				pop3Client.Stat();
				List<MailMessageEx> list = new List<MailMessageEx>();
				foreach (Pop3ListItem current in pop3Client.List())
				{
					MailMessageEx mailMessageEx = pop3Client.RetrMailMessageEx(current);
					if (mailMessageEx != null)
					{
						pop3Client.Dele(current);
						list.Add(mailMessageEx);
					}
				}
				pop3Client.Noop();
				pop3Client.Rset();
				pop3Client.Quit();
				result = list;
			}
			return result;
		}
	}
}
