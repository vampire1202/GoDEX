using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Deployment;
using System.Xml;
using System.Xml.Schema;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

namespace GSM.Cls
{
    class RWconfig
    {
        /// <summary>
        /// 修改配置文件(数据库连接字符串)
        /// </summary>
        /// <param name="connString"></param>
        public static void UpdateConfig(string databaseIP, string saName, string saPassword)
        {
            try
            {
                string m_strFullPath = "";
                Assembly Asm = Assembly.GetExecutingAssembly();
                XmlDocument xmlDoc = new XmlDocument();

                m_strFullPath = Application.ExecutablePath + ".config";
                xmlDoc.Load(m_strFullPath);

                XmlNodeList nodeList = xmlDoc.SelectSingleNode("/configuration/connectionStrings").ChildNodes;
                foreach (XmlNode xn in nodeList)//遍历所有子节点
                {
                    XmlElement xe = (XmlElement)xn;
                    string LastestValue = "Data Source=" + databaseIP + ";Initial Catalog=db_wegoPACS_nkj;User ID=" + saName + ";Password=" + saPassword;
                    xe.SetAttribute("connectionString", LastestValue);
                }

                XmlNodeList nodeList1 = xmlDoc.SelectSingleNode("/configuration/DatabaseUnitTesting").ChildNodes;
                foreach (XmlNode xn in nodeList1)
                {
                    XmlElement xe = (XmlElement)xn;
                    string connValue = "Data Source=" + databaseIP + ";Initial Catalog=db_wegoPACS_nkj;User ID=" + saName + ";Pooling=False";// "Data Source=" + databaseIP + ";Initial Catalog=db_wegoPACS_nkj;User ID=" + saName + ";Password=" + saPassword;
                    xe.SetAttribute("ConnectionString", connValue);
                }

                xmlDoc.Save(m_strFullPath);
            }
            catch (System.NullReferenceException NullEx)
            {
                throw NullEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

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


        public static void UpdateConnectionStringsConfig(string newName, string newConString, string newProviderName)
        {
            bool isModified = false;
            if (ConfigurationManager.ConnectionStrings[newName] != null)
            {
                isModified = true;
            }
            ConnectionStringSettings mySettings =
                new ConnectionStringSettings(newName, newConString, newProviderName);
            Configuration config =
                ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);//ConfigurationUserLevel.None
            if (isModified)
            {
                config.ConnectionStrings.ConnectionStrings.Remove(newName);
            }
            config.ConnectionStrings.ConnectionStrings.Add(mySettings);
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("ConnectionStrings");
        }

        public static string GetAppSettings(string SettingName)
        {
            return ConfigurationManager.AppSettings[SettingName].ToString();
        }

        public static void SetAppSettings(string AppKey, string AppValue)
        {
            System.Configuration.ConfigurationManager.AppSettings.Set(AppKey, AppValue);
            XmlDocument doc = new XmlDocument();
            doc.Load(Application.ExecutablePath + ".config");
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
            doc.Save(Application.ExecutablePath + ".config");
        }
    }
}
