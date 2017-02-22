using System;

namespace ShapefileEditor
{
	/// <summary>
	/// Summary description for RemoveVertexClass.
	/// </summary>
	public class RemoveVertexClass
	{
		private MapWindow.Events.ItemClickedEvent ItemClickedDelegate;
		private MapWindow.Events.MapMouseMoveEvent MapMouseMoveDelegate;
		private MapWindow.Events.MapMouseUpEvent MapMouseUpDelegate;
		private MapWindow.Events.TerminateEvent TerminateDelegate;

		private GlobalFunctions m_globals = null;
		private int m_HDraw;
		private System.Windows.Forms.Cursor m_cursor;
		private SnapClass m_snapper;

		public RemoveVertexClass(GlobalFunctions g)
		{
			m_globals = g;
			ItemClickedDelegate = new MapWindow.Events.ItemClickedEvent(ItemClickedEvent);
			g.Events.AddHandler(ItemClickedDelegate);
			TerminateDelegate = new MapWindow.Events.TerminateEvent(TerminateEvent);
			g.Events.AddHandler(TerminateDelegate);
			m_cursor = new System.Windows.Forms.Cursor(this.GetType(), "RemovePoint.cur");	
			
			MapWinGIS.Shapefile sf = g.CurrentLayer;
			if (sf != null)
				m_snapper = new SnapClass(sf);
			else
				m_snapper = null;
		}

		public void ItemClickedEvent(string ItemName, ref bool Handled)
		{
			if(ItemName == GlobalFunctions.c_RemoveVertexButton)
			{
				if (m_globals.CurrentMode == GlobalFunctions.Modes.RemoveVertex)
				{
					// I'm already in this mode.  Clean up everything.
					Cleanup();
				}
				else
				{
					// Save cursor mode information
					m_globals.SaveCusorMode();

					// Set the cursor.
					MapWindow.Interfaces.View v = m_globals.MapWin.View;
					v.CursorMode = MapWinGIS.tkCursorMode.cmNone;
					v.MapCursor = MapWinGIS.tkCursor.crsrUserDefined;
					v.UserCursorHandle = m_cursor.Handle.ToInt32();

					// Start taking events for MouseMove and MouseUp
					MapMouseMoveDelegate = new MapWindow.Events.MapMouseMoveEvent(MapMouseMoveEvent);
					m_globals.Events.AddHandler(MapMouseMoveDelegate);
					MapMouseUpDelegate = new MapWindow.Events.MapMouseUpEvent(MapMouseUpEvent);
					m_globals.Events.AddHandler(MapMouseUpDelegate);
				}
			}
			else
			{
				Cleanup();
			}
		}

		public void MapMouseMoveEvent(int ScreenX, int ScreenY, ref bool Handled)
		{
			if (m_globals.CurrentMode == GlobalFunctions.Modes.RemoveVertex)
			{
				System.Collections.ArrayList lst = null;
				double px = 0, py = 0;
				m_globals.MapWin.View.PixelToProj((double)ScreenX, (double)ScreenY, ref px, ref py);
				m_globals.MapWin.View.Select(ScreenX, ScreenY, true);
				MarkAllVertices();
				if (m_snapper != null && m_snapper.CanSnap(m_globals.CurrentTolerance, px, py, ref lst))
				{
					MarkSelectedVertices(lst);
				}
			}
		}

		public void MapMouseUpEvent(int Button, int Shift, int x, int y, ref bool Handled)
		{
			if (m_globals.CurrentMode == GlobalFunctions.Modes.RemoveVertex)
			{
				System.Collections.ArrayList lst = null;
				double px = 0, py = 0;
				m_globals.MapWin.View.PixelToProj((double)x, (double)y, ref px, ref py);
				m_globals.MapWin.View.Select(x, y, true);
				if (m_snapper != null && m_snapper.CanSnap(m_globals.CurrentTolerance, px, py, ref lst))
				{
					RemoveVertices(lst);
				}
			}
		}

		private void RemoveVertices(System.Collections.ArrayList lst)
		{
			try
			{
				MapWindow.Interfaces.View v = m_globals.MapWin.View;

				foreach (SnapData data in lst)
				{
					if (IsSelected(data.shpIndex))
					{
						MapWinGIS.Shapefile sf = m_globals.CurrentLayer;
						sf.StartEditingShapes(true, null);

						MapWinGIS.Shape shp = sf.get_Shape(data.shpIndex);
						shp.DeletePoint(data.pointIndex);
					}
				}
			}
			catch (System.Exception ex)
			{
				m_globals.MapWin.ShowErrorDialog(ex);
			}
		}

		private void MarkSelectedVertices(System.Collections.ArrayList lst)
		{
			MapWindow.Interfaces.View v = m_globals.MapWin.View;

			foreach (SnapData data in lst)
			{
				if (IsSelected(data.shpIndex))
				{
					v.Draw.DrawPoint(data.point.x, data.point.y, m_globals.VertexSize, System.Drawing.Color.Blue);
				}
			}
		}

		private bool IsSelected(int shpIndex)
		{
			MapWindow.Interfaces.View v = m_globals.MapWin.View;

			for(int i = 0; i < v.SelectedShapes.NumSelected; i++)
			{
				if(v.SelectedShapes[i].ShapeIndex == shpIndex)
					return true; // shape is selected
			}
			
			// default case (shape is not selected!)
			return false;
		}

		private void MarkAllVertices()
		{
			try
			{
				MapWinGIS.Shape shp = null;
				MapWinGIS.Shapefile sf = m_globals.CurrentLayer;
				MapWindow.Interfaces.View v = m_globals.MapWin.View;

				//clear the drawings
				v.Draw.ClearDrawing(m_HDraw);
				m_HDraw = v.Draw.NewDrawing(MapWinGIS.tkDrawReferenceList.dlSpatiallyReferencedList);
				
				//display all the vertices for each shape
				for(int i = 0; i < v.SelectedShapes.NumSelected; i++)
				{
					shp = sf.get_Shape(v.SelectedShapes[i].ShapeIndex);

					for(int j = 0; j < shp.numPoints; j++)
					{
						MapWinGIS.Point shpPoint = shp.get_Point(j);
						m_globals.MapWin.View.Draw.DrawPoint(shpPoint.x, shpPoint.y, m_globals.VertexSize, System.Drawing.Color.Red);
					}
				}
			}
			catch(System.Exception ex)
			{
				m_globals.MapWin.ShowErrorDialog(ex);
			}
		}

		public void TerminateEvent()
		{
			Cleanup();
		}

		private void Cleanup()
		{
			if (MapMouseMoveDelegate != null) 
			{
				m_globals.Events.RemoveHandler(MapMouseMoveDelegate);
				MapMouseMoveDelegate = null;
			}
			if (MapMouseUpDelegate != null) 
			{
				m_globals.Events.RemoveHandler(MapMouseUpDelegate);
				MapMouseUpDelegate = null;
			}
		}
	}
}
