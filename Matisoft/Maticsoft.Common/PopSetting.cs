using System;
namespace Maticsoft.Common
{
	public class PopSetting
	{
		private string _server;
		private int _port;
		private bool _usessl;
		private string _username;
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
		public int Port
		{
			get
			{
				return this._port;
			}
			set
			{
				this._port = value;
			}
		}
		public bool UseSSL
		{
			get
			{
				return this._usessl;
			}
			set
			{
				this._usessl = value;
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
