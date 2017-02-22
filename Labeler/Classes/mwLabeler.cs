// ********************************************************************************************************
// <copyright file="mwLabeler.cs" company="MapWindow.org">
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

namespace Labeler
{
    using System;
    using System.Windows.Forms;
    using MapWinGIS;
    using System.Runtime.InteropServices;

	public struct Bounds
	{
		public int x1;
		public int y1;
		public int x2;
		public int y2;
	}
	
	public class mwLabeler : MapWindow.Interfaces.IPlugin
	{	
		public MapWindow.Interfaces.IMapWin m_MapWin;
		public System.Windows.Forms.Form m_MapWindowForm;
		public Forms.frmLabeler m_Labelerfrm;
		public MapWinGIS.tkCursorMode m_PreviousCursorMode;
		public MapWinGIS.tkCursor m_PreviousCursor;
		public bool m_MouseDown;
		public int m_hDraw;
		public int m_ParentHandle;
		private Cursor m_Cursor;
		private System.Drawing.Color YELLOW = System.Drawing.Color.Yellow;
		private System.Drawing.Color RED = System.Drawing.Color.Red;
		
		// Constructor
		public mwLabeler()
		{
			try
			{
				m_Cursor = new Cursor(this.GetType(),"cursor.cur");
			}
			catch(System.Exception ex)
			{
				ShowErrorBox("mwLabeler()",ex.Message);
			}

		}

		public string Author
		{
			get
			{
				return "MapWindow Open Source Team";
			}
		}

		public string BuildDate
		{
			get
			{
				return System.IO.File.GetLastWriteTime(this.GetType().Assembly.Location).ToString();
			}
		}

		public string Description
		{
			get
			{
				return "Contains the MapWindow labeling functionality.";
			}
		}

		public string Name
		{
			get
			{
				return "MapWindow Labeler";
			}
		}

		public string SerialNumber
		{
			get
			{
				return "";
			}
		}

		public string Version
		{
			get
			{
				string major = System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).FileMajorPart.ToString();
				string minor = System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).FileMinorPart.ToString();
				string build = System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).FileBuildPart.ToString();
				
				return major + "." + minor + "." + build;
			}
		}

		public void Initialize(MapWindow.Interfaces.IMapWin MapWin, int ParentHandle)
		{
			try
			{
				m_MapWin = MapWin;
				m_ParentHandle = ParentHandle;

				m_MapWindowForm = (System.Windows.Forms.Form)System.Windows.Forms.Control.FromHandle((System.IntPtr)ParentHandle);
			}
			catch(System.Exception ex)
			{
				ShowErrorBox("Initialize()",ex.Message);
			}
		}

		public void Terminate()
		{
			// Close the labeler form
			if (m_Labelerfrm != null && !m_Labelerfrm.IsDisposed) m_Labelerfrm.Close();

			m_MapWin = null;
		}

		public void ItemClicked(string ItemName, ref bool Handled)
		{

		}

		public void LayerRemoved(int Handle)
		{
            if (m_Labelerfrm != null && !m_Labelerfrm.IsDisposed)
            {
                try
                {
                    m_Labelerfrm.Close();
                }
                catch
                { }
            }
		}

		public void LayersCleared()
		{
			
		}

		public void LayerSelected(int Handle)
		{
            if (m_Labelerfrm != null && !m_Labelerfrm.IsDisposed)
            {
                try
                {
                    if (m_Labelerfrm.currentHandle != Handle)
                        m_Labelerfrm.Close();
                }
                catch
                { }
            }
		}
		public void MapMouseDown(int Button, int Shift, int x, int y, ref bool Handled)
		{
	
		}

		public void MapMouseUp(int Button, int Shift, int x, int y, ref bool Handled)
		{
			
		}

		public void MapMouseMove(int ScreenX, int ScreenY, ref bool Handled)
		{
		
		}
		
		public void MapDragFinished(System.Drawing.Rectangle Bounds, ref bool Handled)
		{
		
		}

		public void LegendDoubleClick(int Handle, MapWindow.Interfaces.ClickLocation Location, ref bool Handled)
		{
		
		}

		public void LegendMouseDown(int Handle, int Button, MapWindow.Interfaces.ClickLocation Location, ref bool Handled)
		{
		
		}

		public void LegendMouseUp(int Handle, int Button, MapWindow.Interfaces.ClickLocation Location, ref bool Handled)
		{
		
		}

		public void MapExtentsChanged()
		{

		}

		public void Message(string msg, ref bool Handled)
		{
			if (msg != null && msg.Contains("LABEL_EDIT"))
			{
				Handled = true;
				int handle = Convert.ToInt32 (msg.Substring(11,msg.Length - 11));
				System.Diagnostics.Debug.WriteLine(handle.ToString());

                m_Labelerfrm = new Forms.frmLabeler(this, handle, m_MapWin);
                // look for any labeling files to be loaded
                m_Labelerfrm.SetCurrentLayer(handle);
				m_Labelerfrm.Show();
                m_MapWin.Plugins.BroadcastMessage("LABELING_DONE" + msg.Substring(11,msg.Length - 11));
			}
            else if (msg != null && msg.Contains("LABEL_RELABEL"))
            {
                Handled = true;
                int handle = Convert.ToInt32(msg.Substring(14, msg.Length - 14));
                System.Diagnostics.Debug.WriteLine(handle.ToString());

                m_Labelerfrm = new Forms.frmLabeler(this, handle, m_MapWin);
                // look for any labeling files to be loaded
                m_Labelerfrm.SetCurrentLayer(handle);
                m_Labelerfrm.btnRelabel_Click(null, null);
                m_MapWin.Plugins.BroadcastMessage("LABELING_DONE" + msg.Substring(14, msg.Length - 14));
            }
		}

		public void ProjectLoading(string ProjectFile, string SettingsString)
		{
		}

		public void ProjectSaving(string ProjectFile, ref string SettingsString)
		{

        }

		public void ShapesSelected(int Handle, MapWindow.Interfaces.SelectInfo SelectInfo)
		{
			
		}

		public void AddSelectShapes(object selShapes)
		{
			try
			{
				int[] shapes = (int[])selShapes;
					
				// display selected shapes
				int size = shapes.GetUpperBound(0);
				
				for(int i = 0; i <= size; i++)
				{
					m_MapWin.View.SelectedShapes.AddByIndex(shapes[i],YELLOW);
				}
			}
			catch(System.Exception ex)
			{
				ShowErrorBox("SelectShapes()",ex.Message);
			}
		}

		private void LoadLayer()
		{
	
		}

		private void ShowErrorBox(string functionName,string errorMsg)
		{
			MapWinUtility.Logger.Message("Error in " + functionName + ", Message: " + errorMsg,"Label Editor",System.Windows.Forms.MessageBoxButtons.OK,System.Windows.Forms.MessageBoxIcon.Error, DialogResult.OK);
		}

		public void LayersAdded(MapWindow.Interfaces.Layer[] Layers)
		{

		}
	}
}
