namespace ShapefileEditor.Forms
{
    partial class RotateShapeForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RotateShapeForm));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtAngle = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label4 = new System.Windows.Forms.Label();
            this.txtY = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtX = new System.Windows.Forms.TextBox();
            this.rdOther = new System.Windows.Forms.RadioButton();
            this.rdCentroid = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.button3 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.AccessibleDescription = null;
            this.button1.AccessibleName = null;
            resources.ApplyResources(this.button1, "button1");
            this.button1.BackgroundImage = null;
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Font = null;
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.AccessibleDescription = null;
            this.button2.AccessibleName = null;
            resources.ApplyResources(this.button2, "button2");
            this.button2.BackgroundImage = null;
            this.button2.Font = null;
            this.button2.Name = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AccessibleDescription = null;
            this.label1.AccessibleName = null;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Font = null;
            this.label1.Name = "label1";
            // 
            // label2
            // 
            this.label2.AccessibleDescription = null;
            this.label2.AccessibleName = null;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Font = null;
            this.label2.Name = "label2";
            // 
            // txtAngle
            // 
            this.txtAngle.AccessibleDescription = null;
            this.txtAngle.AccessibleName = null;
            resources.ApplyResources(this.txtAngle, "txtAngle");
            this.txtAngle.BackgroundImage = null;
            this.txtAngle.Font = null;
            this.txtAngle.Name = "txtAngle";
            this.txtAngle.TextChanged += new System.EventHandler(this.txts_TextChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.AccessibleDescription = null;
            this.groupBox1.AccessibleName = null;
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.BackgroundImage = null;
            this.groupBox1.Controls.Add(this.linkLabel1);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtY);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtX);
            this.groupBox1.Controls.Add(this.rdOther);
            this.groupBox1.Controls.Add(this.rdCentroid);
            this.groupBox1.Font = null;
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AccessibleDescription = null;
            this.linkLabel1.AccessibleName = null;
            resources.ApplyResources(this.linkLabel1, "linkLabel1");
            this.linkLabel1.Font = null;
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.TabStop = true;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // label4
            // 
            this.label4.AccessibleDescription = null;
            this.label4.AccessibleName = null;
            resources.ApplyResources(this.label4, "label4");
            this.label4.Font = null;
            this.label4.Name = "label4";
            // 
            // txtY
            // 
            this.txtY.AccessibleDescription = null;
            this.txtY.AccessibleName = null;
            resources.ApplyResources(this.txtY, "txtY");
            this.txtY.BackgroundImage = null;
            this.txtY.Font = null;
            this.txtY.Name = "txtY";
            this.txtY.TextChanged += new System.EventHandler(this.txts_TextChanged);
            // 
            // label3
            // 
            this.label3.AccessibleDescription = null;
            this.label3.AccessibleName = null;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Font = null;
            this.label3.Name = "label3";
            // 
            // txtX
            // 
            this.txtX.AccessibleDescription = null;
            this.txtX.AccessibleName = null;
            resources.ApplyResources(this.txtX, "txtX");
            this.txtX.BackgroundImage = null;
            this.txtX.Font = null;
            this.txtX.Name = "txtX";
            this.txtX.TextChanged += new System.EventHandler(this.txts_TextChanged);
            // 
            // rdOther
            // 
            this.rdOther.AccessibleDescription = null;
            this.rdOther.AccessibleName = null;
            resources.ApplyResources(this.rdOther, "rdOther");
            this.rdOther.BackgroundImage = null;
            this.rdOther.Font = null;
            this.rdOther.Name = "rdOther";
            this.rdOther.TabStop = true;
            this.rdOther.UseVisualStyleBackColor = true;
            this.rdOther.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // rdCentroid
            // 
            this.rdCentroid.AccessibleDescription = null;
            this.rdCentroid.AccessibleName = null;
            resources.ApplyResources(this.rdCentroid, "rdCentroid");
            this.rdCentroid.BackgroundImage = null;
            this.rdCentroid.Font = null;
            this.rdCentroid.Name = "rdCentroid";
            this.rdCentroid.TabStop = true;
            this.rdCentroid.UseVisualStyleBackColor = true;
            this.rdCentroid.CheckedChanged += new System.EventHandler(this.rdCentroid_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AccessibleDescription = null;
            this.label5.AccessibleName = null;
            resources.ApplyResources(this.label5, "label5");
            this.label5.Font = null;
            this.label5.Name = "label5";
            // 
            // trackBar1
            // 
            this.trackBar1.AccessibleDescription = null;
            this.trackBar1.AccessibleName = null;
            resources.ApplyResources(this.trackBar1, "trackBar1");
            this.trackBar1.BackgroundImage = null;
            this.trackBar1.Font = null;
            this.trackBar1.Maximum = 180;
            this.trackBar1.Minimum = -180;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.TickFrequency = 60;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // button3
            // 
            this.button3.AccessibleDescription = null;
            this.button3.AccessibleName = null;
            resources.ApplyResources(this.button3, "button3");
            this.button3.BackgroundImage = null;
            this.button3.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button3.Font = null;
            this.button3.Name = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // RotateShapeForm
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtAngle);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Font = null;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RotateShapeForm";
            this.Load += new System.EventHandler(this.RotateShapeForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RotateShapeForm_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.LinkLabel linkLabel1;
        public System.Windows.Forms.TextBox txtAngle;
        public System.Windows.Forms.TextBox txtY;
        public System.Windows.Forms.TextBox txtX;
        public System.Windows.Forms.RadioButton rdOther;
        public System.Windows.Forms.RadioButton rdCentroid;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Button button3;
    }
}