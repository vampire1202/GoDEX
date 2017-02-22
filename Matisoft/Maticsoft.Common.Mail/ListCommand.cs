using System;
using System.Collections.Generic;
using System.IO;
namespace Maticsoft.Common.Mail
{
	internal sealed class ListCommand : Pop3Command<ListResponse>
	{
		private int _messageId;
		public ListCommand(Stream stream) : base(stream, true, Pop3State.Transaction)
		{
		}
		public ListCommand(Stream stream, int messageId) : this(stream)
		{
			if (messageId < 0)
			{
				throw new ArgumentOutOfRangeException("messageId");
			}
			this._messageId = messageId;
			base.IsMultiline = false;
		}
		protected override byte[] CreateRequestMessage()
		{
			string text = "LIST ";
			if (!base.IsMultiline)
			{
				text += this._messageId.ToString();
			}
			return base.GetRequestMessage(new string[]
			{
				text,
				"\r\n"
			});
		}
		protected override ListResponse CreateResponse(byte[] buffer)
		{
			Pop3Response pop3Response = Pop3Response.CreateResponse(buffer);
			List<Pop3ListItem> list;
			if (base.IsMultiline)
			{
				list = new List<Pop3ListItem>();
				string[] responseLines = base.GetResponseLines(base.StripPop3HostMessage(buffer, pop3Response.HostMessage));
				string[] array = responseLines;
				for (int i = 0; i < array.Length; i++)
				{
					string text = array[i];
					string[] array2 = text.Split(new char[]
					{
						' '
					});
					if (array2.Length < 2)
					{
						throw new Pop3Exception("Invalid line in multiline response:  " + text);
					}
					list.Add(new Pop3ListItem(Convert.ToInt32(array2[0]), Convert.ToInt64(array2[1])));
				}
			}
			else
			{
				list = new List<Pop3ListItem>(1);
				string[] array3 = pop3Response.HostMessage.Split(new char[]
				{
					' '
				});
				if (array3.Length < 3)
				{
					throw new Pop3Exception("Invalid response message: " + pop3Response.HostMessage);
				}
				list.Add(new Pop3ListItem(Convert.ToInt32(array3[1]), Convert.ToInt64(array3[2])));
			}
			return new ListResponse(pop3Response, list);
		}
	}
}
