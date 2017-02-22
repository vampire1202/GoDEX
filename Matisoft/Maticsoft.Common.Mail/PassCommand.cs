using System;
using System.IO;
namespace Maticsoft.Common.Mail
{
	internal sealed class PassCommand : Pop3Command<Pop3Response>
	{
		private string _password;
		public PassCommand(Stream stream, string password) : base(stream, false, Pop3State.Authorization)
		{
			if (string.IsNullOrEmpty(password))
			{
				throw new ArgumentNullException("password");
			}
			this._password = password;
		}
		protected override byte[] CreateRequestMessage()
		{
			return base.GetRequestMessage(new string[]
			{
				"PASS ",
				this._password,
				"\r\n"
			});
		}
	}
}
