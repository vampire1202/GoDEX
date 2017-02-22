using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Threading;
using System.Windows.Forms.DataVisualization.Charting;

namespace GSM.UserControls
{
	/// <summary>
	/// Summary description for RealTimeSample.
	/// </summary>
	public class RealTimeSample : System.Windows.Forms.UserControl
	{
		#region Fields

		// Chart data adding thread
		private Thread addDataRunner;

		// Thread Add Data delegate
		public delegate void AddDataDelegate();
		public AddDataDelegate addDataDel;

		// Chart control
		private System.Windows.Forms.DataVisualization.Charting.Chart chart1;

		// Form fields
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Button startTrending;
		private System.Windows.Forms.Button stopTrending;
		private DateTime minValue, maxValue;
        private Random rand = new Random();
		private System.Windows.Forms.Panel panel1;

        //一级火警阈值
        private int _fire1=0;
        public int _Fire1
        {
            get { return _fire1; }
            set { _fire1 = value; }
        }

        //二级火警阈值
        private int _fire2=0;
        public int _Fire2
        {
            get { return _fire2; }
            set { _fire2 = value; }
        }

        //3级火警阈值
        private int _fire3=0;
        public int _Fire3
        {
            get { return _fire3; }
            set { _fire3 = value; }
        }       


		#endregion // Fields

		#region Construction and Disposing

		public RealTimeSample()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			// Abort thread
			if ( (addDataRunner.ThreadState & ThreadState.Suspended) == ThreadState.Suspended)
			{
				addDataRunner.Resume();
			}
			addDataRunner.Abort();

			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#endregion // Construction and Disposing

		#region Form user event handlers

		/// <summary>
		/// Page load event handler.
		/// </summary>
		/// <param name="sender">Event sender.</param>
		/// <param name="e">Event arguments.</param>
		private void RealTimeSample_Load(object sender, System.EventArgs e)
		{
           
			ThreadStart addDataThreadStart = new ThreadStart(AddDataThreadLoop);
			addDataRunner = new Thread(addDataThreadStart);

			addDataDel += new AddDataDelegate(AddData);

			this.startTrending_Click( null, EventArgs.Empty);
		}

		/// <summary>
		/// Start real time data simulator.
		/// </summary>
		/// <param name="sender">Event sender.</param>
		/// <param name="e">Event arguments.</param>
		private void startTrending_Click(object sender, System.EventArgs e)
		{
			// Disable all controls on the form
			startTrending.Enabled = false;
			// and only Enable the Stop button
			stopTrending.Enabled = true;

			// Predefine the viewing area of the chart
			minValue = DateTime.Now;
			maxValue = minValue.AddSeconds(1);

			chart1.ChartAreas[0].AxisX.Minimum = minValue.ToOADate();
			chart1.ChartAreas[0].AxisX.Maximum = maxValue.ToOADate();

			// Reset number of series in the chart.
			chart1.Series.Clear();

			Series newSeries = new Series( "烟雾浓度实时曲线图" );
			newSeries.ChartType = SeriesChartType.Spline;
			newSeries.BorderWidth = 1;
            newSeries.Color = Color.Blue;
			newSeries.ShadowOffset = 1;
			newSeries.XValueType = ChartValueType.DateTime;
			chart1.Series.Add( newSeries );

            Series newSeriesA1 = new Series("A1级火警阈值:"+_Fire1.ToString());//110
            newSeriesA1.ChartType = SeriesChartType.Spline;
            newSeriesA1.BorderWidth = 2;
            newSeriesA1.Color = Color.Pink;
            newSeriesA1.ShadowOffset = 1;
            newSeriesA1.ToolTip = _Fire1.ToString();
            newSeriesA1.XValueType = ChartValueType.DateTime;
            chart1.Series.Add(newSeriesA1);

            Series newSeriesA2 = new Series("A2级火警阈值:"+_Fire2.ToString());//120
            newSeriesA2.ChartType = SeriesChartType.Spline;
            newSeriesA2.BorderWidth = 2;
            newSeriesA2.Color = Color.LightCoral;
            newSeriesA2.ShadowOffset = 1;
            newSeriesA1.ToolTip = _Fire2.ToString();
            newSeriesA2.XValueType = ChartValueType.DateTime;
            chart1.Series.Add(newSeriesA2);

            Series newSeriesA3 = new Series("A3级火警阈值:"+_Fire3.ToString());//130
            newSeriesA3.ChartType = SeriesChartType.Spline;
            newSeriesA3.BorderWidth = 2;
            newSeriesA3.Color = Color.Red;
            newSeriesA3.ShadowOffset = 1;
            newSeriesA1.ToolTip = _Fire3.ToString();
            newSeriesA3.XValueType = ChartValueType.DateTime;
            chart1.Series.Add(newSeriesA3);

			// start worker threads.
			if ( addDataRunner.IsAlive == true )
			{
				addDataRunner.Resume();
			}
			else
			{
				addDataRunner.Start();
			}
		}

		/// <summary>
		/// Stop real time data simulator.
		/// </summary>
		/// <param name="sender">Event sender.</param>
		/// <param name="e">Event arguments.</param>
		private void stopTrending_Click(object sender, System.EventArgs e)
		{
			// Suspend thread
			if ( addDataRunner.IsAlive == true )
			{
				addDataRunner.Suspend();
			}

			// Enable all controls on the form
			startTrending.Enabled = true;

			// Disable the Stop button
			stopTrending.Enabled = false;
		}

		#endregion

		#region Add new data thread

		/// <summary>
		/// Main loop for the thread that adds data to the chart.
		/// The main purpose of this function is to Invoke AddData
		/// function every 1000ms (1 second).
		/// </summary>
		private void AddDataThreadLoop()
		{
			try
			{
				while (true)
				{
					// Invoke method must be used to interact with the chart
					// control on the form!
					chart1.Invoke(addDataDel);
					// Thread is inactive for 200ms
					Thread.Sleep(200);
				}
			}
			catch
			{
				// Thread is aborted
			}
		}

		/// <summary>
		/// Method which is invoked from the thread to add series points
		/// </summary>
		public void AddData()
		{
			DateTime timeStamp = DateTime.Now;

			AddNewPoint( timeStamp, chart1.Series[0]);
            AddValuePoint(timeStamp, chart1.Series[1], _Fire1);
            AddValuePoint(timeStamp, chart1.Series[2], _Fire2);
            AddValuePoint(timeStamp, chart1.Series[3], _Fire3);
		}

        public void AddValuePoint(DateTime timeStamp, System.Windows.Forms.DataVisualization.Charting.Series ptSeries,int value)
        {
            // Add new data point to its series.
            ptSeries.Points.AddXY(timeStamp.ToOADate(), value);

            // remove all points from the source series older than 20 seconds.
            double removeBefore = timeStamp.AddSeconds((double)(25) * (-1)).ToOADate();


            //remove oldest values to maintain a constant number of data points
            while (ptSeries.Points[0].XValue < removeBefore)
            {
                ptSeries.Points.RemoveAt(0);
            }
            
        }

		/// <summary>
		/// The AddNewPoint function is called for each series in the chart when
		/// new points need to be added.  The new point will be placed at specified
		/// X axis (Date/Time) position with a Y value in a range +/- 1 from the previous
		/// data point's Y value, and not smaller than zero.
		/// </summary>
		/// <param name="timeStamp"></param>
		/// <param name="ptSeries"></param>
		public void AddNewPoint( DateTime timeStamp, System.Windows.Forms.DataVisualization.Charting.Series ptSeries )
		{
			// Add new data point to its series.
			ptSeries.Points.AddXY( timeStamp.ToOADate(), rand.Next(80, 83));
            
            chart1.ChartAreas[0].AxisX.Minimum = ptSeries.Points[0].XValue;
            chart1.ChartAreas[0].AxisY.Maximum = 180;
			chart1.ChartAreas[0].AxisX.Maximum = DateTime.FromOADate(ptSeries.Points[0].XValue).AddSeconds(30).ToOADate();

			chart1.Invalidate();
			// remove all points from the source series older than 20 seconds.
			double removeBefore = timeStamp.AddSeconds( (double)(25) * ( -1 )).ToOADate();

			//remove oldest values to maintain a constant number of data points
			while ( ptSeries.Points[0].XValue < removeBefore )
			{
				ptSeries.Points.RemoveAt(0);
			}

			
		}

		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.startTrending = new System.Windows.Forms.Button();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.stopTrending = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // startTrending
            // 
            this.startTrending.BackColor = System.Drawing.SystemColors.Control;
            this.startTrending.Location = new System.Drawing.Point(6, 3);
            this.startTrending.Name = "startTrending";
            this.startTrending.Size = new System.Drawing.Size(72, 24);
            this.startTrending.TabIndex = 1;
            this.startTrending.Text = "&Start";
            this.startTrending.UseVisualStyleBackColor = false;
            this.startTrending.Click += new System.EventHandler(this.startTrending_Click);
            // 
            // chart1
            // 
            this.chart1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(223)))), ((int)(((byte)(240)))));
            this.chart1.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.TopBottom;
            this.chart1.BackSecondaryColor = System.Drawing.Color.White;
            this.chart1.BorderlineColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(59)))), ((int)(((byte)(105)))));
            this.chart1.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            this.chart1.BorderSkin.SkinStyle = System.Windows.Forms.DataVisualization.Charting.BorderSkinStyle.Emboss;
            chartArea1.Area3DStyle.Inclination = 15;
            chartArea1.Area3DStyle.IsClustered = true;
            chartArea1.Area3DStyle.IsRightAngleAxes = false;
            chartArea1.Area3DStyle.Perspective = 10;
            chartArea1.Area3DStyle.Rotation = 10;
            chartArea1.Area3DStyle.WallWidth = 0;
            chartArea1.AxisX.LabelStyle.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Bold);
            chartArea1.AxisX.LabelStyle.Format = "hh:mm:ss";
            chartArea1.AxisX.LabelStyle.Interval = 10;
            chartArea1.AxisX.LabelStyle.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Seconds;
            chartArea1.AxisX.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.AxisX.MajorGrid.Interval = 10;
            chartArea1.AxisX.MajorGrid.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Seconds;
            chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.AxisX.MajorTickMark.Interval = 10;
            chartArea1.AxisX.MajorTickMark.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Seconds;
            chartArea1.AxisY.IsLabelAutoFit = false;
            chartArea1.AxisY.IsStartedFromZero = false;
            chartArea1.AxisY.LabelStyle.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Bold);
            chartArea1.AxisY.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.AxisY.Maximum = 25;
            chartArea1.AxisY.Minimum = 5;
            chartArea1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(165)))), ((int)(((byte)(191)))), ((int)(((byte)(228)))));
            chartArea1.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.TopBottom;
            chartArea1.BackSecondaryColor = System.Drawing.Color.White;
            chartArea1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            chartArea1.InnerPlotPosition.Auto = false;
            chartArea1.InnerPlotPosition.Height = 85F;
            chartArea1.InnerPlotPosition.Width = 86F;
            chartArea1.InnerPlotPosition.X = 8.3969F;
            chartArea1.InnerPlotPosition.Y = 3.63068F;
            chartArea1.Name = "Default";
            chartArea1.Position.Auto = false;
            chartArea1.Position.Height = 86.76062F;
            chartArea1.Position.Width = 88F;
            chartArea1.Position.X = 5.089137F;
            chartArea1.Position.Y = 5.895753F;
            chartArea1.ShadowColor = System.Drawing.Color.Transparent;
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Alignment = System.Drawing.StringAlignment.Far;
            legend1.BackColor = System.Drawing.Color.Transparent;
            legend1.DockedToChartArea = "Default";
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            legend1.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Bold);
            legend1.IsTextAutoFit = false;
            legend1.LegendStyle = System.Windows.Forms.DataVisualization.Charting.LegendStyle.Row;
            legend1.Name = "Default";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(0, 0);
            this.chart1.Name = "chart1";
            series1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(26)))), ((int)(((byte)(59)))), ((int)(((byte)(105)))));
            series1.ChartArea = "Default";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(64)))), ((int)(((byte)(10)))));
            series1.Legend = "Default";
            series1.Name = "Series1";
            series1.ShadowOffset = 1;
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(648, 386);
            this.chart1.TabIndex = 13;
            // 
            // stopTrending
            // 
            this.stopTrending.BackColor = System.Drawing.SystemColors.Control;
            this.stopTrending.Enabled = false;
            this.stopTrending.Location = new System.Drawing.Point(6, 33);
            this.stopTrending.Name = "stopTrending";
            this.stopTrending.Size = new System.Drawing.Size(72, 23);
            this.stopTrending.TabIndex = 14;
            this.stopTrending.Text = "St&op";
            this.stopTrending.UseVisualStyleBackColor = false;
            this.stopTrending.Click += new System.EventHandler(this.stopTrending_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.stopTrending);
            this.panel1.Controls.Add(this.startTrending);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(648, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(80, 386);
            this.panel1.TabIndex = 29;
            // 
            // RealTimeSample
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Verdana", 9F);
            this.Name = "RealTimeSample";
            this.Size = new System.Drawing.Size(728, 386);
            this.Load += new System.EventHandler(this.RealTimeSample_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion
	}
}
