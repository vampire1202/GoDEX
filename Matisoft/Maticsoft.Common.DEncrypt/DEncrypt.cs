using System;
using System.Security.Cryptography;
using System.Text;
namespace Maticsoft.Common.DEncrypt
{
	public class DEncrypt
	{
		public static string Encrypt(string original)
		{
			return DEncrypt.Encrypt(original, "LITIANPING");
		}
		public static string Decrypt(string original)
		{
			return DEncrypt.Decrypt(original, "LITIANPING", Encoding.Default);
		}
		public static string Encrypt(string original, string key)
		{
			byte[] bytes = Encoding.Default.GetBytes(original);
			byte[] bytes2 = Encoding.Default.GetBytes(key);
			return Convert.ToBase64String(DEncrypt.Encrypt(bytes, bytes2));
		}
		public static string Decrypt(string original, string key)
		{
			return DEncrypt.Decrypt(original, key, Encoding.Default);
		}
		public static string Decrypt(string encrypted, string key, Encoding encoding)
		{
			byte[] encrypted2 = Convert.FromBase64String(encrypted);
			byte[] bytes = Encoding.Default.GetBytes(key);
			return encoding.GetString(DEncrypt.Decrypt(encrypted2, bytes));
		}
		public static byte[] Decrypt(byte[] encrypted)
		{
			byte[] bytes = Encoding.Default.GetBytes("LITIANPING");
			return DEncrypt.Decrypt(encrypted, bytes);
		}
		public static byte[] Encrypt(byte[] original)
		{
			byte[] bytes = Encoding.Default.GetBytes("LITIANPING");
			return DEncrypt.Encrypt(original, bytes);
		}
		public static byte[] MakeMD5(byte[] original)
		{
			MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
			return mD5CryptoServiceProvider.ComputeHash(original);
		}
		public static byte[] Encrypt(byte[] original, byte[] key)
		{
			return new TripleDESCryptoServiceProvider
			{
				Key = DEncrypt.MakeMD5(key),
				Mode = CipherMode.ECB
			}.CreateEncryptor().TransformFinalBlock(original, 0, original.Length);
		}
		public static byte[] Decrypt(byte[] encrypted, byte[] key)
		{
			return new TripleDESCryptoServiceProvider
			{
				Key = DEncrypt.MakeMD5(key),
				Mode = CipherMode.ECB
			}.CreateDecryptor().TransformFinalBlock(encrypted, 0, encrypted.Length);
		}
	}
}
