using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GSM.Cls
{
    class BufferLv:ListView
    {
        public BufferLv()
        {
            SetStyle(ControlStyles.DoubleBuffer |
                                     ControlStyles.OptimizedDoubleBuffer |
                                     ControlStyles.AllPaintingInWmPaint,
                                     true);
            UpdateStyles();   
        }
    }
}
