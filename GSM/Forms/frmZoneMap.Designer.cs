namespace GSM.Forms
{
    partial class frmZoneMap
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.picZoneMap = new System.Windows.Forms.PictureBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbtnEdit = new System.Windows.Forms.ToolStripButton();
            this.tsbtnLock = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.picZoneMap)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // picZoneMap
            // 
            this.picZoneMap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picZoneMap.Enabled = false;
            this.picZoneMap.Location = new System.Drawing.Point(0, 34);
            this.picZoneMap.Name = "picZoneMap";
            this.picZoneMap.Size = new System.Drawing.Size(167, 132);
            this.picZoneMap.TabIndex = 0;
            this.picZoneMap.TabStop = false;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbtnEdit,
            this.tsbtnLock});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(767, 31);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbtnEdit
            // 
            this.tsbtnEdit.Image = global::GSM.Properties.Resources.edit;
            this.tsbtnEdit.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbtnEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnEdit.Name = "tsbtnEdit";
            this.tsbtnEdit.Size = new System.Drawing.Size(60, 28);
            this.tsbtnEdit.Text = "编辑";
            this.tsbtnEdit.Click += new System.EventHandler(this.tsbtnEdit_Click);
            // 
            // tsbtnLock
            // 
            this.tsbtnLock.Enabled = false;
            this.tsbtnLock.Image = global::GSM.Properties.Resources._lock;
            this.tsbtnLock.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbtnLock.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnLock.Name = "tsbtnLock";
            this.tsbtnLock.Size = new System.Drawing.Size(55, 28);
            this.tsbtnLock.Text = "锁定";
            this.tsbtnLock.Click += new System.EventHandler(this.tsbtnLock_Click);
            // 
            // frmZoneMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(767, 571);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.picZoneMap);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MaximizeBox = false;
            this.Name = "frmZoneMap";
            this.Text = "区域地图";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmZoneMap_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picZoneMap)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picZoneMap;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbtnEdit;
        private System.Windows.Forms.ToolStripButton tsbtnLock;
    }
}