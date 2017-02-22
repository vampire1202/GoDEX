/*
 * This code is provided under the Code Project Open Licence (CPOL)
 * See http://www.codeproject.com/info/cpol10.aspx for details
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Security.Permissions;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace System.Windows.Forms
{
	/// <summary>
	/// Description of CustomTabControl.
	/// </summary>
	[ToolboxBitmapAttribute(typeof(TabControl))]
	public class CustomTabControl : TabControl
	{
		
		#region	Construction

		public CustomTabControl(){
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			
			this._BackBuffer = new Bitmap(this.Width, this.Height);
			this._BufferGraphics = Graphics.FromImage(this._BackBuffer);
			
			this._Radius = 1;
			this._Style = TabStyle.Default;
			this._ImageAlign = ContentAlignment.MiddleLeft;
		}

		protected override void OnCreateControl(){
			base.OnCreateControl();
			this.OnFontChanged(EventArgs.Empty);
		}
		
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing) {
				if (this._BufferGraphics != null){
					this._BufferGraphics.Dispose();
				}
				if (this._BackBuffer != null){
					this._BackBuffer.Dispose();
				}
			}
		}

		#endregion
		
		#region Private variables

		private Bitmap _BackBuffer;
		private Graphics _BufferGraphics;
		private int _Radius;
		private int _Overlap;
		private TabStyle _Style;
		private ContentAlignment _ImageAlign;
		private int oldValue;
		private Color _borderColorSelected = Color.Empty;
        private Color _borderColor = Color.Empty;
        
		#endregion
		
		#region Public properties

		[Category("Appearance"), DefaultValue(typeof(TabStyle), "Default"), RefreshProperties(RefreshProperties.All)]
		public TabStyle DisplayStyle {
			get { return this._Style; }
			set {
				
				this._Style = value;
				switch (value) {
					case TabStyle.None:
						this.Alignment = TabAlignment.Top;
						base.Multiline = true;
						this.UpdateStyles();
						this._Overlap = 0;
						this._Radius = 1;
						this.ImageAlign = ContentAlignment.MiddleLeft;
						this.Padding = new Point(1,1);
						break;
						
					case TabStyle.Default:
						this._Overlap = 0;
						this._Radius = 1;
						this.ImageAlign = ContentAlignment.MiddleLeft;
						this.Padding = new Point(6,3);
						break;
						
					case TabStyle.Angled:
						this._Overlap = 7;
						this._Radius = 10;
						this.ImageAlign = ContentAlignment.MiddleRight;
						this.Padding = new Point(11,3);
						break;
						
					case TabStyle.Rounded:
						this._Overlap = 0;
						this._Radius = 10;
						this.ImageAlign = ContentAlignment.MiddleRight;
						this.Padding = new Point(6,3);
						break;
						
					case TabStyle.VisualStudio:
						this._Overlap = 7;
						this._Radius = 10;
						this.ImageAlign = ContentAlignment.MiddleRight;
						this.Padding = new Point(13,1);
						break;
						
					case TabStyle.Chrome:
						this._Overlap = 16;
						this._Radius = 10;
						this.ImageAlign = ContentAlignment.MiddleRight;
						this.Padding = new Point(13,5);
						break;
				}
				
				this.Invalidate();
			}
		}

		[Category("Appearance"), RefreshProperties(RefreshProperties.All)]
		public new bool Multiline {
			get {
				return base.Multiline;
			}
			set {
				base.Multiline = value;
				this.Invalidate();
			}
		}
		
		[Category("Appearance"), DefaultValue(typeof(ContentAlignment), "MiddleLeft")]
		public ContentAlignment ImageAlign {
			get { return this._ImageAlign; }
			set {
				this._ImageAlign = value;
				this.Invalidate();
			}
		}
		
		[Category("Appearance"), DefaultValue(1)]
		public int Radius {
			get { return this._Radius; }
			set {
				if (value < 1){
					throw new ArgumentException("The radius must be greater than 1", "value");
				}
				this._Radius = value;
				this.Invalidate();
			}
		}
		
		[Category("Appearance"), DefaultValue(0)]
		public int Overlap {
			get { return this._Overlap; }
			set {
				if (value < 0){
					throw new ArgumentException("The tabs cannot have a negative overlap", "value");
				}
				this._Overlap = value;
				this.Invalidate();
			}
		}
		
		[Category("Appearance"), DefaultValue(typeof(Point), "6,3")]
		public new Point Padding {
			get { return base.Padding; }
			set {
				base.Padding = value;
			}
		}
		
		[Category("Appearance")]
		public new TabAlignment Alignment {
			get { return base.Alignment; }
			set {
				base.Alignment = value;
				switch (value) {
					case TabAlignment.Top:
					case TabAlignment.Bottom:
						this.Multiline = false;
						break;
					case TabAlignment.Left:
					case TabAlignment.Right:
						this.Multiline = true;
						break;
				}
				
			}
		}

		[Category("Appearance"), DefaultValue(typeof(Color), "")]
        public Color BorderColorSelected
        {
            get { return _borderColorSelected; }
            set { _borderColorSelected = value; 
            	Invalidate(); 
            }
        }

        [Category("Appearance"), DefaultValue(typeof(Color), "")]
        public Color BorderColor
        {
            get { return _borderColor; }
            set { _borderColor = value;
            	Invalidate();
            }
        }
        
		//	Hide the Appearance attribute so it can not be changed
		//	We don't want it as we are doing all the painting.
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "value")]
		public new TabAppearance Appearance{
			get{
				return base.Appearance;
			}
			set{
				//	Don't permit setting to other appearances as we are doing all the painting
				base.Appearance = TabAppearance.Normal;
			}
		}
		
		public override Rectangle DisplayRectangle {
			get {
				//	Special processing to hide tabs
				if (this._Style == TabStyle.None) {
					return new Rectangle(0, 0, Width, Height);
				} else {
					int tabStripHeight = 0;
					int itemHeight = 0;
					
					if (this.Alignment <= TabAlignment.Bottom) {
						itemHeight = this.ItemSize.Height;
					} else {
						itemHeight = this.ItemSize.Width;
					}
					
					tabStripHeight = 5 + (itemHeight * this.RowCount);
					
					Rectangle rect = new Rectangle(4, tabStripHeight, Width - 8, Height - tabStripHeight - 4);
					switch (this.Alignment) {
						case TabAlignment.Top:
							rect = new Rectangle(4, tabStripHeight, Width - 8, Height - tabStripHeight - 4);
							break;
						case TabAlignment.Bottom:
							rect = new Rectangle(4, 4, Width - 8, Height - tabStripHeight - 4);
							break;
						case TabAlignment.Left:
							rect = new Rectangle(tabStripHeight, 4, Width - tabStripHeight - 4, Height - 8);
							break;
						case TabAlignment.Right:
							rect = new Rectangle(4, 4, Width - tabStripHeight - 4, Height - 8);
							break;
					}
					return rect;
				}
			}
		}

		[Browsable(false)]
		public int ActiveIndex {
			get {
				NativeMethods.TCHITTESTINFO hitTestInfo = new NativeMethods.TCHITTESTINFO(this.PointToClient(Control.MousePosition));
				int index = NativeMethods.SendMessage(this.Handle, NativeMethods.TCM_HITTEST, IntPtr.Zero, NativeMethods.ToIntPtr(hitTestInfo)).ToInt32();
				if (index == -1){
					return -1;
				} else {
					if (this.TabPages[index].Enabled){
						return index;
					} else {
						return -1;
					}
				}
			}
		}

		[Browsable(false)]
		public TabPage ActiveTab{
			get{
				int activeIndex = this.ActiveIndex;
				if (activeIndex > -1){
					return this.TabPages[activeIndex];
				} else {
					return null;
				}
			}
		}
		
		#endregion

		#region Events

		[Category("Action")] public event ScrollEventHandler HScroll;
		[Category("Action")] public event EventHandler<TabControlEventArgs> TabImageClick;
		
		#endregion

		#region	Base class event processing

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);

			//	Recreate the buffer for manual double buffering
			if (this.Width > 0 && this.Height > 0){
				if (this._BufferGraphics != null){
					this._BufferGraphics.Dispose();
				}
				if (this._BackBuffer != null){
					this._BackBuffer.Dispose();
				}

				this._BackBuffer = new Bitmap(this.Width, this.Height);
				this._BufferGraphics = Graphics.FromImage(this._BackBuffer);

			}
		}

		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);

			//	Tell the tabs the font has changed
			NativeMethods.SendMessage(this.Handle, NativeMethods.WM_SETFONT, this.Font.ToHfont(), new IntPtr(-1));
			NativeMethods.SendMessage(this.Handle, NativeMethods.WM_FONTCHANGE, IntPtr.Zero, IntPtr.Zero);
			this.UpdateStyles();
			this.Invalidate();
		}

		protected override void OnSelecting(TabControlCancelEventArgs e)
		{
			base.OnSelecting(e);
			
			//	Do not allow selecting of disabled tabs
			if (e.Action == TabControlAction.Selecting && e.TabPage != null && !e.TabPage.Enabled){
				e.Cancel = true;
			}
		}
		
		protected override void OnMove(EventArgs e){
			base.OnMove(e);
			this.Invalidate();
		}
		
		protected override void OnControlAdded(ControlEventArgs e){
			base.OnControlAdded(e);
			this.Invalidate();
		}

		protected override void OnControlRemoved(ControlEventArgs e){
			base.OnControlRemoved(e);
			this.Invalidate();
		}
		
		protected override void OnSelectedIndexChanged(EventArgs e){
			base.OnSelectedIndexChanged(e);
		}

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[System.Diagnostics.DebuggerStepThrough()]
		protected override void WndProc(ref Message m){
			
			switch (m.Msg) {
				case NativeMethods.WM_HSCROLL:
					
					//	Raise the scroll event when the scroller is scrolled
					base.WndProc(ref m);
					this.OnHScroll(new ScrollEventArgs(((ScrollEventType)NativeMethods.LoWord(m.WParam)),oldValue, NativeMethods.HiWord(m.WParam), ScrollOrientation.HorizontalScroll));
					break;
				case NativeMethods.WM_PAINT:
					
					//	Handle painting ourselves rather than call the base OnPaint.
					CustomPaint(ref m);
					break;

				default:
					base.WndProc(ref m);
					break;
					
			}
		}

		protected override void OnMouseClick(MouseEventArgs e){
			int index = this.ActiveIndex;
			
			//	If we are clicking on an image then raise the ImageClicked event instead of raising the standard mouse click event
			//	if there if a handler.
			//	if the event is not handled then just raise the mouse clicked as usual
			if (index > -1 && (this.TabPages[index].ImageIndex > -1 || !string.IsNullOrEmpty(this.TabPages[index].ImageKey))
			    && this.GetTabImageRect(index).Contains(e.Location)
			    && this.TabImageClick != null){
				this.OnTabImageClick(new TabControlEventArgs(this.TabPages[index], index, TabControlAction.Selected));
			}
			base.OnMouseClick(e);
		}

		protected virtual void OnTabImageClick(TabControlEventArgs e){
			if (this.TabImageClick != null){
				this.TabImageClick(this, e);
			}
		}
		
		protected virtual void OnHScroll(ScrollEventArgs e){
			//	repaint the moved tabs
			this.Invalidate();
			
			//	Raise the event
			if (this.HScroll != null){
				this.HScroll(this, e);
			}
			
			if (e.Type == ScrollEventType.EndScroll){
				this.oldValue = e.NewValue;
			}
		}

		#endregion

		#region	Basic drawing methods

		private void CustomPaint(ref Message m)
		{
			//	We render into a bitmap that is then drawn in one shot rather than using
			//	double buffering built into the control as the built in buffering
			// 	messes up the background painting.
			//	Equally the .Net 2.0 BufferedGraphics object causes the background painting
			//	to mess up, which is why we use this .Net 1.1 buffering technique.
			
			//	Buffer code from Gil. Schmidt http://www.codeproject.com/KB/graphics/DoubleBuffering.aspx
			this.PaintTransparentBackground(this._BufferGraphics, this.ClientRectangle);

			if (this.TabCount > 0) {

				//	When top or bottom and scrollable we need to clip the sides from painting the tabs.
				//	Left and right are always multiline.
				if (this.Alignment <= TabAlignment.Bottom && !this.Multiline){
					this._BufferGraphics.Clip = new Region(new RectangleF(this.ClientRectangle.X + 3, this.ClientRectangle.Y, this.ClientRectangle.Width - 6, this.ClientRectangle.Height));
				}
				
				//	Draw each tabpage from right to left.  We do it this way to handle
				//	the overlap correctly.
				if (this.Multiline) {
					for (int row = 0; row < this.RowCount; row++) {
						for (int index = this.TabCount - 1; index >= 0; index--) {
							if (index != this.SelectedIndex && (this.RowCount == 1 || this.GetTabRow(index) == row)) {
								this.DrawTabPage(index, this._BufferGraphics);
							}
						}
					}
				} else {
					for (int index = this.TabCount - 1; index >= 0; index--) {
						if (index != this.SelectedIndex) {
							this.DrawTabPage(index, this._BufferGraphics);
						}
					}
				}

				//	The selected tab must be drawn last so it appears on top.
				if (this.SelectedIndex > -1) {
					this.DrawTabPage(this.SelectedIndex, this._BufferGraphics);
				}
			}
			this._BufferGraphics.Flush();
			
			//	Now paint this to the screen
			
			NativeMethods.PAINTSTRUCT paintStruct = new NativeMethods.PAINTSTRUCT();
			NativeMethods.BeginPaint(m.HWnd, ref paintStruct);
			
			//	We want to paint the whole tabstrip and border every time
			//	so that the hot areas update correctly, along with any overlaps
			
			//	Don't use the supplied graphics object as that does not cover the
			//	whole control and we want the entire tab region repainted every time.
			//using(Graphics screenGraphics = Graphics.FromHdc(paintStruct.hdc))
			using (Graphics screenGraphics = this.CreateGraphics()) {
				
				//	paint the tabs etc.
				screenGraphics.DrawImageUnscaled(this._BackBuffer, 0, 0);
			}
			NativeMethods.EndPaint(m.HWnd, ref paintStruct);
		}
		
		protected void PaintTransparentBackground(Graphics graphics, Rectangle clipRect)
		{
			graphics.Clear(Color.Transparent);

			if ((this.Parent != null)) {
				clipRect.Offset(this.Location);
				PaintEventArgs e = new PaintEventArgs(graphics, clipRect);
				GraphicsState state = graphics.Save();
				graphics.SmoothingMode = SmoothingMode.HighSpeed;
				try {
					graphics.TranslateTransform((float)-this.Location.X, (float)-this.Location.Y);
					this.InvokePaintBackground(this.Parent, e);
					this.InvokePaint(this.Parent, e);
				}
	
				finally {
					graphics.Restore(state);
					clipRect.Offset(-this.Location.X, -this.Location.Y);
				}
			}
		}
		
		private void DrawTabPage(int index, Graphics graphics)
		{
			graphics.SmoothingMode = SmoothingMode.HighSpeed;
			using (GraphicsPath path = this.GetTabPageBorder(index)) {
				
				//	Paint the background
				using (Brush fillBrush = this.GetPageBackgroundBrush(index)){
					graphics.FillPath(fillBrush, path);
				}
				
				//	Paint the tab
				using (GraphicsPath tabpath = this.GetTabBorder(index)) {
					using (Brush fillBrush = this.GetTabBackgroundBrush(index)){
						graphics.FillPath(fillBrush, tabpath);
					}
				}
				
				//	Draw any image
				this.DrawTabImage(index, graphics);

				//	Draw the text
				this.DrawTabText(index, graphics);

				//	Paint the border
				graphics.SmoothingMode = SmoothingMode.HighQuality;
				
				Pen borderPen = null;
				if (index == this.SelectedIndex){
					if (this._borderColorSelected.IsEmpty){
						borderPen = new Pen(ThemedColors.ToolBorder);
					} else {
						borderPen = new Pen(this._borderColorSelected);
					}
				} else {
					if (this._borderColor.IsEmpty){
						borderPen = new Pen(SystemColors.ControlDark);	
					} else {
						borderPen = new Pen(this._borderColor);
					}
				}
				graphics.DrawPath(borderPen, path); 
				borderPen.Dispose();
			}
		}

		private void DrawTabText(int index, Graphics graphics)
		{
			
			Rectangle tabBounds = this.GetTabTextRect(index);
			
			if (this.SelectedIndex == index) {
				graphics.DrawString(this.TabPages[index].Text, this.Font, SystemBrushes.ControlText, tabBounds, this.GetStringFormat());
			} else {
				if (this.TabPages[index].Enabled) {
					graphics.DrawString(this.TabPages[index].Text, this.Font, SystemBrushes.ControlText, tabBounds, this.GetStringFormat());
				} else {
					graphics.DrawString(this.TabPages[index].Text, this.Font, SystemBrushes.ControlDark, tabBounds, this.GetStringFormat());
				}
			}
		}

		private void DrawTabImage(int index, Graphics graphics){
			Image tabImage = null;
			if (this.TabPages[index].ImageIndex > -1 && this.ImageList != null && this.ImageList.Images.Count > this.TabPages[index].ImageIndex){
				tabImage = this.ImageList.Images[this.TabPages[index].ImageIndex];
			} else if (this.TabPages[index].ImageKey.Trim().Length > 0 && this.ImageList != null && this.ImageList.Images.ContainsKey(this.TabPages[index].ImageKey)) {
				tabImage = this.ImageList.Images[this.TabPages[index].ImageKey];
			}

			if (tabImage != null) {
				Rectangle imageRect = this.GetTabImageRect(index);
				if (this.TabPages[index].Enabled){
					graphics.DrawImage(tabImage, imageRect);
				} else {
					ControlPaint.DrawImageDisabled(graphics, tabImage, imageRect.X, imageRect.Y, Color.Transparent);
				}
			}
		}

		#endregion

		#region String formatting and tab background brushes

		private StringFormat GetStringFormat()
		{
			StringFormat format = null;
			
			//	Rotate Text by 90 degrees for left and right tabs
			switch (this.Alignment) {
				case TabAlignment.Top:
				case TabAlignment.Bottom:
					format = new StringFormat();
					break;
				case TabAlignment.Left:
				case TabAlignment.Right:
					format = new StringFormat(StringFormatFlags.DirectionVertical);
					break;
			}
			format.Alignment = StringAlignment.Center;
			format.LineAlignment = StringAlignment.Center;
			return format;
		}

		private Brush GetPageBackgroundBrush(int index){

			//	Capture the colours dependant on selection state of the tab
			Color light = Color.FromArgb(242, 242, 242);
			
			if (this.SelectedIndex == index) {
				light = SystemColors.Window;
			} else if (!this.TabPages[index].Enabled){
				light = Color.FromArgb(207, 207, 207);
			} else if (this.HotTrack && index == this.ActiveIndex){
				//	Enable hot tracking
				light = Color.FromArgb(234, 246, 253);
			}
			
			return new SolidBrush(light);
		}

		private Brush GetTabBackgroundBrush(int index){
			LinearGradientBrush fillBrush = null;

			//	Capture the colours dependant on selection state of the tab
			Color dark = Color.FromArgb(207, 207, 207);
			Color light = Color.FromArgb(242, 242, 242);
			
			if (this.SelectedIndex == index) {
				dark = SystemColors.ControlLight;
				light = SystemColors.Window;
			} else if (!this.TabPages[index].Enabled){
				light = dark;
			} else if (this.HotTrack && index == this.ActiveIndex){
				//	Enable hot tracking
				light = Color.FromArgb(234, 246, 253);
				dark = Color.FromArgb(167, 217, 245);
			}
			
			//	Get the correctly aligned gradient
			Rectangle tabBounds = this.GetTabRectAdjusted(index);
			tabBounds.Inflate(3,3);
			tabBounds.X -= 1;
			tabBounds.Y -= 1;
			switch (this.Alignment) {
				case TabAlignment.Top:
					if (this.SelectedIndex == index) {
						dark = light;
					}
					fillBrush = new LinearGradientBrush(tabBounds, light, dark, LinearGradientMode.Vertical);
					break;
				case TabAlignment.Bottom:
					fillBrush = new LinearGradientBrush(tabBounds, light, dark, LinearGradientMode.Vertical);
					break;
				case TabAlignment.Left:
					fillBrush = new LinearGradientBrush(tabBounds, dark, light, LinearGradientMode.Horizontal);
					break;
				case TabAlignment.Right:
					fillBrush = new LinearGradientBrush(tabBounds, light, dark, LinearGradientMode.Horizontal);
					break;
			}
			
			//	Add the blend
			fillBrush.Blend = this.GetBackgroundBlend();
			
			return fillBrush;
		}

		private Blend GetBackgroundBlend(){
			float[] relativeIntensities = new float[]{0f, 0.7f, 1f};
			float[] relativePositions = new float[]{0f, 0.6f, 1f};

			//	Glass look to top aligned tabs
			if (this.Alignment == TabAlignment.Top){
				relativeIntensities = new float[]{0f, 0.3f, 0.8f, 1f};
				relativePositions = new float[]{0f, 0.5f, 0.51f, 1f};
			}
			
			Blend blend = new Blend();
			blend.Factors = relativeIntensities;
			blend.Positions = relativePositions;
			
			return blend;
		}

		#endregion

		#region Tab borders and bounds properties
		
		private GraphicsPath GetTabPageBorder(int index){
			
			GraphicsPath path = new GraphicsPath();
			Rectangle pageBounds = this.GetPageBounds(index);
			Rectangle tabBounds = this.GetTabRectAdjusted(index);
			
			this.AddTabBorder(path, tabBounds);
			this.AddPageBorder(path, pageBounds, tabBounds);
			
			path.CloseFigure();
			return path;
		}

		private GraphicsPath GetTabBorder(int index){
			
			GraphicsPath path = new GraphicsPath();
			Rectangle tabBounds = this.GetTabRectAdjusted(index);
			
			this.AddTabBorder(path, tabBounds);
			
			path.CloseFigure();
			return path;
		}

		private Rectangle GetPageBounds(int index){
			Rectangle pageBounds = this.TabPages[index].Bounds;
			pageBounds.Width += 1;
			pageBounds.Height += 1;
			pageBounds.X -= 1;
			pageBounds.Y -= 1;
			return pageBounds;
		}

		private Rectangle GetTabRectAdjusted(int index){
			
			Rectangle tabBounds = this.GetTabRect(index);
			bool firstTabinRow = (index == 0);
			if (!firstTabinRow){
				if (this.Alignment <= TabAlignment.Bottom) {
					if (tabBounds.X == 2){
						firstTabinRow = true;
					}
				} else {
					if (tabBounds.Y == 2){
						firstTabinRow = true;
					}
				}
			}
			
			//	Expand to overlap the tabpage
			switch (this.Alignment) {
				case TabAlignment.Top:
					tabBounds.Height += 2;
					break;
				case TabAlignment.Bottom:
					tabBounds.Height += 2;
					tabBounds.Y -= 2;
					break;
				case TabAlignment.Left:
					tabBounds.Width += 2;
					break;
				case TabAlignment.Right:
					tabBounds.X -= 2;
					tabBounds.Width += 2;
					break;
			}
			
			//	Adjust first tab in the row to align with tabpage
			if (firstTabinRow) {
				if (this.Alignment <= TabAlignment.Bottom) {
					tabBounds.X += 1;
					tabBounds.Width -= 1;
				} else {
					tabBounds.Y += 1;
					tabBounds.Height -= 1;
				}
			}

			//	Make non-SelectedTabs smaller for default view
			if (this._Style == TabStyle.Default) {
				if (index != this.SelectedIndex) {
					switch (this.Alignment) {
						case TabAlignment.Top:
							tabBounds.Y += 1;
							tabBounds.Height -= 1;
							break;
						case TabAlignment.Bottom:
							tabBounds.Height -= 1;
							break;
						case TabAlignment.Left:
							tabBounds.X += 1;
							tabBounds.Width -= 1;
							break;
						case TabAlignment.Right:
							tabBounds.Width -= 1;
							break;
					}
				} else {
					switch (this.Alignment) {
						case TabAlignment.Top:
							tabBounds.Y -= 1;
							tabBounds.Height += 1;
							
							if (firstTabinRow){
								tabBounds.Width += 1;
							} else {
								tabBounds.X -= 1;
								tabBounds.Width += 2;
							}
							break;
						case TabAlignment.Bottom:
							tabBounds.Height += 1;

							if (firstTabinRow){
								tabBounds.Width += 1;
							} else {
								tabBounds.X -= 1;
								tabBounds.Width += 2;
							}
							break;
						case TabAlignment.Left:
							tabBounds.X -= 1;
							tabBounds.Width += 1;

							if (firstTabinRow){
								tabBounds.Height += 1;
							} else {
								tabBounds.Y -= 1;
								tabBounds.Height += 2;
							}
							break;
						case TabAlignment.Right:
							tabBounds.Width += 1;
							if (firstTabinRow){
								tabBounds.Height += 1;
							} else {
								tabBounds.Y -= 1;
								tabBounds.Height += 2;
							}
							break;
					}
					
				}
			}

			//	Greate Overlap unless first tab in the row to align with tabpage
			if (!firstTabinRow && this._Overlap > 0) {
				if (this.Alignment <= TabAlignment.Bottom) {
					tabBounds.X -= this._Overlap;
					tabBounds.Width += this._Overlap;
				} else {
					tabBounds.Y -= this._Overlap;
					tabBounds.Height += this._Overlap;
				}
			}
			
			return tabBounds;
		}

		private Rectangle GetTabTextRect(int index){
			Rectangle tabBounds = this.GetTabRectAdjusted(index);
			
			//	If using leading edge (Visual Studio Style) then adjust to allow for it.
			if (this._Style == TabStyle.VisualStudio) {
				if (this.Alignment <= TabAlignment.Bottom) {
					tabBounds.X += (tabBounds.Height - this._Overlap);
					tabBounds.Width -= (tabBounds.Height - this._Overlap);
				} else {
					tabBounds.Y += (tabBounds.Width - this._Overlap);
					tabBounds.Height -= (tabBounds.Width - this._Overlap);
				}
			}

			//	If there is an image allow for it
			if (this.ImageList != null && (this.TabPages[index].ImageIndex > -1 || !string.IsNullOrEmpty(this.TabPages[index].ImageKey))) {
				if (this._ImageAlign == NativeMethods.AnyLeftAlign) {
					if (this.Alignment <= TabAlignment.Bottom) {
						tabBounds.X += 20;
						tabBounds.Width -= 20;
					} else {
						tabBounds.Y += 20;
						tabBounds.Height -= 20;
					}
				} else if (this._ImageAlign == NativeMethods.AnyCenterAlign) {
				} else {
					if (this.Alignment <= TabAlignment.Bottom) {
						tabBounds.Width -= 20;
					} else {
						tabBounds.Height -= 20;
					}
				}
			}
			return tabBounds;
		}

		public int GetTabRow(int index){
			//	All calculations will use this rect as the base point
			//	because the itemsize does not return the correct width.
			Rectangle rect = this.GetTabRect(index);
			
			int row = -1;
			
			switch (this.Alignment) {
				case TabAlignment.Top:
					row = (rect.Y - 2)/rect.Height;
					break;
				case TabAlignment.Bottom:
					row = ((this.Height - rect.Y - 2)/rect.Height) - 1;
					break;
				case TabAlignment.Left:
					row = (rect.X - 2)/rect.Width;
					break;
				case TabAlignment.Right:
					row = ((this.Width - rect.X - 2)/rect.Width) - 1;
					break;
			}
			return row;
		}

		public Point GetTabPosition(int index){

			//	If we are not multiline then the column is the index and the row is 0.
			if (!this.Multiline){
				return new Point(0, index);
			}
			
			//	If there is only one row then the column is the index
			if (this.RowCount == 1){
				return new Point(0, index);
			}
			
			//	We are in a true multi-row scenario
			int row = this.GetTabRow(index);
			Rectangle rect = this.GetTabRect(index);
			int column = -1;
			
			//	Scan from left to right along rows, skipping to next row if it is not the one we want.
			for (int testIndex = 0; testIndex < this.TabCount; testIndex ++){
				Rectangle testRect = this.GetTabRect(testIndex);
				if (this.Alignment <= TabAlignment.Bottom){
					if (testRect.Y == rect.Y){
						column += 1;
					}
				} else {
					if (testRect.X == rect.X){
						column += 1;
					}
				}
				
				if (testRect.Location.Equals(rect.Location)){
					return new Point(row, column);
				}
			}
			
			return new Point(0, 0);
		}
		
		private void AddPageBorder(GraphicsPath path, Rectangle pageBounds, Rectangle tabBounds){
			switch (this.Alignment) {
				case TabAlignment.Top:
					path.AddLine(tabBounds.Right, pageBounds.Y, pageBounds.Right, pageBounds.Y);
					path.AddLine(pageBounds.Right, pageBounds.Y, pageBounds.Right, pageBounds.Bottom);
					path.AddLine(pageBounds.Right, pageBounds.Bottom, pageBounds.X, pageBounds.Bottom);
					path.AddLine(pageBounds.X, pageBounds.Bottom, pageBounds.X, pageBounds.Y);
					path.AddLine(pageBounds.X, pageBounds.Y, tabBounds.X, pageBounds.Y);
					break;
				case TabAlignment.Bottom:
					path.AddLine(tabBounds.X, pageBounds.Bottom, pageBounds.X, pageBounds.Bottom);
					path.AddLine(pageBounds.X, pageBounds.Bottom, pageBounds.X, pageBounds.Y);
					path.AddLine(pageBounds.X, pageBounds.Y, pageBounds.Right, pageBounds.Y);
					path.AddLine(pageBounds.Right, pageBounds.Y, pageBounds.Right, pageBounds.Bottom);
					path.AddLine(pageBounds.Right, pageBounds.Bottom, tabBounds.Right, pageBounds.Bottom);
					break;
				case TabAlignment.Left:
					path.AddLine(pageBounds.X, tabBounds.Y, pageBounds.X, pageBounds.Y);
					path.AddLine(pageBounds.X, pageBounds.Y, pageBounds.Right, pageBounds.Y);
					path.AddLine(pageBounds.Right, pageBounds.Y, pageBounds.Right, pageBounds.Bottom);
					path.AddLine(pageBounds.Right, pageBounds.Bottom, pageBounds.X, pageBounds.Bottom);
					path.AddLine(pageBounds.X, pageBounds.Bottom, pageBounds.X, tabBounds.Bottom);
					break;
				case TabAlignment.Right:
					path.AddLine(pageBounds.Right, tabBounds.Bottom, pageBounds.Right, pageBounds.Bottom);
					path.AddLine(pageBounds.Right, pageBounds.Bottom, pageBounds.X, pageBounds.Bottom);
					path.AddLine(pageBounds.X, pageBounds.Bottom, pageBounds.X, pageBounds.Y);
					path.AddLine(pageBounds.X, pageBounds.Y, pageBounds.Right, pageBounds.Y);
					path.AddLine(pageBounds.Right, pageBounds.Y, pageBounds.Right, tabBounds.Y);
					break;
			}
		}

		private Rectangle GetTabImageRect(int index){
			Rectangle imageRect = new Rectangle();
			Rectangle rect = this.GetTabRectAdjusted(index);

			if (this.Alignment <= TabAlignment.Bottom) {
				if (this._ImageAlign == NativeMethods.AnyLeftAlign){
					imageRect = new Rectangle(rect.X + 4, rect.Y + 1, 16, 16);
				} else if (this._ImageAlign == NativeMethods.AnyCenterAlign){
					imageRect = new Rectangle(rect.X + (int)Math.Floor((double)((rect.Right - rect.X - rect.Height + 2)/2)), rect.Y + 1, 16, 16);
				} else {
					imageRect = new Rectangle(rect.Right - rect.Height - 4, rect.Y + 1, 16, 16);
				}
			} else {
				if (this._ImageAlign == NativeMethods.AnyLeftAlign){
					imageRect = new Rectangle(rect.X + 1, rect.Y + 4, 16, 16);
				} else if (this._ImageAlign == NativeMethods.AnyCenterAlign){
					imageRect = new Rectangle(rect.X + 1, rect.Y + (int)Math.Floor((double)((rect.Bottom - rect.Y - rect.Width + 2)/2)), 16, 16);
				} else {
					imageRect = new Rectangle(rect.X + 1, rect.Bottom - rect.Width - 4, 16, 16);
				}
			}
			return imageRect;
		}

		#endregion

		#region Custom tab shapes

		internal protected virtual void AddTabBorder(GraphicsPath path, Rectangle tabBounds){
			switch (this._Style) {
				case TabStyle.VisualStudio:
					this.AddVSTabBorder(path, tabBounds);
					break;
				case TabStyle.Angled:
					this.AddAngledTabBorder(path, tabBounds);
					break;
				case TabStyle.Rounded:
					this.AddRoundedTabBorder(path, tabBounds);
					break;
				case TabStyle.Default:
					this.AddDefaultTabBorder(path, tabBounds);
					break;
				case TabStyle.Chrome:
					this.AddChromeTabBorder(path, tabBounds);
					break;
			}
		}

		private void AddAngledTabBorder(GraphicsPath path, Rectangle tabBounds){

			switch (this.Alignment) {
				case TabAlignment.Top:
					path.AddLine(tabBounds.X, tabBounds.Bottom, tabBounds.X + this._Radius - 2, tabBounds.Y + 2);
					path.AddLine(tabBounds.X + this._Radius, tabBounds.Y, tabBounds.Right - this._Radius, tabBounds.Y);
					path.AddLine(tabBounds.Right - this._Radius + 2, tabBounds.Y + 2, tabBounds.Right, tabBounds.Bottom);
					break;
				case TabAlignment.Bottom:
					path.AddLine(tabBounds.Right, tabBounds.Y, tabBounds.Right - this._Radius + 2, tabBounds.Bottom - 2);
					path.AddLine(tabBounds.Right - this._Radius, tabBounds.Bottom, tabBounds.X + this._Radius, tabBounds.Bottom);
					path.AddLine(tabBounds.X + this._Radius - 2, tabBounds.Bottom - 2, tabBounds.X, tabBounds.Y);
					break;
				case TabAlignment.Left:
					path.AddLine(tabBounds.Right, tabBounds.Bottom, tabBounds.X + 2, tabBounds.Bottom - this._Radius + 2);
					path.AddLine(tabBounds.X, tabBounds.Bottom - this._Radius, tabBounds.X, tabBounds.Y + this._Radius);
					path.AddLine(tabBounds.X + 2, tabBounds.Y + this._Radius - 2, tabBounds.Right, tabBounds.Y);
					break;
				case TabAlignment.Right:
					path.AddLine(tabBounds.X, tabBounds.Y, tabBounds.Right - 2, tabBounds.Y + this._Radius - 2);
					path.AddLine(tabBounds.Right, tabBounds.Y + this._Radius, tabBounds.Right, tabBounds.Bottom - this._Radius);
					path.AddLine(tabBounds.Right - 2, tabBounds.Bottom - this._Radius + 2, tabBounds.X, tabBounds.Bottom);
					break;
			}
		}
		
		private void AddChromeTabBorder(GraphicsPath path, Rectangle tabBounds){
			int spread;
			int eigth;
			int sixth;
			int quarter;

			if (this.Alignment <= TabAlignment.Bottom){
				spread = (int)Math.Floor((decimal)tabBounds.Height * 2/3);
				eigth = (int)Math.Floor((decimal)tabBounds.Height * 1/8);
				sixth = (int)Math.Floor((decimal)tabBounds.Height * 1/6);
				quarter = (int)Math.Floor((decimal)tabBounds.Height * 1/4);
			} else {
				spread = (int)Math.Floor((decimal)tabBounds.Width * 2/3);
				eigth = (int)Math.Floor((decimal)tabBounds.Width * 1/8);
				sixth = (int)Math.Floor((decimal)tabBounds.Width * 1/6);
				quarter = (int)Math.Floor((decimal)tabBounds.Width * 1/4);
			}
			
			switch (this.Alignment) {
				case TabAlignment.Top:
					
					path.AddCurve(new Point[] {  new Point(tabBounds.X, tabBounds.Bottom)
					              		,new Point(tabBounds.X + sixth, tabBounds.Bottom - eigth)
					              		,new Point(tabBounds.X + spread - quarter, tabBounds.Y + eigth)
					              		,new Point(tabBounds.X + spread, tabBounds.Y)});
					path.AddLine(tabBounds.X + spread, tabBounds.Y, tabBounds.Right - spread, tabBounds.Y);
					path.AddCurve(new Point[] {  new Point(tabBounds.Right - spread, tabBounds.Y)
					              		,new Point(tabBounds.Right - spread + quarter, tabBounds.Y + eigth)
					              		,new Point(tabBounds.Right - sixth, tabBounds.Bottom - eigth)
					              		,new Point(tabBounds.Right, tabBounds.Bottom)});
					break;
				case TabAlignment.Bottom:
					path.AddCurve(new Point[] {  new Point(tabBounds.Right, tabBounds.Y)
					              		,new Point(tabBounds.Right - sixth, tabBounds.Y + eigth)
					              		,new Point(tabBounds.Right - spread + quarter, tabBounds.Bottom - eigth)
					              		,new Point(tabBounds.Right - spread, tabBounds.Bottom)});
					path.AddLine(tabBounds.Right - spread, tabBounds.Bottom, tabBounds.X + spread, tabBounds.Bottom);
					path.AddCurve(new Point[] {  new Point(tabBounds.X + spread, tabBounds.Bottom)
					              		,new Point(tabBounds.X + spread - quarter, tabBounds.Bottom - eigth)
					              		,new Point(tabBounds.X + sixth, tabBounds.Y + eigth)
					              		,new Point(tabBounds.X, tabBounds.Y)});
					break;
				case TabAlignment.Left:
					path.AddCurve(new Point[] {  new Point(tabBounds.Right, tabBounds.Bottom)
					              		,new Point(tabBounds.Right - eigth, tabBounds.Bottom - sixth)
					              		,new Point(tabBounds.X + eigth, tabBounds.Bottom - spread + quarter)
					              		,new Point(tabBounds.X, tabBounds.Bottom - spread)});
					path.AddLine(tabBounds.X, tabBounds.Bottom - spread, tabBounds.X ,tabBounds.Y + spread);
					path.AddCurve(new Point[] {  new Point(tabBounds.X, tabBounds.Y + spread)
					              		,new Point(tabBounds.X + eigth, tabBounds.Y + spread - quarter)
					              		,new Point(tabBounds.Right - eigth, tabBounds.Y + sixth)
					              		,new Point(tabBounds.Right, tabBounds.Y)});

					break;
				case TabAlignment.Right:
					path.AddCurve(new Point[] {  new Point(tabBounds.X, tabBounds.Y)
					              		,new Point(tabBounds.X + eigth, tabBounds.Y + sixth)
					              		,new Point(tabBounds.Right - eigth, tabBounds.Y + spread - quarter)
					              		,new Point(tabBounds.Right, tabBounds.Y + spread)});
					path.AddLine(tabBounds.Right, tabBounds.Y + spread, tabBounds.Right, tabBounds.Bottom - spread);
					path.AddCurve(new Point[] {  new Point(tabBounds.Right, tabBounds.Bottom - spread)
					              		,new Point(tabBounds.Right - eigth, tabBounds.Bottom - spread + quarter)
					              		,new Point(tabBounds.X + eigth, tabBounds.Bottom - sixth)
					              		,new Point(tabBounds.X, tabBounds.Bottom)});
					break;
			}
		}
		
		private void AddRoundedTabBorder(GraphicsPath path, Rectangle tabBounds){
			switch (this.Alignment) {
				case TabAlignment.Top:
					path.AddLine(tabBounds.X, tabBounds.Bottom, tabBounds.X, tabBounds.Y + this._Radius);
					path.AddArc(tabBounds.X, tabBounds.Y, this._Radius * 2, this._Radius * 2, 180, 90);
					path.AddLine(tabBounds.X + this._Radius, tabBounds.Y, tabBounds.Right - this._Radius, tabBounds.Y);
					path.AddArc(tabBounds.Right - this._Radius * 2, tabBounds.Y, this._Radius * 2, this._Radius * 2, 270, 90);
					path.AddLine(tabBounds.Right, tabBounds.Y + this._Radius, tabBounds.Right, tabBounds.Bottom);
					break;
				case TabAlignment.Bottom:
					path.AddLine(tabBounds.Right, tabBounds.Y, tabBounds.Right, tabBounds.Bottom - this._Radius);
					path.AddArc(tabBounds.Right - this._Radius * 2, tabBounds.Bottom - this._Radius * 2, this._Radius * 2, this._Radius * 2, 0, 90);
					path.AddLine(tabBounds.Right - this._Radius, tabBounds.Bottom, tabBounds.X + this._Radius, tabBounds.Bottom);
					path.AddArc(tabBounds.X, tabBounds.Bottom - this._Radius * 2, this._Radius * 2, this._Radius * 2, 90, 90);
					path.AddLine(tabBounds.X, tabBounds.Bottom - this._Radius, tabBounds.X, tabBounds.Y);
					break;
				case TabAlignment.Left:
					path.AddLine(tabBounds.Right, tabBounds.Bottom, tabBounds.X + this._Radius, tabBounds.Bottom);
					path.AddArc(tabBounds.X, tabBounds.Bottom - this._Radius * 2, this._Radius * 2, this._Radius * 2, 90, 90);
					path.AddLine(tabBounds.X, tabBounds.Bottom - this._Radius, tabBounds.X, tabBounds.Y + this._Radius);
					path.AddArc(tabBounds.X, tabBounds.Y, this._Radius * 2, this._Radius * 2, 180, 90);
					path.AddLine(tabBounds.X + this._Radius, tabBounds.Y, tabBounds.Right, tabBounds.Y);
					break;
				case TabAlignment.Right:
					path.AddLine(tabBounds.X, tabBounds.Y, tabBounds.Right - this._Radius, tabBounds.Y);
					path.AddArc(tabBounds.Right - this._Radius * 2, tabBounds.Y, this._Radius * 2, this._Radius * 2, 270, 90);
					path.AddLine(tabBounds.Right, tabBounds.Y + this._Radius, tabBounds.Right, tabBounds.Bottom - this._Radius);
					path.AddArc(tabBounds.Right - this._Radius * 2, tabBounds.Bottom - this._Radius * 2, this._Radius * 2, this._Radius * 2, 0, 90);
					path.AddLine(tabBounds.Right - this._Radius, tabBounds.Bottom, tabBounds.X, tabBounds.Bottom);
					break;
			}
		}

		private void AddDefaultTabBorder(GraphicsPath path, Rectangle tabBounds){
			switch (this.Alignment) {
				case TabAlignment.Top:
					path.AddLine(tabBounds.X, tabBounds.Bottom, tabBounds.X, tabBounds.Y);
					path.AddLine(tabBounds.X, tabBounds.Y, tabBounds.Right, tabBounds.Y);
					path.AddLine(tabBounds.Right, tabBounds.Y, tabBounds.Right, tabBounds.Bottom);
					break;
				case TabAlignment.Bottom:
					path.AddLine(tabBounds.Right, tabBounds.Y, tabBounds.Right, tabBounds.Bottom);
					path.AddLine(tabBounds.Right, tabBounds.Bottom, tabBounds.X, tabBounds.Bottom);
					path.AddLine(tabBounds.X, tabBounds.Bottom, tabBounds.X, tabBounds.Y);
					break;
				case TabAlignment.Left:
					path.AddLine(tabBounds.Right, tabBounds.Bottom, tabBounds.X, tabBounds.Bottom);
					path.AddLine(tabBounds.X, tabBounds.Bottom, tabBounds.X, tabBounds.Y);
					path.AddLine(tabBounds.X, tabBounds.Y, tabBounds.Right, tabBounds.Y);
					break;
				case TabAlignment.Right:
					path.AddLine(tabBounds.X, tabBounds.Y, tabBounds.Right, tabBounds.Y);
					path.AddLine(tabBounds.Right, tabBounds.Y, tabBounds.Right, tabBounds.Bottom);
					path.AddLine(tabBounds.Right, tabBounds.Bottom, tabBounds.X, tabBounds.Bottom);
					break;
			}
		}
		
		private void AddVSTabBorder(GraphicsPath path, Rectangle tabBounds){

			switch (this.Alignment) {
				case TabAlignment.Top:
					path.AddLine(tabBounds.X, tabBounds.Bottom, tabBounds.X + tabBounds.Height - 4, tabBounds.Y + 2);
					path.AddLine(tabBounds.X + tabBounds.Height, tabBounds.Y, tabBounds.Right - 3, tabBounds.Y);
					path.AddArc(tabBounds.Right - 6, tabBounds.Y, 6, 6, 270, 90);
					path.AddLine(tabBounds.Right, tabBounds.Y + 3, tabBounds.Right, tabBounds.Bottom);
					break;
				case TabAlignment.Bottom:
					path.AddLine(tabBounds.Right, tabBounds.Y, tabBounds.Right, tabBounds.Bottom - 3);
					path.AddArc(tabBounds.Right - 6, tabBounds.Bottom - 6, 6, 6, 0, 90);
					path.AddLine(tabBounds.Right - 3, tabBounds.Bottom, tabBounds.X + tabBounds.Height, tabBounds.Bottom);
					path.AddLine(tabBounds.X + tabBounds.Height - 4, tabBounds.Bottom - 2, tabBounds.X, tabBounds.Y);
					break;
				case TabAlignment.Left:
					path.AddLine(tabBounds.Right, tabBounds.Bottom, tabBounds.X + 3, tabBounds.Bottom);
					path.AddArc(tabBounds.X, tabBounds.Bottom - 6, 6, 6, 90, 90);
					path.AddLine(tabBounds.X, tabBounds.Bottom - 3, tabBounds.X, tabBounds.Y + tabBounds.Width);
					path.AddLine(tabBounds.X + 2, tabBounds.Y + tabBounds.Width - 4, tabBounds.Right, tabBounds.Y);
					break;
				case TabAlignment.Right:
					path.AddLine(tabBounds.X, tabBounds.Y, tabBounds.Right - 2, tabBounds.Y + tabBounds.Width - 4);
					path.AddLine(tabBounds.Right, tabBounds.Y + tabBounds.Width, tabBounds.Right, tabBounds.Bottom - 3);
					path.AddArc(tabBounds.Right - 6, tabBounds.Bottom - 6, 6, 6, 0, 90);
					path.AddLine(tabBounds.Right - 3, tabBounds.Bottom, tabBounds.X, tabBounds.Bottom);
					break;
			}
		}

		#endregion
		
	}
}
