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
//1/29/2005 - This code is identical to the public domain version.
//********************************************************************************************************
using System;
using System.Windows.Forms;

namespace ShapefileEditor
{
	/// <summary>
	/// Summary description for RemoveVertex.
	/// </summary>
	public class RemoveVertex
  	{
		//member variables
		private MapWindow.Events m_Events;
		private MapWindow.Interfaces.IMapWin m_MapWin;
		private GlobalFunctions m_globals;
		private int m_hDraw;
		private Cursor m_cursor;
		private SnapClass m_snapClass;
		private int m_prevShape;

		public RemoveVertex(GlobalFunctions globals)
		{
			try
			{
				m_Events = globals.Events;
				m_globals = globals;
				m_hDraw = -1;

				//get the add vertex cursor
				m_cursor = new Cursor(this.GetType(),"RemovePoint.cur");
							
				//register delegates with event handler
				m_Events.AddHandler(new MapWindow.Events.MapMouseDownEvent(MapMouseDown));
				m_Events.AddHandler(new MapWindow.Events.MapMouseMoveEvent(MapMouseMove));
				m_Events.AddHandler(new MapWindow.Events.ItemClickedEvent(ItemClicked));
			}
			catch(System.Exception ex)
			{
				m_MapWin.ShowErrorDialog(ex);
			}
		}

		#region "MapWindow Events"

		private void ItemClicked(string ItemName, ref bool Handled)
		{
			try
			{	
				m_MapWin = m_globals.MapWin;
				if(ItemName == GlobalFunctions.c_RemoveVertexButton)
				{
                    if (m_globals.SelectedShapefileIsReadonly())
                    {
                        MapWinUtility.Logger.Msg("The selected shapefile is read-only and cannot be edited.", "Read-Only Shapefile");
                        return;
                    }

					if(m_globals.CurrentMode != GlobalFunctions.Modes.RemoveVertex)
					{
						m_globals.CurrentMode = GlobalFunctions.Modes.RemoveVertex;
									
						//update the buttons on the toolbar
						m_globals.UpdateButtons();

						//save the previous cusor mode
						m_globals.SaveCusorMode();
                        
						//set cursor mode
						m_MapWin.View.CursorMode = MapWinGIS.tkCursorMode.cmNone;
						m_MapWin.View.MapCursor = MapWinGIS.tkCursor.crsrUserDefined;
						m_MapWin.View.UserCursorHandle = (int)m_cursor.Handle;
						
						//create as new snap class
						if(m_globals.CurrentLayer != null)
							m_snapClass = new SnapClass(m_MapWin);

						m_prevShape = -1;
					}
					else
					{
						m_MapWin.View.Draw.ClearDrawing(m_hDraw);
						m_globals.CurrentMode = GlobalFunctions.Modes.None;

						//set back the old cursor settings
						m_globals.RestoreCursorMode();

						//update the buttons on the toolbar
						m_globals.UpdateButtons();
					}
					Handled = true;						
				}
				else
				{
					if(m_MapWin.Toolbar.ButtonItem(GlobalFunctions.c_RemoveVertexButton).Pressed)
					{
						m_MapWin.View.Draw.ClearDrawing(m_hDraw);
						m_globals.CurrentMode = GlobalFunctions.Modes.None;

						//update the buttons on the toolbar
						m_globals.UpdateButtons();
					}

				}
			}
			catch(System.Exception ex)
			{
				m_MapWin.ShowErrorDialog(ex);
			}
		}

		private void MapMouseDown(int Button, int Shift, int x, int y, ref bool Handled)
		{
			try
			{
				if(m_globals.CurrentMode == GlobalFunctions.Modes.RemoveVertex)
				{
					if(m_globals.CurrentLayer == null) return;
					MapWinGIS.Shapefile shpFile = m_globals.CurrentLayer;

					//get the projection coordinates
					double projX=0,projY=0;
					m_MapWin.View.PixelToProj((double)x,(double)y,ref projX,ref projY);

					//get all the vertex points that are within tolerance
					System.Collections.ArrayList snapPoints = new System.Collections.ArrayList();
                    if (m_snapClass.CanSnap(m_globals.CurrentTolerance, projX, projY, ref snapPoints))
						RemoveSelectedVertex(snapPoints,shpFile);

					//reload the snap class
                    m_snapClass = new SnapClass(m_MapWin);

					Handled = true;
				}
			}
			catch(System.Exception ex)
			{
				m_MapWin.ShowErrorDialog(ex);
			}
		}

		private void MapMouseMove(int ScreenX, int ScreenY, ref bool Handled)
		{
			try
			{
				if(m_globals.CurrentMode == GlobalFunctions.Modes.RemoveVertex)
				{
					double projX=0,projY=0;
					m_MapWin.View.PixelToProj((double)ScreenX,(double)ScreenY,ref projX,ref projY);

					//clear and create a new drawing surface
					ClearDrawings();

					//draw all the vertices that this point is in
					if(m_globals.ShowVertices)
						MarkAllVertices(projX,projY);

					//get all the vertex points that are within tolerance
					System.Collections.ArrayList snapPoints = new System.Collections.ArrayList();
                    if (m_snapClass.CanSnap(m_globals.CurrentTolerance, projX, projY, ref snapPoints))
						DrawAllSnapPoints(snapPoints);
					
					Handled = true;
				}
			}
			catch(System.Exception ex)
			{
				m_MapWin.ShowErrorDialog(ex);
			}

		}
	
		#endregion

		private void DrawAllSnapPoints(System.Collections.ArrayList snapPoints)
		{
			try
			{
				ShapefileEditor.SnapData snapData;
				for(int i=0; i < snapPoints.Count;i++)
				{
					//get the snap point
					snapData =(ShapefileEditor.SnapData)snapPoints[i];
					m_MapWin.View.Draw.DrawPoint(snapData.point.x,snapData.point.y,m_globals.VertexSize,System.Drawing.Color.Blue);
				}
			}
			catch(System.Exception ex)
			{
				m_MapWin.ShowErrorDialog(ex);
			}
		}

		private void MarkAllVertices(double curX, double curY)
		{
			try
			{
				int handle;
				
				if(m_globals.CurrentLayer == null) return;
				handle = m_MapWin.Layers.CurrentLayer;	
				MapWinGIS.Shapefile shpFile = m_globals.CurrentLayer;
				int numShp = shpFile.NumShapes;
				int shpIndex;

                if (m_prevShape != -1)
                {
                    if (!m_MapWin.Layers[m_MapWin.Layers.CurrentLayer].VerticesVisible)
                        m_MapWin.Layers[m_MapWin.Layers.CurrentLayer].HideVertices();
                }

				if(m_MapWin.Layers[m_MapWin.Layers.CurrentLayer].LayerType == MapWindow.Interfaces.eLayerType.PolygonShapefile)
				{
					shpFile.BeginPointInShapefile();
					shpIndex = shpFile.PointInShapefile(curX, curY);
					shpFile.EndPointInShapefile();
				}
				else
				{
					MapWinGIS.Extents bounds = new MapWinGIS.ExtentsClass();
					bounds.SetBounds(curX, curY, 0, curX, curY, 0);
					object resArray = null;
					if(shpFile.SelectShapes(bounds, m_globals.CurrentTolerance*2, MapWinGIS.SelectMode.INTERSECTION, ref resArray))
					{
						shpIndex = (int)((System.Array)resArray).GetValue(0);
					}
					else
						shpIndex = -1;
				}

				if (shpIndex >= 0)
				{
					m_MapWin.Layers[handle].Shapes[shpIndex].ShowVertices(System.Drawing.Color.Red, m_globals.VertexSize);
					m_prevShape = shpIndex;
				}
				else
					m_prevShape = -1;
			}
			catch(System.Exception ex)
			{
				m_MapWin.ShowErrorDialog(ex);
			}
		}

		private void ClearDrawings()
		{
			//clear the drawings
			if(m_hDraw != -1)
				m_MapWin.View.Draw.ClearDrawing(m_hDraw);
			m_hDraw = m_MapWin.View.Draw.NewDrawing(MapWinGIS.tkDrawReferenceList.dlSpatiallyReferencedList);
		}

		private bool PointWithinShape(double x, double y,MapWinGIS.Shapefile shpFile,int shpIndex)
		{
			try
			{				
				MapWinGIS.Shape shp = shpFile.get_Shape(shpIndex);
				
				//check to make sure that the point is within the bounds
				if(x >= shp.Extents.xMin && x <= shp.Extents.xMax && y >= shp.Extents.yMin && y <= shp.Extents.yMax)
					return true;
			}
			catch(System.Exception ex)
			{
				m_MapWin.ShowErrorDialog(ex);
			}

			return false;
		}

		private void RemoveSelectedVertex(System.Collections.ArrayList snapPoints,MapWinGIS.Shapefile shpFile)
		{
			try
			{
				//start editing shapes
				shpFile.StartEditingShapes(true,null);
			
				ShapefileEditor.SnapData snapData;
				int count;

				if(snapPoints.Count == 0)
					return;
			
				if(m_globals.AllowSnapingToVertices) 
					count = snapPoints.Count;
				else
					count = 1;

				//delete all snapPoints
				for(int i=count - 1; i >= 0; i--)
				{
					snapData = (ShapefileEditor.SnapData)snapPoints[i];

                    MapWinGIS.Shape shp = shpFile.get_Shape(snapData.shpIndex);

					if(shpFile.ShapefileType == MapWinGIS.ShpfileType.SHP_POLYLINE)
					{
                        if (shpFile.get_Shape(snapData.shpIndex).numPoints > 2)
                        {
                            if (shp.DeletePoint(snapData.pointIndex))
                                ShiftDownVertices(ref shp, snapData.pointIndex);
                        }
                        else
                            MapWinUtility.Logger.Message("You can not delete this point from this shape. Line shapes must have at least two points. If you want to delete this shape then use remove shape.", "Error in remove vertex", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information, DialogResult.OK);
					}
					else if(shpFile.ShapefileType == MapWinGIS.ShpfileType.SHP_POLYGON)
					{
                        if (shpFile.get_Shape(snapData.shpIndex).numPoints > 4)
                        {
                            if (shp.DeletePoint(snapData.pointIndex))
                                ShiftDownVertices(ref shp, snapData.pointIndex);
                        }
                        else
                            MapWinUtility.Logger.Message("You can not delete this point from this shape. Polygon shapes must have at least three points. If you want to delete this shape then use remove shape.", "Error in remove vertex", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information, DialogResult.OK);
					}
					else if(shpFile.ShapefileType == MapWinGIS.ShpfileType.SHP_POINT)
					{
                        MapWinUtility.Logger.Message("You can not delete this point from this shape. Point shapes must have at least one point. If you want to delete this shape then use remove shape.", "Error in remove vertex", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information, DialogResult.OK);
					}
				}

                m_globals.CreateUndoPoint();

				//stop editing and save changes
				shpFile.StopEditingShapes(true,true,null);
			}
			catch(System.Exception ex)
			{
				m_MapWin.ShowErrorDialog(ex);
			}
		}

        private void ShiftDownVertices(ref MapWinGIS.Shape shp, int deleteIndex)
        {
            if (shp.NumParts > 1)
                {
                    // Shift down part indices if needed (bugzilla 798)
                    bool shifting = false;
                    for (int i = 0; i < shp.NumParts; i++)
                    {
                        if (shp.get_Part(i) > deleteIndex)
                        {
                            // Shift up this one and all remaining
                            shifting = true;
                        }
                        if (shifting)
                        {
                            shp.set_Part(i, Math.Max(0, shp.get_Part(i) - 1));
                        }
                    }
                }
        }
	}
}
