using Microsoft.Win32;
using System;
using System.IO;
using System.Text;
namespace Maticsoft.Common
{
	public class MultipartForm
	{
		private Encoding encoding;
		private MemoryStream ms;
		private string boundary;
		private byte[] formData;
		public byte[] FormData
		{
			get
			{
				if (this.formData == null)
				{
					byte[] bytes = this.encoding.GetBytes("--" + this.boundary + "--\r\n");
					this.ms.Write(bytes, 0, bytes.Length);
					this.formData = this.ms.ToArray();
				}
				return this.formData;
			}
		}
		public string ContentType
		{
			get
			{
				return string.Format("multipart/form-data; boundary={0}", this.boundary);
			}
		}
		public Encoding StringEncoding
		{
			get
			{
				return this.encoding;
			}
			set
			{
				this.encoding = value;
			}
		}
		public MultipartForm()
		{
			this.boundary = string.Format("--{0}--", Guid.NewGuid());
			this.ms = new MemoryStream();
			this.encoding = Encoding.Default;
		}
		public void AddFlie(string name, string filename)
		{
			if (!File.Exists(filename))
			{
				throw new FileNotFoundException("尝试添加不存在的文件。", filename);
			}
			FileStream fileStream = null;
			byte[] array = new byte[0];
			try
			{
				fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
				array = new byte[fileStream.Length];
				fileStream.Read(array, 0, array.Length);
				this.AddFlie(name, Path.GetFileName(filename), array, array.Length);
			}
			catch (Exception)
			{
				throw;
			}
			finally
			{
				if (fileStream != null)
				{
					fileStream.Close();
				}
			}
		}
		public void AddFlie(string name, string filename, byte[] fileData, int dataLength)
		{
			if (dataLength <= 0 || dataLength > fileData.Length)
			{
				dataLength = fileData.Length;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("--{0}\r\n", this.boundary);
			stringBuilder.AppendFormat("Content-Disposition: form-data; name=\"{0}\";filename=\"{1}\"\r\n", name, filename);
			stringBuilder.AppendFormat("Content-Type: {0}\r\n", this.GetContentType(filename));
			stringBuilder.Append("\r\n");
			byte[] bytes = this.encoding.GetBytes(stringBuilder.ToString());
			this.ms.Write(bytes, 0, bytes.Length);
			this.ms.Write(fileData, 0, dataLength);
			byte[] bytes2 = this.encoding.GetBytes("\r\n");
			this.ms.Write(bytes2, 0, bytes2.Length);
		}
		public void AddString(string name, string value)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("--{0}\r\n", this.boundary);
			stringBuilder.AppendFormat("Content-Disposition: form-data; name=\"{0}\"\r\n", name);
			stringBuilder.Append("\r\n");
			stringBuilder.AppendFormat("{0}\r\n", value);
			byte[] bytes = this.encoding.GetBytes(stringBuilder.ToString());
			this.ms.Write(bytes, 0, bytes.Length);
		}
		private string GetContentType(string filename)
		{
			RegistryKey registryKey = null;
			string text = "application/stream";
			try
			{
				registryKey = Registry.ClassesRoot.OpenSubKey(Path.GetExtension(filename));
				text = registryKey.GetValue("Content Type", text).ToString();
			}
			finally
			{
				if (registryKey != null)
				{
					registryKey.Close();
				}
			}
			return text;
		}
	}
}
