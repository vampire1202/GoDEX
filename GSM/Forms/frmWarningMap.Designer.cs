namespace GSM.Forms
{
    partial class frmWarningMap
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblWarningMsg = new System.Windows.Forms.Label();
            this.txtWarningMsg = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel2 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.picZoneMap)).BeginInit();
            this.panel1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // picZoneMap
            // 
            this.picZoneMap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picZoneMap.Enabled = false;
            this.picZoneMap.Location = new System.Drawing.Point(0, 0);
            this.picZoneMap.Name = "picZoneMap";
            this.picZoneMap.Size = new System.Drawing.Size(122, 110);
            this.picZoneMap.TabIndex = 0;
            this.picZoneMap.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.picZoneMap);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(247, 154);
            this.panel1.TabIndex = 1;
            // 
            // lblWarningMsg
            // 
            this.lblWarningMsg.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblWarningMsg.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblWarningMsg.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblWarningMsg.ForeColor = System.Drawing.Color.Black;
            this.lblWarningMsg.Location = new System.Drawing.Point(0, 0);
            this.lblWarningMsg.Name = "lblWarningMsg";
            this.lblWarningMsg.Size = new System.Drawing.Size(269, 235);
            this.lblWarningMsg.TabIndex = 2;
            this.lblWarningMsg.Text = "报警信息";
            // 
            // txtWarningMsg
            // 
            this.txtWarningMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtWarningMsg.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtWarningMsg.Location = new System.Drawing.Point(0, 235);
            this.txtWarningMsg.Multiline = true;
            this.txtWarningMsg.Name = "txtWarningMsg";
            this.txtWarningMsg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtWarningMsg.Size = new System.Drawing.Size(269, 171);
            this.txtWarningMsg.TabIndex = 3;
            this.txtWarningMsg.Text = "确认描述:空";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(29, 17);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "确认报警";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(167, 17);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.txtWarningMsg);
            this.splitContainer1.Panel2.Controls.Add(this.panel2);
            this.splitContainer1.Panel2.Controls.Add(this.lblWarningMsg);
            this.splitContainer1.Size = new System.Drawing.Size(739, 458);
            this.splitContainer1.SplitterDistance = 466;
            this.splitContainer1.TabIndex = 5;
            this.splitContainer1.Resize += new System.EventHandler(this.splitContainer1_Resize);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnOk);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 406);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(269, 52);
            this.panel2.TabIndex = 5;
            // 
            // frmWarningMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(739, 458);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MaximizeBox = false;
            this.Name = "frmWarningMap";
            this.Text = "报警信息";
            this.Load += new System.EventHandler(this.frmZoneMap_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmWarningMap_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.picZoneMap)).EndInit();
            this.panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picZoneMap;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtWarningMsg;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel2;
        public System.Windows.Forms.Label lblWarningMsg;
    }
}