using System;
using System.IO;
namespace Maticsoft.Common.Mail
{
	internal sealed class StatCommand : Pop3Command<StatResponse>
	{
		public StatCommand(Stream stream) : base(stream, false, Pop3State.Transaction)
		{
		}
		protected override byte[] CreateRequestMessage()
		{
			return base.GetRequestMessage(new string[]
			{
				"STAT\r\n"
			});
		}
		protected override StatResponse CreateResponse(byte[] buffer)
		{
			Pop3Response pop3Response = Pop3Response.CreateResponse(buffer);
			string[] array = pop3Response.HostMessage.Split(new char[]
			{
				' '
			});
			if (array.Length < 3)
			{
				throw new Pop3Exception("Invalid response message: " + pop3Response.HostMessage);
			}
			int messageCount = Convert.ToInt32(array[1]);
			long octets = Convert.ToInt64(array[2]);
			return new StatResponse(pop3Response, messageCount, octets);
		}
	}
}
