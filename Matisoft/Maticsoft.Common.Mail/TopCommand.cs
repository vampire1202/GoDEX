using System;
using System.IO;
namespace Maticsoft.Common.Mail
{
	internal sealed class TopCommand : Pop3Command<RetrResponse>
	{
		private int _messageNumber;
		private int _lineCount;
		internal TopCommand(Stream stream, int messageNumber, int lineCount) : base(stream, true, Pop3State.Transaction)
		{
			if (messageNumber < 1)
			{
				throw new ArgumentOutOfRangeException("messageNumber");
			}
			if (lineCount < 0)
			{
				throw new ArgumentOutOfRangeException("lineCount");
			}
			this._messageNumber = messageNumber;
			this._lineCount = lineCount;
		}
		protected override byte[] CreateRequestMessage()
		{
			return base.GetRequestMessage(new string[]
			{
				"TOP ",
				this._messageNumber.ToString(),
				" ",
				this._lineCount.ToString(),
				"\r\n"
			});
		}
		protected override RetrResponse CreateResponse(byte[] buffer)
		{
			Pop3Response pop3Response = Pop3Response.CreateResponse(buffer);
			if (pop3Response == null)
			{
				return null;
			}
			string[] responseLines = base.GetResponseLines(base.StripPop3HostMessage(buffer, pop3Response.HostMessage));
			return new RetrResponse(pop3Response, responseLines);
		}
	}
}
