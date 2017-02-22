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
//As of 1/29/2005 only one function has been added (CloseAddShapeForm) over the public domain version.
//
//********************************************************************************************************
using System;
using System.ComponentModel;
using Modes = ShapefileEditor.GlobalFunctions.Modes;

namespace ShapefileEditor
{
	/// <summary>
	/// Summary description for AddShapeClass.
	/// </summary>
	public class AddShapeClass
	{
		private GlobalFunctions m_globals;
		private System.Windows.Forms.Form m_AddShapeForm;
		private SnapClass m_snapper;
		private MapWinGIS.Shapefile m_CurrentShapefile;
        private MapWindow.Interfaces.IMapWin m_MapWin;
        private int m_hDraw;

		public AddShapeClass(GlobalFunctions g)
		{
			m_globals = g;
			g.Events.AddHandler(new MapWindow.Events.ItemClickedEvent(ItemClickedEvent));
            m_MapWin = g.MapWin;
            m_hDraw = -1;
		}
		///The following function was added by Dan Ames and is an update over the version 3.0 code to handle the AddShapeForm
		public void CloseAddShapeForm()
		{
			if(m_AddShapeForm != null)
			{
				m_AddShapeForm.Close();
			}
		}

		private void ItemClickedEvent(string ItemName, ref bool Handled)
		{
            try
            {
                m_MapWin = m_globals.MapWin;
                m_MapWin.View.Draw.ClearDrawing(m_hDraw);
                //if (m_MapWin.Layers.CurrentLayer != -1) - This produces a crash with a new project loading!
                if (m_MapWin.Layers.NumLayers > 0)
                {
                    if (!m_MapWin.Layers[m_MapWin.Layers.CurrentLayer].VerticesVisible)
                        m_MapWin.Layers[m_MapWin.Layers.CurrentLayer].HideVertices();
                }
                if (m_snapper != null)
                {
                    m_snapper.ClearSnapData();
                    m_snapper = null;
                }
			if (ItemName == GlobalFunctions.c_AddShapeButton)
			{
                if (m_globals.SelectedShapefileIsReadonly())
                {
                    MapWinUtility.Logger.Msg("The selected shapefile is read-only and cannot be edited.", "Read-Only Shapefile");
                    return;
                }

                // Add a new shape now
				if (m_globals.CurrentMode == GlobalFunctions.Modes.AddShape)
				{
					m_globals.CurrentMode = GlobalFunctions.Modes.None;
					m_globals.UpdateButtons();
					if (m_AddShapeForm != null)
					{	
						m_AddShapeForm.Close();
					}
				}
				else
				{
					m_globals.CurrentMode  = GlobalFunctions.Modes.AddShape;
					m_globals.UpdateButtons();
					AddNewShape();
						
				}
				Handled = true;
			}
                else
                {

                    if (m_MapWin.Toolbar.ButtonItem(GlobalFunctions.c_AddShapeButton).Pressed)
                    {
                        if (m_snapper != null)
                        {
                            m_snapper.ClearSnapData();
                            m_snapper = null;
                        }

                        m_MapWin.View.Draw.ClearDrawing(m_hDraw);
                        m_globals.CurrentMode = GlobalFunctions.Modes.None;

                        //get current shapefile layer
                        if (m_MapWin.Layers.NumLayers > 0)
                        {
                            if (!m_MapWin.Layers[m_MapWin.Layers.CurrentLayer].VerticesVisible)
                                m_MapWin.Layers[m_MapWin.Layers.CurrentLayer].HideVertices();
                        }

                        //update the buttons on the toolbar
                        m_globals.UpdateButtons();
                        
                    }

                }
            }
            catch (System.Exception ex)
            {
                m_MapWin.ShowErrorDialog(ex);
            }
        }
		private void AddNewShape()
		{
            // Paul Meems, 26 Oct. 2009, fix for bug #1460
            // Added check for in-memory shapefiles, those have no filename:
            if (!m_globals.MapWin.Layers.IsValidHandle(m_globals.MapWin.Layers.CurrentLayer)) return;
            // End modifications Paul Meems, 26 Oct. 2009

			MapWinGIS.Shapefile sf = (MapWinGIS.Shapefile)m_globals.MapWin.Layers[m_globals.MapWin.Layers.CurrentLayer].GetObject();
            //if (m_snapper == null)
                //m_snapper = new SnapClass(m_globals.MapWin);

			if (m_AddShapeForm == null || m_AddShapeForm.IsDisposed)
			{
				m_AddShapeForm = new AddShapeForm(m_globals, ref m_snapper);
				m_AddShapeForm.Closing += new CancelEventHandler(FormClosing);
				m_globals.MapWindowForm.AddOwnedForm(m_AddShapeForm);
			}
			m_AddShapeForm.Show();
			sf = null;
		}

		private void FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
		{
            m_globals.CurrentMode = Modes.None;
			m_globals.UpdateButtons();
			m_globals.DoEnables();

            m_globals.UpdateView();
		}

		private void LayerSelected(int Handle)
		{
			MapWindow.Interfaces.eLayerType lt = m_globals.MapWin.Layers[Handle].LayerType;
			if (lt == MapWindow.Interfaces.eLayerType.LineShapefile || lt == MapWindow.Interfaces.eLayerType.PointShapefile ||
				lt == MapWindow.Interfaces.eLayerType.PolygonShapefile)
			{
				m_CurrentShapefile = (MapWinGIS.Shapefile)m_globals.MapWin.Layers[Handle].GetObject();
			}
			else
				m_CurrentShapefile = null;
		}
	}
}
