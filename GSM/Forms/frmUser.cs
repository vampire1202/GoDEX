using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GSM.Forms
{
    public partial class frmUser : Form
    {
        frmMain _fmain = new frmMain();
        public frmUser(frmMain fMain)
        {
            InitializeComponent();
            _fmain = fMain;
        }

        private void gbtnOk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtUserPwd.Text))
            {
                MessageBox.Show("请输入密码", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            switch (this.cmbUserRole.SelectedIndex)
            {
                case 0:
                    if (Cls.DESS.Decode(Cls.RWconfig.GetAppSettings("SystemManager")) == this.txtUserPwd.Text)
                    {
                        _fmain.tsslblUserRole.Text = "SystemManager";
                        Cls.RWconfig.SetAppSettings("CurrentUser", "SystemManager");
                        this.Dispose();
                    }
                    else
                    {
                        MessageBox.Show("密码不正确");
                    }
                    break;
                case 1:
                    if (Cls.DESS.Decode(Cls.RWconfig.GetAppSettings("Users")) == this.txtUserPwd.Text)
                    {
                        _fmain.tsslblUserRole.Text = "Users";
                        Cls.RWconfig.SetAppSettings("CurrentUser", "Users");
                        this.Dispose();
                    }
                    else
                    {
                        MessageBox.Show("密码不正确");
                    }
                    break;
                case 2:
                    if (Cls.DESS.Decode(Cls.RWconfig.GetAppSettings("Guests")) == this.txtUserPwd.Text)
                    {
                        _fmain.tsslblUserRole.Text = "Guests";
                        Cls.RWconfig.SetAppSettings("CurrentUser", "Guests");
                        _fmain.tsmiFile.Enabled = false;
                        _fmain.tsmiItem.Enabled = false;
                        _fmain.tsmiLook.Enabled = false;
                        _fmain.toolStripMain.Enabled = false;
                        _fmain.contextMenuStrip1.Enabled = false;
                        this.Dispose();
                    }
                    else
                    {
                        MessageBox.Show("密码不正确");
                    }
                    break;
            }            

        }

        private void frmUser_Load(object sender, EventArgs e)
        {
            
            this.cmbUserRole.Items.Clear();
            this.cmbUserRole.Items.Add("系统管理员");
            this.cmbUserRole.Items.Add("普通用户");
            this.cmbUserRole.Items.Add("观察员");
            this.cmbUserRole.SelectedIndex = 0;
            this.txtUserPwd.Focus();
        }

        private void gbtnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
            _fmain.frmMain_FormClosing(sender, new FormClosingEventArgs(CloseReason.UserClosing, false));
        }

        private void txtUserPwd_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                gbtnOk_Click(sender, new EventArgs());

            if (e.KeyChar == (char)Keys.Escape)
                gbtnCancel_Click(sender, new EventArgs());
        }
    }
}
