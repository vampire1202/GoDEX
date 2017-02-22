namespace GSM.Forms
{
    partial class frmUserLog
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.chk0 = new System.Windows.Forms.CheckBox();
            this.chk1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.chk2 = new System.Windows.Forms.CheckBox();
            this.chk3 = new System.Windows.Forms.CheckBox();
            this.chk4 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.chk5 = new System.Windows.Forms.CheckBox();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.chk7 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.chk8 = new System.Windows.Forms.CheckBox();
            this.chk9 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnSetPrinter = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnUnselect = new System.Windows.Forms.Button();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.dataGridView_Log = new System.Windows.Forms.DataGridView();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Log)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel2);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox1.Location = new System.Drawing.Point(711, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(194, 553);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "筛选";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.flowLayoutPanel2);
            this.panel2.Controls.Add(this.flowLayoutPanel1);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 101);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(188, 449);
            this.panel2.TabIndex = 7;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.AutoScroll = true;
            this.flowLayoutPanel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 317);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(188, 83);
            this.flowLayoutPanel2.TabIndex = 6;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.flowLayoutPanel1.Controls.Add(this.chk0);
            this.flowLayoutPanel1.Controls.Add(this.chk1);
            this.flowLayoutPanel1.Controls.Add(this.checkBox2);
            this.flowLayoutPanel1.Controls.Add(this.chk2);
            this.flowLayoutPanel1.Controls.Add(this.chk3);
            this.flowLayoutPanel1.Controls.Add(this.chk4);
            this.flowLayoutPanel1.Controls.Add(this.checkBox4);
            this.flowLayoutPanel1.Controls.Add(this.chk5);
            this.flowLayoutPanel1.Controls.Add(this.checkBox5);
            this.flowLayoutPanel1.Controls.Add(this.chk7);
            this.flowLayoutPanel1.Controls.Add(this.checkBox3);
            this.flowLayoutPanel1.Controls.Add(this.chk8);
            this.flowLayoutPanel1.Controls.Add(this.chk9);
            this.flowLayoutPanel1.Controls.Add(this.checkBox1);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(188, 317);
            this.flowLayoutPanel1.TabIndex = 5;
            // 
            // chk0
            // 
            this.chk0.Location = new System.Drawing.Point(3, 3);
            this.chk0.Name = "chk0";
            this.chk0.Size = new System.Drawing.Size(76, 24);
            this.chk0.TabIndex = 0;
            this.chk0.Text = "火警警报";
            this.chk0.UseVisualStyleBackColor = true;
            this.chk0.CheckedChanged += new System.EventHandler(this.chk0_Click);
            // 
            // chk1
            // 
            this.chk1.Location = new System.Drawing.Point(85, 3);
            this.chk1.Name = "chk1";
            this.chk1.Size = new System.Drawing.Size(76, 24);
            this.chk1.TabIndex = 0;
            this.chk1.Text = "气流高";
            this.chk1.UseVisualStyleBackColor = true;
            this.chk1.Click += new System.EventHandler(this.chk0_Click);
            // 
            // checkBox2
            // 
            this.checkBox2.Location = new System.Drawing.Point(3, 33);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(76, 24);
            this.checkBox2.TabIndex = 1;
            this.checkBox2.Text = "气流低";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.Click += new System.EventHandler(this.chk0_Click);
            // 
            // chk2
            // 
            this.chk2.Location = new System.Drawing.Point(85, 33);
            this.chk2.Name = "chk2";
            this.chk2.Size = new System.Drawing.Size(69, 24);
            this.chk2.TabIndex = 0;
            this.chk2.Text = "故障";
            this.chk2.UseVisualStyleBackColor = true;
            this.chk2.Click += new System.EventHandler(this.chk0_Click);
            // 
            // chk3
            // 
            this.chk3.Location = new System.Drawing.Point(3, 63);
            this.chk3.Name = "chk3";
            this.chk3.Size = new System.Drawing.Size(76, 24);
            this.chk3.TabIndex = 0;
            this.chk3.Text = "复位";
            this.chk3.UseVisualStyleBackColor = true;
            this.chk3.Click += new System.EventHandler(this.chk0_Click);
            // 
            // chk4
            // 
            this.chk4.Location = new System.Drawing.Point(85, 63);
            this.chk4.Name = "chk4";
            this.chk4.Size = new System.Drawing.Size(76, 24);
            this.chk4.TabIndex = 0;
            this.chk4.Text = "登记";
            this.chk4.UseVisualStyleBackColor = true;
            this.chk4.Click += new System.EventHandler(this.chk0_Click);
            // 
            // checkBox4
            // 
            this.checkBox4.Location = new System.Drawing.Point(3, 93);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(76, 24);
            this.checkBox4.TabIndex = 0;
            this.checkBox4.Text = "学习";
            this.checkBox4.UseVisualStyleBackColor = true;
            this.checkBox4.Click += new System.EventHandler(this.chk0_Click);
            // 
            // chk5
            // 
            this.chk5.Location = new System.Drawing.Point(85, 93);
            this.chk5.Name = "chk5";
            this.chk5.Size = new System.Drawing.Size(76, 24);
            this.chk5.TabIndex = 0;
            this.chk5.Text = "历史警报";
            this.chk5.UseVisualStyleBackColor = true;
            this.chk5.Click += new System.EventHandler(this.chk0_Click);
            // 
            // checkBox5
            // 
            this.checkBox5.Location = new System.Drawing.Point(3, 123);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size(76, 24);
            this.checkBox5.TabIndex = 0;
            this.checkBox5.Text = "确认";
            this.checkBox5.UseVisualStyleBackColor = true;
            this.checkBox5.Click += new System.EventHandler(this.chk0_Click);
            // 
            // chk7
            // 
            this.chk7.Location = new System.Drawing.Point(3, 153);
            this.chk7.Name = "chk7";
            this.chk7.Size = new System.Drawing.Size(116, 24);
            this.chk7.TabIndex = 0;
            this.chk7.Text = "气流阈值设置";
            this.chk7.UseVisualStyleBackColor = true;
            this.chk7.Click += new System.EventHandler(this.chk0_Click);
            // 
            // checkBox3
            // 
            this.checkBox3.Location = new System.Drawing.Point(3, 183);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(116, 24);
            this.checkBox3.TabIndex = 0;
            this.checkBox3.Text = "火警阈值设置";
            this.checkBox3.UseVisualStyleBackColor = true;
            this.checkBox3.Click += new System.EventHandler(this.chk0_Click);
            // 
            // chk8
            // 
            this.chk8.Location = new System.Drawing.Point(3, 213);
            this.chk8.Name = "chk8";
            this.chk8.Size = new System.Drawing.Size(116, 24);
            this.chk8.TabIndex = 0;
            this.chk8.Text = "火警延迟设置";
            this.chk8.UseVisualStyleBackColor = true;
            this.chk8.Click += new System.EventHandler(this.chk0_Click);
            // 
            // chk9
            // 
            this.chk9.Location = new System.Drawing.Point(3, 243);
            this.chk9.Name = "chk9";
            this.chk9.Size = new System.Drawing.Size(116, 24);
            this.chk9.TabIndex = 0;
            this.chk9.Text = "设置设备时间";
            this.chk9.UseVisualStyleBackColor = true;
            this.chk9.Click += new System.EventHandler(this.chk0_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.Location = new System.Drawing.Point(3, 273);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(116, 24);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "抽气泵转速设置";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.Click += new System.EventHandler(this.chk0_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnPrint);
            this.panel3.Controls.Add(this.btnSetPrinter);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 400);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(188, 49);
            this.panel3.TabIndex = 6;
            // 
            // btnPrint
            // 
            this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnPrint.Location = new System.Drawing.Point(103, 11);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(82, 28);
            this.btnPrint.TabIndex = 1;
            this.btnPrint.Text = "打印报告";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnSetPrinter
            // 
            this.btnSetPrinter.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSetPrinter.Location = new System.Drawing.Point(3, 11);
            this.btnSetPrinter.Name = "btnSetPrinter";
            this.btnSetPrinter.Size = new System.Drawing.Size(82, 28);
            this.btnSetPrinter.TabIndex = 1;
            this.btnSetPrinter.Text = "打印机设置";
            this.btnSetPrinter.UseVisualStyleBackColor = true;
            this.btnSetPrinter.Click += new System.EventHandler(this.btnSetPrinter_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnUnselect);
            this.panel1.Controls.Add(this.btnSelectAll);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.dateTimePicker2);
            this.panel1.Controls.Add(this.dateTimePicker1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 17);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(188, 84);
            this.panel1.TabIndex = 6;
            // 
            // btnUnselect
            // 
            this.btnUnselect.Location = new System.Drawing.Point(64, 58);
            this.btnUnselect.Name = "btnUnselect";
            this.btnUnselect.Size = new System.Drawing.Size(55, 23);
            this.btnUnselect.TabIndex = 3;
            this.btnUnselect.Text = "反选";
            this.btnUnselect.UseVisualStyleBackColor = true;
            this.btnUnselect.Click += new System.EventHandler(this.btnUnselect_Click);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Location = new System.Drawing.Point(3, 58);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(55, 23);
            this.btnSelectAll.TabIndex = 3;
            this.btnSelectAll.Text = "全选";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "从";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "至";
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker2.Location = new System.Drawing.Point(26, 33);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(152, 21);
            this.dateTimePicker2.TabIndex = 1;
            this.dateTimePicker2.ValueChanged += new System.EventHandler(this.dateTimePicker2_ValueChanged);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(26, 5);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(152, 21);
            this.dateTimePicker1.TabIndex = 1;
            this.dateTimePicker1.Value = new System.DateTime(2011, 12, 20, 9, 54, 43, 0);
            this.dateTimePicker1.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            // 
            // dataGridView_Log
            // 
            this.dataGridView_Log.AllowUserToAddRows = false;
            this.dataGridView_Log.AllowUserToDeleteRows = false;
            this.dataGridView_Log.AllowUserToOrderColumns = true;
            this.dataGridView_Log.AllowUserToResizeRows = false;
            this.dataGridView_Log.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView_Log.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_Log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_Log.Location = new System.Drawing.Point(0, 0);
            this.dataGridView_Log.Name = "dataGridView_Log";
            this.dataGridView_Log.ReadOnly = true;
            this.dataGridView_Log.RowHeadersWidth = 20;
            this.dataGridView_Log.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView_Log.RowTemplate.Height = 23;
            this.dataGridView_Log.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_Log.Size = new System.Drawing.Size(711, 553);
            this.dataGridView_Log.TabIndex = 2;
            // 
            // frmUserLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(905, 553);
            this.Controls.Add(this.dataGridView_Log);
            this.Controls.Add(this.groupBox1);
            this.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.Document;
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "frmUserLog";
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.Document;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "系统日志";
            this.Load += new System.EventHandler(this.FrmUserLogLoad);
            this.groupBox1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Log)).EndInit();
            this.ResumeLayout(false);

        }
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button btnSetPrinter;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.CheckBox chk1;
        private System.Windows.Forms.CheckBox chk2;
        private System.Windows.Forms.CheckBox chk3;
        private System.Windows.Forms.CheckBox chk9;
        private System.Windows.Forms.CheckBox chk4;
        private System.Windows.Forms.CheckBox chk5;
        private System.Windows.Forms.CheckBox chk7;
        private System.Windows.Forms.GroupBox groupBox1;

        #endregion

        private System.Windows.Forms.CheckBox chk8;
        private System.Windows.Forms.CheckBox chk0;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.DataGridView dataGridView_Log;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Button btnUnselect;
        private System.Windows.Forms.CheckBox checkBox5;
    }
}