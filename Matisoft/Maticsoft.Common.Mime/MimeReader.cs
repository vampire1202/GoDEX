using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
namespace Maticsoft.Common.Mime
{
	public class MimeReader
	{
		private static readonly char[] HeaderWhitespaceChars = new char[]
		{
			' ',
			'\t'
		};
		private Queue<string> _lines;
		private MimeEntity _entity;
		public Queue<string> Lines
		{
			get
			{
				return this._lines;
			}
		}
		private MimeReader()
		{
			this._entity = new MimeEntity();
		}
		private MimeReader(MimeEntity entity, Queue<string> lines) : this()
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity");
			}
			if (lines == null)
			{
				throw new ArgumentNullException("lines");
			}
			this._lines = lines;
			this._entity = new MimeEntity(entity);
		}
		public MimeReader(string[] lines) : this()
		{
			if (lines == null)
			{
				throw new ArgumentNullException("lines");
			}
			this._lines = new Queue<string>(lines);
		}
		private int ParseHeaders()
		{
			string name = string.Empty;
			string text = string.Empty;
			while (this._lines.Count > 0 && !string.IsNullOrEmpty(this._lines.Peek()))
			{
				text = this._lines.Dequeue();
				if (text.StartsWith(" ") || text.StartsWith(Convert.ToString('\t')))
				{
					this._entity.Headers[name] = this._entity.Headers[name] + text;
				}
				else
				{
					int num = text.IndexOf(':');
					if (num >= 0)
					{
						string text2 = text.Substring(0, num);
						string value = text.Substring(num + 1).Trim(MimeReader.HeaderWhitespaceChars);
						this._entity.Headers.Add(text2.ToLower(), value);
						name = text2;
					}
				}
			}
			if (this._lines.Count > 0)
			{
				this._lines.Dequeue();
			}
			return this._entity.Headers.Count;
		}
		private void ProcessHeaders()
		{
			string[] allKeys = this._entity.Headers.AllKeys;
			for (int i = 0; i < allKeys.Length; i++)
			{
				string text = allKeys[i];
				string a;
				if ((a = text) != null)
				{
					if (!(a == "content-description"))
					{
						if (!(a == "content-disposition"))
						{
							if (!(a == "content-id"))
							{
								if (!(a == "content-transfer-encoding"))
								{
									if (!(a == "content-type"))
									{
										if (a == "mime-version")
										{
											this._entity.MimeVersion = this._entity.Headers[text];
										}
									}
									else
									{
										this._entity.SetContentType(MimeReader.GetContentType(this._entity.Headers[text]));
									}
								}
								else
								{
									this._entity.TransferEncoding = this._entity.Headers[text];
									this._entity.ContentTransferEncoding = MimeReader.GetTransferEncoding(this._entity.Headers[text]);
								}
							}
							else
							{
								this._entity.ContentId = this._entity.Headers[text];
							}
						}
						else
						{
							this._entity.ContentDisposition = new ContentDisposition(this._entity.Headers[text]);
						}
					}
					else
					{
						this._entity.ContentDescription = this._entity.Headers[text];
					}
				}
			}
		}
		public MimeEntity CreateMimeEntity()
		{
			MimeEntity result;
			try
			{
				this.ParseHeaders();
				this.ProcessHeaders();
				this.ParseBody();
				this.SetDecodedContentStream();
				result = this._entity;
			}
			catch
			{
				result = null;
			}
			return result;
		}
		private void SetDecodedContentStream()
		{
			switch (this._entity.ContentTransferEncoding)
			{
			case TransferEncoding.QuotedPrintable:
				this._entity.Content = new MemoryStream(this.GetBytes(QuotedPrintableEncoding.Decode(this._entity.EncodedMessage.ToString())), false);
				return;
			case TransferEncoding.Base64:
				this._entity.Content = new MemoryStream(Convert.FromBase64String(this._entity.EncodedMessage.ToString()), false);
				return;
			}
			this._entity.Content = new MemoryStream(this.GetBytes(this._entity.EncodedMessage.ToString()), false);
		}
		private byte[] GetBytes(string content)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (StreamWriter streamWriter = new StreamWriter(memoryStream))
				{
					streamWriter.Write(content);
				}
				result = memoryStream.ToArray();
			}
			return result;
		}
		private void ParseBody()
		{
			if (this._entity.HasBoundary)
			{
				while (this._lines.Count > 0)
				{
					if (string.Equals(this._lines.Peek(), this._entity.EndBoundary))
					{
						return;
					}
					if (this._entity.Parent != null && string.Equals(this._entity.Parent.StartBoundary, this._lines.Peek()))
					{
						return;
					}
					if (string.Equals(this._lines.Peek(), this._entity.StartBoundary))
					{
						this.AddChildEntity(this._entity, this._lines);
					}
					else
					{
						if (string.Equals(this._entity.ContentType.MediaType, MediaTypes.MessageRfc822, StringComparison.InvariantCultureIgnoreCase) && string.Equals(this._entity.ContentDisposition.DispositionType, "attachment", StringComparison.InvariantCultureIgnoreCase))
						{
							this.AddChildEntity(this._entity, this._lines);
							return;
						}
						this._entity.EncodedMessage.Append(this._lines.Dequeue() + "\r\n");
					}
				}
			}
			else
			{
				while (this._lines.Count > 0)
				{
					this._entity.EncodedMessage.Append(this._lines.Dequeue() + "\r\n");
				}
			}
		}
		private void AddChildEntity(MimeEntity entity, Queue<string> lines)
		{
			MimeReader mimeReader = new MimeReader(entity, lines);
			entity.Children.Add(mimeReader.CreateMimeEntity());
		}
		public static ContentType GetContentType(string contentType)
		{
			if (string.IsNullOrEmpty(contentType))
			{
				contentType = "text/plain; charset=us-ascii";
			}
			return new ContentType(contentType);
		}
		public static string GetMediaType(string mediaType)
		{
			if (string.IsNullOrEmpty(mediaType))
			{
				return "text/plain";
			}
			return mediaType.Trim();
		}
		public static string GetMediaMainType(string mediaType)
		{
			int num = mediaType.IndexOf('/');
			if (num < 0)
			{
				return mediaType;
			}
			return mediaType.Substring(0, num);
		}
		public static string GetMediaSubType(string mediaType)
		{
			int num = mediaType.IndexOf('/');
			if (num < 0)
			{
				if (mediaType.Equals("text"))
				{
					return "plain";
				}
				return string.Empty;
			}
			else
			{
				if (mediaType.Length > num)
				{
					return mediaType.Substring(num + 1);
				}
				string mediaMainType = MimeReader.GetMediaMainType(mediaType);
				if (mediaMainType.Equals("text"))
				{
					return "plain";
				}
				return string.Empty;
			}
		}
		public static TransferEncoding GetTransferEncoding(string transferEncoding)
		{
			string a;
			if ((a = transferEncoding.Trim().ToLowerInvariant()) != null)
			{
				if (a == "7bit" || a == "8bit")
				{
					return TransferEncoding.SevenBit;
				}
				if (a == "quoted-printable")
				{
					return TransferEncoding.QuotedPrintable;
				}
				if (a == "base64")
				{
					return TransferEncoding.Base64;
				}
				if (!(a == "binary"))
				{
				}
			}
			return TransferEncoding.Unknown;
		}
	}
}
