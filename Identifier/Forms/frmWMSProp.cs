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
//********************************************************************************************************

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using MapWinGIS;

namespace mwIdentifier.Forms
{
	public class frmWMSProp : System.Windows.Forms.Form
    {
		#region Windows Form Designer generated code
        private System.Windows.Forms.Label lblLyrID;
        private System.Windows.Forms.Label lblLength;
        private System.Windows.Forms.Label lblArea;
        private ListView lv;
        private ColumnHeader columnHeader1;
        private TextBox txtbxError;
        private ColumnHeader columnHeader2;
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmWMSProp));
            this.PopupMenu = new System.Windows.Forms.ContextMenu();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblLength = new System.Windows.Forms.Label();
            this.lblArea = new System.Windows.Forms.Label();
            this.lblLyrID = new System.Windows.Forms.Label();
            this.lv = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.txtbxError = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // PopupMenu
            // 
            resources.ApplyResources(this.PopupMenu, "PopupMenu");
            // 
            // panel1
            // 
            this.panel1.AccessibleDescription = null;
            this.panel1.AccessibleName = null;
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BackgroundImage = null;
            this.panel1.Controls.Add(this.lblLength);
            this.panel1.Controls.Add(this.lblArea);
            this.panel1.Controls.Add(this.lblLyrID);
            this.panel1.Controls.Add(this.lv);
            this.panel1.Controls.Add(this.txtbxError);
            this.panel1.Font = null;
            this.panel1.Name = "panel1";
            // 
            // lblLength
            // 
            this.lblLength.AccessibleDescription = null;
            this.lblLength.AccessibleName = null;
            resources.ApplyResources(this.lblLength, "lblLength");
            this.lblLength.Name = "lblLength";
            // 
            // lblArea
            // 
            this.lblArea.AccessibleDescription = null;
            this.lblArea.AccessibleName = null;
            resources.ApplyResources(this.lblArea, "lblArea");
            this.lblArea.Name = "lblArea";
            // 
            // lblLyrID
            // 
            this.lblLyrID.AccessibleDescription = null;
            this.lblLyrID.AccessibleName = null;
            resources.ApplyResources(this.lblLyrID, "lblLyrID");
            this.lblLyrID.Name = "lblLyrID";
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
            this.lv.SizeChanged += new System.EventHandler(this.lv_SizeChanged);
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // columnHeader2
            // 
            resources.ApplyResources(this.columnHeader2, "columnHeader2");
            // 
            // txtbxError
            // 
            this.txtbxError.AccessibleDescription = null;
            this.txtbxError.AccessibleName = null;
            resources.ApplyResources(this.txtbxError, "txtbxError");
            this.txtbxError.BackgroundImage = null;
            this.txtbxError.ForeColor = System.Drawing.Color.Red;
            this.txtbxError.Name = "txtbxError";
            // 
            // frmWMSProp
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.BackgroundImage = null;
            this.Controls.Add(this.panel1);
            this.Font = null;
            this.Name = "frmWMSProp";
            this.Load += new System.EventHandler(this.frmWMSProp_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmWMSProp_Closing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		private mwIdentPlugin m_parent;
		private System.Drawing.Color RED = System.Drawing.Color.Red;
		private System.Drawing.Color YELLOW = System.Drawing.Color.Yellow; 
		private string m_LayerName;
		private frmEdit frmEdit = new frmEdit();
        private System.Windows.Forms.ContextMenu PopupMenu;
        private bool m_Editable = false;
        public Panel panel1;
        private bool m_HavePanel = false;


	
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmWMSProp(mwIdentPlugin p)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			
			//get a copy of the parent
			m_parent = p;

			//set the parent form
			System.IntPtr tempPtr = (System.IntPtr)m_parent.m_ParentHandle;
			Form mapFrm = (Form)System.Windows.Forms.Control.FromHandle(tempPtr);
			mapFrm.AddOwnedForm(this);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
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


        private void frmWMSProp_Load(object sender, System.EventArgs e)
        {
            int width = lv.Size.Width;
            lv.Columns[0].Width = width / 2;
            lv.Columns[1].Width = width / 2;

            lblLyrID.Text = "Layer ID: ";
            lblArea.Text = "Area: ";
            lblLength.Text = "Length: ";
            txtbxError.Visible = false;
            lv.Visible = true;
        }

        private void frmWMSProp_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            m_parent.Activated = false;
            if (!m_HavePanel) this.Hide();
            m_parent.Deactivate();
        }
	
		private void lv_SizeChanged(object sender, System.EventArgs e)
		{	
			int width = lv.Size.Width;
			lv.Columns[0].Width = width /2;
			lv.Columns[1].Width = width /2;
		}


        public void PopulateForm(bool ShowAfterward, string WMSInfo, string layerName, bool calledBySelf)
        {
            lblLyrID.Text = "Layer ID: ";
            lblArea.Text = "Area: ";
            lblLength.Text = "Length: ";
            txtbxError.Visible = false;
            lv.Visible = true;

            if (!calledBySelf) m_HavePanel = !ShowAfterward;

            try
            {
                m_LayerName = layerName;
                SetTitle();

                //clear the selected box
                m_parent.m_MapWin.View.Draw.ClearDrawing(m_parent.m_hDraw);

                //clear the list view items
                lv.Items.Clear();

                if (WMSInfo.StartsWith("Error"))
                {
                    lv.Visible = false; 
                    txtbxError.Visible = true;
                    if (WMSInfo == "Error: No feature found.")
                    {
                        txtbxError.ForeColor = System.Drawing.Color.Black;
                        txtbxError.Text = "No feature found."; 
                    }
                    else
                    {
                        txtbxError.ForeColor = System.Drawing.Color.Red;
                        txtbxError.Text = WMSInfo;
                    }
                }
                else
                {
                    if (!WMSInfo.Contains("|"))
                    {
                        ShowErrorBox("PopulateForm()", "Unknown WMS Info Format");
                    }
                    else
                    {
                        lv.Visible = true;
                        txtbxError.Visible = false;

                        string[] pairs = WMSInfo.Split('|');
                        for (int i = 0; i < pairs.Length; i++)
                        {
                            if (pairs[i] != "" && pairs[i].Contains("="))
                            {
                                string[] vals = pairs[i].Split('=');
                                if (vals[0] == "_SHAPE_")
                                {

                                }
                                else if (vals[0] == "_LAYERID_")
                                {
                                    lblLyrID.Text = "Layer ID: " + vals[1];
                                }
                                else if (vals[0] == "SHAPE.AREA")
                                {
                                    lblArea.Text = "Area: " + vals[1];
                                }
                                else if (vals[0] == "SHAPE.LEN")
                                {
                                    lblLength.Text = "Length: " + vals[1];
                                }
                                else
                                {
                                    System.Windows.Forms.ListViewItem item;
                                    item = lv.Items.Add(vals[0]);
                                    item.SubItems.Add(vals[1]);
                                }
                            }
                        }
                    }
                }

                if (ShowAfterward) this.Show();
            }
            catch (System.Exception ex)
            {
                ShowErrorBox("PopulateForm()", ex.Message);
            }
        }
        
        private void SetTitle()
		{
			this.Text = "Identifier - " + m_LayerName;
		}

		private void ShowErrorBox(string functionName,string errorMsg)
		{
			MapWinUtility.Logger.Message("Error in " + functionName + ", Message: " + errorMsg,"Identifier",System.Windows.Forms.MessageBoxButtons.OK,System.Windows.Forms.MessageBoxIcon.Error, DialogResult.OK);
		}

        private void DockImage_Click(object sender, EventArgs e)
        {
            m_parent.ToggleDockedStatus(this, panel1);
        }

	}
}
