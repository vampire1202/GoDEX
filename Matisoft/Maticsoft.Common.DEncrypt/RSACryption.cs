using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
namespace Maticsoft.Common.DEncrypt
{
	public class RSACryption
	{
		public void RSAKey(out string xmlKeys, out string xmlPublicKey)
		{
			RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
			xmlKeys = rSACryptoServiceProvider.ToXmlString(true);
			xmlPublicKey = rSACryptoServiceProvider.ToXmlString(false);
		}
		public string RSAEncrypt(string xmlPublicKey, string m_strEncryptString)
		{
			RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
			rSACryptoServiceProvider.FromXmlString(xmlPublicKey);
			byte[] bytes = new UnicodeEncoding().GetBytes(m_strEncryptString);
			byte[] inArray = rSACryptoServiceProvider.Encrypt(bytes, false);
			return Convert.ToBase64String(inArray);
		}
		public string RSAEncrypt(string xmlPublicKey, byte[] EncryptString)
		{
			RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
			rSACryptoServiceProvider.FromXmlString(xmlPublicKey);
			byte[] inArray = rSACryptoServiceProvider.Encrypt(EncryptString, false);
			return Convert.ToBase64String(inArray);
		}
		public string RSADecrypt(string xmlPrivateKey, string m_strDecryptString)
		{
			RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
			rSACryptoServiceProvider.FromXmlString(xmlPrivateKey);
			byte[] rgb = Convert.FromBase64String(m_strDecryptString);
			byte[] bytes = rSACryptoServiceProvider.Decrypt(rgb, false);
			return new UnicodeEncoding().GetString(bytes);
		}
		public string RSADecrypt(string xmlPrivateKey, byte[] DecryptString)
		{
			RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
			rSACryptoServiceProvider.FromXmlString(xmlPrivateKey);
			byte[] bytes = rSACryptoServiceProvider.Decrypt(DecryptString, false);
			return new UnicodeEncoding().GetString(bytes);
		}
		public bool GetHash(string m_strSource, ref byte[] HashData)
		{
			HashAlgorithm hashAlgorithm = HashAlgorithm.Create("MD5");
			byte[] bytes = Encoding.GetEncoding("GB2312").GetBytes(m_strSource);
			HashData = hashAlgorithm.ComputeHash(bytes);
			return true;
		}
		public bool GetHash(string m_strSource, ref string strHashData)
		{
			HashAlgorithm hashAlgorithm = HashAlgorithm.Create("MD5");
			byte[] bytes = Encoding.GetEncoding("GB2312").GetBytes(m_strSource);
			byte[] inArray = hashAlgorithm.ComputeHash(bytes);
			strHashData = Convert.ToBase64String(inArray);
			return true;
		}
		public bool GetHash(FileStream objFile, ref byte[] HashData)
		{
			HashAlgorithm hashAlgorithm = HashAlgorithm.Create("MD5");
			HashData = hashAlgorithm.ComputeHash(objFile);
			objFile.Close();
			return true;
		}
		public bool GetHash(FileStream objFile, ref string strHashData)
		{
			HashAlgorithm hashAlgorithm = HashAlgorithm.Create("MD5");
			byte[] inArray = hashAlgorithm.ComputeHash(objFile);
			objFile.Close();
			strHashData = Convert.ToBase64String(inArray);
			return true;
		}
		public bool SignatureFormatter(string p_strKeyPrivate, byte[] HashbyteSignature, ref byte[] EncryptedSignatureData)
		{
			RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
			rSACryptoServiceProvider.FromXmlString(p_strKeyPrivate);
			RSAPKCS1SignatureFormatter rSAPKCS1SignatureFormatter = new RSAPKCS1SignatureFormatter(rSACryptoServiceProvider);
			rSAPKCS1SignatureFormatter.SetHashAlgorithm("MD5");
			EncryptedSignatureData = rSAPKCS1SignatureFormatter.CreateSignature(HashbyteSignature);
			return true;
		}
		public bool SignatureFormatter(string p_strKeyPrivate, byte[] HashbyteSignature, ref string m_strEncryptedSignatureData)
		{
			RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
			rSACryptoServiceProvider.FromXmlString(p_strKeyPrivate);
			RSAPKCS1SignatureFormatter rSAPKCS1SignatureFormatter = new RSAPKCS1SignatureFormatter(rSACryptoServiceProvider);
			rSAPKCS1SignatureFormatter.SetHashAlgorithm("MD5");
			byte[] inArray = rSAPKCS1SignatureFormatter.CreateSignature(HashbyteSignature);
			m_strEncryptedSignatureData = Convert.ToBase64String(inArray);
			return true;
		}
		public bool SignatureFormatter(string p_strKeyPrivate, string m_strHashbyteSignature, ref byte[] EncryptedSignatureData)
		{
			byte[] rgbHash = Convert.FromBase64String(m_strHashbyteSignature);
			RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
			rSACryptoServiceProvider.FromXmlString(p_strKeyPrivate);
			RSAPKCS1SignatureFormatter rSAPKCS1SignatureFormatter = new RSAPKCS1SignatureFormatter(rSACryptoServiceProvider);
			rSAPKCS1SignatureFormatter.SetHashAlgorithm("MD5");
			EncryptedSignatureData = rSAPKCS1SignatureFormatter.CreateSignature(rgbHash);
			return true;
		}
		public bool SignatureFormatter(string p_strKeyPrivate, string m_strHashbyteSignature, ref string m_strEncryptedSignatureData)
		{
			byte[] rgbHash = Convert.FromBase64String(m_strHashbyteSignature);
			RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
			rSACryptoServiceProvider.FromXmlString(p_strKeyPrivate);
			RSAPKCS1SignatureFormatter rSAPKCS1SignatureFormatter = new RSAPKCS1SignatureFormatter(rSACryptoServiceProvider);
			rSAPKCS1SignatureFormatter.SetHashAlgorithm("MD5");
			byte[] inArray = rSAPKCS1SignatureFormatter.CreateSignature(rgbHash);
			m_strEncryptedSignatureData = Convert.ToBase64String(inArray);
			return true;
		}
		public bool SignatureDeformatter(string p_strKeyPublic, byte[] HashbyteDeformatter, byte[] DeformatterData)
		{
			RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
			rSACryptoServiceProvider.FromXmlString(p_strKeyPublic);
			RSAPKCS1SignatureDeformatter rSAPKCS1SignatureDeformatter = new RSAPKCS1SignatureDeformatter(rSACryptoServiceProvider);
			rSAPKCS1SignatureDeformatter.SetHashAlgorithm("MD5");
			return rSAPKCS1SignatureDeformatter.VerifySignature(HashbyteDeformatter, DeformatterData);
		}
		public bool SignatureDeformatter(string p_strKeyPublic, string p_strHashbyteDeformatter, byte[] DeformatterData)
		{
			byte[] rgbHash = Convert.FromBase64String(p_strHashbyteDeformatter);
			RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
			rSACryptoServiceProvider.FromXmlString(p_strKeyPublic);
			RSAPKCS1SignatureDeformatter rSAPKCS1SignatureDeformatter = new RSAPKCS1SignatureDeformatter(rSACryptoServiceProvider);
			rSAPKCS1SignatureDeformatter.SetHashAlgorithm("MD5");
			return rSAPKCS1SignatureDeformatter.VerifySignature(rgbHash, DeformatterData);
		}
		public bool SignatureDeformatter(string p_strKeyPublic, byte[] HashbyteDeformatter, string p_strDeformatterData)
		{
			RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
			rSACryptoServiceProvider.FromXmlString(p_strKeyPublic);
			RSAPKCS1SignatureDeformatter rSAPKCS1SignatureDeformatter = new RSAPKCS1SignatureDeformatter(rSACryptoServiceProvider);
			rSAPKCS1SignatureDeformatter.SetHashAlgorithm("MD5");
			byte[] rgbSignature = Convert.FromBase64String(p_strDeformatterData);
			return rSAPKCS1SignatureDeformatter.VerifySignature(HashbyteDeformatter, rgbSignature);
		}
		public bool SignatureDeformatter(string p_strKeyPublic, string p_strHashbyteDeformatter, string p_strDeformatterData)
		{
			byte[] rgbHash = Convert.FromBase64String(p_strHashbyteDeformatter);
			RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
			rSACryptoServiceProvider.FromXmlString(p_strKeyPublic);
			RSAPKCS1SignatureDeformatter rSAPKCS1SignatureDeformatter = new RSAPKCS1SignatureDeformatter(rSACryptoServiceProvider);
			rSAPKCS1SignatureDeformatter.SetHashAlgorithm("MD5");
			byte[] rgbSignature = Convert.FromBase64String(p_strDeformatterData);
			return rSAPKCS1SignatureDeformatter.VerifySignature(rgbHash, rgbSignature);
		}
	}
}
