//********************************************************************************************************
//File Name: GlobalFunctions.cs
//Description: Public class of functions used by shapefile editor.
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
//1-Jun-06 - Rob Cairns: Changes to support large shapefiles
//********************************************************************************************************
using System;
using System.IO;
using System.Text;

namespace ShapefileEditor
{
    /// <summary>
    /// Summary description for GlobalFunctions.
    /// </summary>
    public class GlobalFunctions
    {
        #region "Constants"
        public const string c_NewButton = "NewShapefileButton";
        public const string c_AddShapeButton = "AddShapeButton";
        public const string c_RemoveShapeButton = "RemoveShapeButton";
        public const string c_MoveVertexButton = "MoveVertexButton";
        public const string c_MoveShapesButton = "MoveShapesButton";
        // Chris Michaelis 2/5/2007
        public const string c_CopyShapeButton = "CopyShapeButton";
        public const string c_PasteShapeButton = "PasteShapeButton";
        public const string c_CleanupButton = "CleanupButton";
        public const string c_UndoDropdownButton = "UndoDropdownButton";
        public const string c_UndoLastChangeButton = "UndoLastChangeButton";
        public const string c_UndoEnableButton = "UndoEnableButton";
        public const string c_UndoDisableButton = "UndoDisableButton";
        // Chris Michaelis 12/21/2006
        public const string c_RotateShapeButton = "RotateShapeButton";
        // 12/28/2006
        public const string c_ResizeShapeButton = "ResizeShapeButton";
        public const string c_InsertStockShapeButton = "InsertStockShapeButton";
        public const string c_AddVertexButton = "AddVertexButton";
        public const string c_RemoveVertexButton = "RemoveVertexButton";
        public const string c_VertexSizeButton = "VertexSizeButton";
        public const string c_ShowVerticesButton = "ShowVerticesButton";
        public const string c_SnapToVerticesButton = "SnapToVerticesButton";
        public const string c_SnapToAllLayersButton = "SnapToAllLayers";
        public const string c_StayInAddModeButton = "StayInAddModeButton";
        public const string c_GridSnapButton = "GridSnapButton";
        public const string c_SnapToGridButton = "SnapToGridButton";
        public const string c_SnapOptionsButton = "SnapOptionsButton";
        //public const string c_ToolbarName = "ShapefileEditorToolbar";
        public const string c_ToolbarName = "Shapefile Editor";
        public const string c_MenuName = "ShapefileEditorMenu";
        //ARA 04/03/07
        public const string c_MergeShapesButton = "MergeShapesButton";
        public const string c_EraseWithShapeButton = "EraseWithShapeButton";
        public const string c_EraseBeneathShapeButton = "EraseBeneathShapeButton";
        #endregion

        #region "Enumerations"
        public enum Modes { None, AddShape, MoveShape, RotateShape, RemoveShape, MoveVertex, AddVertex, RemoveVertex };
        #endregion

        public int SnapTolerancePixelRadius;


        private MapWinGIS.tkCursorMode m_prevCursorMode;
        private MapWinGIS.tkCursor m_prevCursor;
        private int m_prevCursorHandle;

        private Modes m_CurrentMode;
        private MapWinGIS.Shapefile m_CurrentLayer;
        private MapWindow.Interfaces.IMapWin m_MapWin;
        private System.Windows.Forms.Form m_MapWindowForm;
        private MapWindow.Events m_events;
        private long m_LayerSize;
        private MapWindow.Interfaces.SelectInfo m_SelectInfo;

        public static bool m_StayInAddMode = false;

        //private System.IO.StreamWriter m_logFile = null;
        //private string xm_path = @"c:\temp\MyTest.txt";

        public System.Collections.Stack UndoStack = new System.Collections.Stack();

        /// <summary>
        /// Constructor
        /// </summary>
        public GlobalFunctions(MapWindow.Interfaces.IMapWin mw, System.Windows.Forms.Form ParentForm)
        {
            m_MapWin = mw;
            m_MapWindowForm = ParentForm;
            m_events = new MapWindow.Events();
            m_CurrentMode = Modes.None;
            SnapTolerancePixelRadius = 4;
            m_LayerSize = 0;
        }

        ~GlobalFunctions()
        {
            UndoClearHistory();
            m_MapWin = null;
            m_MapWindowForm = null;
            m_events = null;
            m_CurrentMode = Modes.None;
            m_CurrentLayer = null;
        }

        public bool SelectedShapefileIsReadonly()
        {
            try
            {
                // Paul Meems, 26 Oct. 2009, fix for bug #1460
                // Added check for in-memory shapefiles, those have no filename:
                if (!MapWin.Layers.IsValidHandle(MapWin.Layers.CurrentLayer)) return true;
                // End modifications Paul Meems, 26 Oct. 2009

                string sf = ((MapWinGIS.Shapefile)MapWin.Layers[MapWin.Layers.CurrentLayer].GetObject()).Filename;
                System.IO.FileInfo f = new FileInfo(sf);
                // Note the single & -- bitwise
                if ((f.Attributes & System.IO.FileAttributes.ReadOnly) == System.IO.FileAttributes.ReadOnly)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public void UpdateView()
        {
            MapWin.View.Redraw();

            // Adding shapes seems to make some MW versions lose the fill settings.
            // Cause it to take effect just by setting the value to itself.
            if (MapWin.Layers[MapWin.Layers.CurrentLayer].DrawFill)
            {
                MapWin.Layers[MapWin.Layers.CurrentLayer].DrawFill = true;
                if (MapWin.Layers[MapWin.Layers.CurrentLayer].ColoringScheme != null)
                    MapWin.Layers[MapWin.Layers.CurrentLayer].ColoringScheme = MapWin.Layers[MapWin.Layers.CurrentLayer].ColoringScheme;
            }
            MapWin.View.MapCursor = MapWinGIS.tkCursor.crsrArrow;
        }


        /// <summary>
        /// Enables or disables toolbar buttons and menu items depending on the state of the MapWindow.
        /// </summary>
        public void DoEnables()
        {
            MapWin.Toolbar.ButtonItem(c_NewButton).Enabled = true;

            if ((MapWin == null) || (MapWin.Layers == null) || (MapWin.Layers.NumLayers == 0 || MapWin.Layers[MapWin.Layers.CurrentLayer].LayerType == MapWindow.Interfaces.eLayerType.Grid ||
                MapWin.Layers[MapWin.Layers.CurrentLayer].LayerType == MapWindow.Interfaces.eLayerType.Image ||
                MapWin.Layers[MapWin.Layers.CurrentLayer].LayerType == MapWindow.Interfaces.eLayerType.Invalid))
            {
                MapWin.Toolbar.ButtonItem(c_AddShapeButton).Enabled = false;
                MapWin.Toolbar.ButtonItem(c_MoveShapesButton).Enabled = false;
                MapWin.Toolbar.ButtonItem(c_AddVertexButton).Enabled = false;
                MapWin.Toolbar.ButtonItem(c_MoveVertexButton).Enabled = false;
                MapWin.Toolbar.ButtonItem(c_RemoveShapeButton).Enabled = false;
                MapWin.Toolbar.ButtonItem(c_RemoveVertexButton).Enabled = false;
                MapWin.Toolbar.ButtonItem(c_RotateShapeButton).Enabled = false;
                MapWin.Toolbar.ButtonItem(c_ResizeShapeButton).Enabled = false;
                MapWin.Toolbar.ButtonItem(c_InsertStockShapeButton).Enabled = false;
                MapWin.Toolbar.ButtonItem(c_CopyShapeButton).Enabled = false;
                MapWin.Toolbar.ButtonItem(c_PasteShapeButton).Enabled = false;
                MapWin.Toolbar.ButtonItem(c_UndoDisableButton).Enabled = false;
                MapWin.Toolbar.ButtonItem(c_UndoEnableButton).Enabled = false;
                MapWin.Toolbar.ButtonItem(c_UndoLastChangeButton).Enabled = false;
                MapWin.Toolbar.ButtonItem(c_CleanupButton).Enabled = false;
                //ARA 04/03/07
                MapWin.Toolbar.ButtonItem(c_MergeShapesButton).Enabled = false;
                MapWin.Toolbar.ButtonItem(c_EraseWithShapeButton).Enabled = false;
                MapWin.Toolbar.ButtonItem(c_EraseBeneathShapeButton).Enabled = false;

                return;
            }

            MapWindow.Interfaces.eLayerType lt = MapWin.Layers[MapWin.Layers.CurrentLayer].LayerType;
            if (lt == MapWindow.Interfaces.eLayerType.LineShapefile ||
                lt == MapWindow.Interfaces.eLayerType.PointShapefile ||
                lt == MapWindow.Interfaces.eLayerType.PolygonShapefile)
            {
                MapWin.Toolbar.ButtonItem(c_AddShapeButton).Enabled = true;
                MapWin.Toolbar.ButtonItem(c_MoveShapesButton).Enabled = true;
                MapWin.Toolbar.ButtonItem(c_MoveVertexButton).Enabled = true;
                MapWin.Toolbar.ButtonItem(c_InsertStockShapeButton).Enabled = true;
                //ARA 04/03/07
                MapWin.Toolbar.ButtonItem(c_MergeShapesButton).Enabled = true;

                MapWin.Toolbar.ButtonItem(c_PasteShapeButton).Enabled = true;
                if (!MapWin.Toolbar.ButtonItem(c_UndoEnableButton).Enabled && !MapWin.Toolbar.ButtonItem(c_UndoDisableButton).Enabled)
                    MapWin.Toolbar.ButtonItem(c_UndoEnableButton).Enabled = true;
                MapWin.Toolbar.ButtonItem(c_CleanupButton).Enabled = true;
                                
                if (lt != MapWindow.Interfaces.eLayerType.PointShapefile)
                {
                    MapWin.Toolbar.ButtonItem(c_AddVertexButton).Enabled = true;
                    MapWin.Toolbar.ButtonItem(c_RemoveVertexButton).Enabled = true;
                }
                else
                {
                    MapWin.Toolbar.ButtonItem(c_AddVertexButton).Enabled = false;
                    MapWin.Toolbar.ButtonItem(c_RemoveVertexButton).Enabled = false;
                }

                // Chris Michaelis 12/28/2006
                // Resize and Rotate button only valid for polygons
                // Predefined shapes button only valid for polygons
                if (lt == MapWindow.Interfaces.eLayerType.PolygonShapefile)
                {
                    MapWin.Toolbar.ButtonItem(c_RotateShapeButton).Enabled = true;
                    MapWin.Toolbar.ButtonItem(c_ResizeShapeButton).Enabled = true;
                    MapWin.Toolbar.ButtonItem(c_InsertStockShapeButton).Enabled = true;
                    //ARA 03/05/07
                    MapWin.Toolbar.ButtonItem(c_EraseWithShapeButton).Enabled = true;
                    MapWin.Toolbar.ButtonItem(c_EraseBeneathShapeButton).Enabled = true;
                }
                else
                {
                    MapWin.Toolbar.ButtonItem(c_RotateShapeButton).Enabled = false;
                    MapWin.Toolbar.ButtonItem(c_ResizeShapeButton).Enabled = false;
                    MapWin.Toolbar.ButtonItem(c_InsertStockShapeButton).Enabled = false;
                    MapWin.Toolbar.ButtonItem(c_EraseWithShapeButton).Enabled = false;
                    MapWin.Toolbar.ButtonItem(c_EraseBeneathShapeButton).Enabled = false;
                }
            }
            else
            {
                MapWin.Toolbar.ButtonItem(c_AddShapeButton).Enabled = false;
                MapWin.Toolbar.ButtonItem(c_MoveShapesButton).Enabled = false;
                MapWin.Toolbar.ButtonItem(c_AddVertexButton).Enabled = false;
                MapWin.Toolbar.ButtonItem(c_MoveVertexButton).Enabled = false;
                MapWin.Toolbar.ButtonItem(c_RemoveShapeButton).Enabled = false;
                MapWin.Toolbar.ButtonItem(c_RemoveVertexButton).Enabled = false;
                MapWin.Toolbar.ButtonItem(c_RotateShapeButton).Enabled = false;
                MapWin.Toolbar.ButtonItem(c_ResizeShapeButton).Enabled = false;
                MapWin.Toolbar.ButtonItem(c_InsertStockShapeButton).Enabled = false;
                MapWin.Toolbar.ButtonItem(c_CopyShapeButton).Enabled = false;
                MapWin.Toolbar.ButtonItem(c_PasteShapeButton).Enabled = false;
                MapWin.Toolbar.ButtonItem(c_UndoDropdownButton).Enabled = false;
                MapWin.Toolbar.ButtonItem(c_UndoDisableButton).Enabled = false;
                MapWin.Toolbar.ButtonItem(c_UndoEnableButton).Enabled = false;
                MapWin.Toolbar.ButtonItem(c_UndoLastChangeButton).Enabled = false;
                MapWin.Toolbar.ButtonItem(c_CleanupButton).Enabled = false;
                //ARA 04/03/07
                MapWin.Toolbar.ButtonItem(c_MergeShapesButton).Enabled = false;
                MapWin.Toolbar.ButtonItem(c_EraseWithShapeButton).Enabled = false;
                MapWin.Toolbar.ButtonItem(c_EraseBeneathShapeButton).Enabled = false;
            }

            if (MapWin.Layers[MapWin.Layers.CurrentLayer].Shapes.NumShapes == 0)
            {
                MapWin.Toolbar.ButtonItem(c_MoveShapesButton).Enabled = false;
                MapWin.Toolbar.ButtonItem(c_RemoveShapeButton).Enabled = false;
                MapWin.Toolbar.ButtonItem(c_RemoveVertexButton).Enabled = false;
                MapWin.Toolbar.ButtonItem(c_MoveVertexButton).Enabled = false;
                MapWin.Toolbar.ButtonItem(c_AddVertexButton).Enabled = false;
                MapWin.Toolbar.ButtonItem(c_RotateShapeButton).Enabled = false;
                MapWin.Toolbar.ButtonItem(c_ResizeShapeButton).Enabled = false;
                //ARA 04/03/07
                MapWin.Toolbar.ButtonItem(c_MergeShapesButton).Enabled = false;
                MapWin.Toolbar.ButtonItem(c_EraseWithShapeButton).Enabled = false;
                MapWin.Toolbar.ButtonItem(c_EraseBeneathShapeButton).Enabled = false;

                MapWin.Toolbar.ButtonItem(c_CopyShapeButton).Enabled = false;
            }

            // Chris Michaelis 12/9/2006 -- Added code to enable or disable
            // the Move Shapes button depending on selected shape count.
            if (MapWin.View.SelectedShapes.NumSelected > 0)
            {
                MapWin.Toolbar.ButtonItem(c_RemoveShapeButton).Enabled = true;
                MapWin.Toolbar.ButtonItem(c_MoveShapesButton).Enabled = true;
                MapWin.Toolbar.ButtonItem(c_CopyShapeButton).Enabled = true;

                // Chris Michaelis 12/28/2006
                // Resize and Rotate button only valid for polygons
                if (lt == MapWindow.Interfaces.eLayerType.PolygonShapefile)
                {
                    MapWin.Toolbar.ButtonItem(c_RotateShapeButton).Enabled = true;
                    MapWin.Toolbar.ButtonItem(c_ResizeShapeButton).Enabled = true;

                    //Erase only valid with single selected shape
                    if (MapWin.View.SelectedShapes.NumSelected > 1)
                    {
                        MapWin.Toolbar.ButtonItem(c_EraseWithShapeButton).Enabled = false;
                        MapWin.Toolbar.ButtonItem(c_EraseBeneathShapeButton).Enabled = false;
                    }
                    else
                    {
                        MapWin.Toolbar.ButtonItem(c_EraseWithShapeButton).Enabled = true;
                        MapWin.Toolbar.ButtonItem(c_EraseBeneathShapeButton).Enabled = true;
                    }                    
                }
            }
            else
            {
                MapWin.Toolbar.ButtonItem(c_CopyShapeButton).Enabled = false;
                MapWin.Toolbar.ButtonItem(c_RemoveShapeButton).Enabled = false;
                MapWin.Toolbar.ButtonItem(c_MoveShapesButton).Enabled = false;
                MapWin.Toolbar.ButtonItem(c_RotateShapeButton).Enabled = false;
                MapWin.Toolbar.ButtonItem(c_ResizeShapeButton).Enabled = false;
                MapWin.Toolbar.ButtonItem(c_EraseWithShapeButton).Enabled = false;
                MapWin.Toolbar.ButtonItem(c_EraseBeneathShapeButton).Enabled = false;
            }
        }

        /// <summary>
        /// Makes sure that only one button is toggled at any given time.
        /// </summary>
        /// <param name="PressedButtonName">The name of the button that should be pressed currently.</param>
        public void UpdateButtons()
        {
            LogWrite("UpdateButtons CurrentMode:" + CurrentMode.ToString());
            try
            {
                switch (CurrentMode)
                {
                    case Modes.AddShape:
                        MapWin.Toolbar.ButtonItem(c_AddShapeButton).Pressed = true;
                        MapWin.Toolbar.ButtonItem(c_MoveShapesButton).Pressed = false;
                        MapWin.Toolbar.ButtonItem(c_AddVertexButton).Pressed = false;
                        MapWin.Toolbar.ButtonItem(c_MoveVertexButton).Pressed = false;
                        MapWin.Toolbar.ButtonItem(c_RemoveVertexButton).Pressed = false;
                        break;
                    case Modes.AddVertex:
                        MapWin.Toolbar.ButtonItem(c_AddShapeButton).Pressed = false;
                        MapWin.Toolbar.ButtonItem(c_MoveShapesButton).Pressed = false;
                        MapWin.Toolbar.ButtonItem(c_AddVertexButton).Pressed = true;
                        MapWin.Toolbar.ButtonItem(c_MoveVertexButton).Pressed = false;
                        MapWin.Toolbar.ButtonItem(c_RemoveVertexButton).Pressed = false;
                        break;
                    case Modes.MoveVertex:
                        MapWin.Toolbar.ButtonItem(c_AddShapeButton).Pressed = false;
                        MapWin.Toolbar.ButtonItem(c_MoveShapesButton).Pressed = false;
                        MapWin.Toolbar.ButtonItem(c_AddVertexButton).Pressed = false;
                        MapWin.Toolbar.ButtonItem(c_MoveVertexButton).Pressed = true;
                        MapWin.Toolbar.ButtonItem(c_RemoveVertexButton).Pressed = false;
                        break;
                    case Modes.RemoveVertex:
                        MapWin.Toolbar.ButtonItem(c_AddShapeButton).Pressed = false;
                        MapWin.Toolbar.ButtonItem(c_MoveShapesButton).Pressed = false;
                        MapWin.Toolbar.ButtonItem(c_AddVertexButton).Pressed = false;
                        MapWin.Toolbar.ButtonItem(c_MoveVertexButton).Pressed = false;
                        MapWin.Toolbar.ButtonItem(c_RemoveVertexButton).Pressed = true;
                        break;
                    case Modes.MoveShape:
                        MapWin.Toolbar.ButtonItem(c_AddShapeButton).Pressed = false;
                        MapWin.Toolbar.ButtonItem(c_MoveShapesButton).Pressed = true;
                        MapWin.Toolbar.ButtonItem(c_AddVertexButton).Pressed = false;
                        MapWin.Toolbar.ButtonItem(c_MoveVertexButton).Pressed = false;
                        MapWin.Toolbar.ButtonItem(c_RemoveVertexButton).Pressed = false;

                        break;
                    default:
                        MapWin.Toolbar.ButtonItem(c_AddShapeButton).Pressed = false;
                        MapWin.Toolbar.ButtonItem(c_MoveShapesButton).Pressed = false;
                        MapWin.Toolbar.ButtonItem(c_AddVertexButton).Pressed = false;
                        MapWin.Toolbar.ButtonItem(c_MoveVertexButton).Pressed = false;
                        MapWin.Toolbar.ButtonItem(c_RemoveVertexButton).Pressed = false;
                        break;
                }
            }
            catch (Exception ex)
            {

                LogWrite(ex.StackTrace);
            }
        }

        /// <summary>
        /// Calculates the tolerance radius used for snapping.
        /// </summary>
        /// <returns>Returns the distance in map units of the tolerance radius</returns>
        public double CurrentTolerance
        {
            get
            {
                double mapX = 0, mapY = 0;
                double mapX2 = 0, mapY2 = 0;
                MapWin.View.PixelToProj(0, 0, ref mapX, ref mapY);
                MapWin.View.PixelToProj(SnapTolerancePixelRadius, SnapTolerancePixelRadius, ref mapX2, ref mapY2);
                return PointD.Dist(mapX, mapY, mapX2, mapY2);
            }

        }

        public void RestoreCursorMode()
        {
            //set back the old settings
            // CDM Aug 9 2006 -- ONLY if in one of my modes
            if (CurrentMode == Modes.AddShape ||
                CurrentMode == Modes.MoveShape ||
                CurrentMode == Modes.AddVertex ||
                CurrentMode == Modes.MoveVertex ||
                CurrentMode == Modes.RemoveShape ||
                CurrentMode == Modes.RemoveVertex ||
                CurrentMode == Modes.RotateShape)
            {
                MapWin.View.CursorMode = m_prevCursorMode;
                MapWin.View.MapCursor = m_prevCursor;
                MapWin.View.UserCursorHandle = m_prevCursorHandle;
            }
        }

        public void SaveCusorMode()
        {
            //save the previous settings
            // CDM Aug 9 2006 -- ONLY if in one of my modes
            if (CurrentMode == Modes.AddShape ||
                CurrentMode == Modes.MoveShape ||
                CurrentMode == Modes.AddVertex ||
                CurrentMode == Modes.MoveVertex ||
                CurrentMode == Modes.RemoveShape ||
                CurrentMode == Modes.RemoveVertex)
            {
                m_prevCursorMode = MapWin.View.CursorMode;
                m_prevCursor = MapWin.View.MapCursor;
                m_prevCursorHandle = MapWin.View.UserCursorHandle;
            }
        }

        public Modes CurrentMode
        {
            get
            {
                return m_CurrentMode;
            }
            set
            {
                m_CurrentMode = value;
                UpdateButtons();
            }
        }

        public MapWinGIS.Shapefile CurrentLayer
        {
            get
            {
                try
                {
                    if (m_CurrentLayer == null)
                    {
                        UpdateCurrentLayer();
                    }
                    return m_CurrentLayer;
                }
                catch
                {
                    return null;
                }
            }
        }

        public void UndoClearHistory()
        {
            while (UndoStack.Count > 0)
            {
                string s = (string)UndoStack.Pop();
                string xtmp = System.IO.Path.ChangeExtension(s, ".shx");
                string ddbf = System.IO.Path.ChangeExtension(s, ".dbf");
                if (System.IO.File.Exists(s)) System.IO.File.Delete(s);
                if (System.IO.File.Exists(xtmp)) System.IO.File.Delete(xtmp);
                if (System.IO.File.Exists(ddbf)) System.IO.File.Delete(ddbf);
            }
            UpdateUndoButtons();
        }

        private void UpdateUndoButtons()
        {
            try
            {
                if (MapWin == null) return;

                MapWin.Toolbar.ButtonItem(c_UndoLastChangeButton).Enabled = (UndoStack.Count > 0);

                // Ensure the disable/enable buttons are right, while we're here
                if (UndoStack.Count > 0)
                {
                    MapWin.Toolbar.ButtonItem(c_UndoEnableButton).Enabled = false;
                    MapWin.Toolbar.ButtonItem(c_UndoDisableButton).Enabled = true;
                }
            }
            catch { }
        }

        public void UndoLastChange()
        {
            // MapWindow doesn't lock the shapefile so long as we're
            // not currently editing; so the file can be overwritten
            // and a redraw triggered.
            if (UndoStack.Count == 0) return;

            // Paul Meems, 26 Oct. 2009, fix for bug #1460
            // Added check for in-memory shapefiles, those have no filename:
            if (!MapWin.Layers.IsValidHandle(MapWin.Layers.CurrentLayer)) return;
            // End modifications Paul Meems, 26 Oct. 2009

            MapWinGIS.Shapefile sf = (MapWinGIS.Shapefile)MapWin.Layers[MapWin.Layers.CurrentLayer].GetObject();
            if (sf.EditingShapes || sf.EditingTable) sf.StopEditingShapes(false, true, null);

            // Lock the map and legend -- we're about to do evil things to the underlying map
            MapWin.View.LockMap();
            MapWin.View.LockLegend();
            
            string tmp = (string)UndoStack.Pop();
            string xtmp = System.IO.Path.ChangeExtension(tmp, ".shx");
            string dtmp = System.IO.Path.ChangeExtension(tmp, ".dbf");

            string fail = MapWin.Layers[MapWin.Layers.CurrentLayer].FileName;
            string xfail = System.IO.Path.ChangeExtension(MapWin.Layers[MapWin.Layers.CurrentLayer].FileName, ".shx");
            string dfail = System.IO.Path.ChangeExtension(MapWin.Layers[MapWin.Layers.CurrentLayer].FileName, ".dbf");

            sf.Close();
            
            System.IO.File.Copy(tmp, fail, true);
            System.IO.File.Copy(xtmp, xfail, true);
            System.IO.File.Copy(dtmp, dfail, true);

            System.IO.File.Delete(tmp);
            System.IO.File.Delete(xtmp);
            System.IO.File.Delete(dtmp);

            sf.Open(fail, null);
            
            // Unlock and refresh:
            MapWin.View.UnlockMap();
            MapWin.View.UnlockLegend();
            UpdateView();
            UpdateUndoButtons();
        }

        public void CreateUndoPoint()
        {
            // PRoceed only if Undo is enabled
            if (MapWin.Toolbar.ButtonItem(c_UndoDisableButton).Enabled == false) return;

            try
            {
                string tmp = System.IO.Path.GetTempFileName();
                System.IO.File.Delete(tmp);
                tmp = System.IO.Path.ChangeExtension(tmp, ".shp");

                string xtmp = System.IO.Path.ChangeExtension(tmp, ".shx");
                string dtmp = System.IO.Path.ChangeExtension(tmp, ".dbf");
                string xorig = System.IO.Path.ChangeExtension(MapWin.Layers[MapWin.Layers.CurrentLayer].FileName, ".shx");
                string dorig = System.IO.Path.ChangeExtension(MapWin.Layers[MapWin.Layers.CurrentLayer].FileName, ".dbf");

                System.IO.File.Copy(MapWin.Layers[MapWin.Layers.CurrentLayer].FileName, tmp);
                System.IO.File.Copy(xorig, xtmp);
                System.IO.File.Copy(dorig, dtmp);

                UndoStack.Push(tmp);
                UpdateUndoButtons();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
        }

        public long LayerSize
        {
            get
            {
                try
                {
                    if (m_LayerSize == 0)
                    {
                        System.IO.FileInfo LayerInfo = new System.IO.FileInfo(m_CurrentLayer.Filename);
                        m_LayerSize = LayerInfo.Length;

                    }
                    return m_LayerSize;
                }
                catch
                {
                    return 0;
                }
            }
        }
        public void UpdateCurrentLayer()
        {
            // Clear our undo history and start over:
            UndoClearHistory();

            if (MapWin == null || MapWin.Layers == null)
            {
                m_CurrentLayer = null;
                return;
            }
            int cl = MapWin.Layers.CurrentLayer;
            if (cl == -1)
            {
                m_CurrentLayer = null;
                return;
            }
            if (!MapWin.Layers.IsValidHandle(cl))
                m_CurrentLayer = null;
            else
            {
                MapWindow.Interfaces.eLayerType lt = MapWin.Layers[cl].LayerType;
                if (lt == MapWindow.Interfaces.eLayerType.LineShapefile || lt == MapWindow.Interfaces.eLayerType.PointShapefile || lt == MapWindow.Interfaces.eLayerType.PolygonShapefile)
                {
                    m_CurrentLayer = (MapWinGIS.Shapefile)MapWin.Layers[MapWin.Layers.CurrentLayer].GetObject();
                    // Paul Meems, 26 Oct. 2009, fix for bug #1460
                    // Added check for in-memory shapefiles, those have no filename:
                    if (m_CurrentLayer.Filename == string.Empty)
                    {
                        m_LayerSize = 0;
                    }
                    else
                    {
                        System.IO.FileInfo LayerInfo = new System.IO.FileInfo(m_CurrentLayer.Filename);
                        m_LayerSize = LayerInfo.Length;
                    }
                    // End modifications Paul Meems, 26 Oct. 2009
                }
                else
                    m_CurrentLayer = null;
            }
        }

        public MapWindow.Interfaces.IMapWin MapWin
        {
            get
            {
                return m_MapWin;
            }
        }

        public System.Windows.Forms.Form MapWindowForm
        {
            get
            {
                return m_MapWindowForm;
            }
        }

        public MapWindow.Events Events
        {
            get
            {
                return m_events;
            }
        }

        public bool AllowSnapingToVertices
        {
            get
            {
                return m_MapWin.Menus[c_SnapToVerticesButton].Checked;
            }
        }

        public int VertexSize
        {
            get
            {
                // Chris Michaelis -- This is now a hardcoded value.
                return 4;

                // MapWindow.Interfaces.ComboBoxItem cb = m_MapWin.Toolbar.ComboBoxItem(c_VertexSizeButton);
                // int v = (int)cb.SelectedItem;
                // return v;
            }
        }

        public bool ShowVertices
        {
            get
            {
                return m_MapWin.Menus[c_ShowVerticesButton].Checked;
            }
        }

        public void SetLogFile()
        {
            return; // Use system.diagnostics.debug instead

            /*string path = @"c:\temp\MyTest.txt";

            try
            {
                //Append if exists, else Create the file.
                if (File.Exists(m_path))
                {
                    StreamWriter m_logFile = File.AppendText(path);
                    m_logFile.Flush();
                    m_logFile.Close();
                }
                else
                {
                    using (StreamWriter sw = new StreamWriter(m_path))
                    {
                        sw.Close();
                    };
                };
            }
            catch (Exception ex)
            {
                m_logFile.WriteLine(ex.Message);
                throw;
            }*/
        }

        public void LogWrite(string value)
        {
            System.Diagnostics.Debug.WriteLine(value);
            /*m_logFile = File.AppendText(m_path);
            m_logFile.WriteLine(value);
            m_logFile.Flush();
            m_logFile.Close();*/
        }
        public MapWindow.Interfaces.SelectInfo SelectInfo
        {
            get
            {
                return m_SelectInfo;
            }
            set
            {
                m_SelectInfo = value;
            }
        }


        public frmMergeShapes m_MergeForm;
    }
}
