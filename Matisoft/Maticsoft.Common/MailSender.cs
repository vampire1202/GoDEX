using System;
using System.Net;
using System.Net.Mail;
using System.Text;
namespace Maticsoft.Common
{
	public class MailSender
	{
		public static void Send(string tomail, string bccmail, string subject, string body, params string[] files)
		{
			MailSender.Send(SmtpConfig.Create().SmtpSetting.Sender, tomail, bccmail, subject, body, true, Encoding.Default, true, files);
		}
		public static void Send(string frommail, string tomail, string bccmail, string subject, string body, bool isBodyHtml, Encoding encoding, bool isAuthentication, params string[] files)
		{
			MailSender.Send(SmtpConfig.Create().SmtpSetting.Server, SmtpConfig.Create().SmtpSetting.UserName, SmtpConfig.Create().SmtpSetting.Password, frommail, tomail, "", bccmail, subject, body, isBodyHtml, encoding, isAuthentication, files);
		}
		public static void Send(string server, string username, string password, string frommail, string tomail, string ccmail, string bccmail, string subject, string body, bool isBodyHtml, Encoding encoding, bool isAuthentication, params string[] files)
		{
			SmtpClient smtpClient = new SmtpClient(server);
			MailMessage mailMessage = new MailMessage(frommail, tomail);
			if (bccmail.Length > 1)
			{
				string[] strArray = StringPlus.GetStrArray(bccmail);
				string[] array = strArray;
				for (int i = 0; i < array.Length; i++)
				{
					string text = array[i];
					if (text.Trim() != "")
					{
						MailAddress item = new MailAddress(text.Trim());
						mailMessage.Bcc.Add(item);
					}
				}
			}
			if (ccmail.Length > 1)
			{
				string[] strArray2 = StringPlus.GetStrArray(ccmail);
				string[] array2 = strArray2;
				for (int j = 0; j < array2.Length; j++)
				{
					string text2 = array2[j];
					if (text2.Trim() != "")
					{
						MailAddress item2 = new MailAddress(text2.Trim());
						mailMessage.CC.Add(item2);
					}
				}
			}
			mailMessage.IsBodyHtml = isBodyHtml;
			mailMessage.SubjectEncoding = encoding;
			mailMessage.BodyEncoding = encoding;
			mailMessage.Subject = subject;
			mailMessage.Body = body;
			mailMessage.Attachments.Clear();
			if (files != null && files.Length != 0)
			{
				for (int k = 0; k < files.Length; k++)
				{
					Attachment item3 = new Attachment(files[k]);
					mailMessage.Attachments.Add(item3);
				}
			}
			if (isAuthentication)
			{
				smtpClient.Credentials = new NetworkCredential(username, password);
			}
			smtpClient.Send(mailMessage);
			mailMessage.Attachments.Dispose();
		}
		public static void Send(string recipient, string subject, string body)
		{
			MailSender.Send(SmtpConfig.Create().SmtpSetting.Sender, recipient, "", subject, body, true, Encoding.Default, true, null);
		}
		public static void Send(string Recipient, string Sender, string Subject, string Body)
		{
			MailSender.Send(Sender, Recipient, "", Subject, Body, true, Encoding.UTF8, true, null);
		}
	}
}
