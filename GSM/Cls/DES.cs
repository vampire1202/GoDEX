using System;
using System.Text;
using System.Globalization;
using System.Security.Cryptography;
using System.Windows.Forms;
namespace GSM.Cls
{
    class DES
    {
        // 创建Key
        public string GenerateKey()
        {
            DESCryptoServiceProvider desCrypto = (DESCryptoServiceProvider)DESCryptoServiceProvider.Create();
            return ASCIIEncoding.ASCII.GetString(desCrypto.Key);
        }
        // 加密字符串
        public string EncryptString(string sInputString, string sKey)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(sInputString);
                DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
                DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                ICryptoTransform desencrypt = DES.CreateEncryptor();
                byte[] result = desencrypt.TransformFinalBlock(data, 0, data.Length);
                return BitConverter.ToString(result);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
                return null;
            }
        }

        // 解密字符串
        public string DecryptString(string sInputString, string sKey)
        {
            try
            {
                string[] sInput = sInputString.Split("-".ToCharArray());
                byte[] data = new byte[sInput.Length];
                for (int i = 0; i < sInput.Length; i++)
                {
                    data[i] = byte.Parse(sInput[i], NumberStyles.HexNumber);
                }
                DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
                DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                ICryptoTransform desencrypt = DES.CreateDecryptor();
                byte[] result = desencrypt.TransformFinalBlock(data, 0, data.Length);
                return Encoding.UTF8.GetString(result);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
                return null;
            }

        }
    }
}
