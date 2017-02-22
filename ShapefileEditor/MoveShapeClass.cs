using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ShapefileEditor
{
    public class MoveShapeClass
    {
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

        public MoveShapeClass(GlobalFunctions globals)
		{
			m_globals = globals;
			m_MapWin = globals.MapWin;
	
			//add events
			//register delegates with event handler
			m_globals.Events.AddHandler(new MapWindow.Events.MapMouseDownEvent(MapMouseDown));
			m_globals.Events.AddHandler(new MapWindow.Events.MapMouseMoveEvent(MapMouseMove));
			m_globals.Events.AddHandler(new MapWindow.Events.ItemClickedEvent(ItemClicked));
			m_globals.Events.AddHandler(new MapWindow.Events.MapMouseUpEvent(MapMouseUp));

		}
        private void ItemClicked(string ItemName, ref bool Handled)
        {
            try
            {
                m_MapWin = m_globals.MapWin;
                if (ItemName == GlobalFunctions.c_MoveShapesButton)
                {
                    if (m_globals.CurrentMode != GlobalFunctions.Modes.MoveShape)
                    {
                        m_globals.CurrentMode = GlobalFunctions.Modes.MoveShape;

                        //update the buttons on the toolbar
                        m_globals.UpdateButtons();

                        //save the previous cusor mode
                        m_globals.SaveCusorMode();

                        //set cursor mode
                        m_MapWin.View.CursorMode = MapWinGIS.tkCursorMode.cmNone;
                        m_MapWin.View.MapCursor = MapWinGIS.tkCursor.crsrUserDefined;
                        m_MapWin.View.UserCursorHandle = (int)m_cursor.Handle;

                        //get current shapefile layer
                        if (m_globals.CurrentLayer == null) return;
                        MapWinGIS.Shapefile shpFile = m_globals.CurrentLayer;

                        m_prevShape = -1;
                        //create as new snap class
                        if (m_globals.CurrentLayer == null) return;
                        m_snapClass = new SnapClass(m_MapWin);

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
                    if (m_MapWin.Toolbar.ButtonItem(GlobalFunctions.c_MoveShapesButton).Pressed)
                    {
                        m_MapWin.View.Draw.ClearDrawing(m_hDraw);
                        m_globals.CurrentMode = GlobalFunctions.Modes.None;

                        //get current shapefile layer
                        m_MapWin.Layers[m_MapWin.Layers.CurrentLayer].HideVertices();

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
        private void MapMouseDown(int Button, int Shift, int x, int y, ref bool Handled)
        {
            try
            {
                if (m_globals.CurrentMode == GlobalFunctions.Modes.MoveShape)
                {
                    //get the projection coordinates
                    double projX = 0, projY = 0;
                    m_MapWin.View.PixelToProj((double)x, (double)y, ref projX, ref projY);

                    //get all the vertex points that are within tolerance
                    m_snapPoints = new System.Collections.ArrayList();
                    if (!m_snapClass.CanSnap(m_globals.CurrentTolerance, projX, projY, ref m_snapPoints))
                        return;

                    m_MapDraging = true;
                    Handled = true;
                }
            }
            catch (System.Exception ex)
            {
                m_MapWin.ShowErrorDialog(ex);
            }
        }

        public void MapMouseUp(int Button, int Shift, int x, int y, ref bool Handled)
        {
            try
            {
                if (m_globals.CurrentMode == GlobalFunctions.Modes.MoveShape)
                {
                    if (m_MapDraging == true)
                    {
                        double projX = 0, projY = 0;
                        m_MapWin.View.PixelToProj((double)x, (double)y, ref projX, ref projY);

                        //get the working shapefile
                        if (m_globals.CurrentLayer == null) return;
                        MapWinGIS.Shapefile shpFile = m_globals.CurrentLayer;

                        //start editing the vertex postion
                        shpFile.StartEditingShapes(true, null);



                        //stop editing and save changes
                        shpFile.StopEditingShapes(true, true, null);

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
            catch (System.Exception ex)
            {
                m_MapWin.ShowErrorDialog(ex);
            }
        }

        private void MapMouseMove(int ScreenX, int ScreenY, ref bool Handled)
        {
            try
            {
                if (m_globals.CurrentMode == GlobalFunctions.Modes.MoveShape)
                {
                    double projX = 0, projY = 0;
                    m_MapWin.View.PixelToProj((double)ScreenX, (double)ScreenY, ref projX, ref projY);

                    //clear and create a new drawing surface
                    ClearDrawings();

                    if (m_MapDraging)
                    {
                    }
                    Handled = true;
                }
            }
            catch (System.Exception ex)
            {
                m_MapWin.ShowErrorDialog(ex);
            }

        }
        private void ClearDrawings()
        {
            //clear the drawings
            if (m_hDraw != -1)
                m_MapWin.View.Draw.ClearDrawing(m_hDraw);
            m_hDraw = m_MapWin.View.Draw.NewDrawing(MapWinGIS.tkDrawReferenceList.dlSpatiallyReferencedList);
        }
    }
}
