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
        private MapWinGIS.Shapefile m_objShapefile;

        // Chris Michaelis 12/21/2006
        // Instead of storing these as an object,
        // store as integer (slightly faster) private Object m_objShapes;
        private int[] m_intSelectedShapes;

        private int m_hDraw;
        private bool m_blnMousedown4Dragging;

        private double m_dblInitX;
        private double m_dblInitY;
        private double m_dblDestinyX;
        private double m_dblDestinyY;
        private MapWindow.Interfaces.SelectInfo m_selectInfo;

        // Chris Michaelis 12/9/2006
        // A way to hang on to the cursor we'll add to the map when moving shapes
        private Cursor m_Cursor;

        public MoveShapeClass(GlobalFunctions globals)
        {
            m_globals = globals;
            m_MapWin = globals.MapWin;
            m_Cursor = new Cursor(this.GetType(), "Moveshape.cur");

            //add events
            //register delegates with event handler
            m_globals.Events.AddHandler(new MapWindow.Events.MapMouseDownEvent(MapMouseDown));
            m_globals.Events.AddHandler(new MapWindow.Events.MapMouseMoveEvent(MapMouseMove));
            m_globals.Events.AddHandler(new MapWindow.Events.ItemClickedEvent(ItemClicked));
            m_globals.Events.AddHandler(new MapWindow.Events.MapMouseUpEvent(MapMouseUp));
            m_globals.Events.AddHandler(new MapWindow.Events.ShapesSelectedEvent(ShapesSelected));

        }
        private void ItemClicked(string ItemName, ref bool Handled)
        {
            try
            {
                m_MapWin = m_globals.MapWin;
                if (ItemName == GlobalFunctions.c_MoveShapesButton)
                {
                    if (m_globals.SelectedShapefileIsReadonly())
                    {
                        MapWinUtility.Logger.Msg("The selected shapefile is read-only and cannot be edited.", "Read-Only Shapefile");
                        return;
                    }

                    if (m_globals.CurrentMode != GlobalFunctions.Modes.MoveShape)
                    {
                        m_globals.CurrentMode = GlobalFunctions.Modes.MoveShape;

                        // Chris Michaelis 12/9/2006 - As soon as somebody
                        // enables the "Move Shape" mode, turn off the select mode
                        // and set the map mouse pointer to a useful "Move" icon.

                        // Setting the cursor mode to None causes the selection mode to turn off,
                        // since the Cursor Mode is no longer "Selection".
                        m_globals.MapWin.View.CursorMode = MapWinGIS.tkCursorMode.cmNone;

                        // Set a nice "Move" icon
                        m_Cursor = new Cursor(this.GetType(), "Moveshape.cur");
                        m_globals.MapWin.View.UserCursorHandle = m_Cursor.Handle.ToInt32();
                        m_globals.MapWin.View.MapCursor = MapWinGIS.tkCursor.crsrUserDefined;

                        //update the buttons on the toolbar
                        m_globals.UpdateButtons();

                        //save the previous cusor mode
                        m_globals.SaveCusorMode();
                    }
                    else
                    {
                        // Chris Michaelis 12/9/2006 -- Moved this functionality into a single function call
                        TurnOffMoveShape();
                    }
                    //Handled = true;
                }
                else
                {

                    if (m_MapWin.Toolbar.ButtonItem(GlobalFunctions.c_MoveShapesButton).Pressed)
                    {
                        // Chris Michaelis 12/9/2006 -- Moved into a single function.

                        TurnOffMoveShape();
                    }

                }
            }
            catch (System.Exception ex)
            {
                m_globals.LogWrite(ex.StackTrace);
                //m_MapWin.ShowErrorDialog(ex);
            }
        }

        private void TurnOffMoveShape()
        {
            //set back the old cursor settings
            m_globals.RestoreCursorMode();

            // Chris Michaelis 12/9/2006 -- Set the current mode to None after the call
            // to RestureCursorMode. Also, only set to none if it was MoveShape.
            if (m_globals.CurrentMode == GlobalFunctions.Modes.MoveShape)
                m_globals.CurrentMode = GlobalFunctions.Modes.None;

            // Chris Michaelis 12/9/2006 -- Free our custom cursor:
            if (m_globals.MapWin.View.MapCursor == MapWinGIS.tkCursor.crsrUserDefined)
                m_globals.MapWin.View.MapCursor = MapWinGIS.tkCursor.crsrArrow;

            m_globals.MapWin.View.UserCursorHandle = -1;

            //update the buttons on the toolbar
            m_globals.UpdateButtons();
        }

        private void MapMouseDown(int Button, int Shift, int x, int y, ref bool Handled)
        {
            try
            {
                // Chris Michaelis 12/9/2006 -- If the left button is pressed,
                // allow moving the shape. If the right mouse button is pressed,
                // turn off Move mode (consistency -- since right-click finishes
                // when adding a shape also)
                if (Button == 1)
                {
                    if (m_globals.CurrentMode == GlobalFunctions.Modes.MoveShape)
                    {
                        m_globals.CreateUndoPoint();
                        m_objShapefile = m_globals.CurrentLayer;
                        
                        // Chris Michaelis 12/21/2006
                        // Instead of calling SelectShapes with bounds of currently selected shapes
                        // or trying to iterate through each selected shape and reselect it,
                        // just fill m_objShapes with the selected shape indices.
                        // (m_objShapes has also been changed to m_intSelectedShapes)

                        //m_objShapefile.SelectShapes(m_selectInfo.SelectBounds, 0.0,
                        //    MapWinGIS.SelectMode.INCLUSION, ref m_objShapes);
                        if (m_selectInfo.NumSelected > 0)
                        {
                            m_intSelectedShapes = new int[m_selectInfo.NumSelected];
                            for (int i = 0; i < m_selectInfo.NumSelected; i++)
                                m_intSelectedShapes[i] = m_selectInfo[i].ShapeIndex;
                        }
                        else
                            m_intSelectedShapes = null;

                        GC.Collect(); // Free memory released by above statements setting m_intSelectedShapes if it had been set before
                        

                        m_blnMousedown4Dragging = true;
                        m_dblInitX = x;
                        m_dblInitY = y;
                        Handled = true;
                    }
                }
                else if (Button == 2)
                {
                    if (m_MapWin.Toolbar.ButtonItem(GlobalFunctions.c_MoveShapesButton).Pressed)
                    {
                        TurnOffMoveShape();
                        Handled = true;
                    }
                }
            }
            catch (System.Exception ex)
            {
                m_globals.LogWrite(ex.StackTrace);
                //m_MapWin.ShowErrorDialog(ex);
            }
        }

        public void MapMouseUp(int Button, int Shift, int x, int y, ref bool Handled)
        {
            //dpa - 10/14/2007
            //I'm not sure that the first part of this function does anything. 
            //So, I'm commenting out the "handled=true" part since it's causing 
            //trouble for other plugins who need to catch this event and actually
            //do something with it.
            try
            {
                if (m_globals.CurrentMode == GlobalFunctions.Modes.MoveShape)
                {
                    if (m_blnMousedown4Dragging == true)
                    {
                    }
                }
                m_blnMousedown4Dragging = false;
                //if (Button == 2) Handled = true;
            }
            catch (System.Exception ex)
            {
                m_globals.LogWrite(ex.StackTrace);
                //m_MapWin.ShowErrorDialog(ex);
            }
        }

        private void MapMouseMove(int ScreenX, int ScreenY, ref bool Handled)
        {
            try
            {
                if (m_globals.CurrentMode == GlobalFunctions.Modes.MoveShape)
                {
                    if (m_globals.CurrentLayer != null && m_blnMousedown4Dragging)
                    {
                        ////m_globals.LogWrite("MapMouseMove before:" + ScreenX + ", " + ScreenY);
                        MovePolygons(ScreenX, ScreenY);
                        m_dblInitX = m_dblDestinyX;
                        m_dblInitY = m_dblDestinyY;
                        ////m_globals.LogWrite("MapMouseMove:" + m_dblInitX + ", " + m_dblInitY);
                    }

                    Handled = true;
                }
            }
            catch (System.Exception ex)
            {
                m_globals.LogWrite(ex.StackTrace);
                //m_MapWin.ShowErrorDialog(ex);
            }

        }
        private void ClearDrawings()
        {
            //clear the drawings
            try
            {
                if (m_hDraw != -1)
                    m_MapWin.View.Draw.ClearDrawing(m_hDraw);
                m_hDraw = m_MapWin.View.Draw.NewDrawing(MapWinGIS.tkDrawReferenceList.dlSpatiallyReferencedList);
            }
            catch (Exception ex)
            {

                m_globals.LogWrite(ex.StackTrace);
            }
        }
        private void MovePolygons(int ScreenX, int ScreenY)
        {
            try
            {
                if (m_objShapefile != null && m_selectInfo != null)
                {
                    m_globals.LogWrite("num selected shapes: " + m_selectInfo.NumSelected.ToString());
                    m_dblDestinyX = ScreenX;
                    m_dblDestinyY = ScreenY;
                    double dblProjInitX = 0;
                    double dblProjInitY = 0;
                    double dblDestProjX = 0;
                    double dblDestProjY = 0;
                    m_globals.MapWin.View.PixelToProj(m_dblInitX, m_dblInitY, ref dblProjInitX, ref dblProjInitY);
                    m_globals.MapWin.View.PixelToProj(m_dblDestinyX, m_dblDestinyY, ref dblDestProjX, ref dblDestProjY);

                    double dblDistanceX = Math.Abs(dblProjInitX - dblDestProjX);
                    double dblDistanceY = Math.Abs(dblProjInitY - dblDestProjY);

                    
                    if (m_intSelectedShapes!=null)
                    {
                        //double dblxMin, dblyMin, dblzMin, dblxMax, dblyMax, dblzMax;
                        //m_selectInfo.SelectBounds.GetBounds(out dblxMin, out dblyMin, out dblzMin, out dblxMax, out dblyMax, out dblzMax);

                        // lsu 01-feb-2011: in case a callback is present status bar will be refreshed numerously
                        // which as it can lead to the further problems (refreshes of the frmMain)
                        // so, no callback while changing editing mode
                        MapWinGIS.ICallback callback = m_objShapefile.GlobalCallback;
                        m_objShapefile.GlobalCallback = null;

                        if (m_objShapefile.StartEditingShapes(true, null))
                        {
                            // Chris M 12/21/2006 - no longer necessary, using an integer array now
                            /* int[] intShapes = null;
                            intShapes = (int[])m_objShapes; */

                            MapWinGIS.Shape objShape = null;

                            if (m_intSelectedShapes.Length > 0)
                            {
                                //http://www.mapwindow.org/wiki/index.php/MapWinGIS:SampleCode-VB_Net:SelectShapes
                                int intUpperBound = m_intSelectedShapes.Length;
                                m_globals.LogWrite("Upperbound" + intUpperBound);
                                for (int ixShape = 0; ixShape < intUpperBound; ixShape++)
                                {
                                    // Move each next selected shape.                           
                                    objShape = m_objShapefile.get_Shape(m_intSelectedShapes[ixShape]);
                                    Moveshape(dblDistanceX, dblDistanceY, objShape);
                                    m_globals.LogWrite(">1");
                                }
                            }

                            if (!m_objShapefile.StopEditingShapes(true, true, null))//true, true, ICallback)) 
                            {
                                //ToDo: Stopediting failed.
                                throw new Exception("referencing intShapes failed.");
                            };
                            
                            m_globals.MapWin.View.Redraw();
                        }
                        m_objShapefile.GlobalCallback = callback;

                    }
                    else
                    {
                        throw new Exception("Selectshapes failed.");
                    };


                }
                else
                {
                    //ToDo: Handling shapefile not opened.
                    throw new Exception("shapefile opened failed.");
                }
            }
            catch (Exception ex)
            {
                m_globals.LogWrite(ex.Message);
                m_globals.LogWrite(ex.StackTrace); ;
            }
        }

        public void Moveshape(double dblDistanceX, double dblDistanceY, MapWinGIS.Shape objShape)
        {
            int intNumPoints = objShape.numPoints;
            //m_globals.LogWrite("numPoints = " + intNumPoints.ToString ());
            for (int ixPoint = 0; ixPoint < intNumPoints; ixPoint++)
            {
                //Move each point of the shape.
                MovePoint(objShape, ixPoint, dblDistanceX, dblDistanceY);
            };
        }

        private void MovePoint(MapWinGIS.Shape objShape, int ixPoint, double dblDistanceX, double dblDistanceY)
        {
            //m_globals.LogWrite("X= " + m_dblInitX  + ", " + "Y = " + m_dblInitY);
            //m_globals.LogWrite("DistanceX= " + dblDistanceX + ", " + "DistanceY = " + dblDistanceY);
            //m_globals.LogWrite("cur.Y = " + objShape.get_Point(ixPoint).x.ToString()); 

            if (m_dblInitX < m_dblDestinyX)
            {
                MoveXOverDistance(objShape, ixPoint, dblDistanceX, 1);
            }
            else
            {
                MoveXOverDistance(objShape, ixPoint, dblDistanceX, -1);
            }
            if (m_dblInitY > m_dblDestinyY)
            {
                MoveYOverDistance(objShape, ixPoint, dblDistanceY, 1);
            }
            else
            {
                MoveYOverDistance(objShape, ixPoint, dblDistanceY, -1);
            }
            //m_globals.LogWrite("newPoint.y = " + objShape.get_Point(ixPoint).y);
        }

        private void MoveXOverDistance(MapWinGIS.Shape objShape, int ixPoint, double dblDistance, double dblSgn)
        {
            objShape.get_Point(ixPoint).x = objShape.get_Point(ixPoint).x + dblSgn * dblDistance;

        }
        private void MoveYOverDistance(MapWinGIS.Shape objShape, int ixPoint, double dblDistance, double dblSgn)
        {
            objShape.get_Point(ixPoint).y = objShape.get_Point(ixPoint).y + dblSgn * dblDistance;
        }

        public void ShapesSelected(int Handle, MapWindow.Interfaces.SelectInfo SelectInfo)
        {
            m_selectInfo = SelectInfo;
        }
    }
}
