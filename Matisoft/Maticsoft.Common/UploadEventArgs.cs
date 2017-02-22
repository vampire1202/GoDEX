using System;
namespace Maticsoft.Common
{
	public class UploadEventArgs : EventArgs
	{
		private int bytesSent;
		private int totalBytes;
		public int BytesSent
		{
			get
			{
				return this.bytesSent;
			}
			set
			{
				this.bytesSent = value;
			}
		}
		public int TotalBytes
		{
			get
			{
				return this.totalBytes;
			}
			set
			{
				this.totalBytes = value;
			}
		}
	}
}
