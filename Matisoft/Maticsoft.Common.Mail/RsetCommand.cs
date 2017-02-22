using System;
using System.IO;
namespace Maticsoft.Common.Mail
{
	internal sealed class RsetCommand : Pop3Command<Pop3Response>
	{
		public RsetCommand(Stream stream) : base(stream, false, Pop3State.Transaction)
		{
		}
		protected override byte[] CreateRequestMessage()
		{
			return base.GetRequestMessage(new string[]
			{
				"RSET\r\n"
			});
		}
	}
}
