namespace mwIdentifier.Forms
{
    partial class frmIdentByShape
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmIdentByShape));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbIdentFrom = new System.Windows.Forms.ComboBox();
            this.cmbIdentWith = new System.Windows.Forms.ComboBox();
            this.lblSel = new System.Windows.Forms.Label();
            this.btnSelect = new System.Windows.Forms.Button();
            this.cmdIdentify = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.chbJustToExtents = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
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
            // cmbIdentFrom
            // 
            this.cmbIdentFrom.AccessibleDescription = null;
            this.cmbIdentFrom.AccessibleName = null;
            resources.ApplyResources(this.cmbIdentFrom, "cmbIdentFrom");
            this.cmbIdentFrom.BackgroundImage = null;
            this.cmbIdentFrom.Font = null;
            this.cmbIdentFrom.FormattingEnabled = true;
            this.cmbIdentFrom.Name = "cmbIdentFrom";
            // 
            // cmbIdentWith
            // 
            this.cmbIdentWith.AccessibleDescription = null;
            this.cmbIdentWith.AccessibleName = null;
            resources.ApplyResources(this.cmbIdentWith, "cmbIdentWith");
            this.cmbIdentWith.BackgroundImage = null;
            this.cmbIdentWith.Font = null;
            this.cmbIdentWith.FormattingEnabled = true;
            this.cmbIdentWith.Name = "cmbIdentWith";
            // 
            // lblSel
            // 
            this.lblSel.AccessibleDescription = null;
            this.lblSel.AccessibleName = null;
            resources.ApplyResources(this.lblSel, "lblSel");
            this.lblSel.Font = null;
            this.lblSel.Name = "lblSel";
            // 
            // btnSelect
            // 
            this.btnSelect.AccessibleDescription = null;
            this.btnSelect.AccessibleName = null;
            resources.ApplyResources(this.btnSelect, "btnSelect");
            this.btnSelect.BackgroundImage = null;
            this.btnSelect.Font = null;
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // cmdIdentify
            // 
            this.cmdIdentify.AccessibleDescription = null;
            this.cmdIdentify.AccessibleName = null;
            resources.ApplyResources(this.cmdIdentify, "cmdIdentify");
            this.cmdIdentify.BackgroundImage = null;
            this.cmdIdentify.Font = null;
            this.cmdIdentify.Name = "cmdIdentify";
            this.cmdIdentify.UseVisualStyleBackColor = true;
            this.cmdIdentify.Click += new System.EventHandler(this.cmdIdentify_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.AccessibleDescription = null;
            this.cmdCancel.AccessibleName = null;
            resources.ApplyResources(this.cmdCancel, "cmdCancel");
            this.cmdCancel.BackgroundImage = null;
            this.cmdCancel.Font = null;
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // chbJustToExtents
            // 
            this.chbJustToExtents.AccessibleDescription = null;
            this.chbJustToExtents.AccessibleName = null;
            resources.ApplyResources(this.chbJustToExtents, "chbJustToExtents");
            this.chbJustToExtents.BackgroundImage = null;
            this.chbJustToExtents.Font = null;
            this.chbJustToExtents.Name = "chbJustToExtents";
            this.chbJustToExtents.UseVisualStyleBackColor = true;
            // 
            // frmIdentByShape
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.Controls.Add(this.chbJustToExtents);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdIdentify);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.lblSel);
            this.Controls.Add(this.cmbIdentWith);
            this.Controls.Add(this.cmbIdentFrom);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = null;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmIdentByShape";
            this.Load += new System.EventHandler(this.frmIdentByShape_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbIdentFrom;
        private System.Windows.Forms.ComboBox cmbIdentWith;
        private System.Windows.Forms.Label lblSel;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Button cmdIdentify;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.CheckBox chbJustToExtents;
    }
}