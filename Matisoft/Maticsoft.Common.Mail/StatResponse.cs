using System;
namespace Maticsoft.Common.Mail
{
	internal sealed class StatResponse : Pop3Response
	{
		private int _messageCount;
		private long _octets;
		public int MessageCount
		{
			get
			{
				return this._messageCount;
			}
		}
		public long Octets
		{
			get
			{
				return this._octets;
			}
		}
		public StatResponse(Pop3Response response, int messageCount, long octets) : base(response.ResponseContents, response.HostMessage, response.StatusIndicator)
		{
			this._messageCount = messageCount;
			this._octets = octets;
		}
	}
}
