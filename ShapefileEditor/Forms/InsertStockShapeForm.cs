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
//1/2/2007 - Initial code - Chris Michaelis
//1/24/2009  - Use MapUnitsAlternate for user-specified shape dimensions - Jiri Kadlec
//********************************************************************************************************

// Chris Michaelis 1/2/2007

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MapWindow.Interfaces;

namespace ShapefileEditor.Forms
{
    public partial class InsertStockShapeForm : Form
    {
        private GlobalFunctions g;
        private MapWinGIS.tkCursorMode m_CursorMode;
        private MapWinGIS.tkCursor m_Cursor;
        private bool ignoreRadioCheck;

        //Distance units used for shape dimensions
        private MapWindow.Interfaces.UnitOfMeasure _mapUnits = UnitOfMeasure.Unknown;
        private MapWindow.Interfaces.UnitOfMeasure _alternateUnits = UnitOfMeasure.Unknown;
        private bool _useAlternateUnits = false;

        public InsertStockShapeForm(GlobalFunctions ig)
        {
            InitializeComponent();

            g = ig;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void InsertStockShapeForm_Load(object sender, EventArgs e)
        {            
            // Jiri Kadlec 1/9/2009 set initial text box values to blank
            txtPolySideLength.Text = "";
            txtEllpsWidth.Text = "";
            txtEllpsHeight.Text = "";
            txtCircleRadius.Text = "";
            txtRectWidth.Text = "";
            txtRectHeight.Text = "";
            // Set up default widths and radii
            //txtPolySideLength.Text = Math.Round(((g.MapWin.View.Extents.xMax - g.MapWin.View.Extents.xMin) / 8), 3).ToString();
            //txtEllpsWidth.Text = Math.Round(double.Parse(txtPolySideLength.Text), 3).ToString();
            //txtEllpsHeight.Text = Math.Round((double.Parse(txtPolySideLength.Text) / 2), 3).ToString();
            //txtCircleRadius.Text = Math.Round((double.Parse(txtPolySideLength.Text) / 2), 3).ToString();
            //txtRectWidth.Text = Math.Round(double.Parse(txtPolySideLength.Text), 3).ToString();
            //txtRectHeight.Text = Math.Round((double.Parse(txtPolySideLength.Text) / 2), 3).ToString();

            g.Events.AddHandler(new MapWindow.Events.MapMouseDownEvent(MapMouseDown));
            m_CursorMode = g.MapWin.View.CursorMode;
            g.MapWin.View.CursorMode = MapWinGIS.tkCursorMode.cmNone;
            m_Cursor = g.MapWin.View.MapCursor;
            g.MapWin.View.MapCursor = MapWinGIS.tkCursor.crsrArrow;


            // Jiri Kadlec 1/9/2009 try to use the 'alternate units' if possible
            _mapUnits = MapWinGeoProc.UnitConverter.StringToUOM(g.MapWin.Project.MapUnits);
            _alternateUnits = MapWinGeoProc.UnitConverter.StringToUOM(g.MapWin.Project.MapUnitsAlternate);
            if (_mapUnits != UnitOfMeasure.DecimalDegrees && _alternateUnits != UnitOfMeasure.DecimalDegrees &&
                _alternateUnits != UnitOfMeasure.Unknown)
            {
                _useAlternateUnits = true;
                _alternateUnits = MapWinGeoProc.UnitConverter.StringToUOM(g.MapWin.Project.MapUnitsAlternate);
                lblUnit.Text = g.MapWin.Project.MapUnitsAlternate;
            }
            else
            {
                _useAlternateUnits = false;
                lblUnit.Text = g.MapWin.Project.MapUnits;
            }
            if (lblUnit.Text == "Lat/Long") lblUnit.Text = "Deg.";
            lblUnit1.Text = lblUnit.Text;
            lblUnit2.Text = lblUnit.Text;
            lblUnit3.Text = lblUnit.Text;
            lblUnit4.Text = lblUnit.Text;
            lblUnit5.Text = lblUnit.Text;        
        }

        private void MapMouseDown(int Button, int Shift, int x, int y, ref bool Handled)
        {
            // Paul Meems, 27 Nov. 2009, fix for bug #1514
            // Added check if currentlayer is still a polygon shapefile:
            if (g.MapWin.Layers[g.MapWin.Layers.CurrentLayer].LayerType != eLayerType.PolygonShapefile)
                return;
            // End modifications 27 Nov. 2009
                        
            if (rdRegularPoly.Checked)
            {
                double width = 0;
                if (!double.TryParse(txtPolySideLength.Text, out width) || width <= 0)
                {
                    MapWinUtility.Logger.Message("Please enter a valid width, greater than zero.", "Enter a Width", MessageBoxButtons.OK, MessageBoxIcon.Information, DialogResult.OK);
                    return;
                }
                if (_useAlternateUnits)
                {
                    width = MapWinGeoProc.UnitConverter.ConvertLength(_alternateUnits, _mapUnits, width);
                }

                double px = 0;
                double py = 0;
                g.MapWin.View.PixelToProj(x, y, ref px, ref py);

                AddRegularPolygon((int)nudPolySides.Value, width, px, py);
            }
            else if (rdCircle.Checked)
            {
                double radius = 0;
                if (!double.TryParse(txtCircleRadius.Text, out radius) || radius <= 0)
                {
                    MapWinUtility.Logger.Message("Please enter a valid radius, greater than zero.", "Enter a Radius", MessageBoxButtons.OK, MessageBoxIcon.Information, DialogResult.OK);
                    return;
                }
                if (_useAlternateUnits)
                {
                    radius = MapWinGeoProc.UnitConverter.ConvertLength(_alternateUnits, _mapUnits, radius);
                }

                double px = 0;
                double py = 0;
                g.MapWin.View.PixelToProj(x, y, ref px, ref py);

                AddCircle(radius, px, py);
            }
            else if (rdEllipse.Checked)
            {
                double width = 0;
                double height = 0;
                if (!double.TryParse(txtEllpsHeight.Text, out height) || height <= 0)
                {
                    MapWinUtility.Logger.Message("Please enter a valid height, greater than zero.", "Enter a Height", MessageBoxButtons.OK, MessageBoxIcon.Information, DialogResult.OK);
                    return;
                }
                if (!double.TryParse(txtEllpsWidth.Text, out width) || width <= 0)
                {
                    MapWinUtility.Logger.Message("Please enter a valid width, greater than zero.", "Enter a Width", MessageBoxButtons.OK, MessageBoxIcon.Information, DialogResult.OK);
                    return;
                }
                if (_useAlternateUnits)
                {
                    width = MapWinGeoProc.UnitConverter.ConvertLength(_alternateUnits, _mapUnits, width);
                    height = MapWinGeoProc.UnitConverter.ConvertLength(_alternateUnits, _mapUnits, height);
                }

                double px = 0;
                double py = 0;
                g.MapWin.View.PixelToProj(x, y, ref px, ref py);

                AddEllipse(px - (width / 2), py - (height / 2), px + (width / 2), py + (height / 2));
            }
            else if (rdRectangle.Checked)
            {
                double width = 0;
                double height = 0;
                if (!double.TryParse(txtRectHeight.Text, out height) || height <= 0)
                {
                    MapWinUtility.Logger.Message("Please enter a valid height, greater than zero.", "Enter a Height", MessageBoxButtons.OK, MessageBoxIcon.Information, DialogResult.OK);
                    return;
                }
                if (!double.TryParse(txtRectWidth.Text, out width) || width <= 0)
                {
                    MapWinUtility.Logger.Message("Please enter a valid width, greater than zero.", "Enter a Width", MessageBoxButtons.OK, MessageBoxIcon.Information, DialogResult.OK);
                    return;
                }
                if (_useAlternateUnits)
                {
                    width = MapWinGeoProc.UnitConverter.ConvertLength(_alternateUnits, _mapUnits, width);
                    height = MapWinGeoProc.UnitConverter.ConvertLength(_alternateUnits, _mapUnits, height);
                }

                double px = 0;
                double py = 0;
                g.MapWin.View.PixelToProj(x, y, ref px, ref py);

                AddRectangle(px - (width / 2), py - (height / 2), px + (width / 2), py + (height / 2));
            }

        }

        private void InsertStockShapeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            g.Events.RemoveHandler(new MapWindow.Events.MapMouseDownEvent(MapMouseDown));
            g.MapWin.View.CursorMode = m_CursorMode;
            m_Cursor = g.MapWin.View.MapCursor;
        }

        private void AddRegularPolygon(int sides, double sidelength, double centroidX, double centroidY)
        {
            // Paul Meems, 26 Oct. 2009, fix for bug #1460
            // Added check for in-memory shapefiles, those have no filename:
            if (!g.MapWin.Layers.IsValidHandle(g.MapWin.Layers.CurrentLayer)) return;
            // End modifications Paul Meems, 26 Oct. 2009

		    double pi = 3.1415926535897932384626433832795;
            double apothem = sidelength / (2 * Math.Tan(pi / sides));
            double radius = sidelength / (2 * Math.Sin(pi / sides));

            MapWinGIS.Shape newPolygon = new MapWinGIS.Shape();

		    int partIndex = 0;
		    newPolygon.Create(MapWinGIS.ShpfileType.SHP_POLYGON);
		    newPolygon.InsertPart(0, ref partIndex);

            double i = 0;
		    int shpIndex = 0;

		    // Approximate the polygon with n line segments
		    double increment = ((2 * pi ) / sides);

		    for (; i <= 2 * pi; i += increment, shpIndex++)
		    {
                MapWinGIS.Point newPt = new MapWinGIS.Point();
			        			
			    newPt.x = (radius * Math.Cos(i) + centroidX);
                newPt.y = (radius * Math.Sin(i) + centroidY);
                newPt.Z = 0;

			    newPolygon.InsertPoint(newPt, ref shpIndex);
		    }

		    // Finalize -- add it.
            MapWinGIS.Shapefile sf = (MapWinGIS.Shapefile)g.MapWin.Layers[g.MapWin.Layers.CurrentLayer].GetObject();
            if (!sf.EditingShapes || !sf.EditingTable)
                sf.StartEditingShapes(true, null);

            int addedShapes = sf.NumShapes;
		    sf.EditInsertShape(newPolygon, ref addedShapes);
            
            g.CreateUndoPoint();
            
            sf.StopEditingShapes(true, true, null);

            // And show it:
            g.UpdateView();

            g.MapWin.Plugins.BroadcastMessage("ShapefileEditor: Layer " + g.MapWin.Layers.CurrentLayer.ToString() + ": New Shape Added");
        }

        private void AddCircle(double radius, double centroidX, double centroidY)
        {
            // Paul Meems, 26 Oct. 2009, fix for bug #1460
            // Added check for in-memory shapefiles, those have no filename:
            if (!g.MapWin.Layers.IsValidHandle(g.MapWin.Layers.CurrentLayer)) return;
            // End modifications Paul Meems, 26 Oct. 2009

            double pi = 3.1415926535897932384626433832795;
            MapWinGIS.Shape newPolygon = new MapWinGIS.Shape();

            int partIndex = 0;
            newPolygon.Create(MapWinGIS.ShpfileType.SHP_POLYGON);
            newPolygon.InsertPart(0, ref partIndex);

            double i = 0;
            int shpIndex = 0;

            // Approximate the polygon with n line segments
            double increment = ((2 * pi) / 360);

            for (; i <= 2 * pi; i += increment, shpIndex++)
            {
                MapWinGIS.Point newPt = new MapWinGIS.Point();

                newPt.x = (radius * Math.Cos(i) + centroidX);
                newPt.y = (radius * Math.Sin(i) + centroidY);
                newPt.Z = 0;

                newPolygon.InsertPoint(newPt, ref shpIndex);
            }

            // Finalize -- add it.
            MapWinGIS.Shapefile sf = (MapWinGIS.Shapefile)g.MapWin.Layers[g.MapWin.Layers.CurrentLayer].GetObject();
            if (!sf.EditingShapes || !sf.EditingTable)
                sf.StartEditingShapes(true, null);

            int addedShapes = sf.NumShapes;
            sf.EditInsertShape(newPolygon, ref addedShapes);
            g.CreateUndoPoint(); 
            
            sf.StopEditingShapes(true, true, null);

            // And show it:
            g.UpdateView();

            g.MapWin.Plugins.BroadcastMessage("ShapefileEditor: Layer " + g.MapWin.Layers.CurrentLayer.ToString() + ": New Shape Added");
        }

        private void AddEllipse(double LeftmostX, double LowestY, double RightmostX, double HighestY)
        {
            // Paul Meems, 26 Oct. 2009, fix for bug #1460
            // Added check for in-memory shapefiles, those have no filename:
            if (!g.MapWin.Layers.IsValidHandle(g.MapWin.Layers.CurrentLayer)) return;
            // End modifications Paul Meems, 26 Oct. 2009

            MapWinGIS.Shape newPolygon = new MapWinGIS.Shape();

            int partIndex = 0;
            newPolygon.Create(MapWinGIS.ShpfileType.SHP_POLYGON);
            newPolygon.InsertPart(0, ref partIndex);

            int shpIndex = 0;

            double t, a, b, tinc, centx, centy;
            a = RightmostX - LeftmostX;
            b = HighestY - LowestY;
            tinc = Math.PI * 2 / (a + b);
            centx = (LeftmostX + RightmostX) * .5;
            centy = (LowestY + HighestY) * .5;

            MapWinGIS.Point newPt = new MapWinGIS.Point();
            newPt.x = centx + a;
            newPt.y = centy;
            newPt.Z = 0;
            newPolygon.InsertPoint(newPt, ref shpIndex);

            for (t = 0; t < Math.PI * 2; t += tinc)
            {
                MapWinGIS.Point nextPt = new MapWinGIS.Point();
                nextPt.x = centx + a * Math.Cos(t);
                nextPt.y = centy - b * Math.Sin(t);
                nextPt.Z = 0;
                newPolygon.InsertPoint(nextPt, ref shpIndex);
            }

            // Finalize -- add it.
            MapWinGIS.Shapefile sf = (MapWinGIS.Shapefile)g.MapWin.Layers[g.MapWin.Layers.CurrentLayer].GetObject();
            if (!sf.EditingShapes || !sf.EditingTable)
                sf.StartEditingShapes(true, null);

            int addedShapes = sf.NumShapes;
            sf.EditInsertShape(newPolygon, ref addedShapes);

            g.CreateUndoPoint(); 
            
            sf.StopEditingShapes(true, true, null);

            // And show it:
            g.UpdateView();

            g.MapWin.Plugins.BroadcastMessage("ShapefileEditor: Layer " + g.MapWin.Layers.CurrentLayer.ToString() + ": New Shape Added");
        }

        private void AddRectangle(double LeftmostX, double LowestY, double RightmostX, double HighestY)
        {

            // Paul Meems, 26 Oct. 2009, fix for bug #1460
            // Added check for in-memory shapefiles, those have no filename:
            if (!g.MapWin.Layers.IsValidHandle(g.MapWin.Layers.CurrentLayer)) return;
            // End modifications Paul Meems, 26 Oct. 2009


            MapWinGIS.Shape newPolygon = new MapWinGIS.Shape();

            int partIndex = 0;
            newPolygon.Create(MapWinGIS.ShpfileType.SHP_POLYGON);
            newPolygon.InsertPart(0, ref partIndex);

            int shpIndex = 0;

            MapWinGIS.Point newPt = new MapWinGIS.Point();
            newPt.x = LeftmostX;
            newPt.y = HighestY;
            newPt.Z = 0;
            newPolygon.InsertPoint(newPt, ref shpIndex);

            newPt = new MapWinGIS.Point();
            newPt.x = RightmostX;
            newPt.y = HighestY;
            newPt.Z = 0;
            newPolygon.InsertPoint(newPt, ref shpIndex);

            newPt = new MapWinGIS.Point();
            newPt.x = RightmostX;
            newPt.y = LowestY;
            newPt.Z = 0;
            newPolygon.InsertPoint(newPt, ref shpIndex);

            newPt = new MapWinGIS.Point();
            newPt.x = LeftmostX;
            newPt.y = LowestY;
            newPt.Z = 0;
            newPolygon.InsertPoint(newPt, ref shpIndex);

            // Finalize -- add it.
            MapWinGIS.Shapefile sf = (MapWinGIS.Shapefile)g.MapWin.Layers[g.MapWin.Layers.CurrentLayer].GetObject();
            if (!sf.EditingShapes || !sf.EditingTable)
                sf.StartEditingShapes(true, null);

            int addedShapes = sf.NumShapes;
            sf.EditInsertShape(newPolygon, ref addedShapes);

            g.CreateUndoPoint();

            sf.StopEditingShapes(true, true, null);
            
            // Release memory used by point reallocations above
            GC.Collect();

            // And show it:
            g.UpdateView();

            g.MapWin.Plugins.BroadcastMessage("ShapefileEditor: Layer " + g.MapWin.Layers.CurrentLayer.ToString() + ": New Shape Added");
        }

        private void rdShape_CheckedChanged(object sender, EventArgs e)
        {
            // Since we've got the radio buttons in different group boxes,
            // duplicate the default behavior of unchecking others when one
            // is checked.
            if (ignoreRadioCheck) return;

            ignoreRadioCheck = true;
            if (((Control)sender).Name == "rdRegularPoly")
            {
                rdCircle.Checked = false;
                rdEllipse.Checked = false;
                rdRectangle.Checked = false;
            }
            else if (((Control)sender).Name == "rdCircle")
            {
                rdEllipse.Checked = false;
                rdRegularPoly.Checked = false;
                rdRectangle.Checked = false;
            }
            else if (((Control)sender).Name == "rdEllipse")
            {
                rdCircle.Checked = false;
                rdRegularPoly.Checked = false;
                rdRectangle.Checked = false;
            }
            else if (((Control)sender).Name == "rdRectangle")
            {
                rdCircle.Checked = false;
                rdRegularPoly.Checked = false;
                rdCircle.Checked = false;
            }
            ignoreRadioCheck = false;

            txtPolySideLength.Enabled = rdRegularPoly.Checked;
            nudPolySides.Enabled = rdRegularPoly.Checked;
            txtCircleRadius.Enabled = rdCircle.Checked;
            txtEllpsHeight.Enabled = rdEllipse.Checked;
            txtEllpsWidth.Enabled = rdEllipse.Checked;
            txtRectHeight.Enabled = rdRectangle.Checked;
            txtRectWidth.Enabled = rdRectangle.Checked;
        }
    }
}