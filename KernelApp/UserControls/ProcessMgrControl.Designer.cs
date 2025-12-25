namespace KernelApp.UserControls
{
    partial class ProcessMgrControl
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
            this.actionPanel = new System.Windows.Forms.Panel();
            this.flowLayoutActions = new System.Windows.Forms.FlowLayoutPanel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.txtProcessName = new Guna.UI2.WinForms.Guna2TextBox();
            this.btnCreate = new Guna.UI2.WinForms.Guna2Button();
            this.btnKill = new Guna.UI2.WinForms.Guna2Button();
            this.btnPauseResume = new Guna.UI2.WinForms.Guna2Button();
            this.btnRefresh = new Guna.UI2.WinForms.Guna2Button();
            this.listPanel = new System.Windows.Forms.Panel();
            this.dgvProcesses = new System.Windows.Forms.DataGridView();
            this.colPID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProcessName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMemoryMB = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPriority = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStartTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusPanel = new System.Windows.Forms.Panel();
            this.flowLayoutStatus = new System.Windows.Forms.FlowLayoutPanel();
            this.lblTotalProcesses = new System.Windows.Forms.Label();
            this.lblRunningCount = new System.Windows.Forms.Label();
            this.lblTotalMemory = new System.Windows.Forms.Label();
            this.schedulerTimer = new System.Windows.Forms.Timer(this.components);
            this.mainLayout.SuspendLayout();
            this.actionPanel.SuspendLayout();
            this.flowLayoutActions.SuspendLayout();
            this.listPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProcesses)).BeginInit();
            this.statusPanel.SuspendLayout();
            this.flowLayoutStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainLayout
            // 
            this.mainLayout.BackColor = System.Drawing.Color.Transparent;
            this.mainLayout.ColumnCount = 1;
            this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayout.Controls.Add(this.actionPanel, 0, 0);
            this.mainLayout.Controls.Add(this.listPanel, 0, 1);
            this.mainLayout.Controls.Add(this.statusPanel, 0, 2);
            this.mainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayout.Location = new System.Drawing.Point(10, 10);
            this.mainLayout.Name = "mainLayout";
            this.mainLayout.RowCount = 3;
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.mainLayout.Size = new System.Drawing.Size(1094, 647);
            this.mainLayout.TabIndex = 0;
            // 
            // actionPanel
            // 
            this.actionPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(46)))));
            this.actionPanel.Controls.Add(this.flowLayoutActions);
            this.actionPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.actionPanel.Location = new System.Drawing.Point(3, 3);
            this.actionPanel.Name = "actionPanel";
            this.actionPanel.Padding = new System.Windows.Forms.Padding(10);
            this.actionPanel.Size = new System.Drawing.Size(1088, 64);
            this.actionPanel.TabIndex = 0;
            // 
            // flowLayoutActions
            // 
            this.flowLayoutActions.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutActions.Controls.Add(this.lblTitle);
            this.flowLayoutActions.Controls.Add(this.txtProcessName);
            this.flowLayoutActions.Controls.Add(this.btnCreate);
            this.flowLayoutActions.Controls.Add(this.btnKill);
            this.flowLayoutActions.Controls.Add(this.btnPauseResume);
            this.flowLayoutActions.Controls.Add(this.btnRefresh);
            this.flowLayoutActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutActions.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.flowLayoutActions.Location = new System.Drawing.Point(10, 10);
            this.flowLayoutActions.Name = "flowLayoutActions";
            this.flowLayoutActions.Size = new System.Drawing.Size(1068, 44);
            this.flowLayoutActions.TabIndex = 0;
            this.flowLayoutActions.WrapContents = false;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.lblTitle.Size = new System.Drawing.Size(180, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "PROCESS MANAGER";
            // 
            // txtProcessName
            // 
            this.txtProcessName.BackColor = System.Drawing.Color.Transparent;
            this.txtProcessName.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(75)))));
            this.txtProcessName.BorderRadius = 6;
            this.txtProcessName.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtProcessName.DefaultText = "";
            this.txtProcessName.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtProcessName.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtProcessName.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtProcessName.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtProcessName.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(24)))), ((int)(((byte)(37)))));
            this.txtProcessName.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
            this.txtProcessName.Font = new System.Drawing.Font("Consolas", 10F);
            this.txtProcessName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(173)))), ((int)(((byte)(186)))));
            this.txtProcessName.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
            this.txtProcessName.Location = new System.Drawing.Point(203, 3);
            this.txtProcessName.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            this.txtProcessName.Name = "txtProcessName";
            this.txtProcessName.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(120)))));
            this.txtProcessName.PlaceholderText = "Enter process name (e.g., myapp.exe)";
            this.txtProcessName.SelectedText = "";
            this.txtProcessName.Size = new System.Drawing.Size(400, 38);
            this.txtProcessName.TabIndex = 1;
            // 
            // btnCreate
            // 
            this.btnCreate.Animated = true;
            this.btnCreate.BackColor = System.Drawing.Color.Transparent;
            this.btnCreate.BorderRadius = 6;
            this.btnCreate.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnCreate.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnCreate.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnCreate.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnCreate.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(197)))), ((int)(((byte)(94)))));
            this.btnCreate.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnCreate.ForeColor = System.Drawing.Color.White;
            this.btnCreate.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.btnCreate.Location = new System.Drawing.Point(466, 3);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(120, 38);
            this.btnCreate.TabIndex = 2;
            this.btnCreate.Text = "+ Create";
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // btnKill
            // 
            this.btnKill.Animated = true;
            this.btnKill.BackColor = System.Drawing.Color.Transparent;
            this.btnKill.BorderRadius = 6;
            this.btnKill.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnKill.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnKill.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnKill.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnKill.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnKill.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnKill.ForeColor = System.Drawing.Color.White;
            this.btnKill.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.btnKill.Location = new System.Drawing.Point(592, 3);
            this.btnKill.Name = "btnKill";
            this.btnKill.Size = new System.Drawing.Size(100, 38);
            this.btnKill.TabIndex = 3;
            this.btnKill.Text = "X Kill";
            this.btnKill.Click += new System.EventHandler(this.btnKill_Click);
            // 
            // btnPauseResume
            // 
            this.btnPauseResume.Animated = true;
            this.btnPauseResume.BackColor = System.Drawing.Color.Transparent;
            this.btnPauseResume.BorderRadius = 6;
            this.btnPauseResume.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnPauseResume.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnPauseResume.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnPauseResume.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnPauseResume.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(191)))), ((int)(((byte)(36)))));
            this.btnPauseResume.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnPauseResume.ForeColor = System.Drawing.Color.White;
            this.btnPauseResume.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(158)))), ((int)(((byte)(11)))));
            this.btnPauseResume.Location = new System.Drawing.Point(698, 3);
            this.btnPauseResume.Name = "btnPauseResume";
            this.btnPauseResume.Size = new System.Drawing.Size(100, 38);
            this.btnPauseResume.TabIndex = 4;
            this.btnPauseResume.Text = "|| Pause";
            this.btnPauseResume.Click += new System.EventHandler(this.btnPauseResume_Click);
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
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(70)))), ((int)(((byte)(229)))));
            this.btnRefresh.Location = new System.Drawing.Point(804, 3);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 38);
            this.btnRefresh.TabIndex = 5;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // listPanel
            // 
            this.listPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(46)))));
            this.listPanel.Controls.Add(this.dgvProcesses);
            this.listPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listPanel.Location = new System.Drawing.Point(3, 73);
            this.listPanel.Name = "listPanel";
            this.listPanel.Padding = new System.Windows.Forms.Padding(10);
            this.listPanel.Size = new System.Drawing.Size(1088, 521);
            this.listPanel.TabIndex = 1;
            // 
            // dgvProcesses
            // 
            this.dgvProcesses.AllowUserToAddRows = false;
            this.dgvProcesses.AllowUserToDeleteRows = false;
            this.dgvProcesses.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(46)))));
            this.dgvProcesses.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvProcesses.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvProcesses.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(24)))), ((int)(((byte)(37)))));
            this.dgvProcesses.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvProcesses.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvProcesses.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(65)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Consolas", 11F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(65)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvProcesses.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvProcesses.ColumnHeadersHeight = 40;
            this.dgvProcesses.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvProcesses.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colPID,
            this.colProcessName,
            this.colStatus,
            this.colMemoryMB,
            this.colPriority,
            this.colStartTime});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(24)))), ((int)(((byte)(37)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Consolas", 10F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(173)))), ((int)(((byte)(186)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(85)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvProcesses.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvProcesses.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProcesses.EnableHeadersVisualStyles = false;
            this.dgvProcesses.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(75)))));
            this.dgvProcesses.Location = new System.Drawing.Point(10, 10);
            this.dgvProcesses.MultiSelect = false;
            this.dgvProcesses.Name = "dgvProcesses";
            this.dgvProcesses.ReadOnly = true;
            this.dgvProcesses.RowHeadersVisible = false;
            this.dgvProcesses.RowTemplate.Height = 35;
            this.dgvProcesses.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProcesses.Size = new System.Drawing.Size(1068, 501);
            this.dgvProcesses.TabIndex = 0;
            this.dgvProcesses.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvProcesses_CellFormatting);
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
            // colStatus
            // 
            this.colStatus.FillWeight = 80F;
            this.colStatus.HeaderText = "STATUS";
            this.colStatus.Name = "colStatus";
            this.colStatus.ReadOnly = true;
            // 
            // colMemoryMB
            // 
            this.colMemoryMB.FillWeight = 90F;
            this.colMemoryMB.HeaderText = "MEMORY (MB)";
            this.colMemoryMB.Name = "colMemoryMB";
            this.colMemoryMB.ReadOnly = true;
            // 
            // colPriority
            // 
            this.colPriority.FillWeight = 70F;
            this.colPriority.HeaderText = "PRIORITY";
            this.colPriority.Name = "colPriority";
            this.colPriority.ReadOnly = true;
            // 
            // colStartTime
            // 
            this.colStartTime.FillWeight = 80F;
            this.colStartTime.HeaderText = "START TIME";
            this.colStartTime.Name = "colStartTime";
            this.colStartTime.ReadOnly = true;
            // 
            // statusPanel
            // 
            this.statusPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(46)))));
            this.statusPanel.Controls.Add(this.flowLayoutStatus);
            this.statusPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusPanel.Location = new System.Drawing.Point(3, 600);
            this.statusPanel.Name = "statusPanel";
            this.statusPanel.Padding = new System.Windows.Forms.Padding(15, 5, 15, 5);
            this.statusPanel.Size = new System.Drawing.Size(1088, 44);
            this.statusPanel.TabIndex = 2;
            // 
            // flowLayoutStatus
            // 
            this.flowLayoutStatus.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutStatus.Controls.Add(this.lblTotalProcesses);
            this.flowLayoutStatus.Controls.Add(this.lblRunningCount);
            this.flowLayoutStatus.Controls.Add(this.lblTotalMemory);
            this.flowLayoutStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutStatus.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.flowLayoutStatus.Location = new System.Drawing.Point(15, 5);
            this.flowLayoutStatus.Name = "flowLayoutStatus";
            this.flowLayoutStatus.Size = new System.Drawing.Size(1058, 34);
            this.flowLayoutStatus.TabIndex = 0;
            // 
            // lblTotalProcesses
            // 
            this.lblTotalProcesses.AutoSize = true;
            this.lblTotalProcesses.Font = new System.Drawing.Font("Consolas", 11F);
            this.lblTotalProcesses.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(173)))), ((int)(((byte)(186)))));
            this.lblTotalProcesses.Location = new System.Drawing.Point(0, 0);
            this.lblTotalProcesses.Margin = new System.Windows.Forms.Padding(0, 0, 50, 0);
            this.lblTotalProcesses.Name = "lblTotalProcesses";
            this.lblTotalProcesses.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.lblTotalProcesses.Size = new System.Drawing.Size(152, 26);
            this.lblTotalProcesses.TabIndex = 0;
            this.lblTotalProcesses.Text = "Total Processes: 0";
            // 
            // lblRunningCount
            // 
            this.lblRunningCount.AutoSize = true;
            this.lblRunningCount.Font = new System.Drawing.Font("Consolas", 11F);
            this.lblRunningCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(173)))), ((int)(((byte)(186)))));
            this.lblRunningCount.Location = new System.Drawing.Point(202, 0);
            this.lblRunningCount.Margin = new System.Windows.Forms.Padding(0, 0, 50, 0);
            this.lblRunningCount.Name = "lblRunningCount";
            this.lblRunningCount.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.lblRunningCount.Size = new System.Drawing.Size(88, 26);
            this.lblRunningCount.TabIndex = 1;
            this.lblRunningCount.Text = "Running: 0";
            // 
            // lblTotalMemory
            // 
            this.lblTotalMemory.AutoSize = true;
            this.lblTotalMemory.Font = new System.Drawing.Font("Consolas", 11F);
            this.lblTotalMemory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(173)))), ((int)(((byte)(186)))));
            this.lblTotalMemory.Location = new System.Drawing.Point(340, 0);
            this.lblTotalMemory.Margin = new System.Windows.Forms.Padding(0, 0, 50, 0);
            this.lblTotalMemory.Name = "lblTotalMemory";
            this.lblTotalMemory.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.lblTotalMemory.Size = new System.Drawing.Size(144, 26);
            this.lblTotalMemory.TabIndex = 2;
            this.lblTotalMemory.Text = "Total Memory: 0 MB";
            // 
            // schedulerTimer
            // 
            this.schedulerTimer.Interval = 1500;
            this.schedulerTimer.Tick += new System.EventHandler(this.schedulerTimer_Tick);
            // 
            // ProcessMgrControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(51)))));
            this.Controls.Add(this.mainLayout);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "ProcessMgrControl";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Size = new System.Drawing.Size(1114, 667);
            this.Load += new System.EventHandler(this.ProcessMgrControl_Load);
            this.mainLayout.ResumeLayout(false);
            this.actionPanel.ResumeLayout(false);
            this.flowLayoutActions.ResumeLayout(false);
            this.flowLayoutActions.PerformLayout();
            this.listPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProcesses)).EndInit();
            this.statusPanel.ResumeLayout(false);
            this.flowLayoutStatus.ResumeLayout(false);
            this.flowLayoutStatus.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel mainLayout;
        private System.Windows.Forms.Panel actionPanel;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutActions;
        private System.Windows.Forms.Label lblTitle;
        private Guna.UI2.WinForms.Guna2TextBox txtProcessName;
        private Guna.UI2.WinForms.Guna2Button btnCreate;
        private Guna.UI2.WinForms.Guna2Button btnKill;
        private Guna.UI2.WinForms.Guna2Button btnPauseResume;
        private Guna.UI2.WinForms.Guna2Button btnRefresh;
        private System.Windows.Forms.Panel listPanel;
        private System.Windows.Forms.DataGridView dgvProcesses;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProcessName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMemoryMB;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPriority;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStartTime;
        private System.Windows.Forms.Panel statusPanel;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutStatus;
        private System.Windows.Forms.Label lblTotalProcesses;
        private System.Windows.Forms.Label lblRunningCount;
        private System.Windows.Forms.Label lblTotalMemory;
        private System.Windows.Forms.Timer schedulerTimer;
    }
}
