using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Labeler
{
	/// <summary>
	/// Summary description for frmProgress.
	/// </summary>
	public class frmProgress : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ProgressBar Progress;
		private System.Windows.Forms.Label lbtitle;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmProgress()
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
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmProgress));
            this.Progress = new System.Windows.Forms.ProgressBar();
            this.lbtitle = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Progress
            // 
            resources.ApplyResources(this.Progress, "Progress");
            this.Progress.Name = "Progress";
            // 
            // lbtitle
            // 
            resources.ApplyResources(this.lbtitle, "lbtitle");
            this.lbtitle.Name = "lbtitle";
            // 
            // frmProgress
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.lbtitle);
            this.Controls.Add(this.Progress);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmProgress";
            this.Load += new System.EventHandler(this.frmProgress_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmProgress_Closing);
            this.ResumeLayout(false);

		}
		#endregion


		public void SetProgress(string title,double perc)
		{	
			this.lbtitle.Text = title;
			Progress.Value = (int)perc;
			this.Refresh();
		}

		private void frmProgress_Load(object sender, System.EventArgs e)
		{
		
		}

		private void frmProgress_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			e.Cancel = true;
		}


	}
}
