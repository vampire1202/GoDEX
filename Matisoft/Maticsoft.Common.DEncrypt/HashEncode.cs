using System;
using System.Security.Cryptography;
using System.Text;
namespace Maticsoft.Common.DEncrypt
{
	public class HashEncode
	{
		public static string GetSecurity()
		{
			return HashEncode.HashEncoding(HashEncode.GetRandomValue());
		}
		public static string GetRandomValue()
		{
			Random random = new Random();
			return random.Next(1, 2147483647).ToString();
		}
		public static string HashEncoding(string Security)
		{
			UnicodeEncoding unicodeEncoding = new UnicodeEncoding();
			byte[] bytes = unicodeEncoding.GetBytes(Security);
			SHA512Managed sHA512Managed = new SHA512Managed();
			byte[] array = sHA512Managed.ComputeHash(bytes);
			Security = "";
			byte[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				byte b = array2[i];
				Security = Security + (int)b + "O";
			}
			return Security;
		}
	}
}
