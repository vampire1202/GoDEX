/*
 * Created by SharpDevelop.
 * User: Vampire
 * Date: 2010/11/19
 * Time: 20:35
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace GSM.UserControls
{
	partial class NodeSite
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the control.
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
            this.lblNo = new System.Windows.Forms.Label();
            this.pbStatus = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbStatus)).BeginInit();
            this.SuspendLayout();
            // 
            // lblNo
            // 
            this.lblNo.BackColor = System.Drawing.Color.Transparent;
            this.lblNo.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblNo.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblNo.Location = new System.Drawing.Point(6, 3);
            this.lblNo.Name = "lblNo";
            this.lblNo.Size = new System.Drawing.Size(27, 14);
            this.lblNo.TabIndex = 0;
            this.lblNo.Text = "123";
            this.lblNo.Click += new System.EventHandler(this.lblNo_Click);
            this.lblNo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LblNoMouseDown);
            // 
            // pbStatus
            // 
            this.pbStatus.BackColor = System.Drawing.Color.Transparent;
            this.pbStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbStatus.Location = new System.Drawing.Point(0, 0);
            this.pbStatus.Name = "pbStatus";
            this.pbStatus.Size = new System.Drawing.Size(40, 36);
            this.pbStatus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbStatus.TabIndex = 1;
            this.pbStatus.TabStop = false;
            this.pbStatus.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            // 
            // NodeSite
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.BackgroundImage = global::GSM.Properties.Resources.detector;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.lblNo);
            this.Controls.Add(this.pbStatus);
            this.DoubleBuffered = true;
            this.Name = "NodeSite";
            this.Size = new System.Drawing.Size(40, 36);
            this.Load += new System.EventHandler(this.NodeSite_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbStatus)).EndInit();
            this.ResumeLayout(false);

        }
        public System.Windows.Forms.Label lblNo;
        public System.Windows.Forms.PictureBox pbStatus;
	}
}
