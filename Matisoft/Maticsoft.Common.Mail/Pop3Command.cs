using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
namespace Maticsoft.Common.Mail
{
	internal abstract class Pop3Command<T> : IDisposable where T : Pop3Response
	{
		private const int BufferSize = 1024;
		private const string MultilineMessageTerminator = "\r\n.\r\n";
		private const string MessageTerminator = ".";
		private ManualResetEvent _manualResetEvent;
		private byte[] _buffer;
		private MemoryStream _responseContents;
		private Pop3State _validExecuteState;
		private Stream _networkStream;
		private bool _isMultiline;
		public event Action<string> Trace;
		public Pop3State ValidExecuteState
		{
			get
			{
				return this._validExecuteState;
			}
		}
		public Stream NetworkStream
		{
			get
			{
				return this._networkStream;
			}
			set
			{
				this._networkStream = value;
			}
		}
		protected bool IsMultiline
		{
			get
			{
				return this._isMultiline;
			}
			set
			{
				this._isMultiline = value;
			}
		}
		protected void OnTrace(string message)
		{
			if (this.Trace != null)
			{
				this.Trace(message);
			}
		}
		public Pop3Command(Stream stream, bool isMultiline, Pop3State validExecuteState)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			this._manualResetEvent = new ManualResetEvent(false);
			this._buffer = new byte[1024];
			this._responseContents = new MemoryStream();
			this._networkStream = stream;
			this._isMultiline = isMultiline;
			this._validExecuteState = validExecuteState;
		}
		protected abstract byte[] CreateRequestMessage();
		private void Send(byte[] message)
		{
			try
			{
				this._networkStream.Write(message, 0, message.Length);
			}
			catch (SocketException inner)
			{
				throw new Pop3Exception("Unable to send the request message: " + Encoding.ASCII.GetString(message), inner);
			}
		}
		internal virtual T Execute(Pop3State currentState)
		{
			this.EnsurePop3State(currentState);
			byte[] array = this.CreateRequestMessage();
			if (array != null)
			{
				this.Send(array);
			}
			T t = this.CreateResponse(this.GetResponse());
			if (t == null)
			{
				return default(T);
			}
			this.OnTrace(t.HostMessage);
			return t;
		}
		protected void EnsurePop3State(Pop3State currentState)
		{
			if ((currentState & this.ValidExecuteState) != currentState)
			{
				throw new Pop3Exception(string.Format("This command is being executedin an invalid execution state.  Current:{0}, Valid:{1}", currentState, this.ValidExecuteState));
			}
		}
		protected virtual T CreateResponse(byte[] buffer)
		{
			return Pop3Response.CreateResponse(buffer) as T;
		}
		private byte[] GetResponse()
		{
			AsyncCallback callback;
			if (this._isMultiline)
			{
				callback = new AsyncCallback(this.GetMultiLineResponseCallback);
			}
			else
			{
				callback = new AsyncCallback(this.GetSingleLineResponseCallback);
			}
			byte[] result;
			try
			{
				this.Receive(callback);
				this._manualResetEvent.WaitOne();
				result = this._responseContents.ToArray();
			}
			catch (SocketException inner)
			{
				throw new Pop3Exception("Unable to get response.", inner);
			}
			return result;
		}
		private IAsyncResult Receive(AsyncCallback callback)
		{
			return this._networkStream.BeginRead(this._buffer, 0, this._buffer.Length, callback, null);
		}
		private string WriteReceivedBytesToBuffer(int bytesReceived)
		{
			this._responseContents.Write(this._buffer, 0, bytesReceived);
			byte[] array = this._responseContents.ToArray();
			return Encoding.ASCII.GetString(array, (array.Length > 5) ? (array.Length - 5) : 0, 5);
		}
		private void GetSingleLineResponseCallback(IAsyncResult ar)
		{
			int bytesReceived = this._networkStream.EndRead(ar);
			string text = this.WriteReceivedBytesToBuffer(bytesReceived);
			if (text.EndsWith("\r\n"))
			{
				this._manualResetEvent.Set();
				return;
			}
			this.Receive(new AsyncCallback(this.GetSingleLineResponseCallback));
		}
		private void GetMultiLineResponseCallback(IAsyncResult ar)
		{
			int num = this._networkStream.EndRead(ar);
			string text = this.WriteReceivedBytesToBuffer(num);
			if (text.EndsWith("\r\n.\r\n") || num == 0)
			{
				this._manualResetEvent.Set();
				return;
			}
			this.Receive(new AsyncCallback(this.GetMultiLineResponseCallback));
		}
		protected byte[] GetRequestMessage(params string[] args)
		{
			string text = string.Join(string.Empty, args);
			this.OnTrace(text);
			return Encoding.ASCII.GetBytes(text);
		}
		protected MemoryStream StripPop3HostMessage(byte[] bytes, string header)
		{
			int num = header.Length + 2;
			return new MemoryStream(bytes, num, bytes.Length - num);
		}
		protected string[] GetResponseLines(MemoryStream stream)
		{
			List<string> list = new List<string>();
			string[] result;
			using (StreamReader streamReader = new StreamReader(stream))
			{
				try
				{
					while (true)
					{
						string text = streamReader.ReadLine();
						if (text.StartsWith("."))
						{
							if (text == ".")
							{
								break;
							}
							text = text.Substring(1);
						}
						list.Add(text);
					}
				}
				catch (IOException inner)
				{
					throw new Pop3Exception("Unable to get response lines.", inner);
				}
				result = list.ToArray();
			}
			return result;
		}
		public void Dispose()
		{
			if (this._responseContents != null)
			{
				this._responseContents.Dispose();
			}
		}
	}
}
