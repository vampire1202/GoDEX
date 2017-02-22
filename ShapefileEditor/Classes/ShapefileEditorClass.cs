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
//1/29/2005 - minor changes over public domain version - dpa
//********************************************************************************************************

using System;
using Microsoft.JScript;
using System.Windows.Forms;

namespace ShapefileEditor 
{
	/// <summary>
	/// Summary description for ShapefileEditorClass.
	/// </summary>
	public class ShapefileEditorClass :  MapWindow.Interfaces.IPlugin
    {
        #region Class Variables
        private GlobalFunctions m_globals;
        private AddShapefileClass m_AddShapefileClass;
        private AddShapeClass m_AddShapeClass;
        private MoveShapeClass m_MoveShapeClass;
        // Chris Michaelis 12/21/2006
        private RotateShapeClass m_RotateShapeClass;
        // Chris Michaelis 12/28/2006
        private ResizeShapeClass m_ResizeShapeClass;
        private InsertStockShapeClass m_InsertStockShapeClass;
        private RemoveShapeClass m_RemoveShapeClass;
        private AddVertex m_AddVertex;
        private MoveVertexClass m_MoveVertex;
        private RemoveVertex m_RemoveVertex;

        private System.Collections.Stack m_addedButtons = new System.Collections.Stack();

        private System.Resources.ResourceManager res;

        #endregion 
        
        #region Plugin Information
        /// <summary>
        /// Date that the plugin was built.
        /// </summary>
        public string BuildDate
        {
            get
            {
                return System.IO.File.GetLastAccessTime(System.Reflection.Assembly.GetExecutingAssembly().Location).ToString();
            }
        }

        /// <summary>
        /// Description of the plugin.
        /// </summary>
        public string Description
        {
            get
            {
                return "Edit and create shapefiles and shape geometry.";
            }
        }

        /// <summary>
        /// Author of the plugin.
        /// </summary>
        public string Author
        {
            get
            {
                return "MapWindow Open Source Team, with significant contributions courtesy of StrateGis Groep (www.StrateGis.nl)";
            }
        }
        /// <summary>
        /// Name of the plugin.
        /// </summary>
        public string Name
        {
            get
            {
                return "Shapefile Editor";
            }
        }

        /// <summary>
        /// Version of the plugin.
        /// </summary>
        public string Version
        {
            get
            {
                System.Diagnostics.FileVersionInfo f = System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
                return f.FileMajorPart.ToString() + "." + f.FileMinorPart.ToString() + "." + f.FileBuildPart.ToString();
            }
        }

        /// <summary>
        /// Serial number of the plugin.
        /// The serial number and the name are tied together.  For each name there is a corresponding serial number.
        /// </summary>
        public string SerialNumber
        {
            get
            {
                return "OCGHOFNASNKGD1C";
            }
        }
        #endregion
      
        #region Initialize and Terminate
        /// <summary>
        /// Constructor:
        /// The ShapefileEditorClass implements the MapWindow.Interfaces.IPlugin interface.  This class handles all the direct interaction with the MapWindow.
        /// </summary>
        public ShapefileEditorClass()
        {

        }

        /// <summary>
        /// This event is called when a plugin is loaded or turned on in the MapWindow.
        /// </summary>
        /// <param name="MapWin">The interface to use to access the MapWindow.</param>
        /// <param name="ParentHandle">The window handle of the MapWindow form.  This handle is useful for making the MapWindow the owner of plugin forms.</param>
        public void Initialize(MapWindow.Interfaces.IMapWin MapWin, int ParentHandle)
        {
            m_globals = new GlobalFunctions(MapWin, (System.Windows.Forms.Form)System.Windows.Forms.Control.FromHandle(new System.IntPtr(ParentHandle)));
            m_globals.SetLogFile();

            res = new System.Resources.ResourceManager("ShapefileEditor.Resource", System.Reflection.Assembly.GetExecutingAssembly());

            MapWindow.Interfaces.Toolbar t = MapWin.Toolbar;

            t.AddToolbar(GlobalFunctions.c_ToolbarName);

            MapWindow.Interfaces.ToolbarButton b;
            if (GlobalFunctions.c_ToolbarName.Length > 0)
            {
                t.AddToolbar(GlobalFunctions.c_ToolbarName);
            }

            m_addedButtons.Clear();

            b = t.AddButton(GlobalFunctions.c_NewButton, GlobalFunctions.c_ToolbarName, "", "");
            b.BeginsGroup = true;
            b.Tooltip = res.GetString("tooltipCreateShp");
            b.Category = "Shapefile Editor";
            b.Picture = new System.Drawing.Icon(this.GetType(), "NewShapefile.ico");
            m_addedButtons.Push(GlobalFunctions.c_NewButton);

            // Chris Michaelis 12/28/2006
            b = t.AddButton(GlobalFunctions.c_InsertStockShapeButton, GlobalFunctions.c_ToolbarName, "", "");
            b.BeginsGroup = true;
            b.Tooltip = res.GetString("tooltipAddRegularShp"); 
            b.Category = "Shapefile Editor";
            b.Picture = new System.Drawing.Icon(this.GetType(), "InsertStockShape.ico");
            m_addedButtons.Push(GlobalFunctions.c_InsertStockShapeButton);

            b = t.AddButton(GlobalFunctions.c_AddShapeButton, GlobalFunctions.c_ToolbarName, "", "");
            b.BeginsGroup = true;
            b.Tooltip = res.GetString("tooltipAddShp"); 
            b.Category = "Shapefile Editor";
            b.Picture = new System.Drawing.Icon(this.GetType(), "AddShape.ico");
            m_addedButtons.Push(GlobalFunctions.c_AddShapeButton);

            b = t.AddButton(GlobalFunctions.c_RemoveShapeButton, GlobalFunctions.c_ToolbarName, "", "");
            b.Tooltip = res.GetString("tooltipRemoveShp"); 
            b.Category = "Shapefile Editor";
            b.Picture = new System.Drawing.Icon(this.GetType(), "RemoveShape.ico");
            m_addedButtons.Push(GlobalFunctions.c_RemoveShapeButton);

            b = t.AddButton(GlobalFunctions.c_CopyShapeButton, GlobalFunctions.c_ToolbarName, "", "");
            b.BeginsGroup = true;
            b.Tooltip = res.GetString("tooltipCopyShpToClipboard"); 
            b.Category = "Shapefile Editor";
            b.Picture = new System.Drawing.Icon(this.GetType(), "copy.ico");
            m_addedButtons.Push(GlobalFunctions.c_CopyShapeButton);

            b = t.AddButton(GlobalFunctions.c_PasteShapeButton, GlobalFunctions.c_ToolbarName, "", "");
            b.BeginsGroup = true;
            b.Tooltip = res.GetString("tooltipPasteShpFromClipboard"); 
            b.Category = "Shapefile Editor";
            b.Picture = new System.Drawing.Icon(this.GetType(), "paste.ico");
            m_addedButtons.Push(GlobalFunctions.c_PasteShapeButton);

            //
            // ARA 04/02/07
            //
            b = t.AddButton(GlobalFunctions.c_MergeShapesButton, GlobalFunctions.c_ToolbarName, "", "");
            b.BeginsGroup = true;
            b.Tooltip = res.GetString("tooltipMergeShp");
            b.Category = "Shapefile Editor";
            b.Picture = new System.Drawing.Icon(this.GetType(), "mergeShapes.ico");
            m_addedButtons.Push(GlobalFunctions.c_MergeShapesButton);

            b = t.AddButton(GlobalFunctions.c_EraseWithShapeButton, GlobalFunctions.c_ToolbarName, "", "");
            b.BeginsGroup = true;
            b.Tooltip = res.GetString("tooltipEraseLayerShp"); 
            b.Category = "Shapefile Editor";
            b.Picture = new System.Drawing.Icon(this.GetType(), "erase.ico");
            m_addedButtons.Push(GlobalFunctions.c_EraseWithShapeButton);

            b = t.AddButton(GlobalFunctions.c_EraseBeneathShapeButton, GlobalFunctions.c_ToolbarName, "", "");
            b.BeginsGroup = true;
            b.Tooltip = res.GetString("tooltipEraseLayerBeneathShp"); 
            b.Category = "Shapefile Editor";
            b.Picture = new System.Drawing.Icon(this.GetType(), "createIsland.ico");
            m_addedButtons.Push(GlobalFunctions.c_EraseBeneathShapeButton);

            ////////////////////////////////////////////////////////////////////////////////////////////////
            //  D.S. Modiwirijo 
            //  Moveshapes added.
            ////////////////////////////////////////////////////////////////////////////////////////////////
            b = t.AddButton(GlobalFunctions.c_MoveShapesButton, GlobalFunctions.c_ToolbarName, "", "");
            b.BeginsGroup = true;
            b.Tooltip = res.GetString("tooltipMoveShp"); 
            b.Category = "Shapefile Editor";
            b.Picture = new System.Drawing.Icon(this.GetType(), "Moveshapes.ico");
            m_addedButtons.Push(GlobalFunctions.c_MoveShapesButton);

            // Chris M 12/21/2006
            b = t.AddButton(GlobalFunctions.c_RotateShapeButton, GlobalFunctions.c_ToolbarName, "", "");
            b.BeginsGroup = true;
            b.Tooltip = res.GetString("tooltipRotateShp"); 
            b.Category = "Shapefile Editor";
            b.Picture = new System.Drawing.Icon(this.GetType(), "rotate.ico");
            m_addedButtons.Push(GlobalFunctions.c_RotateShapeButton);

            // Chris M 12/28/2006
            b = t.AddButton(GlobalFunctions.c_ResizeShapeButton, GlobalFunctions.c_ToolbarName, "", "");
            b.BeginsGroup = true;
            b.Tooltip = res.GetString("tooltipResizeShp");
            b.Category = "Shapefile Editor";
            b.Picture = new System.Drawing.Icon(this.GetType(), "resize.ico");
            m_addedButtons.Push(GlobalFunctions.c_ResizeShapeButton);

            b = t.AddButton(GlobalFunctions.c_MoveVertexButton, GlobalFunctions.c_ToolbarName, "", "");
            b.BeginsGroup = true;
            b.Tooltip = res.GetString("tooltipMoveVertex"); 
            b.Category = "Shapefile Editor";
            b.Picture = new System.Drawing.Icon(this.GetType(), "MoveVertex.ico");
            m_addedButtons.Push(GlobalFunctions.c_MoveVertexButton);

            b = t.AddButton(GlobalFunctions.c_AddVertexButton, GlobalFunctions.c_ToolbarName, "", "");
            b.Tooltip = res.GetString("tooltipAddVertex"); 
            b.Category = "Shapefile Editor";
            b.Picture = new System.Drawing.Icon(this.GetType(), "AddVertex.ico");
            m_addedButtons.Push(GlobalFunctions.c_AddVertexButton);

            b = t.AddButton(GlobalFunctions.c_RemoveVertexButton, GlobalFunctions.c_ToolbarName, "", "");
            b.Tooltip = res.GetString("tooltipRemoveVertex"); 
            b.Category = "Shapefile Editor";
            b.Picture = new System.Drawing.Icon(this.GetType(), "RemoveVertex.ico");
            m_addedButtons.Push(GlobalFunctions.c_RemoveVertexButton);

            b = t.AddButton(GlobalFunctions.c_CleanupButton, GlobalFunctions.c_ToolbarName, "", "");
            b.Tooltip = res.GetString("tooltipCheckCleanShp");
            b.Category = "Shapefile Editor";
            b.Picture = new System.Drawing.Icon(this.GetType(), "cleanupcheck.ico");
            m_addedButtons.Push(GlobalFunctions.c_CleanupButton);

            b = t.AddButton(GlobalFunctions.c_UndoDropdownButton, GlobalFunctions.c_ToolbarName, true);
            b.Text = "";
            b.Tooltip = res.GetString("tooltipUndo"); 
            b.Category = "Shapefile Editor";
            b.Picture = new System.Drawing.Icon(this.GetType(), "undo.ico");
            m_addedButtons.Push(GlobalFunctions.c_UndoDropdownButton);

            b = t.AddButton(GlobalFunctions.c_UndoLastChangeButton, GlobalFunctions.c_ToolbarName, GlobalFunctions.c_UndoDropdownButton, "");
            b.Tooltip = res.GetString("tooltipUndoLastChange"); 
            b.Text = b.Tooltip;
            b.Category = "Shapefile Editor";
            m_addedButtons.Push(GlobalFunctions.c_UndoLastChangeButton);

            b = t.AddButton(GlobalFunctions.c_UndoEnableButton, GlobalFunctions.c_ToolbarName, GlobalFunctions.c_UndoDropdownButton, "");
            b.Tooltip = res.GetString("tooltipEnableUndo");
            b.Text = b.Tooltip;
            b.Category = "Shapefile Editor";
            m_addedButtons.Push(GlobalFunctions.c_UndoEnableButton);

            b = t.AddButton(GlobalFunctions.c_UndoDisableButton, GlobalFunctions.c_ToolbarName, GlobalFunctions.c_UndoDropdownButton, "");
            b.Tooltip = res.GetString("tooltipDisableUndo"); 
            b.Text = b.Tooltip;
            b.Enabled = false;
            b.Category = "Shapefile Editor";
            m_addedButtons.Push(GlobalFunctions.c_UndoDisableButton);

            //MapWindow.Interfaces.ComboBoxItem c;
            //c = t.AddComboBox(GlobalFunctions.c_VertexSizeButton, GlobalFunctions.c_ToolbarName,"");
            //c.Items().AddRange(new object[]{1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20});
            //c.SelectedItem = 4;

            t = null;
            b = null;

            MapWindow.Interfaces.MenuItem i;
            MapWindow.Interfaces.Menus m = MapWin.Menus;

            i = m.AddMenu(GlobalFunctions.c_MenuName);
            i.Text = res.GetString("mnuShpEditor"); 
            i.Category = "Shapefile Editor";

            i = m.AddMenu(GlobalFunctions.c_ShowVerticesButton, GlobalFunctions.c_MenuName);
            i.Text = res.GetString("mnuShowVertices"); 
            i.Checked = false;
            i.Category = "Shapefile Editor";
            m_globals.MapWin.Menus[GlobalFunctions.c_ShowVerticesButton].Checked = true;

            i = m.AddMenu(GlobalFunctions.c_SnapToVerticesButton, GlobalFunctions.c_MenuName);
            i.Text = res.GetString("mnuSnapToVertices"); 
            i.Checked = true;
            i.Category = "Shapefile Editor";

            i = m.AddMenu(GlobalFunctions.c_SnapToAllLayersButton, GlobalFunctions.c_MenuName);
            i.Text = res.GetString("mnuSnapToAllLayers"); 
            i.Checked = false;
            i.Category = "Shapefile Editor";

            i = m.AddMenu(GlobalFunctions.c_StayInAddModeButton, GlobalFunctions.c_MenuName);
            i.Text = res.GetString("mnuStayInAddMode");
            i.Checked = false;
            i.Category = "Shapefile Editor";

            i = null;
            m = null;

            m_globals.SaveCusorMode();
            m_globals.DoEnables();

            if (m_globals.MapWin.Layers.NumLayers > 0)
                m_globals.UpdateCurrentLayer();

            m_AddShapefileClass = new AddShapefileClass(m_globals);
            m_AddShapeClass = new AddShapeClass(m_globals);
            m_MoveShapeClass = new MoveShapeClass(m_globals);
            m_RotateShapeClass = new RotateShapeClass(m_globals);
            m_ResizeShapeClass = new ResizeShapeClass(m_globals);
            m_InsertStockShapeClass = new InsertStockShapeClass(m_globals);
            m_RemoveShapeClass = new RemoveShapeClass(m_globals);
            m_AddVertex = new AddVertex(m_globals);
            m_MoveVertex = new MoveVertexClass(m_globals);
            m_RemoveVertex = new RemoveVertex(m_globals);

            m_globals.Events.FireInitializeEvent(MapWin, ParentHandle);
        }

        /// <summary>
        /// This method is called when a plugin is unloaded.  The plugin should remove all toolbars, buttons and menus that it added.
        /// </summary>
        public void Terminate()
        {
            m_globals.MapWin.View.CursorMode = MapWinGIS.tkCursorMode.cmPan;
            m_globals.Events.FireTerminateEvent();

            MapWindow.Interfaces.Toolbar t = m_globals.MapWin.Toolbar;

            while (m_addedButtons.Count > 0)
            {
                try
                {
                    t.RemoveButton((string)m_addedButtons.Pop());
                }
                catch
                { }
            }

            if (GlobalFunctions.c_ToolbarName.Length > 0)
                t.RemoveToolbar(GlobalFunctions.c_ToolbarName);

            MapWindow.Interfaces.Menus m = m_globals.MapWin.Menus;
            m.Remove(GlobalFunctions.c_ShowVerticesButton);
            m.Remove(GlobalFunctions.c_SnapToVerticesButton);
            m.Remove(GlobalFunctions.c_SnapToAllLayersButton);
            m.Remove(GlobalFunctions.c_SnapToGridButton);
            m.Remove(GlobalFunctions.c_SnapOptionsButton);
            m.Remove(GlobalFunctions.c_GridSnapButton);
            m.Remove(GlobalFunctions.c_MenuName);

            t = null;
            m = null;
            m_globals = null;
        }

        #endregion

        #region Implementation of IPlugin

        /// <summary>
        /// Occurs when a user clicks on a toolbar button or menu item.
        /// </summary>
        /// <param name="ItemName">The name of the item clicked on.</param>
        /// <param name="Handled">Reference parameter.  Setting Handled to true prevents other plugins from receiving this event.</param>
        public void ItemClicked(string ItemName, ref bool Handled)
        {
            try
            {
                // Chris M -- This is no longer necessary here as of MW 4.2, it's a little stricter on enforcing the button mode just clicked when coming out of a plug-in button mode
                //if (m_globals.CurrentMode != GlobalFunctions.Modes.None)
                //	m_globals.RestoreCursorMode();

                m_globals.CurrentMode = GlobalFunctions.Modes.None;
                m_globals.Events.FireItemClickedEvent(ItemName, ref Handled);
                m_globals.LogWrite("ItemClicked:" + ItemName);
                switch (ItemName)
                {
                    case GlobalFunctions.c_CopyShapeButton:
                        DoCopyShape();
                        Handled = true;
                        break;

                    case GlobalFunctions.c_PasteShapeButton:
                        DoPasteShape();
                        Handled = true;
                        break;
                    //ARA 04/03/07
                    case GlobalFunctions.c_MergeShapesButton:
                        DoMergeShape();
                        Handled = true;
                        break;
                    case GlobalFunctions.c_EraseWithShapeButton:
                        DoEraseWithShape(false);
                        Handled = true;
                        break;
                    case GlobalFunctions.c_EraseBeneathShapeButton:
                        DoEraseWithShape(true);
                        Handled = true;
                        break;

                    case GlobalFunctions.c_UndoDropdownButton:
                        // No action needed for outer dropdown button
                        Handled = true;
                        break;

                    case GlobalFunctions.c_UndoDisableButton:
                        m_globals.UndoStack.Clear();
                        m_globals.MapWin.Toolbar.ButtonItem(GlobalFunctions.c_UndoDisableButton).Enabled = false;
                        m_globals.MapWin.Toolbar.ButtonItem(GlobalFunctions.c_UndoLastChangeButton).Enabled = false;
                        m_globals.MapWin.Toolbar.ButtonItem(GlobalFunctions.c_UndoEnableButton).Enabled = true;
                        Handled = true;
                        break;

                    case GlobalFunctions.c_UndoEnableButton:
                        m_globals.UndoStack.Clear();
                        m_globals.MapWin.Toolbar.ButtonItem(GlobalFunctions.c_UndoDisableButton).Enabled = true;
                        m_globals.MapWin.Toolbar.ButtonItem(GlobalFunctions.c_UndoLastChangeButton).Enabled = false; // Disabled until something can be undone
                        m_globals.MapWin.Toolbar.ButtonItem(GlobalFunctions.c_UndoEnableButton).Enabled = false;
                        Handled = true;
                        break;

                    case GlobalFunctions.c_UndoLastChangeButton:
                        m_globals.UndoLastChange();
                        Handled = true;
                        break;

                    case GlobalFunctions.c_CleanupButton:
                        Forms.CleanupForm frm = new Forms.CleanupForm((MapWinGIS.Shapefile)m_globals.MapWin.Layers[m_globals.MapWin.Layers.CurrentLayer].GetObject(), m_globals);
                        frm.ShowDialog();

                        m_globals.UpdateView();
                        Handled = true;
                        break;

                    case GlobalFunctions.c_GridSnapButton:
                        Handled = true;
                        break;

                    case GlobalFunctions.c_ShowVerticesButton:
                        m_globals.MapWin.Menus[GlobalFunctions.c_ShowVerticesButton].Checked = !m_globals.MapWin.Menus[GlobalFunctions.c_ShowVerticesButton].Checked;

                        Handled = true;
                        break;
                    case GlobalFunctions.c_SnapToAllLayersButton:
                        m_globals.MapWin.Menus[GlobalFunctions.c_SnapToAllLayersButton].Checked = !m_globals.MapWin.Menus[GlobalFunctions.c_SnapToAllLayersButton].Checked;

                        Handled = true;
                        break;
                    case GlobalFunctions.c_SnapOptionsButton:
                        Handled = true;
                        break;
                    case GlobalFunctions.c_SnapToGridButton:
                        Handled = true;
                        break;
                    case GlobalFunctions.c_SnapToVerticesButton:
                        m_globals.MapWin.Menus[GlobalFunctions.c_SnapToVerticesButton].Checked = !m_globals.MapWin.Menus[GlobalFunctions.c_SnapToVerticesButton].Checked;

                        Handled = true;
                        break;
                    case GlobalFunctions.c_VertexSizeButton:
                        Handled = true;
                        break;

                    case GlobalFunctions.c_StayInAddModeButton:
                        Handled = true;
                        GlobalFunctions.m_StayInAddMode = !m_globals.MapWin.Menus[GlobalFunctions.c_StayInAddModeButton].Checked;
                        m_globals.MapWin.Menus[GlobalFunctions.c_StayInAddModeButton].Checked = GlobalFunctions.m_StayInAddMode;
                        if (!GlobalFunctions.m_StayInAddMode)
                            if (m_AddShapeClass != null) m_AddShapeClass.CloseAddShapeForm();
                        break;
                }
            }
            catch (System.Exception ex)
            {
                m_globals.MapWin.ShowErrorDialog(ex);
            }
        }

        #region Map Events
        /// <summary>
        /// Occurs when shapes have been selected in the MapWindow.
        /// </summary>
        /// <param name="Handle">The handle of the layer that was selected on.</param>
        /// <param name="SelectInfo">Information about all the shapes that were selected.</param>
        public void ShapesSelected(int Handle, MapWindow.Interfaces.SelectInfo SelectInfo)
        {
            m_globals.Events.FireShapesSelectedEvent(Handle, SelectInfo);
            m_globals.DoEnables();
            // TODO:
            m_globals.Events.FireLayersClearedEvent();


        }

        /// <summary>
        /// Method that is called when the MapWindow extents change.
        /// </summary>
        public void MapExtentsChanged()
        {
            m_globals.Events.FireMapExtentsChangedEvent();
        }

        /// <summary>
        /// Occurs when the user presses a mouse button on the MapWindow map display.
        /// </summary>
        /// <param name="Button">The integer representation of the button pressed by the user.  This parameter uses the vb6 mouse button constants (vbLeftButton, etc.).</param>
        /// <param name="Shift">The integer representation of which shift/alt/control keys are pressed. This parameter uses the vb6 shift constants.</param>
        /// <param name="x">X coordinate in pixels.</param>
        /// <param name="y">Y coordinate in pixels.</param>
        /// <param name="Handled">Reference parameter.  When set to true, no other plugins will receive this event.</param>
        public void MapMouseDown(int Button, int Shift, int x, int y, ref bool Handled)
        {
            m_globals.Events.FireMapMouseDownEvent(Button, Shift, x, y, ref Handled);
            ////m_globals.LogWrite("m_globals.CurrentMode = " + m_globals.CurrentMode.ToString());


        }
        
        /// <summary>
        /// Occurs after a user selects a rectangular area in the MapWindow.  Normally this implies selection.
        /// </summary>
        /// <param name="Bounds">The rectangle selected.</param>
        /// <param name="Handled">Reference parameter.  Setting Handled to true prevents other plugins from receiving this event.</param>
        public void MapDragFinished(System.Drawing.Rectangle Bounds, ref bool Handled)
        {
            m_globals.Events.FireMapDragFinishedEvent(Bounds, ref Handled);
        }

        /// <summary>
        /// Occurs when a user releases a mouse button on the MapWindow main map display.
        /// </summary>
        /// <param name="Button">An integer representation of which button(s) were released.  Uses vb6 button constants.</param>
        /// <param name="Shift">An integer representation of the shift/alt/ctrl keys that were pressed at the time the mouse button was released.  Uses vb6 shift constants.</param>
        /// <param name="x">X coordinate in pixels.</param>
        /// <param name="y">Y coordinate in pixels.</param>
        /// <param name="Handled">Reference parameter.  Prevents other plugins from getting this event.</param>
        public void MapMouseUp(int Button, int Shift, int x, int y, ref bool Handled)
        {
            ////m_blnMousedown = false;
            ////m_globals.LogWrite("MapMouseUp");

            m_globals.Events.FireMapMouseUpEvent(Button, Shift, x, y, ref Handled);

            try
            {
                switch (m_globals.CurrentMode)
                {
                    // TODO:
                    case GlobalFunctions.Modes.AddShape:
                        //						if (Button == (int)MapWindow.Interfaces.vb6Buttons.Left)
                        //						{
                        //							// Add this point to the current shape.
                        //							double mapX = 0, mapY = 0;
                        //							m_globals.MapWin.View.PixelToProj(x,y,ref mapX, ref mapY);
                        //							m_AddShapeForm.AddPoint(mapX, mapY);
                        //						}
                        //						else if(Button == (int)MapWindow.Interfaces.vb6Buttons.Right)
                        //						{
                        //							// Finish this shape (this really only applies to polygon/line shapes but should work with point shapes too).
                        //							m_AddShapeForm.Close();
                        //							MapWinGIS.Shape shp = m_AddShapeForm.GetNewShape;
                        //							if (m_CurrentShapefile != null)
                        //							{
                        //								if(m_CurrentShapefile.EditingShapes == false)
                        //									m_CurrentShapefile.StartEditingShapes(false,null);
                        //							
                        //								int newIndex = m_CurrentShapefile.NumShapes;
                        //								m_CurrentShapefile.EditInsertShape(shp, ref newIndex);
                        //							}		
                        //							else // the current layer got changed without me knowing somehow.  Set the current mode to none.
                        //							{
                        //								m_globals.CurrentMode = GlobalFunctions.Modes.None;
                        //								m_globals.DoEnables();
                        //							}
                        //						}
                        break;
                    // end case Modes.AddShape:

                    default:
                        break;
                }
            }
            catch
            {
                m_globals.CurrentMode = GlobalFunctions.Modes.None;
                m_globals.DoEnables();
            }
        }
        
        /// <summary>
        /// Occurs when a user moves the mouse over the MapWindow main display.
        /// </summary>
        /// <param name="ScreenX">X coordinate in pixels.</param>
        /// <param name="ScreenY">Y coordinate in pixels.</param>
        /// <param name="Handled">Reference parameter.  Prevents other plugins from getting this event.</param>
        public void MapMouseMove(int ScreenX, int ScreenY, ref bool Handled)
        {
            ////if (m_globals.CurrentLayer != null && m_blnMoveshapes && m_blnMousedown)
            ////{
            //    ////MovePolygons(ScreenX, ScreenY);
            //    ////m_dblInitX = m_dblDestinyX;
            //    ////m_dblInitY = m_dblDestinyY;
            ////}			
            m_globals.Events.FireMapMouseMoveEvent(ScreenX, ScreenY, ref Handled);

        }

        #endregion

        #region Legend Events

        /// <summary>
        /// Occurs when a user double clicks on the legend.
        /// </summary>
        /// <param name="Handle">The handle of the legend group or item that was clicked on.</param>
        /// <param name="Location">Enumerated.  The location clicked on.</param>
        /// <param name="Handled">Reference parameter.  When set to true it prevents additional plugins from getting this event.</param>
        public void LegendDoubleClick(int Handle, MapWindow.Interfaces.ClickLocation Location, ref bool Handled)
        {
            m_globals.Events.FireLegendDoubleClickEvent(Handle, Location, ref Handled);
        }

        /// <summary>
        /// Occurs when a user presses a mouse button on the legend.
        /// </summary>
        /// <param name="Handle">Layer or group handle that was clicked.</param>
        /// <param name="Button">The integer representation of the button used.  Uses vb6 mouse button constants.</param>
        /// <param name="Location">The part of the legend that was clicked.</param>
        /// <param name="Handled">Reference parameter.  Prevents other plugins from getting this event when set to true.</param>
        public void LegendMouseDown(int Handle, int Button, MapWindow.Interfaces.ClickLocation Location, ref bool Handled)
        {
            m_globals.Events.FireLegendMouseDownEvent(Handle, Button, Location, ref Handled);
        }

        /// <summary>
        /// Occurs when the user releases a mouse button over the legend.
        /// </summary>
        /// <param name="Handle">The handle of the group or layer.</param>
        /// <param name="Button">The integer representation of the button released.  Uses vb6 button constants.</param>
        /// <param name="Location">Enumeration.  Specifies if a group, layer or neither was clicked on.</param>
        /// <param name="Handled">Reference parameter.  Prevents other plugins from getting this event.</param>
        public void LegendMouseUp(int Handle, int Button, MapWindow.Interfaces.ClickLocation Location, ref bool Handled)
        {
            m_globals.Events.FireLegendMouseUpEvent(Handle, Button, Location, ref Handled);
        }

        #endregion

        #region Layer Events

        /// <summary>
        /// Called when 1 or more layers are added to the MapWindow.
        /// </summary>
        /// <param name="Layers">Array of layer objects containing references to all layers added.</param>
        public void LayersAdded(MapWindow.Interfaces.Layer[] Layers)
        {
            m_globals.Events.FireLayersAddedEvent(Layers);
        }

        /// <summary>
        /// Called when a layer is selected or made to be the active layer.
        /// </summary>
        /// <param name="Handle">The layer handle of the newly selected layer.</param>
        public void LayerSelected(int Handle)
        {
            m_globals.UpdateCurrentLayer();

            m_globals.RestoreCursorMode();
            m_globals.CurrentMode = GlobalFunctions.Modes.None;
            m_globals.DoEnables();
            m_globals.UpdateButtons();
            //			m_globals.Events.FireLayerSelectedEvent(Handle);
        }

        /// <summary>
        /// Occurs when a layer is removed from the MapWindow.
        /// </summary>
        /// <param name="Handle">The handle of the layer being removed.</param>
        public void LayerRemoved(int Handle)
        {
            m_globals.RestoreCursorMode();
            m_globals.CurrentMode = GlobalFunctions.Modes.None;
            m_globals.DoEnables();
            m_globals.UpdateButtons();
            m_globals.Events.FireLayerRemovedEvent(Handle);
        }

        /// <summary>
        /// Occurs when the "Clear all layers" button is pressed in the MapWindow.
        /// </summary>
        public void LayersCleared()
        {
            m_globals.RestoreCursorMode();
            m_globals.CurrentMode = GlobalFunctions.Modes.None;
            m_globals.DoEnables();
            m_globals.UpdateButtons();
            m_globals.Events.FireLayersClearedEvent();
        }

        #endregion

        #region Project Events
        /// <summary>
        /// Called when the MapWindow loads a new project.
        /// </summary>
        /// <param name="ProjectFile">The filename of the project file being loaded.</param>
        /// <param name="SettingsString">The settings string that was saved in the project file for this plugin.</param>
        public void ProjectLoading(string ProjectFile, string SettingsString)
        {
            m_globals.RestoreCursorMode();
            m_globals.CurrentMode = GlobalFunctions.Modes.None;
            m_globals.DoEnables();
            m_globals.UpdateButtons();
            m_globals.Events.FireProjectLoadingEvent(ProjectFile, SettingsString);
        }

        /// <summary>
        /// ProjectSaving is called when the MapWindow saves a project.  This is a good chance for the plugin to save any custom settings and create a SettingsString to place in the project file.
        /// </summary>
        /// <param name="ProjectFile">The name of the project file being saved.</param>
        /// <param name="SettingsString">Reference parameter.  The settings string that will be saved in the project file for this plugin.</param>
        public void ProjectSaving(string ProjectFile, ref string SettingsString)
        {
            m_globals.Events.FireProjectSavingEvent(ProjectFile, ref SettingsString);
        }
        #endregion
        
        /// <summary>
		/// Method used by plugins to communicate with other plugins.
		/// </summary>
		/// <param name="msg">The messsage being recieved.</param>
		/// <param name="Handled">Reference parameter.  Set thist to true if this plugin handles recieving the message.  When set to true, no other plugins will receive the message.</param>	
        public void Message(string msg, ref bool Handled)
		{
			if (m_globals != null) m_globals.Events.FireMessageEvent(msg, ref Handled);
		}

		#endregion

        #region Do Functions

        private void DoMergeShape()
        {
            m_globals.CreateUndoPoint();
            m_globals.m_MergeForm = new frmMergeShapes();

            m_globals.m_MergeForm.pluginClass = this;
            m_globals.m_MergeForm.Show();

            m_globals.MapWin.View.CursorMode = MapWinGIS.tkCursorMode.cmSelection;
        }

        private void DoEraseWithShape(bool KeepSelectedShape)
        {
            MapWinGIS.Shapefile sf = new MapWinGIS.Shapefile(); ;
            MapWinGIS.Shapefile tmpSF = new MapWinGIS.Shapefile();
            MapWinGIS.Shape currSelected;
            MapWindow.Interfaces.Layer currLyr;
            MapWinGIS.Extents currExt;
            MapWinGIS.ShapefileColorScheme currCS;
            string sfname, tmpsfName, tmpLyrProp, lyrName, projstr;
            int lyrGlobalPos, lyrGroupHandle, lyrGroupPos, currSelectedIdx, numShapes;
            System.Collections.ArrayList cellVals = new System.Collections.ArrayList();
            bool undoState = m_globals.MapWin.Toolbar.ButtonItem(GlobalFunctions.c_UndoDisableButton).Enabled;

            m_globals.CreateUndoPoint();

            currExt = m_globals.MapWin.View.Extents;
            currLyr = m_globals.MapWin.Layers[m_globals.MapWin.Layers.CurrentLayer];
            lyrName = currLyr.Name;
            sfname = currLyr.FileName;
            tmpsfName = System.IO.Path.ChangeExtension(sfname, "tmp.shp");
            tmpLyrProp = System.IO.Path.ChangeExtension(sfname, "temp.mwsr");

            lyrGlobalPos = currLyr.GlobalPosition;
            lyrGroupHandle = currLyr.GroupHandle;
            lyrGroupPos = currLyr.GroupPosition;
            currCS = (MapWinGIS.ShapefileColorScheme)currLyr.ColoringScheme;
            currLyr.SaveShapeLayerProps(tmpLyrProp);

            sf.Open(sfname, null);
            numShapes = sf.NumShapes;
            projstr = sf.Projection;
            currSelectedIdx = m_globals.MapWin.View.SelectedShapes[0].ShapeIndex;
            currSelected = sf.get_Shape(currSelectedIdx);

            if (KeepSelectedShape)
            {
                cellVals.Clear();
                for (int i = 0; i < sf.NumFields; i++)
                {
                    cellVals.Add(sf.get_CellValue(i, currSelectedIdx));
                }
            }

            m_globals.MapWin.View.LockMap();
            m_globals.MapWin.View.LockLegend();
            m_globals.MapWin.Layers.Remove(currLyr.Handle);

            if (MapWinGeoProc.SpatialOperations.Erase(ref sfname, ref currSelected, ref tmpsfName, true, true))
            {
                sf.Close();
                MapWinGeoProc.DataManagement.DeleteShapefile(ref sfname);
                tmpSF.Open(tmpsfName, null);
                tmpSF.Projection = projstr;
    
                tmpSF.StartEditingShapes(true, null);
                //tmpSF.EditDeleteShape(0);
                if (KeepSelectedShape)
                {
                    currSelectedIdx = tmpSF.NumShapes;
                    tmpSF.EditInsertShape(currSelected, ref currSelectedIdx);
                    for (int i = 0; i < tmpSF.NumFields; i++)
                    {
                        tmpSF.EditCellValue(i, currSelectedIdx, cellVals[i]);
                    }
                }
    
                tmpSF.StopEditingShapes(true, true, null);
    
                if (!tmpSF.SaveAs(sfname, null))
                {
                    MapWinUtility.Logger.Message("Saving the temporary file failed.", "Erase With Current Shape", System.Windows.Forms.MessageBoxButtons.OK, MessageBoxIcon.Error, DialogResult.OK);
                }
                tmpSF.Close();
                MapWinGeoProc.DataManagement.DeleteShapefile(ref tmpsfName);
            }
            else
            {
                MapWinUtility.Logger.Message("The erase function failed.", "Erase With Current Shape", System.Windows.Forms.MessageBoxButtons.OK, MessageBoxIcon.Error, DialogResult.OK);
            }

            currLyr = m_globals.MapWin.Layers.Add(sfname, lyrName);
            currLyr.LoadShapeLayerProps(tmpLyrProp);
            currLyr.GlobalPosition = lyrGlobalPos;
            currLyr.GroupHandle = lyrGroupHandle;
            currLyr.GroupPosition = lyrGroupPos;
            currLyr.ColoringScheme = currCS;
            System.IO.File.Delete(tmpLyrProp);

            if (undoState == true)
            {
                m_globals.MapWin.Toolbar.ButtonItem(GlobalFunctions.c_UndoDisableButton).Enabled = true;
                m_globals.MapWin.Toolbar.ButtonItem(GlobalFunctions.c_UndoEnableButton).Enabled = false;
            }

            m_globals.MapWin.View.Extents = currExt;
            m_globals.MapWin.View.UnlockMap();
            m_globals.MapWin.View.UnlockLegend();
            m_globals.MapWin.View.Redraw();
        }

        private void DoCopyShape()
        {
            string copystring = "MWShapeCopy:";
            for (int i = 0; i < m_globals.MapWin.View.SelectedShapes.NumSelected; i++)
            {
                // Paul Meems
                // Why get the shapefile for every selected shape?
                // It is always the same layer so alwaus the same shapefile.
                MapWinGIS.Shapefile sf = (MapWinGIS.Shapefile)m_globals.MapWin.Layers[m_globals.MapWin.Layers.CurrentLayer].GetObject();
                copystring += sf.get_Shape(m_globals.MapWin.View.SelectedShapes[i].ShapeIndex).SerializeToString();

                copystring += "FIELDS||";
                for (int j = 0; j < sf.NumFields; j++)
                {
                    copystring += sf.get_Field(j).Name + "|" + sf.get_CellValue(j, m_globals.MapWin.View.SelectedShapes[i].ShapeIndex).ToString() + "||";
                }

                copystring += "\n";
            }

            System.Windows.Forms.Clipboard.SetText(copystring);
        }

        private void DoPasteShape()
        {
            string cptext = System.Windows.Forms.Clipboard.GetText();
            if (!cptext.StartsWith("MWShapeCopy:"))
            {
                return;
            }

            // Paul Meems, 26 Oct. 2009, fix for bug #1460
            // Added check for in-memory shapefiles, those have no filename:
            if (!m_globals.MapWin.Layers.IsValidHandle(m_globals.MapWin.Layers.CurrentLayer)) return;
            // End modifications Paul Meems, 26 Oct. 2009

            cptext = cptext.Replace("MWShapeCopy:", "");
            string[] shapes = cptext.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            System.Collections.ArrayList added = new System.Collections.ArrayList();
            if (shapes.Length > 0)
            {
                MapWinGIS.Shapefile sf = (MapWinGIS.Shapefile)m_globals.MapWin.Layers[m_globals.MapWin.Layers.CurrentLayer].GetObject();
                sf.StartEditingShapes(true, null);

                foreach (string s in shapes)
                {
                    string shpgeom = s.Substring(0, s.IndexOf("FIELDS||"));
                    string[] fieldvalues = s.Substring(s.IndexOf("FIELDS||") + 8).Split(new string[] { "||" }, StringSplitOptions.None);

                    MapWinGIS.Shape newShp = new MapWinGIS.Shape();
                    newShp.CreateFromString(shpgeom);

                    if (newShp.ShapeType != sf.ShapefileType)
                    {
                        MapWinUtility.Logger.Message("Warning: Skipping a pasted shape because it is not of the same tye of the shapefile being edited (e.g., line versus polygon).", "Skipping Shape", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information, DialogResult.OK);
                        continue;
                    }

                    // If shape is entirely in current view, shift ths shape slightly so
                    // it's apparent that it was added
                    // If out of current bounds, move it so at least part is in current bounds
                    if (!OutOfView(m_globals.MapWin.View.Extents, newShp.Extents))
                    {
                        double shiftX = (newShp.Extents.xMax - newShp.Extents.xMin) / 6;
                        double shiftY = (newShp.Extents.yMax - newShp.Extents.yMin) / 6;
                        for (int i = 0; i < newShp.numPoints; i++)
                        {
                            newShp.get_Point(i).x += shiftX;
                        }
                    }
                    else
                    {
                        while (OutOfView(m_globals.MapWin.View.Extents, newShp.Extents))
                        {
                            if (OutOfViewX(m_globals.MapWin.View.Extents, newShp.Extents) == -1)
                            {
                                double shiftX = (newShp.Extents.xMax - newShp.Extents.xMin);
                                for (int i = 0; i < newShp.numPoints; i++)
                                    newShp.get_Point(i).x -= shiftX;
                            }
                            else if (OutOfViewX(m_globals.MapWin.View.Extents, newShp.Extents) == +1)
                            {
                                double shiftX = (newShp.Extents.xMax - newShp.Extents.xMin);
                                for (int i = 0; i < newShp.numPoints; i++)
                                    newShp.get_Point(i).x += shiftX;
                            }
                            if (OutOfViewY(m_globals.MapWin.View.Extents, newShp.Extents) == -1)
                            {
                                double shiftY = (newShp.Extents.yMax - newShp.Extents.yMin);
                                for (int i = 0; i < newShp.numPoints; i++)
                                    newShp.get_Point(i).y -= shiftY;
                            }
                            else if (OutOfViewY(m_globals.MapWin.View.Extents, newShp.Extents) == +1)
                            {
                                double shiftY = (newShp.Extents.yMax - newShp.Extents.yMin);
                                for (int i = 0; i < newShp.numPoints; i++)
                                    newShp.get_Point(i).y += shiftY;
                            }
                        }

                        // If we adjusted it into view, and if the shape's total
                        // width and height is less than the view's,
                        // try to wiggle it completely into the view
                        if ((m_globals.MapWin.View.Extents.xMax - m_globals.MapWin.View.Extents.xMin) > (newShp.Extents.xMax - newShp.Extents.xMin) && (m_globals.MapWin.View.Extents.yMax - m_globals.MapWin.View.Extents.yMin) > (newShp.Extents.yMax - newShp.Extents.yMin))
                        {
                            // But give up before we make user wait too long, if hard to fit into view
                            int MaxReps = 200;
                            int CurrReps = 0;
                            while (CurrReps < MaxReps && !ExtentsFullyContained(m_globals.MapWin.View.Extents, newShp.Extents))
                            {
                                CurrReps++;
                                if (ExtentsFullyContainedX(m_globals.MapWin.View.Extents, newShp.Extents) == -1)
                                {
                                    double shiftX = (newShp.Extents.xMax - newShp.Extents.xMin) / 20;
                                    for (int i = 0; i < newShp.numPoints; i++)
                                        newShp.get_Point(i).x -= shiftX;
                                }
                                else if (ExtentsFullyContainedX(m_globals.MapWin.View.Extents, newShp.Extents) == +1)
                                {
                                    double shiftX = (newShp.Extents.xMax - newShp.Extents.xMin) / 20;
                                    for (int i = 0; i < newShp.numPoints; i++)
                                        newShp.get_Point(i).x += shiftX;
                                }
                                if (ExtentsFullyContainedY(m_globals.MapWin.View.Extents, newShp.Extents) == -1)
                                {
                                    double shiftY = (newShp.Extents.yMax - newShp.Extents.yMin) / 20;
                                    for (int i = 0; i < newShp.numPoints; i++)
                                        newShp.get_Point(i).y -= shiftY;
                                }
                                else if (ExtentsFullyContainedY(m_globals.MapWin.View.Extents, newShp.Extents) == +1)
                                {
                                    double shiftY = (newShp.Extents.yMax - newShp.Extents.yMin) / 20;
                                    for (int i = 0; i < newShp.numPoints; i++)
                                        newShp.get_Point(i).y += shiftY;
                                }
                            }
                        }
                    }

                    // Done, insert.
                    int shpindex = sf.NumShapes;
                    sf.EditInsertShape(newShp, ref shpindex);
                    added.Add(shpindex);

                    // And add field values:
                    for (int f = 0; f < fieldvalues.Length; f++)
                    {
                        string[] fieldvaluepair = fieldvalues[f].Split(new char[] { '|' }, StringSplitOptions.None);
                        if (fieldvaluepair.Length == 2)
                        {
                            if (fieldvaluepair[0].ToLower() != "mwshapeid" && fieldvaluepair[0].ToLower() != "id")
                            {
                                int fldIdx = FindField(fieldvaluepair[0], ref sf);
                                if (fldIdx != -1)
                                    sf.EditCellValue(fldIdx, shpindex, fieldvaluepair[1]);
                            }
                        }
                    }
                }

                m_globals.CreateUndoPoint();

                sf.StopEditingShapes(true, true, null);

                m_globals.UpdateView();

                // Select the added one(s) to be moved easily (or deleted etc)
                m_globals.MapWin.View.SelectedShapes.ClearSelectedShapes();
                for (int i = 0; i < added.Count; i++)
                    m_globals.MapWin.View.SelectedShapes.AddByIndex((int)added[i], m_globals.MapWin.View.SelectColor);

                m_globals.MapWin.Plugins.BroadcastMessage("ShapefileEditor: Layer " + m_globals.MapWin.Layers.CurrentLayer.ToString() + ": New Shape Added");
            }
        }

#endregion
        
        #region Helper Functions

        public int FindField(string Name, ref MapWinGIS.Shapefile sf)
        {
            for (int z = 0; z < sf.NumFields; z++)
                if (sf.get_Field(z).Name.ToLower() == Name.ToLower()) return z;

            return -1;
        }

        public static bool ExtentsFullyContained(MapWinGIS.Extents Container, MapWinGIS.Extents Contained)
        {
            return (Contained.xMin >= Container.xMin && Contained.yMin >= Container.yMin && Contained.xMax <= Container.xMax && Contained.yMax <= Container.yMax);
        }

        public static int ExtentsFullyContainedX(MapWinGIS.Extents Container, MapWinGIS.Extents Contained)
        {
            if (Contained.xMin >= Container.xMin && Contained.yMin >= Container.yMin && Contained.xMax <= Container.xMax && Contained.yMax <= Container.yMax) return 0;

            if (Contained.xMin >= Container.xMin)
                return -1;
            else
                return +1;
        }
        public static int ExtentsFullyContainedY(MapWinGIS.Extents Container, MapWinGIS.Extents Contained)
        {
            if (Contained.yMin >= Container.yMin && Contained.yMin >= Container.yMin && Contained.yMax <= Container.yMax && Contained.yMax <= Container.yMax) return 0;

            if (Contained.yMin >= Container.yMin)
                return -1;
            else
                return +1;
        }

        public static bool OutOfView(MapWinGIS.Extents View, MapWinGIS.Extents Consider)
        {
            return ((Consider.xMin > View.xMax || Consider.xMax < View.xMin) || (Consider.yMin > View.yMax || Consider.yMax < View.yMin) ? true : false);
        }

        public static int OutOfViewY(MapWinGIS.Extents View, MapWinGIS.Extents Consider)
        {
            if (Consider.yMin > View.yMax)
                return -1;

            if (Consider.yMax < View.yMin)
                return +1;

            return 0;
        }

        public static int OutOfViewX(MapWinGIS.Extents View, MapWinGIS.Extents Consider)
        {
            if (Consider.xMin > View.xMax)
                return -1;

            if (Consider.xMax < View.xMin)
                return +1;

            return 0;
        }

        public bool MergeShapes()
        {
            MapWindow.Interfaces.SelectInfo lstSelected = m_globals.MapWin.View.SelectedShapes;
            if (lstSelected.NumSelected < 2)
            {
                MapWinUtility.Logger.Message("You must select at least two shapes to merge.", "Merge Shapes", System.Windows.Forms.MessageBoxButtons.OK, MessageBoxIcon.Information, DialogResult.OK);
                return false;
            }
            else
            {
                // Paul Meems, 26 Oct. 2009, fix for bug #1460
                // Added check for in-memory shapefiles, those have no filename:
                if (!m_globals.MapWin.Layers.IsValidHandle(m_globals.MapWin.Layers.CurrentLayer)) return false;
                // End modifications Paul Meems, 26 Oct. 2009

                MapWinGIS.Shapefile sf = new MapWinGIS.Shapefile();
                MapWinGIS.Shape mergeShape, currShape;
                System.Collections.SortedList sl = new System.Collections.SortedList();
                string sfname;
                sf = (MapWinGIS.Shapefile) m_globals.MapWin.Layers[m_globals.MapWin.Layers.CurrentLayer].GetObject();
                sfname = sf.Filename;

                mergeShape = sf.get_Shape(lstSelected[0].ShapeIndex);
                sl.Add(lstSelected[0].ShapeIndex, lstSelected[0].ShapeIndex);

                for (int i = 1; i < lstSelected.NumSelected; i++)
                {
                    sl.Add(lstSelected[i].ShapeIndex, lstSelected[i].ShapeIndex);
                    
                    currShape = sf.get_Shape(lstSelected[i].ShapeIndex);

                    if (!MapWinGeoProc.SpatialOperations.MergeShapes(ref mergeShape, ref currShape, out mergeShape))
                    {
                        MapWinUtility.Logger.Message("Merge Failed.", "Merge Shapes", MessageBoxButtons.OK, MessageBoxIcon.Error, DialogResult.OK);
                        sf.Close();
                        return false;
                    }
                }
                sf.StartEditingShapes(true, null);
                int idxMerged = sf.NumShapes;
                sf.EditInsertShape(mergeShape, ref idxMerged);

                for (int i = 0; i < sf.NumFields; i++)
                {
                    sf.EditCellValue(i, idxMerged, sf.get_CellValue(i, lstSelected[0].ShapeIndex));
                }
                m_globals.MapWin.View.ClearSelectedShapes();

                for (int i = sl.Count - 1; i >= 0; i--)
                {
                    sf.EditDeleteShape((int)sl.GetByIndex(i));
                }
                sf.StopEditingShapes(true, true, null);

                m_globals.MapWin.Plugins.BroadcastMessage("ShapefileEditor: Layer " + m_globals.MapWin.Layers.CurrentLayer.ToString() + ": Shapes Merged");

                m_globals.m_MergeForm.Close();
                m_globals.MapWin.View.LockMap();
                m_globals.MapWin.View.LockLegend();
                sf.Close();
                sf.Open(sfname, null);
                m_globals.MapWin.View.UnlockMap();
                m_globals.MapWin.View.UnlockLegend();
                m_globals.MapWin.View.Redraw();
            }

            return true;
        }
        #endregion
    }
}
