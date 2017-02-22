using System;
using System.IO;
namespace Maticsoft.Common.Mail
{
	internal sealed class RetrCommand : Pop3Command<RetrResponse>
	{
		private int _message;
		public RetrCommand(Stream stream, int message) : base(stream, true, Pop3State.Transaction)
		{
			if (message < 0)
			{
				throw new ArgumentOutOfRangeException("message");
			}
			this._message = message;
		}
		protected override byte[] CreateRequestMessage()
		{
			return base.GetRequestMessage(new string[]
			{
				"RETR ",
				this._message.ToString(),
				"\r\n"
			});
		}
		protected override RetrResponse CreateResponse(byte[] buffer)
		{
			Pop3Response pop3Response = Pop3Response.CreateResponse(buffer);
			string[] responseLines = base.GetResponseLines(base.StripPop3HostMessage(buffer, pop3Response.HostMessage));
			return new RetrResponse(pop3Response, responseLines);
		}
	}
}
