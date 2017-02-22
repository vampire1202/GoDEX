namespace GSM.Forms
{
    partial class frmPort
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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cmbPort = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbPortBack = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.udordertimespan = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cmbRate2 = new System.Windows.Forms.ComboBox();
            this.cmbRate1 = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.udofflinetime = new System.Windows.Forms.NumericUpDown();
            this.udLunxunTime = new System.Windows.Forms.NumericUpDown();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udordertimespan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udofflinetime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udLunxunTime)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(116, 251);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(74, 27);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(204, 251);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(74, 27);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cmbPort
            // 
            this.cmbPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPort.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbPort.FormattingEnabled = true;
            this.cmbPort.Location = new System.Drawing.Point(94, 28);
            this.cmbPort.Name = "cmbPort";
            this.cmbPort.Size = new System.Drawing.Size(104, 24);
            this.cmbPort.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(32, 34);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 1;
            this.label4.Text = "主通讯口:";
            // 
            // cmbPortBack
            // 
            this.cmbPortBack.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPortBack.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbPortBack.FormattingEnabled = true;
            this.cmbPortBack.Location = new System.Drawing.Point(94, 66);
            this.cmbPortBack.Name = "cmbPortBack";
            this.cmbPortBack.Size = new System.Drawing.Size(104, 24);
            this.cmbPortBack.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 111);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "巡查间隔:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(32, 72);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 12);
            this.label5.TabIndex = 1;
            this.label5.Text = "次通讯口:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.udLunxunTime);
            this.groupBox2.Controls.Add(this.udofflinetime);
            this.groupBox2.Controls.Add(this.udordertimespan);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.cmbRate2);
            this.groupBox2.Controls.Add(this.cmbRate1);
            this.groupBox2.Controls.Add(this.cmbPort);
            this.groupBox2.Controls.Add(this.cmbPortBack);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(3, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(380, 232);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            // 
            // udordertimespan
            // 
            this.udordertimespan.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.udordertimespan.Location = new System.Drawing.Point(94, 184);
            this.udordertimespan.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.udordertimespan.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.udordertimespan.Name = "udordertimespan";
            this.udordertimespan.Size = new System.Drawing.Size(104, 26);
            this.udordertimespan.TabIndex = 4;
            this.udordertimespan.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(203, 191);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 12);
            this.label7.TabIndex = 3;
            this.label7.Text = "毫秒";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(205, 111);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "秒";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(212, 70);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(48, 17);
            this.label9.TabIndex = 1;
            this.label9.Text = "波特率:";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(212, 32);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(48, 17);
            this.label8.TabIndex = 1;
            this.label8.Text = "波特率:";
            // 
            // cmbRate2
            // 
            this.cmbRate2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRate2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbRate2.FormattingEnabled = true;
            this.cmbRate2.Items.AddRange(new object[] {
            "1200",
            "2400",
            "4800",
            "9600",
            "14400",
            "19200",
            "38400",
            "56000",
            "",
            ""});
            this.cmbRate2.Location = new System.Drawing.Point(262, 66);
            this.cmbRate2.Name = "cmbRate2";
            this.cmbRate2.Size = new System.Drawing.Size(104, 24);
            this.cmbRate2.TabIndex = 0;
            // 
            // cmbRate1
            // 
            this.cmbRate1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRate1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbRate1.FormattingEnabled = true;
            this.cmbRate1.Items.AddRange(new object[] {
            "1200",
            "2400",
            "4800",
            "9600",
            "14400",
            "19200",
            "38400",
            "56000",
            "",
            ""});
            this.cmbRate1.Location = new System.Drawing.Point(262, 28);
            this.cmbRate1.Name = "cmbRate1";
            this.cmbRate1.Size = new System.Drawing.Size(104, 24);
            this.cmbRate1.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(32, 191);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 12);
            this.label6.TabIndex = 1;
            this.label6.Text = "命令间隔:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 151);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "离线检测周期:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(205, 151);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(17, 12);
            this.label10.TabIndex = 5;
            this.label10.Text = "秒";
            // 
            // udofflinetime
            // 
            this.udofflinetime.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.udofflinetime.Location = new System.Drawing.Point(94, 144);
            this.udofflinetime.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.udofflinetime.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.udofflinetime.Name = "udofflinetime";
            this.udofflinetime.Size = new System.Drawing.Size(104, 26);
            this.udofflinetime.TabIndex = 4;
            this.udofflinetime.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // udLunxunTime
            // 
            this.udLunxunTime.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.udLunxunTime.Location = new System.Drawing.Point(94, 104);
            this.udLunxunTime.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.udLunxunTime.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.udLunxunTime.Name = "udLunxunTime";
            this.udLunxunTime.Size = new System.Drawing.Size(104, 26);
            this.udLunxunTime.TabIndex = 4;
            this.udLunxunTime.Value = new decimal(new int[] {
            10,
            0,
            0,
            0}); 
            // 
            // frmPort
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(395, 303);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "frmPort";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "通讯设置";
            this.Load += new System.EventHandler(this.frmPort_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udordertimespan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udofflinetime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udLunxunTime)).EndInit();
            this.ResumeLayout(false);

        }
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbPortBack;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox cmbPort;
        
        void BtnLoopOkClick(object sender, System.EventArgs e)
        {

        }
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cmbRate2;
        private System.Windows.Forms.ComboBox cmbRate1;
        private System.Windows.Forms.NumericUpDown udordertimespan;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown udofflinetime;
        private System.Windows.Forms.NumericUpDown udLunxunTime;
    }
}