using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Office.Core;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;

namespace GoDex
{
    public partial class Report : System.Web.UI.Page
    {
        public string dtfrom, dtto;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetNavigateStyle();
                if (!Directory.Exists(Server.MapPath("Report")))
                    Directory.CreateDirectory(Server.MapPath("Report"));
                this.cmbNodes.Items.Add("所有");
                for (int i = 1; i < 255; i++)
                {
                    this.cmbNodes.Items.Add(i.ToString());
                }
                this.cmbNodes.SelectedIndex = 0;
                Bind();
            }
            dtfrom = Request.Form["datefrom"];
            dtto = Request.Form["dateto"];
        }

        void SetNavigateStyle()
        {
            Label lblTitle = (Label)Page.Master.FindControl("lblTitle");
            lblTitle.Text = "当前位置:  查询报表";
            HyperLink hlMonitor = (HyperLink)Page.Master.FindControl("hrefMonitor");
            hlMonitor.Attributes.CssStyle.Value = null;
            HyperLink hlHome = (HyperLink)Page.Master.FindControl("hrefHome");
            hlHome.Attributes.CssStyle.Value = null;
            HyperLink hlNodes = (HyperLink)Page.Master.FindControl("hrefNodes");
            hlNodes.Attributes.CssStyle.Value = null;
            HyperLink hlReport = (HyperLink)Page.Master.FindControl("hrefReport");
            hlReport.Attributes.CssStyle.Value = "background:url(../images/navbg.png) no-repeat;";
        }

        DataSet Bind()
        {
            GoDexData.BLL.warn bllwarn = new GoDexData.BLL.warn();
            string strWhere = string.Empty;
            strWhere = " ID >= 1 ";
            if (this.cmbNodes.SelectedIndex != 0)
            {
                strWhere += " and machineNo='" + this.cmbNodes.Text + "'";
            }
            if (!string.IsNullOrEmpty(dtfrom))
                strWhere += " and  warnDateTime >= '" + dtfrom + "'";
            if (!string.IsNullOrEmpty(dtto))
                strWhere += "and warnDateTime <= '" + dtto + "'";

            DataSet ds = bllwarn.GetList(strWhere + " order by warnDateTime");

            GridView1.DataSource = ds;
            GridView1.DataKeyNames = new string[] { "ID" };//主键
            GridView1.DataBind();
            return ds;
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            Bind();
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //如果是绑定数据行 
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //鼠标经过时，行背景色变 
                e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#E6F5FA'");
                //鼠标移出时，行背景色变 
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#FFFFFF'");
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;

            Bind();
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            Bind();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            //CreateExcel(Bind(),"查询节点_" + this.cmbNodes.Text + "_from_" + dtfrom + "_to_" + dtto);
            OutputExcel(Bind(), "查询节点_" + this.cmbNodes.Text + "_from_" + dtfrom + "_to_" + dtto);

            //string filePath = Server.MapPath("\\Report\\" + "查询节点_" + this.cmbNodes.Text + "_from_" + dtfrom + "_to_" + dtto + ".csv");
            ////File.CreateText(filePath);
            //File.WriteAllText(filePath, "节点编号,警报信息,警报描述,当前用户,警报日期\r\n",System.Text.Encoding.Unicode);
            //FileStream fs = new FileStream(filePath, FileMode.Append, FileAccess.Write);
            //DataSet ds = Bind();
            //foreach (DataRow dr in ds.Tables[0].Rows)
            //{
            //    utils.AddText(fs, dr["machineNo"].ToString() + "," + dr["warnLeval"].ToString() + "," + dr["warnDiscrib"].ToString() + "," + dr["userName"].ToString() + "," + dr["warnDateTime"].ToString()+"\r\n");
            //}
            //fs.Flush();
            //fs.Close();
            //fs.Dispose();
            //string filePath = Server.MapPath("~/Report/") + "a.xlsx";
            //DownloadFile(filePath);
        }

        public void CreateExcel_1(DataSet ds, string FileName)
        {
            HttpResponse resp;
            resp = Page.Response;
            resp.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            resp.AppendHeader("Content-Disposition", "attachment;filename=" + FileName);
            string colHeaders = "", ls_item = ""; //定义表对象与行对象，同时用DataSet对其值进行初始化DataTable dt=ds.Tables[0]; 
            DataRow[] myRow = ds.Tables[0].Select();//可以类似dt.Select("id>10")之形式达到数据筛选目的
            int i = 0;
            int cl = ds.Tables[0].Columns.Count; //取得数据表各列标题，各标题之间以t分割，最后一个列标题后加回车符for(i=0;i<cl;i++)
            {
                if (i == (cl - 1))//最后一列，加n
                {
                    colHeaders += ds.Tables[0].Columns[i].ColumnName.ToString() + "n";
                }
                else
                {
                    colHeaders += ds.Tables[0].Columns[i].ColumnName.ToString() + "t";
                }

            }
            resp.Write(colHeaders);
            //向HTTP输出流中写入取得的数据信息 

            //逐行处理数据   
            foreach (DataRow row in myRow)
            {
                //当前行数据写入HTTP输出流，并且置空ls_item以便下行数据     
                for (i = 0; i < cl; i++)
                {
                    if (i == (cl - 1))//最后一列，加n
                    {
                        ls_item += row.ToString() + "n";
                    }
                    else
                    {
                        ls_item += row.ToString() + "t";
                    }

                }
                resp.Write(ls_item);
                ls_item = "";

            }
            resp.End();
        }

        public void CreateExcel(DataSet ds, string FileName)
        {
            string filename = FileName + ".xls"; //导出Excel的名字
            try
            {
                Excel.Application excel = new Excel.Application();//申明一个对象;
                Excel._Workbook book;
                Excel._Worksheet sheet;
                book = excel.Workbooks.Add(true);
                sheet = (Excel.Worksheet)book.ActiveSheet;

                excel.Cells[1, 1] = FileName;//在单元格中赋值，row&column都是从1开始 

                excel.Visible = false;
                book.SaveCopyAs(Server.MapPath("Report") + "\\" + filename);//保存excel在名为file的文件夹下，这个文件夹必须先存在；
                excel.Quit();//如果不注释掉，会弹出提示框询问你是否保存sheet1；
            }
            catch (Exception ex)
            {
                Response.Write("script language=javascript>alert('Encounter one error!');history.go(-1)</script>"); //Exception handling
            }
            //将保存在file文件夹下的excel，以文件流的方式导出
            string path = Server.MapPath("Report") + "\\" + filename;
            System.IO.FileInfo file = new System.IO.FileInfo(path);
            Response.Clear();     //[将文件从“服务端”导出到“客户端”]
            Response.Charset = "GB2312";  //[设置写入流的字符编码方式为GB2312]
            Response.ContentEncoding = System.Text.Encoding.UTF8; //[设置写入流的文字编码格式：UTF8]
            //Response.AddHeader("Content-Disposition", "attachment:filename=" + Server.UrlEncode(file.Name));
            Response.AddHeader("Content-Disposition", "attachment;filename=" + Server.UrlEncode(file.Name));//注意attachment后面是分号不是冒号，不然打开后的excel为.aspx页面的HTML代码;// 添加头信息，为"文件下载/另存为"对话框指定默认文件名
            Response.AddHeader("Content-Length", file.Length.ToString());// 添加头信息，指定文件大小，让浏览器能够显示下载进度
            Response.ContentType = "application/ms-excel";//指定内容类型，导出格式为excel，也可以是ms-word，etc;// 指定返回的是一个不能被客户端读取的流，必须被下载
            Response.WriteFile(file.FullName);// 把文件流发送到客户端
            Response.End(); // 停止页面的执行
            File.Delete(filename);//[关闭文件流] 
        }

        public void OutputExcel(DataSet dv, string strFileName)
        {
            //dv为要输出到Excel的数据，str为标题名称
            GC.Collect();
            Excel.Application excel;// = new Application();
            int rowIndex = 4;
            int colIndex = 1;
            Excel._Workbook xBk;
            Excel._Worksheet xSt;
            excel = new Excel.Application();
            xBk = excel.Workbooks.Add(true);
            xSt = (Excel._Worksheet)xBk.ActiveSheet;
            //
            //取得标题
            //
            foreach (DataColumn col in dv.Tables[0].Columns)
            {
                colIndex++;
                excel.Cells[4, colIndex] = col.ColumnName;
                xSt.Range[excel.Cells[4, colIndex], excel.Cells[4, colIndex]].HorizontalAlignment = XlVAlign.xlVAlignCenter;//设置标题格式为居中对齐
            }

            //
            //取得表格中的数据
            //
            foreach (DataRow row in dv.Tables[0].Rows)
            {
                rowIndex++;
                colIndex = 1;
                foreach (DataColumn col in dv.Tables[0].Columns)
                {
                    colIndex++;
                    if (col.DataType == System.Type.GetType("System.DateTime"))
                    {
                        excel.Cells[rowIndex, colIndex] = (Convert.ToDateTime(row[col.ColumnName].ToString())).ToString("yyyy-MM-dd");
                        xSt.Range[excel.Cells[rowIndex, colIndex], excel.Cells[rowIndex, colIndex]].HorizontalAlignment = XlVAlign.xlVAlignCenter;//设置日期型的字段格式为居中
                    }
                    else if (col.DataType == System.Type.GetType("System.String"))
                    {
                        excel.Cells[rowIndex, colIndex] = "'" + row[col.ColumnName].ToString();
                        xSt.Range[excel.Cells[rowIndex, colIndex], excel.Cells[rowIndex, colIndex]].HorizontalAlignment = XlVAlign.xlVAlignCenter;//设置字符型的字段格式为居中
                    }
                    else
                    {
                        excel.Cells[rowIndex, colIndex] = row[col.ColumnName].ToString();
                    }
                }
            }
            //
            //加载一个合计行
            //
            int rowSum = rowIndex + 1;
            int colSum = 2;
            excel.Cells[rowSum, 2] = "合计";
            xSt.Range[excel.Cells[rowSum, 2], excel.Cells[rowSum, 2]].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            //
            //设置选中的部分的颜色
            //
            xSt.Range[excel.Cells[rowSum, colSum], excel.Cells[rowSum, colIndex]].Select();
            xSt.Range[excel.Cells[rowSum, colSum], excel.Cells[rowSum, colIndex]].Interior.ColorIndex = 19;//设置为浅黄色，共计有56种
            //
            //取得整个报表的标题
            //
            excel.Cells[2, 2] = strFileName;
            //
            //设置整个报表的标题格式
            //
            xSt.Range[excel.Cells[2, 2], excel.Cells[2, 2]].Font.Bold = true;
            xSt.Range[excel.Cells[2, 2], excel.Cells[2, 2]].Font.Size = 22;
            //
            //设置报表表格为最适应宽度

            xSt.Range[excel.Cells[4, 2], excel.Cells[rowSum, colIndex]].Select();
            xSt.Range[excel.Cells[4, 2], excel.Cells[rowSum, colIndex]].Columns.AutoFit();
            //
            //设置整个报表的标题为跨列居中
            //
            xSt.Range[excel.Cells[2, 2], excel.Cells[2, colIndex]].Select();
            xSt.Range[excel.Cells[2, 2], excel.Cells[2, colIndex]].HorizontalAlignment = XlHAlign.xlHAlignCenterAcrossSelection;
            //
            //绘制边框
            //
            xSt.Range[excel.Cells[4, 2], excel.Cells[rowSum, colIndex]].Borders.LineStyle = 1;
            //
            //显示效果
            //
            excel.Visible = true;

            //xSt.Export(Server.MapPath(".")+"\\"+this.xlfile.Text+".xls",SheetExportActionEnum.ssExportActionNone,Microsoft.Office.Interop.OWC.SheetExportFormat.ssExportHTML);
            xBk.SaveCopyAs(Server.MapPath("~/Report") + "/" + strFileName + ".xls");
            //Response.Write("script language=javascript>alert('"+ Server.MapPath("~/Report").ToString() +"!');</script>"); //Exception handling

            xBk.Close(false, null, null);
            excel.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xBk);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xSt);
            xBk = null;
            excel = null;
            xSt = null;
            GC.Collect();
            DownloadFile(Server.MapPath("~/Report") + "/" + strFileName + ".xls");
        }

        void DownloadFile(string _fileName)
        {
            FileInfo fileInfo = new FileInfo(_fileName);
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("Content-Disposition", "attachment;filename=" + _fileName);
            Response.AddHeader("Content-Length", fileInfo.Length.ToString());
            Response.AddHeader("Content-Transfer-Encoding", "binary");
            Response.ContentType = "application/octet-stream";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
            Response.WriteFile(fileInfo.FullName);
            Response.Flush();
            Response.End();
        }

        //void DownloadFile(string filename)
        //{
        //    try
        //    {


        //    GC.Collect();
        //    string path = filename;
        //    FileStream fs = new FileStream(path, FileMode.Open);
        //    byte[] buffer = new byte[fs.Length];
        //    fs.Read(buffer, 0, buffer.Length);

        //    fs.Close();
        //    File.Delete(path);


        //    Response.ContentType = "application/ms-excel";
        //    Response.Charset = "GB2312";
        //    Response.ContentEncoding = System.Text.Encoding.UTF8;

        //    Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode("~/Report/a.xlsx"));
        //    Response.OutputStream.Write(buffer, 0, buffer.Length);
        //    Response.Flush();
        //    Response.End(); 
        //         }
        //    catch(Exception ee)
        //    {
        //        throw ee;
        //    }
        //}
    }
}