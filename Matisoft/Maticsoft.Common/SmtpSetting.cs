using System;
namespace Maticsoft.Common
{
	public class SmtpSetting
	{
		private string _server;
		private bool _authentication;
		private string _username;
		private string _sender;
		private string _password;
		public string Server
		{
			get
			{
				return this._server;
			}
			set
			{
				this._server = value;
			}
		}
		public bool Authentication
		{
			get
			{
				return this._authentication;
			}
			set
			{
				this._authentication = value;
			}
		}
		public string UserName
		{
			get
			{
				return this._username;
			}
			set
			{
				this._username = value;
			}
		}
		public string Sender
		{
			get
			{
				return this._sender;
			}
			set
			{
				this._sender = value;
			}
		}
		public string Password
		{
			get
			{
				return this._password;
			}
			set
			{
				this._password = value;
			}
		}
	}
}
