using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Security.Cryptography;
using Microsoft.Win32;
using System.Configuration;
using System.Management;
using System.Management.Instrumentation;
using System.IO;

namespace GSM.Cls
{
    class SoftRegister
    {
        ///   <summary>    
        ///   获取硬盘ID        
        ///   </summary>    
        ///   <returns> string </returns>    
        public static string GetHDid()
        {
            string HDid = " ";
            using (ManagementClass cimobject1 = new ManagementClass("Win32_DiskDrive"))
            {
                ManagementObjectCollection moc1 = cimobject1.GetInstances();
                foreach (ManagementObject mo in moc1)
                {
                    HDid = (string)mo.Properties["Model"].Value;
                    mo.Dispose();
                }
            }
            return HDid.ToString();
        }

        ///   <summary>    
        ///   获取网卡硬件地址    
        ///   </summary>    
        ///   <returns> string </returns>    
        public static string GetMoAddress()
        {
            string MoAddress = " ";
            using (ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration"))
            {
                ManagementObjectCollection moc2 = mc.GetInstances();
                foreach (ManagementObject mo in moc2)
                {
                    if ((bool)mo["IPEnabled"] == true)
                        MoAddress = mo["MacAddress"].ToString().Replace(":", "");
                    mo.Dispose();
                }
            }
            return MoAddress.ToString();
        }

        // 取得设备硬盘的卷标号
        public static string GetDiskVolumeSerialNumber()
        {
            ManagementObject disk = new ManagementObject("win32_logicaldisk.deviceid=\"c:\"");
            disk.Get();
            return disk.GetPropertyValue("VolumeSerialNumber").ToString();
        }

        //获得CPU的序列号
        public static string getCpu()
        {
            string strCpu = null;
            ManagementClass myCpu = new ManagementClass("win32_Processor");
            ManagementObjectCollection myCpuConnection = myCpu.GetInstances();
            foreach (ManagementObject myObject in myCpuConnection)
            {
                strCpu = myObject.Properties["Processorid"].Value.ToString();
                break;
            }
            return strCpu;
        }
        //生成机器码
        public static string getMNum()
        {
            string strNum =  GetMoAddress() + getCpu() +GetDiskVolumeSerialNumber();//获得24位Cpu和硬盘序列号  GetHDid() +

            //string strMNum = strNum.Substring(0, 24);//从生成的字符串中取出前24个字符做为机器码
            //return strMNum;
            return strNum.Replace(" ", "").Replace("-", "");

        }


        public static int[] intCode = new int[127];//存储密钥
        public static int[] intNumber = new int[25];//存机器码的Ascii值
        public static char[] Charcode = new char[25];//存储机器码字

        public static void setIntCode()//给数组赋值小于10的数
        {
            for (int i = 1; i < intCode.Length; i++)
            {
                intCode[i] = i % 9;
            }
        }

        ////生成注册码      

        //public static string getRNum()
        //{
        //    setIntCode();  //初始化127位数组
        //    for (int i = 1; i < Charcode.Length; i++)//把机器码存入数组中
        //    {
        //        Charcode[i] = Convert.ToChar(getMNum().Substring(i - 1, 1));
        //    }
        //    for (int j = 1; j < intNumber.Length; j++)//把字符的ASCII值存入一个整数组中。
        //    {
        //        intNumber[j] = intCode[Convert.ToInt32(Charcode[j])] + Convert.ToInt32(Charcode[j]);
        //    }
        //    string strAsciiName = "";  //用于存储注册码
        //    for (int j = 1; j < intNumber.Length; j++)
        //    {
        //        if (intNumber[j] >= 48 && intNumber[j] <= 57)//判断字符ASCII值是否0－9之间
        //        {
        //            strAsciiName += Convert.ToChar(intNumber[j]).ToString();
        //        }
        //        else if (intNumber[j] >= 65 && intNumber[j] <= 90)//判断字符ASCII值是否A－Z之间
        //        {
        //            strAsciiName += Convert.ToChar(intNumber[j]).ToString();
        //        }
        //        else if (intNumber[j] >= 97 && intNumber[j] <= 122)//判断字符ASCII值是否a－z之间
        //        {
        //            strAsciiName += Convert.ToChar(intNumber[j]).ToString();
        //        }
        //        else//判断字符ASCII值不在以上范围内
        //        {
        //            if (intNumber[j] > 122)//判断字符ASCII值是否大于z
        //            {
        //                strAsciiName += Convert.ToChar(intNumber[j] - 10).ToString();
        //            }
        //            else
        //            {
        //                strAsciiName += Convert.ToChar(intNumber[j] - 9).ToString();
        //            }
        //        }
        //    }
        //    return strAsciiName;
        //}

        ///MD5加密
        public static string MD5Encrypt(string pToEncrypt, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);
            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream CS = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            CS.Write(inputByteArray, 0, inputByteArray.Length);
            CS.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            ret.ToString();
            return ret.ToString();
        }

        ///MD5解密
        public static  string MD5Decrypt(string pToDecrypt, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
            for (int x = 0; x < pToDecrypt.Length / 2; x++)
            {
                int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }

            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream CS = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            CS.Write(inputByteArray, 0, inputByteArray.Length);
            CS.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            return System.Text.Encoding.Default.GetString(ms.ToArray());
        }

        public static string getMd5(string password)
        {
            byte[] pasArray = System.Text.Encoding.Default.GetBytes(password);
            pasArray = new System.Security.Cryptography.MD5CryptoServiceProvider().ComputeHash(pasArray);
            string rMd5Str = "";
            foreach (byte ibyte in pasArray)
                rMd5Str += ibyte.ToString("x").PadLeft(2, '0');
            return rMd5Str;
        }
    }
}
