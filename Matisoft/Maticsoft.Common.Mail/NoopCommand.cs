using System;
using System.IO;
namespace Maticsoft.Common.Mail
{
	internal sealed class NoopCommand : Pop3Command<Pop3Response>
	{
		public NoopCommand(Stream stream) : base(stream, false, Pop3State.Transaction)
		{
		}
		protected override byte[] CreateRequestMessage()
		{
			return base.GetRequestMessage(new string[]
			{
				"NOOP\r\n"
			});
		}
	}
}
