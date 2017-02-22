using System;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
namespace Maticsoft.Common.Mail
{
	internal sealed class ConnectCommand : Pop3Command<ConnectResponse>
	{
		private TcpClient _client;
		private string _hostname;
		private int _port;
		private bool _useSsl;
		public ConnectCommand(TcpClient client, string hostname, int port, bool useSsl) : base(new MemoryStream(), false, Pop3State.Unknown)
		{
			if (client == null)
			{
				throw new ArgumentNullException("client");
			}
			if (string.IsNullOrEmpty(hostname))
			{
				throw new ArgumentNullException("hostname");
			}
			if (port < 1)
			{
				throw new ArgumentOutOfRangeException("port");
			}
			this._client = client;
			this._hostname = hostname;
			this._port = port;
			this._useSsl = useSsl;
		}
		protected override byte[] CreateRequestMessage()
		{
			return null;
		}
		internal override ConnectResponse Execute(Pop3State currentState)
		{
			base.EnsurePop3State(currentState);
			try
			{
				this._client.Connect(this._hostname, this._port);
				this.SetClientStream();
			}
			catch (SocketException inner)
			{
				throw new Pop3Exception(string.Format("Unable to connect to {0}:{1}.", this._hostname, this._port), inner);
			}
			return base.Execute(currentState);
		}
		private void SetClientStream()
		{
			if (this._useSsl)
			{
				try
				{
					base.NetworkStream = new SslStream(this._client.GetStream(), true);
					((SslStream)base.NetworkStream).AuthenticateAsClient(this._hostname);
					return;
				}
				catch (ArgumentException inner)
				{
					throw new Pop3Exception("Unable to create Ssl Stream for hostname: " + this._hostname, inner);
				}
				catch (AuthenticationException inner2)
				{
					throw new Pop3Exception("Unable to authenticate ssl stream for hostname: " + this._hostname, inner2);
				}
				catch (InvalidOperationException inner3)
				{
					throw new Pop3Exception("There was a problem  attempting to authenticate this SSL stream for hostname: " + this._hostname, inner3);
				}
			}
			base.NetworkStream = this._client.GetStream();
		}
		protected override ConnectResponse CreateResponse(byte[] buffer)
		{
			Pop3Response response = Pop3Response.CreateResponse(buffer);
			return new ConnectResponse(response, base.NetworkStream);
		}
	}
}
