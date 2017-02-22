/*
 * Created by SharpDevelop.
 * User: Vampire
 * Date: 2010/8/17
 * Time: 21:09
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GSM.Forms
{
	/// <summary>
	/// Description of frmFront.
	/// </summary>
	public partial class frmFront : Form
	{
		public frmFront()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}

        public int ProgressValue
        {
            get { return this.verticalProgressBar1.Value; }
            set { this.verticalProgressBar1.Value = value; }
        }

	}
}
