namespace ShapefileEditor.Forms
{
    partial class ResizeShapeForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ResizeShapeForm));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPercent = new System.Windows.Forms.TextBox();
            this.rdShrink = new System.Windows.Forms.RadioButton();
            this.rdGrow = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdPercent = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rdDistance = new System.Windows.Forms.RadioButton();
            this.txtDist = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
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
            // txtPercent
            // 
            this.txtPercent.AccessibleDescription = null;
            this.txtPercent.AccessibleName = null;
            resources.ApplyResources(this.txtPercent, "txtPercent");
            this.txtPercent.BackgroundImage = null;
            this.txtPercent.Font = null;
            this.txtPercent.Name = "txtPercent";
            this.txtPercent.TextChanged += new System.EventHandler(this.txts_TextChanged);
            // 
            // rdShrink
            // 
            this.rdShrink.AccessibleDescription = null;
            this.rdShrink.AccessibleName = null;
            resources.ApplyResources(this.rdShrink, "rdShrink");
            this.rdShrink.BackgroundImage = null;
            this.rdShrink.Font = null;
            this.rdShrink.Name = "rdShrink";
            this.rdShrink.TabStop = true;
            this.rdShrink.UseVisualStyleBackColor = true;
            // 
            // rdGrow
            // 
            this.rdGrow.AccessibleDescription = null;
            this.rdGrow.AccessibleName = null;
            resources.ApplyResources(this.rdGrow, "rdGrow");
            this.rdGrow.BackgroundImage = null;
            this.rdGrow.Font = null;
            this.rdGrow.Name = "rdGrow";
            this.rdGrow.TabStop = true;
            this.rdGrow.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AccessibleDescription = null;
            this.label3.AccessibleName = null;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Font = null;
            this.label3.Name = "label3";
            // 
            // trackBar1
            // 
            this.trackBar1.AccessibleDescription = null;
            this.trackBar1.AccessibleName = null;
            resources.ApplyResources(this.trackBar1, "trackBar1");
            this.trackBar1.BackgroundImage = null;
            this.trackBar1.Font = null;
            this.trackBar1.Maximum = 200;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.TickFrequency = 25;
            this.trackBar1.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.trackBar1.Value = 50;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // label4
            // 
            this.label4.AccessibleDescription = null;
            this.label4.AccessibleName = null;
            resources.ApplyResources(this.label4, "label4");
            this.label4.Font = null;
            this.label4.Name = "label4";
            // 
            // groupBox1
            // 
            this.groupBox1.AccessibleDescription = null;
            this.groupBox1.AccessibleName = null;
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.BackgroundImage = null;
            this.groupBox1.Controls.Add(this.txtPercent);
            this.groupBox1.Controls.Add(this.rdPercent);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.trackBar1);
            this.groupBox1.Font = null;
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // rdPercent
            // 
            this.rdPercent.AccessibleDescription = null;
            this.rdPercent.AccessibleName = null;
            resources.ApplyResources(this.rdPercent, "rdPercent");
            this.rdPercent.BackgroundImage = null;
            this.rdPercent.Checked = true;
            this.rdPercent.Font = null;
            this.rdPercent.Name = "rdPercent";
            this.rdPercent.TabStop = true;
            this.rdPercent.UseVisualStyleBackColor = true;
            this.rdPercent.CheckedChanged += new System.EventHandler(this.rdShape_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.AccessibleDescription = null;
            this.groupBox2.AccessibleName = null;
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.BackgroundImage = null;
            this.groupBox2.Controls.Add(this.rdDistance);
            this.groupBox2.Controls.Add(this.txtDist);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Font = null;
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // rdDistance
            // 
            this.rdDistance.AccessibleDescription = null;
            this.rdDistance.AccessibleName = null;
            resources.ApplyResources(this.rdDistance, "rdDistance");
            this.rdDistance.BackgroundImage = null;
            this.rdDistance.Font = null;
            this.rdDistance.Name = "rdDistance";
            this.rdDistance.UseVisualStyleBackColor = true;
            this.rdDistance.CheckedChanged += new System.EventHandler(this.rdShape_CheckedChanged);
            // 
            // txtDist
            // 
            this.txtDist.AccessibleDescription = null;
            this.txtDist.AccessibleName = null;
            resources.ApplyResources(this.txtDist, "txtDist");
            this.txtDist.BackgroundImage = null;
            this.txtDist.Font = null;
            this.txtDist.Name = "txtDist";
            // 
            // label5
            // 
            this.label5.AccessibleDescription = null;
            this.label5.AccessibleName = null;
            resources.ApplyResources(this.label5, "label5");
            this.label5.Font = null;
            this.label5.Name = "label5";
            // 
            // label6
            // 
            this.label6.AccessibleDescription = null;
            this.label6.AccessibleName = null;
            resources.ApplyResources(this.label6, "label6");
            this.label6.Font = null;
            this.label6.Name = "label6";
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
            // ResizeShapeForm
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.Controls.Add(this.button3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.rdGrow);
            this.Controls.Add(this.rdShrink);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Font = null;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ResizeShapeForm";
            this.Load += new System.EventHandler(this.RotateShapeForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.TextBox txtPercent;
        public System.Windows.Forms.RadioButton rdShrink;
        public System.Windows.Forms.RadioButton rdGrow;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        public System.Windows.Forms.TextBox txtDist;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.RadioButton rdPercent;
        private System.Windows.Forms.RadioButton rdDistance;
        private System.Windows.Forms.Button button3;
    }
}