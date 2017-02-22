using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace GSM.UC
{
    public partial class NodeChild : UserControl
    {
        private Point mouseOffset;

        private bool isMouseDown = false; 


        public NodeChild()
        {
            InitializeComponent();
        }

        private string _id;
        public string _ID
        {
            get { return _id; }
            set { this._id = value; }
        }

        private int _area;
        public int _Area
        {
            get { return _area; }
            set { this._area = value; }
        }
 
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            base.OnMouseDown(e);
        }

        private void lblNodeChild_MouseDown(object sender, MouseEventArgs e)
        {
            base.OnMouseDown(e);
        }
    }
}
