using Maticsoft.Common.Mime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
namespace Maticsoft.Common.Mail
{
	public sealed class Pop3Client : IDisposable
	{
		private static readonly int DefaultPort = 110;
		private TcpClient _client;
		private Stream _clientStream;
		private string _hostname;
		private int _port;
		private bool _useSsl;
		private string _username;
		private string _password;
		private Pop3State _currentState;
		public event Action<string> Trace;
		public string Hostname
		{
			get
			{
				return this._hostname;
			}
		}
		public int Port
		{
			get
			{
				return this._port;
			}
		}
		public bool UseSsl
		{
			get
			{
				return this._useSsl;
			}
		}
		public string Username
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
		public Pop3State CurrentState
		{
			get
			{
				return this._currentState;
			}
		}
		private void OnTrace(string message)
		{
			if (this.Trace != null)
			{
				this.Trace(message);
			}
		}
		public Pop3Client(string hostname, string username, string password) : this(hostname, Pop3Client.DefaultPort, false, username, password)
		{
		}
		public Pop3Client(string hostname, bool useSsl, string username, string password) : this(hostname, Pop3Client.DefaultPort, useSsl, username, password)
		{
		}
		public Pop3Client(string hostname, int port, bool useSsl, string username, string password) : this()
		{
			if (string.IsNullOrEmpty(hostname))
			{
				throw new ArgumentNullException("hostname");
			}
			if (port < 0)
			{
				throw new ArgumentOutOfRangeException("port");
			}
			if (string.IsNullOrEmpty(username))
			{
				throw new ArgumentNullException("username");
			}
			if (string.IsNullOrEmpty(password))
			{
				throw new ArgumentNullException("password");
			}
			this._hostname = hostname;
			this._port = port;
			this._useSsl = useSsl;
			this._username = username;
			this._password = password;
		}
		private Pop3Client()
		{
			this._client = new TcpClient();
			this._currentState = Pop3State.Unknown;
		}
		private void EnsureConnection()
		{
			if (!this._client.Connected)
			{
				throw new Pop3Exception("Pop3 client is not connected.");
			}
		}
		private void SetState(Pop3State state)
		{
			this._currentState = state;
		}
		private void EnsureResponse(Pop3Response response, string error)
		{
			if (response == null)
			{
				throw new Pop3Exception("Unable to get Response.  Response object null.");
			}
			if (response.StatusIndicator)
			{
				return;
			}
			string message = string.Empty;
			if (string.IsNullOrEmpty(error))
			{
				message = response.HostMessage;
			}
			else
			{
				message = error + ": " + error;
			}
			throw new Pop3Exception(message);
		}
		private void EnsureResponse(Pop3Response response)
		{
			this.EnsureResponse(response, string.Empty);
		}
		private void TraceCommand<TCommand, TResponse>(TCommand command) where TCommand : Pop3Command<TResponse> where TResponse : Pop3Response
		{
			if (this.Trace != null)
			{
				command.Trace += delegate(string message)
				{
					this.OnTrace(message);
				};
			}
		}
		private void Connect()
		{
			if (this._client == null)
			{
				this._client = new TcpClient();
			}
			if (this._client.Connected)
			{
				return;
			}
			this.SetState(Pop3State.Unknown);
			ConnectResponse connectResponse;
			using (ConnectCommand connectCommand = new ConnectCommand(this._client, this._hostname, this._port, this._useSsl))
			{
				this.TraceCommand<ConnectCommand, ConnectResponse>(connectCommand);
				connectResponse = connectCommand.Execute(this.CurrentState);
				this.EnsureResponse(connectResponse);
			}
			this.SetClientStream(connectResponse.NetworkStream);
			this.SetState(Pop3State.Authorization);
		}
		private void SetClientStream(Stream networkStream)
		{
			if (this._clientStream != null)
			{
				this._clientStream.Dispose();
			}
			this._clientStream = networkStream;
		}
		public void Authenticate()
		{
			this.Connect();
			using (UserCommand userCommand = new UserCommand(this._clientStream, this._username))
			{
				this.ExecuteCommand<Pop3Response, UserCommand>(userCommand);
			}
			using (PassCommand passCommand = new PassCommand(this._clientStream, this._password))
			{
				this.ExecuteCommand<Pop3Response, PassCommand>(passCommand);
			}
			this._currentState = Pop3State.Transaction;
		}
		public void Dele(Pop3ListItem item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			using (DeleCommand deleCommand = new DeleCommand(this._clientStream, item.MessageId))
			{
				this.ExecuteCommand<Pop3Response, DeleCommand>(deleCommand);
			}
		}
		public void Noop()
		{
			using (NoopCommand noopCommand = new NoopCommand(this._clientStream))
			{
				this.ExecuteCommand<Pop3Response, NoopCommand>(noopCommand);
			}
		}
		public void Rset()
		{
			using (RsetCommand rsetCommand = new RsetCommand(this._clientStream))
			{
				this.ExecuteCommand<Pop3Response, RsetCommand>(rsetCommand);
			}
		}
		public Stat Stat()
		{
			StatResponse statResponse;
			using (StatCommand statCommand = new StatCommand(this._clientStream))
			{
				statResponse = this.ExecuteCommand<StatResponse, StatCommand>(statCommand);
			}
			return new Stat(statResponse.MessageCount, statResponse.Octets);
		}
		public List<Pop3ListItem> List()
		{
			ListResponse listResponse;
			using (ListCommand listCommand = new ListCommand(this._clientStream))
			{
				listResponse = this.ExecuteCommand<ListResponse, ListCommand>(listCommand);
			}
			return listResponse.Items;
		}
		public Pop3ListItem List(int messageId)
		{
			ListResponse listResponse;
			using (ListCommand listCommand = new ListCommand(this._clientStream, messageId))
			{
				listResponse = this.ExecuteCommand<ListResponse, ListCommand>(listCommand);
			}
			return new Pop3ListItem(listResponse.MessageNumber, listResponse.Octets);
		}
		public MimeEntity RetrMimeEntity(Pop3ListItem item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			if (item.MessageId < 1)
			{
				throw new ArgumentOutOfRangeException("item.MessageId");
			}
			RetrResponse retrResponse;
			using (RetrCommand retrCommand = new RetrCommand(this._clientStream, item.MessageId))
			{
				retrResponse = this.ExecuteCommand<RetrResponse, RetrCommand>(retrCommand);
			}
			MimeReader mimeReader = new MimeReader(retrResponse.MessageLines);
			return mimeReader.CreateMimeEntity();
		}
		public MailMessageEx Top(int messageId, int lineCount)
		{
			if (messageId < 1)
			{
				throw new ArgumentOutOfRangeException("messageId");
			}
			if (lineCount < 0)
			{
				throw new ArgumentOutOfRangeException("lineCount");
			}
			RetrResponse retrResponse;
			using (TopCommand topCommand = new TopCommand(this._clientStream, messageId, lineCount))
			{
				retrResponse = this.ExecuteCommand<RetrResponse, TopCommand>(topCommand);
			}
			MimeReader mimeReader = new MimeReader(retrResponse.MessageLines);
			MimeEntity mimeEntity = mimeReader.CreateMimeEntity();
			MailMessageEx mailMessageEx = mimeEntity.ToMailMessageEx();
			mailMessageEx.Octets = retrResponse.Octets;
			mailMessageEx.MessageNumber = messageId;
			return mimeEntity.ToMailMessageEx();
		}
		public MailMessageEx RetrMailMessageEx(Pop3ListItem item)
		{
			MailMessageEx mailMessageEx = this.RetrMimeEntity(item).ToMailMessageEx();
			if (mailMessageEx != null)
			{
				mailMessageEx.MessageNumber = item.MessageId;
			}
			return mailMessageEx;
		}
		public void Quit()
		{
			using (QuitCommand quitCommand = new QuitCommand(this._clientStream))
			{
				this.ExecuteCommand<Pop3Response, QuitCommand>(quitCommand);
				if (this.CurrentState.Equals(Pop3State.Transaction))
				{
					this.SetState(Pop3State.Update);
				}
				this.Disconnect();
				this.SetState(Pop3State.Unknown);
			}
		}
		private TResponse ExecuteCommand<TResponse, TCommand>(TCommand command) where TResponse : Pop3Response where TCommand : Pop3Command<TResponse>
		{
			this.EnsureConnection();
			this.TraceCommand<TCommand, TResponse>(command);
			TResponse tResponse = command.Execute(this.CurrentState);
			this.EnsureResponse(tResponse);
			return tResponse;
		}
		private void Disconnect()
		{
			if (this._clientStream != null)
			{
				this._clientStream.Close();
			}
			if (this._client != null)
			{
				this._client.Close();
				this._client = null;
			}
		}
		public void Dispose()
		{
			this.Disconnect();
		}
	}
}
