namespace KernelApp.UserControls
{
    partial class MemoryVisControl
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
            this.mainLayout = new System.Windows.Forms.TableLayoutPanel();
            this.headerPanel = new System.Windows.Forms.Panel();
            this.flowLayoutHeader = new System.Windows.Forms.FlowLayoutPanel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblTotalMemory = new System.Windows.Forms.Label();
            this.lblUsedMemory = new System.Windows.Forms.Label();
            this.lblFreeMemory = new System.Windows.Forms.Label();
            this.lblUsagePercent = new System.Windows.Forms.Label();
            this.btnRefreshMemory = new Guna.UI2.WinForms.Guna2Button();
            this.memoryBarPanel = new System.Windows.Forms.Panel();
            this.lblMemoryBarTitle = new System.Windows.Forms.Label();
            this.pnlMemoryBarOuter = new System.Windows.Forms.Panel();
            this.pnlMemoryBarInner = new System.Windows.Forms.Panel();
            this.lblBarPercent = new System.Windows.Forms.Label();
            this.segmentsPanel = new System.Windows.Forms.Panel();
            this.lblSegmentsTitle = new System.Windows.Forms.Label();
            this.pnlSegmentsContainer = new System.Windows.Forms.FlowLayoutPanel();
            this.processMemoryPanel = new System.Windows.Forms.Panel();
            this.dgvProcessMemory = new System.Windows.Forms.DataGridView();
            this.colPID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProcessName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMemoryMB = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMemoryPercent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblProcessMemoryTitle = new System.Windows.Forms.Label();
            this.statusPanel = new System.Windows.Forms.Panel();
            this.flowLayoutStatus = new System.Windows.Forms.FlowLayoutPanel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblLastUpdate = new System.Windows.Forms.Label();
            this.refreshTimer = new System.Windows.Forms.Timer(this.components);
            this.mainLayout.SuspendLayout();
            this.headerPanel.SuspendLayout();
            this.flowLayoutHeader.SuspendLayout();
            this.memoryBarPanel.SuspendLayout();
            this.pnlMemoryBarOuter.SuspendLayout();
            this.segmentsPanel.SuspendLayout();
            this.processMemoryPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProcessMemory)).BeginInit();
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
            this.mainLayout.Controls.Add(this.memoryBarPanel, 0, 1);
            this.mainLayout.Controls.Add(this.segmentsPanel, 0, 2);
            this.mainLayout.Controls.Add(this.processMemoryPanel, 0, 3);
            this.mainLayout.Controls.Add(this.statusPanel, 0, 4);
            this.mainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayout.Location = new System.Drawing.Point(10, 10);
            this.mainLayout.Name = "mainLayout";
            this.mainLayout.RowCount = 5;
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 85F));
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
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
            this.headerPanel.Size = new System.Drawing.Size(1085, 54);
            this.headerPanel.TabIndex = 0;
            // 
            // flowLayoutHeader
            // 
            this.flowLayoutHeader.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutHeader.Controls.Add(this.lblTitle);
            this.flowLayoutHeader.Controls.Add(this.lblTotalMemory);
            this.flowLayoutHeader.Controls.Add(this.lblUsedMemory);
            this.flowLayoutHeader.Controls.Add(this.lblFreeMemory);
            this.flowLayoutHeader.Controls.Add(this.lblUsagePercent);
            this.flowLayoutHeader.Controls.Add(this.btnRefreshMemory);
            this.flowLayoutHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutHeader.Location = new System.Drawing.Point(12, 8);
            this.flowLayoutHeader.Name = "flowLayoutHeader";
            this.flowLayoutHeader.Size = new System.Drawing.Size(1061, 38);
            this.flowLayoutHeader.TabIndex = 0;
            this.flowLayoutHeader.WrapContents = false;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(0, 0, 25, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.lblTitle.Size = new System.Drawing.Size(144, 27);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "MEMORY MANAGER";
            // 
            // lblTotalMemory
            // 
            this.lblTotalMemory.AutoSize = true;
            this.lblTotalMemory.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Bold);
            this.lblTotalMemory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(173)))), ((int)(((byte)(186)))));
            this.lblTotalMemory.Location = new System.Drawing.Point(169, 0);
            this.lblTotalMemory.Margin = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.lblTotalMemory.Name = "lblTotalMemory";
            this.lblTotalMemory.Padding = new System.Windows.Forms.Padding(0, 9, 0, 0);
            this.lblTotalMemory.Size = new System.Drawing.Size(128, 26);
            this.lblTotalMemory.TabIndex = 1;
            this.lblTotalMemory.Text = "TOTAL: 4096 MB";
            // 
            // lblUsedMemory
            // 
            this.lblUsedMemory.AutoSize = true;
            this.lblUsedMemory.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Bold);
            this.lblUsedMemory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.lblUsedMemory.Location = new System.Drawing.Point(317, 0);
            this.lblUsedMemory.Margin = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.lblUsedMemory.Name = "lblUsedMemory";
            this.lblUsedMemory.Padding = new System.Windows.Forms.Padding(0, 9, 0, 0);
            this.lblUsedMemory.Size = new System.Drawing.Size(104, 26);
            this.lblUsedMemory.TabIndex = 2;
            this.lblUsedMemory.Text = "USED: 0 MB";
            // 
            // lblFreeMemory
            // 
            this.lblFreeMemory.AutoSize = true;
            this.lblFreeMemory.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Bold);
            this.lblFreeMemory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(197)))), ((int)(((byte)(94)))));
            this.lblFreeMemory.Location = new System.Drawing.Point(441, 0);
            this.lblFreeMemory.Margin = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.lblFreeMemory.Name = "lblFreeMemory";
            this.lblFreeMemory.Padding = new System.Windows.Forms.Padding(0, 9, 0, 0);
            this.lblFreeMemory.Size = new System.Drawing.Size(104, 26);
            this.lblFreeMemory.TabIndex = 3;
            this.lblFreeMemory.Text = "FREE: 0 MB";
            // 
            // lblUsagePercent
            // 
            this.lblUsagePercent.AutoSize = true;
            this.lblUsagePercent.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Bold);
            this.lblUsagePercent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(191)))), ((int)(((byte)(36)))));
            this.lblUsagePercent.Location = new System.Drawing.Point(565, 0);
            this.lblUsagePercent.Margin = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.lblUsagePercent.Name = "lblUsagePercent";
            this.lblUsagePercent.Padding = new System.Windows.Forms.Padding(0, 9, 0, 0);
            this.lblUsagePercent.Size = new System.Drawing.Size(80, 26);
            this.lblUsagePercent.TabIndex = 4;
            this.lblUsagePercent.Text = "USAGE: 0%";
            // 
            // btnRefreshMemory
            // 
            this.btnRefreshMemory.Animated = true;
            this.btnRefreshMemory.BackColor = System.Drawing.Color.Transparent;
            this.btnRefreshMemory.BorderRadius = 6;
            this.btnRefreshMemory.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnRefreshMemory.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnRefreshMemory.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnRefreshMemory.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnRefreshMemory.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
            this.btnRefreshMemory.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.btnRefreshMemory.ForeColor = System.Drawing.Color.White;
            this.btnRefreshMemory.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(70)))), ((int)(((byte)(229)))));
            this.btnRefreshMemory.Location = new System.Drawing.Point(668, 3);
            this.btnRefreshMemory.Name = "btnRefreshMemory";
            this.btnRefreshMemory.Size = new System.Drawing.Size(90, 32);
            this.btnRefreshMemory.TabIndex = 5;
            this.btnRefreshMemory.Text = "Refresh";
            this.btnRefreshMemory.Click += new System.EventHandler(this.btnRefreshMemory_Click);
            // 
            // memoryBarPanel
            // 
            this.memoryBarPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(46)))));
            this.memoryBarPanel.Controls.Add(this.lblMemoryBarTitle);
            this.memoryBarPanel.Controls.Add(this.pnlMemoryBarOuter);
            this.memoryBarPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.memoryBarPanel.Location = new System.Drawing.Point(3, 63);
            this.memoryBarPanel.Name = "memoryBarPanel";
            this.memoryBarPanel.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.memoryBarPanel.Size = new System.Drawing.Size(1085, 79);
            this.memoryBarPanel.TabIndex = 1;
            // 
            // lblMemoryBarTitle
            // 
            this.lblMemoryBarTitle.AutoSize = true;
            this.lblMemoryBarTitle.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold);
            this.lblMemoryBarTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(173)))), ((int)(((byte)(186)))));
            this.lblMemoryBarTitle.Location = new System.Drawing.Point(12, 8);
            this.lblMemoryBarTitle.Name = "lblMemoryBarTitle";
            this.lblMemoryBarTitle.Size = new System.Drawing.Size(126, 14);
            this.lblMemoryBarTitle.TabIndex = 0;
            this.lblMemoryBarTitle.Text = "MEMORY USAGE BAR";
            // 
            // pnlMemoryBarOuter
            // 
            this.pnlMemoryBarOuter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlMemoryBarOuter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(24)))), ((int)(((byte)(37)))));
            this.pnlMemoryBarOuter.Controls.Add(this.pnlMemoryBarInner);
            this.pnlMemoryBarOuter.Controls.Add(this.lblBarPercent);
            this.pnlMemoryBarOuter.Location = new System.Drawing.Point(12, 30);
            this.pnlMemoryBarOuter.Name = "pnlMemoryBarOuter";
            this.pnlMemoryBarOuter.Padding = new System.Windows.Forms.Padding(3);
            this.pnlMemoryBarOuter.Size = new System.Drawing.Size(1061, 38);
            this.pnlMemoryBarOuter.TabIndex = 1;
            // 
            // pnlMemoryBarInner
            // 
            this.pnlMemoryBarInner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
            this.pnlMemoryBarInner.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlMemoryBarInner.Location = new System.Drawing.Point(3, 3);
            this.pnlMemoryBarInner.Name = "pnlMemoryBarInner";
            this.pnlMemoryBarInner.Size = new System.Drawing.Size(0, 32);
            this.pnlMemoryBarInner.TabIndex = 0;
            // 
            // lblBarPercent
            // 
            this.lblBarPercent.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblBarPercent.BackColor = System.Drawing.Color.Transparent;
            this.lblBarPercent.Font = new System.Drawing.Font("Consolas", 11F, System.Drawing.FontStyle.Bold);
            this.lblBarPercent.ForeColor = System.Drawing.Color.White;
            this.lblBarPercent.Location = new System.Drawing.Point(455, 7);
            this.lblBarPercent.Name = "lblBarPercent";
            this.lblBarPercent.Size = new System.Drawing.Size(150, 22);
            this.lblBarPercent.TabIndex = 1;
            this.lblBarPercent.Text = "0%";
            this.lblBarPercent.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // segmentsPanel
            // 
            this.segmentsPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(46)))));
            this.segmentsPanel.Controls.Add(this.lblSegmentsTitle);
            this.segmentsPanel.Controls.Add(this.pnlSegmentsContainer);
            this.segmentsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.segmentsPanel.Location = new System.Drawing.Point(3, 148);
            this.segmentsPanel.Name = "segmentsPanel";
            this.segmentsPanel.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.segmentsPanel.Size = new System.Drawing.Size(1085, 124);
            this.segmentsPanel.TabIndex = 2;
            // 
            // lblSegmentsTitle
            // 
            this.lblSegmentsTitle.AutoSize = true;
            this.lblSegmentsTitle.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold);
            this.lblSegmentsTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(173)))), ((int)(((byte)(186)))));
            this.lblSegmentsTitle.Location = new System.Drawing.Point(12, 8);
            this.lblSegmentsTitle.Name = "lblSegmentsTitle";
            this.lblSegmentsTitle.Size = new System.Drawing.Size(203, 14);
            this.lblSegmentsTitle.TabIndex = 0;
            this.lblSegmentsTitle.Text = "MEMORY SEGMENTS (BY PROCESS)";
            // 
            // pnlSegmentsContainer
            // 
            this.pnlSegmentsContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlSegmentsContainer.AutoScroll = true;
            this.pnlSegmentsContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(24)))), ((int)(((byte)(37)))));
            this.pnlSegmentsContainer.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.pnlSegmentsContainer.Location = new System.Drawing.Point(12, 28);
            this.pnlSegmentsContainer.Name = "pnlSegmentsContainer";
            this.pnlSegmentsContainer.Padding = new System.Windows.Forms.Padding(4);
            this.pnlSegmentsContainer.Size = new System.Drawing.Size(1061, 104);
            this.pnlSegmentsContainer.TabIndex = 1;
            this.pnlSegmentsContainer.WrapContents = false;
            // 
            // processMemoryPanel
            // 
            this.processMemoryPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(46)))));
            this.processMemoryPanel.Controls.Add(this.lblProcessMemoryTitle);
            this.processMemoryPanel.Controls.Add(this.dgvProcessMemory);
            this.processMemoryPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.processMemoryPanel.Location = new System.Drawing.Point(3, 278);
            this.processMemoryPanel.Name = "processMemoryPanel";
            this.processMemoryPanel.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.processMemoryPanel.Size = new System.Drawing.Size(1085, 326);
            this.processMemoryPanel.TabIndex = 3;
            // 
            // lblProcessMemoryTitle
            // 
            this.lblProcessMemoryTitle.AutoSize = true;
            this.lblProcessMemoryTitle.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold);
            this.lblProcessMemoryTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(173)))), ((int)(((byte)(186)))));
            this.lblProcessMemoryTitle.Location = new System.Drawing.Point(12, 8);
            this.lblProcessMemoryTitle.Name = "lblProcessMemoryTitle";
            this.lblProcessMemoryTitle.Size = new System.Drawing.Size(182, 14);
            this.lblProcessMemoryTitle.TabIndex = 0;
            this.lblProcessMemoryTitle.Text = "PROCESS MEMORY ALLOCATION";
            // 
            // dgvProcessMemory
            // 
            this.dgvProcessMemory.AllowUserToAddRows = false;
            this.dgvProcessMemory.AllowUserToDeleteRows = false;
            this.dgvProcessMemory.AllowUserToResizeRows = false;
            this.dgvProcessMemory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvProcessMemory.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvProcessMemory.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(24)))), ((int)(((byte)(37)))));
            this.dgvProcessMemory.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvProcessMemory.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvProcessMemory.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvProcessMemory.ColumnHeadersHeight = 32;
            this.dgvProcessMemory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvProcessMemory.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colPID,
            this.colProcessName,
            this.colMemoryMB,
            this.colMemoryPercent});
            this.dgvProcessMemory.EnableHeadersVisualStyles = false;
            this.dgvProcessMemory.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(75)))));
            this.dgvProcessMemory.Location = new System.Drawing.Point(12, 28);
            this.dgvProcessMemory.MultiSelect = false;
            this.dgvProcessMemory.Name = "dgvProcessMemory";
            this.dgvProcessMemory.ReadOnly = true;
            this.dgvProcessMemory.RowHeadersVisible = false;
            this.dgvProcessMemory.RowTemplate.Height = 26;
            this.dgvProcessMemory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProcessMemory.Size = new System.Drawing.Size(1061, 290);
            this.dgvProcessMemory.TabIndex = 1;
            // 
            // colPID
            // 
            this.colPID.FillWeight = 60F;
            this.colPID.HeaderText = "PID";
            this.colPID.Name = "colPID";
            this.colPID.ReadOnly = true;
            // 
            // colProcessName
            // 
            this.colProcessName.FillWeight = 150F;
            this.colProcessName.HeaderText = "PROCESS NAME";
            this.colProcessName.Name = "colProcessName";
            this.colProcessName.ReadOnly = true;
            // 
            // colMemoryMB
            // 
            this.colMemoryMB.FillWeight = 100F;
            this.colMemoryMB.HeaderText = "MEMORY (MB)";
            this.colMemoryMB.Name = "colMemoryMB";
            this.colMemoryMB.ReadOnly = true;
            // 
            // colMemoryPercent
            // 
            this.colMemoryPercent.FillWeight = 100F;
            this.colMemoryPercent.HeaderText = "% OF TOTAL";
            this.colMemoryPercent.Name = "colMemoryPercent";
            this.colMemoryPercent.ReadOnly = true;
            // 
            // statusPanel
            // 
            this.statusPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(46)))));
            this.statusPanel.Controls.Add(this.flowLayoutStatus);
            this.statusPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusPanel.Location = new System.Drawing.Point(3, 610);
            this.statusPanel.Name = "statusPanel";
            this.statusPanel.Padding = new System.Windows.Forms.Padding(12, 4, 12, 4);
            this.statusPanel.Size = new System.Drawing.Size(1085, 32);
            this.statusPanel.TabIndex = 4;
            // 
            // flowLayoutStatus
            // 
            this.flowLayoutStatus.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutStatus.Controls.Add(this.lblStatus);
            this.flowLayoutStatus.Controls.Add(this.lblLastUpdate);
            this.flowLayoutStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutStatus.Location = new System.Drawing.Point(12, 4);
            this.flowLayoutStatus.Name = "flowLayoutStatus";
            this.flowLayoutStatus.Size = new System.Drawing.Size(1061, 24);
            this.flowLayoutStatus.TabIndex = 0;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Consolas", 9F);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(197)))), ((int)(((byte)(94)))));
            this.lblStatus.Location = new System.Drawing.Point(0, 0);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(0, 0, 40, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.lblStatus.Size = new System.Drawing.Size(154, 18);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "Memory Manager Active";
            // 
            // lblLastUpdate
            // 
            this.lblLastUpdate.AutoSize = true;
            this.lblLastUpdate.Font = new System.Drawing.Font("Consolas", 9F);
            this.lblLastUpdate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(173)))), ((int)(((byte)(186)))));
            this.lblLastUpdate.Location = new System.Drawing.Point(194, 0);
            this.lblLastUpdate.Margin = new System.Windows.Forms.Padding(0);
            this.lblLastUpdate.Name = "lblLastUpdate";
            this.lblLastUpdate.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.lblLastUpdate.Size = new System.Drawing.Size(154, 18);
            this.lblLastUpdate.TabIndex = 1;
            this.lblLastUpdate.Text = "Last Update: --:--:--";
            // 
            // refreshTimer
            // 
            this.refreshTimer.Interval = 2000;
            this.refreshTimer.Tick += new System.EventHandler(this.refreshTimer_Tick);
            // 
            // MemoryVisControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(51)))));
            this.Controls.Add(this.mainLayout);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "MemoryVisControl";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Size = new System.Drawing.Size(1111, 665);
            this.Load += new System.EventHandler(this.MemoryVisControl_Load);
            this.mainLayout.ResumeLayout(false);
            this.headerPanel.ResumeLayout(false);
            this.flowLayoutHeader.ResumeLayout(false);
            this.flowLayoutHeader.PerformLayout();
            this.memoryBarPanel.ResumeLayout(false);
            this.memoryBarPanel.PerformLayout();
            this.pnlMemoryBarOuter.ResumeLayout(false);
            this.segmentsPanel.ResumeLayout(false);
            this.segmentsPanel.PerformLayout();
            this.processMemoryPanel.ResumeLayout(false);
            this.processMemoryPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProcessMemory)).EndInit();
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
        private System.Windows.Forms.Label lblTotalMemory;
        private System.Windows.Forms.Label lblUsedMemory;
        private System.Windows.Forms.Label lblFreeMemory;
        private System.Windows.Forms.Label lblUsagePercent;
        private Guna.UI2.WinForms.Guna2Button btnRefreshMemory;
        private System.Windows.Forms.Panel memoryBarPanel;
        private System.Windows.Forms.Label lblMemoryBarTitle;
        private System.Windows.Forms.Panel pnlMemoryBarOuter;
        private System.Windows.Forms.Panel pnlMemoryBarInner;
        private System.Windows.Forms.Label lblBarPercent;
        private System.Windows.Forms.Panel segmentsPanel;
        private System.Windows.Forms.Label lblSegmentsTitle;
        private System.Windows.Forms.FlowLayoutPanel pnlSegmentsContainer;
        private System.Windows.Forms.Panel processMemoryPanel;
        private System.Windows.Forms.Label lblProcessMemoryTitle;
        private System.Windows.Forms.DataGridView dgvProcessMemory;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProcessName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMemoryMB;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMemoryPercent;
        private System.Windows.Forms.Panel statusPanel;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutStatus;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblLastUpdate;
        private System.Windows.Forms.Timer refreshTimer;
    }
}
