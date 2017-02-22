/*
 * Created by SharpDevelop.
 * User: Vampire
 * Date: 2010/8/17
 * Time: 21:09
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace GSM.Forms
{
	partial class frmFront
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFront));
			this.panel4 = new System.Windows.Forms.Panel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.panel3 = new System.Windows.Forms.Panel();
			this.verticalProgressBar1 = new VerticalProgressBar.VerticalProgressBar();
			this.SuspendLayout();
			// 
			// panel4
			// 
			this.panel4.Location = new System.Drawing.Point(69, 131);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(17, 19);
			this.panel4.TabIndex = 1;
			// 
			// panel1
			// 
			this.panel1.Location = new System.Drawing.Point(69, 179);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(17, 19);
			this.panel1.TabIndex = 1;
			// 
			// panel2
			// 
			this.panel2.Location = new System.Drawing.Point(69, 231);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(17, 19);
			this.panel2.TabIndex = 1;
			// 
			// panel3
			// 
			this.panel3.BackColor = System.Drawing.Color.Yellow;
			this.panel3.ForeColor = System.Drawing.Color.PaleGoldenrod;
			this.panel3.Location = new System.Drawing.Point(69, 279);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(17, 19);
			this.panel3.TabIndex = 1;
			// 
			// verticalProgressBar1
			// 
			this.verticalProgressBar1.BorderStyle = VerticalProgressBar.BorderStyles.Classic;
			this.verticalProgressBar1.Color = System.Drawing.Color.Navy;
			this.verticalProgressBar1.Location = new System.Drawing.Point(17, 127);
			this.verticalProgressBar1.Maximum = 100;
			this.verticalProgressBar1.Minimum = 0;
			this.verticalProgressBar1.Name = "verticalProgressBar1";
			this.verticalProgressBar1.Size = new System.Drawing.Size(20, 183);
			this.verticalProgressBar1.Step = 10;
			this.verticalProgressBar1.Style = VerticalProgressBar.Styles.Classic;
			this.verticalProgressBar1.TabIndex = 2;
			this.verticalProgressBar1.Value = 50;
			// 
			// frmFront
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.ClientSize = new System.Drawing.Size(167, 330);
			this.ControlBox = false;
			this.Controls.Add(this.verticalProgressBar1);
			this.Controls.Add(this.panel3);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.panel4);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "frmFront";
			this.Text = "前置面板";
			this.ResumeLayout(false);
		}
		private VerticalProgressBar.VerticalProgressBar verticalProgressBar1;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel4;
	}
}
