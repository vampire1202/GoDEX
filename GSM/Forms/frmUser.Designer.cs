namespace GSM.Forms
{
    partial class frmUser
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
            this.cmbUserRole = new System.Windows.Forms.ComboBox();
            this.txtUserPwd = new System.Windows.Forms.TextBox();
            this.a1Panel1 = new Owf.Controls.A1Panel();
            this.gbtnOk = new Glass.GlassButton();
            this.gbtnCancel = new Glass.GlassButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cmbUserRole
            // 
            this.cmbUserRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUserRole.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbUserRole.FormattingEnabled = true;
            this.cmbUserRole.Location = new System.Drawing.Point(92, 26);
            this.cmbUserRole.Name = "cmbUserRole";
            this.cmbUserRole.Size = new System.Drawing.Size(179, 27);
            this.cmbUserRole.TabIndex = 1;
            this.cmbUserRole.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtUserPwd_KeyPress);
            // 
            // txtUserPwd
            // 
            this.txtUserPwd.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtUserPwd.Location = new System.Drawing.Point(92, 56);
            this.txtUserPwd.Name = "txtUserPwd";
            this.txtUserPwd.PasswordChar = '*';
            this.txtUserPwd.Size = new System.Drawing.Size(179, 26);
            this.txtUserPwd.TabIndex = 2;
            this.txtUserPwd.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtUserPwd_KeyPress);
            // 
            // a1Panel1
            // 
            this.a1Panel1.BackColor = System.Drawing.Color.AliceBlue;
            this.a1Panel1.BorderColor = System.Drawing.Color.SlateGray;
            this.a1Panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.a1Panel1.GradientEndColor = System.Drawing.Color.SkyBlue;
            this.a1Panel1.GradientStartColor = System.Drawing.Color.SkyBlue;
            this.a1Panel1.Image = null;
            this.a1Panel1.ImageLocation = new System.Drawing.Point(4, 4);
            this.a1Panel1.Location = new System.Drawing.Point(4, 4);
            this.a1Panel1.Name = "a1Panel1";
            this.a1Panel1.RoundCornerRadius = 10;
            this.a1Panel1.Size = new System.Drawing.Size(298, 142);
            this.a1Panel1.TabIndex = 4;
            // 
            // gbtnOk
            // 
            this.gbtnOk.Location = new System.Drawing.Point(92, 97);
            this.gbtnOk.Name = "gbtnOk";
            this.gbtnOk.Size = new System.Drawing.Size(75, 23);
            this.gbtnOk.TabIndex = 5;
            this.gbtnOk.Text = "确定";
            this.gbtnOk.Click += new System.EventHandler(this.gbtnOk_Click);
            // 
            // gbtnCancel
            // 
            this.gbtnCancel.Location = new System.Drawing.Point(196, 97);
            this.gbtnCancel.Name = "gbtnCancel";
            this.gbtnCancel.Size = new System.Drawing.Size(75, 23);
            this.gbtnCancel.TabIndex = 5;
            this.gbtnCancel.Text = "取消";
            this.gbtnCancel.Click += new System.EventHandler(this.gbtnCancel_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.SkyBlue;
            this.label2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(16, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 14);
            this.label2.TabIndex = 6;
            this.label2.Text = "用户密码:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.SkyBlue;
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(16, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 14);
            this.label1.TabIndex = 6;
            this.label1.Text = "用户身份:";
            // 
            // frmUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(302, 146);
            this.ControlBox = false;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.gbtnCancel);
            this.Controls.Add(this.gbtnOk);
            this.Controls.Add(this.txtUserPwd);
            this.Controls.Add(this.cmbUserRole);
            this.Controls.Add(this.a1Panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmUser";
            this.Padding = new System.Windows.Forms.Padding(4, 4, 0, 0);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "身份确认";
            this.Load += new System.EventHandler(this.frmUser_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbUserRole;
        private System.Windows.Forms.TextBox txtUserPwd;
        private Owf.Controls.A1Panel a1Panel1;
        private Glass.GlassButton gbtnOk;
        private Glass.GlassButton gbtnCancel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}