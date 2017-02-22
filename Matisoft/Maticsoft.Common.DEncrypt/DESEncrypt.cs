using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;
namespace Maticsoft.Common.DEncrypt
{
	public class DESEncrypt
	{
		public static string Encrypt(string Text)
		{
			return DESEncrypt.Encrypt(Text, "litianping");
		}
		public static string Encrypt(string Text, string sKey)
		{
			DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
			byte[] bytes = Encoding.Default.GetBytes(Text);
			dESCryptoServiceProvider.Key = Encoding.ASCII.GetBytes(FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
			dESCryptoServiceProvider.IV = Encoding.ASCII.GetBytes(FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
			MemoryStream memoryStream = new MemoryStream();
			CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateEncryptor(), CryptoStreamMode.Write);
			cryptoStream.Write(bytes, 0, bytes.Length);
			cryptoStream.FlushFinalBlock();
			StringBuilder stringBuilder = new StringBuilder();
			byte[] array = memoryStream.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				byte b = array[i];
				stringBuilder.AppendFormat("{0:X2}", b);
			}
			return stringBuilder.ToString();
		}
		public static string Encrypt(string Text, out string key, out string iv)
		{
			key = "";
			iv = "";
			DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
			byte[] bytes = Encoding.Default.GetBytes(Text);
			dESCryptoServiceProvider.GenerateIV();
			dESCryptoServiceProvider.GenerateKey();
			iv = new UnicodeEncoding().GetString(dESCryptoServiceProvider.IV);
			key = new UnicodeEncoding().GetString(dESCryptoServiceProvider.Key);
			MemoryStream memoryStream = new MemoryStream();
			CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateEncryptor(), CryptoStreamMode.Write);
			cryptoStream.Write(bytes, 0, bytes.Length);
			cryptoStream.FlushFinalBlock();
			StringBuilder stringBuilder = new StringBuilder();
			byte[] array = memoryStream.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				byte b = array[i];
				stringBuilder.AppendFormat("{0:X2}", b);
			}
			return stringBuilder.ToString();
		}
		public static string Decrypt(string Text)
		{
			return DESEncrypt.Decrypt(Text, "litianping");
		}
		public static string Decrypt(string Text, string sKey)
		{
			DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
			int num = Text.Length / 2;
			byte[] array = new byte[num];
			for (int i = 0; i < num; i++)
			{
				int num2 = Convert.ToInt32(Text.Substring(i * 2, 2), 16);
				array[i] = (byte)num2;
			}
			dESCryptoServiceProvider.Key = Encoding.ASCII.GetBytes(FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
			dESCryptoServiceProvider.IV = Encoding.ASCII.GetBytes(FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
			MemoryStream memoryStream = new MemoryStream();
			CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateDecryptor(), CryptoStreamMode.Write);
			cryptoStream.Write(array, 0, array.Length);
			cryptoStream.FlushFinalBlock();
			return Encoding.Default.GetString(memoryStream.ToArray());
		}
		public static string Decrypt(string Text, string key, string iv)
		{
			byte[] bytes = new UnicodeEncoding().GetBytes(key);
			byte[] bytes2 = new UnicodeEncoding().GetBytes(iv);
			DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
			int num = Text.Length / 2;
			byte[] array = new byte[num];
			for (int i = 0; i < num; i++)
			{
				int num2 = Convert.ToInt32(Text.Substring(i * 2, 2), 16);
				array[i] = (byte)num2;
			}
			dESCryptoServiceProvider.Key = bytes;
			dESCryptoServiceProvider.IV = bytes2;
			MemoryStream memoryStream = new MemoryStream();
			CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateDecryptor(), CryptoStreamMode.Write);
			cryptoStream.Write(array, 0, array.Length);
			cryptoStream.FlushFinalBlock();
			return Encoding.Default.GetString(memoryStream.ToArray());
		}
	}
}
