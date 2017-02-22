using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Threading;
using System.IO;

namespace GSM.Forms
{
    public partial class frmCurve : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        private string _nodeNo = string.Empty;
        private string _type = string.Empty;
        Series S_fire1;
        Series S_fire2;
        Series S_fire3;
        Series S_fch;
        Series S_airlow;
        Series S_airhigh;
        Series S_ach;
        private string _Date = DateTime.Now.ToString("yyyyMMdd");
        public frmCurve(string nodeNo,string type)
        {
            _nodeNo = nodeNo;
            _type = type;
            InitializeComponent();
            setChart();
        }

        private void frmCurve_Load(object sender, EventArgs e)
        {
            chart.Titles[0].Text = _Date + " 曲线图";
            switch (_type)
            {
                case "设备类型:单管单区":
                    this.glassButton3.Enabled = true;
                    this.glassButton4.Enabled = false;
                    this.glassButton5.Enabled = false;
                    this.glassButton6.Enabled = false;

                    this.glassButton7.Enabled = true;
                    this.glassButton8.Enabled = false;
                    this.glassButton9.Enabled = false;
                    this.glassButton10.Enabled = false;
                    break;
                case "设备类型:四管单区":
                    this.glassButton3.Enabled = true;
                    this.glassButton4.Enabled = false;
                    this.glassButton5.Enabled = false;
                    this.glassButton6.Enabled = false;

                    this.glassButton7.Enabled = true;
                    this.glassButton8.Enabled = true;
                    this.glassButton9.Enabled = true;
                    this.glassButton10.Enabled = true;
                    break;
                case "设备类型:四管四区":
                    this.glassButton3.Enabled = true;
                    this.glassButton4.Enabled = true;
                    this.glassButton5.Enabled = true;
                    this.glassButton6.Enabled = true;

                    this.glassButton7.Enabled = true;
                    this.glassButton8.Enabled = true;
                    this.glassButton9.Enabled = true;
                    this.glassButton10.Enabled = true;
                    break;
            }
            readFireData(_nodeNo, 1);
            readAirData(_nodeNo, 1);
        }

        private void readFireData(string nodeNo, int chanel)
        {
            S_fire1.Points.Clear();
            S_fire2.Points.Clear();
            S_fire3.Points.Clear();
            S_fch.Points.Clear();
            S_fch.Name = "通道" + chanel.ToString();
            string filename = AppDomain.CurrentDomain.BaseDirectory + "Curve\\" + nodeNo + "_fire_ch" + chanel.ToString() + "_" + _Date + ".lin";
            if (File.Exists(filename))
            {
                try
                {
                    // Create an instance of StreamReader to read from a file.
                    // The using statement also closes the StreamReader.
                    using (StreamReader sr = new StreamReader(filename))
                    {
                        string line;
                        // Read and display lines from the file until the end of 
                        // the file is reached.
                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] sline = line.Split(',');
                            DataPoint dp1 = new DataPoint(double.Parse(sline[4]), double.Parse(sline[0]));
                            S_fire1.Points.Add(dp1);
                            DataPoint dp2 = new DataPoint(double.Parse(sline[4]), double.Parse(sline[1]));
                            S_fire2.Points.Add(dp2);
                            DataPoint dp3 = new DataPoint(double.Parse(sline[4]), double.Parse(sline[2]));
                            S_fire3.Points.Add(dp3);
                            DataPoint dpValue = new DataPoint(double.Parse(sline[4]), double.Parse(sline[3]));
                            S_fch.Points.Add(dpValue);
                        }
                    }
                }
                catch
                {

                }
                finally
                {
                    chart.Invalidate();
                }
            }
            //}
        }

        private void readAirData(string nodeNo, int channel)
        {
            S_airlow.Points.Clear();
            S_airhigh.Points.Clear();
            S_ach.Points.Clear();
            S_ach.Name = "管路" + channel.ToString();
            string filename = AppDomain.CurrentDomain.BaseDirectory + "Curve\\" + nodeNo + "_air_ch" + channel.ToString() + "_" + _Date + ".lin";
            if (File.Exists(filename))
            {
                try
                {
                    // Create an instance of StreamReader to read from a file.
                    // The using statement also closes the StreamReader.
                    using (StreamReader sr = new StreamReader(filename))
                    {
                        string line;
                        // Read and display lines from the file until the end of 
                        // the file is reached.
                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] sline = line.Split(',');
                            DataPoint dp1 = new DataPoint(double.Parse(sline[3]), double.Parse(sline[0]));
                            S_airlow.Points.Add(dp1);

                            DataPoint dp2 = new DataPoint(double.Parse(sline[3]), double.Parse(sline[1]));
                            S_airhigh.Points.Add(dp2);

                            DataPoint dpValue = new DataPoint(double.Parse(sline[3]), double.Parse(sline[2]));

                            S_ach.Points.Add(dpValue);
                        }
                    }
                }
                catch
                {

                }
                finally
                {
                    chart.Invalidate();
                }
            }
        }

        private void setChart()
        {
            S_fire1 = new Series("一级火警阈值");
            S_fire2 = new Series("二级火警阈值");
            S_fire3 = new Series("三级火警阈值");
            S_fch = new Series("通道");
            S_fire1.Color = Color.Pink;
            S_fire2.Color = Color.LightCoral;
            S_fire3.Color = Color.Red;
            S_fch.Color = Color.Blue;
            S_fire1.ChartArea = S_fire2.ChartArea = S_fire3.ChartArea = S_fch.ChartArea = "ChartArea1";
            S_airlow = new Series("气流低阈值");
            S_airhigh = new Series("气流高阈值");
            S_ach = new Series("管路");
            S_airlow.Color = Color.Yellow;
            S_airhigh.Color = Color.Orange;
            S_ach.Color = Color.Blue;
            S_airlow.ChartArea = S_airhigh.ChartArea = S_ach.ChartArea = "ChartArea2";
            this.chart.Series.Add(S_fire3);
            this.chart.Series.Add(S_fire2);
            this.chart.Series.Add(S_fire1);
            this.chart.Series.Add(S_airhigh);
            this.chart.Series.Add(S_airlow);
            
            this.chart.Series.Add(S_fch);
            this.chart.Series.Add(S_ach);

            foreach (Series s in this.chart.Series)
            {
                s.XValueType = ChartValueType.Time;
                s.YValueType = ChartValueType.Int32;               
            }

            for (int i = 0; i < 5; i++)
            {
                chart.Series[i].ChartType = SeriesChartType.StepLine; 
                chart.Series[i].IsValueShownAsLabel = false;
                chart.Series[i].LabelForeColor = Color.DarkGreen;
            }

            for (int i = 5; i < 7; i++)
            {
                chart.Series[i].IsValueShownAsLabel = false;
                chart.Series[i].LabelForeColor = Color.Blue;
            }


            // Set automatic zooming
            chart.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chart.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            // Set automatic scrolling 
            chart.ChartAreas[0].CursorX.AutoScroll = true;
            chart.ChartAreas[0].CursorY.AutoScroll = true;

            // Set automatic zooming
            chart.ChartAreas[1].AxisX.ScaleView.Zoomable = true;
            chart.ChartAreas[1].AxisY.ScaleView.Zoomable = true;
            // Set automatic scrolling 
            chart.ChartAreas[1].CursorX.AutoScroll = true;
            chart.ChartAreas[1].CursorY.AutoScroll = true;

            S_fch.ChartType = S_ach.ChartType = SeriesChartType.Line;
            S_fch.MarkerStyle = S_ach.MarkerStyle = MarkerStyle.Circle;
            S_fch.MarkerSize = S_ach.MarkerSize = 3;
            S_fch.MarkerColor = S_ach.MarkerColor = Color.Magenta;
            S_fch.MarkerBorderColor = S_ach.MarkerBorderColor = Color.Blue;
            S_fch.MarkerBorderWidth = S_ach.MarkerBorderWidth = 1;

            chart.Legends[0].LegendStyle = LegendStyle.Row;
            chart.Legends[0].BorderDashStyle = ChartDashStyle.Solid;
            chart.Legends[0].BorderColor = Color.MidnightBlue;
            chart.Legends[0].ShadowOffset = 2;
        }

        private void frmCurve_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            readFireData(_nodeNo, 1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            readFireData(_nodeNo, 2);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            readFireData(_nodeNo, 3);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            readFireData(_nodeNo, 4);
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            _Date = dateTimePicker1.Value.ToString("yyyyMMdd");
            chart.Titles[0].Text = _Date + " 曲线图";
            readFireData(_nodeNo, 1);
            readAirData(_nodeNo, 1);
        }

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    this.chart.Printing.PrintPreview();
        //}

        //private void button10_Click(object sender, EventArgs e)
        //{
        //    this.chart.Printing.PageSetup();
        //}

        private void glassButton1_Click(object sender, EventArgs e)
        {
            this.dateTimePicker1.Value = this.dateTimePicker1.Value.AddDays(-1);
        }

        private void glassButton2_Click(object sender, EventArgs e)
        {
            this.dateTimePicker1.Value = this.dateTimePicker1.Value.AddDays(1);
        }

        private void glassButton3_Click(object sender, EventArgs e)
        {
            readFireData(_nodeNo, 1);
        }

        private void glassButton4_Click(object sender, EventArgs e)
        {
            readFireData(_nodeNo, 2);
        }

        private void glassButton5_Click(object sender, EventArgs e)
        {
            readFireData(_nodeNo, 3);
        }

        private void glassButton6_Click(object sender, EventArgs e)
        {
            readFireData(_nodeNo, 4);
        }

        private void glassButton7_Click(object sender, EventArgs e)
        {
            readAirData(_nodeNo, 1);
        }

        private void glassButton8_Click(object sender, EventArgs e)
        {
            readAirData(_nodeNo, 2);
        }

        private void glassButton9_Click(object sender, EventArgs e)
        {
            readAirData(_nodeNo, 3);
        }

        private void glassButton10_Click(object sender, EventArgs e)
        {
            readAirData(_nodeNo, 4);
        }

        private void glassButton11_Click(object sender, EventArgs e)
        {
            this.chart.Printing.PageSetup();
        }

        private void glassButton12_Click(object sender, EventArgs e)
        {
            this.chart.Printing.PrintPreview();
        }

        private void glassButton13_Click(object sender, EventArgs e)
        {
            this.chart.Printing.Print(true);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                for (int i = 0; i < 5; i++)
                {
                    chart.Series[i].IsValueShownAsLabel = true;                    
                }
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    chart.Series[i].IsValueShownAsLabel = false;
                }
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                for (int i = 5; i < 7; i++)
                {
                    chart.Series[i].IsValueShownAsLabel = true;
                }
            }
            else
            {
                for (int i = 5; i < 7; i++)
                {
                    chart.Series[i].IsValueShownAsLabel = false;
                }
            }
        }

    }
}
