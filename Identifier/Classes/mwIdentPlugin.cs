//********************************************************************************************************
//The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
//you may not use this file except in compliance with the License. You may obtain a copy of the License at 
//http://www.mozilla.org/MPL/ 
//Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
//ANY KIND, either express or implied. See the License for the specificlanguage governing rights and 
//limitations under the License. 
//
//The Original Code is MapWindow Identifier Plug-in. 
//
//The Initial Developer of this version of the Original Code is Daniel P. Ames using portions created by 
//Utah State University and the Idaho National Engineering and Environmental Lab that were released as 
//public domain in March 2004.  
//
//Contributor(s): (Open source contributors should list themselves and their modifications here). 
//2/1/2005 - jlk - allow toolbar to be added to MapWindow main toolbar
//9/29/2005 - dpa - removed the labeler and placed it in a separate plug-in.
//					also renamed this just "Identifier" since it identifies both features and grids.
//6/5/2006 - Chris Michaelis - Changed this plug-in to use the MapWindow UIPanel w/ pushpin icons
//********************************************************************************************************

using System;
using System.Windows.Forms;	
using MapWinGIS;

namespace mwIdentifier
{	
	public struct Bounds
	{
		public int x1;
		public int y1;
		public int x2;
		public int y2;
	}

    public class mwIdentPlugin : MapWindow.Interfaces.IPlugin
    {
        //Internationalization added 11/1/2005 Lailin Chen
        private System.Resources.ResourceManager resMan = new System.Resources.ResourceManager("mwIdentifier.Resource", System.Reflection.Assembly.GetExecutingAssembly());

        public MapWindow.Interfaces.IMapWin m_MapWin;
        public bool m_Activated;
        private bool m_OnWMSLayer;
        private string m_WMSLayerIDReturn;
        public mwIdentifier.Forms.frmGridProp m_GridPropfrm;
        public mwIdentifier.Forms.frmShapeFileProp m_shpFilePropfrm;
        public mwIdentifier.Forms.frmWMSProp m_WMSPropfrm;
        public MapWinGIS.tkCursorMode m_PreviousCursorMode;
        public MapWinGIS.tkCursor m_PreviousCursor;
        public bool m_MouseDown;
        private MapWindow.Interfaces.ToolbarButton m_IdentBtn;
        private MapWindow.Interfaces.ToolbarButton m_IdentBtn2;
        private MapWindow.Interfaces.ToolbarButton m_IdentByShape;
        private Forms.frmIdentByShape identbyshapeForm;
        public int m_hDraw;
        public int m_ParentHandle;
        private Cursor m_Cursor;
        private Bounds m_Bounds;
        private System.Drawing.Color YELLOW = System.Drawing.Color.Yellow;
        private System.Drawing.Color RED = System.Drawing.Color.Red;
        private string m_ToolbarName = "";
        public bool m_HavePanel = false;
        private Panel m_UIPanel = null;
        private string m_UIPanel_LastType = "";

        //Constructor
        public mwIdentPlugin()
        {
            try
            {
                Deactivate();
                //m_Cursor = new Cursor(this.GetType(), "ico_help.ico");
                m_Cursor = new Cursor(this.GetType(), "Ident10262006_2.ico");
            }
            catch (System.Exception ex)
            {
                ShowErrorBox("mwIdentPlugin()", ex.Message);
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
                return "Identifier for raster and vector data.";
            }
        }

        public string Name
        {
            get
            {
                return resMan.GetString("mwIdentifier.Name");
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

                m_shpFilePropfrm = new mwIdentifier.Forms.frmShapeFileProp(this);
                m_GridPropfrm = new mwIdentifier.Forms.frmGridProp(this);
                m_WMSPropfrm = new mwIdentifier.Forms.frmWMSProp(this);

                System.Drawing.Icon icon;
                System.Drawing.Icon icon2;
                icon = m_shpFilePropfrm.Icon;
                icon2 = new System.Drawing.Icon(this.GetType(), "IdentByShape.ico");
                MapWindow.Interfaces.ToolbarButton button;

                //Add the tool bar
                if (m_ToolbarName.Length > 0)
                    m_MapWin.Toolbar.AddToolbar(m_ToolbarName);

                //add buttons and the tool tip
                // button = m_MapWin.Toolbar.AddButton("Identifier", m_ToolbarName, null, null);
                button = m_MapWin.Toolbar.AddButton("ddIdentifier", m_ToolbarName, true);
                if (button == null)
                    throw new Exception("Failed to add Identifier Button (" + m_MapWin.LastError + ")");
                button.Tooltip = "Identifier";
                button.Text = "";
                m_IdentBtn = button;
                m_IdentBtn.Picture = icon;

                button = m_MapWin.Toolbar.AddButton("Identify", m_ToolbarName, "ddIdentifier", "");
                if (button == null)
                    throw new Exception("Failed to add Identifier Button (" + m_MapWin.LastError + ")");
                button.Text = "Identifier";
                m_IdentBtn2 = button;
                m_IdentBtn2.Picture = icon;

                button = m_MapWin.Toolbar.AddButton("Identify by Shape(s)", m_ToolbarName, "ddIdentifier", "");
                if (button == null)
                    throw new Exception("Failed to add Identifier Button (" + m_MapWin.LastError + ")");
                button.Text = "Identify by Shape(s)";
                m_IdentByShape = button;
                m_IdentByShape.Picture = icon2;
            }
            catch (System.Exception ex)
            {
                ShowErrorBox("Initialize()", ex.Message);
            }
        }

        public void Terminate()
        {
            if (m_HavePanel && m_UIPanel != null)
            {
                m_MapWin.UIPanel.DeletePanel("Identifier");
                m_HavePanel = false;
                m_UIPanel = null;
            }

            //unload the buttons
            m_MapWin.Toolbar.RemoveButton("Identifier");

            //unload the toolbar
            if (m_ToolbarName.Length > 0)
                m_MapWin.Toolbar.RemoveToolbar(m_ToolbarName);

            m_MapWin = null;
        }

        public void ItemClicked(string ItemName, ref bool Handled)
        {
            try
            {
                //check to see if it was the Identifier pressed
                if (ItemName == "Identify")
                {
                    if (Activated == true)
                        Activated = false;
                    else
                        Activated = true;

                    //we handled this event
                    Handled = true;
                }
                else if (Activated == true)
                    Activated = false;

                if (ItemName == "Identify by Shape(s)")
                {
                    if (identbyshapeForm == null || identbyshapeForm.IsDisposed)
                        identbyshapeForm = new Forms.frmIdentByShape(m_MapWin, this);

                    //identbyshapeForm.Show();
                    //Paul Meems, 4 april 2008
                    //Add parenthandle so the form stays above MW instead of falling behind:
                    identbyshapeForm.Show(System.Windows.Forms.Form.FromHandle(new IntPtr(m_ParentHandle)));
                }
            }
            catch (System.Exception ex)
            {
                ShowErrorBox("ItemClicked()", ex.Message);
            }
        }

        public void LayerRemoved(int Handle)
        {

        }

        public void LayersCleared()
        {

        }

        public void LayerSelected(int Handle)
        {
            if (Handle == -1) return;

            //only do this if not on a wms layer, because wms layers are handled through the Message
            if (!m_OnWMSLayer)
            {
                if (Activated)
                {
                    // Force reloading of the new layer by toggling activation
                    MapWinGIS.Extents oldExts = m_MapWin.View.Extents;
                    //Activated = false;
                    //Activated = true;
                    LoadLayer();
                    m_MapWin.View.Extents = oldExts;
                }
            }
        }

        public void MapMouseDown(int Button, int Shift, int x, int y, ref bool Handled)
        {
            if (Activated)
            {
                m_MouseDown = true;

                //clear the previous drawing
                m_MapWin.View.Draw.ClearDrawing(m_hDraw);

                m_Bounds.x1 = x;
                m_Bounds.y1 = y;

                //set handled
                Handled = true;
            }
        }

        public void MapMouseUp(int Button, int Shift, int x, int y, ref bool Handled)
        {
            try
            {
                if (Activated)
                {
                    m_MouseDown = false;

                    //exit if their is no layers
                    if (m_MapWin.Layers.NumLayers <= 0) return;

                    //if a wms layer is selected and visible, broadcast so position can be checked
                    if (m_OnWMSLayer)
                    {
                        if (m_MapWin.Layers[m_MapWin.Layers.CurrentLayer].Visible)
                        {
                            m_MapWin.Plugins.BroadcastMessage("Identifier_Clicked_WMS " + x.ToString() + " " + y.ToString());
                        }
                        else
                        {
                            m_WMSLayerIDReturn = "Error: WMS Data Not Visible. Try Zooming In.";
                            LoadLayer();
                        }
                    }
                    else //Go about the normal selection
                    {
                        MapWinGIS.Extents BoundBox;
                        object selShapes = new object();
                        double tolx1 = 0, toly1 = 0, tolx2 = 0, toly2 = 0, tol = 0;
                        object obj = m_MapWin.Layers[m_MapWin.Layers.CurrentLayer].GetObject();
                        string layerName = m_MapWin.Layers[m_MapWin.Layers.CurrentLayer].Name;
                        MapWindow.Interfaces.eLayerType LayerType = m_MapWin.Layers[m_MapWin.Layers.CurrentLayer].LayerType;

                        //get the selected region
                        BoundBox = GetBoundBox(m_Bounds);

                        //if layer is a shapefile then do the following
                        if (LayerType == MapWindow.Interfaces.eLayerType.LineShapefile
                            || LayerType == MapWindow.Interfaces.eLayerType.PointShapefile
                            || LayerType == MapWindow.Interfaces.eLayerType.PolygonShapefile)
                        {
                            if (m_GridPropfrm.Visible)
                                m_GridPropfrm.Hide();

                            Shapefile shpFile = (Shapefile)obj;

                            //clear all the selected shapes
                            m_MapWin.View.SelectedShapes.ClearSelectedShapes();

                            //calculate tolerance
                            double r;
                            if (m_MapWin.Layers[m_MapWin.Layers.CurrentLayer].PointType == MapWinGIS.tkPointType.ptUserDefined)
                            {
                                MapWinGIS.Image image = m_MapWin.Layers[m_MapWin.Layers.CurrentLayer].UserPointType;
                                r = (image.Width + image.Height) / 2;
                            }
                            else
                            {
                                r = 3;
                            }

                            m_MapWin.View.PixelToProj(x - r, y - r, ref tolx1, ref toly1);
                            m_MapWin.View.PixelToProj(x + r, y + r, ref tolx2, ref toly2);
                            tol = System.Math.Sqrt(System.Math.Pow((tolx1 - tolx2), 2) + System.Math.Pow((toly1 - toly2), 2));

                            bool SelectedShapes = shpFile.SelectShapes(BoundBox, tol, MapWinGIS.SelectMode.INTERSECTION, ref selShapes);

                            //display the selected shapes
                            if (SelectedShapes)
                            {
                                AddSelectShapes(selShapes);
                                m_shpFilePropfrm.PopulateForm(!m_HavePanel, shpFile, (int[])selShapes, m_MapWin.Layers[m_MapWin.Layers.CurrentLayer].Name, false);
                            }
                            else
                            {
                                m_shpFilePropfrm.PopulateForm(!m_HavePanel, shpFile, m_MapWin.Layers[m_MapWin.Layers.CurrentLayer].Name, false);
                            }

                        }
                        //if the layer is a grid do the following
                        else if (LayerType == MapWindow.Interfaces.eLayerType.Grid)
                        {
                            if (m_shpFilePropfrm.Visible)
                            {
                                m_WMSPropfrm.Hide();
                                m_shpFilePropfrm.Hide();
                            }

                            //get the grid object
                            Grid grid = m_MapWin.Layers[m_MapWin.Layers.CurrentLayer].GetGridObject;

                            if (grid != null)
                                m_GridPropfrm.PopulateForm(!m_HavePanel, grid, layerName, BoundBox, m_MapWin.Layers.CurrentLayer);
                            else
                                m_GridPropfrm.Hide();
                        }
                        //if the layer is a image do the following
                        else if (LayerType == MapWindow.Interfaces.eLayerType.Image)
                        {
                            if (m_shpFilePropfrm.Visible)
                            {
                                m_WMSPropfrm.Hide();
                                m_shpFilePropfrm.Hide();
                            }

                            Grid grid = FindAssociatedGrid(((MapWinGIS.Image)obj).Filename);

                            if (grid != null)
                                m_GridPropfrm.PopulateForm(!m_HavePanel, grid, layerName, BoundBox, m_MapWin.Layers.CurrentLayer);
                            else
                            {
                                // Identify by values -- so just open the first band of the image.
                                // This only works for greyscale of course in this version (one band grids only)

                                Grid tmpGrid = new Grid();
                                try
                                {
                                    // Open as a grid to get values
                                    if (tmpGrid.Open(((MapWinGIS.Image)obj).Filename, GridDataType.UnknownDataType, false, GridFileType.UseExtension, null))
                                    {
                                        m_GridPropfrm.PopulateForm(!m_HavePanel, tmpGrid, layerName, BoundBox, m_MapWin.Layers.CurrentLayer);
                                    }
                                    else
                                    {
                                        MapWinUtility.Logger.Message("No information on this image can be displayed.", "Identifier", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information, DialogResult.OK);
                                        m_GridPropfrm.Hide();
                                    }
                                }
                                catch
                                {
                                    m_GridPropfrm.Hide();
                                }
                            }
                        }
                    }
                    
                    //set handled
                    Handled = true;
                }
            }
            catch (System.Exception ex)
            {
                ShowErrorBox("MapMouseUp()", ex.Message);
            }
        }

        public void MapMouseMove(int ScreenX, int ScreenY, ref bool Handled)
        {
            double x1, y1;
            double x2, y2;
            double x = 0, y = 0;

            //update coordinates
            m_MapWin.View.PixelToProj(ScreenX, ScreenY, ref x, ref y);

            // Chris Michaelis - moved to main MapWindow app.
            //m_Panel.Text = "X: " + Math.Round(x,3).ToString() + "    Y: " + Math.Round(y,3).ToString();

            if (Activated)
            {
                m_Bounds.x2 = ScreenX;
                m_Bounds.y2 = ScreenY; ;
                x1 = m_Bounds.x1;
                y1 = m_Bounds.y1;
                x2 = m_Bounds.x2;
                y2 = m_Bounds.y2;

                //get black color
                System.Drawing.Color black = System.Drawing.Color.Black;

                //draw a selection box if your dragging
                if (m_MouseDown)
                {
                    //clear the previous drawing
                    m_MapWin.View.Draw.ClearDrawing(m_hDraw);

                    //draw the bound box
                    m_hDraw = m_MapWin.View.Draw.NewDrawing(MapWinGIS.tkDrawReferenceList.dlScreenReferencedList);
                    m_MapWin.View.Draw.DrawLine(x1, y1, x2, y1, 1, black);
                    m_MapWin.View.Draw.DrawLine(x1, y1, x1, y2, 1, black);
                    m_MapWin.View.Draw.DrawLine(x1, y2, x2, y2, 1, black);
                    m_MapWin.View.Draw.DrawLine(x2, y1, x2, y2, 1, black);
                }

            }
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
            if (msg.StartsWith("ODP_Selected_WMS_Layer") || msg.StartsWith("ODP_Added_WMS_Layer"))
            {
                m_OnWMSLayer = true;
                m_WMSLayerIDReturn = "";

                if (Activated)
                {
                    // Force reloading of the new layer by toggling activation
                    MapWinGIS.Extents oldExts = m_MapWin.View.Extents;
                    LoadLayer();
                    m_MapWin.View.Extents = oldExts;
                }
            }
            else if (msg.StartsWith("ODP_Selected_Non_WMS_Layer"))
            {
                m_OnWMSLayer = false;
                m_WMSLayerIDReturn = "";
            }
            else if (msg.StartsWith("ODP_Identifier_Data_Return"))
            {
                if (Activated)
                {
                    m_WMSLayerIDReturn = msg.Replace("ODP_Identifier_Data_Return ", "");
                    // Force reloading of the new layer by toggling activation
                    MapWinGIS.Extents oldExts = m_MapWin.View.Extents;
                    LoadLayer();
                    m_MapWin.View.Extents = oldExts;
                }
            }
        }

        public void ProjectLoading(string ProjectFile, string SettingsString)
        {
            if (SettingsString != string.Empty)
            {
                char[] a = new char[] { '=' };
                string[] s = SettingsString.Split(a);
                if (s[1].ToLower() == bool.TrueString.ToLower())
                {
                    m_shpFilePropfrm.Editable = true;
                }
                else if (s[1].ToLower() == bool.FalseString.ToLower())
                {
                    m_shpFilePropfrm.Editable = false;
                }
                else
                {
                    m_shpFilePropfrm.Editable = true;
                }
            }
            else
            {
                m_shpFilePropfrm.Editable = true;
            }
        }

        public void ProjectSaving(string ProjectFile, ref string SettingsString)
        {
            SettingsString = "Editable=" + m_shpFilePropfrm.Editable.ToString();
        }

        public void ShapesSelected(int Handle, MapWindow.Interfaces.SelectInfo SelectInfo)
        {
            if (identbyshapeForm != null && identbyshapeForm.Visible) identbyshapeForm.UpdateSelectedShapes();
        }

        public void ActivateNoLoad()
        {
                        m_Activated = true;

                        //save the previous mode and cursor
                        if (m_MapWin.View.UserCursorHandle != (int)m_Cursor.Handle)
                        {
                            m_PreviousCursorMode = m_MapWin.View.CursorMode;
                            m_PreviousCursor = m_MapWin.View.MapCursor;
                        }
                        m_MapWin.View.CursorMode = MapWinGIS.tkCursorMode.cmNone;
                        m_MapWin.View.UserCursorHandle = (int)m_Cursor.Handle;
                        m_MapWin.View.MapCursor = tkCursor.crsrUserDefined;
                        //make the button sate pressed
                        m_IdentBtn.Pressed = true;
        }


        public bool Activated
        {
            get
            {
                return m_Activated;
            }
            set
            {
                try
                {
                    if (value == true)
                    {

                        m_Activated = true;

                        //save the previous mode and cursor
                        if (m_MapWin.View.UserCursorHandle != (int)m_Cursor.Handle)
                        {
                            m_PreviousCursorMode = m_MapWin.View.CursorMode;
                            m_PreviousCursor = m_MapWin.View.MapCursor;
                        }
                        m_MapWin.View.CursorMode = MapWinGIS.tkCursorMode.cmNone;
                        m_MapWin.View.UserCursorHandle = (int)m_Cursor.Handle;
                        m_MapWin.View.MapCursor = tkCursor.crsrUserDefined;
                        //make the button sate pressed
                        m_IdentBtn.Pressed = true;

                        //load the current layers properies
                        LoadLayer();
                    }
                    else
                    {
                        Deactivate();
                        m_MapWin.View.Draw.ClearDrawing(m_hDraw);
                        m_MapWin.View.Draw.ClearDrawing(m_GridPropfrm.m_hDraw);

                        //make the button sate unPressed
                        m_IdentBtn.Pressed = false;

                        //hide all the forms
                        m_GridPropfrm.Hide();
                        m_shpFilePropfrm.Hide();
                        m_WMSPropfrm.Hide();

                        //close everything
                        m_shpFilePropfrm.Unitialize();

                        //set the cursorMode and MapCursor back to it last state --
                        //if it is still mine -- otherwise probably launched another tool already.
                        if (m_MapWin.View.UserCursorHandle == (int)m_Cursor.Handle)
                        {
                            m_MapWin.View.UserCursorHandle = -1;
                            m_MapWin.View.CursorMode = m_PreviousCursorMode;
                            m_MapWin.View.MapCursor = m_PreviousCursor;
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    ShowErrorBox("Activated()", ex.Message);
                }

            }
        }



        public MapWinGIS.Extents GetBoundBox(Bounds bounds)
        {
            double maxX, maxY, minX, minY;
            double x1 = 0, y1 = 0, x2 = 0, y2 = 0;
            MapWinGIS.Extents boundBox = new MapWinGIS.Extents();

            //get the projection points
            m_MapWin.View.PixelToProj(bounds.x1, bounds.y1, ref x1, ref y1);
            m_MapWin.View.PixelToProj(bounds.x2, bounds.y2, ref x2, ref y2);

            //Set max and min X values
            if (x1 >= x2)
            {
                maxX = x1;
                minX = x2;
            }
            else
            {
                maxX = x2;
                minX = x1;
            }

            //Set max and min Y values
            if (y1 >= y2)
            {
                maxY = y1;
                minY = y2;
            }
            else
            {
                maxY = y2;
                minY = y1;
            }

            boundBox.SetBounds(minX, minY, 0, maxX, maxY, 0);

            return boundBox;
        }

        public void AddSelectShapes(object selShapes)
        {
            try
            {
                int[] shapes = (int[])selShapes;

                //display selected shapes
                int size = shapes.GetUpperBound(0);

                for (int i = 0; i <= size; i++)
                {
                    m_MapWin.View.SelectedShapes.AddByIndex(shapes[i], YELLOW);
                }
            }
            catch (System.Exception ex)
            {
                ShowErrorBox("SelectShapes()", ex.Message);
            }
        }

        private void OnPanelClose(string Caption)
        {
            if (Caption == "Identifier")
            {
                Activated = false;
            }

            try
            {
                System.Windows.Forms.Form.FromHandle(new IntPtr(m_ParentHandle)).Focus();
            }
            catch
            { }
        }

        public void LoadLayerAlternate(MapWindow.Interfaces.eLayerType LayerType, string layerName)
        {
            try
            {
                //exit if their is no layers to load
                if (m_MapWin.Layers.NumLayers <= 0) return;               

                // Always try to create the panel; if it doesn't exist,
                // it will be created, and if it does exist, a handle will be returned.
                m_UIPanel = m_MapWin.UIPanel.CreatePanel("Identifier", DockStyle.None);
                // Not intuitive -- m_UIPanel = m_MapWin.UIPanel.CreatePanel("Identifier", MapWindow.Interfaces.MapWindowDockStyle.RightAutoHide);
                if (m_UIPanel != null)
                {
                    m_MapWin.UIPanel.AddOnCloseHandler("Identifier", OnPanelClose);
                    m_HavePanel = true;
                }

                //if the layer is a shapefile
                if (LayerType == MapWindow.Interfaces.eLayerType.LineShapefile
                    || LayerType == MapWindow.Interfaces.eLayerType.PointShapefile
                    || LayerType == MapWindow.Interfaces.eLayerType.PolygonShapefile)
                {
                    if (m_GridPropfrm.Visible)
                        m_GridPropfrm.Hide();

                }
                //if the layer is a grid
                else if (LayerType == MapWindow.Interfaces.eLayerType.Grid)
                {
                    if (m_shpFilePropfrm.Visible)
                    {
                        m_shpFilePropfrm.Hide();
                        m_WMSPropfrm.Hide();
                    }
                }
                //if the layer is a image
                else if (LayerType == MapWindow.Interfaces.eLayerType.Image)
                {
                    if (m_shpFilePropfrm.Visible)
                    {
                        m_shpFilePropfrm.Hide();
                        m_WMSPropfrm.Hide();
                    }
                }

                if (m_HavePanel && m_UIPanel != null)
                {
                    //if the layer is a shapefile
                    if (LayerType == MapWindow.Interfaces.eLayerType.LineShapefile
                        || LayerType == MapWindow.Interfaces.eLayerType.PointShapefile
                        || LayerType == MapWindow.Interfaces.eLayerType.PolygonShapefile)
                    {
                        if (m_shpFilePropfrm.panel1 == null || m_shpFilePropfrm.panel1.IsDisposed)
                        {
                            m_shpFilePropfrm = new mwIdentifier.Forms.frmShapeFileProp(this);
                        }

                        if (m_UIPanel_LastType != "shapefile" && m_UIPanel_LastType != "")
                        {
                            while (m_UIPanel.Controls.Count > 0)
                                m_UIPanel.Controls.RemoveAt(0);
                        }

                        if (m_UIPanel.Controls.Count == 0)
                        {
                            m_UIPanel.Controls.Add(m_shpFilePropfrm.panel1);
                            m_UIPanel_LastType = "shapefile";
                        }
                        else
                        {
                            m_MapWin.UIPanel.SetPanelVisible("Identifier", true);
                        }

                        m_shpFilePropfrm.Hide();
                    }
                    //if the layer is a grid or image
                    else if (LayerType == MapWindow.Interfaces.eLayerType.Grid || LayerType == MapWindow.Interfaces.eLayerType.Image)
                    {
                        if (m_GridPropfrm.panel1 == null || m_GridPropfrm.panel1.IsDisposed)
                        {
                            m_GridPropfrm = new mwIdentifier.Forms.frmGridProp(this);
                        }

                        if (m_UIPanel_LastType != "grid" && m_UIPanel_LastType != "")
                        {
                            while (m_UIPanel.Controls.Count > 0)
                                m_UIPanel.Controls.RemoveAt(0);
                        }

                        if (m_UIPanel.Controls.Count == 0)
                        {
                            m_UIPanel.Controls.Add(m_GridPropfrm.panel1);
                            m_UIPanel_LastType = "grid";
                        }
                        else
                        {
                            m_MapWin.UIPanel.SetPanelVisible("Identifier", true);
                        }

                        m_GridPropfrm.Hide();
                    }
                }
                // else Normal identifier with a separate window
            }
            catch (System.Exception ex)
            {
                ShowErrorBox("LoadLayer()", ex.Message);
            }
        }


        private void LoadLayer()
        {
            try
            {
                //exit if their is no layers to load
                if (m_MapWin.Layers.NumLayers <= 0) return;

                // Always try to create the panel; if it doesn't exist,
                // it will be created, and if it does exist, a handle will be returned.
                m_UIPanel = m_MapWin.UIPanel.CreatePanel("Identifier", DockStyle.None);
                // Not intuitive -- m_UIPanel = m_MapWin.UIPanel.CreatePanel("Identifier", MapWindow.Interfaces.MapWindowDockStyle.RightAutoHide);
                if (m_UIPanel != null)
                {
                    m_MapWin.UIPanel.AddOnCloseHandler("Identifier", OnPanelClose);
                    m_HavePanel = true;
                }
                
                if (m_OnWMSLayer)
                {
                    if (m_GridPropfrm.Visible || m_shpFilePropfrm.Visible)
                    {
                        m_shpFilePropfrm.Hide();
                        m_GridPropfrm.Hide();
                    }

                    if (m_WMSLayerIDReturn != "")
                    {
                        m_WMSPropfrm.PopulateForm(!m_HavePanel, m_WMSLayerIDReturn, m_MapWin.Layers[m_MapWin.Layers.CurrentLayer].Name, false);
                    }

                    if (m_HavePanel && m_UIPanel != null)
                    {
                        if (m_WMSPropfrm.panel1 == null || m_WMSPropfrm.panel1.IsDisposed)
                        {
                            m_WMSPropfrm = new mwIdentifier.Forms.frmWMSProp(this);
                        }

                        if (m_UIPanel_LastType != "WMS" && m_UIPanel_LastType != "")
                        {
                            while (m_UIPanel.Controls.Count > 0)
                                m_UIPanel.Controls.RemoveAt(0);
                        }

                        if (m_UIPanel.Controls.Count == 0)
                        {
                            m_UIPanel.Controls.Add(m_WMSPropfrm.panel1);
                            m_UIPanel_LastType = "WMS";
                        }
                        else
                        {
                            m_MapWin.UIPanel.SetPanelVisible("Identifier", true);
                        }

                        m_WMSPropfrm.Hide();
                    }
                }
                else
                {
                    //get the object from mapwindow
                    object obj = m_MapWin.Layers[m_MapWin.Layers.CurrentLayer].GetObject();
                    string layerName = m_MapWin.Layers[m_MapWin.Layers.CurrentLayer].Name;
                    MapWindow.Interfaces.eLayerType LayerType = m_MapWin.Layers[m_MapWin.Layers.CurrentLayer].LayerType;


                    //if the layer is a shapefile
                    if (LayerType == MapWindow.Interfaces.eLayerType.LineShapefile
                        || LayerType == MapWindow.Interfaces.eLayerType.PointShapefile
                        || LayerType == MapWindow.Interfaces.eLayerType.PolygonShapefile)
                    {
                        if (m_GridPropfrm.Visible || m_WMSPropfrm.Visible)
                        {
                            m_WMSPropfrm.Hide();
                            m_GridPropfrm.Hide();
                        }

                        //check to see if there is any previously selected shapes
                        if (m_MapWin.View.SelectedShapes.NumSelected > 0)
                        {
                            int numSel = m_MapWin.View.SelectedShapes.NumSelected;
                            int[] shpIndex = new int[numSel];

                            for (int i = 0; i < numSel; i++)
                            {
                                shpIndex[i] = m_MapWin.View.SelectedShapes[i].ShapeIndex;
                            }
                            m_shpFilePropfrm.PopulateForm(!m_HavePanel, (Shapefile)obj, shpIndex, m_MapWin.Layers[m_MapWin.Layers.CurrentLayer].Name, false);
                        }
                        else
                        {
                            m_shpFilePropfrm.PopulateForm(!m_HavePanel, (Shapefile)obj, m_MapWin.Layers[m_MapWin.Layers.CurrentLayer].Name, false);
                        }
                    }
                    //if the layer is a grid
                    else if (LayerType == MapWindow.Interfaces.eLayerType.Grid)
                    {
                        if (m_shpFilePropfrm.Visible || m_WMSPropfrm.Visible)
                        {
                            m_WMSPropfrm.Hide();
                            m_shpFilePropfrm.Hide();
                        }

                        //get the grid object
                        Grid grid = m_MapWin.Layers[m_MapWin.Layers.CurrentLayer].GetGridObject;

                        if (grid != null)
                            m_GridPropfrm.PopulateForm(!m_HavePanel, grid, layerName, null, m_MapWin.Layers.CurrentLayer);
                        else
                            m_GridPropfrm.Hide();
                    }
                    //if the layer is a image
                    else if (LayerType == MapWindow.Interfaces.eLayerType.Image)
                    {
                        // Chris Michaelis - 5-17-2007 - Allow identification of image values (the color values), better than nothing.
                        // Do this when no associated grid is found of course.

                        if (m_shpFilePropfrm.Visible || m_WMSPropfrm.Visible)
                        {
                            m_WMSPropfrm.Hide();
                            m_shpFilePropfrm.Hide();
                        }

                        Grid grid = FindAssociatedGrid(((MapWinGIS.Image)obj).Filename);

                        if (grid != null)
                            m_GridPropfrm.PopulateForm(!m_HavePanel, grid, layerName, null, m_MapWin.Layers.CurrentLayer);
                        else
                        {
                            // Identify by values -- so just open the first band of the image.
                            // This only works for greyscale of course in this version (one band grids only)

                            Grid tmpGrid = new Grid();
                            try
                            {
                                // Open as a grid to get values
                                if (tmpGrid.Open(((MapWinGIS.Image)obj).Filename, GridDataType.UnknownDataType, false, GridFileType.UseExtension, null))
                                {
                                    m_GridPropfrm.PopulateForm(!m_HavePanel, tmpGrid, layerName, null, m_MapWin.Layers.CurrentLayer);
                                }
                                else
                                {
                                    MapWinUtility.Logger.Message("No information on this image can be displayed.", "Identifier", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information, DialogResult.OK);
                                    m_GridPropfrm.Hide();
                                }
                            }
                            catch
                            {
                                m_GridPropfrm.Hide();
                            }
                        }

                    }

                    if (m_HavePanel && m_UIPanel != null)
                    {
                        //if the layer is a shapefile
                        if (LayerType == MapWindow.Interfaces.eLayerType.LineShapefile
                            || LayerType == MapWindow.Interfaces.eLayerType.PointShapefile
                            || LayerType == MapWindow.Interfaces.eLayerType.PolygonShapefile)
                        {
                            if (m_shpFilePropfrm.panel1 == null || m_shpFilePropfrm.panel1.IsDisposed)
                            {
                                m_shpFilePropfrm = new mwIdentifier.Forms.frmShapeFileProp(this);
                            }

                            if (m_UIPanel_LastType != "shapefile" && m_UIPanel_LastType != "")
                            {
                                while (m_UIPanel.Controls.Count > 0)
                                    m_UIPanel.Controls.RemoveAt(0);
                            }

                            if (m_UIPanel.Controls.Count == 0)
                            {
                                m_UIPanel.Controls.Add(m_shpFilePropfrm.panel1);
                                m_UIPanel_LastType = "shapefile";
                            }
                            else
                            {
                                m_MapWin.UIPanel.SetPanelVisible("Identifier", true);
                            }

                            m_shpFilePropfrm.Hide();
                        }
                        //if the layer is a grid or image
                        else if (LayerType == MapWindow.Interfaces.eLayerType.Grid || LayerType == MapWindow.Interfaces.eLayerType.Image)
                        {
                            if (m_GridPropfrm.panel1 == null || m_GridPropfrm.panel1.IsDisposed)
                            {
                                m_GridPropfrm = new mwIdentifier.Forms.frmGridProp(this);
                            }

                            if (m_UIPanel_LastType != "grid" && m_UIPanel_LastType != "")
                            {
                                while (m_UIPanel.Controls.Count > 0)
                                    m_UIPanel.Controls.RemoveAt(0);
                            }

                            if (m_UIPanel.Controls.Count == 0)
                            {
                                m_UIPanel.Controls.Add(m_GridPropfrm.panel1);
                                m_UIPanel_LastType = "grid";
                            }
                            else
                            {
                                m_MapWin.UIPanel.SetPanelVisible("Identifier", true);
                            }

                            m_GridPropfrm.Hide();
                        }
                    }
                    // else Normal identifier with a separate window
                }
            }
            catch (System.Exception ex)
            {
                ShowErrorBox("LoadLayer()", ex.Message);
            }
        }

        public void ToggleDockedStatus(System.Windows.Forms.Form Sender, System.Windows.Forms.Panel ToggledPanel)
        {
            if (m_HavePanel)
            {
                if (m_OnWMSLayer)
                {
                    m_UIPanel.Controls.Remove(m_WMSPropfrm.panel1);
                    m_MapWin.UIPanel.DeletePanel("Identifier");
                    m_HavePanel = false;
                    m_UIPanel = null;
                    m_WMSPropfrm.Show();

                }
                else if (m_MapWin.Layers[m_MapWin.Layers.CurrentLayer].LayerType == MapWindow.Interfaces.eLayerType.LineShapefile || m_MapWin.Layers[m_MapWin.Layers.CurrentLayer].LayerType == MapWindow.Interfaces.eLayerType.PointShapefile || m_MapWin.Layers[m_MapWin.Layers.CurrentLayer].LayerType == MapWindow.Interfaces.eLayerType.PolygonShapefile)
                {
                    m_UIPanel.Controls.Remove(m_shpFilePropfrm.panel1);
                    m_MapWin.UIPanel.DeletePanel("Identifier");
                    m_HavePanel = false;
                    m_UIPanel = null;
                    m_shpFilePropfrm.Show();
                }
                else
                {
                    m_UIPanel.Controls.Remove(m_GridPropfrm.panel1);
                    m_MapWin.UIPanel.DeletePanel("Identifier");
                    m_HavePanel = false;
                    m_UIPanel = null;
                    m_GridPropfrm.Show();
                }
            }
            else
            {
                if (m_OnWMSLayer)
                {
                    m_UIPanel = m_MapWin.UIPanel.CreatePanel("Identifier", DockStyle.None);
                    if (m_UIPanel == null) return;
                    m_HavePanel = true;
                    m_UIPanel.Controls.Add(m_WMSPropfrm.panel1);
                    m_WMSPropfrm.Hide();
                }
                else if (m_MapWin.Layers[m_MapWin.Layers.CurrentLayer].LayerType == MapWindow.Interfaces.eLayerType.LineShapefile || m_MapWin.Layers[m_MapWin.Layers.CurrentLayer].LayerType == MapWindow.Interfaces.eLayerType.PointShapefile || m_MapWin.Layers[m_MapWin.Layers.CurrentLayer].LayerType == MapWindow.Interfaces.eLayerType.PolygonShapefile)
                {
                    m_UIPanel = m_MapWin.UIPanel.CreatePanel("Identifier", DockStyle.None);
                    if (m_UIPanel == null) return;
                    m_HavePanel = true;
                    m_UIPanel.Controls.Add(m_shpFilePropfrm.panel1);
                    m_shpFilePropfrm.Hide();
                }
                else
                {
                    m_UIPanel = m_MapWin.UIPanel.CreatePanel("Identifier", DockStyle.None);
                    if (m_UIPanel == null) return;
                    m_HavePanel = true;
                    m_UIPanel.Controls.Add(m_GridPropfrm.panel1);
                    m_GridPropfrm.Hide();
                }
            }
        }

        private Grid FindAssociatedGrid(string fileName)
        {
            MapWinGIS.ESRIGridManager esriManager = new ESRIGridManager();
            string bgdGrid = System.IO.Path.ChangeExtension(fileName, ".bgd");
            string ascGrid = System.IO.Path.ChangeExtension(fileName, ".asc");

            Grid grid = new MapWinGIS.GridClass();

            //check for all grid types to see if one exists
            if (System.IO.File.Exists(bgdGrid))
            {
                grid.Open(bgdGrid, MapWinGIS.GridDataType.UnknownDataType, true, MapWinGIS.GridFileType.UseExtension, null);
                return grid;
            }
            else if (System.IO.File.Exists(ascGrid))
            {
                grid.Open(ascGrid, MapWinGIS.GridDataType.UnknownDataType, true, MapWinGIS.GridFileType.UseExtension, null);
                return grid;
            }
            else if (esriManager.CanUseESRIGrids())
            {
                if (esriManager.IsESRIGrid(fileName))
                {
                    grid.Open(fileName, MapWinGIS.GridDataType.UnknownDataType, true, MapWinGIS.GridFileType.UseExtension, null);
                    return grid;
                }
            }

            grid.Close();

            //could not find any info on this image
            // No need to warn -- we'll show pixel values -> MapWinUtility.Logger.Message("Could not find any information about this image", "Identifier", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
            return null;
        }

        private void ShowErrorBox(string functionName, string errorMsg)
        {
            MapWinUtility.Logger.Message("Error in " + functionName + ", Message: " + errorMsg, "Identifier", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error, DialogResult.OK);
        }

        public void LayersAdded(MapWindow.Interfaces.Layer[] Layers)
        {
        }

        public void Deactivate()
        {
            m_Activated = false;
            if (m_HavePanel && m_UIPanel != null)
            {
                // m_MapWin.UIPanel.DeletePanel("Identifier");
                // m_HavePanel = false;
                // m_UIPanel = null;
                m_MapWin.UIPanel.SetPanelVisible("Identifier", false);
            }
        }
    }
	
}
