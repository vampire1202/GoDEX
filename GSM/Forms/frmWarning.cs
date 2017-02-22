using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GSM.Forms
{
    public partial class frmWarning : Form
    {
        public frmWarning()
        {
            InitializeComponent();
        }
        
        void BtnCancelClick(object sender, EventArgs e)
        {
        	this.Dispose();
        }
        
        void BtnOkClick(object sender, EventArgs e)
        {
        	SetAlarm();
        	this.Dispose();
        }
        
        void SetAlarm()
        {
			Cls.RWconfig.SetAppSettings("Warning1",this.txtWarning1.Text);
			Cls.RWconfig.SetAppSettings("Warning2",this.txtWarning2.Text);
			Cls.RWconfig.SetAppSettings("AireLow1",this.txtAirLow1.Text);
			Cls.RWconfig.SetAppSettings("AireLow2",this.txtAirLow2.Text);
			Cls.RWconfig.SetAppSettings("AireHigh1",this.txtAirHigh1.Text);
			Cls.RWconfig.SetAppSettings("AireHigh2",this.txtAirHigh2.Text);
			Cls.RWconfig.SetAppSettings("Fire11",this.txtFire11.Text);
			Cls.RWconfig.SetAppSettings("Fire12",this.txtFire12.Text);
			Cls.RWconfig.SetAppSettings("Fire13",this.txtFire13.Text);
			Cls.RWconfig.SetAppSettings("Fire21",this.txtFire21.Text );
			Cls.RWconfig.SetAppSettings("Fire22",this.txtFire22.Text);
			Cls.RWconfig.SetAppSettings("Fire23",this.txtFire23.Text);
			Cls.RWconfig.SetAppSettings("Fire31",this.txtFire31.Text);
			Cls.RWconfig.SetAppSettings("Fire32",this.txtFire32.Text);
			Cls.RWconfig.SetAppSettings("Fire33",this.txtFire33.Text);

            Cls.RWconfig.SetAppSettings("Fire41", this.txtFire41.Text);
            Cls.RWconfig.SetAppSettings("Fire42", this.txtFire42.Text);
            Cls.RWconfig.SetAppSettings("Fire43", this.txtFire43.Text);

			Cls.RWconfig.SetAppSettings("AlarmNum",this.txtAlarmNum.Text);
        }
        
        void FrmWarningLoad(object sender, EventArgs e)
        {
        	GetAlarmSet();
        }
        
        void GetAlarmSet()
        {
        	this.txtAlarmNum.Text = Cls.RWconfig.GetAppSettings("AlarmNum");
        	this.txtWarning1.Text = Cls.RWconfig.GetAppSettings("Warning1");
        	this.txtWarning2.Text = Cls.RWconfig.GetAppSettings("Warning2");
        	this.txtAirLow1.Text = Cls.RWconfig.GetAppSettings("AireLow1");
        	this.txtAirLow2.Text = Cls.RWconfig.GetAppSettings("AireLow2");
        	this.txtAirHigh1.Text = Cls.RWconfig.GetAppSettings("AireHigh1");
        	this.txtAirHigh2.Text = Cls.RWconfig.GetAppSettings("AireHigh2");
        	this.txtFire11.Text = Cls.RWconfig.GetAppSettings("Fire11");
        	this.txtFire12.Text = Cls.RWconfig.GetAppSettings("Fire12");
        	this.txtFire13.Text = Cls.RWconfig.GetAppSettings("Fire13");
        	this.txtFire21.Text = Cls.RWconfig.GetAppSettings("Fire21");
        	this.txtFire22.Text = Cls.RWconfig.GetAppSettings("Fire22");
        	this.txtFire23.Text = Cls.RWconfig.GetAppSettings("Fire23");
        	this.txtFire31.Text = Cls.RWconfig.GetAppSettings("Fire31");
            this.txtFire32.Text = Cls.RWconfig.GetAppSettings("Fire32");
            this.txtFire33.Text = Cls.RWconfig.GetAppSettings("Fire33");
            this.txtFire41.Text = Cls.RWconfig.GetAppSettings("Fire41");
            this.txtFire42.Text = Cls.RWconfig.GetAppSettings("Fire42");
            this.txtFire43.Text = Cls.RWconfig.GetAppSettings("Fire43");
        }
        
        void TxtAlarmNumTextChanged(object sender, EventArgs e)
        {
        	 if (!GSM.Cls.Method.IsInt(this.txtAlarmNum.Text))
            {
                txtAlarmNum.Text = "1";
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
