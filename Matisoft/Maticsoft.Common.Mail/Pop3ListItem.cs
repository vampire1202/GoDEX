using System;
namespace Maticsoft.Common.Mail
{
	public class Pop3ListItem
	{
		private int _messageNumber;
		private long _octets;
		public int MessageId
		{
			get
			{
				return this._messageNumber;
			}
			set
			{
				this._messageNumber = value;
			}
		}
		public long Octets
		{
			get
			{
				return this._octets;
			}
			set
			{
				this._octets = value;
			}
		}
		public Pop3ListItem(int messageNumber, long octets)
		{
			if (messageNumber < 0)
			{
				throw new ArgumentOutOfRangeException("messageNumber");
			}
			if (octets < 1L)
			{
				throw new ArgumentOutOfRangeException("octets");
			}
			this._messageNumber = messageNumber;
			this._octets = octets;
		}
	}
}
