using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Cache;
using System.Net.Security;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography.X509Certificates;
using System.Text;
namespace Maticsoft.Common
{
	public class WebClient
	{
		private Encoding encoding = Encoding.Default;
		private string respHtml = "";
		private WebProxy proxy;
		private static CookieContainer cc;
		private WebHeaderCollection requestHeaders;
		private WebHeaderCollection responseHeaders;
		private int bufferSize = 15240;
		public event EventHandler<UploadEventArgs> UploadProgressChanged;
		public event EventHandler<DownloadEventArgs> DownloadProgressChanged;
		public int BufferSize
		{
			get
			{
				return this.bufferSize;
			}
			set
			{
				this.bufferSize = value;
			}
		}
		public WebHeaderCollection ResponseHeaders
		{
			get
			{
				return this.responseHeaders;
			}
		}
		public WebHeaderCollection RequestHeaders
		{
			get
			{
				return this.requestHeaders;
			}
		}
		public WebProxy Proxy
		{
			get
			{
				return this.proxy;
			}
			set
			{
				this.proxy = value;
			}
		}
		public Encoding Encoding
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
		public string RespHtml
		{
			get
			{
				return this.respHtml;
			}
			set
			{
				this.respHtml = value;
			}
		}
		public CookieContainer CookieContainer
		{
			get
			{
				return WebClient.cc;
			}
			set
			{
				WebClient.cc = value;
			}
		}
		static WebClient()
		{
			WebClient.LoadCookiesFromDisk();
		}
		public WebClient()
		{
			this.requestHeaders = new WebHeaderCollection();
			this.responseHeaders = new WebHeaderCollection();
		}
		public string GetHtml(string url)
		{
			HttpWebRequest request = this.CreateRequest(url, "GET");
			this.respHtml = this.encoding.GetString(this.GetData(request));
			return this.respHtml;
		}
		public void DownloadFile(string url, string filename)
		{
			FileStream fileStream = null;
			try
			{
				HttpWebRequest request = this.CreateRequest(url, "GET");
				byte[] data = this.GetData(request);
				fileStream = new FileStream(filename, FileMode.Create, FileAccess.Write);
				fileStream.Write(data, 0, data.Length);
			}
			finally
			{
				if (fileStream != null)
				{
					fileStream.Close();
				}
			}
		}
		public byte[] GetData(string url)
		{
			HttpWebRequest request = this.CreateRequest(url, "GET");
			return this.GetData(request);
		}
		public string Post(string url, string postData)
		{
			byte[] bytes = this.encoding.GetBytes(postData);
			return this.Post(url, bytes);
		}
		public string Post(string url, byte[] postData)
		{
			HttpWebRequest httpWebRequest = this.CreateRequest(url, "POST");
			httpWebRequest.ContentType = "application/x-www-form-urlencoded";
			httpWebRequest.ContentLength = (long)postData.Length;
			httpWebRequest.KeepAlive = true;
			this.PostData(httpWebRequest, postData);
			this.respHtml = this.encoding.GetString(this.GetData(httpWebRequest));
			return this.respHtml;
		}
		public string Post(string url, MultipartForm mulitpartForm)
		{
			HttpWebRequest httpWebRequest = this.CreateRequest(url, "POST");
			httpWebRequest.ContentType = mulitpartForm.ContentType;
			httpWebRequest.ContentLength = (long)mulitpartForm.FormData.Length;
			httpWebRequest.KeepAlive = true;
			this.PostData(httpWebRequest, mulitpartForm.FormData);
			this.respHtml = this.encoding.GetString(this.GetData(httpWebRequest));
			return this.respHtml;
		}
		private byte[] GetData(HttpWebRequest request)
		{
			HttpWebResponse httpWebResponse = (HttpWebResponse)request.GetResponse();
			Stream responseStream = httpWebResponse.GetResponseStream();
			this.responseHeaders = httpWebResponse.Headers;
			DownloadEventArgs downloadEventArgs = new DownloadEventArgs();
			if (this.responseHeaders[HttpResponseHeader.ContentLength] != null)
			{
				downloadEventArgs.TotalBytes = Convert.ToInt32(this.responseHeaders[HttpResponseHeader.ContentLength]);
			}
			MemoryStream memoryStream = new MemoryStream();
			byte[] array = new byte[this.bufferSize];
			int num;
			while ((num = responseStream.Read(array, 0, array.Length)) > 0)
			{
				memoryStream.Write(array, 0, num);
				if (this.DownloadProgressChanged != null)
				{
					downloadEventArgs.BytesReceived += num;
					downloadEventArgs.ReceivedData = new byte[num];
					Array.Copy(array, downloadEventArgs.ReceivedData, num);
					this.DownloadProgressChanged(this, downloadEventArgs);
				}
			}
			responseStream.Close();
			if (this.ResponseHeaders[HttpResponseHeader.ContentEncoding] != null)
			{
				MemoryStream memoryStream2 = new MemoryStream();
				array = new byte[100];
				string a;
				if ((a = this.ResponseHeaders[HttpResponseHeader.ContentEncoding].ToLower()) != null)
				{
					if (a == "gzip")
					{
						GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress);
						while ((num = gZipStream.Read(array, 0, array.Length)) > 0)
						{
							memoryStream2.Write(array, 0, num);
						}
						return memoryStream2.ToArray();
					}
					if (a == "deflate")
					{
						DeflateStream deflateStream = new DeflateStream(memoryStream, CompressionMode.Decompress);
						while ((num = deflateStream.Read(array, 0, array.Length)) > 0)
						{
							memoryStream2.Write(array, 0, num);
						}
						return memoryStream2.ToArray();
					}
				}
			}
			return memoryStream.ToArray();
		}
		private void PostData(HttpWebRequest request, byte[] postData)
		{
			int num = 0;
			int num2 = this.bufferSize;
			Stream requestStream = request.GetRequestStream();
			UploadEventArgs uploadEventArgs = new UploadEventArgs();
			uploadEventArgs.TotalBytes = postData.Length;
			int num3;
			while ((num3 = postData.Length - num) > 0)
			{
				if (num2 > num3)
				{
					num2 = num3;
				}
				requestStream.Write(postData, num, num2);
				num += num2;
				if (this.UploadProgressChanged != null)
				{
					uploadEventArgs.BytesSent = num;
					this.UploadProgressChanged(this, uploadEventArgs);
				}
			}
			requestStream.Close();
		}
		private HttpWebRequest CreateRequest(string url, string method)
		{
			Uri uri = new Uri(url);
			if (uri.Scheme == "https")
			{
				ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(this.CheckValidationResult);
			}
			HttpRequestCachePolicy defaultCachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.Revalidate);
			HttpWebRequest.DefaultCachePolicy = defaultCachePolicy;
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
			httpWebRequest.AllowAutoRedirect = false;
			httpWebRequest.AllowWriteStreamBuffering = false;
			httpWebRequest.Method = method;
			if (this.proxy != null)
			{
				httpWebRequest.Proxy = this.proxy;
			}
			httpWebRequest.CookieContainer = WebClient.cc;
			foreach (string name in this.requestHeaders.Keys)
			{
				httpWebRequest.Headers.Add(name, this.requestHeaders[name]);
			}
			this.requestHeaders.Clear();
			return httpWebRequest;
		}
		private bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
		{
			return true;
		}
		private static void SaveCookiesToDisk()
		{
			string path = Environment.GetFolderPath(Environment.SpecialFolder.Cookies) + "\\webclient.cookie";
			FileStream fileStream = null;
			try
			{
				fileStream = new FileStream(path, FileMode.Create);
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				binaryFormatter.Serialize(fileStream, WebClient.cc);
			}
			finally
			{
				if (fileStream != null)
				{
					fileStream.Close();
				}
			}
		}
		private static void LoadCookiesFromDisk()
		{
			WebClient.cc = new CookieContainer();
			string path = Environment.GetFolderPath(Environment.SpecialFolder.Cookies) + "\\webclient.cookie";
			if (!File.Exists(path))
			{
				return;
			}
			FileStream fileStream = null;
			try
			{
				fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				WebClient.cc = (CookieContainer)binaryFormatter.Deserialize(fileStream);
			}
			finally
			{
				if (fileStream != null)
				{
					fileStream.Close();
				}
			}
		}
	}
}
