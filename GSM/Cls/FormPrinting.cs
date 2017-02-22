// Version 2.2
// C#
// Accept control other than a Form
// You can add your own print functions for specific controls and for report title formatting
// Controls can expand
// Expendable TextBox multiline added
// Expandable ListBox added
// Expendable FlexGrid added (Grid from Component One : code in comments only [no references] )
// DataGrid printing
// More comments in source code
// Report printed can continue on many pages
// Can display a trace of controls printed
// Can print page number using your own String.Format
// Report can be print to a multiframe Tif file (example: for faxing)
// Better algorithm to compute extension of container control with rezizable side by side childs
// Test

using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Collections;
using System.Windows.Forms;
using System.Data;
using System.Drawing.Imaging;

namespace FormPrinting
{

    public class FormPrinting
    {
        #region "public section"
        // publics property that can be changed
        public bool TextBoxBoxed = false; // box around TextBox controls
        public bool TabControlBoxed = true; // box around TabControl controls
        public bool LabelInBold = true; // Print all labels in bold
        public bool PrintPreview = true; // enabled Print preview instead of direct printing
        public bool DisabledControlsInGray = false; // Color for disabled controls
        public bool PageNumbering = false; //If true, reserve space at the bottom of each page and print page number
        public string PageNumberingFormat = "ตฺ{0}าณ"; // String format for page number
        public OrientationENum Orientation = OrientationENum.Automatic; // choose print orientation (Automatic, Protrait or Landscape)
        public ControlPrinting DelegatePrintingReportTitle; // Function that will print report title. Can be changed
        //public bool ControlsCanExpand = true; // Indicate if class accept control's expansion
        public Single TopMargin = 0; //If 0, use default margin, else use this
        public Single BottomMargin = 0; //If 0, use default margin, else use this
        public HorizontalAlignment HAlignment = HorizontalAlignment.Center;

        public enum OrientationENum
        {
            Automatic = 1,
            Portrait = 2,
            Lanscape = 3
        }

        public enum ParentControlPrinting
        {
            BeforeChilds = 1,
            AfterChilds = 2,
        }

        // delegate for providing of print function by control type
        public delegate void ControlPrinting(System.Windows.Forms.Control c,
            ParentControlPrinting typePrint,
            MultiPageManagement mp,
            Single x, Single y,
            ref Single extendedHeight, out bool ScanForChildControls);

        //Constructor
        public FormPrinting(System.Windows.Forms.Control f)
        {
            _f = f;
            _TextBoxLikeControl = new ArrayList();
            _DelegatesforControls = new ArrayList();
            _Pen = new Pen(Color.Black);

            // add build-in types and functions

            AddTextBoxLikeControl("ComboBox");
            AddTextBoxLikeControl("DateTimePicker");
            AddTextBoxLikeControl("DateTimeSlicker");
            AddTextBoxLikeControl("NumericUpDown");

            AddDelegateToPrintControl("TextBox", new ControlPrinting(PrintTextBox));
            AddDelegateToPrintControl("System.Windows.Forms.Label", new ControlPrinting(PrintLabel));
            AddDelegateToPrintControl("System.Windows.Forms.CheckBox", new ControlPrinting(PrintCheckBox));
            AddDelegateToPrintControl("System.Windows.Forms.RadioButton", new ControlPrinting(PrintRadioButton));
            AddDelegateToPrintControl("System.Windows.Forms.GroupBox", new ControlPrinting(PrintGroupBox));
            AddDelegateToPrintControl("System.Windows.Forms.Panel", new ControlPrinting(PrintPanel));
            AddDelegateToPrintControl("System.Windows.Forms.TabControl", new ControlPrinting(PrintTabControl));
            AddDelegateToPrintControl("System.Windows.Forms.PictureBox", new ControlPrinting(PrintPictureBox));
            AddDelegateToPrintControl("System.Windows.Forms.ListBox", new ControlPrinting(PrintListBox));
            AddDelegateToPrintControl("System.Windows.Forms.DataGrid", new ControlPrinting(PrintDataGrid));
        }

        /// <summary>
        /// Let user add TextBox like control type name
        /// </summary>
        /// <param name="stringType">TextBox like control type name</param>
        public void AddTextBoxLikeControl(string stringType)
        {
            _TextBoxLikeControl.Add(stringType);
        }

        /// <summary>
        /// Let users provide their own print function for specific control type
        /// </summary>
        /// <param name="stringType">Control type name</param>
        /// <param name="printFunction">function (must match with FormPrinting.ControlPrinting delegate)</param>
        public void AddDelegateToPrintControl(string stringType, ControlPrinting printFunction)
        {
            _DelegateforControls d = new _DelegateforControls();
            d.typ = stringType;
            d.PrintFunction = printFunction;
            _DelegatesforControls.Add(d);
        }
        #endregion

        #region  "Private data"
        //		private Font _printFont;
        private Pen _Pen;
        private Brush _Brush;
        private System.Windows.Forms.Control _f;
        private ArrayList _TextBoxLikeControl;
        private System.Single _xform;
        private ArrayList _DelegatesforControls;
        private MultiPageManagement _MultiPage;
        private System.Text.StringBuilder _traceLog;
        private int _indent;

        private class _DelegateforControls
        {
            public string typ;
            public ControlPrinting PrintFunction;
        }
        #endregion

        #region "Managing PrintDocument or TIF object"
        /// <summary>
        /// Launch printing. Calculate start position and orientation
        /// </summary>
        public void Print()
        {
            try
            {
                PrintDocument pd = new PrintDocument();
                InitPrinting(ref pd);
                pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
                pd.DocumentName = _f.Text;
                if (PrintPreview)
                {
                    PrintPreviewDialog pp = new PrintPreviewDialog();
                    pp.Document = pd;
                    pp.WindowState = FormWindowState.Maximized;
                    pp.ShowDialog();
                }
                else
                {
                    pd.Print();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void PrintToTifFile(string fileName)
        {
            try
            {
                // Compute bitmap propertiy about same as default printer
                PrintDocument pd = new PrintDocument();
                InitPrinting(ref pd);
                System.Drawing.Bitmap bitmapAllPages, bitmapAdditionnalPage;
                bitmapAllPages = new Bitmap(pd.DefaultPageSettings.Bounds.Width, pd.DefaultPageSettings.Bounds.Height);
                bitmapAdditionnalPage = new Bitmap(pd.DefaultPageSettings.Bounds.Width, pd.DefaultPageSettings.Bounds.Height);
                Graphics g;

                //Prepare parameters to save multiframe image
                System.Drawing.Imaging.EncoderParameters eps;
                eps = new System.Drawing.Imaging.EncoderParameters();
                bool firstPage = true;

                // Print pages
                do
                {
                    if (firstPage)
                        g = Graphics.FromImage(bitmapAllPages);
                    else
                        g = Graphics.FromImage(bitmapAdditionnalPage);
                    g.Clear(System.Drawing.Color.White);
                    _MultiPage.NewPage(g);
                    Single extendedHeight;
                    Single y;

                    y = 0;
                    extendedHeight = 0;
                    bool scanForChildControls;
                    if (DelegatePrintingReportTitle == null)
                        PrintReportTitle(_f, ParentControlPrinting.BeforeChilds, _MultiPage, _xform, y, ref extendedHeight, out scanForChildControls);
                    else
                        DelegatePrintingReportTitle(_f, ParentControlPrinting.BeforeChilds, _MultiPage, _xform, y, ref extendedHeight, out scanForChildControls);
                    y += extendedHeight;

                    // Print each control on the form
                    Single globalExtendedHeight;
                    PrintControls(_f, _MultiPage, _xform, y, out globalExtendedHeight);

                    if (firstPage)
                    {
                        //Create the parameter and choose multi-frame
                        eps.Param[0] = new EncoderParameter(Encoder.SaveFlag, (long)EncoderValue.MultiFrame);
                        //Get the correct encoder from the list of available encoders
                        ImageCodecInfo[] infos = ImageCodecInfo.GetImageEncoders();
                        int n = 0;
                        while (infos[n].MimeType != "image/tiff")
                            n++;
                        //save the first page
                        bitmapAllPages.Save(fileName + ".tif", infos[n], eps);
                    }
                    else
                    {
                        //Create the parameter and choose FrameDimensionPage
                        eps.Param[0] = new EncoderParameter(Encoder.SaveFlag, (long)EncoderValue.FrameDimensionPage);
                        //save the first page
                        bitmapAllPages.SaveAdd(bitmapAdditionnalPage, eps);
                    }
                    firstPage = false;
                } while (!_MultiPage.LastPage());

                // Flush pages to file
                eps.Param[0] = new EncoderParameter(Encoder.SaveFlag, (long)EncoderValue.Flush);
                bitmapAllPages.SaveAdd(eps);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public string GetTrace()
        {
            return _traceLog.ToString();
        }

        private void InitPrinting(ref PrintDocument pd)
        {

            _traceLog = new System.Text.StringBuilder();
            _indent = 0;
            // Calculate Form position for printing
            switch (Orientation)
            {
                case OrientationENum.Automatic:
                    if (_f.Size.Width > (pd.DefaultPageSettings.Bounds.Width - pd.DefaultPageSettings.Margins.Right - pd.DefaultPageSettings.Margins.Left))
                        pd.DefaultPageSettings.Landscape = true;
                    break;
                case OrientationENum.Lanscape:
                    pd.DefaultPageSettings.Landscape = true;
                    break;
                case OrientationENum.Portrait:
                    pd.DefaultPageSettings.Landscape = false;
                    break;
            }

            // Set page area
            Single pageTop = pd.DefaultPageSettings.Margins.Top;
            Single pageBottom = pd.DefaultPageSettings.Bounds.Height - pd.DefaultPageSettings.Margins.Bottom;
            Single pageLeft = pd.DefaultPageSettings.Margins.Left;
            Single pageRight = pd.DefaultPageSettings.Bounds.Width - pd.DefaultPageSettings.Margins.Right;
            if (TopMargin != 0)
                pageTop = TopMargin;
            if (BottomMargin != 0)
                pageTop = BottomMargin;

            // Calculate left position of print
            switch (this.HAlignment)
            {
                case HorizontalAlignment.Left:
                    _xform = pageLeft;
                    break;
                case HorizontalAlignment.Center:
                    _xform = (pd.DefaultPageSettings.Bounds.Width - _f.Size.Width) / 2;
                    break;
                case HorizontalAlignment.Right:
                    _xform = pageRight - _f.Size.Width;
                    break;
            }

            _MultiPage = new MultiPageManagement(pageTop, pageBottom, pageLeft, pageRight, _f.Font, PageNumbering, PageNumberingFormat);
        }

        /// <summary>
        /// Event handler called by the print document engine. Called for each pages
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ev"></param>
        private void pd_PrintPage(object sender, PrintPageEventArgs ev)
        {
            _MultiPage.NewPage(ev.Graphics);
            Single extendedHeight;
            Single y;

            y = 0;
            extendedHeight = 0;
            bool scanForChildControls;
            if (DelegatePrintingReportTitle == null)
                PrintReportTitle(_f, ParentControlPrinting.BeforeChilds, _MultiPage, _xform, y, ref extendedHeight, out scanForChildControls);
            else
                DelegatePrintingReportTitle(_f, ParentControlPrinting.BeforeChilds, _MultiPage, _xform, y, ref extendedHeight, out scanForChildControls);
            y += extendedHeight;

            // Print each control on the form
            Single globalExtendedHeight;
            PrintControls(_f, _MultiPage, _xform, y, out globalExtendedHeight);

            if (_MultiPage.LastPage())
            {
                ev.HasMorePages = false;
                _MultiPage.ResetPage();
            }
            else
                ev.HasMorePages = true;
        }

        public void PrintReportTitle(System.Windows.Forms.Control c,
            ParentControlPrinting typePrint,
            MultiPageManagement mp,
            Single x, Single y,
            ref Single extendedHeight, out bool ScanForChildControls)
        {
            ScanForChildControls = false;
            Font printFont = new Font(c.Font.Name, (Single)(c.Font.Size * 1.3), FontStyle.Bold);
            float fontHeight = mp.FontHeight(printFont);
            Pen pen = new Pen(Color.Black, 2);
            extendedHeight = fontHeight + 3 + pen.Width + 1;

            mp.BeginPrintUnit(y, extendedHeight);
            mp.DrawString(c.Text, printFont, Brushes.Black, x, y, c.Width, fontHeight);
            y += fontHeight + 3;
            mp.DrawLines(pen, x, y, x + c.Size.Width, y);
            mp.EndPrintUnit();
        }

        #endregion

        #region "Recursive printing of controls"
        /// <summary>
        /// Print child controls of a "parent" control.
        /// Controls are printed from top to bottom.
        /// This is the function that calculate position of each control depending of the extension of child controls
        /// </summary>
        /// <param name="c">"parent" control</param>
        /// <param name="mp">printing page object</param>
        /// <param name="x">X position of "parent" control</param>
        /// <param name="y">Y position of "parent" control</param>
        /// <param name="globalExtendedHeight">necessary height grow of "parent" control to fit with combination of child control growing</param>
        public void PrintControls(System.Windows.Forms.Control c,
            MultiPageManagement mp,
            Single x, Single y,
            out Single globalExtendedHeight)
        {
            int nbCtrl = c.Controls.Count;
            Single[] yPos = new Single[nbCtrl];
            System.Windows.Forms.Control[] controls = new System.Windows.Forms.Control[nbCtrl];

            //Do a list of child controls
            for (int i = 0; i < nbCtrl; i++)
            {
                controls[i] = c.Controls[i];
                yPos[i] = c.Controls[i].Location.Y;
            }

            //Sort them by vertical position
            System.Array.Sort(yPos, controls);

            //Print them from top to bottom
            globalExtendedHeight = 0;
            //Single globalextendedYPos = 0;
            System.Collections.ArrayList extendedYPos = new System.Collections.ArrayList();

            // *****************************************************************
            //		This loop over child controls calculate position of them.
            // Algorithm take care of controls that expand besides and above.
            // It keep an arraylist of original and printed (after expansion) bottom
            // position of expanded control.
            // So control is push down if it was originally below expanded controls
            // *****************************************************************
            for (int i = 0; i < nbCtrl; i++)
            {
                // Set y position of control depending of extension of controls above him
                Single pushDownHeight = 0;
                foreach (Element e in extendedYPos)
                    if (controls[i].Location.Y >= e.originalBottom) //completely under it
                    {
                        if (e.totalPushDown > pushDownHeight)
                            pushDownHeight = e.totalPushDown;
                    }
                Single cp = controls[i].Location.Y + pushDownHeight;

                Single extendedHeight;
                PrintControl(controls[i], mp, x + controls[i].Location.X, y + cp, out extendedHeight);
                if (extendedHeight > 0)
                {
                    //Keep extention with y position
                    Element e = new Element();
                    e.originalBottom = controls[i].Location.Y + controls[i].Height;
                    e.printedBottom = cp + controls[i].Height + extendedHeight;
                    extendedYPos.Add(e);
                }
            }
            // same computation for bottom of container control. Its bottom line is
            // below all child controls. So it is extended the same as the most pushed
            // child control.
            globalExtendedHeight = 0;
            foreach (Element e in extendedYPos)
                if (e.totalPushDown > globalExtendedHeight)
                    globalExtendedHeight = e.totalPushDown;

        }
        private class Element
        {
            public Single originalBottom;
            public Single printedBottom;
            public Single totalPushDown
            { get { return printedBottom - originalBottom; } }
        }

        /// <summary>
        /// This sub simply use recursivity to print child controls and print
        /// the parent control (height may grow depending of child controls).
        /// Extension is push up to the previous parent.
        /// </summary>
        /// <param name="c"></param>
        /// <param name="mp"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="extendedHeight"></param>
        public void PrintControl(System.Windows.Forms.Control c,
            MultiPageManagement mp,
            Single x, Single y,
            out Single extendedHeight)
        {
            extendedHeight = 0;
            if (c.Visible == true)
            {
                bool scanForChildControls;
                try
                {
                    // Print my header
                    PrintOneControl(c, ParentControlPrinting.BeforeChilds, mp, x, y, ref extendedHeight, out scanForChildControls);

                    // Print Contained controls
                    if (scanForChildControls)
                    {
                        y += extendedHeight;

                        _indent += 1;
                        Single ChildControlsExtendedHeight;
                        PrintControls(c, mp, x, y, out ChildControlsExtendedHeight);
                        _indent -= 1;

                        // Print my bottom (habitually a frame arround child controls)
                        PrintOneControl(c, ParentControlPrinting.AfterChilds, mp, x, y, ref ChildControlsExtendedHeight, out scanForChildControls);
                        extendedHeight += ChildControlsExtendedHeight;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error printing control of type " +
                        c.GetType().ToString() +
                        System.Environment.NewLine +
                        System.Environment.NewLine + ex.ToString());
                }
            }
        }

        /// <summary>
        /// Print a control (not is children). Call the print function depending of the type of the control
        /// </summary>
        /// <param name="c"></param>
        /// <param name="mp"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="extendedHeight"></param>
        public void PrintOneControl(System.Windows.Forms.Control c,
            ParentControlPrinting typePrint,
            MultiPageManagement mp,
            Single x, Single y,
            ref Single extendedHeight, out bool ScanForChildControls)
        {
            // Silver color if disable
            if (c.Enabled || !DisabledControlsInGray)
            {
                _Pen = new Pen(Color.Black);
                _Brush = Brushes.Black;
            }
            else
            {
                _Pen = new Pen(Color.Silver);
                _Brush = Brushes.Silver;
            }

            ScanForChildControls = true;
            string s = c.GetType().ToString();
            bool founded = false;
            //First check if it's a text box like control
            if (!founded)
                foreach (string sType in _TextBoxLikeControl)
                    if (s.IndexOf(sType) >= 0)
                    {
                        Single h;
                        h = mp.FontHeight(new Font(c.Font.Name, c.Font.Size));
                        extendedHeight = mp.BeginPrintUnit(y, h);
                        PrintText(c, mp, x, y, TextBoxBoxed, false, TextBoxBoxed, HorizontalAlignment.Left);
                        mp.EndPrintUnit();
                        founded = true;
                        ScanForChildControls = false;
                        break;
                    }

            //Process other type of control, beginning at the end of the list (use last add for a type)
            if (!founded)
                for (int i = _DelegatesforControls.Count - 1; i >= 0; i--)  // from end to the beginning
                {
                    _DelegateforControls d = (_DelegateforControls)_DelegatesforControls[i];
                    if (s.EndsWith(d.typ))
                    {
                        d.PrintFunction(c, typePrint, mp, x, y, ref extendedHeight, out ScanForChildControls);
                        break;
                    }
                }
            _traceLog.Append(_indent.ToString() + " - " + typePrint.ToString() + "  " + s + " (" + c.Text + ":" + c.Name + ")  X=" + x.ToString() + "   Y=" + y.ToString() + "   H=" + c.Height.ToString() + "   + " + extendedHeight.ToString() + System.Environment.NewLine);
        }
        #endregion

        #region "Text printing"
        /// <summary>
        /// Print a single line text for many controls. Do some formatting
        /// </summary>
        /// <param name="c">Control to print (must have these properties :font, text, width and height)</param>
        /// <param name="mp">Graphics object (printed page)</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="tbBoxed">Draw a box arround text</param>
        /// <param name="inBold">use bold font</param>
        /// <param name="verticalCentering">Adjust vertical to obtain precise alignment from different type of control</param>
        /// <param name="hAlignment">Horizontal alignment of text in control print area</param>
        public void PrintText(System.Windows.Forms.Control c,
                            MultiPageManagement mp,
                            Single x, Single y,
                            bool tbBoxed, bool inBold, bool verticalCentering,
                            HorizontalAlignment hAlignment)
        {
            Single yAdjusted = y;
            //Box
            if (tbBoxed)
                mp.DrawRectangle(_Pen, x, y, c.Width, c.Height);

            //Text
            Font printFont;
            if (inBold)
                printFont = new Font(c.Font.Name, c.Font.Size, FontStyle.Bold);
            else
                printFont = new Font(c.Font.Name, c.Font.Size);

            if (verticalCentering)
            {
                Single fontHeight = printFont.GetHeight(mp.Graphics());
                Single deltaHeight = (c.Height - fontHeight) / 2;
                yAdjusted += deltaHeight;
            }
            else
                yAdjusted += 2;

            mp.DrawString(c.Text, printFont, _Brush, x, yAdjusted, c.Width, c.Height, BuildFormat(hAlignment));
        }

        public StringFormat BuildFormat(HorizontalAlignment hAlignment)
        {
            StringFormat drawFormat = new StringFormat();
            switch (hAlignment)
            {
                case HorizontalAlignment.Left:
                    drawFormat.Alignment = StringAlignment.Near;
                    break;
                case HorizontalAlignment.Center:
                    drawFormat.Alignment = StringAlignment.Center;
                    break;
                case HorizontalAlignment.Right:
                    drawFormat.Alignment = StringAlignment.Far;
                    break;
            }
            return drawFormat;
        }

        public string TrimBlankLines(string s)
        {
            if (s == null)
                return s;
            else
            {
                for (int i = s.Length; i == 1; i--)
                    if ((s[i].ToString() != Keys.Enter.ToString()) && (s[i].ToString() != Keys.LineFeed.ToString()) && (s[i].ToString() != " "))
                        return s.Substring(0, i);
                return s;
            }
        }
        #endregion

        #region "Controls printing"
        /// <summary>
        /// Print single line or multi lines TextBox.
        /// </summary>
        public void PrintTextBox(System.Windows.Forms.Control c,
            ParentControlPrinting typePrint,
            MultiPageManagement mp,
            Single x, Single y,
            ref Single extendedHeight, out bool ScanForChildControls)
        {
            ScanForChildControls = false;

            TextBox tb = (System.Windows.Forms.TextBox)c;
            Single h = mp.FontHeight(new Font(tb.Font.Name, tb.Font.Size));
            if (!tb.Multiline)
            {
                extendedHeight = mp.BeginPrintUnit(y, h);
                PrintText(c, mp, x, y, TextBoxBoxed, tb.Font.Bold, TextBoxBoxed, tb.TextAlign);
                mp.EndPrintUnit();
            }
            else  //multiline
            {
                // cut text into lines that fit in printed textBox width
                // Define an area of one line height and call MeasureString on it
                // This method return charactersFitted for the line
                ArrayList lines = new ArrayList();
                System.Drawing.Graphics g = mp.Graphics();
                SizeF areaForOneLineOfText = new SizeF();
                areaForOneLineOfText.Height = h;
                areaForOneLineOfText.Width = tb.Width;
                int charactersFitted;
                int linesFilled;
                int separatorCharPos;
                Font printFont = new Font(tb.Font.Name, tb.Font.Size);

                int pos = 0;
                do
                {
                    g.MeasureString(tb.Text.Substring(pos),
                                    printFont,
                                    areaForOneLineOfText,
                                    new StringFormat(),
                                    out charactersFitted,
                                    out	linesFilled);
                    // this method with one line cut the last word....
                    // So, I have to go bak in the string to find a separator
                    // Happyly, MeasureString count separator character in charactersFitted
                    // For example, return "this is the first line " with space at the end
                    // even if there is not room for last space
                    separatorCharPos = charactersFitted;
                    if (charactersFitted < tb.Text.Substring(pos).Length) //le restant n'entre pas au complet sur la ligne
                    {
                        do
                        { separatorCharPos--; }
                        while ((separatorCharPos > pos) && !System.Char.IsSeparator(tb.Text, pos + separatorCharPos));
                        if (separatorCharPos == pos)   // no separator
                            separatorCharPos = charactersFitted;
                        else
                            separatorCharPos++;
                    }
                    lines.Add(tb.Text.Substring(pos, separatorCharPos));
                    pos += separatorCharPos;
                }
                while ((pos < tb.Text.Length) && (charactersFitted > 0));

                // Print lines one by one
                Single yItem = y;
                Single extraHeight;
                Single extraHeightFirstLine = 0;	//Remenber if first line is pull down
                for (int i = 0; i < lines.Count; i++)
                {
                    extraHeight = mp.BeginPrintUnit(yItem, h);   //Space required for tab page caption
                    if (i == 0)
                        extraHeightFirstLine = extraHeight;
                    mp.DrawString((string)lines[i], printFont, _Brush, x, yItem, tb.Width, h);
                    mp.EndPrintUnit();
                    yItem += h + extraHeight;
                }

                if ((yItem - y) > tb.Height)
                    extendedHeight = (yItem - y) - tb.Height;

                if (TextBoxBoxed)
                {
                    _Pen = new Pen(Color.Gray);
                    //Draw a rectangle arround list box. Start downer if first line pulled down
                    mp.DrawFrame(_Pen, x, y + extraHeightFirstLine, tb.Width, tb.Height + extendedHeight - extraHeightFirstLine);
                }
            }
        }

        public void PrintLabel(System.Windows.Forms.Control c,
            ParentControlPrinting typePrint,
            MultiPageManagement mp,
            Single x, Single y,
            ref Single extendedHeight, out bool ScanForChildControls)
        {
            ScanForChildControls = false;
            // Convert ContentAlignment (property of labels) to HorizontalAlignment (Left, center, Right)
            HorizontalAlignment ha;
            string ss = c.Text;
            ContentAlignment ha2 = ((Label)c).TextAlign;
            switch (ha2)
            {
                case ContentAlignment.BottomLeft:
                    ha = HorizontalAlignment.Left;
                    break;
                case ContentAlignment.TopLeft:
                    ha = HorizontalAlignment.Left;
                    break;
                case ContentAlignment.MiddleLeft:
                    ha = HorizontalAlignment.Left;
                    break;

                case ContentAlignment.BottomCenter:
                    ha = HorizontalAlignment.Center;
                    break;
                case ContentAlignment.TopCenter:
                    ha = HorizontalAlignment.Center;
                    break;
                case ContentAlignment.MiddleCenter:
                    ha = HorizontalAlignment.Center;
                    break;

                case ContentAlignment.BottomRight:
                    ha = HorizontalAlignment.Right;
                    break;
                case ContentAlignment.TopRight:
                    ha = HorizontalAlignment.Right;
                    break;
                case ContentAlignment.MiddleRight:
                    ha = HorizontalAlignment.Right;
                    break;
                default:
                    ha = HorizontalAlignment.Left;
                    break;
            }
            Single h = mp.FontHeight(new Font(c.Font.Name, c.Font.Size));
            if (c.Height > h)
                h = c.Height;
            extendedHeight = mp.BeginPrintUnit(y, h);
            PrintText(c, mp, x, y, false, LabelInBold, false, ha);
            mp.EndPrintUnit();
        }

        public void PrintCheckBox(System.Windows.Forms.Control c,
            ParentControlPrinting typePrint,
            MultiPageManagement mp,
            Single x, Single y,
            ref Single extendedHeight, out bool ScanForChildControls)
        {
            ScanForChildControls = false;
            Single h = mp.FontHeight(new Font(c.Font.Name, c.Font.Size));
            extendedHeight = mp.BeginPrintUnit(y, h);

            mp.DrawRectangle(_Pen, x, y, h, h);
            if (((CheckBox)c).Checked)
            {
                Single d = 3;
                mp.DrawLines(_Pen, x + d, y + d, x + h - d, y + h - d);
                PointF[] points2 = new PointF[] { new PointF(x + h - d, y + d), new PointF(x + d, y + h - d) };
                mp.DrawLines(_Pen, x + h - d, y + d, x + d, y + h - d);
            }
            PrintText(c, mp, (float)(x + (h * 1.4)), (float)y - 2, false, false, false, HorizontalAlignment.Left);
            mp.EndPrintUnit();
        }

        public void PrintRadioButton(System.Windows.Forms.Control c,
            ParentControlPrinting typePrint,
            MultiPageManagement mp,
            Single x, Single y,
            ref Single extendedHeight, out bool ScanForChildControls)
        {
            ScanForChildControls = false;
            Single h = mp.FontHeight(new Font(c.Font.Name, c.Font.Size));
            extendedHeight = mp.BeginPrintUnit(y, h);
            mp.DrawEllipse(_Pen, x, y, h, h);
            if (((RadioButton)c).Checked)
            {
                Single d = 3;
                mp.FillEllipse(_Brush, x + d, y + d, h - d - d, h - d - d);
            }
            PrintText(c, mp, (float)(x + (h * 1.4)), y - 2, false, false, false, HorizontalAlignment.Left);
            mp.EndPrintUnit();
        }

        public void PrintPanel(System.Windows.Forms.Control c,
            ParentControlPrinting typePrint,
            MultiPageManagement mp,
            Single x, Single y,
            ref Single extendedHeight, out bool ScanForChildControls)
        {
            ScanForChildControls = true;
            if (typePrint == ParentControlPrinting.AfterChilds)
                if (((System.Windows.Forms.Panel)c).BorderStyle != BorderStyle.None)
                {
                    if (((System.Windows.Forms.Panel)c).BorderStyle == BorderStyle.Fixed3D)
                        _Pen = new Pen(Color.Silver);
                    // if height less than 10, just print an horizontal line
                    if ((c.Height < 10) && (c.Controls.Count == 0))
                    {
                        extendedHeight += mp.BeginPrintUnit(y, 1);
                        mp.DrawLines(_Pen, x, y, x + c.Width, y);
                        mp.EndPrintUnit();
                    }
                    else
                        mp.DrawFrame(_Pen, x, y, c.Width, c.Height + extendedHeight);
                }
        }

        public void PrintGroupBox(System.Windows.Forms.Control c,
            ParentControlPrinting typePrint,
            MultiPageManagement mp,
            Single x, Single y,
            ref Single extendedHeight, out bool ScanForChildControls)
        {
            ScanForChildControls = true;
            Font printFont = new Font(c.Font.Name, c.Font.Size);
            Single h = mp.FontHeight(printFont);
            _Pen = new Pen(Color.Silver);
            switch (typePrint)
            {
                case ParentControlPrinting.BeforeChilds:
                    // if height less than 10, just print an horizontal line
                    if ((c.Height < 10) && (c.Controls.Count == 0))
                    {
                        extendedHeight += mp.BeginPrintUnit(y, 1);
                        mp.DrawLines(_Pen, x, y, x + c.Width, y);
                        mp.EndPrintUnit();
                    }
                    else
                    {
                        Single extraHeight = mp.BeginPrintUnit(y, h);   //Space required for group caption
                        //PrintText(c, mp, x + h, y, false, true, false, HorizontalAlignment.Left);
                        mp.DrawString(c.Text, printFont, Brushes.Black, x + h, y, c.Width - h - h, h);
                        mp.EndPrintUnit();
                        extendedHeight += extraHeight;
                    };
                    break;
                case ParentControlPrinting.AfterChilds:
                    mp.DrawFrame(_Pen, x, y, c.Width, c.Height + extendedHeight);
                    break;
            }
        }

        public void PrintTabControl(System.Windows.Forms.Control c,
            ParentControlPrinting typePrint,
            MultiPageManagement mp,
            Single x, Single y,
            ref Single extendedHeight, out bool ScanForChildControls)
        {
            ScanForChildControls = true;
            System.Windows.Forms.TabControl tc = (TabControl)c;
            _Pen = new Pen(Color.Gray);
            switch (typePrint)
            {
                case ParentControlPrinting.BeforeChilds:
                    //Nom du TabPage
                    Single extraHeight = mp.BeginPrintUnit(y, tc.ItemSize.Height);   //Space required for tab page caption
                    System.Windows.Forms.TabPage tp = tc.SelectedTab;
                    Font printFont = new Font(tp.Font.Name, tp.Font.Size, FontStyle.Bold);
                    Single h = mp.FontHeight(printFont);
                    if (h > tc.ItemSize.Height)
                        h = tc.ItemSize.Height;
                    mp.DrawString(tp.Text, printFont, Brushes.Black, x, y + (tc.ItemSize.Height - h) / 2, tp.Width, h);
                    mp.DrawLines(_Pen, x, y + tc.ItemSize.Height, x + tc.Width, y + tc.ItemSize.Height);
                    mp.EndPrintUnit();
                    extendedHeight += extraHeight;
                    break;
                case ParentControlPrinting.AfterChilds:
                    if (TabControlBoxed)
                        mp.DrawFrame(_Pen, x, y, c.Width, c.Height + extendedHeight);
                    break;
            }
        }

        public void PrintPictureBox(System.Windows.Forms.Control c,
            ParentControlPrinting typePrint,
            MultiPageManagement mp,
            Single x, Single y,
            ref Single extendedHeight, out bool ScanForChildControls)
        {
            ScanForChildControls = false;
            extendedHeight = mp.BeginPrintUnit(y, c.Height);
            PictureBox pic = (PictureBox)c;
            mp.DrawImage(pic.Image, x, y, c.Width, c.Height);
            mp.EndPrintUnit();
        }

        public void PrintListBox(System.Windows.Forms.Control c,
            ParentControlPrinting typePrint,
            MultiPageManagement mp,
            Single x, Single y,
            ref Single extendedHeight, out bool ScanForChildControls)
        {
            ScanForChildControls = false;
            ListBox lb = (ListBox)c;
            Single yItem = y;
            Single extraHeight;
            Single extraHeightFirstLine = 0;	//Remenber if first line is pull down
            Font printFont = new Font(lb.Font.Name, lb.Font.Size, FontStyle.Bold);

            int oldPos = lb.SelectedIndex; //save position
            for (int i = 0; i < lb.Items.Count; i++)
            {
                extraHeight = mp.BeginPrintUnit(yItem, lb.ItemHeight);   //Space required for tab page caption
                if (i == 0)
                    extraHeightFirstLine = extraHeight;
                lb.SelectedIndex = i; // set position to obtain Text of current item
                mp.DrawString(lb.Text, printFont, _Brush, x, yItem, lb.Width, lb.ItemHeight);
                mp.EndPrintUnit();
                yItem += lb.ItemHeight + extraHeight;
            }
            lb.SelectedIndex = oldPos; //restore position

            if ((yItem - y) > lb.Height)
                extendedHeight = (yItem - y) - lb.Height;

            _Pen = new Pen(Color.Gray);
            //Draw a rectangle arround list box. Start downer if first line pulled down
            mp.DrawFrame(_Pen, x, y + extraHeightFirstLine, c.Width, c.Height + extendedHeight - extraHeightFirstLine);
        }

        public void PrintDataGrid(System.Windows.Forms.Control c,
            ParentControlPrinting typePrint,
            MultiPageManagement mp,
            Single x, Single y,
            ref Single extendedHeight, out bool ScanForChildControls)
        {
            ScanForChildControls = false;

            DataGrid dg = (DataGrid)c;
            Single extraHeight = 0;
            Single extraHeightHeaderLine = 0;
            Font printFont = new Font(dg.Font.Name, dg.Font.Size, FontStyle.Bold);
            Single h = mp.FontHeight(printFont);

            // Column header
            DataGridTableStyle myGridTableStyle;
            if (dg.TableStyles.Count == 0)
            {
                myGridTableStyle = new DataGridTableStyle();
                dg.TableStyles.Add(myGridTableStyle);
            }

            // Header of each column
            Single xPos = x;
            Single yPos = y;
            Single w;
            extraHeightHeaderLine = mp.BeginPrintUnit(yPos, h + 1);  //Space required for header text
            for (int i = 0; i < dg.TableStyles[0].GridColumnStyles.Count; i++)
            {
                string caption = dg.TableStyles[0].GridColumnStyles[i].HeaderText;
                w = dg.TableStyles[0].GridColumnStyles[i].Width;
                if (xPos + w > x + dg.Width)
                    w = x + dg.Width - xPos;
                if (xPos < x + dg.Width)
                    mp.DrawString(caption, printFont, _Brush, xPos, yPos, w, h);
                if (i == 0)  // Draw horizontal line below header
                    mp.DrawLines(_Pen, x, yPos + h, x + dg.Width, yPos + h);
                xPos += w;
            }
            mp.EndPrintUnit();
            yPos += h + 1 + extraHeightHeaderLine;

            // Get dataTable displayed in DataGrid
            // This function only support DataGrid with DataTable as DataSource
            // This is the only case I code to obtain data in DataGrid
            DataTable dt = null;
            if (dg.DataSource is System.Data.DataTable)
                dt = (DataTable)dg.DataSource;
            else
                if ((dg.DataSource is System.Data.DataSet) && (dg.DataMember != null))
                {
                    DataSet ds = (DataSet)dg.DataSource;
                    if (ds.Tables.Contains(dg.DataMember))
                        dt = ds.Tables[dg.DataMember];
                }

            // Loop on DataRow, with embed loop on columns
            if (dt != null)
                foreach (DataRow dr in dt.Rows)
                {
                    extraHeight = mp.BeginPrintUnit(yPos, h);  //Space required for header text
                    xPos = x;
                    for (int i = 0; i < dg.TableStyles[0].GridColumnStyles.Count; i++)
                    {
                        string caption = dr[i].ToString();
                        w = dg.TableStyles[0].GridColumnStyles[i].Width;
                        if (xPos + w > x + dg.Width)
                            w = x + dg.Width - xPos;
                        if (xPos < x + dg.Width)
                            mp.DrawString(caption, printFont, _Brush, xPos, yPos, w, h);
                        xPos += w;
                    };
                    mp.EndPrintUnit();
                    yPos += h + extraHeight;
                }

            // Draw horizontal line at the bottom of DataGrid
            if (yPos < y + dg.Height + extraHeightHeaderLine)
                yPos = y + dg.Height + extraHeightHeaderLine;
            mp.BeginPrintUnit(yPos, 1);
            mp.DrawLines(_Pen, x, yPos, x + dg.Width, yPos);
            mp.EndPrintUnit();

            // Finally, Compute extendedHeight
            if ((yPos - y) > dg.Height)
                extendedHeight = (yPos - y) - dg.Height;
        }

        //		public void PrintFlexGrid(System.Windows.Forms.Control c,
        //			ParentControlPrinting typePrint, 
        //			MultiPageManagement mp, 
        //			Single x, Single y,
        //			ref Single extendedHeight, out bool ScanForChildControls)
        //		{
        //			ScanForChildControls = false;
        //			C1.Win.C1FlexGrid.C1FlexGrid g = (C1.Win.C1FlexGrid.C1FlexGrid) c;
        //
        //			// print rows and column header (row[0])
        //			RectangleF r = new RectangleF();
        //			PointF[] points;
        //			r.Y = y; //+ r.Height + _Pen.Width ;
        //			r.Height = g.Font.GetHeight(ev.Graphics);
        //
        //			for (int i = 0; i < g.Rows.Count; i++) // each row
        //			{
        //				r.X = x;
        //				for (int j = 0; j<g.Cols.Count; j++) // each column
        //					if ((g.Cols[j].Visible) && (r.X < x + g.Width)) //if not out of grid
        //					{
        //						r.Width = g.Cols[j].Width;
        //						if ((r.X + r.Width) > (x + g.Width)) //overtaking
        //							r.Width = (x + g.Width) - r.X;
        //						ev.Graphics.DrawString(g.GetDataDisplay(i,j), g.Font, _Brush, r, new StringFormat());
        //						r.X += r.Width;
        //					}
        //				r.Y += r.Height;
        //				if (i == 0) //Columns header, Add an horizontal line below
        //				{
        //					r.Y += 1;
        //					points = new PointF[] {new PointF(x, r.Y), new PointF(x + g.Width, r.Y)};
        //					ev.Graphics.DrawLines(_Pen, points);
        //					r.Y += _Pen.Width ;
        //				}
        //			}
        //
        //			// Add a bottom line
        //			points = new PointF[] {new PointF(x, r.Y), new PointF(x + g.Width, r.Y)};
        //			ev.Graphics.DrawLines(_Pen, points);
        //			r.Y += _Pen.Width ;
        //
        //			// Compute extendedHeight
        //			if (r.Y > y + g.Height)
        //				extendedHeight = r.Y - (y + g.Height);
        //		}

        //		StringFormat _StringFormatFromFlexGridAlign(C1.Win.C1FlexGrid.TextAlignEnum fg)
        //		{
        //			StringFormat sf = new StringFormat();
        //			string s = fg.ToString().ToUpper();
        //			if (s.IndexOf("LEFT") == 0)
        //				sf.Alignment = System.Drawing.StringAlignment.Near; 
        //			if (s.IndexOf("RIGHT") == 0)
        //				sf.Alignment = System.Drawing.StringAlignment.Far;
        //			if (s.IndexOf("CENTER") == 0)
        //				sf.Alignment = System.Drawing.StringAlignment.Center; 
        //			return sf;
        //		}


        #endregion

        #region "Multi page class"
        public class MultiPageManagement
        {
            private bool _PageOverflow;
            private Single _realPageTop, _realPageHeight, _UsablePageHeight;
            private Single _realPageLeft, _realPageRight;
            private Single _CurrentPageTop, _CurrentPageBottom;
            private int _PageNumber = 0;
            //private PrintPageEventArgs _Ev;
            private Graphics _G;
            private bool _PrintUnit;
            private bool _PrintInCurrentPage;
            private Single _PrintUnitPullDown;
            private bool _pageNumbering;
            private Font _FontForPageNumering;
            private string _PageNumberingFormat;

            public System.Drawing.Graphics Graphics()
            {
                return _G;
            }

            // Constructor
            public MultiPageManagement(Single pageTop, Single pageBottom, Single pageLeft, Single pageRight, Font formFont, bool pageNumbering, string pageNumberingFormat)
            {
                _realPageTop = pageTop;
                _realPageHeight = pageBottom - pageTop;
                _realPageLeft = pageLeft;
                _realPageRight = pageRight;
                _pageNumbering = pageNumbering;
                if (_pageNumbering)
                {
                    _PageNumberingFormat = pageNumberingFormat;
                    _FontForPageNumering = new Font(formFont.Name, (Single)(formFont.Size * 0.8));
                }
            }

            /// <summary>
            /// Check if items printed below current page
            /// </summary>
            /// <returns>Return true if there is need for another page</returns>
            public bool LastPage()
            {
                return (!_PageOverflow);
            }

            /// <summary>
            /// Change page. Reset page properties
            /// </summary>
            /// <param name="g">Graphics object</param>
            public void NewPage(Graphics g)
            {
                _G = g;
                _PageNumber += 1;
                _UsablePageHeight = _realPageHeight;
                if (_pageNumbering)
                {
                    Single fontHeightForPageNumbering = FontHeight(_FontForPageNumering);
                    _UsablePageHeight -= (Single)(fontHeightForPageNumbering * 1.5);
                    // Compute rectangular space for page number
                    RectangleF _recForPageNumbering = new RectangleF();
                    _recForPageNumbering.X = _realPageLeft;
                    _recForPageNumbering.Y = _realPageTop + _realPageHeight - fontHeightForPageNumbering;
                    _recForPageNumbering.Width = _realPageRight - _realPageLeft;
                    _recForPageNumbering.Height = fontHeightForPageNumbering;
                    // impression
                    StringFormat drawFormat = new StringFormat();
                    drawFormat.Alignment = StringAlignment.Far;
                    _G.DrawString(String.Format(_PageNumberingFormat, _PageNumber), _FontForPageNumering, Brushes.Black, _recForPageNumbering, drawFormat);
                }
                _CurrentPageTop = _UsablePageHeight * (_PageNumber - 1);
                _CurrentPageBottom = _CurrentPageTop + _UsablePageHeight;
                _PageOverflow = false;
            }

            public void ResetPage()
            {
                _PageNumber = 0;
            }

            public Single BeginPrintUnit(Single y, Single neededHeight)
            {
                if (neededHeight > _UsablePageHeight)
                    throw new Exception("Needed height cannot exceed 1 page. Page height = " + _UsablePageHeight);
                _PrintUnit = true;


                // Verify if unit goes accross a page break
                // if it's the case, calculate vertical push down height to place the
                // top of unit just below page break
                Single pageBreakPos;
                Single printingPos = y;
                for (pageBreakPos = _UsablePageHeight; pageBreakPos < (y + neededHeight); pageBreakPos += _UsablePageHeight)
                    if ((y <= pageBreakPos) && ((y + neededHeight - 1) > pageBreakPos)) //Accross
                        printingPos = pageBreakPos;

                _PrintUnitPullDown = printingPos - y;

                // test if unit is in current page else if another page is needed
                _PrintInCurrentPage = false;
                if (printingPos + neededHeight - 1 > _CurrentPageBottom)
                    _PageOverflow = true;
                else
                    if (printingPos >= _CurrentPageTop)
                        _PrintInCurrentPage = true;

                return _PrintUnitPullDown;
            }

            private Single _ConvertToPage(Single y)
            {
                Single newY = y - _CurrentPageTop + _realPageTop;
                if (_PrintUnit)
                    return newY += _PrintUnitPullDown;
                return newY;
            }

            public void EndPrintUnit()
            {
                _PrintUnit = false;
            }

            private bool PrintUnitIsInCurrentPage()
            {
                if (!_PrintUnit)
                    throw new Exception("Must be in a print unit to print");
                return _PrintInCurrentPage;
            }

            public Single FontHeight(Font font)
            {
                return font.GetHeight(_G);
            }

            public void DrawLines(Pen pen, Single x1, Single y1, Single x2, Single y2)
            {
                if (PrintUnitIsInCurrentPage())
                {
                    Single y1page = _ConvertToPage(y1);
                    Single y2page = _ConvertToPage(y2);
                    PointF[] points = new PointF[2];
                    points[0].X = x1;
                    points[0].Y = y1page;
                    points[1].X = x2;
                    points[1].Y = y2page;
                    _G.DrawLines(pen, points);
                }
            }

            public void DrawString(string s, Font printFont, Brush brush, Single x, Single y, Single w, Single h)
            {
                DrawString(s, printFont, brush, x, y, w, h, new StringFormat());
            }

            public void DrawString(string s, Font printFont, Brush brush, Single x, Single y, Single w, Single h, StringFormat sf)
            {
                if (PrintUnitIsInCurrentPage())
                {
                    Single yPage = _ConvertToPage(y);
                    RectangleF r = new RectangleF();
                    r.X = x;
                    r.Y = yPage;
                    r.Width = w;
                    r.Height = h;
                    _G.DrawString(s, printFont, brush, r, sf);
                }
            }

            public void DrawRectangle(Pen pen, Single x, Single y, Single w, Single h)
            {
                if (PrintUnitIsInCurrentPage())
                {
                    Single yPage = _ConvertToPage(y);
                    _G.DrawRectangle(pen, x, yPage, w, h);
                }
            }

            public void DrawEllipse(Pen pen, Single x, Single y, Single w, Single h)
            {
                if (PrintUnitIsInCurrentPage())
                {
                    Single yPage = _ConvertToPage(y);
                    _G.DrawEllipse(pen, x, yPage, w, h);
                }
            }

            public void FillEllipse(Brush brush, Single x, Single y, Single w, Single h)
            {
                if (PrintUnitIsInCurrentPage())
                {
                    Single yPage = _ConvertToPage(y);
                    _G.FillEllipse(brush, x, yPage, w, h);
                }
            }

            public void DrawImage(Image image, Single x, Single y, Single w, Single h)
            {
                if (PrintUnitIsInCurrentPage())
                {
                    Single yPage = _ConvertToPage(y);
                    _G.DrawImage(image, x, yPage, w, h);
                }
            }

            public void DrawFrame(Pen pen, Single x, Single y, Single w, Single h)
            {
                PointF[] points = new PointF[2];
                Single yTop = _CurrentPageTop;
                Single yBottom = _CurrentPageBottom;

                if (y + h <= _CurrentPageTop)	//Bottom of Frame above current page
                    return;
                if (y >= _CurrentPageBottom)	//Top of Frame below current page
                {
                    _PageOverflow = true;
                    return;
                }

                // Assign X coordinate for horizontal lines
                points[0].X = x;
                points[1].X = x + w;

                // Draw top line
                if (y >= _CurrentPageTop)	//Top in current page
                {
                    yTop = y;
                    points[0].Y = _ConvertToPage(yTop);
                    points[1].Y = _ConvertToPage(yTop);
                    _G.DrawLines(pen, points);
                }

                // Draw bottom line
                if (y + h <= _CurrentPageBottom) //Bottom in current page
                {
                    yBottom = y + h;
                    points[0].Y = _ConvertToPage(yBottom);
                    points[1].Y = _ConvertToPage(yBottom);
                    _G.DrawLines(pen, points);
                }
                else
                    _PageOverflow = true;

                // Assign Y coordinate for vertical lines
                points[0].Y = _ConvertToPage(yTop);
                points[1].Y = _ConvertToPage(yBottom);

                // Draw left line
                points[0].X = x;
                points[1].X = x;
                _G.DrawLines(pen, points);

                // Draw right line
                points[0].X = x + w;
                points[1].X = x + w;
                _G.DrawLines(pen, points);
            }
        }
        #endregion
    }
}