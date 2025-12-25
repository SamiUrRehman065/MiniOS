using KernelApp.Properties;
using System.Drawing;
using System.Windows.Forms;

namespace KernelApp
{
    partial class MiniOs
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
            this.components = new System.ComponentModel.Container();
            this.MenuPanel = new Guna.UI2.WinForms.Guna2Panel();
            this.pnlActiveIndicator = new Guna.UI2.WinForms.Guna2Panel();
            this.btnSyscallLogs = new Guna.UI2.WinForms.Guna2Button();
            this.btnShutdown = new Guna.UI2.WinForms.Guna2Button();
            this.btnMemoryUsage = new Guna.UI2.WinForms.Guna2Button();
            this.btnSystemConsole = new Guna.UI2.WinForms.Guna2Button();
            this.btnProcessManager = new Guna.UI2.WinForms.Guna2Button();
            this.lblAppTitle = new System.Windows.Forms.Label();
            this.guna2CirclePictureBox1 = new Guna.UI2.WinForms.Guna2CirclePictureBox();
            this.headerpanel = new Guna.UI2.WinForms.Guna2Panel();
            this.pnlWindowControls = new System.Windows.Forms.Panel();
            this.btnClose = new Guna.UI2.WinForms.Guna2CircleButton();
            this.btnMaximize = new Guna.UI2.WinForms.Guna2CircleButton();
            this.btnMinimize = new Guna.UI2.WinForms.Guna2CircleButton();
            this.lblCurrentView = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lbldate = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblTime = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.MainPanel = new Guna.UI2.WinForms.Guna2Panel();
            this.taskbarPanel = new Guna.UI2.WinForms.Guna2Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.pnlSystemTray = new System.Windows.Forms.Panel();
            this.lblCpu = new System.Windows.Forms.Label();
            this.lblMemory = new System.Windows.Forms.Label();
            this.clockTimer = new System.Windows.Forms.Timer(this.components);
            this.MenuPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.guna2CirclePictureBox1)).BeginInit();
            this.headerpanel.SuspendLayout();
            this.pnlWindowControls.SuspendLayout();
            this.taskbarPanel.SuspendLayout();
            this.pnlSystemTray.SuspendLayout();
            this.SuspendLayout();
            // 
            // MenuPanel
            // 
            this.MenuPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.MenuPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(46)))));
            this.MenuPanel.Controls.Add(this.pnlActiveIndicator);
            this.MenuPanel.Controls.Add(this.btnSyscallLogs);
            this.MenuPanel.Controls.Add(this.btnShutdown);
            this.MenuPanel.Controls.Add(this.btnMemoryUsage);
            this.MenuPanel.Controls.Add(this.btnSystemConsole);
            this.MenuPanel.Controls.Add(this.btnProcessManager);
            this.MenuPanel.Controls.Add(this.lblAppTitle);
            this.MenuPanel.Controls.Add(this.guna2CirclePictureBox1);
            this.MenuPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.MenuPanel.Location = new System.Drawing.Point(0, 0);
            this.MenuPanel.Name = "MenuPanel";
            this.MenuPanel.Size = new System.Drawing.Size(273, 736);
            this.MenuPanel.TabIndex = 1;
            // 
            // pnlActiveIndicator
            // 
            this.pnlActiveIndicator.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
            this.pnlActiveIndicator.Location = new System.Drawing.Point(0, 241);
            this.pnlActiveIndicator.Name = "pnlActiveIndicator";
            this.pnlActiveIndicator.Size = new System.Drawing.Size(4, 48);
            this.pnlActiveIndicator.TabIndex = 12;
            // 
            // btnSyscallLogs
            // 
            this.btnSyscallLogs.Animated = true;
            this.btnSyscallLogs.BackColor = System.Drawing.Color.Transparent;
            this.btnSyscallLogs.BorderRadius = 8;
            this.btnSyscallLogs.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnSyscallLogs.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnSyscallLogs.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnSyscallLogs.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnSyscallLogs.FillColor = System.Drawing.Color.Transparent;
            this.btnSyscallLogs.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.btnSyscallLogs.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(173)))), ((int)(((byte)(186)))));
            this.btnSyscallLogs.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(65)))));
            this.btnSyscallLogs.HoverState.ForeColor = System.Drawing.Color.White;
            this.btnSyscallLogs.Image = null;
            this.btnSyscallLogs.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnSyscallLogs.Location = new System.Drawing.Point(12, 403);
            this.btnSyscallLogs.Name = "btnSyscallLogs";
            this.btnSyscallLogs.Size = new System.Drawing.Size(249, 48);
            this.btnSyscallLogs.TabIndex = 10;
            this.btnSyscallLogs.Text = "  Syscall Logs";
            this.btnSyscallLogs.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnSyscallLogs.Click += new System.EventHandler(this.btnSyscallLogs_Click);
            // 
            // btnShutdown
            // 
            this.btnShutdown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnShutdown.Animated = true;
            this.btnShutdown.BackColor = System.Drawing.Color.Transparent;
            this.btnShutdown.BorderRadius = 8;
            this.btnShutdown.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnShutdown.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnShutdown.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnShutdown.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnShutdown.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnShutdown.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.btnShutdown.ForeColor = System.Drawing.Color.White;
            this.btnShutdown.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(35)))), ((int)(((byte)(51)))));
            this.btnShutdown.Location = new System.Drawing.Point(12, 676);
            this.btnShutdown.Name = "btnShutdown";
            this.btnShutdown.Size = new System.Drawing.Size(249, 48);
            this.btnShutdown.TabIndex = 6;
            this.btnShutdown.Text = "  Shutdown";
            this.btnShutdown.Click += new System.EventHandler(this.btnShutdown_Click);
            // 
            // btnMemoryUsage
            // 
            this.btnMemoryUsage.Animated = true;
            this.btnMemoryUsage.BackColor = System.Drawing.Color.Transparent;
            this.btnMemoryUsage.BorderRadius = 8;
            this.btnMemoryUsage.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnMemoryUsage.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnMemoryUsage.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnMemoryUsage.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnMemoryUsage.FillColor = System.Drawing.Color.Transparent;
            this.btnMemoryUsage.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.btnMemoryUsage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(173)))), ((int)(((byte)(186)))));
            this.btnMemoryUsage.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(65)))));
            this.btnMemoryUsage.HoverState.ForeColor = System.Drawing.Color.White;
            this.btnMemoryUsage.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnMemoryUsage.Location = new System.Drawing.Point(12, 349);
            this.btnMemoryUsage.Name = "btnMemoryUsage";
            this.btnMemoryUsage.Size = new System.Drawing.Size(249, 48);
            this.btnMemoryUsage.TabIndex = 5;
            this.btnMemoryUsage.Text = "  Memory Usage";
            this.btnMemoryUsage.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnMemoryUsage.Click += new System.EventHandler(this.btnMemoryUsage_Click);
            // 
            // btnSystemConsole
            // 
            this.btnSystemConsole.Animated = true;
            this.btnSystemConsole.BackColor = System.Drawing.Color.Transparent;
            this.btnSystemConsole.BorderRadius = 8;
            this.btnSystemConsole.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnSystemConsole.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnSystemConsole.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnSystemConsole.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnSystemConsole.FillColor = System.Drawing.Color.Transparent;
            this.btnSystemConsole.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.btnSystemConsole.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(173)))), ((int)(((byte)(186)))));
            this.btnSystemConsole.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(65)))));
            this.btnSystemConsole.HoverState.ForeColor = System.Drawing.Color.White;
            this.btnSystemConsole.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnSystemConsole.Location = new System.Drawing.Point(12, 295);
            this.btnSystemConsole.Name = "btnSystemConsole";
            this.btnSystemConsole.Size = new System.Drawing.Size(249, 48);
            this.btnSystemConsole.TabIndex = 4;
            this.btnSystemConsole.Text = "  Console";
            this.btnSystemConsole.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnSystemConsole.Click += new System.EventHandler(this.btnSystemConsole_Click);
            // 
            // btnProcessManager
            // 
            this.btnProcessManager.Animated = true;
            this.btnProcessManager.BackColor = System.Drawing.Color.Transparent;
            this.btnProcessManager.BorderRadius = 8;
            this.btnProcessManager.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnProcessManager.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnProcessManager.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnProcessManager.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnProcessManager.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(65)))));
            this.btnProcessManager.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.btnProcessManager.ForeColor = System.Drawing.Color.White;
            this.btnProcessManager.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(75)))));
            this.btnProcessManager.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnProcessManager.Location = new System.Drawing.Point(12, 241);
            this.btnProcessManager.Name = "btnProcessManager";
            this.btnProcessManager.Size = new System.Drawing.Size(249, 48);
            this.btnProcessManager.TabIndex = 3;
            this.btnProcessManager.Text = "  Task Manager";
            this.btnProcessManager.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnProcessManager.Click += new System.EventHandler(this.btnProcessManager_Click);
            // 
            // lblAppTitle
            // 
            this.lblAppTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblAppTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblAppTitle.ForeColor = System.Drawing.Color.White;
            this.lblAppTitle.Location = new System.Drawing.Point(3, 180);
            this.lblAppTitle.Name = "lblAppTitle";
            this.lblAppTitle.Size = new System.Drawing.Size(267, 40);
            this.lblAppTitle.TabIndex = 11;
            this.lblAppTitle.Text = "MiniOS";
            this.lblAppTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // guna2CirclePictureBox1
            // 
            this.guna2CirclePictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.guna2CirclePictureBox1.FillColor = System.Drawing.Color.Transparent;
            this.guna2CirclePictureBox1.Image = global::KernelApp.Properties.Resources.system;
            this.guna2CirclePictureBox1.ImageRotate = 0F;
            this.guna2CirclePictureBox1.Location = new System.Drawing.Point(61, 20);
            this.guna2CirclePictureBox1.Name = "guna2CirclePictureBox1";
            this.guna2CirclePictureBox1.ShadowDecoration.Color = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
            this.guna2CirclePictureBox1.ShadowDecoration.Enabled = true;
            this.guna2CirclePictureBox1.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.guna2CirclePictureBox1.ShadowDecoration.Depth = 20;
            this.guna2CirclePictureBox1.Size = new System.Drawing.Size(150, 150);
            this.guna2CirclePictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.guna2CirclePictureBox1.TabIndex = 2;
            this.guna2CirclePictureBox1.TabStop = false;
            // 
            // headerpanel
            // 
            this.headerpanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(24)))), ((int)(((byte)(37)))));
            this.headerpanel.Controls.Add(this.pnlWindowControls);
            this.headerpanel.Controls.Add(this.lblCurrentView);
            this.headerpanel.Controls.Add(this.lbldate);
            this.headerpanel.Controls.Add(this.lblTime);
            this.headerpanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerpanel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.headerpanel.Location = new System.Drawing.Point(273, 0);
            this.headerpanel.Name = "headerpanel";
            this.headerpanel.Size = new System.Drawing.Size(1115, 50);
            this.headerpanel.TabIndex = 2;
            this.headerpanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.headerpanel_MouseDown);
            // 
            // pnlWindowControls
            // 
            this.pnlWindowControls.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlWindowControls.BackColor = System.Drawing.Color.Transparent;
            this.pnlWindowControls.Controls.Add(this.btnClose);
            this.pnlWindowControls.Controls.Add(this.btnMaximize);
            this.pnlWindowControls.Controls.Add(this.btnMinimize);
            this.pnlWindowControls.Location = new System.Drawing.Point(1005, 10);
            this.pnlWindowControls.Name = "pnlWindowControls";
            this.pnlWindowControls.Size = new System.Drawing.Size(100, 30);
            this.pnlWindowControls.TabIndex = 4;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnClose.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnClose.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnClose.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnClose.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(95)))), ((int)(((byte)(86)))));
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(59)))), ((int)(((byte)(48)))));
            this.btnClose.Location = new System.Drawing.Point(70, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.btnClose.Size = new System.Drawing.Size(24, 24);
            this.btnClose.TabIndex = 2;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnMaximize
            // 
            this.btnMaximize.BackColor = System.Drawing.Color.Transparent;
            this.btnMaximize.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnMaximize.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnMaximize.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnMaximize.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnMaximize.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(201)))), ((int)(((byte)(63)))));
            this.btnMaximize.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnMaximize.ForeColor = System.Drawing.Color.White;
            this.btnMaximize.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(180)))), ((int)(((byte)(50)))));
            this.btnMaximize.Location = new System.Drawing.Point(38, 3);
            this.btnMaximize.Name = "btnMaximize";
            this.btnMaximize.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.btnMaximize.Size = new System.Drawing.Size(24, 24);
            this.btnMaximize.TabIndex = 1;
            this.btnMaximize.Click += new System.EventHandler(this.btnMaximize_Click);
            // 
            // btnMinimize
            // 
            this.btnMinimize.BackColor = System.Drawing.Color.Transparent;
            this.btnMinimize.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnMinimize.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnMinimize.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnMinimize.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnMinimize.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(189)))), ((int)(((byte)(46)))));
            this.btnMinimize.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnMinimize.ForeColor = System.Drawing.Color.White;
            this.btnMinimize.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(170)))), ((int)(((byte)(30)))));
            this.btnMinimize.Location = new System.Drawing.Point(6, 3);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.btnMinimize.Size = new System.Drawing.Size(24, 24);
            this.btnMinimize.TabIndex = 0;
            this.btnMinimize.Click += new System.EventHandler(this.btnMinimize_Click);
            // 
            // lblCurrentView
            // 
            this.lblCurrentView.BackColor = System.Drawing.Color.Transparent;
            this.lblCurrentView.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold);
            this.lblCurrentView.ForeColor = System.Drawing.Color.White;
            this.lblCurrentView.Location = new System.Drawing.Point(20, 10);
            this.lblCurrentView.Name = "lblCurrentView";
            this.lblCurrentView.Size = new System.Drawing.Size(150, 30);
            this.lblCurrentView.TabIndex = 0;
            this.lblCurrentView.Text = "Task Manager";
            // 
            // lbldate
            // 
            this.lbldate.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lbldate.BackColor = System.Drawing.Color.Transparent;
            this.lbldate.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular);
            this.lbldate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(173)))), ((int)(((byte)(186)))));
            this.lbldate.Location = new System.Drawing.Point(450, 14);
            this.lbldate.Name = "lbldate";
            this.lbldate.Size = new System.Drawing.Size(200, 22);
            this.lbldate.TabIndex = 1;
            this.lbldate.Text = "Date";
            this.lbldate.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTime
            // 
            this.lblTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTime.BackColor = System.Drawing.Color.Transparent;
            this.lblTime.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.lblTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
            this.lblTime.Location = new System.Drawing.Point(880, 14);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(100, 22);
            this.lblTime.TabIndex = 3;
            this.lblTime.Text = "00:00:00";
            this.lblTime.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // MainPanel
            // 
            this.MainPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MainPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(51)))));
            this.MainPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(75)))));
            this.MainPanel.BorderRadius = 12;
            this.MainPanel.BorderThickness = 1;
            this.MainPanel.Location = new System.Drawing.Point(283, 60);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Padding = new System.Windows.Forms.Padding(5);
            this.MainPanel.Size = new System.Drawing.Size(1095, 626);
            this.MainPanel.TabIndex = 4;
            // 
            // taskbarPanel
            // 
            this.taskbarPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(24)))), ((int)(((byte)(37)))));
            this.taskbarPanel.Controls.Add(this.lblStatus);
            this.taskbarPanel.Controls.Add(this.pnlSystemTray);
            this.taskbarPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.taskbarPanel.Location = new System.Drawing.Point(273, 700);
            this.taskbarPanel.Name = "taskbarPanel";
            this.taskbarPanel.Size = new System.Drawing.Size(1115, 36);
            this.taskbarPanel.TabIndex = 5;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
            this.lblStatus.Location = new System.Drawing.Point(15, 10);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(95, 15);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "System Ready";
            // 
            // pnlSystemTray
            // 
            this.pnlSystemTray.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlSystemTray.BackColor = System.Drawing.Color.Transparent;
            this.pnlSystemTray.Controls.Add(this.lblCpu);
            this.pnlSystemTray.Controls.Add(this.lblMemory);
            this.pnlSystemTray.Location = new System.Drawing.Point(850, 5);
            this.pnlSystemTray.Name = "pnlSystemTray";
            this.pnlSystemTray.Size = new System.Drawing.Size(255, 26);
            this.pnlSystemTray.TabIndex = 0;
            // 
            // lblCpu
            // 
            this.lblCpu.AutoSize = true;
            this.lblCpu.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblCpu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(173)))), ((int)(((byte)(186)))));
            this.lblCpu.Location = new System.Drawing.Point(10, 5);
            this.lblCpu.Name = "lblCpu";
            this.lblCpu.Size = new System.Drawing.Size(63, 15);
            this.lblCpu.TabIndex = 0;
            this.lblCpu.Text = "CPU: 0%";
            // 
            // lblMemory
            // 
            this.lblMemory.AutoSize = true;
            this.lblMemory.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblMemory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(173)))), ((int)(((byte)(186)))));
            this.lblMemory.Location = new System.Drawing.Point(130, 5);
            this.lblMemory.Name = "lblMemory";
            this.lblMemory.Size = new System.Drawing.Size(80, 15);
            this.lblMemory.TabIndex = 1;
            this.lblMemory.Text = "MEM: 0 MB";
            // 
            // clockTimer
            // 
            this.clockTimer.Interval = 1000;
            this.clockTimer.Tick += new System.EventHandler(this.clockTimer_Tick);
            // 
            // MiniOs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(24)))), ((int)(((byte)(37)))));
            this.ClientSize = new System.Drawing.Size(1388, 736);
            this.Controls.Add(this.taskbarPanel);
            this.Controls.Add(this.MainPanel);
            this.Controls.Add(this.headerpanel);
            this.Controls.Add(this.MenuPanel);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MiniOs";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MiniOs";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MiniOs_Load);
            this.MenuPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.guna2CirclePictureBox1)).EndInit();
            this.headerpanel.ResumeLayout(false);
            this.headerpanel.PerformLayout();
            this.pnlWindowControls.ResumeLayout(false);
            this.taskbarPanel.ResumeLayout(false);
            this.taskbarPanel.PerformLayout();
            this.pnlSystemTray.ResumeLayout(false);
            this.pnlSystemTray.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private Guna.UI2.WinForms.Guna2Panel MenuPanel;
        private Guna.UI2.WinForms.Guna2Panel headerpanel;
        private Guna.UI2.WinForms.Guna2Button btnProcessManager;
        private Guna.UI2.WinForms.Guna2Button btnShutdown;
        private Guna.UI2.WinForms.Guna2Button btnMemoryUsage;
        private Guna.UI2.WinForms.Guna2Button btnSystemConsole;
        private Guna.UI2.WinForms.Guna2HtmlLabel lbldate;
        private Guna.UI2.WinForms.Guna2Panel MainPanel;
        private Guna.UI2.WinForms.Guna2Button btnSyscallLogs;
        private Guna.UI2.WinForms.Guna2CirclePictureBox guna2CirclePictureBox1;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblCurrentView;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblTime;
        private Label lblAppTitle;
        private Guna.UI2.WinForms.Guna2Panel pnlActiveIndicator;
        private Guna.UI2.WinForms.Guna2Panel taskbarPanel;
        private Panel pnlSystemTray;
        private Label lblCpu;
        private Label lblMemory;
        private Label lblStatus;
        private Panel pnlWindowControls;
        private Guna.UI2.WinForms.Guna2CircleButton btnClose;
        private Guna.UI2.WinForms.Guna2CircleButton btnMaximize;
        private Guna.UI2.WinForms.Guna2CircleButton btnMinimize;
        private Timer clockTimer;
    }
}