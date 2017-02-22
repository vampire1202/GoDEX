//********************************************************************************************************
//The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
//you may not use this file except in compliance with the License. You may obtain a copy of the License at 
//http://www.mozilla.org/MPL/ 
//Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
//ANY KIND, either express or implied. See the License for the specificlanguage governing rights and 
//limitations under the License. 
//
//The Original Code is MapWindow Open Source. 
//
//The Initial Developer of this version of the Original Code is Daniel P. Ames using portions created by 
//Utah State University and the Idaho National Engineering and Environmental Lab that were released as 
//public domain in March 2004.  
//
//Contributor(s): (Open source contributors should list themselves and their modifications here). 
//1/29/2005 - This code is identical to the public domain version. - dpa
//4/6/2008 - Earljon Hidalgo - Fix Bug #841
//********************************************************************************************************
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace ShapefileEditor
{
	/// <summary>
	/// Summary description for AddShapefileForm.
	/// </summary>
	public class AddShapefileForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label lblFilename;
		internal System.Windows.Forms.TextBox txtFilename;
		private System.Windows.Forms.Label lblType;
		internal System.Windows.Forms.ComboBox cmbType;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// The AddShapefileForm prompts the end-user for information needed to create a new shapefile.
		/// </summary>
		public AddShapefileForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddShapefileForm));
            this.lblFilename = new System.Windows.Forms.Label();
            this.txtFilename = new System.Windows.Forms.TextBox();
            this.lblType = new System.Windows.Forms.Label();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblFilename
            // 
            resources.ApplyResources(this.lblFilename, "lblFilename");
            this.lblFilename.Name = "lblFilename";
            // 
            // txtFilename
            // 
            resources.ApplyResources(this.txtFilename, "txtFilename");
            this.txtFilename.Name = "txtFilename";
            this.txtFilename.TextChanged += new System.EventHandler(this.txtFilename_TextChanged);
            this.txtFilename.Validating += new System.ComponentModel.CancelEventHandler(this.txtFilename_Validating);
            // 
            // lblType
            // 
            resources.ApplyResources(this.lblType, "lblType");
            this.lblType.Name = "lblType";
            // 
            // cmbType
            // 
            this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbType.Items.AddRange(new object[] {
            resources.GetString("cmbType.Items"),
            resources.GetString("cmbType.Items1"),
            resources.GetString("cmbType.Items2")});
            resources.ApplyResources(this.cmbType, "cmbType");
            this.cmbType.Name = "cmbType";
            // 
            // btnBrowse
            // 
            resources.ApplyResources(this.btnBrowse, "btnBrowse");
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.Name = "btnOK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // AddShapefileForm
            // 
            this.AcceptButton = this.btnOK;
            resources.ApplyResources(this, "$this");
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.cmbType);
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.txtFilename);
            this.Controls.Add(this.lblFilename);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddShapefileForm";
            this.Load += new System.EventHandler(this.AddShapefileForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void btnBrowse_Click(object sender, System.EventArgs e)
		{
			// Prompt for shapefile save location
			System.Windows.Forms.SaveFileDialog dlg = new System.Windows.Forms.SaveFileDialog();
			dlg.AddExtension = true;
			dlg.DefaultExt = "shp";
			dlg.CheckPathExists = true;
			dlg.CheckFileExists = false;
			dlg.Filter = "ESRI Shapefiles (*.shp)|*.shp";
			dlg.Title = "Choose location for new shapefile";
			dlg.ValidateNames = true;
			dlg.OverwritePrompt = false;
			if (dlg.ShowDialog(this.Parent) == System.Windows.Forms.DialogResult.OK)
			{
				txtFilename.Text = dlg.FileName;
			}
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			if (txtFilename.Text.IndexOfAny(System.IO.Path.GetInvalidPathChars()) >= 0)
			{
                MapWinUtility.Logger.Msg("Invalid characters were detected in the filename!  Please only use valid characters.", "Invalid characters in filename!");
				return;
			}

			if(System.IO.Path.IsPathRooted(txtFilename.Text) == false) 
			{
                MapWinUtility.Logger.Msg("The file path that you have specified is not rooted.  Please specify the full path.", "Filename not rooted!");
				return;
			}

			string dir = System.IO.Path.GetDirectoryName(txtFilename.Text);
			if (System.IO.Directory.Exists(dir) == false)
			{
				if (MapWinUtility.Logger.Message("The folder '" + dir + "' does not exist.  Do you wish to create it?", "Confirm folder creation", System.Windows.Forms.MessageBoxButtons.YesNo, MessageBoxIcon.Question, DialogResult.Yes) == System.Windows.Forms.DialogResult.Yes)
				{
					System.IO.Directory.CreateDirectory(dir);
				}
				else
					return;
			}

			if (System.IO.File.Exists(txtFilename.Text) == true)
			{
				if (MapWinUtility.Logger.Message("The file '" + txtFilename.Text + "' already exists.  Do you wish to overwrite this file?", "Confirm file replace", System.Windows.Forms.MessageBoxButtons.YesNo, MessageBoxIcon.Question, DialogResult.Yes) == System.Windows.Forms.DialogResult.Yes)
				{
					// Delete all the files related to this shapefile before creating a new shapefile.
					string shp = txtFilename.Text;
					string shx = System.IO.Path.ChangeExtension(shp, ".shx");
					string dbf = System.IO.Path.ChangeExtension(shp, ".dbf");

					try {System.IO.File.Delete(shp);} catch {}
					try {System.IO.File.Delete(shx);} catch {}
					try {System.IO.File.Delete(dbf);} catch {}
				}
				else
					return;
			}
			this.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.Hide();
		}

		private void txtFilename_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
            // 4/6/2008 - Earljon Hidalgo - Fix Bug #841
            // Before we assume the last path is a filename with no extension,
            // check first if last directory is a valid one.
            if (System.IO.Directory.Exists(txtFilename.Text))
            {
                txtFilename.Text = System.IO.Path.Combine(txtFilename.Text.Trim(), "NewShape.shp");
                return;
            }

			if (txtFilename.Text.Length > 0 && txtFilename.Text.ToLower().EndsWith(".shp") == false)
			{
				if (txtFilename.Text.EndsWith("."))
					txtFilename.Text += "shp";
				else
					txtFilename.Text += ".shp";
			}
		}

		private void AddShapefileForm_Load(object sender, System.EventArgs e)
		{
			if(cmbType.Items.Count > 0)
				cmbType.SelectedIndex = 0;
		}

		private void txtFilename_TextChanged(object sender, System.EventArgs e)
		{
			//check to see if this path is valid
			if (txtFilename.Text.IndexOfAny(System.IO.Path.GetInvalidPathChars()) >= 0)
			{
				//invalid path
				return;
			}

			if(System.IO.Path.IsPathRooted(txtFilename.Text) == false) 	
			{
				//invalid path
				return;
			}
		}
	}
}
