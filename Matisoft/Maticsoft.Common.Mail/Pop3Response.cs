using System;
using System.IO;
namespace Maticsoft.Common.Mail
{
	internal class Pop3Response
	{
		private byte[] _responseContents;
		private bool _statusIndicator;
		private string _hostMessage;
		internal byte[] ResponseContents
		{
			get
			{
				return this._responseContents;
			}
		}
		public bool StatusIndicator
		{
			get
			{
				return this._statusIndicator;
			}
		}
		public string HostMessage
		{
			get
			{
				return this._hostMessage;
			}
		}
		public Pop3Response(byte[] responseContents, string hostMessage, bool statusIndicator)
		{
			if (responseContents == null)
			{
				throw new ArgumentNullException("responseBuffer");
			}
			if (string.IsNullOrEmpty(hostMessage))
			{
				throw new ArgumentNullException("hostMessage");
			}
			this._responseContents = responseContents;
			this._hostMessage = hostMessage;
			this._statusIndicator = statusIndicator;
		}
		public static Pop3Response CreateResponse(byte[] responseContents)
		{
			MemoryStream stream = new MemoryStream(responseContents);
			Pop3Response result;
			using (StreamReader streamReader = new StreamReader(stream))
			{
				string text = streamReader.ReadLine();
				if (text == null)
				{
					result = null;
				}
				else
				{
					bool statusIndicator = text.StartsWith("+OK");
					result = new Pop3Response(responseContents, text, statusIndicator);
				}
			}
			return result;
		}
	}
}
