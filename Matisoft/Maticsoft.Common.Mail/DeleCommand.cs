using System;
using System.IO;
namespace Maticsoft.Common.Mail
{
	internal sealed class DeleCommand : Pop3Command<Pop3Response>
	{
		private int _messageId = -2147483648;
		public DeleCommand(Stream stream, int messageId) : base(stream, false, Pop3State.Transaction)
		{
			if (messageId < 0)
			{
				throw new ArgumentOutOfRangeException("_messageId");
			}
			this._messageId = messageId;
		}
		protected override byte[] CreateRequestMessage()
		{
			return base.GetRequestMessage(new string[]
			{
				"DELE " + this._messageId.ToString() + "\r\n"
			});
		}
	}
}
