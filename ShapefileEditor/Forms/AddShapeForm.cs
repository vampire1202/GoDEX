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
//1/29/2005 - This code is identical to the public domain version. - dpa
//6/2/2005  - User input validation error mark problem fixed - Lailin Chen
//27-May-06 Line 500: added (m_Shape.NumPoints > 2) - otherwise closes if first point is a snap point - Rob Cairns 
//********************************************************************************************************
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using lt = MapWindow.Interfaces.eLayerType;

namespace ShapefileEditor
{
	/// <summary>
	/// Summary description for AddShapeForm.
	/// </summary>
	public class AddShapeForm : System.Windows.Forms.Form
	{
		private ShapeClass m_Shape;
		private MapWindow.Interfaces.eLayerType m_SFType;
		private MapWinGIS.Shape m_retval;
		private GlobalFunctions m_globals;
		private MapWinGIS.tkCursorMode m_oldCursorMode;
		private MapWinGIS.tkCursor m_oldCursor;
		private int m_oldCursorHandle;
		private System.Windows.Forms.Cursor m_cursor;
		private int m_drawHandle;
		private MapWinGIS.Shapefile m_sf;
		private int m_PointSize;
		private SnapClass m_snapper;
        private MapWindow.Interfaces.View view;
		
		private MapWindow.Events.MapMouseMoveEvent m_MapMouseMoveDelegate;
		private MapWindow.Events.MapMouseUpEvent m_MapMouseUpDelegate;
		private MapWindow.Events.LayerSelectedEvent m_LayerSelectedDelegate;

		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnAddPoint;
		private System.Windows.Forms.ErrorProvider ErrorProvider1;
		internal System.Windows.Forms.TextBox txtX;
        internal System.Windows.Forms.TextBox txtY;
        private IContainer components;

		/// <summary>
		/// Constructor.
		/// The AddShapeForm is used when creating a new shape.  
		/// The user can either click on the map to generate new points, lines or polygons, or input coordinates in the text boxes provided on the form.
		/// </summary>
		public AddShapeForm(GlobalFunctions g, ref SnapClass snapper)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			m_globals = g;
			m_cursor = new System.Windows.Forms.Cursor(this.GetType(), "InsertPoint.cur");
			view = g.MapWin.View;
			m_oldCursorMode = view.CursorMode;
			m_oldCursor = view.MapCursor;
			m_oldCursorHandle = view.UserCursorHandle;
			view.CursorMode = MapWinGIS.tkCursorMode.cmNone;
			view.MapCursor = MapWinGIS.tkCursor.crsrWait;

			m_SFType = g.MapWin.Layers[g.MapWin.Layers.CurrentLayer].LayerType;
			m_Shape = new ShapeClass();

			// Sign me up for these events!
			m_MapMouseMoveDelegate = new MapWindow.Events.MapMouseMoveEvent(MouseMoveEvent);
			g.Events.AddHandler(m_MapMouseMoveDelegate);
			m_MapMouseUpDelegate = new MapWindow.Events.MapMouseUpEvent(MouseUpEvent);
			g.Events.AddHandler(m_MapMouseUpDelegate);
			m_LayerSelectedDelegate = new MapWindow.Events.LayerSelectedEvent(LayerSelectedEvent);
			g.Events.AddHandler(m_LayerSelectedDelegate);
			
			MapWindow.Interfaces.eLayerType t = g.MapWin.Layers[g.MapWin.Layers.CurrentLayer].LayerType;
			
			m_sf = g.CurrentLayer;

			if ((m_snapper == null) && ((t == lt.LineShapefile) || (t == lt.PointShapefile) || (t == lt.PolygonShapefile)))
				m_snapper = new SnapClass(m_globals.MapWin);

			view.MapCursor = MapWinGIS.tkCursor.crsrUserDefined;
			view.UserCursorHandle = m_cursor.Handle.ToInt32();

			m_PointSize = 6;
		}	

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddShapeForm));
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAddPoint = new System.Windows.Forms.Button();
            this.ErrorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.txtX = new System.Windows.Forms.TextBox();
            this.txtY = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.ErrorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AccessibleDescription = null;
            this.label2.AccessibleName = null;
            resources.ApplyResources(this.label2, "label2");
            this.ErrorProvider1.SetError(this.label2, resources.GetString("label2.Error"));
            this.label2.Font = null;
            this.ErrorProvider1.SetIconAlignment(this.label2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label2.IconAlignment"))));
            this.ErrorProvider1.SetIconPadding(this.label2, ((int)(resources.GetObject("label2.IconPadding"))));
            this.label2.Name = "label2";
            // 
            // label1
            // 
            this.label1.AccessibleDescription = null;
            this.label1.AccessibleName = null;
            resources.ApplyResources(this.label1, "label1");
            this.ErrorProvider1.SetError(this.label1, resources.GetString("label1.Error"));
            this.label1.Font = null;
            this.ErrorProvider1.SetIconAlignment(this.label1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label1.IconAlignment"))));
            this.ErrorProvider1.SetIconPadding(this.label1, ((int)(resources.GetObject("label1.IconPadding"))));
            this.label1.Name = "label1";
            // 
            // btnAddPoint
            // 
            this.btnAddPoint.AccessibleDescription = null;
            this.btnAddPoint.AccessibleName = null;
            resources.ApplyResources(this.btnAddPoint, "btnAddPoint");
            this.btnAddPoint.BackgroundImage = null;
            this.ErrorProvider1.SetError(this.btnAddPoint, resources.GetString("btnAddPoint.Error"));
            this.btnAddPoint.Font = null;
            this.ErrorProvider1.SetIconAlignment(this.btnAddPoint, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("btnAddPoint.IconAlignment"))));
            this.ErrorProvider1.SetIconPadding(this.btnAddPoint, ((int)(resources.GetObject("btnAddPoint.IconPadding"))));
            this.btnAddPoint.Name = "btnAddPoint";
            this.btnAddPoint.Click += new System.EventHandler(this.btnAddPoint_Click);
            // 
            // ErrorProvider1
            // 
            this.ErrorProvider1.ContainerControl = this;
            resources.ApplyResources(this.ErrorProvider1, "ErrorProvider1");
            // 
            // txtX
            // 
            this.txtX.AccessibleDescription = null;
            this.txtX.AccessibleName = null;
            resources.ApplyResources(this.txtX, "txtX");
            this.txtX.BackgroundImage = null;
            this.ErrorProvider1.SetError(this.txtX, resources.GetString("txtX.Error"));
            this.txtX.Font = null;
            this.ErrorProvider1.SetIconAlignment(this.txtX, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("txtX.IconAlignment"))));
            this.ErrorProvider1.SetIconPadding(this.txtX, ((int)(resources.GetObject("txtX.IconPadding"))));
            this.txtX.Name = "txtX";
            this.txtX.TextChanged += new System.EventHandler(this.txt_TextChanged);
            // 
            // txtY
            // 
            this.txtY.AccessibleDescription = null;
            this.txtY.AccessibleName = null;
            resources.ApplyResources(this.txtY, "txtY");
            this.txtY.BackgroundImage = null;
            this.ErrorProvider1.SetError(this.txtY, resources.GetString("txtY.Error"));
            this.txtY.Font = null;
            this.ErrorProvider1.SetIconAlignment(this.txtY, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("txtY.IconAlignment"))));
            this.ErrorProvider1.SetIconPadding(this.txtY, ((int)(resources.GetObject("txtY.IconPadding"))));
            this.txtY.Name = "txtY";
            this.txtY.TextChanged += new System.EventHandler(this.txt_TextChanged);
            // 
            // AddShapeForm
            // 
            this.AcceptButton = this.btnAddPoint;
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.BackgroundImage = null;
            this.Controls.Add(this.btnAddPoint);
            this.Controls.Add(this.txtY);
            this.Controls.Add(this.txtX);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Font = null;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = null;
            this.Name = "AddShapeForm";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.AddShapeForm_Closing);
            ((System.ComponentModel.ISupportInitialize)(this.ErrorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion


		public void LayerSelectedEvent(int Handle)
		{
			if (m_globals.CurrentLayer != null)
			{
				MapWindow.Interfaces.eLayerType t = m_globals.MapWin.Layers[m_globals.MapWin.Layers.CurrentLayer].LayerType;
			
				if ((m_snapper == null) && ((t == lt.LineShapefile) || (t == lt.PointShapefile) || (t == lt.PolygonShapefile)))
                    m_snapper = new SnapClass(m_globals.MapWin);
			}
			else
				m_snapper = null;
		}

		/// <summary>
		/// Makes sure that the text input boxes only contain numeric characters.
		/// </summary>
		/// <returns>Returns true if the text boxes contain only numeric characters, false otherwise.</returns>
		private bool ValidateText()
		{
            // Redone by Chris M Aug 2006
            double garbage;
            if (Double.TryParse(txtX.Text, out garbage) && Double.TryParse(txtY.Text, out garbage))
                return true;
            else
                return false;

            // Auugh! It hurts my eyes! -- Chris M
            //int i;
            //for(i = 0; i < txtX.Text.Length -1; i++)
            //    if(!char.IsDigit(txtX.Text,i))
            //        return false;
			
            //for(i = 0; i < txtY.Text.Length - 1; i++)
            //    if(!char.IsDigit(txtY.Text, i))
            //        return false;

			// return true;
		}

		/// <summary>
		/// Event handler for the Add Point button.
		/// </summary>
		/// <param name="sender">The object that created the event.</param>
		/// <param name="e">Button click parameters.</param>
		private void btnAddPoint_Click(object sender, System.EventArgs e)
		{
			if(ValidateText()) 
			{
				AddPoint(System.Convert.ToDouble(txtX.Text), System.Convert.ToDouble(txtY.Text));	
			}
		}
		
		/// <summary>
		/// Adds a point to the current shape.  If a shape is not yet initialized, it will initialize it.
		/// </summary>
		/// <param name="x">X coordinate in projected coordinates.</param>
		/// <param name="y">Y coordinate in projected coordinates.</param>
		internal void AddPoint(double x, double y)
		{
			if(m_Shape == null)
			{
				m_Shape = new ShapeClass();
			}

			if ((m_Shape.NumPoints > 0) && (m_SFType == lt.LineShapefile) && (m_Shape[m_Shape.NumPoints - 1].x == x) && (m_Shape[m_Shape.NumPoints - 1].y == y))
				this.Close(); // a line tried to add two points to the end, meaning that the line is finished.

			PointD newPoint = new PointD(x,y);
			m_Shape.AddPoint(newPoint);
			
			if (m_SFType == lt.PointShapefile) // point shapefiles can only have a single point.
				this.Close();
		
			if ((m_SFType == lt.PolygonShapefile) && (m_Shape.NumPoints > 2) && (m_Shape[0].x == newPoint.x) && (m_Shape[0].y == newPoint.y))
				this.Close(); // a polygon or line has doubled back on itself.
		}

		/// <summary>
		/// Event that occurs when the AddShapeForm is closing.  I generate the return value here.
		/// </summary>
		/// <param name="sender">The object causing the close event.</param>
		/// <param name="e">Event data.</param>
		private void AddShapeForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			m_globals.Events.RemoveHandler(m_MapMouseMoveDelegate);
			m_globals.Events.RemoveHandler(m_MapMouseUpDelegate);

			m_globals.MapWin.View.CursorMode = m_oldCursorMode;
			m_globals.MapWin.View.MapCursor = m_oldCursor;
			m_globals.MapWin.View.UserCursorHandle = m_oldCursorHandle;

            if (m_sf.EditingShapes == true)
            {
                m_globals.CreateUndoPoint();
                m_sf.StopEditingShapes(true, true, null);
            }

			if (m_Shape.NumPoints > 0)
				m_retval = m_Shape.ToMWShape(m_sf.ShapefileType);
			else
				m_retval = null;

			//Added by Lailin Chen to clear the SnapData list after use 12/12/2005
			m_snapper.ClearSnapData();
		}

		/// <summary>
		/// Returns the shape input by the user as a MapWinGIS.Shape.  If the user has not finished defining the shape, the value will be null.
		/// </summary>
		public MapWinGIS.Shape GetNewShape
		{
			get
			{
				if (m_Shape.NumPoints > 0)
					m_retval = m_Shape.ToMWShape(m_sf.ShapefileType);
				else
					m_retval = null;

				return m_retval;
			}
		}
		
		private void MouseMoveEvent(int ScreenX, int ScreenY, ref bool Handled)
		{
			try
			{
				if ((m_globals.CurrentMode == GlobalFunctions.Modes.AddShape) && (this.Visible))
				{
					m_globals.MapWin.View.UserCursorHandle = m_cursor.Handle.ToInt32();
					double x = 0, y = 0;
					m_globals.MapWin.View.PixelToProj((double)ScreenX, (double)ScreenY, ref x, ref y);
					PointD snappedPoint = null;
					System.Collections.ArrayList bestPoints = null;
					if (m_snapper != null && m_snapper.CanSnap(m_globals.CurrentTolerance, x, y, m_Shape, ref bestPoints))
					{
						snappedPoint = ((SnapData)bestPoints[0]).point;
						this.txtX.Text = snappedPoint.x.ToString();
						this.txtY.Text = snappedPoint.y.ToString();
						if (m_Shape.NumPoints > 0) 
						{
							m_globals.MapWin.View.Draw.ClearDrawing(m_drawHandle);
							m_drawHandle = m_globals.MapWin.View.Draw.NewDrawing(MapWinGIS.tkDrawReferenceList.dlSpatiallyReferencedList);
							DrawShape();
							MapWindow.Interfaces.Draw d = m_globals.MapWin.View.Draw;
							if (m_globals.MapWin.Layers[m_globals.MapWin.Layers.CurrentLayer].LayerType == MapWindow.Interfaces.eLayerType.PolygonShapefile)
								d.DrawLine(m_Shape[m_Shape.NumPoints - 1].x, m_Shape[m_Shape.NumPoints -1].y, snappedPoint.x, snappedPoint.y, 2, m_globals.MapWin.Layers[m_globals.MapWin.Layers.CurrentLayer].OutlineColor);
							else
                                d.DrawLine(m_Shape[m_Shape.NumPoints - 1].x, m_Shape[m_Shape.NumPoints -1].y, snappedPoint.x, snappedPoint.y, 2, m_globals.MapWin.Layers[m_globals.MapWin.Layers.CurrentLayer].Color);
						}
					}
					else
					{
						this.txtX.Text = x.ToString();
						this.txtY.Text = y.ToString();
						if (m_Shape.NumPoints > 0) 
						{
							m_globals.MapWin.View.Draw.ClearDrawing(m_drawHandle);
							m_drawHandle = m_globals.MapWin.View.Draw.NewDrawing(MapWinGIS.tkDrawReferenceList.dlSpatiallyReferencedList);
							DrawShape();
							MapWindow.Interfaces.Draw d = m_globals.MapWin.View.Draw;
							d.DrawLine(m_Shape[m_Shape.NumPoints - 1].x, m_Shape[m_Shape.NumPoints -1].y, x, y, 2, System.Drawing.Color.Blue);

						}
					}
				}
			}
			catch
			{
			}
		}

        private void ResetForNew()
        {
            m_snapper = new SnapClass(m_globals.MapWin);
            m_Shape = new ShapefileEditor.ShapeClass();

            m_globals.UpdateButtons();
            m_globals.DoEnables();

            m_globals.UpdateView();

            if (m_cursor == null) m_cursor = new System.Windows.Forms.Cursor(this.GetType(), "InsertPoint.cur");
            m_oldCursorMode = view.CursorMode;
            m_oldCursor = view.MapCursor;
            m_oldCursorHandle = view.UserCursorHandle;
            view.CursorMode = MapWinGIS.tkCursorMode.cmNone;

            view.MapCursor = MapWinGIS.tkCursor.crsrUserDefined;
            view.UserCursorHandle = m_cursor.Handle.ToInt32();
        }

		private void AddShape(ShapeClass s)
		{
			if(m_SFType == MapWindow.Interfaces.eLayerType.LineShapefile)
			{
				if (s.NumPoints < 2)
				{
                    ResetForNew();
                    MapWinUtility.Logger.Message("You must add at least two points for a line.", "Not Enough Points", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, DialogResult.OK);
                    return;
				}
			}
			else if(m_SFType == MapWindow.Interfaces.eLayerType.PolygonShapefile)
			{
                // 3 + 1 for termination point - first must equal last, meaning we have to have 4 for a polygon
				if (s.NumPoints < 4)
				{
                    ResetForNew();
                    MapWinUtility.Logger.Message("You must add at least three points for a polygon.", "Not Enough Points", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, DialogResult.OK);
                    return;
				}
			}

			if (m_sf.EditingShapes == false)
			{
				if(m_sf.StartEditingShapes(true, null) == false)
				{
                    MapWinUtility.Logger.Msg("Could not edit the shapefile.", "Error");
                    if (!GlobalFunctions.m_StayInAddMode)
                        this.Close();
                    else
                        ResetForNew(); 
                    return;
				}
			}

			int newIndex = m_sf.NumShapes;
			MapWinGIS.Shape shp = s.ToMWShape(m_sf.ShapefileType);
			//MapWinGIS.ShapefileColorScheme cs = m_globals.MapWin.Layers[m_globals.MapWin.Layers.CurrentLayer].ColoringScheme;
			if(m_sf.EditInsertShape(shp, ref newIndex) == false)
			{
                MapWinUtility.Logger.Msg("Failed to add the new shape to the shapefile.", "Error");
			}
			else
			{
				if (m_snapper != null)
					m_snapper.AddShapeData(newIndex, m_globals.MapWin.Layers.CurrentLayer);

                // Save
                if (m_sf.EditingShapes == true)
                {
                    m_globals.CreateUndoPoint();
                    m_sf.StopEditingShapes(true, true, null);
                }

                m_globals.MapWin.Plugins.BroadcastMessage("ShapefileEditor: Layer " + m_globals.MapWin.Layers.CurrentLayer.ToString() + ": New Shape Added");
			}
			m_globals.MapWin.View.Draw.ClearDrawing(m_drawHandle);
            if (!GlobalFunctions.m_StayInAddMode)
                this.Close();
            else
                ResetForNew();
        }

		private void DrawShape()
		{
			MapWindow.Interfaces.Draw d = m_globals.MapWin.View.Draw;
			MapWindow.Interfaces.Layer l = m_globals.MapWin.Layers[m_globals.MapWin.Layers.CurrentLayer];

			PointD prev = null;

			for (int i = 0; i < m_Shape.NumPoints; i++)
			{
				PointD p = m_Shape[i];
				d.DrawPoint(p.x, p.y, m_PointSize, System.Drawing.Color.Blue);
				
				if (prev != null)
				{
					if (l.LayerType == MapWindow.Interfaces.eLayerType.PolygonShapefile)
						d.DrawLine(prev.x, prev.y, p.x, p.y, (int)l.LineOrPointSize, l.OutlineColor);
					else
						d.DrawLine(prev.x, prev.y, p.x, p.y, (int)l.LineOrPointSize, l.Color);
				}
				
				prev = p;
			}
			
		}

		public void MouseUpEvent(int Button, int Shift, int x, int y, ref bool Handled)
		{
			Handled = true;

			if (Button == (int)MapWindow.Interfaces.vb6Buttons.Left)
			{
				PointD snappedPoint = null;
				System.Collections.ArrayList bestPoints = null;
				double newx = 0, newy = 0;
				m_globals.MapWin.View.PixelToProj((double)x, (double)y, ref newx, ref newy);
				if (m_snapper != null && m_snapper.CanSnap(m_globals.CurrentTolerance, newx, newy, m_Shape, ref bestPoints) == true)
				{
					snappedPoint = ((SnapData)bestPoints[0]).point;
					m_Shape.AddPoint(snappedPoint);
					
					if (m_SFType == MapWindow.Interfaces.eLayerType.PolygonShapefile)
					{
						int last = m_Shape.NumPoints - 1;
                        if ((m_Shape.NumPoints > 2) && (m_Shape[0].x == m_Shape[last].x || m_Shape[0].y == m_Shape[last].y))
						{
							// the polygon has been finished.
							AddShape(m_Shape);
							return;
						}
					}
					else if(m_SFType == MapWindow.Interfaces.eLayerType.PointShapefile)
					{
						AddShape(m_Shape);
						return;
					}
					
				}
				else // can't snap
				{
					double tx = 0, ty = 0;
					m_globals.MapWin.View.PixelToProj(x, y, ref tx, ref ty);
					m_Shape.AddPoint(new PointD(tx, ty));

					if(m_SFType == MapWindow.Interfaces.eLayerType.PointShapefile)
						AddShape(m_Shape);
				}

				DrawShape();
			}
			else // right button clicked, finish the shape off
			{
				if (m_SFType == MapWindow.Interfaces.eLayerType.PolygonShapefile)
				{
					// Make sure the first and last points are the same
					int last = m_Shape.NumPoints - 1;
					if (m_Shape[0].x != m_Shape[last].x || m_Shape[0].y != m_Shape[last].y)
					{
						// they are not the same
						m_Shape.AddPoint(new PointD(m_Shape[0].x, m_Shape[0].y));
					}
				}

				AddShape(m_Shape);
			}
			m_globals.MapWin.View.Draw.ClearDrawing(m_drawHandle);
		}

        /// <summary>
		/// Event handler for the X coordinte text box.
		/// If an invalid character is entered, the event handler does not accept it.
		/// The event handler also enables/disables the Add Point button.
		/// </summary>
        private void txt_TextChanged(object sender, EventArgs e)
        {
            double garbage;
            if (!double.TryParse(((TextBox)sender).Text, out garbage))
			{
				ErrorProvider1.SetError((TextBox)sender,"Please only enter valid numeric coordinates");
			}
			// Added by Lailin Chen 6/2/2005 start1
			else if(ErrorProvider1.GetError((TextBox)sender) != null)
			{
				ErrorProvider1.SetError((TextBox)sender, null);
			}
			// Added by Lailin Chen 6/2/2005 end1
			btnAddPoint.Enabled = ValidateText();

        }
	} // end class
}
