using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ShapefileEditor.Forms
{
    public partial class CleanupForm : Form
    {
        private GlobalFunctions g;
        private MapWinGIS.Shapefile sf;

        public CleanupForm(MapWinGIS.Shapefile shapeFile, GlobalFunctions ig)
        {
            InitializeComponent();

            sf = shapeFile;
            g = ig;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            double tolerance = 0;
            if (!double.TryParse(txtTolerance.Text, out tolerance))
            {
                MapWinUtility.Logger.Message("Please enter only numbers in the distance field.", "Enter Only Numbers", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, DialogResult.OK);
                return;
            }

            this.Cursor = Cursors.WaitCursor;

            prgShape.Visible = true;
            prgPoint.Visible = true;
            
            sf.StartEditingShapes(true, null);
            long totalRemoved = 0;
            long fromShapes = 0;
            int numPts;
            System.Collections.ArrayList removeList = new System.Collections.ArrayList();

            if (sf.NumShapes > 0)
            {
                prgShape.Maximum = sf.NumShapes;

                for (int z = 0; z < sf.NumShapes; z++)
                {
                    MapWinGIS.Shape shp = sf.get_Shape(z);
                    if (z < 100 || z % 5 == 0)
                    {
                        prgShape.Value = z;
                        this.Refresh();
                    }
                    Application.DoEvents();

                    numPts = shp.numPoints;
                    if (numPts > 0)
                    {
                        prgPoint.Maximum = numPts;

                        for (int i = numPts - 1; i > 0; i--) // 0 never needs to actually be hit, inner loop will compare against all zeros
                        {
                            if (i % 5 == 0)
                            {
                                prgPoint.Value = numPts - i;
                                this.Refresh();
                            }

                            for (int j = i - 1; j >= 0; j--)
                            {
                                if (Math.Sqrt(Math.Pow(shp.get_Point(i).x - shp.get_Point(j).x, 2) + Math.Pow(shp.get_Point(i).y - shp.get_Point(j).y, 2)) < tolerance)
                                {
                                    // Make sure that !(i == numPts - 1 && j == 0) -- polygon completion point
                                    if (!removeList.Contains(i) && !(i == numPts - 1 && j == 0)) removeList.Add(i);
                                }
                            }
                        }
                    }

                    if (removeList.Count > 0)
                    {
                        if (removeList.Count >= shp.numPoints - 1)
                        {
                            // Probably not a good thing.....
                            MapWinUtility.Logger.Message("Aborting: Proceeding will remove all points from one or more shapes. The distance may need to be smaller, particularly for unprojected (latitute and longitude) coordinate systems.", "Aborting -- All Points Would Be Removed", MessageBoxButtons.OK, MessageBoxIcon.Error, DialogResult.OK);
                            sf.StopEditingShapes(false, true, null);
                            this.Cursor = Cursors.Default;
                            prgPoint.Value = 0;
                            prgShape.Value = 0;
                            prgPoint.Visible = false;
                            prgShape.Visible = false;
                            this.Cursor = Cursors.Default;
                            return;
                        }

                        totalRemoved += removeList.Count;
                        fromShapes++;

                        while (removeList.Count > 0)
                        {
                            for (int part = 0; part < shp.NumParts; part++)
                            {
                                if (shp.get_Part(part) >= (int)removeList[0])
                                    shp.set_Part(part, shp.get_Part(part) - 1);
                            }
                            shp.DeletePoint((int)removeList[0]);
                            removeList.RemoveAt(0);
                        }

                        // If this is a polygon and there are now less than 3 shapes, panic
                        if ((shp.ShapeType == MapWinGIS.ShpfileType.SHP_POLYGON|| shp.ShapeType == MapWinGIS.ShpfileType.SHP_POLYGONZ || shp.ShapeType == MapWinGIS.ShpfileType.SHP_POLYGONM) && shp.numPoints < 3)
                        {
                            // Probably not a good thing.....
                            MapWinUtility.Logger.Message("Aborting: Proceeding will leave less than 3 points in a polygon. The distance may need to be smaller, particularly for unprojected (latitute and longitude) coordinate systems.", "Aborting -- Polygons Would Be Destroyed", MessageBoxButtons.OK, MessageBoxIcon.Error, DialogResult.OK);
                            sf.StopEditingShapes(false, true, null);
                            this.Cursor = Cursors.Default;
                            prgPoint.Value = 0;
                            prgShape.Value = 0;
                            prgPoint.Visible = false;
                            prgShape.Visible = false;
                            this.Cursor = Cursors.Default;
                            return;
                        }

                        // If the first and last points are not the same now, reclose it
                        if (shp.get_Point(0).x != shp.get_Point(shp.numPoints - 1).x || shp.get_Point(0).y != shp.get_Point(shp.numPoints - 1).y)
                        {
                            MapWinGIS.Point pnt = new MapWinGIS.Point();
                            pnt.x = shp.get_Point(0).x;
                            pnt.y = shp.get_Point(0).y;
                            pnt.Z = shp.get_Point(0).Z;
                            int ptidx = shp.numPoints;
                            shp.InsertPoint(pnt, ref ptidx);
                        }
                    }
                }
            }

            prgPoint.Value = prgPoint.Maximum;
            prgShape.Value = prgShape.Maximum;

            g.CreateUndoPoint();

            sf.StopEditingShapes(true, true, null);

            this.Cursor = Cursors.Default;

            if (totalRemoved > 0)
                MapWinUtility.Logger.Message("There were " + totalRemoved.ToString() + " points removed from " + fromShapes.ToString() + " shapes.", "Finished", MessageBoxButtons.OK, MessageBoxIcon.Information, DialogResult.OK);
            else
                MapWinUtility.Logger.Message("Finished -- no extra points needed to be removed.", "Finished", MessageBoxButtons.OK, MessageBoxIcon.Information, DialogResult.OK);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void CleanupForm_Load(object sender, EventArgs e)
        {
            double LikelyTol = ((g.MapWin.View.Extents.xMax - g.MapWin.View.Extents.xMin) / 19000);
            try
            {
                if (LikelyTol < 1)
                    txtTolerance.Text = LikelyTol.ToString().Substring(0, Math.Min(9, LikelyTol.ToString().Length));
                else
                {
                    if (LikelyTol.ToString().Length > 9)
                        txtTolerance.Text = Math.Round(LikelyTol).ToString();
                    else
                        txtTolerance.Text = LikelyTol.ToString();
                }
            }
            catch
            {
                txtTolerance.Text = LikelyTol.ToString();
            }
        }
    }
}