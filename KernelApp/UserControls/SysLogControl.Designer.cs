namespace KernelApp.UserControls
{
    partial class SysLogControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.mainLayout = new System.Windows.Forms.TableLayoutPanel();
            this.headerPanel = new System.Windows.Forms.Panel();
            this.flowLayoutHeader = new System.Windows.Forms.FlowLayoutPanel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnRefresh = new Guna.UI2.WinForms.Guna2Button();
            this.btnClear = new Guna.UI2.WinForms.Guna2Button();
            this.btnExport = new Guna.UI2.WinForms.Guna2Button();
            this.chkAutoRefresh = new Guna.UI2.WinForms.Guna2CheckBox();
            this.lblLogPath = new System.Windows.Forms.Label();
            this.logGridPanel = new System.Windows.Forms.Panel();
            this.dgvLogs = new System.Windows.Forms.DataGridView();
            this.colTimestamp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMessage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rawLogPanel = new System.Windows.Forms.Panel();
            this.lblRawTitle = new System.Windows.Forms.Label();
            this.rtbRawLog = new System.Windows.Forms.RichTextBox();
            this.statusPanel = new System.Windows.Forms.Panel();
            this.flowLayoutStatus = new System.Windows.Forms.FlowLayoutPanel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblLogCount = new System.Windows.Forms.Label();
            this.lblFileSize = new System.Windows.Forms.Label();
            this.lblLastRefresh = new System.Windows.Forms.Label();
            this.refreshTimer = new System.Windows.Forms.Timer(this.components);
            this.mainLayout.SuspendLayout();
            this.headerPanel.SuspendLayout();
            this.flowLayoutHeader.SuspendLayout();
            this.logGridPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLogs)).BeginInit();
            this.rawLogPanel.SuspendLayout();
            this.statusPanel.SuspendLayout();
            this.flowLayoutStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainLayout
            // 
            this.mainLayout.BackColor = System.Drawing.Color.Transparent;
            this.mainLayout.ColumnCount = 1;
            this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayout.Controls.Add(this.headerPanel, 0, 0);
            this.mainLayout.Controls.Add(this.logGridPanel, 0, 1);
            this.mainLayout.Controls.Add(this.rawLogPanel, 0, 2);
            this.mainLayout.Controls.Add(this.statusPanel, 0, 3);
            this.mainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayout.Location = new System.Drawing.Point(10, 10);
            this.mainLayout.Name = "mainLayout";
            this.mainLayout.RowCount = 4;
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.mainLayout.Size = new System.Drawing.Size(1091, 645);
            this.mainLayout.TabIndex = 0;
            // 
            // headerPanel
            // 
            this.headerPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(46)))));
            this.headerPanel.Controls.Add(this.flowLayoutHeader);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.headerPanel.Location = new System.Drawing.Point(3, 3);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.headerPanel.Size = new System.Drawing.Size(1085, 49);
            this.headerPanel.TabIndex = 0;
            // 
            // flowLayoutHeader
            // 
            this.flowLayoutHeader.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutHeader.Controls.Add(this.lblTitle);
            this.flowLayoutHeader.Controls.Add(this.btnRefresh);
            this.flowLayoutHeader.Controls.Add(this.btnClear);
            this.flowLayoutHeader.Controls.Add(this.btnExport);
            this.flowLayoutHeader.Controls.Add(this.chkAutoRefresh);
            this.flowLayoutHeader.Controls.Add(this.lblLogPath);
            this.flowLayoutHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutHeader.Location = new System.Drawing.Point(12, 8);
            this.flowLayoutHeader.Name = "flowLayoutHeader";
            this.flowLayoutHeader.Size = new System.Drawing.Size(1061, 33);
            this.flowLayoutHeader.TabIndex = 0;
            this.flowLayoutHeader.WrapContents = false;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.lblTitle.Size = new System.Drawing.Size(116, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "SYSCALL LOGS";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Animated = true;
            this.btnRefresh.BackColor = System.Drawing.Color.Transparent;
            this.btnRefresh.BorderRadius = 6;
            this.btnRefresh.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnRefresh.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnRefresh.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnRefresh.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnRefresh.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(70)))), ((int)(((byte)(229)))));
            this.btnRefresh.Location = new System.Drawing.Point(139, 3);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(85, 28);
            this.btnRefresh.TabIndex = 1;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnClear
            // 
            this.btnClear.Animated = true;
            this.btnClear.BackColor = System.Drawing.Color.Transparent;
            this.btnClear.BorderRadius = 6;
            this.btnClear.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnClear.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnClear.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnClear.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnClear.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnClear.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.btnClear.Location = new System.Drawing.Point(225, 3);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(80, 28);
            this.btnClear.TabIndex = 2;
            this.btnClear.Text = "Clear Log";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnExport
            // 
            this.btnExport.Animated = true;
            this.btnExport.BackColor = System.Drawing.Color.Transparent;
            this.btnExport.BorderRadius = 6;
            this.btnExport.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnExport.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnExport.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnExport.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnExport.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(197)))), ((int)(((byte)(94)))));
            this.btnExport.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.btnExport.ForeColor = System.Drawing.Color.White;
            this.btnExport.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.btnExport.Location = new System.Drawing.Point(311, 3);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(80, 28);
            this.btnExport.TabIndex = 3;
            this.btnExport.Text = "Export";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // chkAutoRefresh
            // 
            this.chkAutoRefresh.AutoSize = true;
            this.chkAutoRefresh.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
            this.chkAutoRefresh.CheckedState.BorderRadius = 4;
            this.chkAutoRefresh.CheckedState.BorderThickness = 0;
            this.chkAutoRefresh.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
            this.chkAutoRefresh.Font = new System.Drawing.Font("Consolas", 9F);
            this.chkAutoRefresh.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(173)))), ((int)(((byte)(186)))));
            this.chkAutoRefresh.Location = new System.Drawing.Point(397, 3);
            this.chkAutoRefresh.Margin = new System.Windows.Forms.Padding(3, 3, 20, 3);
            this.chkAutoRefresh.Name = "chkAutoRefresh";
            this.chkAutoRefresh.Size = new System.Drawing.Size(108, 18);
            this.chkAutoRefresh.TabIndex = 4;
            this.chkAutoRefresh.Text = "Auto Refresh";
            this.chkAutoRefresh.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(75)))));
            this.chkAutoRefresh.UncheckedState.BorderRadius = 4;
            this.chkAutoRefresh.UncheckedState.BorderThickness = 1;
            this.chkAutoRefresh.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(24)))), ((int)(((byte)(37)))));
            this.chkAutoRefresh.CheckedChanged += new System.EventHandler(this.chkAutoRefresh_CheckedChanged);
            // 
            // lblLogPath
            // 
            this.lblLogPath.AutoSize = true;
            this.lblLogPath.Font = new System.Drawing.Font("Consolas", 9F);
            this.lblLogPath.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(120)))));
            this.lblLogPath.Location = new System.Drawing.Point(525, 0);
            this.lblLogPath.Margin = new System.Windows.Forms.Padding(0);
            this.lblLogPath.Name = "lblLogPath";
            this.lblLogPath.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.lblLogPath.Size = new System.Drawing.Size(140, 22);
            this.lblLogPath.TabIndex = 5;
            this.lblLogPath.Text = "Logs\\kernel.log";
            // 
            // logGridPanel
            // 
            this.logGridPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(46)))));
            this.logGridPanel.Controls.Add(this.dgvLogs);
            this.logGridPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logGridPanel.Location = new System.Drawing.Point(3, 58);
            this.logGridPanel.Name = "logGridPanel";
            this.logGridPanel.Padding = new System.Windows.Forms.Padding(10);
            this.logGridPanel.Size = new System.Drawing.Size(1085, 327);
            this.logGridPanel.TabIndex = 1;
            // 
            // dgvLogs
            // 
            this.dgvLogs.AllowUserToAddRows = false;
            this.dgvLogs.AllowUserToDeleteRows = false;
            this.dgvLogs.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(46)))));
            this.dgvLogs.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvLogs.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvLogs.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(24)))), ((int)(((byte)(37)))));
            this.dgvLogs.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvLogs.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvLogs.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(65)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(65)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvLogs.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvLogs.ColumnHeadersHeight = 35;
            this.dgvLogs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvLogs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTimestamp,
            this.colType,
            this.colMessage});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(24)))), ((int)(((byte)(37)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Consolas", 9.75F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(173)))), ((int)(((byte)(186)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(85)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvLogs.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvLogs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLogs.EnableHeadersVisualStyles = false;
            this.dgvLogs.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(75)))));
            this.dgvLogs.Location = new System.Drawing.Point(10, 10);
            this.dgvLogs.MultiSelect = false;
            this.dgvLogs.Name = "dgvLogs";
            this.dgvLogs.ReadOnly = true;
            this.dgvLogs.RowHeadersVisible = false;
            this.dgvLogs.RowTemplate.Height = 28;
            this.dgvLogs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLogs.Size = new System.Drawing.Size(1065, 307);
            this.dgvLogs.TabIndex = 0;
            this.dgvLogs.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvLogs_CellFormatting);
            // 
            // colTimestamp
            // 
            this.colTimestamp.FillWeight = 80F;
            this.colTimestamp.HeaderText = "TIMESTAMP";
            this.colTimestamp.Name = "colTimestamp";
            this.colTimestamp.ReadOnly = true;
            // 
            // colType
            // 
            this.colType.FillWeight = 50F;
            this.colType.HeaderText = "TYPE";
            this.colType.Name = "colType";
            this.colType.ReadOnly = true;
            // 
            // colMessage
            // 
            this.colMessage.FillWeight = 200F;
            this.colMessage.HeaderText = "MESSAGE";
            this.colMessage.Name = "colMessage";
            this.colMessage.ReadOnly = true;
            // 
            // rawLogPanel
            // 
            this.rawLogPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(46)))));
            this.rawLogPanel.Controls.Add(this.lblRawTitle);
            this.rawLogPanel.Controls.Add(this.rtbRawLog);
            this.rawLogPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rawLogPanel.Location = new System.Drawing.Point(3, 391);
            this.rawLogPanel.Name = "rawLogPanel";
            this.rawLogPanel.Padding = new System.Windows.Forms.Padding(10, 8, 10, 10);
            this.rawLogPanel.Size = new System.Drawing.Size(1085, 216);
            this.rawLogPanel.TabIndex = 2;
            // 
            // lblRawTitle
            // 
            this.lblRawTitle.AutoSize = true;
            this.lblRawTitle.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold);
            this.lblRawTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(173)))), ((int)(((byte)(186)))));
            this.lblRawTitle.Location = new System.Drawing.Point(10, 8);
            this.lblRawTitle.Name = "lblRawTitle";
            this.lblRawTitle.Size = new System.Drawing.Size(98, 14);
            this.lblRawTitle.TabIndex = 0;
            this.lblRawTitle.Text = "RAW LOG FILE";
            // 
            // rtbRawLog
            // 
            this.rtbRawLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbRawLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(25)))));
            this.rtbRawLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbRawLog.Font = new System.Drawing.Font("Consolas", 9.75F);
            this.rtbRawLog.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(197)))), ((int)(((byte)(94)))));
            this.rtbRawLog.Location = new System.Drawing.Point(10, 28);
            this.rtbRawLog.Name = "rtbRawLog";
            this.rtbRawLog.ReadOnly = true;
            this.rtbRawLog.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtbRawLog.Size = new System.Drawing.Size(1065, 178);
            this.rtbRawLog.TabIndex = 1;
            this.rtbRawLog.Text = "";
            // 
            // statusPanel
            // 
            this.statusPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(46)))));
            this.statusPanel.Controls.Add(this.flowLayoutStatus);
            this.statusPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusPanel.Location = new System.Drawing.Point(3, 613);
            this.statusPanel.Name = "statusPanel";
            this.statusPanel.Padding = new System.Windows.Forms.Padding(12, 4, 12, 4);
            this.statusPanel.Size = new System.Drawing.Size(1085, 29);
            this.statusPanel.TabIndex = 3;
            // 
            // flowLayoutStatus
            // 
            this.flowLayoutStatus.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutStatus.Controls.Add(this.lblStatus);
            this.flowLayoutStatus.Controls.Add(this.lblLogCount);
            this.flowLayoutStatus.Controls.Add(this.lblFileSize);
            this.flowLayoutStatus.Controls.Add(this.lblLastRefresh);
            this.flowLayoutStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutStatus.Location = new System.Drawing.Point(12, 4);
            this.flowLayoutStatus.Name = "flowLayoutStatus";
            this.flowLayoutStatus.Size = new System.Drawing.Size(1061, 21);
            this.flowLayoutStatus.TabIndex = 0;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Consolas", 9F);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(197)))), ((int)(((byte)(94)))));
            this.lblStatus.Location = new System.Drawing.Point(0, 0);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(0, 0, 30, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.lblStatus.Size = new System.Drawing.Size(126, 17);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "Log Monitor Active";
            // 
            // lblLogCount
            // 
            this.lblLogCount.AutoSize = true;
            this.lblLogCount.Font = new System.Drawing.Font("Consolas", 9F);
            this.lblLogCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(173)))), ((int)(((byte)(186)))));
            this.lblLogCount.Location = new System.Drawing.Point(156, 0);
            this.lblLogCount.Margin = new System.Windows.Forms.Padding(0, 0, 30, 0);
            this.lblLogCount.Name = "lblLogCount";
            this.lblLogCount.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.lblLogCount.Size = new System.Drawing.Size(84, 17);
            this.lblLogCount.TabIndex = 1;
            this.lblLogCount.Text = "Entries: 0";
            // 
            // lblFileSize
            // 
            this.lblFileSize.AutoSize = true;
            this.lblFileSize.Font = new System.Drawing.Font("Consolas", 9F);
            this.lblFileSize.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(173)))), ((int)(((byte)(186)))));
            this.lblFileSize.Location = new System.Drawing.Point(270, 0);
            this.lblFileSize.Margin = new System.Windows.Forms.Padding(0, 0, 30, 0);
            this.lblFileSize.Name = "lblFileSize";
            this.lblFileSize.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.lblFileSize.Size = new System.Drawing.Size(98, 17);
            this.lblFileSize.TabIndex = 2;
            this.lblFileSize.Text = "Size: 0 bytes";
            // 
            // lblLastRefresh
            // 
            this.lblLastRefresh.AutoSize = true;
            this.lblLastRefresh.Font = new System.Drawing.Font("Consolas", 9F);
            this.lblLastRefresh.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(173)))), ((int)(((byte)(186)))));
            this.lblLastRefresh.Location = new System.Drawing.Point(398, 0);
            this.lblLastRefresh.Margin = new System.Windows.Forms.Padding(0);
            this.lblLastRefresh.Name = "lblLastRefresh";
            this.lblLastRefresh.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.lblLastRefresh.Size = new System.Drawing.Size(154, 17);
            this.lblLastRefresh.TabIndex = 3;
            this.lblLastRefresh.Text = "Last Refresh: --:--:--";
            // 
            // refreshTimer
            // 
            this.refreshTimer.Interval = 3000;
            this.refreshTimer.Tick += new System.EventHandler(this.refreshTimer_Tick);
            // 
            // SysLogControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(51)))));
            this.Controls.Add(this.mainLayout);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "SysLogControl";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Size = new System.Drawing.Size(1111, 665);
            this.Load += new System.EventHandler(this.SysLogControl_Load);
            this.mainLayout.ResumeLayout(false);
            this.headerPanel.ResumeLayout(false);
            this.flowLayoutHeader.ResumeLayout(false);
            this.flowLayoutHeader.PerformLayout();
            this.logGridPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLogs)).EndInit();
            this.rawLogPanel.ResumeLayout(false);
            this.rawLogPanel.PerformLayout();
            this.statusPanel.ResumeLayout(false);
            this.flowLayoutStatus.ResumeLayout(false);
            this.flowLayoutStatus.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel mainLayout;
        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutHeader;
        private System.Windows.Forms.Label lblTitle;
        private Guna.UI2.WinForms.Guna2Button btnRefresh;
        private Guna.UI2.WinForms.Guna2Button btnClear;
        private Guna.UI2.WinForms.Guna2Button btnExport;
        private Guna.UI2.WinForms.Guna2CheckBox chkAutoRefresh;
        private System.Windows.Forms.Label lblLogPath;
        private System.Windows.Forms.Panel logGridPanel;
        private System.Windows.Forms.DataGridView dgvLogs;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTimestamp;
        private System.Windows.Forms.DataGridViewTextBoxColumn colType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMessage;
        private System.Windows.Forms.Panel rawLogPanel;
        private System.Windows.Forms.Label lblRawTitle;
        private System.Windows.Forms.RichTextBox rtbRawLog;
        private System.Windows.Forms.Panel statusPanel;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutStatus;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblLogCount;
        private System.Windows.Forms.Label lblFileSize;
        private System.Windows.Forms.Label lblLastRefresh;
        private System.Windows.Forms.Timer refreshTimer;
    }
}
