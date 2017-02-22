using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO.Ports;

namespace GSM.Cls
{
    class ReadComm
    {
        internal class CommReader
        {
            //单个缓冲区最大长度
            private const int max = 9;
            //数据计数器
            private int count = 0;
            //private int countData = 1;
            //变长标志数
            private int bufCount = 0;
            //数字缓冲区
            private Byte[] buffer = new Byte[max];
            /// 
            /// 串口控件
            /// 
            private SerialPort _Comm;
            /// 
            /// 扫描的时间间隔 单位毫秒
            /// 
            private Int32 _interval;

            //数据处理函数
            public delegate void HandleCommData(Byte[] data);
            //事件侦听
            public event HandleCommData Handlers;

            //负责读写Comm的线程
            private Thread _workerThread;

            internal CommReader(SerialPort comm, Int32 interval)
            {
                _Comm = comm;
                //创建读取线程
                _workerThread = new Thread(new ThreadStart(ReadComm));
                //确保扫描时间间隔不要太小,造成线程长期占用cpu
                if (interval < 50)
                    _interval = 50;
                else
                    _interval = interval;
            }

            //读取串口数据,为线程执行函数
            public void ReadComm()
            {
                try
                {
                    while (true)
                    {
                        Object obj = null;
                        try
                        {
                            //每隔一定时间,从串口读入一字节
                            //如未读到,obj为null
                            if (_Comm.IsOpen)
                                obj = _Comm.ReadByte();
                        }
                        catch
                        {

                        }

                        if (obj == null)
                        { //未读到数据,线程休眠
                            Thread.Sleep(_interval);
                            continue;
                        }
                        //将读到的一字节数据存入缓存,这里需要做一转换                    
                        buffer[count] = Convert.ToByte(obj);
                        if (buffer[0] == 0xFE)
                        {
                            count++;
                        }

                        if (buffer[1] == 0x04)
                            bufCount = 6;
                        else
                            bufCount = 9;

                        //计算接收数据的结束位                    
                        //当达到指定长度时,这里的判断条件可以根据要求变为:
                        // 判断当前读到的字节是否为结束位,等等
                        //计算结束标志位,协议为除了开始标志位的其他数据值的异或值

                        if (bufCount == 6 && count == 6)
                        {
                            if (buffer[1] == 0x04)
                            {
                                //复制数据,并清空缓存,计数器也置零
                                Byte[] data = new Byte[6];//bufCount                        
                                //Array.Copy(buffer, data, bufCount);
                                Array.Copy(buffer, 0, data, 0, 6);
                                count = 0;
                                Array.Clear(buffer, 0, max);
                                //通知处理器处理数据
                                if (Handlers != null)
                                    Handlers(data);
                            }
                            else
                            {
                                Array.Clear(buffer, 0, max);
                                count = 0;
                            }
                        }

                        //存储上传 协议
                        if (bufCount == 9 && count == 9)
                        {
                            if (buffer[1] == 0x08)
                            {
                                //复制数据,并清空缓存,计数器也置零
                                Byte[] data = new Byte[9];//bufCount                        
                                //Array.Copy(buffer, data, bufCount);
                                Array.Copy(buffer, 0, data, 0, 9);
                                count = 0;
                                Array.Clear(buffer, 0, max);
                                //通知处理器处理数据
                                if (Handlers != null)
                                    Handlers(data);
                            }
                            else
                            {
                                Array.Clear(buffer, 0, max);
                                count = 0;
                            }
                        }
                    }
                }
                catch {
                    
                }
            }

            //启动读入器
            public void Start()
            {
                //启动读取线程
                if (_workerThread.IsAlive)
                    return; 
                if (!_Comm.IsOpen)
                    _Comm.Open();
                _workerThread.Start();
                while (!_workerThread.IsAlive)
                {
                    Stop();
                };
            }

            //停止读入
            public void Stop()
            {
                //停止读取线程
                if (_workerThread.IsAlive)
                {
                    _workerThread.Abort();
                    _workerThread.Join();
                }
                //_Comm.Close();
            }
        }
    }
}
