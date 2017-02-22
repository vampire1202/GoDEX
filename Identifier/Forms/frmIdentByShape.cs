//********************************************************************************************************
//File Name: frmIdentByShape.cs
//Description: Dialog form to display errors to users and allow reporting to developer team.
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
//1/18/2007 - Chris Michaelis - Created initial form source code
//2/14/2008 - Jiri Kadlec (jk)- Ensured proper initialization of selected shapes in cmdIdentify_Click()
//                              Moved message strings to resource file to enable localization
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace mwIdentifier.Forms
{
    public partial class frmIdentByShape : Form
    {
        MapWindow.Interfaces.IMapWin m_MapWin;
        mwIdentPlugin m_Plugin;
        MapWinGIS.tkCursorMode oldCur;
        
        // 2/15/2008 Jiri Kadlec - internalization
        System.Resources.ResourceManager resMan =
            new System.Resources.ResourceManager("MwIdentifier.Resource", System.Reflection.Assembly.GetExecutingAssembly());

        public frmIdentByShape(MapWindow.Interfaces.IMapWin MapWin, mwIdentPlugin Plugin)
        {
            InitializeComponent();

            m_MapWin = MapWin;
            m_Plugin = Plugin;
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmIdentByShape_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < m_MapWin.Layers.NumLayers; i++)
            {
                // 2/15/2008 Jiri Kadlec - ensure an 
                
                // Only allow polygon masks
                if (m_MapWin.Layers[m_MapWin.Layers.GetHandle(i)].LayerType == MapWindow.Interfaces.eLayerType.PolygonShapefile)
                {
                    this.cmbIdentWith.Items.Add("(" + m_MapWin.Layers.GetHandle(i).ToString() + ") " + m_MapWin.Layers[m_MapWin.Layers.GetHandle(i)].Name);
                }
                
                // Don't allow images
                if (m_MapWin.Layers[m_MapWin.Layers.GetHandle(i)].LayerType != MapWindow.Interfaces.eLayerType.Image)
                {
                    cmbIdentFrom.Items.Add("(" + m_MapWin.Layers.GetHandle(i).ToString() + ") " + m_MapWin.Layers[m_MapWin.Layers.GetHandle(i)].Name);
                    
                    // 2/15/2008 Jiri Kadlec - ensure selected layer in map legend is also selected in comboBox
                    if (i == m_MapWin.Layers.CurrentLayer)
                    {
                        cmbIdentFrom.SelectedIndex = cmbIdentFrom.Items.Count - 1;
                    }
                }
            }

            if (cmbIdentFrom.SelectedIndex == -1 && cmbIdentFrom.Items.Count > 0) cmbIdentFrom.SelectedIndex = 0;
            if (cmbIdentWith.SelectedIndex == -1 && cmbIdentWith.Items.Count > 0) cmbIdentWith.SelectedIndex = 0;

            UpdateSelectedShapes();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (btnSelect.Text != resMan.GetString("msgDone.Text"))
            {
                btnSelect.Text = resMan.GetString("msgDone.Text");
                if (cmbIdentWith.SelectedIndex != -1)
                {
                    int layerHandle = -1;
                    string s = cmbIdentWith.Items[cmbIdentWith.SelectedIndex].ToString();
                    if (s.Contains(")")) s = s.Substring(0, s.IndexOf(")"));
                    s = s.Replace("(", "");
                    if (!int.TryParse(s, out layerHandle)) return;

                    if (layerHandle != -1)
                    {
                        m_MapWin.Layers.CurrentLayer = layerHandle;
                    }
                    oldCur = m_MapWin.View.CursorMode;
                    m_MapWin.View.CursorMode = MapWinGIS.tkCursorMode.cmSelection;
                }
            }
            else
            {
                m_MapWin.View.CursorMode  = oldCur;
                btnSelect.Text = resMan.GetString("msgSelectShapes.Text"); //"&Select Shapes"
            }

            UpdateSelectedShapes();
        }

        public void UpdateSelectedShapes()
        {
            lblSel.Text = "(" + m_MapWin.View.SelectedShapes.NumSelected.ToString() + resMan.GetString("msgShapesSelected.Text");
        }

        private void cmdIdentify_Click(object sender, EventArgs e)
        {
            if (m_MapWin.View.SelectedShapes.NumSelected == 0)
            {
                MapWinUtility.Logger.Msg(resMan.GetString("msgZeroShapesSelected.Text"), resMan.GetString("titleSelectShapes.Text"));
                return;
            }

            if (cmbIdentFrom.SelectedIndex == -1)
            {
                MapWinUtility.Logger.Msg(resMan.GetString("msgIdentFromNotSelected.Text"), resMan.GetString("titleSpecifyLayer.Text"));
                return;
            }

            if (cmbIdentWith.SelectedIndex == -1)
            {
                MapWinUtility.Logger.Msg(resMan.GetString("msgIdentWithNotSelected.Text"), resMan.GetString("titleSpecifyLayer.Text"));
                return;
            }
            if (m_MapWin.View.SelectedShapes.NumSelected == 0) return;

            // If it's a polygon layer we're identifying, call SelectByPolygon
            // If it's a grid, extract by mask
            // ...then, open that temporary file (not Add to Map, just "Open"), and summarize all data within that
            // file.
            int fromLayerHandle = -1;
            int maskLayerHandle = -1;

            if (cmbIdentFrom.SelectedIndex != -1)
            {
                string s = cmbIdentFrom.Items[cmbIdentFrom.SelectedIndex].ToString();
                if (s.Contains(")")) s = s.Substring(0, s.IndexOf(")"));
                s = s.Replace("(", "");
                if (!int.TryParse(s, out fromLayerHandle)) return;

                if (fromLayerHandle == -1)
                {
                    return;
                }
            }
            if (cmbIdentWith.SelectedIndex != -1)
            {
                string s = cmbIdentWith.Items[cmbIdentWith.SelectedIndex].ToString();
                if (s.Contains(")")) s = s.Substring(0, s.IndexOf(")"));
                s = s.Replace("(", "");
                if (!int.TryParse(s, out maskLayerHandle)) return;

                if (maskLayerHandle == -1)
                {
                    return;
                }
            }

            string TempPath = System.IO.Path.GetTempFileName();
            System.IO.File.Delete(TempPath);

            MapWinGIS.Shape IdentifyBy;
            MapWinGIS.Shapefile sf = (MapWinGIS.Shapefile)m_MapWin.Layers[m_MapWin.Layers.CurrentLayer].GetObject();
            
            if (m_MapWin.View.SelectedShapes.NumSelected > 1)
            {
                // Get 0 and 1 first to initialize IdentifyBy
                MapWinGIS.Shape shp1 = sf.get_Shape(m_MapWin.View.SelectedShapes[0].ShapeIndex);
                MapWinGIS.Shape shp2 = sf.get_Shape(m_MapWin.View.SelectedShapes[1].ShapeIndex);
                MapWinGeoProc.SpatialOperations.MergeShapes(ref shp1, ref shp2, out IdentifyBy);
                // ...now, the rest
                if (m_MapWin.View.SelectedShapes.NumSelected > 2)
                    for (int i = 2; i < m_MapWin.View.SelectedShapes.NumSelected; i++)
                    {
                        MapWinGIS.Shape tmpResultShp;
                        MapWinGIS.Shape shp3 = sf.get_Shape(m_MapWin.View.SelectedShapes[i].ShapeIndex);
                        MapWinGeoProc.SpatialOperations.MergeShapes(ref IdentifyBy, ref shp3, out tmpResultShp);
                        IdentifyBy = tmpResultShp;
                        tmpResultShp = null;
                    }
                // Ready to identify based on a single shape now regardless of multiple selected
            }
            else
                IdentifyBy = sf.get_Shape(m_MapWin.View.SelectedShapes[0].ShapeIndex);

            MapWindow.Interfaces.eLayerType layerType = m_MapWin.Layers[fromLayerHandle].LayerType;
            if (layerType == MapWindow.Interfaces.eLayerType.Grid)
            {
                // Grid
                TempPath = System.IO.Path.ChangeExtension(TempPath, ".bgd");
                string fn = m_MapWin.Layers[fromLayerHandle].FileName;
                MapWinGeoProc.SpatialOperations.ClipGridWithPolygon(ref fn, ref IdentifyBy, ref TempPath, chbJustToExtents.Checked);
                MapWinGIS.Grid grd = new MapWinGIS.Grid();
                grd.Open(TempPath, MapWinGIS.GridDataType.UnknownDataType, true, MapWinGIS.GridFileType.UseExtension, null);
                if (grd == null || grd.Header == null)
                {
                    MapWinUtility.Logger.Msg(resMan.GetString("msgNoGridValues.Text"), resMan.GetString("titleNoGridValues.Text"));
                    return;
                }
                m_Plugin.ActivateNoLoad();
                m_Plugin.LoadLayerAlternate(layerType, m_MapWin.Layers[fromLayerHandle].Name);
                
                MapWinGIS.Extents exts = new MapWinGIS.Extents();
                exts.SetBounds(grd.Header.XllCenter, grd.Header.YllCenter + grd.Header.NumberRows * grd.Header.dY, 0, grd.Header.XllCenter + grd.Header.NumberCols * grd.Header.dX, grd.Header.YllCenter, 0);
                m_MapWin.Layers.CurrentLayer = fromLayerHandle;
                
                m_Plugin.m_GridPropfrm.PopulateForm(!m_Plugin.m_HavePanel, grd, m_MapWin.Layers[fromLayerHandle].Name, exts, fromLayerHandle);
                this.Close();
            }
            else
            {
                // SF
                if (!chbJustToExtents.Checked)
                {
                    string fn = m_MapWin.Layers[fromLayerHandle].FileName;

                    // 2/14/2008 jk the results ArrayList cannot be null,
                    // when it was null it caused an exception in SpatialOperations.SelectWithPolygon method
                    //System.Collections.ArrayList results = null;
                    System.Collections.ArrayList results = new System.Collections.ArrayList();
                    
                    MapWinGeoProc.SpatialOperations.SelectWithPolygon(ref fn, ref IdentifyBy, ref results);

                    // Switch current layer over to the one we're identifying so that the shapes
                    // can be reselected for visual effect
                    m_MapWin.Layers.CurrentLayer = fromLayerHandle;
                    m_Plugin.ActivateNoLoad();
                    m_Plugin.LoadLayerAlternate(layerType, m_MapWin.Layers[fromLayerHandle].Name);

                    int[] iresults = new int[results.Count];
                    for (int i = 0; i < results.Count; i++)
                    {
                        iresults[i] = (int)results[i];
                        m_MapWin.View.SelectedShapes.AddByIndex((int)results[i], m_MapWin.View.SelectColor);
                    }

                    m_Plugin.m_shpFilePropfrm.PopulateForm(!m_Plugin.m_HavePanel, (MapWinGIS.Shapefile)m_MapWin.Layers[fromLayerHandle].GetObject(), iresults, m_MapWin.Layers[fromLayerHandle].Name, false);

                    this.Close();
                }
                else
                {
                    object rslt = null;
                    m_MapWin.Layers.CurrentLayer = fromLayerHandle;
                    MapWinGIS.Shapefile DestSF = (MapWinGIS.Shapefile)m_MapWin.Layers[fromLayerHandle].GetObject();
                    
                    DestSF.SelectShapes(IdentifyBy.Extents, 0.1, MapWinGIS.SelectMode.INTERSECTION, ref rslt);

                    m_Plugin.ActivateNoLoad();
                    m_Plugin.LoadLayerAlternate(layerType, m_MapWin.Layers[fromLayerHandle].Name);

                    int[] results = (int[])rslt;
                    for (int i = 0; i < results.Length; i++)
                    {
                        m_MapWin.View.SelectedShapes.AddByIndex((int)results[i], m_MapWin.View.SelectColor);
                    }

                    m_Plugin.m_shpFilePropfrm.PopulateForm(!m_Plugin.m_HavePanel, DestSF, results, m_MapWin.Layers[fromLayerHandle].Name, false);

                    this.Close();
                }
            }
        }
    }
}