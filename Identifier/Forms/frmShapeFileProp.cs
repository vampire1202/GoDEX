//********************************************************************************************************
//The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
//you may not use this file except in compliance with the License. You may obtain a copy of the License at 
//http://www.mozilla.org/MPL/ 
//Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
//ANY KIND, either express or implied. See the License for the specificlanguage governing rights and 
//limitations under the License. 
//
//The Original Code is MapWindow Identifier Plug-in. 
//
//The Initial Developer of this version of the Original Code is Daniel P. Ames using portions created by 
//Utah State University and the Idaho National Engineering and Environmental Lab that were released as 
//public domain in March 2004.  
//
//Contributor(s): (Open source contributors should list themselves and their modifications here). 
//2/14/2008 - Jiri Kadlec - changed "Unitialize" method so that identified shapes remain selected after
//                          the form is closed.
//********************************************************************************************************

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using MapWinGIS;

namespace mwIdentifier.Forms
{
    public class frmShapeFileProp : System.Windows.Forms.Form
    {

        private mwIdentPlugin m_parent;
        private System.Drawing.Color RED = System.Drawing.Color.Red;
        private System.Drawing.Color YELLOW = System.Drawing.Color.Yellow;
        private string m_LayerName;
        private int[] m_shpIndex;
        private frmEdit frmEdit = new frmEdit();
        private string m_FieldName = "";
        private int m_SelShape;
        private Shapefile m_ShapeFile = null;
        private System.Windows.Forms.ContextMenu PopupMenu;
        private System.Windows.Forms.MenuItem EditMenu;
        private bool m_Editable = false;
        public Panel panel1;
        private ComboBox cbFieldName;
        private ListView lv;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private ComboBox cb;
        private bool m_HavePanel = false;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public frmShapeFileProp(mwIdentPlugin p)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //get a copy of the parent
            m_parent = p;
            m_SelShape = -1;

            //set the parent form
            System.IntPtr tempPtr = (System.IntPtr)m_parent.m_ParentHandle;
            Form mapFrm = (Form)System.Windows.Forms.Control.FromHandle(tempPtr);
            mapFrm.AddOwnedForm(this);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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

        public bool Editable
        {
            get
            {
                return m_Editable;
            }
            set
            {
                m_Editable = value;
            }
        }
        public void PopulateForm(bool ShowAfterward, MapWinGIS.Shapefile shapeFile, string layerName, bool calledBySelf)
        {
            if (!calledBySelf) m_HavePanel = !ShowAfterward;

            try
            {
                int numFields;
                string fieldName;
                m_ShapeFile = shapeFile;
                m_shpIndex = null;

                m_LayerName = layerName;
                SetTitle();

                //new selected shapes
                m_SelShape = -1;

                //clear the selected box
                m_parent.m_MapWin.View.Draw.ClearDrawing(m_parent.m_hDraw);

                //clear the list view items
                lv.Items.Clear();
                cb.Items.Clear();
                cb.Text = "";

                //clear the combo Box field Name
                cbFieldName.Items.Clear();
                cbFieldName.Items.Add("Shape Index");
                cb.Text = "";

                numFields = shapeFile.NumFields;
                for (int i = 0; i < numFields; i++)
                {
                    fieldName = shapeFile.get_Field(i).Name;
                    lv.Items.Add(fieldName);

                    //add all the field name to the combo box
                    cbFieldName.Items.Add(fieldName);
                }

                //select the first field
                if (cbFieldName.Items.IndexOf(m_FieldName) == -1)
                    cbFieldName.SelectedIndex = 0;
                else
                    cbFieldName.SelectedItem = m_FieldName;

                if (ShowAfterward) this.Show();
            }
            catch (System.Exception ex)
            {
                ShowErrorBox("PopulateForm()", ex.Message);
            }
        }

        public void PopulateForm(bool ShowAfterward, MapWinGIS.Shapefile shapeFile, int[] shpIndex, string layerName, bool calledBySelf)
        {
            if (!calledBySelf) m_HavePanel = !ShowAfterward;

            try
            {
                System.Windows.Forms.ListViewItem item;
                int numFields;
                m_shpIndex = shpIndex;
                m_LayerName = layerName;
                m_ShapeFile = shapeFile;
                SetTitle();

                //new selected shapes
                m_SelShape = -1;

                //clear the list view items
                lv.Items.Clear();

                //clear the combo Box field Name
                cb.Items.Clear();
                cb.Text = "";
                cbFieldName.Items.Clear();
                cbFieldName.Items.Add("Shape Index");

                //clear the selected box
                m_parent.m_MapWin.View.Draw.ClearDrawing(m_parent.m_hDraw);

                numFields = shapeFile.NumFields;
                for (int i = 0; i < numFields; i++)
                {
                    var fieldName = shapeFile.get_Field(i).Name;
                    item = lv.Items.Add(fieldName);

                    // PM Fixing Bug #1864: Identifyer tool crashes after selecting a shape with empty fields
                    // item.SubItems.Add(shapeFile.get_CellValue(i, shpIndex[0]).ToString());
                    var cellValue = "NULL";
                    if (shapeFile.get_CellValue(i, shpIndex[0]) != null)
                    {
                        cellValue = shapeFile.get_CellValue(i, shpIndex[0]).ToString();
                    }

                    item.SubItems.Add(cellValue);

                    // Add all the field name to the combo box
                    cbFieldName.Items.Add(fieldName);
                }

                //color the current shape red
                if (m_SelShape != -1)
                {
                    m_parent.m_MapWin.View.SelectedShapes.AddByIndex(m_SelShape, YELLOW);
                }

                m_SelShape = shpIndex[0];

                m_parent.m_MapWin.View.SelectedShapes.AddByIndex(shpIndex[0], RED);

                //select the first field
                if (cbFieldName.Items.IndexOf(m_FieldName) == -1)
                    cbFieldName.SelectedIndex = 0;
                else
                    cbFieldName.SelectedItem = m_FieldName;

                if (ShowAfterward) this.Show();
            }
            catch (System.Exception ex)
            {
                ShowErrorBox("PopulateForm()", ex.Message);
            }
        }

        private void lv_SizeChanged(object sender, System.EventArgs e)
        {
            int width = lv.Size.Width;
            lv.Columns[0].Width = width / 2;
            lv.Columns[1].Width = width / 2;

            //calculate the postion of the combo boxes
            cbFieldName.Width = lv.Columns[0].Width;
            System.Drawing.Point p = new System.Drawing.Point(cbFieldName.Location.X + cbFieldName.Width + 5, cb.Location.Y);
            cb.Location = p;
            cb.Width = lv.Columns[1].Width - 5;

        }

        private void btnOk_Click(object sender, System.EventArgs e)
        {
            Unitialize();
            m_parent.Activated = false;
            m_parent.Deactivate();
        }

        public void Unitialize()
        {
            cb.Items.Clear();
            if (!m_HavePanel) this.Hide();

            // Jiri Kadlec 2/14/2008 selected shapes shouldn't be cleared when closing the form ( bug #766 )
            //if(m_SelShape != -1)
            //{
            //    m_parent.m_MapWin.View.ClearSelectedShapes();
            //    m_parent.m_MapWin.View.SelectedShapes.AddByIndex(m_SelShape,YELLOW);
            //}

            m_SelShape = -1;
            m_ShapeFile = null;
            m_shpIndex = null;
        }

        private void frmShapeFileProp_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Unitialize();
            m_parent.Activated = false;
            if (!m_HavePanel) this.Hide();
            m_parent.Deactivate();
        }

        private void cb_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            PopulateForm(!m_HavePanel, m_ShapeFile, m_shpIndex[cb.SelectedIndex], true);
        }

        private void cbFieldName_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            m_FieldName = cbFieldName.Text;

            try
            {
                //clear the combo box
                cb.Items.Clear();
                if (m_shpIndex != null)
                {
                    int numIndexs = m_shpIndex.GetUpperBound(0);

                    //load field name
                    if (cbFieldName.SelectedIndex != 0)
                    {
                        for (int i = 0; i <= numIndexs; i++)
                        {
                            cb.Items.Add(m_ShapeFile.get_CellValue(cbFieldName.SelectedIndex - 1, m_shpIndex[i]));
                        }

                        //if selected shape is -1 then there is no selected shapes so ignore
                        if (m_SelShape != -1)
                            cb.SelectedIndex = FindIndex(m_SelShape);
                    }
                    //load shapeIndexes
                    else
                    {
                        for (int i = 0; i <= numIndexs; i++)
                        {
                            cb.Items.Add(m_shpIndex[i]);
                        }

                        //if selected shape is -1 then there is no selected shapes so ignore
                        if (m_SelShape != -1)
                            cb.SelectedItem = m_SelShape;
                    }
                }
            }
            catch (System.Exception ex)
            {
                ShowErrorBox("cbFieldName_SelectedIndexChanged()", ex.Message);
            }
        }

        private int FindIndex(int SelShape)
        {
            //finds the index of the position of the selected shape
            int numIndexs = m_shpIndex.GetUpperBound(0);
            for (int i = 0; i <= numIndexs; i++)
            {
                if (m_shpIndex[i] == SelShape)
                    return i;
            }
            return 0;
        }

        private void PopulateForm(bool ShowAfterward, MapWinGIS.Shapefile shapeFile, int shpIndex, bool calledBySelf)
        {
            if (!calledBySelf) m_HavePanel = !ShowAfterward;

            try
            {
                System.Windows.Forms.ListViewItem item;
                int numFields;
                m_ShapeFile = shapeFile;

                //clear the list view items
                lv.Items.Clear();

                numFields = shapeFile.NumFields;
                for (int i = 0; i < numFields; i++)
                {
                    // PM Fixing Bug #1864: Identifyer tool crashes after selecting a shape with empty fields
                    // string s = shapeFile.get_CellValue(i, shpIndex).ToString();
                    // TODO: Why is this done again? We did this in line 188 as well
                    var cellValue = "NULL";
                    if (shapeFile.get_CellValue(i, shpIndex) != null)
                    {
                        cellValue = shapeFile.get_CellValue(i, shpIndex).ToString();
                    }

                    item = lv.Items.Add(shapeFile.get_Field(i).Name);
                    item.SubItems.Add(cellValue);

                    item.ForeColor = Color.Black;
                    if (cellValue.ToLower().StartsWith("http") || cellValue.ToLower().StartsWith("file://"))
                    {
                        item.ForeColor = Color.Blue;
                    }
                }

                //color the current shape red
                if (m_SelShape != -1)
                    m_parent.m_MapWin.View.SelectedShapes.AddByIndex(m_SelShape, YELLOW);
                m_SelShape = shpIndex;
                m_parent.m_MapWin.View.SelectedShapes.AddByIndex(shpIndex, RED);

                if (ShowAfterward) this.Show();
            }
            catch (System.Exception ex)
            {
                ShowErrorBox("PopulateForm()", ex.Message);
            }
        }


        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmShapeFileProp));
            this.PopupMenu = new System.Windows.Forms.ContextMenu();
            this.EditMenu = new System.Windows.Forms.MenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbFieldName = new System.Windows.Forms.ComboBox();
            this.lv = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.cb = new System.Windows.Forms.ComboBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // PopupMenu
            // 
            this.PopupMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.EditMenu});
            resources.ApplyResources(this.PopupMenu, "PopupMenu");
            // 
            // EditMenu
            // 
            resources.ApplyResources(this.EditMenu, "EditMenu");
            this.EditMenu.Index = 0;
            this.EditMenu.Click += new System.EventHandler(this.EditMenu_Click);
            // 
            // panel1
            // 
            this.panel1.AccessibleDescription = null;
            this.panel1.AccessibleName = null;
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BackgroundImage = null;
            this.panel1.Controls.Add(this.cbFieldName);
            this.panel1.Controls.Add(this.lv);
            this.panel1.Controls.Add(this.cb);
            this.panel1.Font = null;
            this.panel1.Name = "panel1";
            // 
            // cbFieldName
            // 
            this.cbFieldName.AccessibleDescription = null;
            this.cbFieldName.AccessibleName = null;
            resources.ApplyResources(this.cbFieldName, "cbFieldName");
            this.cbFieldName.BackgroundImage = null;
            this.cbFieldName.Font = null;
            this.cbFieldName.Name = "cbFieldName";
            this.cbFieldName.SelectedIndexChanged += new System.EventHandler(this.cbFieldName_SelectedIndexChanged);
            // 
            // lv
            // 
            this.lv.AccessibleDescription = null;
            this.lv.AccessibleName = null;
            resources.ApplyResources(this.lv, "lv");
            this.lv.BackgroundImage = null;
            this.lv.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lv.Font = null;
            this.lv.FullRowSelect = true;
            this.lv.GridLines = true;
            this.lv.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lv.MultiSelect = false;
            this.lv.Name = "lv";
            this.lv.UseCompatibleStateImageBehavior = false;
            this.lv.View = System.Windows.Forms.View.Details;
            this.lv.SelectedIndexChanged += new System.EventHandler(this.lv_SelectedIndexChanged);
            this.lv.SizeChanged += new System.EventHandler(this.lv_SizeChanged);
            this.lv.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lv_MouseDown);
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // columnHeader2
            // 
            resources.ApplyResources(this.columnHeader2, "columnHeader2");
            // 
            // cb
            // 
            this.cb.AccessibleDescription = null;
            this.cb.AccessibleName = null;
            resources.ApplyResources(this.cb, "cb");
            this.cb.BackgroundImage = null;
            this.cb.Font = null;
            this.cb.Name = "cb";
            this.cb.SelectedIndexChanged += new System.EventHandler(this.cb_SelectedIndexChanged);
            // 
            // frmShapeFileProp
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.BackgroundImage = null;
            this.Controls.Add(this.panel1);
            this.Font = null;
            this.Name = "frmShapeFileProp";
            this.Load += new System.EventHandler(this.frmShapeFileProp_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmShapeFileProp_Closing);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private void lv_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if (lv.SelectedIndices.Count > 0 && m_SelShape != -1 && m_Editable == true)
                {
                    System.Drawing.Point p = new System.Drawing.Point();
                    p.X = e.X;
                    p.Y = e.Y;
                    PopupMenu.Show(lv, p);
                }
            }
        }

        private void EditMenu_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (lv.SelectedIndices.Count > 0 && m_SelShape != -1)
                {
                    string fieldValue = frmEdit.ShowFrm(this, lv.SelectedItems[0].SubItems[1].Text);

                    //if the cancel button wasn't pressed then save the new value
                    if (fieldValue != null)
                    {
                        m_ShapeFile.StartEditingTable(null);

                        if (m_ShapeFile.EditCellValue(lv.SelectedIndices[0], m_SelShape, fieldValue) == false)
                            MapWinUtility.Logger.Msg("Error in EditMenu_Click(), Message: " + m_ShapeFile.get_ErrorMsg(m_ShapeFile.LastErrorCode));

                        m_ShapeFile.StopEditingTable(true, null);

                        //update the value in the list view
                        lv.SelectedItems[0].SubItems[1].Text = fieldValue;
                    }
                }
            }
            catch (System.Exception ex)
            {
                ShowErrorBox("EditMenu_Click()", ex.Message);
            }

        }

        private void DeleteShapeFile(string fileName)
        {
            string f1, f2, f3;

            f1 = System.IO.Path.ChangeExtension(fileName, ".shp");
            f2 = System.IO.Path.ChangeExtension(fileName, ".shx");
            f3 = System.IO.Path.ChangeExtension(fileName, ".dbf");

            if (System.IO.File.Exists(f1))
                System.IO.File.Delete(f1);
            if (System.IO.File.Exists(f2))
                System.IO.File.Delete(f2);
            if (System.IO.File.Exists(f3))
                System.IO.File.Delete(f3);
        }

        private void SetTitle()
        {
            this.Text = "Identifier - " + m_LayerName;
        }

        private void frmShapeFileProp_Load(object sender, System.EventArgs e)
        {
            int width = lv.Size.Width;
            lv.Columns[0].Width = width / 2;
            lv.Columns[1].Width = width / 2;
        }

        private void ShowErrorBox(string functionName, string errorMsg)
        {
            MapWinUtility.Logger.Message("Error in " + functionName + ", Message: " + errorMsg, "Identifier", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error, DialogResult.OK);
        }

        private void DockImage_Click(object sender, EventArgs e)
        {
            m_parent.ToggleDockedStatus(this, panel1);
        }

        private void lv_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lv.SelectedItems.Count > 0)
            {
                string s = lv.SelectedItems[0].SubItems[1].Text;
                if (s.ToLower().StartsWith("http") || s.ToLower().StartsWith("file://"))
                {
                    System.Diagnostics.Process.Start(s);
                }
            }
        }
    }
}
