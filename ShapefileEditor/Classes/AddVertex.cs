//********************************************************************************************************
//File Name: AddVertex.cs
//Description: Public class used by shapefile editor to add vertices to a shapefile.
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
//1/29/2005: The code at this point is identical to the public domain code.
//********************************************************************************************************
using System;
using System.Windows.Forms;
using System.IO;

namespace ShapefileEditor
{
	/// <summary>
	/// Summary description for AddVertex.
	/// </summary>
	public class AddVertex
	{
		private struct Point 
		{
			public double x, y;
			public Point(double X, double Y){x = X;y = Y;}
		}

		//member variables
		private MapWindow.Events m_Events;
		private MapWindow.Interfaces.IMapWin m_MapWin;
		private GlobalFunctions m_Globals;
		private MapWinGIS.tkCursorMode m_prevCursorMode;
		private MapWinGIS.tkCursor m_prevCursor;
		private int m_prevCursorHandle;
		private int m_prevShape;
	
		int m_HDraw;
		Cursor m_Cursor;
			
		public AddVertex(GlobalFunctions globals)
		{
			try
			{
				m_Events = globals.Events;
				m_Globals = globals;
				m_HDraw = -1;

				//get the add vertex cursor
				m_Cursor = new Cursor(this.GetType(),"InsertPoint.cur");
							
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

		private void MapMouseDown(int Button, int Shift, int x, int y, ref bool Handled)
		{
			try
			{
				double projX=0,projY=0,newX=0,newY=0;
				int insertIndex = -1;
				MapWinGIS.Shapefile shpFile;

				if(m_Globals.CurrentMode == GlobalFunctions.Modes.AddVertex)
				{
					m_MapWin.View.PixelToProj((double)x,(double)y,ref projX,ref projY);
					
					if(m_Globals.CurrentLayer == null) return;
					shpFile = m_Globals.CurrentLayer;
				
					for(int i=0; i < shpFile.NumShapes; i++)
					{
						//draw the location of the point if it is within the tolerance
						if(WithinTolerance(projX,projY,i,m_Globals.CurrentTolerance,ref newX,ref newY,ref insertIndex))
						{
							//Add this point to the shapefile
							if(!AddPointToShapeFile(newX,newY,shpFile,i,insertIndex))
								MapWinUtility.Logger.Msg("Failed to insert vertex", "Failed to Add Vertex");
						}
					}

					//redraw all vertices
					if(m_Globals.ShowVertices)
						MarkAllVertices(projX,projY);

					//handled this event
					Handled = true;
				}
			}
			catch(System.Exception ex)
			{
				m_MapWin.ShowErrorDialog(ex);
			}
		}

		private void ItemClicked(string ItemName, ref bool Handled)
		{
			try
			{	
				m_MapWin = m_Globals.MapWin;
				if(ItemName == GlobalFunctions.c_AddVertexButton)
				{
                    if (m_Globals.SelectedShapefileIsReadonly())
                    {
                        MapWinUtility.Logger.Msg("The selected shapefile is read-only and cannot be edited.", "Read-Only Shapefile");
                        return;
                    }

					if(m_Globals.CurrentMode != GlobalFunctions.Modes.AddVertex)
					{
						m_Globals.CurrentMode = GlobalFunctions.Modes.AddVertex;
						
						//update the buttons on the toolbar
						m_Globals.UpdateButtons();

						//save the previous settings
						m_prevCursorMode = m_MapWin.View.CursorMode;
						m_prevCursor = m_MapWin.View.MapCursor;
						m_prevCursorHandle = m_MapWin.View.UserCursorHandle;
					
						//set cursor mode to none
						m_MapWin.View.CursorMode = MapWinGIS.tkCursorMode.cmNone;
						m_MapWin.View.MapCursor = MapWinGIS.tkCursor.crsrUserDefined;
						m_MapWin.View.UserCursorHandle = (int)m_Cursor.Handle;
						
						//load the vertex's from the current shapefile layer
						if(m_Globals.CurrentLayer == null) return;
						MapWinGIS.Shapefile shpFile = m_Globals.CurrentLayer;

						m_prevShape = -1;
					}
					else
					{
						if(m_HDraw != -1)
							m_MapWin.View.Draw.ClearDrawing(m_HDraw);
						m_Globals.CurrentMode = GlobalFunctions.Modes.None;

						//set back the old settings
						m_MapWin.View.CursorMode= m_prevCursorMode;
						m_MapWin.View.MapCursor = m_prevCursor;
						m_MapWin.View.UserCursorHandle = m_prevCursorHandle; 

						//update the buttons on the toolbar
						m_Globals.UpdateButtons();

						//set cursor mode 
						m_MapWin.View.CursorMode = MapWinGIS.tkCursorMode.cmPan;

					}
					Handled = true;						
				}
				else
				{
					if(m_MapWin.Toolbar.ButtonItem(GlobalFunctions.c_AddVertexButton).Pressed)
					{
						m_MapWin.View.Draw.ClearDrawing(m_HDraw);
						m_Globals.CurrentMode = GlobalFunctions.Modes.None;

						//hide all vertices
                        if (m_MapWin.Layers.NumLayers > 0)
                        {
                            if (!m_MapWin.Layers[m_MapWin.Layers.CurrentLayer].VerticesVisible)
                                m_MapWin.Layers[m_MapWin.Layers.CurrentLayer].HideVertices();
                        }

						//update the buttons on the toolbar
						m_Globals.UpdateButtons();
					}
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
				double projX=0,projY=0,newX=0,newY=0;
				int insertIndex = -1,shpIndex=-1;
				MapWinGIS.Shapefile shpFile;

				if(m_Globals.CurrentMode == GlobalFunctions.Modes.AddVertex)
				{
					m_MapWin.View.PixelToProj((double)ScreenX,(double)ScreenY,ref projX,ref projY);
					if(m_Globals.CurrentLayer == null) return;
					shpFile = m_Globals.CurrentLayer;
						
					//clear all previous drawings and create a new drawing suface
					ClearDrawings();
	
					//draw all the vertices that this point is in
					if(m_Globals.ShowVertices)
						MarkAllVertices(projX,projY);
					
					for(int i=0; i < shpFile.NumShapes; i++)
					{
						if(m_MapWin.Layers[m_MapWin.Layers.CurrentLayer].LayerType == MapWindow.Interfaces.eLayerType.PolygonShapefile)
						{
							shpFile.BeginPointInShapefile();
							shpIndex = shpFile.PointInShapefile(projX, projY);
							shpFile.EndPointInShapefile();
						}
						else
						{
							MapWinGIS.Extents bounds = new MapWinGIS.ExtentsClass();
							bounds.SetBounds(projX, projY, 0, projX, projY, 0);
							object resArray = null;
							if(shpFile.SelectShapes(bounds, m_Globals.CurrentTolerance, MapWinGIS.SelectMode.INTERSECTION, ref resArray))
							{
								shpIndex = (int)((System.Array)resArray).GetValue(0);
							}
							else
								shpIndex = -1;
						}
					
						if (shpIndex != -1)
						{
							//draw the location of the point if it is within the tolerance
							if(WithinTolerance(projX,projY,shpIndex,m_Globals.CurrentTolerance,ref newX,ref newY,ref insertIndex))
							{
								m_MapWin.View.Draw.DrawPoint(newX,newY,m_Globals.VertexSize,System.Drawing.Color.Blue);
								break;
							}
						}
					}

					//handled this event
					Handled = true;
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
				
				if(m_Globals.CurrentLayer == null) return;
				handle = m_MapWin.Layers.CurrentLayer;	
				MapWinGIS.Shapefile shpFile = m_Globals.CurrentLayer;
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
					if(shpFile.SelectShapes(bounds, m_Globals.CurrentTolerance*2, MapWinGIS.SelectMode.INTERSECTION, ref resArray))
					{
						shpIndex = (int)((System.Array)resArray).GetValue(0);
					}
					else
						shpIndex = -1;
				}

				if (shpIndex >= 0)
				{
					m_MapWin.Layers[handle].Shapes[shpIndex].ShowVertices(System.Drawing.Color.Red, m_Globals.VertexSize);
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

		/********************************************************************/
		// Test the point if it's within the line segements of the curShape
		// returns the insertIndex if within tolerance else -1
		/********************************************************************/
		private bool WithinTolerance(double x, double y,int shpIndex,double tol,ref double storeX, ref double storeY,ref int insertIndex)
		{
			int numPoints =0;
			Point p1,p2;
			double m1,m2;
			double dist,shortestDist=9999999999;
			double b1,b2;
			double newX, newY;
			int index = -1;

			try
			{
				//get the array of points for this shape
				MapWinGIS.Shapefile sf = m_Globals.CurrentLayer;
				System.Array points = sf.QuickPoints(shpIndex,ref numPoints);
				
				//System.Collections.ArrayList points = (System.Collections.ArrayList)m_Points[shpIndex];

				if(numPoints <= 1)
					return false;

				for(int p=0; p < numPoints*2 - 2; p+=2)
				{
					p1 = new Point((double)points.GetValue(p),(double)points.GetValue(p+1));
					p2 = new Point((double)points.GetValue(p+2),(double)points.GetValue(p+3));
					//p1 = (Point)points[i];
					//p2 = (Point)points[i+1];

					//find the slope make sure there is no divide by zero
					if(p2.x == p1.x)
						m1 = 0;
					else
						m1 = (p2.y - p1.y)/(p2.x - p1.x);

					//find the y-intercept
					b1 = p1.y - m1*p1.x;

					//find the slope of the perpendicular line relative to the given point
					if(m1 == 0)
						m2 = 0;
					else
						m2 = -1/m1;

					//find the y-intercept of the perpendicular line relative to the given point
					b2 = y - m2*x;

					//find the intersection point between the two lines
					if(m1 - m2 == 0) // Horizontal line
					{
                        newX = p1.x;
                        if (p1.y == p2.y)
                        {
                            newY = m1 * newX + b1;
                            newX = x;
                        }
                        else
                            newY = m1 * newX + b2;
					}
					else
					{
						newX = (b2 - b1)/(m1 - m2);
						newY = m1*newX + b1;
					}

					//check to make sure the new point is within the line segment
					if(PointWithinLineSegementBounds(p1,p2,new Point(newX,newY)))
					{
						//find the dist between the cursor point and the new point
						dist = PointD.Dist(x,y,newX,newY);
					
						//keep track of the shortest distance 
						if(dist <= shortestDist)
						{
							storeX = newX;
							storeY = newY;
							shortestDist = dist;
							index = p/2;
						}
					}
				}

				if(shortestDist <= tol)
				{
					insertIndex = index+1;
					return true;
				}
			}
			catch(System.Exception ex)
			{
				m_MapWin.ShowErrorDialog(ex);
			}

			//the point is not within the line segment tolerance
			insertIndex = -1;
			return false;                            
		}

		private bool PointWithinLineSegementBounds(Point p1,Point p2,Point p)
		{
			double minX,minY;
			double maxX,maxY;

			//find the min and max X values
			minX = System.Math.Min(p1.x, p2.x);
			minY = System.Math.Min(p1.y, p2.y);
			maxX = System.Math.Max(p1.x, p2.x);
			maxY = System.Math.Max(p1.y, p2.y);

			//check to make sure that the point is within the bounds
			if(p.x >= minX && p.x <= maxX && p.y >= minY && p.y <= maxY)
				return true;
			else
				return false;
		
		}

		private bool AddPointToShapeFile(double x,double y,MapWinGIS.Shapefile shpFile,int shpIndex,int insertIndex)
		{
			try
			{
				MapWinGIS.Point p;
				
				//start editing shapes
				shpFile.StartEditingShapes(true,null);
				MapWinGIS.Shape shp = shpFile.get_Shape(shpIndex);

				p = new MapWinGIS.Point();
				p.x = x;
				p.y = y;

                if (!shp.InsertPoint(p, ref insertIndex))
                {
                    return false;
                }
                else if (shp.NumParts > 1)
                {
                    // Shift up part indices if needed (bugzilla 798)
                    bool shifting = false;
                    for (int i = 0; i < shp.NumParts; i++)
                    {
                        if (shp.get_Part(i) >= insertIndex)
                        {
                            // Shift up this one and all remaining
                            shifting = true;
                        }
                        if (shifting)
                        {
                            shp.set_Part(i, shp.get_Part(i) + 1);
                        }
                    }
                }

                m_Globals.CreateUndoPoint();
            
				//stop editing and save changes
				shpFile.StopEditingShapes(true,true,null);
				
				return true;
			}
			catch(System.Exception ex)
			{
				m_MapWin.ShowErrorDialog(ex);
			}

			return false;
		}

		private void ClearDrawings()
		{
			//clear the drawings
			if(m_HDraw != -1)
				m_MapWin.View.Draw.ClearDrawing(m_HDraw);
			m_HDraw = m_MapWin.View.Draw.NewDrawing(MapWinGIS.tkDrawReferenceList.dlSpatiallyReferencedList);
		}
	}
}
