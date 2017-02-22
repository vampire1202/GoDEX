namespace GSM.Forms
{
    partial class frmSetUserPwd
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
            this.a1Panel1 = new Owf.Controls.A1Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.gbtnOk = new Glass.GlassButton();
            this.gbtnEdit = new Glass.GlassButton();
            this.listView1 = new System.Windows.Forms.ListView();
            this.gbtnDel = new Glass.GlassButton();
            this.cmbUserRole = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtUserCode = new System.Windows.Forms.TextBox();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.txtUserTel = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtConfirmPwd = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // a1Panel1
            // 
            this.a1Panel1.BorderColor = System.Drawing.Color.Gray;
            this.a1Panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.a1Panel1.GradientEndColor = System.Drawing.Color.SkyBlue;
            this.a1Panel1.GradientStartColor = System.Drawing.Color.SkyBlue;
            this.a1Panel1.Image = null;
            this.a1Panel1.ImageLocation = new System.Drawing.Point(4, 4);
            this.a1Panel1.Location = new System.Drawing.Point(5, 5);
            this.a1Panel1.Name = "a1Panel1";
            this.a1Panel1.RoundCornerRadius = 10;
            this.a1Panel1.Size = new System.Drawing.Size(770, 249);
            this.a1Panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.SkyBlue;
            this.label1.ForeColor = System.Drawing.Color.MidnightBlue;
            this.label1.Location = new System.Drawing.Point(6, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "身份类型:";
            // 
            // gbtnOk
            // 
            this.gbtnOk.Location = new System.Drawing.Point(16, 217);
            this.gbtnOk.Name = "gbtnOk";
            this.gbtnOk.ShineColor = System.Drawing.Color.DeepSkyBlue;
            this.gbtnOk.Size = new System.Drawing.Size(75, 23);
            this.gbtnOk.TabIndex = 0;
            this.gbtnOk.Text = "添加";
            this.gbtnOk.Visible = false;
            this.gbtnOk.Click += new System.EventHandler(this.gbtnOk_Click);
            // 
            // gbtnEdit
            // 
            this.gbtnEdit.Location = new System.Drawing.Point(177, 217);
            this.gbtnEdit.Name = "gbtnEdit";
            this.gbtnEdit.ShineColor = System.Drawing.Color.DeepSkyBlue;
            this.gbtnEdit.Size = new System.Drawing.Size(75, 23);
            this.gbtnEdit.TabIndex = 1;
            this.gbtnEdit.Text = "修改信息";
            this.gbtnEdit.Click += new System.EventHandler(this.gbtnEdit_Click);
            // 
            // listView1
            // 
            this.listView1.AllowColumnReorder = true;
            this.listView1.CheckBoxes = true;
            this.listView1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listView1.FullRowSelect = true;
            this.listView1.Location = new System.Drawing.Point(264, 13);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(498, 227);
            this.listView1.TabIndex = 5;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listView1_ItemSelectionChanged);
            // 
            // gbtnDel
            // 
            this.gbtnDel.Location = new System.Drawing.Point(97, 217);
            this.gbtnDel.Name = "gbtnDel";
            this.gbtnDel.ShineColor = System.Drawing.Color.DeepSkyBlue;
            this.gbtnDel.Size = new System.Drawing.Size(75, 23);
            this.gbtnDel.TabIndex = 2;
            this.gbtnDel.Text = "删除";
            this.gbtnDel.Visible = false;
            this.gbtnDel.Click += new System.EventHandler(this.gbtnDel_Click);
            // 
            // cmbUserRole
            // 
            this.cmbUserRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUserRole.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cmbUserRole.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbUserRole.FormattingEnabled = true;
            this.cmbUserRole.Location = new System.Drawing.Point(67, 13);
            this.cmbUserRole.Name = "cmbUserRole";
            this.cmbUserRole.Size = new System.Drawing.Size(169, 24);
            this.cmbUserRole.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.SkyBlue;
            this.label2.ForeColor = System.Drawing.Color.MidnightBlue;
            this.label2.Location = new System.Drawing.Point(6, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "用户代码:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.SkyBlue;
            this.label3.ForeColor = System.Drawing.Color.MidnightBlue;
            this.label3.Location = new System.Drawing.Point(6, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "用户名称:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.SkyBlue;
            this.label4.ForeColor = System.Drawing.Color.MidnightBlue;
            this.label4.Location = new System.Drawing.Point(6, 109);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 1;
            this.label4.Text = "联系方式:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.SkyBlue;
            this.label5.ForeColor = System.Drawing.Color.MidnightBlue;
            this.label5.Location = new System.Drawing.Point(26, 138);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 12);
            this.label5.TabIndex = 1;
            this.label5.Text = "密码:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.SkyBlue;
            this.label6.ForeColor = System.Drawing.Color.MidnightBlue;
            this.label6.Location = new System.Drawing.Point(6, 168);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 12);
            this.label6.TabIndex = 1;
            this.label6.Text = "确认密码:";
            // 
            // txtUserCode
            // 
            this.txtUserCode.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtUserCode.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtUserCode.Location = new System.Drawing.Point(67, 42);
            this.txtUserCode.Name = "txtUserCode";
            this.txtUserCode.ReadOnly = true;
            this.txtUserCode.Size = new System.Drawing.Size(169, 22);
            this.txtUserCode.TabIndex = 1;
            // 
            // txtUserName
            // 
            this.txtUserName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtUserName.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtUserName.Location = new System.Drawing.Point(67, 72);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.ReadOnly = true;
            this.txtUserName.Size = new System.Drawing.Size(169, 22);
            this.txtUserName.TabIndex = 2;
            // 
            // txtUserTel
            // 
            this.txtUserTel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtUserTel.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtUserTel.Location = new System.Drawing.Point(67, 102);
            this.txtUserTel.Name = "txtUserTel";
            this.txtUserTel.Size = new System.Drawing.Size(169, 22);
            this.txtUserTel.TabIndex = 3;
            // 
            // txtPassword
            // 
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPassword.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtPassword.Location = new System.Drawing.Point(67, 132);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(169, 22);
            this.txtPassword.TabIndex = 4;
            // 
            // txtConfirmPwd
            // 
            this.txtConfirmPwd.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtConfirmPwd.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtConfirmPwd.Location = new System.Drawing.Point(67, 162);
            this.txtConfirmPwd.Name = "txtConfirmPwd";
            this.txtConfirmPwd.PasswordChar = '*';
            this.txtConfirmPwd.Size = new System.Drawing.Size(169, 22);
            this.txtConfirmPwd.TabIndex = 5;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.SkyBlue;
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtConfirmPwd);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtPassword);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtUserTel);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtUserName);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtUserCode);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.cmbUserRole);
            this.groupBox1.Location = new System.Drawing.Point(16, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(242, 198);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            // 
            // frmSetUserPwd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(775, 254);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.gbtnDel);
            this.Controls.Add(this.gbtnEdit);
            this.Controls.Add(this.gbtnOk);
            this.Controls.Add(this.a1Panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "frmSetUserPwd";
            this.Padding = new System.Windows.Forms.Padding(5, 5, 0, 0);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "用户管理";
            this.Load += new System.EventHandler(this.frmSetUserPwd_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Owf.Controls.A1Panel a1Panel1;
        private System.Windows.Forms.Label label1;
        private Glass.GlassButton gbtnOk;
        private Glass.GlassButton gbtnEdit;
        private System.Windows.Forms.ListView listView1;
        private Glass.GlassButton gbtnDel;
        private System.Windows.Forms.ComboBox cmbUserRole;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtUserCode;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.TextBox txtUserTel;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtConfirmPwd;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}