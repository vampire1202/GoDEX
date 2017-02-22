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
    public partial class RotateShapeForm : Form
    {
        private GlobalFunctions g;
        public MapWinGIS.Shapefile sf;
        private MapWinGIS.Shape m_Shape;
        private MapWinGIS.Point m_Centroid;
        private MapWinGIS.tkCursorMode m_CursorMode;
        private int m_DrawingHandle = -1;

        public MapWinGIS.Shape Shape
        {
            get
            {
                return m_Shape;
            }
            set
            {
                m_Shape = value;
                m_Centroid = MapWinGeoProc.Statistics.Centroid(ref m_Shape);
                txtX.Text = m_Centroid.x.ToString();
                txtY.Text = m_Centroid.y.ToString();

                if (m_DrawingHandle != -1) g.MapWin.View.Draw.ClearDrawing(m_DrawingHandle);
                m_DrawingHandle = g.MapWin.View.Draw.NewDrawing(MapWinGIS.tkDrawReferenceList.dlSpatiallyReferencedList);
                g.MapWin.View.Draw.DrawCircle(m_Centroid.x, m_Centroid.y, 2, Color.Blue, true);
            }
        }

        public RotateShapeForm(GlobalFunctions ig)
        {
            InitializeComponent();

            g = ig;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            txtX.Enabled = rdOther.Checked;
            txtY.Enabled = rdOther.Checked;
            linkLabel1.Enabled = rdOther.Checked;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (txtAngle.Text == "")
            {
                MapWinUtility.Logger.Message("Please enter a rotation angle.", "Enter a Rotation Angle", MessageBoxButtons.OK, MessageBoxIcon.Information, DialogResult.OK);
                return;
            }

            double RotX = 0;
            double RotY = 0;

            if (rdCentroid.Checked)
            {
                RotX = m_Centroid.x;
                RotY = m_Centroid.y;
            }
            else
            {
                RotX = double.Parse(txtX.Text);
                RotY = double.Parse(txtY.Text);
            }

            double angle;
            if (!double.TryParse(txtAngle.Text, out angle))
            {
                MapWinUtility.Logger.Message("The rotation angle was not understood.", "Bad Rotation Angle", MessageBoxButtons.OK, MessageBoxIcon.Information, DialogResult.OK);
                return;
            }

            RotateShape(m_Shape, angle, RotX, RotY);

            g.MapWin.View.Redraw();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Hide();
        }

        private void rdCentroid_CheckedChanged(object sender, EventArgs e)
        {
            txtX.Enabled = rdOther.Checked;
            txtY.Enabled = rdOther.Checked;
            linkLabel1.Enabled = rdOther.Checked;
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
            if (newVal < -180) newVal = -180;
            if (newVal > 180) newVal = 180;

            this.trackBar1.Value = newVal;

            if (s.Name == "txtX" || s.Name == "txtY") RedrawRotationCircleFromEntered();
        }

        private void RotateShapeForm_Load(object sender, EventArgs e)
        {
            rdCentroid.Checked = true;
            txtAngle.Text = "0";
            trackBar1.Value = 0;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            txtAngle.Text = trackBar1.Value.ToString();
        }

        private void RotateShape(MapWinGIS.Shape shape, double Angle, double aboutX, double aboutY)
        {
            for (int i = 0; i < shape.numPoints; i++)
            {
                MapWinGIS.Point pt = shape.get_Point(i);
                double Nx = 0, Ny = 0;
                Rotate(Angle, pt.x, pt.y, aboutX, aboutY, ref Nx, ref Ny);
                pt.x = Nx;
                pt.y = Ny;
            }
        }

        private void Rotate(double Angle, double x, double y, double ox, double oy, ref double Nx, ref double Ny)
        {
            Rotate(Angle, x - ox, y - oy, ref Nx, ref Ny);
            Nx += ox;
            Ny += oy;
        }

        private void Rotate(double Angle, double x, double y, ref double Nx, ref double Ny)
        {
            double SinVal, CosVal;
            const double PIDiv180 = 0.017453292519943295769236907684886;

            Angle *= PIDiv180;
            SinVal = Math.Sin(Angle);
            CosVal = Math.Cos(Angle);
            Nx = x * CosVal - y * SinVal;
            Ny = y * CosVal + x * SinVal;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            m_CursorMode = g.MapWin.View.CursorMode;
            g.MapWin.View.CursorMode = MapWinGIS.tkCursorMode.cmNone;
            g.Events.AddHandler(new MapWindow.Events.MapMouseDownEvent(MapMouseDown));           
        }

        private void MapMouseDown(int Button, int Shift, int x, int y, ref bool Handled)
        {
            g.Events.RemoveHandler(new MapWindow.Events.MapMouseDownEvent(MapMouseDown));

            double px = 0, py = 0;
            g.MapWin.View.PixelToProj(x, y, ref px, ref py);

            txtX.Text = px.ToString();
            txtY.Text = py.ToString();

            g.MapWin.View.CursorMode = m_CursorMode;
            RedrawRotationCircleFromEntered();
        }

        private void RedrawRotationCircleFromEntered()
        {
            g.MapWin.View.Draw.ClearDrawing(m_DrawingHandle);
            m_DrawingHandle = g.MapWin.View.Draw.NewDrawing(MapWinGIS.tkDrawReferenceList.dlSpatiallyReferencedList);
            try
            {
                g.MapWin.View.Draw.DrawCircle(double.Parse(txtX.Text), double.Parse(txtY.Text), 2, Color.Blue, true);
            }
            catch
            { }
        }

        private void RotateShapeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_DrawingHandle != -1) g.MapWin.View.Draw.ClearDrawing(m_DrawingHandle);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Hide();
        }

    }
}