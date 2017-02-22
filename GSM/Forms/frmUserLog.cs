using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Printing;
using System.Xml;
using System.Xml.XPath;

namespace GSM.Forms
{
    public partial class frmUserLog : WeifenLuo.WinFormsUI.Docking.DockContent
    {  
        private Font printFont;
        private StreamReader streamToPrint;
        private string _findInfo = string.Empty;
        private string _OtherInfo = string.Empty; 
        //private string _logPath = AppDomain.CurrentDomain.BaseDirectory + "\\sysLog\\Go-DES_Log_" + DateTime.Now.ToString("yyyyMMdd") + ".log";
        private string _currentLogFile = string.Empty;
        private string _nodeID = string.Empty; 
        private PrintDocument pd;
        private string nodeConfigPath=string.Empty;

        public frmUserLog()
        { 
            InitializeComponent();
        }
       
        /// <summary>
        /// 配置文件
        /// </summary>
        
        //public string NodeConfigPath
        //{
        //    get{
        //        return this.nodeConfigPath;
        //    }
        //    set {this.nodeConfigPath = value;}
        //}
        
        void FrmUserLogLoad(object sender, EventArgs e)
        {
            //readLog();
            getNode(this.flowLayoutPanel2);
        } 

        //获取日志
        void readLog()
        {
            GoDexData.BLL.log blllog = new GoDexData.BLL.log();
            DataSet dsLog = blllog.GetList(" acDateTime>='" + this.dateTimePicker1.Value + "' and acDateTime<='" + this.dateTimePicker2.Value + "'");
            this.dataGridView_Log.DataSource = dsLog.Tables[0];
            this.dataGridView_Log.Refresh(); 
        } 

        //获取当前nodeID列表
        void getNode(FlowLayoutPanel flp)
        {
            flp.Controls.Clear();
            GoDexData.BLL.nodeInfo bllNi = new GoDexData.BLL.nodeInfo();
            DataSet dsNi = bllNi.GetAllList();
            foreach (DataRow dr in dsNi.Tables[0].Rows)
            {
                CheckBox cb = new CheckBox();
                cb.Text = "节点:"+ dr["machineNo"].ToString();
                cb.Name = dr["machineNo"].ToString(); 
                cb.Click += new EventHandler(cb_Click);
                flp.Controls.Add(cb);
            }
        }

        void cb_Click(object sender, EventArgs e)
        {
            _OtherInfo = null;
            CheckBox chk = (CheckBox)sender;

            foreach (CheckBox cb in this.flowLayoutPanel2.Controls)
            {
                cb.CheckState = CheckState.Unchecked;
            }

            chk.CheckState = CheckState.Checked;

            foreach (CheckBox cb in this.flowLayoutPanel2.Controls)
            {
                if (cb.CheckState == CheckState.Checked)
                {
                    _OtherInfo += " machineNo= '" + cb.Name + "' or";
                }
            }

            createLog();
        } 

        private void chk0_Click(object sender, EventArgs e)
        {
            //this.richTextBox1.Clear();
            //this.listView1.Items.Clear();
            this.dataGridView_Log.DataSource = null; 
            _findInfo = string.Empty;
            foreach (CheckBox ctl in this.flowLayoutPanel1.Controls)
            {
                if (ctl.CheckState == CheckState.Checked)
                    _findInfo += " action like '%"+ ctl.Text + "%' or";
            }

            foreach (CheckBox ctl in this.flowLayoutPanel2.Controls)
            {
                ctl.Checked = false;
            }
            _OtherInfo = null;
            createLog();
        } 

        //或查询
        private void createLog()
        { 
            if (string.IsNullOrEmpty(_findInfo) && string.IsNullOrEmpty(_OtherInfo))
            { 
                readLog();
            }
            else
            {
                string strWhere = string.Empty;
                GoDexData.BLL.log bllLog = new GoDexData.BLL.log();
                DataSet dsLog = null ;
                string findInfo = string.Empty;
                string otherInfo = string.Empty;
                string allfindInfo = string.Empty;

                if(!string.IsNullOrEmpty(_findInfo))
                     findInfo = _findInfo.Trim().Substring(0, _findInfo.Length - 3);
                if(!string.IsNullOrEmpty(_OtherInfo)) 
                     otherInfo = _OtherInfo.Trim().Substring(0, _OtherInfo.Length - 3);
                try
                {
                    if (!string.IsNullOrEmpty(otherInfo) && !string.IsNullOrEmpty(findInfo))
                    { allfindInfo = "(" + findInfo + ") and " + otherInfo; }
                    else if (string.IsNullOrEmpty(otherInfo))
                    { allfindInfo = findInfo; }
                    else if (string.IsNullOrEmpty(findInfo))
                    { allfindInfo = otherInfo; }

                    allfindInfo += "and acDateTime>='" + this.dateTimePicker1.Value + "' and acDateTime<='" + this.dateTimePicker2.Value + "'"; 

                    dsLog = bllLog.GetList(allfindInfo);
                    this.dataGridView_Log.DataSource = dsLog.Tables[0]; 
                }
                catch
                {

                }
                finally
                {
                    //_OtherInfo = this.richTextBox1.Text;
                    if(dsLog !=null)
                        dsLog.Dispose();
                }
            }
        }


        // The PrintPage event is raised for each page to be printed.
        private void pd_PrintPage(object sender, PrintPageEventArgs ev)
        {
            float linesPerPage = 0;
            float yPos = 0;
            int count = 0;
            float leftMargin = ev.MarginBounds.Left / 2;
            float topMargin = ev.MarginBounds.Top;
            string line = null;
            string title = "GO-DEX早期预警系统 日志记录";

            // Calculate the number of lines per page.
            linesPerPage = ev.MarginBounds.Height / printFont.GetHeight(ev.Graphics);
            //ev.PageBounds.Width/2-(title.Length * printFont.Size)/2
            ev.Graphics.DrawString(title, new Font("宋体", 18, FontStyle.Bold), Brushes.Black, new PointF(leftMargin, topMargin));
            ev.Graphics.DrawString(DateTime.Now.ToLongDateString(), printFont, Brushes.Black, new PointF(leftMargin + 2 * ev.PageBounds.Width / 3, topMargin + 8));
            ev.Graphics.DrawLine(new Pen(Brushes.Black, 2), new Point((int)leftMargin, (int)topMargin + 32), new Point(ev.PageBounds.Width - (int)leftMargin, (int)topMargin + 32));

            // Print each line of the file.
            while (count < linesPerPage &&
               ((line = streamToPrint.ReadLine()) != null))
            {
                yPos = topMargin + 45 + (count * printFont.GetHeight(ev.Graphics));
                ev.Graphics.DrawString(line, printFont, Brushes.Black, leftMargin, yPos, new StringFormat());
                count++;
            }
            // If more lines exist, print another page.
            if (line != null)
                ev.HasMorePages = true;
            else
                ev.HasMorePages = false;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "printInfo.log");
            DataTable dt =  (DataTable)this.dataGridView_Log.DataSource;
            
            foreach (DataRow dr in dt.Rows)
            {
                sw.Write(dr["节点编号"].ToString() + "-" + dr["操作用户"].ToString()+":"+dr["操作内容"].ToString() +"-"+ dr["时间"].ToString()+ "\r\n\r\n");
            } 
            sw.Flush();
            sw.Close();

            try
            {
                streamToPrint = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "printInfo.log");
                try
                {
                    printFont = new Font("宋体", 12);
                    pd = new PrintDocument();
                    pd.PrintPage += new PrintPageEventHandler
                       (this.pd_PrintPage);
                    PrintPreviewDialog ppd = new PrintPreviewDialog();
                    ppd.Document = pd;
                    ppd.VerticalScroll.Enabled = true;
                    ppd.Width = Screen.PrimaryScreen.Bounds.Width;
                    ppd.Height = Screen.PrimaryScreen.Bounds.Height;
                    ppd.Left = 0;
                    ppd.Top = 0;
                    ppd.ShowDialog();
                    //pd.Print();
                }
                finally
                {
                    streamToPrint.Close();
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnSetPrinter_Click(object sender, EventArgs e)
        {
            PrintDialog pdi = new PrintDialog();
            pdi.ShowDialog();
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            this.dataGridView_Log.DataSource = null;
            _findInfo = string.Empty; 

            foreach (CheckBox ctl in this.flowLayoutPanel1.Controls)
            {
                ctl.CheckState = CheckState.Checked;
                if (ctl.CheckState == CheckState.Checked)
                    _findInfo += " action like '%" + ctl.Text + "%' or";
            }

            foreach (CheckBox ctl in this.flowLayoutPanel2.Controls)
            {
                ctl.Checked = false;
            }
            _OtherInfo = null;
            createLog();
        }

        private void btnUnselect_Click(object sender, EventArgs e)
        {
            this.dataGridView_Log.DataSource = null;
 
            foreach (CheckBox ctl in this.flowLayoutPanel1.Controls)
            {
                ctl.CheckState = CheckState.Unchecked;
            }

            foreach (CheckBox ctl in this.flowLayoutPanel2.Controls)
            {
                ctl.Checked = false;
            }

            _findInfo = null;
            _OtherInfo = null;
            createLog();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            createLog();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            createLog();
        } 
    }
}
