using System;
using System.IO;
namespace Maticsoft.Common.Mail
{
	internal sealed class UserCommand : Pop3Command<Pop3Response>
	{
		private string _username;
		public UserCommand(Stream stream, string username) : base(stream, false, Pop3State.Authorization)
		{
			if (string.IsNullOrEmpty(username))
			{
				throw new ArgumentNullException("username");
			}
			this._username = username;
		}
		protected override byte[] CreateRequestMessage()
		{
			return base.GetRequestMessage(new string[]
			{
				"USER ",
				this._username,
				"\r\n"
			});
		}
	}
}
