using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;

namespace GSM.Forms
{
    public partial class frmSetUserPwd : Form
    {
        public frmSetUserPwd()
        {
            InitializeComponent();
            this.cmbUserRole.Items.Clear();
            //this.cmbUserRole.Items.Add("生产商");
            this.cmbUserRole.Items.Add("代理商");
            this.cmbUserRole.Items.Add("管理员");
            this.cmbUserRole.Items.Add("操作员");
            this.cmbUserRole.SelectedIndex = 0;
        }

        private string _userrole;
        public string _UserRole
        {
            get { return _userrole; }
            set { _userrole = value; }
        }

        private string _appUserXmlPath = AppDomain.CurrentDomain.BaseDirectory + "godexApp.userxml";

        private void frmSetUserPwd_Load(object sender, EventArgs e)
        {
            CreateUserList();
        }
        /// <summary>
        /// Listview初始化
        /// </summary>
        private void CreateUserList()
        {
            Cls.XmlHelper.DecryptXml(_appUserXmlPath);
            listView1.Columns.Clear();
            listView1.Items.Clear();
            // Set the view to show details.
            listView1.View = View.Details;
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            // Allow the user to edit item text.
            listView1.LabelEdit = false;
            // Allow the user to rearrange columns.
            listView1.AllowColumnReorder = true;
            // Display check boxes.
            listView1.CheckBoxes = true;
            // Select the item and subitems when selection is made.
            listView1.FullRowSelect = true;


            // Display grid lines.
            listView1.GridLines = true;
            listView1.MultiSelect = false;

            // Sort the items in the list in ascending order.
            listView1.Sorting = SortOrder.None;
            listView1.Columns.Add("用户代码", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("姓名", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("联系方式", 150, HorizontalAlignment.Left);
            listView1.Columns.Add("身份类型", -2, HorizontalAlignment.Left);

            XmlDocument xd = new XmlDocument();
            xd.Load(this._appUserXmlPath);
            XmlNode xn = xd.SelectSingleNode("/root");
            foreach (XmlNode xnc in xn.ChildNodes)
            {
                ListViewItem item1 = new ListViewItem(xnc.Attributes["code"].Value, 0);
                item1.SubItems.Add(xnc.Attributes["name"].Value);
                item1.SubItems.Add(xnc.Attributes["telephone"].Value);
                switch (xnc.Attributes["role"].Value)
                {
                    //case "Administrator":
                    //    item1.SubItems.Add("生产商");
                    //    item1.ForeColor = Color.DarkRed; 
                    //    break;
                    case "Agency":
                        item1.SubItems.Add("代理商");
                        item1.ForeColor = Color.Red; 
                        break;
                    case "Manager":
                        item1.SubItems.Add("管理员");
                        item1.ForeColor = Color.Blue; 
                        break;
                    case "Users":
                        item1.SubItems.Add("操作员");
                        item1.ForeColor = Color.Black; 
                        break;
                }
                this.listView1.Items.AddRange(new ListViewItem[] { item1 });
            }

            //ImageList imageListSmall = new ImageList();
            //ImageList imageListLarge = new ImageList();
            //imageListLarge.Images.Add(global::GSM.Properties.Resources.systemManager);
            //imageListLarge.Images.Add(global::GSM.Properties.Resources.commonUser);

            //listView1.LargeImageList = imageListLarge;
            //listView1.SmallImageList = imageListSmall;
            Cls.XmlHelper.EncryptXml(_appUserXmlPath, "root");
        }

        private void gbtnOk_Click(object sender, EventArgs e)
        {
            string msg = string.Empty;
            if (string.IsNullOrEmpty(this.txtUserCode.Text))
                msg += "用户代码不能为空!\r\n";
            if (string.IsNullOrEmpty(this.txtPassword.Text))
                msg += "用户密码不能为空!\r\n";
            if (this.txtPassword.Text != this.txtConfirmPwd.Text)
                msg += "密码不一致,请重新输入!\r\n";
            if (userIsExit(this.txtUserCode.Text))
            {
                msg += "用户已存在!\r\n";
            }
            if (!string.IsNullOrEmpty(msg))
            {
                MessageBox.Show(msg);
                return;
            }
            //添加用户
            addUser(this.txtUserCode.Text);
        }

        private void addUser(string userCode)
        {
            //try
            //{
            //    Cls.XmlHelper.DecryptXml(_appUserXmlPath);
            //    XmlDocument xd = new XmlDocument();
            //    xd.Load(_appUserXmlPath);
            //    XmlNode xnRoot = xd.SelectSingleNode("root");
            //    //插入用户
            //    XmlElement xe = xd.CreateElement("user");
            //    xe.SetAttribute("code", this.txtUserCode.Text);
            //    xe.SetAttribute("name", this.txtUserName.Text);
            //    xe.SetAttribute("telephone", this.txtUserTel.Text);
            //    xe.SetAttribute("password", Cls.DESS.Encode(this.txtPassword.Text));
            //    switch (this.cmbUserRole.SelectedIndex)
            //    {
            //        case 0:
            //            xe.SetAttribute("role", "SystemManager");
            //            break;
            //        case 1:
            //            xe.SetAttribute("role", "CommonUser");
            //            break;
            //        case 2:
            //            xe.SetAttribute("role", "Observer");
            //            break;
            //        case 3:
            //            xe.SetAttribute("role", "Observer");
            //            break;
            //    }
            //    xnRoot.AppendChild(xe);
            //    xd.Save(_appUserXmlPath);
            //    CreateUserList();
            //}
            //catch (Exception ee) { MessageBox.Show(ee.ToString()); }
        }

        //检测用户是否存在
        private Boolean userIsExit(string userCode)
        {
            Boolean userIsExit = false;
            try
            {
                Cls.XmlHelper.DecryptXml(_appUserXmlPath);
                XmlDocument xd = new XmlDocument();
                xd.Load(_appUserXmlPath);
                XmlNode xnRoot = xd.SelectSingleNode("/root");
                foreach (XmlNode xn in xnRoot.ChildNodes)
                {
                    if (xn.Attributes["code"].Value == userCode)
                    {
                        userIsExit = true;
                        break;
                    }
                }

            }
            catch { }
            return userIsExit;
        }

        private void gbtnDel_Click(object sender, EventArgs e)
        {
            try
            {
                Cls.XmlHelper.DecryptXml(_appUserXmlPath);
                XmlDocument xd = new XmlDocument();
                xd.Load(this._appUserXmlPath);
                if (DialogResult.OK == MessageBox.Show("确认删除所选用户?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
                {
                    foreach (ListViewItem lvi in this.listView1.CheckedItems)
                    {
                        XmlNode xn = xd.SelectSingleNode("/root/user[@code='" + lvi.SubItems[0].Text + "']");
                        if (lvi.SubItems[0].Text != "admin")
                        {
                            xn.ParentNode.RemoveChild(xn);
                        }
                        else
                        {
                            MessageBox.Show("不允许删除管理员");
                        }
                    }
                }
                xd.Save(this._appUserXmlPath);
                CreateUserList();
                this.cmbUserRole.Enabled = true;
                this.txtUserCode.Enabled = true;
                this.txtUserCode.Text = this.txtUserName.Text = this.txtUserTel.Text = this.txtPassword.Text = this.txtConfirmPwd.Text = string.Empty;
            }
            catch { }

        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                //MessageBox.Show(e.Item.SubItems[0].Text);
                Cls.XmlHelper.DecryptXml(_appUserXmlPath);
                XmlDocument xd = new XmlDocument();
                xd.Load(this._appUserXmlPath);
                XmlNode xn = xd.SelectSingleNode("/root/user[@code='" + e.Item.SubItems[0].Text + "']");
                if (e.Item.SubItems[0].Text == "Agency")
                {
                    this.txtUserCode.Enabled = false;
                    this.cmbUserRole.Enabled = false; 
                }
                else
                {
                    this.txtUserCode.Enabled = true;
                    this.cmbUserRole.Enabled = true;
                }

                switch (xn.Attributes["role"].Value)
                {
                    //case "Administrator":
                    //    this.cmbUserRole.SelectedIndex = 0; 
                    //    break;
                    case "Agency":
                        this.cmbUserRole.SelectedIndex = 0; 
                        break;
                    case "Manager":
                        this.cmbUserRole.SelectedIndex = 1;
                        break;
                    case "Users":
                        this.cmbUserRole.SelectedIndex = 2;
                        break;
                }

                this.txtUserCode.Text = xn.Attributes["code"].Value;
                this.txtUserName.Text = xn.Attributes["name"].Value;
                this.txtUserTel.Text = xn.Attributes["telephone"].Value;
                this.txtPassword.Text = this.txtConfirmPwd.Text = Cls.DESS.Decode(xn.Attributes["password"].Value); 
                Cls.XmlHelper.EncryptXml(_appUserXmlPath, "root");
            }
            else
            {
                this.cmbUserRole.Enabled = true;
                this.txtUserCode.Enabled = true;
                this.txtUserCode.Text = this.txtUserName.Text = this.txtUserTel.Text = this.txtPassword.Text = this.txtConfirmPwd.Text = string.Empty;
            }
        }

        private void gbtnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if(this.txtConfirmPwd.Text!=this.txtPassword.Text)
                {
                    MessageBox.Show("前后密码不一致，请重新输入！");
                    return;
                }

              if(_UserRole=="Agency" && this.cmbUserRole.SelectedIndex == 0 )
              {
                  MessageBox.Show("不允许修改！");
                  return;
              }

                //MessageBox.Show(this.listView1.Items[listView1.SelectedIndices[0]].SubItems[0].Text);
                Cls.XmlHelper.DecryptXml(_appUserXmlPath);
                XmlDocument xd = new XmlDocument();
                xd.Load(this._appUserXmlPath);
                XmlNode xn = xd.SelectSingleNode("/root/user[@code='" + this.txtUserCode.Text + "']");
                if (xn !=null)
                {
                    //插入用户
                    xn.Attributes["name"].Value = this.txtUserName.Text;
                    xn.Attributes["telephone"].Value = this.txtUserTel.Text; 
                    xn.Attributes["password"].Value = Cls.DESS.Encode(this.txtPassword.Text);
                    switch (this.cmbUserRole.SelectedIndex)
                    {
                        //case 0:
                        //    xn.Attributes["role"].Value = "Administrator";
                        //    break;
                        case 0:
                            xn.Attributes["role"].Value = "Agency";
                            break;
                        case 1:
                            xn.Attributes["role"].Value = "Manager";
                            break;
                        case 2:
                            xn.Attributes["role"].Value = "Users";
                            break;
                    }
                    xd.Save(_appUserXmlPath);
                    Cls.XmlHelper.EncryptXml(this._appUserXmlPath, "root");
                    CreateUserList();
                }
            }
            catch (Exception ee) { MessageBox.Show(ee.ToString()); }
        }
    }
}
