// ********************************************************************************************************
// <copyright file="frmLabeler.cs" company="MapWindow.org">
// Copyright (c) MapWindow.org. All rights reserved.
// </copyright>
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http:// Www.mozilla.org/MPL/ 
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either express or implied. See the License for the specificlanguage governing rights and 
// limitations under the License. 
// 
// The Initial Developer of this version of the Original Code is Paul Meems.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
// Change Log: 
// Date            Changed By      Notes
// 10 August 2009  Paul Meems      Added some fixes for bug 1379 & 1380 and 
//              made changes recommended by StyleCop
// ********************************************************************************************************

namespace Forms
{
    using System;
    using System.Drawing;
    using System.Collections;
    using System.Windows.Forms;
    using System.Diagnostics;
    using MapWinGIS;

    public class frmLabeler : System.Windows.Forms.Form
    {

        // member variable
        private Labeler.mwLabeler m_parent;
        private System.Collections.Hashtable m_Layers;
        private MapWinGIS.tkCursor m_PreviousCursor;
        private Cursor m_Cursor;
        private bool m_Modifed;
        private string m_MapWinVersion;
        private Labeler.frmProgress ProgressBar;
        public int currentHandle = -1;
        private bool PopulatingFields = false;
        private MapWindow.Interfaces.IMapWin m_MapWin;

        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnFont;
        private System.Windows.Forms.FontDialog fontDialog;
        private System.Windows.Forms.TextBox txtFont;
        private System.Windows.Forms.ComboBox cbAlign;
        private System.Windows.Forms.ComboBox cbField;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ColorDialog colorDialog;
        private System.Windows.Forms.TextBox txtColor;
        private System.Windows.Forms.Button btnColor;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox cbEnableMinExtents;
        private System.Windows.Forms.Button btnSaveMinExtents;
        private System.Windows.Forms.CheckBox LabelsShadowCheckBox;
        private System.Windows.Forms.CheckBox LabelsScaleCheckBox;
        private System.Windows.Forms.TextBox txtShadowColor;
        private System.Windows.Forms.Button btnShadowColor;
        private System.Windows.Forms.ColorDialog shadowColorDialog;
        private System.Windows.Forms.Button btnSetScaleSize;
        private System.Windows.Forms.CheckBox UseLabelCollisionCheckBox;
        private System.Windows.Forms.CheckBox RemoveDuplicatesCheckBox;
        private Button btnRelabel;
        private ComboBox cbField2;
        private System.Windows.Forms.Label label5;
        private GroupBox groupBox3;
        private TextBox txtSecondLineAppend;
        private System.Windows.Forms.Label label9;
        private TextBox txtSecondLinePrepend;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private TextBox txtFirstLineAppend;
        private System.Windows.Forms.Label label8;
        private TextBox txtFirstLinePrepend;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private GroupBox groupBox4;
        private ComboBox cmbRotateField;
        private CheckBox chbRotate;
        private TextBox txtScale;
        private System.Windows.Forms.Label label12;
        private CheckBox LabelAllPartsCheckBox;
        private System.ComponentModel.Container components = null;

        public frmLabeler(Labeler.mwLabeler p, int LayerHandle, MapWindow.Interfaces.IMapWin MapWin)
        {
            // MapWinUtility.Logger.Message("frmLabeler Constructor");
            try
            {
                // 
                // Required for Windows Form Designer support
                // 
                InitializeComponent();

                currentHandle = LayerHandle;
                m_Layers = new Hashtable();
                m_parent = p;

                // set the pointing cursor
                m_Cursor = new Cursor(m_parent.GetType(), "pointing.cur");

                ProgressBar = new Labeler.frmProgress();
                ProgressBar.Owner = this;

                // set the parent form
                System.IntPtr tempPtr = (System.IntPtr)m_parent.m_ParentHandle;
                Form mapFrm = (Form)System.Windows.Forms.Control.FromHandle(tempPtr);
                mapFrm.AddOwnedForm(this);

                // get the mapwindow version
                m_MapWinVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo(mapFrm.GetType().Assembly.Location).FileVersion.ToString();

                btnColor.BackColor = colorDialog.Color;
                txtColor.Text = colorDialog.Color.ToString();
                shadowColorDialog.Color = Color.White;
                btnShadowColor.BackColor = shadowColorDialog.Color;
                txtShadowColor.Text = shadowColorDialog.Color.ToString();

                m_MapWin = MapWin;
            }
            catch (System.Exception ex)
            {
                ShowErrorBox("frmLabeler()", ex.Message);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLabeler));
            this.btnApply = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cbAlign = new System.Windows.Forms.ComboBox();
            this.fontDialog = new System.Windows.Forms.FontDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtFont = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.LabelAllPartsCheckBox = new System.Windows.Forms.CheckBox();
            this.UseLabelCollisionCheckBox = new System.Windows.Forms.CheckBox();
            this.btnSetScaleSize = new System.Windows.Forms.Button();
            this.btnShadowColor = new System.Windows.Forms.Button();
            this.txtShadowColor = new System.Windows.Forms.TextBox();
            this.LabelsScaleCheckBox = new System.Windows.Forms.CheckBox();
            this.LabelsShadowCheckBox = new System.Windows.Forms.CheckBox();
            this.btnColor = new System.Windows.Forms.Button();
            this.txtColor = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnFont = new System.Windows.Forms.Button();
            this.RemoveDuplicatesCheckBox = new System.Windows.Forms.CheckBox();
            this.cbField = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtScale = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.cbEnableMinExtents = new System.Windows.Forms.CheckBox();
            this.btnSaveMinExtents = new System.Windows.Forms.Button();
            this.shadowColorDialog = new System.Windows.Forms.ColorDialog();
            this.btnRelabel = new System.Windows.Forms.Button();
            this.cbField2 = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtSecondLineAppend = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtSecondLinePrepend = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtFirstLineAppend = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtFirstLinePrepend = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.cmbRotateField = new System.Windows.Forms.ComboBox();
            this.chbRotate = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnApply
            // 
            resources.ApplyResources(this.btnApply, "btnApply");
            this.btnApply.Name = "btnApply";
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnOk
            // 
            resources.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.Name = "btnOk";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cbAlign
            // 
            this.cbAlign.Items.AddRange(new object[] {
            resources.GetString("cbAlign.Items"),
            resources.GetString("cbAlign.Items1"),
            resources.GetString("cbAlign.Items2")});
            resources.ApplyResources(this.cbAlign, "cbAlign");
            this.cbAlign.Name = "cbAlign";
            this.cbAlign.SelectedIndexChanged += new System.EventHandler(this.cbAlign_SelectedIndexChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // txtFont
            // 
            this.txtFont.BackColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this.txtFont, "txtFont");
            this.txtFont.Name = "txtFont";
            this.txtFont.ReadOnly = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.LabelAllPartsCheckBox);
            this.groupBox1.Controls.Add(this.UseLabelCollisionCheckBox);
            this.groupBox1.Controls.Add(this.btnSetScaleSize);
            this.groupBox1.Controls.Add(this.btnShadowColor);
            this.groupBox1.Controls.Add(this.txtShadowColor);
            this.groupBox1.Controls.Add(this.LabelsScaleCheckBox);
            this.groupBox1.Controls.Add(this.LabelsShadowCheckBox);
            this.groupBox1.Controls.Add(this.btnColor);
            this.groupBox1.Controls.Add(this.txtColor);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.btnFont);
            this.groupBox1.Controls.Add(this.txtFont);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cbAlign);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.RemoveDuplicatesCheckBox);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // LabelAllPartsCheckBox
            // 
            resources.ApplyResources(this.LabelAllPartsCheckBox, "LabelAllPartsCheckBox");
            this.LabelAllPartsCheckBox.Name = "LabelAllPartsCheckBox";
            // 
            // UseLabelCollisionCheckBox
            // 
            resources.ApplyResources(this.UseLabelCollisionCheckBox, "UseLabelCollisionCheckBox");
            this.UseLabelCollisionCheckBox.Name = "UseLabelCollisionCheckBox";
            this.UseLabelCollisionCheckBox.CheckedChanged += new System.EventHandler(this.UseLabelCollisionCheckBox_CheckedChanged);
            // 
            // btnSetScaleSize
            // 
            resources.ApplyResources(this.btnSetScaleSize, "btnSetScaleSize");
            this.btnSetScaleSize.Name = "btnSetScaleSize";
            this.btnSetScaleSize.Click += new System.EventHandler(this.btnSetScaleSize_Click);
            // 
            // btnShadowColor
            // 
            resources.ApplyResources(this.btnShadowColor, "btnShadowColor");
            this.btnShadowColor.Name = "btnShadowColor";
            this.btnShadowColor.Click += new System.EventHandler(this.btnShadowColor_Click);
            // 
            // txtShadowColor
            // 
            this.txtShadowColor.BackColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this.txtShadowColor, "txtShadowColor");
            this.txtShadowColor.Name = "txtShadowColor";
            this.txtShadowColor.ReadOnly = true;
            // 
            // LabelsScaleCheckBox
            // 
            resources.ApplyResources(this.LabelsScaleCheckBox, "LabelsScaleCheckBox");
            this.LabelsScaleCheckBox.Name = "LabelsScaleCheckBox";
            this.LabelsScaleCheckBox.CheckedChanged += new System.EventHandler(this.LabelScaleCheckBox_CheckedChanged);
            // 
            // LabelsShadowCheckBox
            // 
            resources.ApplyResources(this.LabelsShadowCheckBox, "LabelsShadowCheckBox");
            this.LabelsShadowCheckBox.Name = "LabelsShadowCheckBox";
            this.LabelsShadowCheckBox.CheckedChanged += new System.EventHandler(this.LabelsShadowCheckBox_CheckedChanged);
            // 
            // btnColor
            // 
            resources.ApplyResources(this.btnColor, "btnColor");
            this.btnColor.Name = "btnColor";
            this.btnColor.Click += new System.EventHandler(this.btnColor_Click);
            // 
            // txtColor
            // 
            this.txtColor.BackColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this.txtColor, "txtColor");
            this.txtColor.Name = "txtColor";
            this.txtColor.ReadOnly = true;
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // btnFont
            // 
            resources.ApplyResources(this.btnFont, "btnFont");
            this.btnFont.Name = "btnFont";
            this.btnFont.Click += new System.EventHandler(this.btnFont_Click);
            // 
            // RemoveDuplicatesCheckBox
            // 
            resources.ApplyResources(this.RemoveDuplicatesCheckBox, "RemoveDuplicatesCheckBox");
            this.RemoveDuplicatesCheckBox.Name = "RemoveDuplicatesCheckBox";
            this.RemoveDuplicatesCheckBox.CheckedChanged += new System.EventHandler(this.RemoveDuplicatesCheckBox_CheckedChanged);
            // 
            // cbField
            // 
            this.cbField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbField.Items.AddRange(new object[] {
            resources.GetString("cbField.Items")});
            resources.ApplyResources(this.cbField, "cbField");
            this.cbField.Name = "cbField";
            this.cbField.SelectedIndexChanged += new System.EventHandler(this.cbField_SelectedIndexChanged);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtScale);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.cbEnableMinExtents);
            this.groupBox2.Controls.Add(this.btnSaveMinExtents);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // txtScale
            // 
            this.txtScale.BackColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this.txtScale, "txtScale");
            this.txtScale.Name = "txtScale";
            this.txtScale.TextChanged += new System.EventHandler(this.txtScale_TextChanged);
            this.txtScale.Leave += new System.EventHandler(this.txtScale_Leave);
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // cbEnableMinExtents
            // 
            resources.ApplyResources(this.cbEnableMinExtents, "cbEnableMinExtents");
            this.cbEnableMinExtents.Name = "cbEnableMinExtents";
            this.cbEnableMinExtents.CheckedChanged += new System.EventHandler(this.cbEnableMinExtents_CheckedChanged);
            // 
            // btnSaveMinExtents
            // 
            resources.ApplyResources(this.btnSaveMinExtents, "btnSaveMinExtents");
            this.btnSaveMinExtents.Name = "btnSaveMinExtents";
            this.btnSaveMinExtents.Click += new System.EventHandler(this.btnSaveMinExtents_Click);
            // 
            // btnRelabel
            // 
            resources.ApplyResources(this.btnRelabel, "btnRelabel");
            this.btnRelabel.Name = "btnRelabel";
            this.btnRelabel.Click += new System.EventHandler(this.btnRelabel_Click);
            // 
            // cbField2
            // 
            this.cbField2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbField2.Items.AddRange(new object[] {
            resources.GetString("cbField2.Items")});
            resources.ApplyResources(this.cbField2, "cbField2");
            this.cbField2.Name = "cbField2";
            this.cbField2.SelectedIndexChanged += new System.EventHandler(this.cbField2_SelectedIndexChanged);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtSecondLineAppend);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.txtSecondLinePrepend);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.txtFirstLineAppend);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.txtFirstLinePrepend);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label6);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // txtSecondLineAppend
            // 
            this.txtSecondLineAppend.BackColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this.txtSecondLineAppend, "txtSecondLineAppend");
            this.txtSecondLineAppend.Name = "txtSecondLineAppend";
            this.txtSecondLineAppend.TextChanged += new System.EventHandler(this.txtAppendPrepend_TextChanged);
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // txtSecondLinePrepend
            // 
            this.txtSecondLinePrepend.BackColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this.txtSecondLinePrepend, "txtSecondLinePrepend");
            this.txtSecondLinePrepend.Name = "txtSecondLinePrepend";
            this.txtSecondLinePrepend.TextChanged += new System.EventHandler(this.txtAppendPrepend_TextChanged);
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // txtFirstLineAppend
            // 
            this.txtFirstLineAppend.BackColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this.txtFirstLineAppend, "txtFirstLineAppend");
            this.txtFirstLineAppend.Name = "txtFirstLineAppend";
            this.txtFirstLineAppend.TextChanged += new System.EventHandler(this.txtAppendPrepend_TextChanged);
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // txtFirstLinePrepend
            // 
            this.txtFirstLinePrepend.BackColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this.txtFirstLinePrepend, "txtFirstLinePrepend");
            this.txtFirstLinePrepend.Name = "txtFirstLinePrepend";
            this.txtFirstLinePrepend.TextChanged += new System.EventHandler(this.txtAppendPrepend_TextChanged);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.cmbRotateField);
            this.groupBox4.Controls.Add(this.chbRotate);
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // cmbRotateField
            // 
            this.cmbRotateField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRotateField.Items.AddRange(new object[] {
            resources.GetString("cmbRotateField.Items")});
            resources.ApplyResources(this.cmbRotateField, "cmbRotateField");
            this.cmbRotateField.Name = "cmbRotateField";
            this.cmbRotateField.SelectedIndexChanged += new System.EventHandler(this.cmbRotateField_SelectedIndexChanged);
            // 
            // chbRotate
            // 
            resources.ApplyResources(this.chbRotate, "chbRotate");
            this.chbRotate.Name = "chbRotate";
            this.chbRotate.CheckedChanged += new System.EventHandler(this.chbRotate_CheckedChanged);
            // 
            // frmLabeler
            // 
            resources.ApplyResources(this, "$this");
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.cbField2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnRelabel);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.cbField);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnApply);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLabeler";
            this.Load += new System.EventHandler(this.frmLabeler_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmLabeler_Closing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private Hashtable FindFieldCache = new Hashtable();
        private int FindField(string Name, ref MapWinGIS.Shapefile sf)
        {
            if (!FindFieldCache.Contains(sf.Filename)) FindFieldCache.Add(sf.Filename, new Hashtable());
            if (((Hashtable)FindFieldCache[sf.Filename]).Contains(Name)) return (int)((Hashtable)FindFieldCache[sf.Filename])[Name];

            for (int i = 0; i < sf.NumFields; i++)
                if (sf.get_Field(i).Name.ToLower().Trim() == Name.ToLower().Trim())
                {
                    ((Hashtable)FindFieldCache[sf.Filename]).Add(Name, i);
                    return i;
                }

            return -1;
        }

        public void SetCurrentLayer(int handle)
        {
            currentHandle = handle;
            Initialize();
        }

        private void btnFont_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (currentHandle != -1 && this.PopulatingFields == false)
                {
                    System.Windows.Forms.DialogResult result;
                    Label label = (Label)m_Layers[currentHandle];

                    // Paul Meems 6/11/2009
                    // Bug #953: Labeler selects wrong font 
                    if (label.font.ToString() != "") fontDialog.Font = label.font;
                    result = fontDialog.ShowDialog();

                    if (result == System.Windows.Forms.DialogResult.Cancel)
                        return;

                    // save changes                    
                    label.font = fontDialog.Font;
                    label.Modified = true;

                    txtFont.Text = fontDialog.Font.Name + ", " + fontDialog.Font.Size.ToString();

                    // reset the label
                    m_Layers[currentHandle] = label;

                    // set modified
                    if (!PopulatingFields)
                        this.SetModified(true);
                }
            }
            catch (System.Exception ex)
            {
                ShowErrorBox("btnFont_Click()", ex.Message);
            }
        }

        private void btnColor_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (currentHandle != -1 && this.PopulatingFields == false)
                {
                    System.Windows.Forms.DialogResult result;
                    result = colorDialog.ShowDialog();

                    if (result == System.Windows.Forms.DialogResult.Cancel)
                        return;

                    // save changes

                    Label label = (Label)m_Layers[currentHandle];
                    label.color = colorDialog.Color;
                    label.Modified = true;

                    txtColor.Text = colorDialog.Color.ToString();
                    btnColor.BackColor = colorDialog.Color;

                    // reset the label
                    m_Layers[currentHandle] = label;

                    // set modified
                    if (!PopulatingFields)
                        this.SetModified(true);
                }
            }
            catch (System.Exception ex)
            {
                ShowErrorBox("btnColor_Click()", ex.Message);
            }
        }

        private void cbField_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (currentHandle != -1 && this.PopulatingFields == false)
                {
                    // save changes

                    Label label = (Label)m_Layers[currentHandle];

                    if (label.field == 0 && cbField.SelectedIndex != 0)
                        label.CalculatePos = true;

                    label.field = cbField.SelectedIndex;
                    label.Modified = true;
                    label.updateHeaderOnly = false;

                    // reset the label
                    m_Layers[currentHandle] = label;

                    // set modified
                    if (!PopulatingFields)
                        this.SetModified(true);
                }
            }
            catch (System.Exception ex)
            {
                ShowErrorBox("cbField_SelectedIndexChanged()", ex.Message);
            }

        }

        private void cbField2_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (currentHandle != -1 && this.PopulatingFields == false)
                {
                    // save changes

                    Label label = (Label)m_Layers[currentHandle];

                    if (label.field2 == 0 && cbField2.SelectedIndex != 0)
                        label.CalculatePos = true;

                    label.field2 = cbField2.SelectedIndex;
                    label.Modified = true;
                    label.updateHeaderOnly = false;

                    // reset the label
                    m_Layers[currentHandle] = label;

                    // set modified
                    if (!PopulatingFields)
                        this.SetModified(true);
                }
            }
            catch (System.Exception ex)
            {
                ShowErrorBox("cbField_SelectedIndexChanged()", ex.Message);
            }

        }

        private void cbAlign_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (currentHandle != -1 && this.PopulatingFields == false)
                {
                    // save changes

                    Label label = (Label)m_Layers[currentHandle];
                    label.alignment = (MapWinGIS.tkHJustification)cbAlign.SelectedIndex;
                    label.Modified = true;

                    // reset the label
                    m_Layers[currentHandle] = label;

                    // set modified
                    if (!PopulatingFields)
                        this.SetModified(true);
                }
            }
            catch (System.Exception ex)
            {
                ShowErrorBox("cbField_SelectedIndexChanged()", ex.Message);
            }

        }

        private void LoadShapeFileLayers()
        {
            MapWindow.Interfaces.eLayerType layerType;
            Label label;

            layerType = m_parent.m_MapWin.Layers[currentHandle].LayerType;

            // check to make sure it is a shapefile
            if (layerType == MapWindow.Interfaces.eLayerType.LineShapefile
                || layerType == MapWindow.Interfaces.eLayerType.PointShapefile
                || layerType == MapWindow.Interfaces.eLayerType.PolygonShapefile)
            {
                if (m_Layers.Contains(currentHandle) == false && !m_parent.m_MapWin.Layers[currentHandle].HideFromLegend)
                {
                    label = new Label();
                    label.points = new System.Collections.ArrayList();
                    label.handle = currentHandle;
                    label.alignment = MapWinGIS.tkHJustification.hjCenter;
                    label.field = 0;
                    label.field2 = 0;
                    label.font = txtFont.Font;
                    label.color = System.Drawing.Color.Black;
                    label.shadowColor = System.Drawing.Color.White;
                    label.UseShadows = false;
                    label.Scaled = false;
                    label.UseLabelCollision = false;
                    label.RemoveDuplicates = false;
                    label.labelShape = new System.Collections.ArrayList();
                    label.xml_LblFile = "";
                    label.updateHeaderOnly = false;
                    label.UseMinExtents = false;
                    label.scale = 0;
                    label.extents = null;

                    // load all the layers into the hashtable
                    m_Layers.Add(currentHandle, label);
                }
            }
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            System.Windows.Forms.DialogResult result;
            if (this.m_Modifed == true)
            {
                // Add the Cancel button to enable user to go back to Labeler window.
                // Enhancement Added: 04/15/2008 Earljon Hidalgo
                // result = MapWinUtility.Logger.Message("Do you wish to save your changes?","Labeler",System.Windows.Forms.MessageBoxButtons.YesNo,System.Windows.Forms.MessageBoxIcon.Question, DialogResult.Yes);
                result = MapWinUtility.Logger.Message("Do you wish to save your changes?", "Labeler", System.Windows.Forms.MessageBoxButtons.YesNoCancel, System.Windows.Forms.MessageBoxIcon.Question, DialogResult.Yes);

                // Return to Labeler window if Cancel is clicked.
                if (result == System.Windows.Forms.DialogResult.Cancel) return;
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    if (!ValidateRotationField()) return;

                    // Apply the lableing
                    ApplyChanges();
                    SaveAllLabelingInfo();
                }

            }
            this.Hide();
            this.m_parent.m_MapWindowForm.Focus();
        }

        private void frmLabeler_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // MapWinUtility.Logger.Message("frmLabeler_Closing");
            e.Cancel = true;
            this.Hide();
            this.m_parent.m_MapWindowForm.Focus();
        }

        private void btnApply_Click(object sender, System.EventArgs e)
        {
            if (!ValidateRotationField()) return;
            // Apply the labeling
            if (this.m_Modifed == true)
            {
                ApplyChanges();
                SaveAllLabelingInfo();
            }

            this.ProgressBar.Hide();
        }

        private void btnOk_Click(object sender, System.EventArgs e)
        {
            if (!ValidateRotationField()) return;

            // Apply the labeling
            if (this.m_Modifed == true)
            {
                ApplyChanges();
                SaveAllLabelingInfo();
            }

            this.ProgressBar.Hide();
            this.Hide();
            this.m_parent.m_MapWindowForm.Focus();
        }

        private void ApplyChanges()
        {
            try
            {
                m_parent.m_MapWin.View.LockMap();

                int numShapes = 0;
                int prg = 0;
                int lprg = 0;
                double x = 0, y = 0, rotation = 0;
                string shapeValue = "";
                Shapefile shpFile = null;
                Hashtable labelPoints = new Hashtable();

                // change the cursor to a wait cursor
                m_PreviousCursor = m_parent.m_MapWin.View.MapCursor;
                m_parent.m_MapWin.View.MapCursor = MapWinGIS.tkCursor.crsrWait;

                Label label = (Label)m_Layers[currentHandle];
                shpFile = (Shapefile)m_parent.m_MapWin.Layers[label.handle].GetObject();

                shpFile.BeginPointInShapefile();

                // if the label field is not None then do the following
                if (label.field != 0 && label.Modified == true && label.updateHeaderOnly == false)
                {
                    ProgressBar.Show();
                    // set the progress bar
                    ProgressBar.SetProgress("Calculating label positions", 0);

                    // set the fontstyle
                    // Paul Meems 6/11/2009
                    // Bug #913: Label setup ignores font style 
                    System.Drawing.FontStyle fstyle = new System.Drawing.FontStyle();
                    if (label.font.Bold) fstyle |= System.Drawing.FontStyle.Bold;
                    if (label.font.Italic) fstyle |= System.Drawing.FontStyle.Italic;
                    if (label.font.Underline) fstyle |= System.Drawing.FontStyle.Underline;
                    m_parent.m_MapWin.Layers[currentHandle].Font(label.font.Name, (int)label.font.Size, fstyle);

                    // find out the number of shapes in the shapefile
                    numShapes = shpFile.NumShapes;

                    label.points = new System.Collections.ArrayList();
                    label.labelShape = new System.Collections.ArrayList();
                    label.CalculatePos = true;

                    Stopwatch sw = new Stopwatch();
                    sw.Start();

                    #region Add labels to every shape
                    // add labels to every shape
                    for (int j = 0; j < numShapes; j++)
                    {
                        int shapeNumParts = 0;
                        int indexStart = -1;
                        if (label.LabelAllParts == true)
                        {
                            shapeNumParts = shpFile.get_Shape(j).NumParts;
                            indexStart = 0;
                        }

                        MapWinGIS.Shape shape = shpFile.get_Shape(j);
                        int numPoints = 0;
                        double[] shapePoints = shpFile.QuickPoints(j, ref numPoints) as double[];
                        for (int i = indexStart; i < shapeNumParts; i++)
                        {

                            if (label.CalculatePos == true)
                            {
                                FindXYValues(shpFile, j, shape, i, shapePoints, ref x, ref y);

                                //If either of the numbers is NAN then its a hole and we don't label the part
                                if (double.IsNaN(x) || double.IsNaN(y))
                                    continue;

                                //if (label.field2 != 0 || label.PrependLine2 != "" || label.AppendLine2 != "")
                                //{
                                //    double disregardX = 0, adjustY0 = 0, adjustY1 = 0;
                                //    m_MapWin.View.PixelToProj(0, CreateGraphics().MeasureString("ZZZ", label.font).Height / 2, ref disregardX, ref adjustY0);
                                //    m_MapWin.View.PixelToProj(0, 0, ref disregardX, ref adjustY1);
                                //    y += Math.Abs(adjustY1 - adjustY0);
                                //}

                                // set the x and y values
                                Point p = new Point();
                                p.x = x; p.y = y;

                                if (chbRotate.Checked)
                                {
                                    object fldVal = shpFile.get_CellValue(FindField(cmbRotateField.Text, ref shpFile), j);
                                    if (fldVal == null)
                                    {
                                        rotation = 0;
                                    }
                                    else
                                    {
                                        double.TryParse(fldVal.ToString(), out rotation);
                                    }
                                    p.rotation = rotation;
                                }
                                else
                                {
                                    p.rotation = 0;
                                    rotation = 0;
                                }
                                label.points.Add(p);
                            }
                            else
                            {
                                x = ((Point)label.points[j]).x;
                                y = ((Point)label.points[j]).y;
                                if (chbRotate.Checked)
                                {
                                    object fldVal = shpFile.get_CellValue(FindField(cmbRotateField.Text, ref shpFile), j);
                                    if (fldVal == null)
                                    {
                                        rotation = 0;
                                    }
                                    else
                                    {
                                        double.TryParse(fldVal.ToString(), out rotation);
                                    }
                                }
                                else
                                {
                                    rotation = 0;
                                }
                            }

                            shapeValue = txtFirstLinePrepend.Text + shpFile.get_CellValue(label.field - 1, j).ToString() + txtFirstLineAppend.Text;


                            if (label.field2 != 0)
                            {
                                shapeValue += Environment.NewLine + i.ToString();
                                shapeValue += txtSecondLinePrepend.Text + shpFile.get_CellValue(label.field2 - 1, j).ToString() + txtSecondLineAppend.Text;
                            }

                            label.labelShape.Add(1 + j);
                            m_parent.m_MapWin.Layers[currentHandle].AddLabelEx(shapeValue, label.color, x, y, label.alignment, rotation);

                            label.AppendLine1 = txtFirstLineAppend.Text;
                            label.AppendLine2 = txtSecondLineAppend.Text;
                            label.PrependLine1 = txtFirstLinePrepend.Text;
                            label.PrependLine2 = txtSecondLinePrepend.Text;

                            // set the progress bar
                            prg = (int)(j / (double)numShapes * 100);
                            if (prg > lprg)
                            {
                                lprg = prg;
                                ProgressBar.SetProgress("Calculating label positions.", prg);
                            }
                        }
                    }
                    #endregion

                    sw.Stop();
                    Debug.WriteLine("Time to label: " + sw.ElapsedMilliseconds.ToString());

                    // } Needed when duplicate removal code is uncommented

                    m_parent.m_MapWin.Layers[currentHandle].LabelsScale = label.Scaled;
                    m_parent.m_MapWin.Layers[currentHandle].LabelsShadow = label.UseShadows;
                    m_parent.m_MapWin.Layers[currentHandle].LabelsShadowColor = label.shadowColor;
                    m_parent.m_MapWin.Layers[currentHandle].UseLabelCollision = label.UseLabelCollision;
                    m_parent.m_MapWin.Layers[currentHandle].StandardViewWidth = label.StandardViewWidth;
                    // Force labels visible after applying (during editing):
                    m_parent.m_MapWin.Layers[currentHandle].LabelsVisible = true;

                    // save the label info for this layer
                    m_Layers[currentHandle] = label;
                }
                else
                {
                    if (label.field == 0)
                        m_parent.m_MapWin.Layers[currentHandle].ClearLabels();
                }

                shpFile.EndPointInShapefile();
            }
            catch (System.Exception ex)
            {                
                ShowErrorBox("ApplyChanges()", ex.Message);
            }
            finally
            {
                //if(this.ProgressBar.Visible)
                    this.ProgressBar.Hide();

                m_parent.m_MapWin.View.UnlockMap();
                // change the cursor back to it's default
                m_parent.m_MapWin.View.MapCursor = m_PreviousCursor;
            }

        }

        public void OpenLabelingInfo()
        {
            if (currentHandle == -1) return;

            Label label = new Label();
            Labeler.Classes.XMLLabelFile xmlLabel = new Labeler.Classes.XMLLabelFile(this.m_parent.m_MapWin, this.m_MapWinVersion);

            // clear all previous labels
            m_Layers.Clear();

            // load all labeling info

            if (xmlLabel.LoadLabelInfo(m_parent.m_MapWin, m_parent.m_MapWin.Layers[currentHandle], ref label, this))
                m_Layers.Add(currentHandle, label);
        }

        private void SaveAllLabelingInfo()
        {
            try
            {
                Shapefile shpFile = null;

                Label label = (Label)m_Layers[currentHandle];
                shpFile = (Shapefile)m_parent.m_MapWin.Layers[currentHandle].GetObject();

                if (label.Modified || label.LabelExtentsChanged)
                {
                    // save the changes to the .lbl file
                    SaveLBLFile(ref label, shpFile.Filename);
                    // SaveLBLFile((Forms.Label)m_Layers[currentHandle],shpFile.Filename);

                    // force mapwindow to update it labeling info for this layer
                    m_parent.m_MapWin.Layers[label.handle].UpdateLabelInfo();

                    label.Modified = false;
                    label.updateHeaderOnly = true;
                    label.CalculatePos = false;
                    label.LabelExtentsChanged = false;
                    m_Layers[currentHandle] = label;
                }

                // set modified 
                this.SetModified(false);
            }
            catch (System.Exception ex)
            {
                ShowErrorBox("SaveAllLabelingInfo()", ex.Message);
            }
        }

        private void SaveLBLFile(ref Label labels, string shpFileName)
        {
            string fileName1 = System.IO.Path.ChangeExtension(shpFileName, ".lbl");
            string fileName2 = "";
            string projectDirName = "";

            if (m_parent.m_MapWin.View.LabelsUseProjectLevel)
            {
                if (m_parent.m_MapWin.Project.FileName != null && m_parent.m_MapWin.Project.FileName.Trim() != "")
                {
                    projectDirName = System.IO.Path.GetFileNameWithoutExtension(m_parent.m_MapWin.Project.FileName);
                    fileName2 = projectDirName + @"\" + System.IO.Path.ChangeExtension(System.IO.Path.GetFileName(shpFileName), ".lbl");
                }
            }

            // if the field = 0 (none) then delete the file if it exists
            if (labels.field == 0)
            {
                if (System.IO.File.Exists(fileName1))
                    System.IO.File.Delete(fileName1);
                if (fileName2 != "" && System.IO.File.Exists(fileName2))
                    System.IO.File.Delete(fileName2);
            }
            else
            {
                Labeler.Classes.XMLLabelFile xmlLabel = new Labeler.Classes.XMLLabelFile(this.m_parent.m_MapWin, this.m_MapWinVersion);
                if (labels.updateHeaderOnly && labels.xml_LblFile != "")
                {
                    xmlLabel.ReplaceHeader(ref labels, fileName1);
                }
                else
                {
                    xmlLabel.SaveLabelInfo(ref labels, fileName1);
                }

                try
                {
                    if (fileName2 != "")
                    {
                        if (System.IO.File.Exists(fileName2))
                            System.IO.File.Delete(fileName2);

                        if (!System.IO.Directory.Exists(projectDirName))
                            System.IO.Directory.CreateDirectory(projectDirName);

                        System.IO.File.Copy(fileName1, fileName2);
                    }
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Finds the centroid of the shape if partIndex = -1 or the centroid of the part if partIndex >= 0
        /// </summary>
        /// <param name="shpFile">The shapefile to find he centroid of</param>
        /// <param name="shapeIndex">The shapeID of the shape to use</param>
        /// <param name="shape"></param>
        /// <param name="shapePoints"></param>
        /// <param name="partIndex">The part to look for the centroid of, -1 for the whole shape</param>
        /// <param name="x">X coord of centroid</param>
        /// <param name="y">Y coord of centroid</param>
        private void FindXYValues(MapWinGIS.Shapefile shpFile, int shapeIndex, MapWinGIS.Shape shape, int partIndex, double[] shapePoints, ref double x, ref double y)
        {
            // Paul Meems 23 Oct. 2009, Bug #1456
            // FindXYValues won't always return an X and Y, when for a shapefile no X and Y values are returned at all
            // MW goes in to a continious loop
            // I've changed this func a lot to fix this.
            // The original func is below and is renamed FindXYValues_Old
            MapWinUtility.Logger.Dbg(string.Format("Starting FindXYValues for shapeid: {0}, partID: {1} of file {2}",shapeIndex, partIndex, System.IO.Path.GetFileName(shpFile.Filename)));
            try
            {
                #region Polygon
                // modified: Z and M types added by Cornelius Mende
                // Modified2: replaced | with ||, Paul Meems 23 Oct. 2009
                if (shape.ShapeType == MapWinGIS.ShpfileType.SHP_POLYGON || shape.ShapeType == MapWinGIS.ShpfileType.SHP_POLYGONM || shape.ShapeType == MapWinGIS.ShpfileType.SHP_POLYGONZ) 
                {
                    // Paul Meems 23 Oct. 2009, Bug #1456
                    // To make sure at least and X and Y are returned
                    // Start with calling shp.Centroid
                    MapWinGIS.IPoint myTestPoint = GetTestCentroid(shpFile.get_Shape(shapeIndex));
                    // If myTestPoint is in the shape, stop and return point:
                    x = myTestPoint.x;
                    y = myTestPoint.y;
                    if (partIndex == -1 || shape.NumParts <= 1)
                    {
                        // Only 1 part needs to be labeled,
                        // if the point is in the shape stop the calculation.
                        if (shpFile.PointInShape(shapeIndex, x, y))
                        {
                            // Found a correct point, no need to look any further:
                            MapWinUtility.Logger.Dbg("Found a correct point using shape.Centroid()");
                            return;
                        }
                    }
                    // End modification Paul Meems 23 Oct. 2009, Bug #1456
                    
                    double area = 0;
                    int count = shape.numPoints;
                    int startPartIndex;
                    int stopPartIndex;

                    //Decides which part to calculate the centroid of
                    if (partIndex == -1 || shape.NumParts <= 1)
                    {
                        startPartIndex = 0;
                        stopPartIndex = count;
                    }
                    else
                    {
                        startPartIndex = shape.get_Part(partIndex);
                        if (partIndex < shape.NumParts - 1)
                            stopPartIndex = shape.get_Part(partIndex + 1);
                        else
                            stopPartIndex = count;
                    }

                    //If it has less then 4 points then we can quickly approximate calculate its centroid quickly from its center point
                    if (stopPartIndex - startPartIndex <= 4)
                    {
                        double minX = double.MaxValue;
                        double minY = double.MaxValue;
                        double maxX = double.MinValue;
                        double maxY = double.MinValue;

                        try
                        {
                            for (int i = startPartIndex; i < stopPartIndex; i++)
                            {
                                if (shapePoints[2 * i] < minX)
                                    minX = shapePoints[2 * i];
                                if (shapePoints[(2 * i) + 1] < minY)
                                    minY = shapePoints[(2 * i) + 1];
                                if (shapePoints[2 * i] > maxX)
                                    maxX = shapePoints[2 * i];
                                if (shapePoints[(2 * i) + 1] > maxY)
                                    maxY = shapePoints[(2 * i) + 1];
                            }
                        }
                        catch (System.Exception ex)
                        {                             
                            return; 
                        }                
                        

                        // May/12/2010 DK === Is this centroid? or just the size of the polygon?
                        //x = minX - minX;
                        //y = maxY - minY;

                        x = minX + (maxX - minX) / 2;
                        y = minY + (maxY - minY) / 2;
                        return;
                    }
                    else
                    {
                        // calculate the area of the poly
                        for (int i = startPartIndex; i < stopPartIndex; i++)
                        {
                            if (i != stopPartIndex - 1)
                                area += (shapePoints[2 * i] * shapePoints[(2 * i) + 3] - shapePoints[(2 * i) + 2] * shapePoints[(2 * i) + 1]);
                            else
                                area += (shapePoints[2 * i] * shapePoints[(2 * startPartIndex) + 1] - shapePoints[2 * startPartIndex] * shapePoints[(2 * i) + 1]);

                        }
                        area *= .5;

                        //If the area is greater than 0 then its a hole (we got the math inverted some how but it doesn't matter)
                        if (area > 0)
                        {
                            //x = double.NaN;
                            //y = double.NaN;

                            // Paul Meems 23 Oct. 2009, Bug #1456
                            // It might be a hole if it is a multipart shape, else it is just outside the shape.
                            // If it is outside the shape, just return the test centroid coordianates:
                            if (shape.NumParts > 1)
                            {
                                MapWinUtility.Logger.Dbg("Probably found a hole in a multipart shape");
                                x = double.NaN;
                                y = double.NaN;
                            }
                            else
                            {
                                MapWinUtility.Logger.Dbg("Point is outside the shape");
                            }
                            // End modification, Paul Meems 23 Oct. 2009                            
                            return;
                        }

                        
                        // Paul Meems 23 Oct. 2009, Bug #1456
                        // Don't just set to zero, save shape.Centroid values
                        double xCentroid = x;
                        double yCentroid = y;
                        // End modification, Paul Meems 23 Oct. 2009                            
                        x = 0;
                        y = 0;

                        for (int i = startPartIndex; i < stopPartIndex; i++)
                        {
                            if (i != stopPartIndex - 1)
                            {
                                x += (shapePoints[2 * i] + shapePoints[(2 * i) + 2]) * (shapePoints[2 * i] * shapePoints[(2 * i) + 3] - shapePoints[(2 * i) + 2] * shapePoints[(2 * i) + 1]);
                                y += (shapePoints[(2 * i) + 1] + shapePoints[(2 * i) + 3]) * (shapePoints[2 * i] * shapePoints[(2 * i) + 3] - shapePoints[(2 * i) + 2] * shapePoints[(2 * i) + 1]);
                            }
                            else
                            {
                                x += (shapePoints[2 * i] + shapePoints[2 * startPartIndex]) * (shapePoints[2 * i] * shapePoints[(2 * startPartIndex) + 1] - shapePoints[2 * startPartIndex] * shapePoints[(2 * i) + 1]);
                                y += (shapePoints[(2 * i) + 1] + shapePoints[(2 * startPartIndex) + 1]) * (shapePoints[2 * i] * shapePoints[(2 * startPartIndex) + 1] - shapePoints[2 * startPartIndex] * shapePoints[(2 * i) + 1]);
                            }
                        }
                        x *= 1 / (6 * area);
                        y *= 1 / (6 * area);

                        // test to make sure the centroid is the poly
                        if (shpFile.PointInShape(shapeIndex, x, y) == false)
                        {
                            double minFound = double.MaxValue;
                            int ptIndex = 0;
                            for (int i = startPartIndex; i < stopPartIndex; i++)
                            {
                                double tempDist = Math.Sqrt(((shapePoints[2 * i] - x) * (shapePoints[2 * i] - x)) + ((shapePoints[(2 * i) + 1] - y) * (shapePoints[(2 * i) + 1] - y)));
                                if (tempDist < minFound)
                                {
                                    ptIndex = i;
                                    minFound = tempDist;
                                }
                            }
                            x = shapePoints[2 * ptIndex];
                            y = shapePoints[(2 * ptIndex) + 1];
                        }

                        if (x == -1 || y == -1 || x == 0 || y == 0)
                        {
                            // Detection failed... simple center?
                            //x = ((shape.Extents.xMax - shape.Extents.xMin) / 2) + shape.Extents.xMin;
                            //y = ((shape.Extents.yMax - shape.Extents.yMin) / 2) + shape.Extents.yMin;

                            // Paul Meems 23 Oct. 2009, Bug #1456
                            // Use earlier calculated centroid values:
                            x = xCentroid;
                            y = yCentroid;
                            MapWinUtility.Logger.Dbg("Use earlier calculated centroid values");
                            // End modification, Paul Meems 23 Oct. 2009
                        }

                        return;
                    }
                }
                #endregion

                #region Point
                // modified: Z and M types added by Cornelius Mende
                // Modified2: replaced | with ||, Paul Meems 23 Oct. 2009
                else if (shape.ShapeType == MapWinGIS.ShpfileType.SHP_POINT || shape.ShapeType == MapWinGIS.ShpfileType.SHP_POINTM || shape.ShapeType == MapWinGIS.ShpfileType.SHP_POINTZ) 
                {
                    x = shape.Extents.xMax;
                    y = shape.Extents.yMax;
                    return;
                }
                #endregion

                #region Multipoint
                // modified: Z and M types added by Cornelius Mende
                // Modified2: replaced | with ||, Paul Meems 23 Oct. 2009
                else if (shape.ShapeType == MapWinGIS.ShpfileType.SHP_MULTIPOINT || shape.ShapeType == MapWinGIS.ShpfileType.SHP_MULTIPOINTM || shape.ShapeType == MapWinGIS.ShpfileType.SHP_MULTIPOINTZ)
                {
                    x = (shape.Extents.xMin + shape.Extents.xMax) / 2;
                    y = (shape.Extents.yMin + shape.Extents.yMax) / 2;
                    return;
                }
                #endregion

                #region Line
                // modified: Z and M types added by Cornelius Mende
                else if (shape.ShapeType == MapWinGIS.ShpfileType.SHP_POLYLINE || shape.ShapeType == MapWinGIS.ShpfileType.SHP_POLYLINEM || shape.ShapeType == MapWinGIS.ShpfileType.SHP_POLYLINEZ)
                {
                    int count = shape.numPoints, index1 = 0, index2 = 0;
                    double max_length = -1.0;
                    for (int i = 1; i < count; i++)
                    {
                        double length = GetLineLength(shape.get_Point(i - 1), shape.get_Point(i));
                        if (length > max_length)
                        {
                            index1 = i - 1;
                            index2 = i;
                            max_length = length;
                        }
                    }

                    double opposite = shape.get_Point(index2).y - shape.get_Point(index1).y;
                    double adjacent = shape.get_Point(index2).x - shape.get_Point(index1).x;
                    // 					double opposite = shape.get_Point(count/2).y-shape.get_Point(0).y;
                    // 					double adjacent = shape.get_Point(count/2).x-shape.get_Point(0).x;

                    // rotation = Math.Atan(opposite/adjacent) * (180 / Math.PI);
                    double temp_x1 = shape.get_Point(index1).x, temp_x2 = shape.get_Point(index2).x, temp_y1 = shape.get_Point(index1).y, temp_y2 = shape.get_Point(index2).y;
                    x = temp_x1 + ((temp_x2 - temp_x1) / 2);
                    y = temp_y1 + ((temp_y2 - temp_y1) / 2);
                    // 					x = shape.get_Point(count/2).x;
                    // 					y = shape.get_Point(count/2).y;
                    return;
                }
                #endregion
            }
            catch (System.Exception ex)
            {
                ShowErrorBox("FindXYValues()", shpFile.Filename + ", " + shapeIndex + ", " + ex.Message);
            }
        }

        /// <summary>
        /// Get centroid which will be used if no correct label point can be found.
        /// This functions tests if the shape is valid, if not it will be buffered and 
        /// hopefully be valid now, the valid buffered shape will be used to get the centroid
        /// If that won't work, the center is determined from the extents
        /// </summary>
        /// <author>Paul Meems</author>
        /// <date>23 Oct. 2009</date>
        /// <param name="testShape">The polygon shape to create the centroid of</param>
        /// <returns>The most likely centroid</returns>
        private MapWinGIS.IPoint GetTestCentroid(MapWinGIS.IShape testShape)
        {
            MapWinGIS.IPoint testPoint = null;
            if (testShape.IsValid)
            {
                testPoint = testShape.Centroid;                
            }
            else
            {
                // Try to make it valid by buffering:
                MapWinGIS.IShape bufferedShape = testShape.Buffer(0.0, 16);
                if (bufferedShape != null && bufferedShape.IsValid)
                {
                    testPoint = bufferedShape.Centroid;
                }
                else
                {
                    // Still not valid, use the extent to get the center:
                    MapWinGIS.Extents ext = testShape.Extents;
                    double x = ((ext.xMax + ext.xMin) / 2) + ext.xMin;
                    double y = ((ext.yMax + ext.yMin) / 2) + ext.xMin;
                    testPoint = new MapWinGIS.Point();
                    testPoint.x = x;
                    testPoint.y = y;
                }
            }
            return testPoint;
        }

        // Back-up
        private void FindXYValues_Old(MapWinGIS.Shapefile shpFile, int shapeIndex, MapWinGIS.Shape shape, int partIndex, double[] shapePoints, ref double x, ref double y)
        {
            try
            {
                if (shape.ShapeType == MapWinGIS.ShpfileType.SHP_POLYGON | shape.ShapeType == MapWinGIS.ShpfileType.SHP_POLYGONM | shape.ShapeType == MapWinGIS.ShpfileType.SHP_POLYGONZ) // modified: Z and M types added by Cornelius Mende
                {
                    double area = 0;
                    int count = shape.numPoints;
                    int startPartIndex;
                    int stopPartIndex;

                    //Decides which part to calculate the centroid of
                    if (partIndex == -1 || shape.NumParts <= 1)
                    {
                        startPartIndex = 0;
                        stopPartIndex = count;
                    }
                    else
                    {
                        startPartIndex = shape.get_Part(partIndex);
                        if (partIndex < shape.NumParts - 1)
                            stopPartIndex = shape.get_Part(partIndex + 1);
                        else
                            stopPartIndex = count;
                    }

                    //If it has less then 4 points then we can quickly approximate calculate its centroid quickly from its center point
                    if (stopPartIndex - startPartIndex <= 4)
                    {
                        double minX = double.MaxValue;
                        double minY = double.MaxValue;
                        double maxX = double.MinValue;
                        double maxY = double.MinValue;

                        for (int i = startPartIndex; i < stopPartIndex; i++)
                        {
                            if (shapePoints[2 * i] < minX)
                                minX = shapePoints[2 * i];
                            if (shapePoints[(2 * i) + 1] < minY)
                                minY = shapePoints[(2 * i) + 1];
                            if (shapePoints[2 * i] > maxX)
                                maxX = shapePoints[2 * i];
                            if (shapePoints[(2 * i) + 1] > maxY)
                                maxY = shapePoints[(2 * i) + 1];
                        }
                        x = maxX - minX;
                        y = maxY - minY;
                        return;
                    }
                    else
                    {
                        // calculate the area of the poly
                        for (int i = startPartIndex; i < stopPartIndex; i++)
                        {
                            if (i != stopPartIndex - 1)
                                area += (shapePoints[2 * i] * shapePoints[(2 * i) + 3] - shapePoints[(2 * i) + 2] * shapePoints[(2 * i) + 1]);
                            else
                                area += (shapePoints[2 * i] * shapePoints[(2 * startPartIndex) + 1] - shapePoints[2 * startPartIndex] * shapePoints[(2 * i) + 1]);

                        }
                        area *= .5;

                        //If the area is greater than 0 then its a hole (we got the math inverted some how but it doesn't matter)
                        if (area > 0)
                        {
                            x = double.NaN;
                            y = double.NaN;
                            return;
                        }

                        // calculate the centroid
                        x = 0;
                        y = 0;
                        for (int i = startPartIndex; i < stopPartIndex; i++)
                        {
                            if (i != stopPartIndex - 1)
                            {
                                x += (shapePoints[2 * i] + shapePoints[(2 * i) + 2]) * (shapePoints[2 * i] * shapePoints[(2 * i) + 3] - shapePoints[(2 * i) + 2] * shapePoints[(2 * i) + 1]);
                                y += (shapePoints[(2 * i) + 1] + shapePoints[(2 * i) + 3]) * (shapePoints[2 * i] * shapePoints[(2 * i) + 3] - shapePoints[(2 * i) + 2] * shapePoints[(2 * i) + 1]);
                            }
                            else
                            {
                                x += (shapePoints[2 * i] + shapePoints[2 * startPartIndex]) * (shapePoints[2 * i] * shapePoints[(2 * startPartIndex) + 1] - shapePoints[2 * startPartIndex] * shapePoints[(2 * i) + 1]);
                                y += (shapePoints[(2 * i) + 1] + shapePoints[(2 * startPartIndex) + 1]) * (shapePoints[2 * i] * shapePoints[(2 * startPartIndex) + 1] - shapePoints[2 * startPartIndex] * shapePoints[(2 * i) + 1]);
                            }
                        }
                        x *= 1 / (6 * area);
                        y *= 1 / (6 * area);

                        MapWinGIS.IPoint mytestPoint = shpFile.get_Shape(shapeIndex).Centroid;

                        // test to make sure the centroid is the poly
                        if (shpFile.PointInShape(shapeIndex, x, y) == false)
                        {
                            double minFound = double.MaxValue;
                            int ptIndex = 0;
                            for (int i = startPartIndex; i < stopPartIndex; i++)
                            {
                                double tempDist = Math.Sqrt(((shapePoints[2 * i] - x) * (shapePoints[2 * i] - x)) + ((shapePoints[(2 * i) + 1] - y) * (shapePoints[(2 * i) + 1] - y)));
                                if (tempDist < minFound)
                                {
                                    ptIndex = i;
                                    minFound = tempDist;
                                }
                            }
                            x = shapePoints[2 * ptIndex];
                            y = shapePoints[(2 * ptIndex) + 1];
                        }

                        if (x == -1 || y == -1 || x == 0 || y == 0)
                        {
                            // Detection failed... simple center?
                            x = ((shape.Extents.xMax - shape.Extents.xMin) / 2) + shape.Extents.xMin;
                            y = ((shape.Extents.yMax - shape.Extents.yMin) / 2) + shape.Extents.yMin;
                        }

                        return;
                    }
                }
                else if (shape.ShapeType == MapWinGIS.ShpfileType.SHP_POINT | shape.ShapeType == MapWinGIS.ShpfileType.SHP_POINTM | shape.ShapeType == MapWinGIS.ShpfileType.SHP_POINTZ) // modified: Z and M types added by Cornelius Mende
                {
                    x = shape.Extents.xMax;
                    y = shape.Extents.yMax;
                    return;
                }
                else if (shape.ShapeType == MapWinGIS.ShpfileType.SHP_MULTIPOINT | shape.ShapeType == MapWinGIS.ShpfileType.SHP_MULTIPOINTM | shape.ShapeType == MapWinGIS.ShpfileType.SHP_MULTIPOINTZ) // modified: Z and M types added by Cornelius Mende
                {
                    x = (shape.Extents.xMin + shape.Extents.xMax) / 2;
                    y = (shape.Extents.yMin + shape.Extents.yMax) / 2;
                    return;
                }
                else if (shape.ShapeType == MapWinGIS.ShpfileType.SHP_POLYLINE || shape.ShapeType == MapWinGIS.ShpfileType.SHP_POLYLINEM || shape.ShapeType == MapWinGIS.ShpfileType.SHP_POLYLINEZ)
                {
                    int count = shape.numPoints, index1 = 0, index2 = 0;
                    double max_length = -1.0;
                    for (int i = 1; i < count; i++)
                    {
                        double length = GetLineLength(shape.get_Point(i - 1), shape.get_Point(i));
                        if (length > max_length)
                        {
                            index1 = i - 1;
                            index2 = i;
                            max_length = length;
                        }
                    }

                    double opposite = shape.get_Point(index2).y - shape.get_Point(index1).y;
                    double adjacent = shape.get_Point(index2).x - shape.get_Point(index1).x;
                    // 					double opposite = shape.get_Point(count/2).y-shape.get_Point(0).y;
                    // 					double adjacent = shape.get_Point(count/2).x-shape.get_Point(0).x;

                    // rotation = Math.Atan(opposite/adjacent) * (180 / Math.PI);
                    double temp_x1 = shape.get_Point(index1).x, temp_x2 = shape.get_Point(index2).x, temp_y1 = shape.get_Point(index1).y, temp_y2 = shape.get_Point(index2).y;
                    x = temp_x1 + ((temp_x2 - temp_x1) / 2);
                    y = temp_y1 + ((temp_y2 - temp_y1) / 2);
                    // 					x = shape.get_Point(count/2).x;
                    // 					y = shape.get_Point(count/2).y;
                    return;
                }
            }
            catch (System.Exception ex)
            {
                ShowErrorBox("FindXYValues()", ex.Message);
            }
        }

        private double GetLineLength(MapWinGIS.Point p1, MapWinGIS.Point p2)
        {
            double x_length = Math.Abs(p1.x - p2.x);
            double y_length = Math.Abs(p1.y - p2.y);
            return Math.Sqrt(x_length * x_length + y_length * y_length);
        }

        // Not used:
        private void FindXY(MapWinGIS.Shapefile shpFile, int shapeIndex, ref double cX, ref double cY)
        {
            MapWinGIS.Shape shape = shpFile.get_Shape(shapeIndex);

            double xFirst = -1, xLast = -1;
            double tolx1 = 0, toly1 = 0, tolx2 = 0, toly2 = 0, stepSize = 0;

            // caluculate step size
            m_parent.m_MapWin.View.PixelToProj(0, 0, ref tolx1, ref toly1);
            m_parent.m_MapWin.View.PixelToProj(1, 0, ref tolx2, ref toly2);
            stepSize = System.Math.Pow((tolx1 - tolx2), 2) + System.Math.Pow((toly1 - toly2), 2);

            double xMin = shape.Extents.xMin;
            double xMax = shape.Extents.xMax;

            for (double i = xMin; i <= xMax; i += stepSize)
            {
                if (shpFile.PointInShape(shapeIndex, i, cY))
                {
                    // if the x value is in the boundary and it's the first time found then save
                    if (xFirst == -1)
                    {
                        xFirst = i;
                    }
                    // if the end x value is in the shape then set that to the last point
                    else if (i + stepSize >= xMax)
                    {
                        xLast = i;
                    }
                }
                else
                {
                    // if the first x value was already found then save the last x value
                    if (xFirst != -1)
                    {
                        xLast = i;

                        // exit from the for loop
                        break;
                    }
                }

            }

            // return the x value that lies between the two poly boundary
            cX = xFirst + (xLast - xFirst) / 2;

        }

        private void Initialize()
        {
            try
            {
                OpenLabelingInfo();
                LoadShapeFileLayers();

                // clear all fields
                cbField.Text = "";
                cbField2.Text = "";
                // Paul Meems, 22 Oct. 2009
                // Added:
                cmbRotateField.Text = "";
                // End
                txtFont.Text = "";
                txtColor.Text = "";
                cbAlign.Text = "";
                txtShadowColor.Text = "";
                cbEnableMinExtents.Checked = false;
                btnSaveMinExtents.Enabled = false;
                btnApply.Enabled = false;
            }
            catch (System.Exception ex)
            {
                ShowErrorBox("Initialize() 1", ex.Message);
                MessageBox.Show(ex.ToString());
            }
            try
            {
                // not a shapefile
                if (!m_Layers.Contains(currentHandle))
                    return;

                Label label;

                label = (Label)m_Layers[currentHandle];

                // populate the fields
                if (LoadShapeFileFields(label.handle) == false)
                {
                    return;
                }

                txtFont.Text = label.font.Name + ", " + label.font.Size.ToString();
                txtColor.Text = label.color.ToString();
                txtShadowColor.Text = label.shadowColor.ToString();

                if (label.field < cbField.Items.Count)
                    cbField.SelectedIndex = label.field;
                if (label.field2 < cbField2.Items.Count)
                    cbField2.SelectedIndex = label.field2;
                for (int i = 0; i < cmbRotateField.Items.Count; i++)
                    if (label.RotationField == cmbRotateField.Items[i].ToString())
                    {
                        cmbRotateField.SelectedIndex = i;
                        break;
                    }
                chbRotate.Checked = (label.RotationField != "");

                if (label.alignment == MapWinGIS.tkHJustification.hjCenter)
                    cbAlign.Text = "Center";
                else if (label.alignment == MapWinGIS.tkHJustification.hjLeft)
                    cbAlign.Text = "Left";
                else if (label.alignment == MapWinGIS.tkHJustification.hjRight)
                    cbAlign.Text = "Right";

                LabelsShadowCheckBox.Checked = label.UseShadows;
                LabelsScaleCheckBox.Checked = label.Scaled;
                UseLabelCollisionCheckBox.Checked = label.UseLabelCollision;
                if(label.extents != null)
                    label.scale = ExtentsToScale(label.extents);
                txtScale.Text = "1:" + Math.Round(label.scale).ToString();
                RemoveDuplicatesCheckBox.Checked = label.RemoveDuplicates;

                btnColor.BackColor = label.color;
                btnShadowColor.BackColor = label.shadowColor;

                colorDialog.Color = label.color;
                shadowColorDialog.Color = label.shadowColor;

                cbEnableMinExtents.Checked = label.UseMinExtents;
                btnSaveMinExtents.Enabled = cbEnableMinExtents.Checked;

                txtFirstLineAppend.Text = label.AppendLine1;
                txtSecondLineAppend.Text = label.AppendLine2;
                txtFirstLinePrepend.Text = label.PrependLine1;
                txtSecondLinePrepend.Text = label.PrependLine2;
                // Make sure there's no changes made upon loading.
                // This makes the Apply button disabled and bypass
                // the question on the Close() method.
                // Added 04/15/2008 Earljon Hidalgo
                SetModified(false);
            }
            catch (System.Exception ex)
            {
                ShowErrorBox("Initialize() 2", ex.Message);
                MessageBox.Show(ex.ToString());
            }
        }

        private bool LoadShapeFileFields(int handle)
        {
            try
            {
                // clear all the fields
                cbField.Items.Clear();
                cbField.Items.Add("None");
                cbField2.Items.Clear();
                cbField2.Items.Add("None");
                cmbRotateField.Items.Clear();
                cmbRotateField.Items.Add("None");

                // check to see if that layer exiss
                if (m_parent.m_MapWin.Layers.IsValidHandle(handle) == false)
                    return false;

                Shapefile shpfile = (Shapefile)m_parent.m_MapWin.Layers[handle].GetObject();

                if (shpfile == null)
                    return false;

                int numFields = shpfile.NumFields;
                for (int i = 0; i < numFields; i++)
                {
                    cbField.Items.Add(shpfile.get_Field(i).Name);
                    cbField2.Items.Add(shpfile.get_Field(i).Name);
                    cmbRotateField.Items.Add(shpfile.get_Field(i).Name);
                }

                return true;

            }
            catch (System.Exception ex)
            {
                ShowErrorBox("LoadShapeFileFields()", ex.Message);
            }
            return false;
        }

        private void ShowErrorBox(string functionName, string errorMsg)
        {
            MapWinUtility.Logger.Message("Error in " + functionName + ", Message: " + errorMsg, "Label Editor", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error, DialogResult.OK);
        }

        private int RGB(int r, int g, int b)
        {
            if (b > 255 || b < 0)
                return -1;
            if (r > 255 || r < 0)
                return -1;
            if (g > 255 || g < 0)
                return -1;

            int retval = b;

            retval = retval << 8;

            retval += g;

            retval = retval << 8;

            retval += r;

            return retval;
        }

        private void cbEnableMinExtents_CheckedChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (currentHandle != -1 && this.PopulatingFields == false)
                {
                    // Enable the use of of min extents on the lables

                    Label label = (Label)m_Layers[currentHandle];

                    // if null then set extents to the current view extents
                    if (label.extents == null)
                    {
                        label.extents = this.m_parent.m_MapWin.View.Extents;
                        label.scale = ExtentsToScale(label.extents);
                    }

                    if (cbEnableMinExtents.Checked)
                    {
                        btnSaveMinExtents.Enabled = true;
                        label.UseMinExtents = true;
                    }
                    else
                    {
                        btnSaveMinExtents.Enabled = false;
                        label.UseMinExtents = false;
                    }

                    label.LabelExtentsChanged = true;

                    // reset the label
                    m_Layers[currentHandle] = label;

                    // set modified
                    if (!PopulatingFields)
                        this.SetModified(true);
                }
            }
            catch (System.Exception ex)
            {
                ShowErrorBox("cbEnableMinExtents_CheckedChanged()", ex.Message);
            }
        }

        private void btnSaveMinExtents_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (currentHandle != -1 && this.PopulatingFields == false)
                {
                    txtScale.Text = "1:" + Math.Round(ExtentsToScale(this.m_parent.m_MapWin.View.Extents)).ToString();
                    Label label = (Label)m_Layers[currentHandle];
                    label.scale = double.Parse(txtScale.Text.Replace("1:", ""));
                    label.extents = ScaleToExtents(label.scale, m_parent.m_MapWin.View.Extents);
                    label.LabelExtentsChanged = true;
                    label.Modified = true;

                    // reset the label
                    m_Layers[currentHandle] = label;
                    
                    // set modified 
                    this.SetModified(true);
                }
            }
            catch (System.Exception ex)
            {
                ShowErrorBox("btnSaveMinExtents_Click()", ex.Message);
            }
        }
        
        private int RGB(System.Drawing.Color c)
        {
            return RGB(c.R, c.G, c.B);
        }

        public void CheckZoomLevelProp()
        {
            Label lb;
            System.Collections.DictionaryEntry dict;
            System.Collections.IEnumerator enumerator = m_Layers.GetEnumerator();

            while (enumerator.MoveNext())
            {
                dict = (System.Collections.DictionaryEntry)enumerator.Current;
                lb = (Label)dict.Value;

                if (lb.UseMinExtents == false) return;

                Point p1 = new Point(lb.extents.xMin, lb.extents.yMin);
                Point p2 = new Point(lb.extents.xMax, lb.extents.yMax);

                double dist1 = Math.Sqrt(Math.Pow((p1.y - p2.y), 2) + Math.Pow((p1.x - p2.x), 2));

                Point p3 = new Point(this.m_parent.m_MapWin.View.Extents.xMin, this.m_parent.m_MapWin.View.Extents.yMin);
                Point p4 = new Point(this.m_parent.m_MapWin.View.Extents.xMax, this.m_parent.m_MapWin.View.Extents.yMax);

                double dist2 = Math.Sqrt(Math.Pow((p3.y - p4.y), 2.0) + Math.Pow((p3.x - p4.x), 2.0));

                if (dist1 >= dist2)
                {
                    this.m_parent.m_MapWin.Layers[(int)dict.Key].LabelsVisible = true;
                }
                else
                {
                    this.m_parent.m_MapWin.Layers[(int)dict.Key].LabelsVisible = false;
                }
            }
        }

        private void SetModified(bool modified)
        {
            this.m_Modifed = modified;
            this.btnApply.Enabled = modified;
        }

        private void LabelsShadowCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            UpdateLabelsShadow();
            // set modified
            if (!PopulatingFields)
                this.SetModified(true);
        }

        private void UpdateLabelsShadow()
        {
            if (currentHandle != -1)
            {

                Label label = (Label)m_Layers[currentHandle];

                if (LabelsShadowCheckBox.Checked == true)
                {
                    label.UseShadows = true;
                    // this.m_parent.m_MapWin.Layers[handle].LabelsShadow = true;
                }
                else
                {
                    label.UseShadows = false;
                    // this.m_parent.m_MapWin.Layers[handle].LabelsShadow = false;
                }
                label.Modified = true;
                m_Layers[currentHandle] = label;
            }
        }

        private void LabelScaleCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            UpdateLabelsScale();
            // set modified
            if (!PopulatingFields)
                this.SetModified(true);
        }

        private void UpdateLabelsScale()
        {
            if (currentHandle != -1)
            {

                Label label = (Label)m_Layers[currentHandle];

                if (LabelsScaleCheckBox.Checked == true)
                {
                    // Set view width right away
                    // this.m_parent.m_MapWin.Layers[handle].StandardViewWidth = 8*(this.m_parent.m_MapWin.View.Extents.xMax - this.m_parent.m_MapWin.View.Extents.xMin);
                    label.StandardViewWidth = 8 * (this.m_parent.m_MapWin.View.Extents.xMax - this.m_parent.m_MapWin.View.Extents.xMin);
                    label.Scaled = true;
                    // this.m_parent.m_MapWin.Layers[handle].LabelsScale = true;
                }
                else
                {
                    label.Scaled = false;
                    // this.m_parent.m_MapWin.Layers[handle].LabelsScale = false;
                }
                label.Modified = true;
                m_Layers[currentHandle] = label;
            }
        }

        private void btnShadowColor_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (currentHandle != -1 && this.PopulatingFields == false)
                {
                    System.Windows.Forms.DialogResult result;
                    result = shadowColorDialog.ShowDialog();

                    if (result == System.Windows.Forms.DialogResult.Cancel)
                        return;

                    // save changes

                    Label label = (Label)m_Layers[currentHandle];

                    txtShadowColor.Text = shadowColorDialog.Color.ToString();
                    btnShadowColor.BackColor = shadowColorDialog.Color;

                    label.shadowColor = shadowColorDialog.Color;
                    label.Modified = true;

                    // reset the label
                    m_Layers[currentHandle] = label;

                    // set modified
                    if (!PopulatingFields)
                        this.SetModified(true);
                }
            }
            catch (System.Exception ex)
            {
                ShowErrorBox("btnColor_Click()", ex.Message);
            }
        }

        private void btnSetScaleSize_Click(object sender, System.EventArgs e)
        {
            if (currentHandle != -1)
            {

                Label label = (Label)m_Layers[currentHandle];
                // When the current view width is multiplied by 8, you get the correct standardViewWidth to display the scaled labels at the correct font size for the current view.
                // this.m_parent.m_MapWin.Layers[handle].StandardViewWidth = 8*(this.m_parent.m_MapWin.View.Extents.xMax - this.m_parent.m_MapWin.View.Extents.xMin);
                label.StandardViewWidth = 8 * (this.m_parent.m_MapWin.View.Extents.xMax - this.m_parent.m_MapWin.View.Extents.xMin);
                label.Modified = true;
                m_Layers[currentHandle] = label;

                // set modified
                if (!PopulatingFields)
                    this.SetModified(true);
            }
        }

        private void UseLabelCollisionCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            UpdateLabelsCollision();
            // set modified
            if (!PopulatingFields)
                this.SetModified(true);
        }

        private void UpdateLabelsCollision()
        {
            if (currentHandle == -1) return;
            Label label = (Label)m_Layers[currentHandle];
            if (UseLabelCollisionCheckBox.Checked == true)
            {
                label.UseLabelCollision = true;
                this.m_parent.m_MapWin.Layers[currentHandle].UseLabelCollision = true;
            }
            else
            {
                label.UseLabelCollision = false;
                this.m_parent.m_MapWin.Layers[currentHandle].UseLabelCollision = false;
            }
            label.Modified = true;
            m_Layers[currentHandle] = label;
        }

        private void RemoveDuplicatesCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            UpdateRemoveDuplicates();
            // set modified
            if (!PopulatingFields)
                this.SetModified(true);
        }

        private void UpdateRemoveDuplicates()
        {
            if (currentHandle == -1) return;

            Label label = (Label)m_Layers[currentHandle];
            if (RemoveDuplicatesCheckBox.Checked == true)
            {
                label.RemoveDuplicates = true;
            }
            else
            {
                label.RemoveDuplicates = false;

            }
            label.Modified = true;
            label.updateHeaderOnly = false;
            m_Layers[currentHandle] = label;
        }

        public void btnRelabel_Click(object sender, EventArgs e)
        {
            if (!ValidateRotationField()) return;

            if (currentHandle == -1) return;
            Label label = (Label)m_Layers[currentHandle];

            if (label.field == 0 && cbField.SelectedIndex != 0)
                label.CalculatePos = true;

            if (label.field2 == 0 && cbField2.SelectedIndex != 0)
                label.CalculatePos = true;

            label.field = cbField.SelectedIndex;
            label.field2 = cbField2.SelectedIndex;
            label.Modified = true;
            label.updateHeaderOnly = false;
            label.LabelAllParts = LabelAllPartsCheckBox.Checked;

            // reset the label
            m_Layers[currentHandle] = label;

            // set modified
            if (!PopulatingFields)
                this.SetModified(true);

            ApplyChanges();
            SaveAllLabelingInfo();
        }

        private void txtAppendPrepend_TextChanged(object sender, EventArgs e)
        {
            m_Modifed = true;
        }

        private void cmbRotateField_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!PopulatingFields && ValidateRotationField())
            {
                if (currentHandle != -1)
                {
                    Label label = (Label)m_Layers[currentHandle];
                    label.RotationField = cmbRotateField.Text;
                    m_Layers[currentHandle] = label;
                }
                this.SetModified(true);
            }
        }

        private bool ValidateRotationField()
        {
            if (currentHandle == -1)
                return false;

            if (cmbRotateField.Text == "None" || cmbRotateField.Text == "" || !chbRotate.Checked)
            {
                chbRotate.Checked = false;
                if (currentHandle != -1 && this.PopulatingFields == false)
                {
                    Label label = (Label)m_Layers[currentHandle];
                    label.RotationField = "";
                }
                return true; // OK
            }

            Shapefile shpfile = (Shapefile)m_parent.m_MapWin.Layers[currentHandle].GetObject();

            if (shpfile == null)
                return false;

            double unused = 0;
            for (int i = 0; i < shpfile.NumFields; i++)
            {
                if (shpfile.get_Field(i).Name.ToLower().Trim() == cmbRotateField.Text.ToLower().Trim())
                {
                    for (int j = 0; j < shpfile.NumShapes; j++)
                    {
                        if (!double.TryParse(shpfile.get_CellValue(i, j).ToString(), out unused))
                        {
                            MessageBox.Show("One or more of the values in the selected rotation field are not numeric. Please select a different field, or uncheck the option to rotate labels by field." + Environment.NewLine + Environment.NewLine + "This option is used to rotate the label by the number of degrees specified in a field.", "Non-numeric Value Found");
                            return false;
                        }
                    }
                    break;
                }
            }

            if (this.PopulatingFields == false)
            {
                Label label = (Label)m_Layers[currentHandle];
                label.RotationField = cmbRotateField.Text;
            }

            return true;
        }

        private void chbRotate_CheckedChanged(object sender, EventArgs e)
        {
            if (!PopulatingFields && currentHandle != -1)
            {
                Label label = (Label)m_Layers[currentHandle];
                label.RotationField = cmbRotateField.Text;
                m_Layers[currentHandle] = label;
                this.SetModified(true);
            }
        }

        private void txtScale_TextChanged(object sender, EventArgs e)
        {
            this.SetModified(true);
        }

        private void txtScale_Leave(object sender, EventArgs e)
        {
            double scaleValue;
            if (!double.TryParse(txtScale.Text.Replace("1:", ""), out scaleValue))
            {
                MessageBox.Show("Please enter the scale in the form of 1:100000, or click Use Current to use the current zoom level.",  "Invalid Scale", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            Label label = (Label)m_Layers[currentHandle];
            label.scale = double.Parse(txtScale.Text.Replace("1:", ""));
            label.extents = ScaleToExtents(label.scale, m_parent.m_MapWin.View.Extents);
            label.LabelExtentsChanged = true;
            label.Modified = true;

            // reset the label
            m_Layers[currentHandle] = label;
        }

        private void frmLabeler_Load(object sender, EventArgs e)
        {

        }

        private double ExtentsToScale(MapWinGIS.Extents ext)
        {
            AxMapWinGIS.AxMap MapMain = (AxMapWinGIS.AxMap)m_MapWin.GetOCX;
            return MapWinGeoProc.ScaleTools.CalcScale(ext, m_MapWin.Project.MapUnits, MapMain.Width, MapMain.Height);
        }

        private MapWinGIS.Extents ScaleToExtents(double Scale, MapWinGIS.Extents ext)
        {
            AxMapWinGIS.AxMap MapMain = (AxMapWinGIS.AxMap)m_MapWin.GetOCX;
            MapWinGIS.Point pt = new MapWinGIS.Point();
            pt.x = (ext.xMin + ext.xMax) / 2;
            pt.y = (ext.yMin + ext.yMax) / 2;
            return MapWinGeoProc.ScaleTools.ExtentFromScale(Convert.ToInt32(Scale), pt, m_MapWin.Project.MapUnits, MapMain.Width, MapMain.Height);
        }
    }
 
    public struct Label
    {
        public int handle;
        public MapWinGIS.tkHJustification alignment;
        public Font font;
        public int field;
        public int field2;
        public bool UseMinExtents;
        public MapWinGIS.Extents extents;
        public double scale;
        public System.Drawing.Color color;
        public System.Drawing.Color shadowColor;
        public System.Collections.ArrayList points;
        public bool CalculatePos;
        public bool Modified;
        public bool LabelExtentsChanged;
        public bool Scaled;
        public bool UseShadows;
        public int Offset;
        public double StandardViewWidth;
        public bool UseLabelCollision;
        public bool RemoveDuplicates;
        public System.Collections.ArrayList labelShape;
        public String xml_LblFile;
        public bool updateHeaderOnly;
        public string AppendLine1;
        public string AppendLine2;
        public string PrependLine1;
        public string PrependLine2;
        public string RotationField;
        public bool LabelAllParts;
    }

    public struct Point
    {
        public Point(double xVal, double yVal) { x = xVal; y = yVal; rotation = 0; }
        public double x;
        public double y;
        public double rotation;
    }

    public class LabelPoints
    {
        public LabelPoints() { labels = new ArrayList(); endPoints = new ArrayList(); }
        public LabelPoints(Point l, Point end, Point end2) { labels = new ArrayList(); endPoints = new ArrayList(); labels.Add(l); endPoints.Add(end); endPoints.Add(end2); }
        public ArrayList labels;
        public ArrayList endPoints;
    }

    public class Cluster
    {
        public Cluster() { shapeIndex = new ArrayList(); endPoints = new ArrayList(); }
        public ArrayList shapeIndex;
        public ArrayList endPoints;
    }
}
