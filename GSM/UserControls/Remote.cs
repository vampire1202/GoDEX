using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GSM.UserControls
{
    public partial class Remote : UserControl
    {
        /// <summary>
        /// 监测器编号
        /// </summary>       
        public string Detector
        {
            get { return this.detector.Text; }
            set { this.detector.Text = value; }
        }

        /// <summary>
        /// 灵敏度
        /// </summary>
        public double Sensitivity
        {
            get { return double.Parse(this.sensitivity.Text); }
            set { this.sensitivity.Text = value.ToString(); }
        }

        /// <summary>
        /// 均值
        /// </summary>
        public double Mean
        {
            get { return double.Parse(this.mean.Text); }
            set { this.mean.Text = value.ToString(); }
        }

        /// <summary>
        /// 方差
        /// </summary>
        public double Variance
        {
            get { return double.Parse(this.variance.Text); }
            set { this.variance.Text = value.ToString(); }
        }

        /// <summary>
        /// 快速学习
        /// </summary>
        public string FastLearn
        {
            get { return this.fastLearn.Text; }
            set { this.fastLearn.Text = value; }
        }

        /// <summary>
        /// 日夜
        /// </summary>
        public string DayLight
        {
            get { return  this.dayLight.Text ; }
            set { this.dayLight.Text = value ; }
        }

        /// <summary>
        /// 误报概率
        /// </summary>
        public int Probability
        {
            get { return int.Parse(this.probability.Text); }
            set { this.probability.Text = value.ToString(); }
        }

        /// <summary>
        /// 预警1
        /// </summary>
        public double PreAlarm1
        {
            get { return double.Parse(this.preAlarm1.Text); }
            set { this.preAlarm1.Text = value.ToString(); }
        }
        
        /// <summary>
        /// 预警2
        /// </summary>
        public double PreAlarm2
        {
            get { return double.Parse(this.preAlarm2.Text); }
            set { this.preAlarm2.Text = value.ToString(); }
        }
        
        /// <summary>
        /// 火警级别1
        /// </summary>
        public double FireLevel1
        {
            get { return double.Parse(this.fireLevel1.Text); }
            set { this.fireLevel1.Text = value.ToString(); }
        }
        
        /// <summary>
        /// 火警级别2
        /// </summary>
        public double FireLevel2
        {
            get { return double.Parse(this.fireLevel2.Text); }
            set { this.fireLevel2.Text = value.ToString(); }
        }     


        public Remote()
        {
            InitializeComponent();
        }

        private void Remote_Load(object sender, EventArgs e)
        {

        }

        private void Sensitivity_Click(object sender, EventArgs e)
        {

        }

        private void Probability_Click(object sender, EventArgs e)
        {

        }
    }
}
