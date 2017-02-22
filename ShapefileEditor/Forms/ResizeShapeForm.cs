// Christopher Michaelis 12/28/2006

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ShapefileEditor.Forms
{
    public partial class ResizeShapeForm : Form
    {
        private GlobalFunctions g;
        public MapWinGIS.Shapefile sf;
        public int Shape;

        private bool ignoreRadioCheck = false;

        public ResizeShapeForm(GlobalFunctions ig)
        {
            InitializeComponent();

            g = ig;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!rdGrow.Checked && !rdShrink.Checked)
            {
                MapWinUtility.Logger.Message("Please select whether to shrink or expand the shape.", "Choose Operation", MessageBoxButtons.OK, MessageBoxIcon.Information, DialogResult.OK);
                return;
            }

            double trueDist = 0;

            MapWinGIS.Shape inShp = sf.get_Shape(Shape);

            if (rdDistance.Checked)
            {
                if (!double.TryParse(txtDist.Text, out trueDist))
                {
                    MapWinUtility.Logger.Message("The distance was not understood.", "Bad Resize Distance", MessageBoxButtons.OK, MessageBoxIcon.Information, DialogResult.OK);
                    return;
                }
            }
            else
            {
                double by = 0;
                if (!double.TryParse(txtPercent.Text, out by))
                {
                    MapWinUtility.Logger.Message("The percentage was not understood.", "Bad Resize Percentage", MessageBoxButtons.OK, MessageBoxIcon.Information, DialogResult.OK);
                    return;
                }

                if (by < 1)
                {
                    MapWinUtility.Logger.Message("Please enter a percentage greater than 1%.", "Bad Resize Percentage", MessageBoxButtons.OK, MessageBoxIcon.Information, DialogResult.OK);
                    return;
                }
                if (by > 99 && rdShrink.Checked)
                {
                    MapWinUtility.Logger.Message("When shrinking a shape, please enter a percentage less than 99%.", "Bad Resize Percentage", MessageBoxButtons.OK, MessageBoxIcon.Information, DialogResult.OK);
                    return;
                }

                trueDist = Math.Abs(inShp.Extents.xMax - inShp.Extents.xMin) * (by / 100);
            }

            if (rdShrink.Checked)
            {
                if (rdPercent.Checked) trueDist /= 2;
                ShrinkOrGrowPoly(ref inShp, -1 * trueDist);
            }
            else
            {
                ShrinkOrGrowPoly(ref inShp, trueDist);
            }

            g.MapWin.View.Redraw();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Hide();
        }

        private void txts_TextChanged(object sender, EventArgs e)
        {
            TextBox s = (TextBox)sender;

            // Empty, or typing decimals or a negative number, quietly
            // let the user proceed
            if (s.Text == "") return;
            if (s.Text == ".") return;
            if (s.Text == "-") return;

            double rslt;

            if (!Double.TryParse(s.Text, out rslt))
            {
                MapWinUtility.Logger.Message("Please enter only numbers here.", "Numbers Only", MessageBoxButtons.OK, MessageBoxIcon.Information, DialogResult.OK);
                return;
            }

            int newVal = (int)rslt;
            if (newVal < 0) newVal = 0;
            if (newVal > 200) newVal = 200;

            trackBar1.Value = newVal;
            s.Text = newVal.ToString();
        }

        private void RotateShapeForm_Load(object sender, EventArgs e)
        {
            rdShrink.Checked = true;
            txtPercent.Text = "50";
            trackBar1.Value = 50;
        }

        private void ShrinkOrGrowPoly(ref MapWinGIS.Shape shp, double distance)
        {
            MapWinGIS.Point centroid = MapWinGeoProc.Statistics.Centroid(ref shp);
            // Avoid OLE calls; cache centroid point in local variable
            double cx = centroid.x;
            double cy = centroid.y;
            for (int i = 0; i < shp.numPoints; i++)
            {
                double ox = shp.get_Point(i).x;
                double oy = shp.get_Point(i).y;
                // Find the adjusted "real" distance (rdistance)
                double rdistance = Math.Sqrt(Math.Pow(cy - oy, 2) + Math.Pow(cx - ox, 2)) + distance;
                // Find the full distance of point to centroid (fdistance)
                double fdistance = Math.Sqrt(Math.Pow(cy - oy, 2) + Math.Pow(cx - ox, 2));

                if (rdistance < 0.00001)
                {
                    MapWinUtility.Logger.Message("Shrinking the shape by this amount will result in losing some shape geometry; aborting.", "Aborting - Loss of Geometry", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, DialogResult.OK);
                    return;
                }

                // Find the new point (project the old point along the line from point to midpoint)
                shp.get_Point(i).x = cx + (rdistance / fdistance) * (ox - cx);
                shp.get_Point(i).y = cy + (rdistance / fdistance) * (oy - cy);
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            txtPercent.Text = trackBar1.Value.ToString();
        }

        private void rdShape_CheckedChanged(object sender, EventArgs e)
        {
            // Since we've got the radio buttons in different group boxes,
            // duplicate the default behavior of unchecking others when one
            // is checked.
            if (ignoreRadioCheck) return;

            ignoreRadioCheck = true;
            if (((Control)sender).Name == "rdPercent")
            {
                txtPercent.Enabled = true;
                trackBar1.Enabled = true;

                txtDist.Enabled = false;
                rdDistance.Checked = false;
            }
            else if (((Control)sender).Name == "rdDistance")
            {
                txtDist.Enabled = true;

                txtPercent.Enabled = false;
                trackBar1.Enabled = false;
                rdPercent.Checked = false;
            }
            ignoreRadioCheck = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Hide();
        }
    }
}