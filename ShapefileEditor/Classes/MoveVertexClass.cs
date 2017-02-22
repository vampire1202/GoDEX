//********************************************************************************************************
//File Name: MoveVertexClass.cs
//Description: Public class used by shapefile editor to move vertices in a shapefile.
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
//1/29/2005 This code is identical to the public domain version.
//********************************************************************************************************
using System;
using System.Windows.Forms;

namespace ShapefileEditor
{
	/// <summary>
	/// Summary description for MoveVertexClass.
	/// </summary>
	public class MoveVertexClass
	{
		private struct Point 
		{
			public double x, y;
			public Point(double X, double Y){x = X;y = Y;}
		}

		//private members
		private GlobalFunctions m_globals;
		private MapWindow.Interfaces.IMapWin m_MapWin;
		private Cursor m_cursor;
		private Cursor m_snapCursor;
		private int m_hDraw;
		private bool m_MapDraging;
		private SnapClass m_snapClass;
		private System.Collections.ArrayList m_snapPoints;
		private int m_prevShape;
		//private System.Collections.ArrayList m_Points;
		
		public MoveVertexClass(GlobalFunctions globals)
		{
			m_globals = globals;
			m_MapWin = globals.MapWin;

			//load the cursor
			m_cursor = new Cursor(this.GetType(),"Move.cur");
			m_snapCursor = new Cursor(this.GetType(),"MoveSnap.cur");
			m_hDraw = -1;
	
			//add events
			//register delegates with event handler
			m_globals.Events.AddHandler(new MapWindow.Events.MapMouseDownEvent(MapMouseDown));
			m_globals.Events.AddHandler(new MapWindow.Events.MapMouseMoveEvent(MapMouseMove));
			m_globals.Events.AddHandler(new MapWindow.Events.ItemClickedEvent(ItemClicked));
			m_globals.Events.AddHandler(new MapWindow.Events.MapMouseUpEvent(MapMouseUp));
		}
		
		#region "MapWindow Events"

		private void ItemClicked(string ItemName, ref bool Handled)
		{
			try
			{	
				m_MapWin = m_globals.MapWin;
				if(ItemName == GlobalFunctions.c_MoveVertexButton)
				{
                    if (m_globals.SelectedShapefileIsReadonly())
                    {
                        MapWinUtility.Logger.Msg("The selected shapefile is read-only and cannot be edited.", "Read-Only Shapefile");
                        return;
                    }

					if(m_globals.CurrentMode != GlobalFunctions.Modes.MoveVertex)
					{
                        m_globals.LogWrite("if2");
						m_globals.CurrentMode = GlobalFunctions.Modes.MoveVertex;
									
						//update the buttons on the toolbar
						m_globals.UpdateButtons();

						//save the previous cusor mode
						m_globals.SaveCusorMode();
                        
						//set cursor mode
						m_MapWin.View.CursorMode = MapWinGIS.tkCursorMode.cmNone;
						m_MapWin.View.MapCursor = MapWinGIS.tkCursor.crsrUserDefined;
						m_MapWin.View.UserCursorHandle = (int)m_cursor.Handle;
						
						//get current shapefile layer
						if(m_globals.CurrentLayer == null) return;
						MapWinGIS.Shapefile shpFile = m_globals.CurrentLayer;
												
						m_prevShape = -1;
						//create as new snap class
						if(m_globals.CurrentLayer == null) return;
                        m_snapClass = new SnapClass(m_MapWin);

					}
					else
					{
                        m_globals.LogWrite("else2");
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
                    m_globals.LogWrite("else1");
					if(m_MapWin.Toolbar.ButtonItem(GlobalFunctions.c_MoveVertexButton).Pressed)
					{
                        m_globals.LogWrite("if3");
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
			catch(System.Exception ex)
			{
                m_MapWin.ShowErrorDialog(ex);
			}
		}

		private void MapMouseDown(int Button, int Shift, int x, int y, ref bool Handled)
		{
			try
			{
				if(m_globals.CurrentMode == GlobalFunctions.Modes.MoveVertex)
				{
					//get the projection coordinates
					double projX=0,projY=0;
					m_MapWin.View.PixelToProj((double)x,(double)y,ref projX,ref projY);

					//get all the vertex points that are within tolerance
					m_snapPoints = new System.Collections.ArrayList();
                    if (!m_snapClass.CanSnap(m_globals.CurrentTolerance, projX, projY, ref m_snapPoints))
						return;

					m_MapDraging = true;
					Handled = true;
				}
			}
			catch(System.Exception ex)
			{
				m_MapWin.ShowErrorDialog(ex);
			}
		}

		public void MapMouseUp(int Button, int Shift, int x, int y, ref bool Handled)
		{
			try
			{
				if(m_globals.CurrentMode == GlobalFunctions.Modes.MoveVertex)
				{
					if(m_MapDraging == true)
					{
						double projX=0,projY=0;
						m_MapWin.View.PixelToProj((double)x,(double)y,ref projX,ref projY);

						//get the working shapefile
						if(m_globals.CurrentLayer == null) return;
						MapWinGIS.Shapefile shpFile = m_globals.CurrentLayer;

						//start editing the vertex postion
						shpFile.StartEditingShapes(true,null);

						//get all the vertex points that are within tolerance
						System.Collections.ArrayList snapPoints = new System.Collections.ArrayList();
                        if (m_snapClass.CanSnap(m_globals.CurrentTolerance, projX, projY, ref snapPoints))
							DrawAllSnapPoints(snapPoints);

						ShapefileEditor.SnapData snapPoint;
						MapWinGIS.Shape shp;
						MapWinGIS.Point p;

						//if allow snaping then change all vertex in the snaping tolerance
						int count;
						if(m_globals.AllowSnapingToVertices)
							count = m_snapPoints.Count;
						else
							count = Math.Min(m_snapPoints.Count, 1);

                        for (int i = 0; i < count; i++)
                        {
                            snapPoint = (ShapefileEditor.SnapData)m_snapPoints[i];
                            shp = shpFile.get_Shape(snapPoint.shpIndex);
                            p = shp.get_Point(snapPoint.pointIndex);

                            //check to see if were going to snap to a point
                            if (snapPoints.Count > 0 && m_globals.AllowSnapingToVertices)
                            {
                                p.x = ((ShapefileEditor.SnapData)snapPoints[0]).point.x;
                                p.y = ((ShapefileEditor.SnapData)snapPoints[0]).point.y;
                            }
                            else
                            {
                                p.x = projX;
                                p.y = projY;
                            }
                        }

                        m_globals.CreateUndoPoint();

						//stop editing and save changes
						shpFile.StopEditingShapes(true,true,null);
                        
						//set the cursor to move cur
						m_MapWin.View.UserCursorHandle = (int)m_cursor.Handle;

						// update snap class
                        m_snapClass = new SnapClass(m_MapWin);
					}
									
					ClearDrawings();
					m_MapDraging = false;
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
				if(m_globals.CurrentMode == GlobalFunctions.Modes.MoveVertex)
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

					if(m_MapDraging)
					{
						if(snapPoints.Count != 0 && m_globals.AllowSnapingToVertices)
							m_MapWin.View.UserCursorHandle = (int)m_snapCursor.Handle;
						else
							m_MapWin.View.UserCursorHandle = (int)m_cursor.Handle;

						DrawMoveLine(ScreenX,ScreenY,m_snapPoints);
					}
					Handled = true;
				}
			}
			catch(System.Exception ex)
			{
				m_MapWin.ShowErrorDialog(ex);
			}

		}

		private void DrawAllSnapPoints(System.Collections.ArrayList snapPoints)
		{
			try
			{
				ShapefileEditor.SnapData snapData;
				for(int i=0; i < snapPoints.Count;i++)
				{
					//get the snap point
					snapData =(ShapefileEditor.SnapData)snapPoints[i];
					m_MapWin.View.Draw.DrawPoint(snapData.point.x,snapData.point.y, m_globals.VertexSize,System.Drawing.Color.Blue);
				}
			}
			catch(System.Exception ex)
			{
				m_MapWin.ShowErrorDialog(ex);
			}
		}

		private void DrawMoveLine(double x, double y,System.Collections.ArrayList snapPoints)
		{
			try
			{
				if(snapPoints.Count == 0) return;

				double projX=0,projY=0;
				m_MapWin.View.PixelToProj(x,y,ref projX,ref projY);

				//get the working shapefile
				if(m_globals.CurrentLayer == null) return;
				MapWinGIS.Shapefile shpFile = m_globals.CurrentLayer;
			
				//get the current vertex index
				ShapefileEditor.SnapData snapData =(ShapefileEditor.SnapData) snapPoints[0];
				int vertexIndex = snapData.pointIndex;

				//get the current shape
				MapWinGIS.Shape shp = shpFile.get_Shape(snapData.shpIndex);

				if(shpFile.ShapefileType == MapWinGIS.ShpfileType.SHP_POINT)
				{
					m_MapWin.View.Draw.DrawPoint(projX,projY, m_globals.VertexSize,System.Drawing.Color.Red);
				}
				else
				{
					if(vertexIndex > 0 && vertexIndex < shp.numPoints -1)
					{
						//draw a line from the prev to current to next vertex
						MapWinGIS.Point prevPoint = shp.get_Point(vertexIndex - 1);
						MapWinGIS.Point nextPoint = shp.get_Point(vertexIndex + 1);

						m_MapWin.View.Draw.DrawLine(prevPoint.x,prevPoint.y,projX,projY,2,System.Drawing.Color.Red);
						m_MapWin.View.Draw.DrawLine(projX,projY,nextPoint.x,nextPoint.y,2,System.Drawing.Color.Red);
					}
					else if(vertexIndex == 0)
					{
						MapWinGIS.Point nextPoint = shp.get_Point(vertexIndex + 1);
						m_MapWin.View.Draw.DrawLine(projX,projY,nextPoint.x,nextPoint.y,2,System.Drawing.Color.Red);
					}
					else if(vertexIndex == shp.numPoints -1)
					{
						MapWinGIS.Point prevPoint = shp.get_Point(vertexIndex - 1);
						m_MapWin.View.Draw.DrawLine(prevPoint.x,prevPoint.y,projX,projY,2,System.Drawing.Color.Red);
					}
				}
						
			}
			catch(System.Exception ex)
			{
				m_MapWin.ShowErrorDialog(ex);
			}
		}
	
		#endregion

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
                    if (shpFile.BeginPointInShapefile())
                    {
                        shpIndex = shpFile.PointInShapefile(curX, curY);
                        shpFile.EndPointInShapefile();
                    }
                    else
                        shpIndex = -1;
				}
				else
				{
					MapWinGIS.Extents bounds = new MapWinGIS.ExtentsClass();
					bounds.SetBounds(curX, curY, 0, curX, curY, 0);
					object resArray = null;
					if(shpFile.SelectShapes(bounds, m_globals.CurrentTolerance * 2, MapWinGIS.SelectMode.INTERSECTION, ref resArray))
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

		private bool PointInView(Point p)
		{
			MapWinGIS.Extents e = m_globals.MapWin.View.Extents;
			if (p.x < e.xMax && p.x > e.xMin && p.y < e.yMax && p.y > e.yMin)
				return true;
			else
				return false;
		}

		private void ClearDrawings()
		{
			//clear the drawings
			if(m_hDraw != -1)
				m_MapWin.View.Draw.ClearDrawing(m_hDraw);
			m_hDraw = m_MapWin.View.Draw.NewDrawing(MapWinGIS.tkDrawReferenceList.dlSpatiallyReferencedList);
		}

//		private bool PointWithinShape(double x, double y,MapWinGIS.Shapefile shpFile,int shpIndex)
//		{
//			try
//			{				
//				MapWinGIS.Shape shp = shpFile.get_Shape(shpIndex);
//				
//				//check to make sure that the point is within the bounds
//				if(x >= shp.Extents.xMin && x <= shp.Extents.xMax && y >= shp.Extents.yMin && y <= shp.Extents.yMax)
//					return true;
//			}
//			catch(System.Exception ex)
//			{
//				m_MapWin.ShowErrorDialog(ex);
//			}
//
//			return false;
//		}

	}
}
