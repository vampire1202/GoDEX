namespace GSM
{
    partial class frmReg
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
            this.txtKey = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.mathineCode = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.getMachineCode = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtKey
            // 
            this.txtKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtKey.Location = new System.Drawing.Point(3, 17);
            this.txtKey.Multiline = true;
            this.txtKey.Name = "txtKey";
            this.txtKey.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtKey.Size = new System.Drawing.Size(606, 149);
            this.txtKey.TabIndex = 4;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(441, 284);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(82, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "注册";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // mathineCode
            // 
            this.mathineCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mathineCode.Location = new System.Drawing.Point(3, 17);
            this.mathineCode.Multiline = true;
            this.mathineCode.Name = "mathineCode";
            this.mathineCode.ReadOnly = true;
            this.mathineCode.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.mathineCode.Size = new System.Drawing.Size(606, 56);
            this.mathineCode.TabIndex = 9;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtKey);
            this.groupBox1.Location = new System.Drawing.Point(2, 109);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(612, 169);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "请输入注册码";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.mathineCode);
            this.groupBox2.Location = new System.Drawing.Point(2, 27);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(612, 76);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "你的机器码";
            // 
            // getMachineCode
            // 
            this.getMachineCode.Location = new System.Drawing.Point(1, 2);
            this.getMachineCode.Name = "getMachineCode";
            this.getMachineCode.Size = new System.Drawing.Size(108, 23);
            this.getMachineCode.TabIndex = 12;
            this.getMachineCode.Text = "点击获取机器码";
            this.getMachineCode.UseVisualStyleBackColor = true;
            this.getMachineCode.Click += new System.EventHandler(this.getMachineCode_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(529, 284);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(82, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "退出";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // frmReg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(627, 316);
            this.ControlBox = false;
            this.Controls.Add(this.getMachineCode);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button2);
            this.MaximizeBox = false;
            this.Name = "frmReg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "注册";
            this.Load += new System.EventHandler(this.frmReg_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmReg_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtKey;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox mathineCode;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button getMachineCode;
        private System.Windows.Forms.Button button1;
    }
}