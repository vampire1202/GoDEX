using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GSM
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool createNew;
            using (System.Threading.Mutex mutex = new System.Threading.Mutex(true, Application.ProductName, out createNew))
            {
                if (createNew)
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new frmLoginIn());
                }
                else
                {
                    MessageBox.Show("应用程序已经在运行中...");
                    System.Threading.Thread.Sleep(1000);
                    System.Environment.Exit(1);
                }
            }

            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Control.CheckForIllegalCrossThreadCalls = false;
            //Application.Run(new frmLoginIn(new frmMain())); 
        }
        
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
        	MessageBox.Show(e.ExceptionObject.ToString());
        	return;
            //e.ExceptionObject
        }

        static void Application_ApplicationExit(object sender, EventArgs e)
        {
            // 可在程序即将退出时做点事情。
            try{}
            catch(Exception ee)
            {
            	MessageBox.Show(ee.ToString());
            	return;
            }
            
            //throw new Exception("The method or operation is not implemented.");
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {            
            // e.Exception 可通过判断e的Exception来查找异常。
            MessageBox.Show(e.Exception.ToString());
            return;
        } 

    }
}
