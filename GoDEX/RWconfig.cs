using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Deployment;
using System.Xml;
using System.Xml.Schema; 
using System.Reflection;
using System.IO;
using System.Windows;
using System.Reflection;

namespace GoDex
{
    class RWconfig
    {
        /// <summary>
        /// 修改配置文件(数据库连接字符串)
        /// </summary>
        /// <param name="connString"></param>


        private static int GetIndex(XmlElement xe)
        {
            return xe.GetAttribute("name").IndexOf("SMSRouter");
        }



        public static string GetConnectionStringsConfig(string connectionName)
        {
            try
            {
                string connectionString =
                        ConfigurationManager.ConnectionStrings[connectionName].ConnectionString.ToString();
                Console.WriteLine(connectionString);
                return connectionString;
            }
            catch
            {
                return "";
            }
        }

        public static string GetAppSettings(string SettingName)
        {
            return ConfigurationManager.AppSettings[SettingName].ToString();
        }

        public static void SetAppSettings(string AppKey, string AppValue)
        {
            System.Configuration.ConfigurationManager.AppSettings.Set(AppKey, AppValue);
            XmlDocument doc = new XmlDocument();
            doc.Load(Assembly.GetExecutingAssembly().Location + ".config");
            XmlNode node = doc.SelectSingleNode(@"//add[@key='" + AppKey + "']");
            XmlElement ele = (XmlElement)node;
            if (node != null)
                ele.SetAttribute("value", AppValue);
            else
            {
                XmlElement xElem2;
                xElem2 = doc.CreateElement("add");
                xElem2.SetAttribute("key", AppKey);
                xElem2.SetAttribute("value", AppValue);
                node.AppendChild(xElem2);
            }
            doc.Save(Assembly.GetExecutingAssembly().Location + ".config");
        }
    }
}
