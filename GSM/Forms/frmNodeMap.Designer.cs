namespace GSM.Forms
{
    partial class frmNodeMap
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.treeViewNodes = new System.Windows.Forms.TreeView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblNode = new System.Windows.Forms.Label();
            this.lblNodeID = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbNodeType = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtNodePos = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnShowNodes = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cmbNodeZoneID = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnAllMapFile = new System.Windows.Forms.Button();
            this.btnZoneMapFile = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.picZoneMap = new System.Windows.Forms.PictureBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.btnShowFullMap = new System.Windows.Forms.Button();
            this.cmbZoneID = new System.Windows.Forms.ComboBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picZoneMap)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeViewNodes
            // 
            this.treeViewNodes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewNodes.Location = new System.Drawing.Point(3, 44);
            this.treeViewNodes.Name = "treeViewNodes";
            this.treeViewNodes.Size = new System.Drawing.Size(246, 319);
            this.treeViewNodes.TabIndex = 0;
            this.treeViewNodes.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeViewNodesAfterSelect);
            this.treeViewNodes.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewNodes_NodeMouseClick);
            this.treeViewNodes.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewNodes_NodeMouseDoubleClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "设备类型";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(148, 2);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "设备编号";
            // 
            // lblNode
            // 
            this.lblNode.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblNode.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblNode.Location = new System.Drawing.Point(8, 17);
            this.lblNode.Name = "lblNode";
            this.lblNode.Size = new System.Drawing.Size(136, 23);
            this.lblNode.TabIndex = 2;
            this.lblNode.Text = "类型";
            // 
            // lblNodeID
            // 
            this.lblNodeID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNodeID.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblNodeID.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblNodeID.Location = new System.Drawing.Point(150, 17);
            this.lblNodeID.Name = "lblNodeID";
            this.lblNodeID.Size = new System.Drawing.Size(84, 23);
            this.lblNodeID.TabIndex = 2;
            this.lblNodeID.Text = "ID";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 56);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 12);
            this.label5.TabIndex = 1;
            this.label5.Text = "设备型号:";
            // 
            // cmbNodeType
            // 
            this.cmbNodeType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbNodeType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNodeType.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbNodeType.FormattingEnabled = true;
            this.cmbNodeType.Location = new System.Drawing.Point(65, 49);
            this.cmbNodeType.Name = "cmbNodeType";
            this.cmbNodeType.Size = new System.Drawing.Size(169, 24);
            this.cmbNodeType.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(30, 84);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 12);
            this.label6.TabIndex = 1;
            this.label6.Text = "位置:";
            // 
            // txtNodePos
            // 
            this.txtNodePos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNodePos.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtNodePos.Location = new System.Drawing.Point(65, 79);
            this.txtNodePos.Name = "txtNodePos";
            this.txtNodePos.Size = new System.Drawing.Size(169, 23);
            this.txtNodePos.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.treeViewNodes);
            this.groupBox1.Controls.Add(this.btnShowNodes);
            this.groupBox1.Controls.Add(this.panel2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(252, 536);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "节点列表";
            // 
            // btnShowNodes
            // 
            this.btnShowNodes.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnShowNodes.Location = new System.Drawing.Point(3, 17);
            this.btnShowNodes.Name = "btnShowNodes";
            this.btnShowNodes.Size = new System.Drawing.Size(246, 27);
            this.btnShowNodes.TabIndex = 7;
            this.btnShowNodes.Text = "刷新在线节点列表";
            this.btnShowNodes.UseVisualStyleBackColor = true;
            this.btnShowNodes.Click += new System.EventHandler(this.btnShowNodes_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.cmbNodeZoneID);
            this.panel2.Controls.Add(this.txtNodePos);
            this.panel2.Controls.Add(this.cmbNodeType);
            this.panel2.Controls.Add(this.label10);
            this.panel2.Controls.Add(this.lblNodeID);
            this.panel2.Controls.Add(this.btnSave);
            this.panel2.Controls.Add(this.lblNode);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(3, 363);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(246, 170);
            this.panel2.TabIndex = 6;
            // 
            // cmbNodeZoneID
            // 
            this.cmbNodeZoneID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbNodeZoneID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNodeZoneID.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbNodeZoneID.FormattingEnabled = true;
            this.cmbNodeZoneID.Location = new System.Drawing.Point(65, 108);
            this.cmbNodeZoneID.Name = "cmbNodeZoneID";
            this.cmbNodeZoneID.Size = new System.Drawing.Size(169, 24);
            this.cmbNodeZoneID.TabIndex = 3;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(8, 115);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(59, 12);
            this.label10.TabIndex = 1;
            this.label10.Text = "区域编号:";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(150, 138);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(84, 23);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "确认";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnAllMapFile
            // 
            this.btnAllMapFile.Location = new System.Drawing.Point(414, 3);
            this.btnAllMapFile.Name = "btnAllMapFile";
            this.btnAllMapFile.Size = new System.Drawing.Size(86, 23);
            this.btnAllMapFile.TabIndex = 3;
            this.btnAllMapFile.Text = "设置全局图";
            this.btnAllMapFile.UseVisualStyleBackColor = true;
            this.btnAllMapFile.Click += new System.EventHandler(this.btnAllMapFile_Click);
            // 
            // btnZoneMapFile
            // 
            this.btnZoneMapFile.Location = new System.Drawing.Point(189, 4);
            this.btnZoneMapFile.Name = "btnZoneMapFile";
            this.btnZoneMapFile.Size = new System.Drawing.Size(109, 23);
            this.btnZoneMapFile.TabIndex = 5;
            this.btnZoneMapFile.Text = "区域地图-浏览...";
            this.btnZoneMapFile.UseVisualStyleBackColor = true;
            this.btnZoneMapFile.Click += new System.EventHandler(this.BtnZoneMapFileClick);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.panel1);
            this.groupBox3.Controls.Add(this.panel3);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(671, 536);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "地图设置";
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.picZoneMap);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 49);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(665, 484);
            this.panel1.TabIndex = 6;
            // 
            // picZoneMap
            // 
            this.picZoneMap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picZoneMap.Location = new System.Drawing.Point(1, 0);
            this.picZoneMap.Name = "picZoneMap";
            this.picZoneMap.Size = new System.Drawing.Size(387, 315);
            this.picZoneMap.TabIndex = 0;
            this.picZoneMap.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.btnZoneMapFile);
            this.panel3.Controls.Add(this.btnShowFullMap);
            this.panel3.Controls.Add(this.btnAllMapFile);
            this.panel3.Controls.Add(this.cmbZoneID);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(3, 17);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(665, 32);
            this.panel3.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "区域编号:";
            // 
            // btnShowFullMap
            // 
            this.btnShowFullMap.Location = new System.Drawing.Point(506, 3);
            this.btnShowFullMap.Name = "btnShowFullMap";
            this.btnShowFullMap.Size = new System.Drawing.Size(86, 23);
            this.btnShowFullMap.TabIndex = 3;
            this.btnShowFullMap.Text = "查看全局图";
            this.btnShowFullMap.UseVisualStyleBackColor = true;
            this.btnShowFullMap.Click += new System.EventHandler(this.btnShowFullMap_Click);
            // 
            // cmbZoneID
            // 
            this.cmbZoneID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbZoneID.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbZoneID.FormattingEnabled = true;
            this.cmbZoneID.Location = new System.Drawing.Point(78, 3);
            this.cmbZoneID.Name = "cmbZoneID";
            this.cmbZoneID.Size = new System.Drawing.Size(105, 24);
            this.cmbZoneID.TabIndex = 3;
            this.cmbZoneID.SelectedIndexChanged += new System.EventHandler(this.CmbZoneIDSelectedIndexChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox3);
            this.splitContainer1.Size = new System.Drawing.Size(927, 536);
            this.splitContainer1.SplitterDistance = 252;
            this.splitContainer1.TabIndex = 7;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 536);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(927, 22);
            this.statusStrip1.TabIndex = 8;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // frmNodeMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(927, 558);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmNodeMap";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "地图设置";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmNodeMap_Load);
            this.groupBox1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picZoneMap)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;

        #endregion

        private System.Windows.Forms.TreeView treeViewNodes;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblNode;
        private System.Windows.Forms.Label lblNodeID;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbNodeType;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtNodePos;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmbNodeZoneID;
        private System.Windows.Forms.Button btnZoneMapFile;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.PictureBox picZoneMap;
        private System.Windows.Forms.ComboBox cmbZoneID;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnShowFullMap;
        private System.Windows.Forms.Button btnShowNodes;
        public System.Windows.Forms.Button btnAllMapFile;
    }
}