namespace GSM
{
    partial class frmLoginIn
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLoginIn));
            this.a1Panel1 = new Owf.Controls.A1Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbUserCode = new System.Windows.Forms.ComboBox();
            this.txtPwd = new System.Windows.Forms.TextBox();
            this.gbtnOk = new Glass.GlassButton();
            this.gbtnCancel = new Glass.GlassButton();
            this.SuspendLayout();
            // 
            // a1Panel1
            // 
            this.a1Panel1.BackColor = System.Drawing.Color.AliceBlue;
            this.a1Panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.a1Panel1.BorderColor = System.Drawing.Color.DeepSkyBlue;
            this.a1Panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.a1Panel1.GradientEndColor = System.Drawing.Color.SkyBlue;
            this.a1Panel1.GradientStartColor = System.Drawing.Color.SkyBlue;
            this.a1Panel1.Image = global::GSM.Properties.Resources.logoffpng;
            this.a1Panel1.ImageLocation = new System.Drawing.Point(20, 38);
            this.a1Panel1.Location = new System.Drawing.Point(10, 10);
            this.a1Panel1.Name = "a1Panel1";
            this.a1Panel1.RoundCornerRadius = 10;
            this.a1Panel1.ShadowOffSet = 10;
            this.a1Panel1.Size = new System.Drawing.Size(390, 202);
            this.a1Panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.SkyBlue;
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Location = new System.Drawing.Point(96, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "用户名称:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.SkyBlue;
            this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label2.Location = new System.Drawing.Point(96, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "用户密码:";
            // 
            // cmbUserCode
            // 
            this.cmbUserCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUserCode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbUserCode.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbUserCode.FormattingEnabled = true;
            this.cmbUserCode.Location = new System.Drawing.Point(156, 50);
            this.cmbUserCode.Name = "cmbUserCode";
            this.cmbUserCode.Size = new System.Drawing.Size(167, 24);
            this.cmbUserCode.TabIndex = 2;
            this.cmbUserCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmbUserCode_KeyPress);
            // 
            // txtPwd
            // 
            this.txtPwd.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPwd.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtPwd.Location = new System.Drawing.Point(156, 93);
            this.txtPwd.Name = "txtPwd";
            this.txtPwd.PasswordChar = '*';
            this.txtPwd.Size = new System.Drawing.Size(167, 22);
            this.txtPwd.TabIndex = 3;
            this.txtPwd.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmbUserCode_KeyPress);
            // 
            // gbtnOk
            // 
            this.gbtnOk.Location = new System.Drawing.Point(156, 137);
            this.gbtnOk.Name = "gbtnOk";
            this.gbtnOk.Size = new System.Drawing.Size(75, 23);
            this.gbtnOk.TabIndex = 4;
            this.gbtnOk.Text = "确定";
            this.gbtnOk.Click += new System.EventHandler(this.gbtnOk_Click);
            // 
            // gbtnCancel
            // 
            this.gbtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.gbtnCancel.Location = new System.Drawing.Point(248, 137);
            this.gbtnCancel.Name = "gbtnCancel";
            this.gbtnCancel.Size = new System.Drawing.Size(75, 23);
            this.gbtnCancel.TabIndex = 5;
            this.gbtnCancel.Text = "取消";
            this.gbtnCancel.Click += new System.EventHandler(this.gbtnCancel_Click);
            // 
            // frmLoginIn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.CancelButton = this.gbtnCancel;
            this.ClientSize = new System.Drawing.Size(400, 212);
            this.ControlBox = false;
            this.Controls.Add(this.gbtnCancel);
            this.Controls.Add(this.gbtnOk);
            this.Controls.Add(this.txtPwd);
            this.Controls.Add(this.cmbUserCode);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.a1Panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmLoginIn";
            this.Padding = new System.Windows.Forms.Padding(10, 10, 0, 0);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Go-DEX 用户登录";
            this.Load += new System.EventHandler(this.frmLoginIn_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmLoginIn_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Owf.Controls.A1Panel a1Panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbUserCode;
        private System.Windows.Forms.TextBox txtPwd;
        private Glass.GlassButton gbtnOk;
        private Glass.GlassButton gbtnCancel;
    }
}