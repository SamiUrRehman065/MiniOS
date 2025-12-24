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
            this.MenuPanel = new Guna.UI2.WinForms.Guna2Panel();
            this.btnSyscallLogs = new System.Windows.Forms.Button();
            this.btnShutdown = new System.Windows.Forms.Button();
            this.btnMemoryUsage = new System.Windows.Forms.Button();
            this.btnSystemConsole = new System.Windows.Forms.Button();
            this.btnProcessManager = new System.Windows.Forms.Button();
            this.guna2CirclePictureBox1 = new Guna.UI2.WinForms.Guna2CirclePictureBox();
            this.headerpanel = new Guna.UI2.WinForms.Guna2Panel();
            this.lbldashboard = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lbldate = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.MainPanel = new Guna.UI2.WinForms.Guna2Panel();
            this.MenuPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.guna2CirclePictureBox1)).BeginInit();
            this.headerpanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // MenuPanel
            // 
            this.MenuPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.MenuPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.MenuPanel.BorderColor = System.Drawing.Color.SkyBlue;
            this.MenuPanel.BorderThickness = 2;
            this.MenuPanel.Controls.Add(this.btnSyscallLogs);
            this.MenuPanel.Controls.Add(this.btnShutdown);
            this.MenuPanel.Controls.Add(this.btnMemoryUsage);
            this.MenuPanel.Controls.Add(this.btnSystemConsole);
            this.MenuPanel.Controls.Add(this.btnProcessManager);
            this.MenuPanel.Controls.Add(this.guna2CirclePictureBox1);
            this.MenuPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.MenuPanel.Location = new System.Drawing.Point(0, 0);
            this.MenuPanel.Name = "MenuPanel";
            this.MenuPanel.Size = new System.Drawing.Size(273, 736);
            this.MenuPanel.TabIndex = 1;
            // 
            // btnSyscallLogs
            // 
            this.btnSyscallLogs.BackColor = System.Drawing.Color.Transparent;
            this.btnSyscallLogs.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnSyscallLogs.FlatAppearance.BorderSize = 0;
            this.btnSyscallLogs.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.btnSyscallLogs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSyscallLogs.Font = new System.Drawing.Font("Segoe UI Semibold", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSyscallLogs.ForeColor = System.Drawing.Color.White;
            this.btnSyscallLogs.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSyscallLogs.Location = new System.Drawing.Point(-1, 403);
            this.btnSyscallLogs.Name = "btnSyscallLogs";
            this.btnSyscallLogs.Padding = new System.Windows.Forms.Padding(3);
            this.btnSyscallLogs.Size = new System.Drawing.Size(271, 53);
            this.btnSyscallLogs.TabIndex = 10;
            this.btnSyscallLogs.Text = "    Syscall Logs";
            this.btnSyscallLogs.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSyscallLogs.UseVisualStyleBackColor = false;
            this.btnSyscallLogs.Click += new System.EventHandler(this.btnSyscallLogs_Click);
            // 
            // btnShutdown
            // 
            this.btnShutdown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnShutdown.BackColor = System.Drawing.Color.Transparent;
            this.btnShutdown.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnShutdown.FlatAppearance.BorderSize = 0;
            this.btnShutdown.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.btnShutdown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShutdown.Font = new System.Drawing.Font("Segoe UI Semibold", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShutdown.ForeColor = System.Drawing.Color.White;
            this.btnShutdown.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnShutdown.Location = new System.Drawing.Point(2, 680);
            this.btnShutdown.Name = "btnShutdown";
            this.btnShutdown.Padding = new System.Windows.Forms.Padding(3);
            this.btnShutdown.Size = new System.Drawing.Size(271, 53);
            this.btnShutdown.TabIndex = 6;
            this.btnShutdown.Text = "   Shutdown";
            this.btnShutdown.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnShutdown.UseVisualStyleBackColor = false;
            this.btnShutdown.Click += new System.EventHandler(this.btnShutdown_Click);
            // 
            // btnMemoryUsage
            // 
            this.btnMemoryUsage.BackColor = System.Drawing.Color.Transparent;
            this.btnMemoryUsage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnMemoryUsage.FlatAppearance.BorderSize = 0;
            this.btnMemoryUsage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.btnMemoryUsage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMemoryUsage.Font = new System.Drawing.Font("Segoe UI Semibold", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMemoryUsage.ForeColor = System.Drawing.Color.White;
            this.btnMemoryUsage.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMemoryUsage.Location = new System.Drawing.Point(3, 336);
            this.btnMemoryUsage.Name = "btnMemoryUsage";
            this.btnMemoryUsage.Padding = new System.Windows.Forms.Padding(3);
            this.btnMemoryUsage.Size = new System.Drawing.Size(271, 53);
            this.btnMemoryUsage.TabIndex = 5;
            this.btnMemoryUsage.Text = "   Memory Usage";
            this.btnMemoryUsage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMemoryUsage.UseVisualStyleBackColor = false;
            this.btnMemoryUsage.Click += new System.EventHandler(this.btnMemoryUsage_Click);
            // 
            // btnSystemConsole
            // 
            this.btnSystemConsole.BackColor = System.Drawing.Color.Transparent;
            this.btnSystemConsole.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnSystemConsole.FlatAppearance.BorderSize = 0;
            this.btnSystemConsole.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.btnSystemConsole.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSystemConsole.Font = new System.Drawing.Font("Segoe UI Semibold", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSystemConsole.ForeColor = System.Drawing.Color.White;
            this.btnSystemConsole.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSystemConsole.Location = new System.Drawing.Point(3, 269);
            this.btnSystemConsole.Name = "btnSystemConsole";
            this.btnSystemConsole.Padding = new System.Windows.Forms.Padding(3);
            this.btnSystemConsole.Size = new System.Drawing.Size(271, 53);
            this.btnSystemConsole.TabIndex = 4;
            this.btnSystemConsole.Text = "   Console";
            this.btnSystemConsole.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSystemConsole.UseVisualStyleBackColor = false;
            this.btnSystemConsole.Click += new System.EventHandler(this.btnSystemConsole_Click);
            // 
            // btnProcessManager
            // 
            this.btnProcessManager.BackColor = System.Drawing.Color.Transparent;
            this.btnProcessManager.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnProcessManager.FlatAppearance.BorderSize = 0;
            this.btnProcessManager.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.btnProcessManager.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProcessManager.Font = new System.Drawing.Font("Segoe UI Semibold", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProcessManager.ForeColor = System.Drawing.Color.White;
            this.btnProcessManager.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnProcessManager.Location = new System.Drawing.Point(3, 202);
            this.btnProcessManager.Name = "btnProcessManager";
            this.btnProcessManager.Padding = new System.Windows.Forms.Padding(3);
            this.btnProcessManager.Size = new System.Drawing.Size(271, 53);
            this.btnProcessManager.TabIndex = 3;
            this.btnProcessManager.Text = "   Task Manager";
            this.btnProcessManager.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnProcessManager.UseVisualStyleBackColor = false;
            this.btnProcessManager.Click += new System.EventHandler(this.btnProcessManager_Click);
            // 
            // guna2CirclePictureBox1
            // 
            this.guna2CirclePictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.guna2CirclePictureBox1.FillColor = System.Drawing.Color.Transparent;
            this.guna2CirclePictureBox1.Image = global::KernelApp.Properties.Resources.system;
            this.guna2CirclePictureBox1.ImageRotate = 0F;
            this.guna2CirclePictureBox1.Location = new System.Drawing.Point(3, 3);
            this.guna2CirclePictureBox1.Name = "guna2CirclePictureBox1";
            this.guna2CirclePictureBox1.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.guna2CirclePictureBox1.Size = new System.Drawing.Size(267, 166);
            this.guna2CirclePictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.guna2CirclePictureBox1.TabIndex = 2;
            this.guna2CirclePictureBox1.TabStop = false;
            // 
            // headerpanel
            // 
            this.headerpanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.headerpanel.BackColor = System.Drawing.Color.SteelBlue;
            this.headerpanel.BorderColor = System.Drawing.Color.SkyBlue;
            this.headerpanel.BorderThickness = 2;
            this.headerpanel.Controls.Add(this.lbldashboard);
            this.headerpanel.Controls.Add(this.lbldate);
            this.headerpanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerpanel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.headerpanel.Location = new System.Drawing.Point(273, 0);
            this.headerpanel.Name = "headerpanel";
            this.headerpanel.Size = new System.Drawing.Size(1115, 68);
            this.headerpanel.TabIndex = 2;
            // 
            // lbldashboard
            // 
            this.lbldashboard.AutoSize = false;
            this.lbldashboard.BackColor = System.Drawing.Color.Transparent;
            this.lbldashboard.BackgroundImage = global::KernelApp.Properties.Resources.ic_dashboard_128_28270;
            this.lbldashboard.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.lbldashboard.Font = new System.Drawing.Font("Segoe UI", 19.8F, System.Drawing.FontStyle.Bold);
            this.lbldashboard.ForeColor = System.Drawing.Color.White;
            this.lbldashboard.Location = new System.Drawing.Point(26, 10);
            this.lbldashboard.Name = "lbldashboard";
            this.lbldashboard.Size = new System.Drawing.Size(278, 47);
            this.lbldashboard.TabIndex = 0;
            this.lbldashboard.Text = "DashBoard";
            this.lbldashboard.TextAlignment = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lbldate
            // 
            this.lbldate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbldate.BackColor = System.Drawing.Color.Transparent;
            this.lbldate.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbldate.ForeColor = System.Drawing.Color.White;
            this.lbldate.Location = new System.Drawing.Point(1086, 10);
            this.lbldate.Name = "lbldate";
            this.lbldate.Size = new System.Drawing.Size(17, 43);
            this.lbldate.TabIndex = 1;
            this.lbldate.Text = "*";
            this.lbldate.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainPanel
            // 
            this.MainPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MainPanel.BackColor = System.Drawing.Color.White;
            this.MainPanel.BorderColor = System.Drawing.Color.Gray;
            this.MainPanel.BorderRadius = 20;
            this.MainPanel.BorderThickness = 2;
            this.MainPanel.Location = new System.Drawing.Point(273, 68);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(1115, 668);
            this.MainPanel.TabIndex = 4;
            // 
            // MiniOs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1388, 736);
            this.Controls.Add(this.MainPanel);
            this.Controls.Add(this.headerpanel);
            this.Controls.Add(this.MenuPanel);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.HelpButton = true;
            this.IsMdiContainer = true;
            this.Name = "MiniOs";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MiniOs";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MiniOs_Load);
            this.MenuPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.guna2CirclePictureBox1)).EndInit();
            this.headerpanel.ResumeLayout(false);
            this.headerpanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private Guna.UI2.WinForms.Guna2Panel MenuPanel;
        private Guna.UI2.WinForms.Guna2Panel headerpanel;
        private Button btnProcessManager;
        private Button btnShutdown;
        private Button btnMemoryUsage;
        private Button btnSystemConsole;
        private Guna.UI2.WinForms.Guna2HtmlLabel lbldate;
        private Guna.UI2.WinForms.Guna2Panel MainPanel;
        private Button btnSyscallLogs;
        private Guna.UI2.WinForms.Guna2CirclePictureBox guna2CirclePictureBox1;
        private Guna.UI2.WinForms.Guna2HtmlLabel lbldashboard;
    }
}