using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Windows.Forms;
using System.IO;

namespace GSM.Cls
{
    public class Method
    {
        public static bool IsNumeric(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$");
        }

        public static bool IsInt(string value)
        {
            return Regex.IsMatch(value, @"^[1-9]{1}[\d]*$");
        }

        public static void writeLogData(int nodeID, string tipInfo, string userRole, DateTime acDateTime, string sign)
        {
            try
            {
                GoDexData.BLL.log bllLog = new GoDexData.BLL.log();
                GoDexData.Model.log modelLog = new GoDexData.Model.log();
                modelLog.machineNo = nodeID;
                modelLog.action = tipInfo;
                modelLog.userName = userRole;
                modelLog.acDateTime = DateTime.Now;
                bllLog.Add(modelLog);
            }
            catch { }
        } 

        // 把十六进制字符串转换成字节型和把字节型转换成十六进制字符串 converter hex string to byte and byte to hex string 
        public static string ByteToString(byte[] InBytes)
        {
            string StringOut = "";
            foreach (byte InByte in InBytes)
            {
                StringOut = StringOut + String.Format("{0:X2} ", InByte);
            }
            return StringOut;
        }

        //特殊转换
        public static double TenTo16(string strNumber)
        {
            double int16 = 0;
            int j = strNumber.Length;

            for (int i = 0; i < j; i++)
            {
                int16 += Convert.ToDouble(strNumber.Substring(i, 1)) * (System.Math.Pow(16, j - i - 1));
            }
            return int16;
        }

        //特殊转换
        public static int AscTo16(string strNumber)
        {
            return (int.Parse(strNumber) / 16) * 10 + (int.Parse(strNumber) % 16);
        }

        public static long strTo16(string str)
        {
            return Convert.ToInt64(str, 16); 
        } 

        public static byte[] StringToByte(string InString)
        {
            string[] ByteStrings;
            ByteStrings = InString.Split(" ".ToCharArray());
            byte[] ByteOut;
            ByteOut = new byte[ByteStrings.Length - 1];
            for (int i = 0; i == ByteStrings.Length - 1; i++)
            {
                ByteOut[i] = Convert.ToByte(("0x" + ByteStrings[i]));
            }
            return ByteOut;
        }
       
        /// <summary>
        /// 创建节点文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="xmlFileName"></param>
        public static void CreateXmlFile(string xmlFileName)
        {
        	//loopNum =int.Parse(Cls.RWconfig.GetAppSettings("loopNum").ToString());
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = ("   ");            
            using (XmlWriter writer = XmlWriter.Create(xmlFileName, settings))
            {
                // Write XML data. 
                writer.WriteStartElement("nodeGroup");
                writer.WriteAttributeString("lxTime","60000");//默认轮询时间1分钟
                writer.WriteEndElement();
                writer.Flush(); 
            }
        }
        /// <summary>
        /// 创建节点的地图xml配置文档
        /// </summary>
        /// <param name="path"></param>
        /// <param name="xmlFileName"></param>
        public static void CreateMapXmlFile(string xmlFileName)
        {
            //loopNum =int.Parse(Cls.RWconfig.GetAppSettings("loopNum").ToString());
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = ("   ");
            using (XmlWriter writer = XmlWriter.Create(xmlFileName, settings))
            {
                // Write XML data. 
                writer.WriteStartElement("Map");
                writer.WriteAttributeString("path", "");//默认轮询时间1分钟
                for(int i=1;i<=100;i++)
                {
                	writer.WriteStartElement("zoneMap");
                	writer.WriteAttributeString("zoneID",i.ToString());
                	writer.WriteAttributeString("imgPath","");
                	writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.Flush();
            }
        }

        /// <summary>
        /// 创建系统用户文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="xmlFileName"></param>
        public static void CreateUserXmlFile(string xmlFileName)
        {
            //loopNum =int.Parse(Cls.RWconfig.GetAppSettings("loopNum").ToString());
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = ("   ");
            using (XmlWriter writer = XmlWriter.Create(xmlFileName, settings))
            {
                // Write XML data. 
                writer.WriteStartElement("root");
                ////生产商
                //writer.WriteStartElement("user");
                //writer.WriteAttributeString("code", "Administrator");
                //writer.WriteAttributeString("name", "生产商");
                //writer.WriteAttributeString("telephone", "000");
                //writer.WriteAttributeString("password", Cls.DESS.Encode("godexadminlk"));
                //writer.WriteAttributeString("role", "Administrator");
                //writer.WriteEndElement();

                //系统管理员
                writer.WriteStartElement("user");
                writer.WriteAttributeString("code", "Agency");
                writer.WriteAttributeString("name", "代理商");
                writer.WriteAttributeString("telephone", "000");
                writer.WriteAttributeString("password", Cls.DESS.Encode("godex10"));
                writer.WriteAttributeString("role", "Agency");
                writer.WriteEndElement();
                //普通用户
                writer.WriteStartElement("user");
                writer.WriteAttributeString("code", "Manager");
                writer.WriteAttributeString("name", "管理员");
                writer.WriteAttributeString("telephone", "000");
                writer.WriteAttributeString("password", Cls.DESS.Encode("godex11"));
                writer.WriteAttributeString("role", "Manager");
                writer.WriteEndElement();
                //观察员
                writer.WriteStartElement("user");
                writer.WriteAttributeString("code", "Users");
                writer.WriteAttributeString("name", "操作员");
                writer.WriteAttributeString("telephone", "000");
                writer.WriteAttributeString("password", Cls.DESS.Encode("godex12"));
                writer.WriteAttributeString("role", "Users");
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.Flush();
            }
        }

        /// <summary>
        /// 创建节点的曲线文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="xmlFileName"></param>
        public static void createBinCurve(string filePath,int fire1,int fire2,int fire3,int value)
        {
            using (BinaryWriter binWriter =
                        new BinaryWriter(File.Open(filePath, FileMode.OpenOrCreate | FileMode.Append)))
            {
                binWriter.Write(fire1);
                binWriter.Write(fire2);
                binWriter.Write(fire3);
                binWriter.Write(value);               
                binWriter.Write(DateTime.Now.ToOADate());
                binWriter.Flush();
                binWriter.Close();
            }
        }

        /// <summary>
        /// 创建节点的烟雾值文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="xmlFileName"></param>
        public static void createFireCurveTxt(string filePath, int fire1, int fire2, int fire3,int fire4, int value)
        {
            using (StreamWriter binWriter =
                        new StreamWriter(File.Open(filePath, FileMode.OpenOrCreate | FileMode.Append)))
            {
                binWriter.Write(fire1 + "," + fire2 + "," + fire3 + ","+ fire4 + "," + value + "," + DateTime.Now.ToOADate() + "\r\n");
                binWriter.Flush();
                binWriter.Close();
            }
        }

        /// <summary>
        /// 创建节点的气流值文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="xmlFileName"></param>
        public static void createAirCurveTxt(string filePath, int aire1, int aire2, int value)
        {
            using (StreamWriter binWriter =
                        new StreamWriter(File.Open(filePath, FileMode.OpenOrCreate | FileMode.Append)))
            {
                binWriter.Write(aire1 + "," + aire2 + "," + value + "," + DateTime.Now.ToOADate()+"\r\n");
                binWriter.Flush();
                binWriter.Close();
            }
        }


        public static byte[] Read2Buffer (Stream stream, int BufferLen)
        {   
            // 如果指定的无效长度的缓冲区，则指定一个默认的长度作为缓存大小   
            if (BufferLen < 1)
            {   BufferLen = 0x8000;   }   
            // 初始化一个缓存区   
            byte[] buffer = new byte[BufferLen];   
            int read=0;      
            int block;   
            // 每次从流中读取缓存大小的数据，知道读取完所有的流为止   
            while ((block = stream.Read(buffer, read, buffer.Length-read)) > 0)
            {   
                // C#读取文件之重新设定读取位置   
                read += block;        
                // C#读取文件之检查是否到达了缓存的边界，检查是否还有可以读取的信息   
                if (read == buffer.Length)
                {   // 尝试读取一个字节   
                    int nextByte = stream.ReadByte(); 
                    // C#读取文件之读取失败则说明读取完成可以返回结果  
                    if (nextByte==-1)
                    {   
                        return buffer; 
                    }    
                    // 调整数组大小准备继续读取  
                    byte[] newBuf = new byte[buffer.Length*2]; 

                    Array.Copy(buffer, newBuf, buffer.Length); 
                    newBuf[read]=(byte)nextByte;  
                    buffer = newBuf;  
                    // buffer是一个引用（指针），  
                    //这里意在重新设定buffer指针指向一个更大的内存   
                    read++;   
                }   
            }  
            // 如果缓存太大则使用ret来收缩前面while读取的buffer，然后直接返回  
            byte[] ret = new byte[read];  
            Array.Copy(buffer, ret, read); 
            return ret;  
        } 



#region 写系统日志
        public static void writeLog(string wLog)
        {
        	using (StreamWriter w = File.AppendText(System.AppDomain.CurrentDomain.BaseDirectory+ "\\sysLog\\Go-DES_Log_"+DateTime.Now.ToString("yyyyMMdd")+".log"))
            {
                Log(wLog, w);
                // Close the writer and underlying file.
                w.Close();
            }
        }

        public static void Log(String logMessage, TextWriter w)
        {            
            w.WriteLine("{0}",logMessage);
            // Update the underlying file.
            w.Flush();
        }

        //public static void DumpLog(StreamReader r)
        //{
        //    // While not at the end of the file, read and write lines.
        //    String line;
        //    while ((line = r.ReadLine()) != null)
        //    {
        //        Console.WriteLine(line);
        //    }
        //    r.Close();
        //}
        #endregion

        //public static void InsertNode(string path, string node,string insertNodeName)
        //{
        //    try
        //    {
        //        XmlDocument doc = new XmlDocument();
        //        doc.Load(path);
        //        XmlNode xn = doc.SelectSingleNode(node);
        //        doc.CreateNode(node, "node", "namespaceuri");
                
        //    }
        //    catch { }
        //}

        /// <summary>
        /// 插入节点下的子节点
        /// </summary>
        /// <param name="path">xml文件路径</param>
        /// <param name="node">选择插入的节点</param>
        /// <param name="element">节点下的子节点,若无则创建</param>
        /// <param name="attribute">设置子节点属性</param>
        /// <param name="value">子节点属性值</param>
        public static void Insert(string path, string node, string element, string attribute, string value)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                XmlNode xn = doc.SelectSingleNode(node);
                if (element.Equals(""))
                {
                    if (!attribute.Equals(""))
                    {
                        XmlElement xe = (XmlElement)xn;
                        xe.SetAttribute(attribute, value);
                    }
                }
                else
                {
                    XmlElement xe = doc.CreateElement(element);
                    if (attribute.Equals(""))
                        xe.InnerText = value;
                    else
                        xe.SetAttribute(attribute, value);
                    xn.AppendChild(xe);
                }
                doc.Save(path);
            }
            catch { }
        }

        /// <summary>
        /// 插入节点
        /// </summary>
        /// <param name="xmlPath">xml文件路径</param>
        /// <param name="nodeID">插入Node的配置节点</param>
        public static void InsertNode(string xmlPath,string nodeID)
        {
            XmlDocument xd = new XmlDocument();
            xd.Load(xmlPath);
            XmlNodeList xnl = xd.SelectNodes("/nodeGroup/node[@ID='" + nodeID + "']");//选择nodeGroup节点下node的ID属性=当前节点的ID
            if (xnl.Count  == 0)
            {   
                XmlNode xnRoot = xd.SelectSingleNode("nodeGroup");
                XmlElement xe = xd.CreateElement("node");
                xe.SetAttribute("ID", nodeID);
                xe.SetAttribute("mathineType", "");//节点设备类型
                xe.SetAttribute("mathineXinghao", "未设置型号");//节点设备型号
                xe.SetAttribute("airSpeed", "9");//节点抽气泵转速
                xe.SetAttribute("address", "未定义");//节点地址
                
                xe.SetAttribute("zoneMapID","0");
                xe.SetAttribute("zoneMapX", "0");
                xe.SetAttribute("zoneMapY", "0");
                xe.SetAttribute("fullMapX", "0");
                xe.SetAttribute("fullMapY", "0");
                xnRoot.AppendChild(xe);

                XmlNode xnChl = xd.SelectSingleNode("/nodeGroup/node[@ID='" + nodeID + "']");	//选择ID为nodeID的node节点					
                XmlElement xe1 = xd.CreateElement("a1fire");//A1火警
                xe1.SetAttribute("ch1", "0");
                xe1.SetAttribute("ch2", "0");
                xe1.SetAttribute("ch3", "0");
                xe1.SetAttribute("ch4", "0");
                xe1.SetAttribute("ch5", "0");
                xnChl.AppendChild(xe1);

                XmlElement xe2 = xd.CreateElement("a2fire");//A2火警
                xe2.SetAttribute("ch1", "0");
                xe2.SetAttribute("ch2", "0");
                xe2.SetAttribute("ch3", "0");
                xe2.SetAttribute("ch4", "0");
                xe2.SetAttribute("ch5", "0");
                xnChl.AppendChild(xe2);

                XmlElement xe3 = xd.CreateElement("a3fire");//A3火警
                xe3.SetAttribute("ch1", "0");
                xe3.SetAttribute("ch2", "0");
                xe3.SetAttribute("ch3", "0");
                xe3.SetAttribute("ch4", "0");
                xe3.SetAttribute("ch5", "0");
                xnChl.AppendChild(xe3);

                XmlElement xe44 = xd.CreateElement("a4fire");//A4火警
                xe44.SetAttribute("ch1", "0");
                xe44.SetAttribute("ch2", "0");
                xe44.SetAttribute("ch3", "0");
                xe44.SetAttribute("ch4", "0");
                xe44.SetAttribute("ch5", "0");
                xnChl.AppendChild(xe44);

                XmlElement xe4 = xd.CreateElement("airlow");//气流低
                xe4.SetAttribute("ch1", "0");
                xe4.SetAttribute("ch2", "0");
                xe4.SetAttribute("ch3", "0");
                xe4.SetAttribute("ch4", "0");
                xe4.SetAttribute("ch5", "0");
                xnChl.AppendChild(xe4);

                XmlElement xe5 = xd.CreateElement("airhigh");//气流高
                xe5.SetAttribute("ch1", "0");
                xe5.SetAttribute("ch2", "0");
                xe5.SetAttribute("ch3", "0");
                xe5.SetAttribute("ch4", "0");
                xe5.SetAttribute("ch5", "0");
                xnChl.AppendChild(xe5);

                XmlElement xe7 = xd.CreateElement("fireTime");
                xe7.SetAttribute("A1Time", "0");
                xe7.SetAttribute("A2Time", "0");
                xe7.SetAttribute("A3Time", "0");
                xe7.SetAttribute("A4Time", "0");
                xnChl.AppendChild(xe7);

                XmlElement xe6 = xd.CreateElement("date");//气流高
                xe6.SetAttribute("date1", "2010");
                xe6.SetAttribute("date2", "00");
                xe6.SetAttribute("date3", "00");
                xe6.SetAttribute("date4", "00");
                xe6.SetAttribute("date5", "00");
                xe6.SetAttribute("date6", "00");
                xnChl.AppendChild(xe6);



                //XmlElement area1 = xd.CreateElement("Area1");//区1
                //area1.SetAttribute("zoneMapX", "0");
                //area1.SetAttribute("zoneMapY", "0");
                //area1.SetAttribute("fullMapX", "0");
                //area1.SetAttribute("fullMapY", "0");

                //XmlElement area2 = xd.CreateElement("Area2");//区2
                //area2.SetAttribute("zoneMapX", "0");
                //area2.SetAttribute("zoneMapY", "0");
                //area2.SetAttribute("fullMapX", "0");
                //area2.SetAttribute("fullMapY", "0");

                //XmlElement area3 = xd.CreateElement("Area3");//区3
                //area3.SetAttribute("zoneMapX", "0");
                //area3.SetAttribute("zoneMapY", "0");
                //area3.SetAttribute("fullMapX", "0");
                //area3.SetAttribute("fullMapY", "0");

                //XmlElement area4 = xd.CreateElement("Area4");//区4
                //area4.SetAttribute("zoneMapX", "0");
                //area4.SetAttribute("zoneMapY", "0");
                //area4.SetAttribute("fullMapX", "0");
                //area4.SetAttribute("fullMapY", "0");


            }
            xd.Save(xmlPath);
        }
        
        public static void changeNodeAttribute(string xmlPath,string nodeID,string attribute,string attributeString)
        {
        	XmlDocument xd = new XmlDocument();
            xd.Load(xmlPath);
            XmlNode xnl = xd.SelectSingleNode("/nodeGroup/node[@ID='" + nodeID + "']");//选择nodeGroup节点下node的ID属性=当前节点的ID
            if (xnl !=null)
            {
            	xnl.Attributes[attribute].Value =attributeString ;
            }
            xd.Save(xmlPath);
        }
    }
    

    public class CirclePanel
    {
        [System.Runtime.InteropServices.DllImport("gdi32")]
        private static extern IntPtr BeginPath(IntPtr hdc);
        [System.Runtime.InteropServices.DllImport("gdi32")]
        private static extern int SetBkMode(IntPtr hdc, int nBkMode);
        const int TRANSPARENT = 1;
        [System.Runtime.InteropServices.DllImport("gdi32")]
        private static extern IntPtr EndPath(IntPtr hdc);
        [System.Runtime.InteropServices.DllImport("gdi32")]
        private static extern IntPtr PathToRegion(IntPtr hdc);
        [System.Runtime.InteropServices.DllImport("gdi32")]
        private static extern int Ellipse(IntPtr hdc, int x1, int y1, int x2, int y2);
        [System.Runtime.InteropServices.DllImport("user32")]
        private static extern IntPtr SetWindowRgn(IntPtr hwnd, IntPtr hRgn, bool bRedraw);
        [System.Runtime.InteropServices.DllImport("user32")]
        private static extern IntPtr GetDC(IntPtr hwnd);
        public CirclePanel()
        {
        }
        public void MakePanelToCircle(Panel pb)
        {
            IntPtr dc;
            IntPtr region;
            dc = GetDC(pb.Handle);
            BeginPath(dc);
            SetBkMode(dc, TRANSPARENT);
            Ellipse(dc, 0, 0, pb.Width, pb.Height);
            EndPath(dc);
            region = PathToRegion(dc);
            SetWindowRgn(pb.Handle, region, false);
        }
    }

    public class XmlHelper
    {
        public XmlHelper()
        {
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="attribute">属性名,非空时返回该属性值,否则返回串联值</param>
        /// <returns>string</returns>
        /**************************************************
         * 使用示列:
         * XmlHelper.Read(path, "/Node", "")
         * XmlHelper.Read(path, "/Node/Element[@Attribute='Name']", "Attribute")
         ************************************************/
        public static string Read(string path, string node, string attribute)
        {
            string value = "";
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                XmlNode xn = doc.SelectSingleNode(node);
                value = (attribute.Equals("") ? xn.InnerText : xn.Attributes[attribute].Value);
            }
            catch { }
            return value;
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="element">元素名,非空时插入新元素,否则在该元素中插入属性</param>
        /// <param name="attribute">属性名,非空时插入该元素属性值,否则插入元素值</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        /**************************************************
         * 使用示列:
         * XmlHelper.Insert(path, "/Node", "Element", "", "Value")
         * XmlHelper.Insert(path, "/Node", "Element", "Attribute", "Value")
         * XmlHelper.Insert(path, "/Node", "", "Attribute", "Value")
         ************************************************/
        public static void Insert(string path, string node, string element, string attribute, string value)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                XmlNode xn = doc.SelectSingleNode(node);
                if (element.Equals(""))
                {
                    if (!attribute.Equals(""))
                    {
                        XmlElement xe = (XmlElement)xn;
                        xe.SetAttribute(attribute, value);
                    }
                }
                else
                {
                    XmlElement xe = doc.CreateElement(element);
                    if (attribute.Equals(""))
                        xe.InnerText = value;
                    else
                        xe.SetAttribute(attribute, value);
                    xn.AppendChild(xe);
                }
                doc.Save(path);
            }
            catch { }
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="attribute">属性名,非空时修改该节点属性值,否则修改节点值</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        /**************************************************
         * 使用示列:
         * XmlHelper.Insert(path, "/Node", "", "Value")
         * XmlHelper.Insert(path, "/Node", "Attribute", "Value")
         ************************************************/
        public static void Update(string path, string node, string attribute, string value)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                XmlNode xn = doc.SelectSingleNode(node);
                XmlElement xe = (XmlElement)xn;
                if (attribute.Equals(""))
                    xe.InnerText = value;
                else
                    xe.SetAttribute(attribute, value);
                doc.Save(path);
            }
            catch(Exception ee) {MessageBox.Show(ee.ToString()); }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="attribute">属性名,非空时删除该节点属性值,否则删除节点值</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        /**************************************************
         * 使用示列:
         * XmlHelper.Delete(path, "/Node", "")
         * XmlHelper.Delete(path, "/Node", "Attribute")
         ************************************************/
        public static void Delete(string path, string node, string attribute)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                XmlNode xn = doc.SelectSingleNode(node);
                XmlElement xe = (XmlElement)xn;
                if (attribute.Equals(""))
                    xn.ParentNode.RemoveChild(xn);
                else
                    xe.RemoveAttribute(attribute);
                doc.Save(path);
            }
            catch { }
        }



        /// <summary>
        /// 加密方法
        /// </summary>
        /// <param name="Doc"></param>
        /// <param name="ElementName"></param>
        /// <param name="Key"></param>
        public static void Encrypt(XmlDocument Doc, string ElementName, SymmetricAlgorithm Key)
        {
            XmlElement elementToEncrypt = Doc.GetElementsByTagName(ElementName)[0] as XmlElement;
            EncryptedXml eXml = new EncryptedXml();
            byte[] encryptedElement = eXml.EncryptData(elementToEncrypt, Key, false);
            EncryptedData edElement = new EncryptedData();
            edElement.Type = EncryptedXml.XmlEncElementUrl;
            string encryptionMethod = null;

            if (Key is TripleDES)
            {
                encryptionMethod = EncryptedXml.XmlEncTripleDESUrl;
            }
            else if (Key is DES)
            {
                encryptionMethod = EncryptedXml.XmlEncDESUrl;
            }
            if (Key is Rijndael)
            {
                switch (Key.KeySize)
                {
                    case 128:
                        encryptionMethod = EncryptedXml.XmlEncAES128Url;
                        break;
                    case 192:
                        encryptionMethod = EncryptedXml.XmlEncAES192Url;
                        break;
                    case 256:
                        encryptionMethod = EncryptedXml.XmlEncAES256Url;
                        break;
                }
            }
            edElement.EncryptionMethod = new EncryptionMethod(encryptionMethod);
            edElement.CipherData.CipherValue = encryptedElement;
            EncryptedXml.ReplaceElement(elementToEncrypt, edElement, false);
        }

        /// <summary>
        /// 解密方法
        /// </summary>
        /// <param name="Doc"></param>
        /// <param name="encryptedElement"></param>
        /// <param name="Alg"></param>
        public static void Decrypt(XmlElement encryptedElement, SymmetricAlgorithm key)
        {
            EncryptedData edElement = new EncryptedData();
            edElement.LoadXml(encryptedElement);
            EncryptedXml exml = new EncryptedXml();
            byte[] rgbOutput = exml.DecryptData(edElement, key);
            exml.ReplaceData(encryptedElement, rgbOutput);
        }

        /// <summary>
        /// 加密xml文件
        /// </summary>
        /// <param name="xmlPath"></param>
        /// <param name="node"></param>
        public static void EncryptXml(string xmlPath,string node)
        {
            RijndaelManaged key = new RijndaelManaged();
            //设置密钥:key为32位=数字或字母16个=汉字8个
            byte[] byteKey = Encoding.Unicode.GetBytes("godex20110331lxc");
            key.Key = byteKey;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.PreserveWhitespace = true;
            xmlDoc.Load(xmlPath);//加载要加密的XML文件            
            Encrypt(xmlDoc,node, key);//需要加密的节点

            if (key != null)
            {
                key.Clear();
            }
            xmlDoc.Save(xmlPath);//生成加密后的XML文件
        }

        /// <summary>
        /// 解密xml文件
        /// </summary>
        /// <param name="Doc"></param>
        /// <param name="Alg"></param>
        public static void Decrypt(XmlDocument Doc, SymmetricAlgorithm Alg)
       {
           // Check the arguments. 
           if (Doc == null)
               throw new ArgumentNullException("Doc");
           if (Alg == null)
               throw new ArgumentNullException("Alg");

           // Find the EncryptedData element in the XmlDocument.
           XmlElement encryptedElement = Doc.GetElementsByTagName("EncryptedData")[0] as XmlElement;

           // If the EncryptedData element was not found, throw anexception.
           if (encryptedElement == null)
           {
               return;
               //throw new XmlException("The EncryptedData element was notfound.");
           }
           // Create an EncryptedData object and populate it.
           EncryptedData edElement = new EncryptedData();
           edElement.LoadXml(encryptedElement);

           // Create a new EncryptedXml object.
           EncryptedXml exml = new EncryptedXml();


           // Decrypt the element using the symmetric key.
           byte[] rgbOutput = exml.DecryptData(edElement, Alg);

           // Replace the encryptedData element with the plaintext XMLelement.
           exml.ReplaceData(encryptedElement, rgbOutput);

       }



        /// <summary>
        /// 解密xml文件
        /// </summary>
        /// <param name="xmlPath"></param>
        /// <param name="node"></param>
        /// <param name="EncryptedData"></param>
        public static void DecryptXml(string xmlPath)
        {
            RijndaelManaged key = new RijndaelManaged();
            //设置密钥:key为32位=数字或字母16个=汉字8个
            byte[] byteKey = Encoding.Unicode.GetBytes("godex20110331lxc");
            key.Key = byteKey;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlPath);
            Decrypt(xmlDoc, key);
            if (key != null)
            {
                key.Clear();
            }
            xmlDoc.Save(xmlPath);//生成解密后的XML文件
        }
    }
}
