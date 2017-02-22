namespace ShapefileEditor.Forms
{
    partial class InsertStockShapeForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InsertStockShapeForm));
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPolySideLength = new System.Windows.Forms.TextBox();
            this.lblUnit1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.grpPoly = new System.Windows.Forms.GroupBox();
            this.rdRegularPoly = new System.Windows.Forms.RadioButton();
            this.nudPolySides = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.grpCircle = new System.Windows.Forms.GroupBox();
            this.rdCircle = new System.Windows.Forms.RadioButton();
            this.txtCircleRadius = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.lblUnit4 = new System.Windows.Forms.Label();
            this.grpEllipse = new System.Windows.Forms.GroupBox();
            this.txtEllpsHeight = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.lblUnit = new System.Windows.Forms.Label();
            this.txtEllpsWidth = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.lblUnit5 = new System.Windows.Forms.Label();
            this.rdEllipse = new System.Windows.Forms.RadioButton();
            this.grpRectangle = new System.Windows.Forms.GroupBox();
            this.txtRectHeight = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.lblUnit3 = new System.Windows.Forms.Label();
            this.txtRectWidth = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.lblUnit2 = new System.Windows.Forms.Label();
            this.rdRectangle = new System.Windows.Forms.RadioButton();
            this.grpPoly.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPolySides)).BeginInit();
            this.grpCircle.SuspendLayout();
            this.grpEllipse.SuspendLayout();
            this.grpRectangle.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // txtPolySideLength
            // 
            resources.ApplyResources(this.txtPolySideLength, "txtPolySideLength");
            this.txtPolySideLength.Name = "txtPolySideLength";
            // 
            // lblUnit1
            // 
            resources.ApplyResources(this.lblUnit1, "lblUnit1");
            this.lblUnit1.Name = "lblUnit1";
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // grpPoly
            // 
            this.grpPoly.Controls.Add(this.rdRegularPoly);
            this.grpPoly.Controls.Add(this.nudPolySides);
            this.grpPoly.Controls.Add(this.label2);
            this.grpPoly.Controls.Add(this.txtPolySideLength);
            this.grpPoly.Controls.Add(this.label3);
            this.grpPoly.Controls.Add(this.lblUnit1);
            resources.ApplyResources(this.grpPoly, "grpPoly");
            this.grpPoly.Name = "grpPoly";
            this.grpPoly.TabStop = false;
            // 
            // rdRegularPoly
            // 
            resources.ApplyResources(this.rdRegularPoly, "rdRegularPoly");
            this.rdRegularPoly.Checked = true;
            this.rdRegularPoly.Name = "rdRegularPoly";
            this.rdRegularPoly.TabStop = true;
            this.rdRegularPoly.UseVisualStyleBackColor = true;
            this.rdRegularPoly.CheckedChanged += new System.EventHandler(this.rdShape_CheckedChanged);
            // 
            // nudPolySides
            // 
            resources.ApplyResources(this.nudPolySides, "nudPolySides");
            this.nudPolySides.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.nudPolySides.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.nudPolySides.Name = "nudPolySides";
            this.nudPolySides.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // grpCircle
            // 
            this.grpCircle.Controls.Add(this.rdCircle);
            this.grpCircle.Controls.Add(this.txtCircleRadius);
            this.grpCircle.Controls.Add(this.label6);
            this.grpCircle.Controls.Add(this.lblUnit4);
            resources.ApplyResources(this.grpCircle, "grpCircle");
            this.grpCircle.Name = "grpCircle";
            this.grpCircle.TabStop = false;
            // 
            // rdCircle
            // 
            resources.ApplyResources(this.rdCircle, "rdCircle");
            this.rdCircle.Name = "rdCircle";
            this.rdCircle.TabStop = true;
            this.rdCircle.UseVisualStyleBackColor = true;
            this.rdCircle.CheckedChanged += new System.EventHandler(this.rdShape_CheckedChanged);
            // 
            // txtCircleRadius
            // 
            resources.ApplyResources(this.txtCircleRadius, "txtCircleRadius");
            this.txtCircleRadius.Name = "txtCircleRadius";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // lblUnit4
            // 
            resources.ApplyResources(this.lblUnit4, "lblUnit4");
            this.lblUnit4.Name = "lblUnit4";
            // 
            // grpEllipse
            // 
            this.grpEllipse.Controls.Add(this.txtEllpsHeight);
            this.grpEllipse.Controls.Add(this.label9);
            this.grpEllipse.Controls.Add(this.lblUnit);
            this.grpEllipse.Controls.Add(this.txtEllpsWidth);
            this.grpEllipse.Controls.Add(this.label5);
            this.grpEllipse.Controls.Add(this.lblUnit5);
            this.grpEllipse.Controls.Add(this.rdEllipse);
            resources.ApplyResources(this.grpEllipse, "grpEllipse");
            this.grpEllipse.Name = "grpEllipse";
            this.grpEllipse.TabStop = false;
            // 
            // txtEllpsHeight
            // 
            resources.ApplyResources(this.txtEllpsHeight, "txtEllpsHeight");
            this.txtEllpsHeight.Name = "txtEllpsHeight";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // lblUnit
            // 
            resources.ApplyResources(this.lblUnit, "lblUnit");
            this.lblUnit.Name = "lblUnit";
            // 
            // txtEllpsWidth
            // 
            resources.ApplyResources(this.txtEllpsWidth, "txtEllpsWidth");
            this.txtEllpsWidth.Name = "txtEllpsWidth";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // lblUnit5
            // 
            resources.ApplyResources(this.lblUnit5, "lblUnit5");
            this.lblUnit5.Name = "lblUnit5";
            // 
            // rdEllipse
            // 
            resources.ApplyResources(this.rdEllipse, "rdEllipse");
            this.rdEllipse.Name = "rdEllipse";
            this.rdEllipse.TabStop = true;
            this.rdEllipse.UseVisualStyleBackColor = true;
            this.rdEllipse.CheckedChanged += new System.EventHandler(this.rdShape_CheckedChanged);
            // 
            // grpRectangle
            // 
            this.grpRectangle.Controls.Add(this.txtRectHeight);
            this.grpRectangle.Controls.Add(this.label11);
            this.grpRectangle.Controls.Add(this.lblUnit3);
            this.grpRectangle.Controls.Add(this.txtRectWidth);
            this.grpRectangle.Controls.Add(this.label13);
            this.grpRectangle.Controls.Add(this.lblUnit2);
            this.grpRectangle.Controls.Add(this.rdRectangle);
            resources.ApplyResources(this.grpRectangle, "grpRectangle");
            this.grpRectangle.Name = "grpRectangle";
            this.grpRectangle.TabStop = false;
            // 
            // txtRectHeight
            // 
            resources.ApplyResources(this.txtRectHeight, "txtRectHeight");
            this.txtRectHeight.Name = "txtRectHeight";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // lblUnit3
            // 
            resources.ApplyResources(this.lblUnit3, "lblUnit3");
            this.lblUnit3.Name = "lblUnit3";
            // 
            // txtRectWidth
            // 
            resources.ApplyResources(this.txtRectWidth, "txtRectWidth");
            this.txtRectWidth.Name = "txtRectWidth";
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // lblUnit2
            // 
            resources.ApplyResources(this.lblUnit2, "lblUnit2");
            this.lblUnit2.Name = "lblUnit2";
            // 
            // rdRectangle
            // 
            resources.ApplyResources(this.rdRectangle, "rdRectangle");
            this.rdRectangle.Name = "rdRectangle";
            this.rdRectangle.TabStop = true;
            this.rdRectangle.UseVisualStyleBackColor = true;
            this.rdRectangle.CheckedChanged += new System.EventHandler(this.rdShape_CheckedChanged);
            // 
            // InsertStockShapeForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpRectangle);
            this.Controls.Add(this.grpEllipse);
            this.Controls.Add(this.grpCircle);
            this.Controls.Add(this.grpPoly);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InsertStockShapeForm";
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.Load += new System.EventHandler(this.InsertStockShapeForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.InsertStockShapeForm_FormClosing);
            this.grpPoly.ResumeLayout(false);
            this.grpPoly.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPolySides)).EndInit();
            this.grpCircle.ResumeLayout(false);
            this.grpCircle.PerformLayout();
            this.grpEllipse.ResumeLayout(false);
            this.grpEllipse.PerformLayout();
            this.grpRectangle.ResumeLayout(false);
            this.grpRectangle.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPolySideLength;
        private System.Windows.Forms.Label lblUnit1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox grpPoly;
        private System.Windows.Forms.RadioButton rdRegularPoly;
        private System.Windows.Forms.NumericUpDown nudPolySides;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox grpCircle;
        private System.Windows.Forms.RadioButton rdCircle;
        private System.Windows.Forms.TextBox txtCircleRadius;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblUnit4;
        private System.Windows.Forms.GroupBox grpEllipse;
        private System.Windows.Forms.RadioButton rdEllipse;
        private System.Windows.Forms.TextBox txtEllpsHeight;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblUnit;
        private System.Windows.Forms.TextBox txtEllpsWidth;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblUnit5;
        private System.Windows.Forms.GroupBox grpRectangle;
        private System.Windows.Forms.TextBox txtRectHeight;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lblUnit3;
        private System.Windows.Forms.TextBox txtRectWidth;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label lblUnit2;
        private System.Windows.Forms.RadioButton rdRectangle;
    }
}