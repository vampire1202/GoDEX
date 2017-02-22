using System;
using System.IO;
namespace Maticsoft.Common.Mail
{
	internal sealed class QuitCommand : Pop3Command<Pop3Response>
	{
		public QuitCommand(Stream stream) : base(stream, false, Pop3State.Authorization | Pop3State.Transaction)
		{
		}
		protected override byte[] CreateRequestMessage()
		{
			return base.GetRequestMessage(new string[]
			{
				"QUIT\r\n"
			});
		}
	}
}
