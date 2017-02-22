using System;
namespace Maticsoft.Common.Mail
{
	public sealed class Stat
	{
		private int _messageCount;
		private long _octets;
		public int MessageCount
		{
			get
			{
				return this._messageCount;
			}
			set
			{
				this._messageCount = value;
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
		public Stat(int messageCount, long octets)
		{
			if (messageCount < 0)
			{
				throw new ArgumentOutOfRangeException("messageCount");
			}
			if (octets < 0L)
			{
				throw new ArgumentOutOfRangeException("octets");
			}
			this._messageCount = messageCount;
			this._octets = octets;
		}
	}
}
