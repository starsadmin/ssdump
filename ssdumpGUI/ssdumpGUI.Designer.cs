namespace ssdump
{
    partial class ssdumpGUI
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
            menuStrip1 = new System.Windows.Forms.MenuStrip();
            fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            editHostsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            lblHost = new System.Windows.Forms.Label();
            cboHostList = new System.Windows.Forms.ComboBox();
            lblUsername = new System.Windows.Forms.Label();
            txtUsername = new System.Windows.Forms.TextBox();
            txtPassword = new System.Windows.Forms.TextBox();
            lblPassword = new System.Windows.Forms.Label();
            ckbNoData = new System.Windows.Forms.CheckBox();
            txtDatabase = new System.Windows.Forms.TextBox();
            lblDatabase = new System.Windows.Forms.Label();
            lblTables = new System.Windows.Forms.Label();
            txtTables = new System.Windows.Forms.TextBox();
            saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            btnExecute = new System.Windows.Forms.Button();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            ckbUseWinAuth = new System.Windows.Forms.CheckBox();
            lblWinAuth = new System.Windows.Forms.Label();
            groupBox1 = new System.Windows.Forms.GroupBox();
            tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            lblCreateUser = new System.Windows.Forms.Label();
            ckbCreateDatabase = new System.Windows.Forms.CheckBox();
            lblCreateDatabase = new System.Windows.Forms.Label();
            lblNoData = new System.Windows.Forms.Label();
            ckbCreateUser = new System.Windows.Forms.CheckBox();
            gpbConnectionSettings = new System.Windows.Forms.GroupBox();
            tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            lblMaxPacket = new System.Windows.Forms.Label();
            txtTimeout = new System.Windows.Forms.TextBox();
            lblTimeout = new System.Windows.Forms.Label();
            lblEncryption = new System.Windows.Forms.Label();
            ckbEncryption = new System.Windows.Forms.CheckBox();
            txtMaxPacket = new System.Windows.Forms.TextBox();
            lblVersion = new System.Windows.Forms.Label();
            menuStrip1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            groupBox1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            gpbConnectionSettings.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new System.Drawing.Size(40, 40);
            menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { fileToolStripMenuItem, settingsToolStripMenuItem });
            menuStrip1.Location = new System.Drawing.Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new System.Windows.Forms.Padding(17, 6, 0, 6);
            menuStrip1.Size = new System.Drawing.Size(1043, 57);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new System.Drawing.Size(87, 45);
            fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new System.Drawing.Size(230, 54);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // settingsToolStripMenuItem
            // 
            settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { editHostsToolStripMenuItem });
            settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            settingsToolStripMenuItem.Size = new System.Drawing.Size(149, 45);
            settingsToolStripMenuItem.Text = "Settings";
            // 
            // editHostsToolStripMenuItem
            // 
            editHostsToolStripMenuItem.Name = "editHostsToolStripMenuItem";
            editHostsToolStripMenuItem.Size = new System.Drawing.Size(356, 54);
            editHostsToolStripMenuItem.Text = "Edit Host List";
            editHostsToolStripMenuItem.Click += editHostsToolStripMenuItem_Click;
            // 
            // lblHost
            // 
            lblHost.Anchor = System.Windows.Forms.AnchorStyles.Left;
            lblHost.AutoSize = true;
            lblHost.Location = new System.Drawing.Point(8, 19);
            lblHost.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            lblHost.Name = "lblHost";
            lblHost.Size = new System.Drawing.Size(87, 41);
            lblHost.TabIndex = 1;
            lblHost.Text = "Host:";
            lblHost.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboHostList
            // 
            cboHostList.FormattingEnabled = true;
            cboHostList.Location = new System.Drawing.Point(444, 9);
            cboHostList.Margin = new System.Windows.Forms.Padding(8, 9, 8, 9);
            cboHostList.Name = "cboHostList";
            cboHostList.Size = new System.Drawing.Size(548, 49);
            cboHostList.TabIndex = 1;
            // 
            // lblUsername
            // 
            lblUsername.Anchor = System.Windows.Forms.AnchorStyles.Left;
            lblUsername.AutoSize = true;
            lblUsername.Location = new System.Drawing.Point(8, 179);
            lblUsername.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new System.Drawing.Size(159, 41);
            lblUsername.TabIndex = 3;
            lblUsername.Text = "Username:";
            lblUsername.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtUsername
            // 
            txtUsername.Location = new System.Drawing.Point(444, 169);
            txtUsername.Margin = new System.Windows.Forms.Padding(8, 9, 8, 9);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new System.Drawing.Size(548, 47);
            txtUsername.TabIndex = 3;
            // 
            // txtPassword
            // 
            txtPassword.Location = new System.Drawing.Point(444, 249);
            txtPassword.Margin = new System.Windows.Forms.Padding(8, 9, 8, 9);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '*';
            txtPassword.Size = new System.Drawing.Size(548, 47);
            txtPassword.TabIndex = 4;
            // 
            // lblPassword
            // 
            lblPassword.Anchor = System.Windows.Forms.AnchorStyles.Left;
            lblPassword.AutoSize = true;
            lblPassword.Location = new System.Drawing.Point(8, 259);
            lblPassword.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new System.Drawing.Size(150, 41);
            lblPassword.TabIndex = 4;
            lblPassword.Text = "Password:";
            lblPassword.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ckbNoData
            // 
            ckbNoData.AutoSize = true;
            ckbNoData.BackColor = System.Drawing.SystemColors.Control;
            ckbNoData.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            ckbNoData.Dock = System.Windows.Forms.DockStyle.Left;
            ckbNoData.ForeColor = System.Drawing.SystemColors.ControlText;
            ckbNoData.Location = new System.Drawing.Point(801, 9);
            ckbNoData.Margin = new System.Windows.Forms.Padding(8, 9, 8, 9);
            ckbNoData.Name = "ckbNoData";
            ckbNoData.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            ckbNoData.Size = new System.Drawing.Size(64, 55);
            ckbNoData.TabIndex = 0;
            ckbNoData.Text = " ";
            ckbNoData.UseVisualStyleBackColor = false;
            // 
            // txtDatabase
            // 
            txtDatabase.Location = new System.Drawing.Point(444, 329);
            txtDatabase.Margin = new System.Windows.Forms.Padding(8, 9, 8, 9);
            txtDatabase.Name = "txtDatabase";
            txtDatabase.Size = new System.Drawing.Size(548, 47);
            txtDatabase.TabIndex = 5;
            // 
            // lblDatabase
            // 
            lblDatabase.Anchor = System.Windows.Forms.AnchorStyles.Left;
            lblDatabase.AutoSize = true;
            lblDatabase.Location = new System.Drawing.Point(8, 341);
            lblDatabase.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            lblDatabase.Name = "lblDatabase";
            lblDatabase.Size = new System.Drawing.Size(235, 41);
            lblDatabase.TabIndex = 5;
            lblDatabase.Text = "Database Name:";
            lblDatabase.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTables
            // 
            lblTables.AutoSize = true;
            lblTables.Location = new System.Drawing.Point(42, 549);
            lblTables.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            lblTables.Name = "lblTables";
            lblTables.Size = new System.Drawing.Size(400, 41);
            lblTables.TabIndex = 6;
            lblTables.Text = "Table schemas (one per line):";
            // 
            // txtTables
            // 
            txtTables.Location = new System.Drawing.Point(51, 599);
            txtTables.Margin = new System.Windows.Forms.Padding(8, 9, 8, 9);
            txtTables.Multiline = true;
            txtTables.Name = "txtTables";
            txtTables.Size = new System.Drawing.Size(976, 231);
            txtTables.TabIndex = 1;
            // 
            // btnExecute
            // 
            btnExecute.Location = new System.Drawing.Point(422, 1571);
            btnExecute.Margin = new System.Windows.Forms.Padding(8, 9, 8, 9);
            btnExecute.Name = "btnExecute";
            btnExecute.Size = new System.Drawing.Size(612, 73);
            btnExecute.TabIndex = 4;
            btnExecute.Text = "Save SQL Server Dump to a File...";
            btnExecute.UseVisualStyleBackColor = true;
            btnExecute.Click += btnExecute_Click;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 42.77778F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 57.22222F));
            tableLayoutPanel1.Controls.Add(ckbUseWinAuth, 1, 1);
            tableLayoutPanel1.Controls.Add(lblWinAuth, 0, 1);
            tableLayoutPanel1.Controls.Add(txtPassword, 1, 3);
            tableLayoutPanel1.Controls.Add(lblPassword, 0, 3);
            tableLayoutPanel1.Controls.Add(cboHostList, 1, 0);
            tableLayoutPanel1.Controls.Add(lblHost, 0, 0);
            tableLayoutPanel1.Controls.Add(lblUsername, 0, 2);
            tableLayoutPanel1.Controls.Add(txtUsername, 1, 2);
            tableLayoutPanel1.Controls.Add(txtDatabase, 1, 4);
            tableLayoutPanel1.Controls.Add(lblDatabase, 0, 4);
            tableLayoutPanel1.Location = new System.Drawing.Point(34, 117);
            tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(8, 9, 8, 9);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 5;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            tableLayoutPanel1.Size = new System.Drawing.Size(1020, 404);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // ckbUseWinAuth
            // 
            ckbUseWinAuth.Anchor = System.Windows.Forms.AnchorStyles.Left;
            ckbUseWinAuth.AutoSize = true;
            ckbUseWinAuth.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            ckbUseWinAuth.Checked = true;
            ckbUseWinAuth.CheckState = System.Windows.Forms.CheckState.Checked;
            ckbUseWinAuth.Location = new System.Drawing.Point(444, 97);
            ckbUseWinAuth.Margin = new System.Windows.Forms.Padding(8, 9, 8, 9);
            ckbUseWinAuth.Name = "ckbUseWinAuth";
            ckbUseWinAuth.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            ckbUseWinAuth.Size = new System.Drawing.Size(64, 45);
            ckbUseWinAuth.TabIndex = 2;
            ckbUseWinAuth.Text = " ";
            ckbUseWinAuth.UseVisualStyleBackColor = true;
            ckbUseWinAuth.CheckedChanged += ckbUseWinAuth_CheckedChanged;
            // 
            // lblWinAuth
            // 
            lblWinAuth.Anchor = System.Windows.Forms.AnchorStyles.Left;
            lblWinAuth.AutoSize = true;
            lblWinAuth.Location = new System.Drawing.Point(8, 99);
            lblWinAuth.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            lblWinAuth.Name = "lblWinAuth";
            lblWinAuth.Size = new System.Drawing.Size(408, 41);
            lblWinAuth.TabIndex = 2;
            lblWinAuth.Text = "Use Windows Authentication:";
            lblWinAuth.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(tableLayoutPanel2);
            groupBox1.Location = new System.Drawing.Point(51, 880);
            groupBox1.Margin = new System.Windows.Forms.Padding(8, 9, 8, 9);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new System.Windows.Forms.Padding(8, 9, 8, 9);
            groupBox1.Size = new System.Drawing.Size(983, 315);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "Options";
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 83.57349F));
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.42651F));
            tableLayoutPanel2.Controls.Add(lblCreateUser, 0, 2);
            tableLayoutPanel2.Controls.Add(ckbCreateDatabase, 1, 1);
            tableLayoutPanel2.Controls.Add(lblCreateDatabase, 0, 1);
            tableLayoutPanel2.Controls.Add(ckbNoData, 1, 0);
            tableLayoutPanel2.Controls.Add(lblNoData, 0, 0);
            tableLayoutPanel2.Controls.Add(ckbCreateUser, 1, 2);
            tableLayoutPanel2.Location = new System.Drawing.Point(17, 60);
            tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(8, 9, 8, 9);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 3;
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            tableLayoutPanel2.Size = new System.Drawing.Size(949, 221);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // lblCreateUser
            // 
            lblCreateUser.Anchor = System.Windows.Forms.AnchorStyles.Left;
            lblCreateUser.AutoSize = true;
            lblCreateUser.Location = new System.Drawing.Point(8, 163);
            lblCreateUser.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            lblCreateUser.Name = "lblCreateUser";
            lblCreateUser.Size = new System.Drawing.Size(462, 41);
            lblCreateUser.TabIndex = 12;
            lblCreateUser.Text = "Include CREATE USER statements:";
            lblCreateUser.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ckbCreateDatabase
            // 
            ckbCreateDatabase.AutoSize = true;
            ckbCreateDatabase.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            ckbCreateDatabase.Dock = System.Windows.Forms.DockStyle.Left;
            ckbCreateDatabase.Location = new System.Drawing.Point(801, 82);
            ckbCreateDatabase.Margin = new System.Windows.Forms.Padding(8, 9, 8, 9);
            ckbCreateDatabase.Name = "ckbCreateDatabase";
            ckbCreateDatabase.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            ckbCreateDatabase.Size = new System.Drawing.Size(64, 55);
            ckbCreateDatabase.TabIndex = 1;
            ckbCreateDatabase.Text = " ";
            ckbCreateDatabase.UseVisualStyleBackColor = true;
            // 
            // lblCreateDatabase
            // 
            lblCreateDatabase.Anchor = System.Windows.Forms.AnchorStyles.Left;
            lblCreateDatabase.AutoSize = true;
            lblCreateDatabase.Location = new System.Drawing.Point(8, 89);
            lblCreateDatabase.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            lblCreateDatabase.Name = "lblCreateDatabase";
            lblCreateDatabase.Size = new System.Drawing.Size(529, 41);
            lblCreateDatabase.TabIndex = 10;
            lblCreateDatabase.Text = "Include CREATE DATABASE statements:";
            lblCreateDatabase.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblNoData
            // 
            lblNoData.Anchor = System.Windows.Forms.AnchorStyles.Left;
            lblNoData.AutoSize = true;
            lblNoData.Location = new System.Drawing.Point(8, 16);
            lblNoData.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            lblNoData.Name = "lblNoData";
            lblNoData.Size = new System.Drawing.Size(327, 41);
            lblNoData.TabIndex = 9;
            lblNoData.Text = "Schema Only (No Data)";
            lblNoData.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ckbCreateUser
            // 
            ckbCreateUser.AutoSize = true;
            ckbCreateUser.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            ckbCreateUser.Dock = System.Windows.Forms.DockStyle.Left;
            ckbCreateUser.Location = new System.Drawing.Point(801, 155);
            ckbCreateUser.Margin = new System.Windows.Forms.Padding(8, 9, 8, 9);
            ckbCreateUser.Name = "ckbCreateUser";
            ckbCreateUser.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            ckbCreateUser.Size = new System.Drawing.Size(64, 57);
            ckbCreateUser.TabIndex = 2;
            ckbCreateUser.Text = " ";
            ckbCreateUser.UseVisualStyleBackColor = true;
            // 
            // gpbConnectionSettings
            // 
            gpbConnectionSettings.Controls.Add(tableLayoutPanel3);
            gpbConnectionSettings.Location = new System.Drawing.Point(51, 1214);
            gpbConnectionSettings.Margin = new System.Windows.Forms.Padding(8, 9, 8, 9);
            gpbConnectionSettings.Name = "gpbConnectionSettings";
            gpbConnectionSettings.Padding = new System.Windows.Forms.Padding(8, 9, 8, 9);
            gpbConnectionSettings.Size = new System.Drawing.Size(983, 315);
            gpbConnectionSettings.TabIndex = 3;
            gpbConnectionSettings.TabStop = false;
            gpbConnectionSettings.Text = "Connection Settings";
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 2;
            tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 83.57349F));
            tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.42651F));
            tableLayoutPanel3.Controls.Add(lblMaxPacket, 0, 2);
            tableLayoutPanel3.Controls.Add(txtTimeout, 1, 1);
            tableLayoutPanel3.Controls.Add(lblTimeout, 0, 1);
            tableLayoutPanel3.Controls.Add(lblEncryption, 0, 0);
            tableLayoutPanel3.Controls.Add(ckbEncryption, 1, 0);
            tableLayoutPanel3.Controls.Add(txtMaxPacket, 1, 2);
            tableLayoutPanel3.Location = new System.Drawing.Point(17, 38);
            tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(8, 9, 8, 9);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 3;
            tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            tableLayoutPanel3.Size = new System.Drawing.Size(949, 259);
            tableLayoutPanel3.TabIndex = 0;
            // 
            // lblMaxPacket
            // 
            lblMaxPacket.Anchor = System.Windows.Forms.AnchorStyles.Left;
            lblMaxPacket.AutoSize = true;
            lblMaxPacket.Location = new System.Drawing.Point(8, 195);
            lblMaxPacket.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            lblMaxPacket.Name = "lblMaxPacket";
            lblMaxPacket.Size = new System.Drawing.Size(270, 41);
            lblMaxPacket.TabIndex = 15;
            lblMaxPacket.Text = "Max packet length:";
            lblMaxPacket.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtTimeout
            // 
            txtTimeout.Dock = System.Windows.Forms.DockStyle.Left;
            txtTimeout.Location = new System.Drawing.Point(801, 95);
            txtTimeout.Margin = new System.Windows.Forms.Padding(8, 9, 8, 9);
            txtTimeout.Name = "txtTimeout";
            txtTimeout.Size = new System.Drawing.Size(114, 47);
            txtTimeout.TabIndex = 1;
            txtTimeout.Text = "10";
            // 
            // lblTimeout
            // 
            lblTimeout.Anchor = System.Windows.Forms.AnchorStyles.Left;
            lblTimeout.AutoSize = true;
            lblTimeout.Location = new System.Drawing.Point(8, 108);
            lblTimeout.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            lblTimeout.Name = "lblTimeout";
            lblTimeout.Size = new System.Drawing.Size(425, 41);
            lblTimeout.TabIndex = 11;
            lblTimeout.Text = "Connection timeout (seconds):";
            lblTimeout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblEncryption
            // 
            lblEncryption.Anchor = System.Windows.Forms.AnchorStyles.Left;
            lblEncryption.AutoSize = true;
            lblEncryption.Location = new System.Drawing.Point(8, 22);
            lblEncryption.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            lblEncryption.Name = "lblEncryption";
            lblEncryption.Size = new System.Drawing.Size(373, 41);
            lblEncryption.TabIndex = 14;
            lblEncryption.Text = "Use encrypted connection:";
            lblEncryption.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ckbEncryption
            // 
            ckbEncryption.AutoSize = true;
            ckbEncryption.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            ckbEncryption.Dock = System.Windows.Forms.DockStyle.Left;
            ckbEncryption.Location = new System.Drawing.Point(801, 9);
            ckbEncryption.Margin = new System.Windows.Forms.Padding(8, 9, 8, 9);
            ckbEncryption.Name = "ckbEncryption";
            ckbEncryption.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            ckbEncryption.Size = new System.Drawing.Size(64, 68);
            ckbEncryption.TabIndex = 0;
            ckbEncryption.Text = " ";
            ckbEncryption.UseVisualStyleBackColor = true;
            // 
            // txtMaxPacket
            // 
            txtMaxPacket.Dock = System.Windows.Forms.DockStyle.Left;
            txtMaxPacket.Location = new System.Drawing.Point(801, 181);
            txtMaxPacket.Margin = new System.Windows.Forms.Padding(8, 9, 8, 9);
            txtMaxPacket.Name = "txtMaxPacket";
            txtMaxPacket.Size = new System.Drawing.Size(114, 47);
            txtMaxPacket.TabIndex = 2;
            // 
            // lblVersion
            // 
            lblVersion.AutoSize = true;
            lblVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            lblVersion.Location = new System.Drawing.Point(-3, 1646);
            lblVersion.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            lblVersion.Name = "lblVersion";
            lblVersion.Size = new System.Drawing.Size(56, 29);
            lblVersion.TabIndex = 20;
            lblVersion.Text = "v0.0";
            // 
            // ssdumpGUI
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(17F, 41F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1043, 1536);
            Controls.Add(lblVersion);
            Controls.Add(gpbConnectionSettings);
            Controls.Add(groupBox1);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(btnExecute);
            Controls.Add(txtTables);
            Controls.Add(lblTables);
            Controls.Add(menuStrip1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MainMenuStrip = menuStrip1;
            Margin = new System.Windows.Forms.Padding(8, 9, 8, 9);
            MaximizeBox = false;
            MaximumSize = new System.Drawing.Size(1075, 1624);
            MinimumSize = new System.Drawing.Size(1075, 1624);
            Name = "ssdumpGUI";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "ssdump (SQL Server Dump)";
            Activated += ssdumpGUI_Activated;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            groupBox1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            gpbConnectionSettings.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editHostsToolStripMenuItem;
        private System.Windows.Forms.Label lblHost;
        private System.Windows.Forms.ComboBox cboHostList;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.CheckBox ckbNoData;
        private System.Windows.Forms.TextBox txtDatabase;
        private System.Windows.Forms.Label lblDatabase;
        private System.Windows.Forms.Label lblTables;
        private System.Windows.Forms.TextBox txtTables;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label lblNoData;
        private System.Windows.Forms.Label lblCreateUser;
        private System.Windows.Forms.CheckBox ckbCreateDatabase;
        private System.Windows.Forms.Label lblCreateDatabase;
        private System.Windows.Forms.CheckBox ckbCreateUser;
        private System.Windows.Forms.GroupBox gpbConnectionSettings;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label lblMaxPacket;
        private System.Windows.Forms.TextBox txtTimeout;
        private System.Windows.Forms.Label lblTimeout;
        private System.Windows.Forms.Label lblEncryption;
        private System.Windows.Forms.CheckBox ckbEncryption;
        private System.Windows.Forms.TextBox txtMaxPacket;
        private System.Windows.Forms.Label lblWinAuth;
        private System.Windows.Forms.CheckBox ckbUseWinAuth;
        private System.Windows.Forms.Label lblVersion;
    }
}