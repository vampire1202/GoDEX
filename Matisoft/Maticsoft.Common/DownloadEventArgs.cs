using System;
namespace Maticsoft.Common
{
	public class DownloadEventArgs : EventArgs
	{
		private int bytesReceived;
		private int totalBytes;
		private byte[] receivedData;
		public int BytesReceived
		{
			get
			{
				return this.bytesReceived;
			}
			set
			{
				this.bytesReceived = value;
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
		public byte[] ReceivedData
		{
			get
			{
				return this.receivedData;
			}
			set
			{
				this.receivedData = value;
			}
		}
	}
}
