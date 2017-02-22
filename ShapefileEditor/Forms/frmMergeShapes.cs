using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ShapefileEditor
{
    public partial class frmMergeShapes : Form
    {
        public ShapefileEditorClass pluginClass;

        public frmMergeShapes()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            pluginClass.MergeShapes();
        }
    }
}