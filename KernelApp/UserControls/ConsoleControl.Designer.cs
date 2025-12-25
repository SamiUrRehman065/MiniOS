namespace KernelApp.UserControls
{
    partial class ConsoleControl
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
            this.mainLayout = new System.Windows.Forms.TableLayoutPanel();
            this.headerPanel = new System.Windows.Forms.Panel();
            this.flowLayoutHeader = new System.Windows.Forms.FlowLayoutPanel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnClear = new Guna.UI2.WinForms.Guna2Button();
            this.btnExport = new Guna.UI2.WinForms.Guna2Button();
            this.consolePanel = new System.Windows.Forms.Panel();
            this.rtbConsoleOutput = new System.Windows.Forms.RichTextBox();
            this.inputPanel = new System.Windows.Forms.Panel();
            this.flowLayoutInput = new System.Windows.Forms.FlowLayoutPanel();
            this.lblPrompt = new System.Windows.Forms.Label();
            this.txtCommand = new Guna.UI2.WinForms.Guna2TextBox();
            this.btnExecute = new Guna.UI2.WinForms.Guna2Button();
            this.statusPanel = new System.Windows.Forms.Panel();
            this.flowLayoutStatus = new System.Windows.Forms.FlowLayoutPanel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblCommandCount = new System.Windows.Forms.Label();
            this.mainLayout.SuspendLayout();
            this.headerPanel.SuspendLayout();
            this.flowLayoutHeader.SuspendLayout();
            this.consolePanel.SuspendLayout();
            this.inputPanel.SuspendLayout();
            this.flowLayoutInput.SuspendLayout();
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
            this.mainLayout.Controls.Add(this.consolePanel, 0, 1);
            this.mainLayout.Controls.Add(this.inputPanel, 0, 2);
            this.mainLayout.Controls.Add(this.statusPanel, 0, 3);
            this.mainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayout.Location = new System.Drawing.Point(10, 10);
            this.mainLayout.Name = "mainLayout";
            this.mainLayout.RowCount = 4;
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
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
            this.flowLayoutHeader.Controls.Add(this.btnClear);
            this.flowLayoutHeader.Controls.Add(this.btnExport);
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
            this.lblTitle.Margin = new System.Windows.Forms.Padding(0, 0, 30, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.lblTitle.Size = new System.Drawing.Size(152, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "SYSTEM CONSOLE";
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
            this.btnClear.Location = new System.Drawing.Point(185, 3);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(80, 28);
            this.btnClear.TabIndex = 1;
            this.btnClear.Text = "Clear";
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
            this.btnExport.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
            this.btnExport.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.btnExport.ForeColor = System.Drawing.Color.White;
            this.btnExport.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(70)))), ((int)(((byte)(229)))));
            this.btnExport.Location = new System.Drawing.Point(271, 3);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(80, 28);
            this.btnExport.TabIndex = 2;
            this.btnExport.Text = "Export";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // consolePanel
            // 
            this.consolePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(25)))));
            this.consolePanel.Controls.Add(this.rtbConsoleOutput);
            this.consolePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.consolePanel.Location = new System.Drawing.Point(3, 58);
            this.consolePanel.Name = "consolePanel";
            this.consolePanel.Padding = new System.Windows.Forms.Padding(10);
            this.consolePanel.Size = new System.Drawing.Size(1085, 489);
            this.consolePanel.TabIndex = 1;
            // 
            // rtbConsoleOutput
            // 
            this.rtbConsoleOutput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(25)))));
            this.rtbConsoleOutput.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbConsoleOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbConsoleOutput.Font = new System.Drawing.Font("Consolas", 11F);
            this.rtbConsoleOutput.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(197)))), ((int)(((byte)(94)))));
            this.rtbConsoleOutput.Location = new System.Drawing.Point(10, 10);
            this.rtbConsoleOutput.Name = "rtbConsoleOutput";
            this.rtbConsoleOutput.ReadOnly = true;
            this.rtbConsoleOutput.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtbConsoleOutput.Size = new System.Drawing.Size(1065, 469);
            this.rtbConsoleOutput.TabIndex = 0;
            this.rtbConsoleOutput.Text = "";
            // 
            // inputPanel
            // 
            this.inputPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(46)))));
            this.inputPanel.Controls.Add(this.flowLayoutInput);
            this.inputPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.inputPanel.Location = new System.Drawing.Point(3, 553);
            this.inputPanel.Name = "inputPanel";
            this.inputPanel.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.inputPanel.Size = new System.Drawing.Size(1085, 54);
            this.inputPanel.TabIndex = 2;
            // 
            // flowLayoutInput
            // 
            this.flowLayoutInput.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutInput.Controls.Add(this.lblPrompt);
            this.flowLayoutInput.Controls.Add(this.txtCommand);
            this.flowLayoutInput.Controls.Add(this.btnExecute);
            this.flowLayoutInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutInput.Location = new System.Drawing.Point(12, 8);
            this.flowLayoutInput.Name = "flowLayoutInput";
            this.flowLayoutInput.Size = new System.Drawing.Size(1061, 38);
            this.flowLayoutInput.TabIndex = 0;
            this.flowLayoutInput.WrapContents = false;
            // 
            // lblPrompt
            // 
            this.lblPrompt.AutoSize = true;
            this.lblPrompt.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold);
            this.lblPrompt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(197)))), ((int)(((byte)(94)))));
            this.lblPrompt.Location = new System.Drawing.Point(0, 0);
            this.lblPrompt.Margin = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.lblPrompt.Name = "lblPrompt";
            this.lblPrompt.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.lblPrompt.Size = new System.Drawing.Size(90, 27);
            this.lblPrompt.TabIndex = 0;
            this.lblPrompt.Text = "MiniOS>";
            // 
            // txtCommand
            // 
            this.txtCommand.BackColor = System.Drawing.Color.Transparent;
            this.txtCommand.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(75)))));
            this.txtCommand.BorderRadius = 6;
            this.txtCommand.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtCommand.DefaultText = "";
            this.txtCommand.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtCommand.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtCommand.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtCommand.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtCommand.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(25)))));
            this.txtCommand.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(197)))), ((int)(((byte)(94)))));
            this.txtCommand.Font = new System.Drawing.Font("Consolas", 11F);
            this.txtCommand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(197)))), ((int)(((byte)(94)))));
            this.txtCommand.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(197)))), ((int)(((byte)(94)))));
            this.txtCommand.Location = new System.Drawing.Point(98, 3);
            this.txtCommand.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            this.txtCommand.Name = "txtCommand";
            this.txtCommand.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(100)))));
            this.txtCommand.PlaceholderText = "Enter command (type \'help\' for available commands)";
            this.txtCommand.SelectedText = "";
            this.txtCommand.Size = new System.Drawing.Size(850, 34);
            this.txtCommand.TabIndex = 1;
            this.txtCommand.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCommand_KeyDown);
            // 
            // btnExecute
            // 
            this.btnExecute.Animated = true;
            this.btnExecute.BackColor = System.Drawing.Color.Transparent;
            this.btnExecute.BorderRadius = 6;
            this.btnExecute.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnExecute.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnExecute.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnExecute.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnExecute.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(197)))), ((int)(((byte)(94)))));
            this.btnExecute.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnExecute.ForeColor = System.Drawing.Color.White;
            this.btnExecute.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.btnExecute.Location = new System.Drawing.Point(961, 3);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(90, 34);
            this.btnExecute.TabIndex = 2;
            this.btnExecute.Text = "Execute";
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
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
            this.flowLayoutStatus.Controls.Add(this.lblCommandCount);
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
            this.lblStatus.Margin = new System.Windows.Forms.Padding(0, 0, 40, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.lblStatus.Size = new System.Drawing.Size(98, 17);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "Console Ready";
            // 
            // lblCommandCount
            // 
            this.lblCommandCount.AutoSize = true;
            this.lblCommandCount.Font = new System.Drawing.Font("Consolas", 9F);
            this.lblCommandCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(173)))), ((int)(((byte)(186)))));
            this.lblCommandCount.Location = new System.Drawing.Point(138, 0);
            this.lblCommandCount.Margin = new System.Windows.Forms.Padding(0);
            this.lblCommandCount.Name = "lblCommandCount";
            this.lblCommandCount.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.lblCommandCount.Size = new System.Drawing.Size(98, 17);
            this.lblCommandCount.TabIndex = 1;
            this.lblCommandCount.Text = "Commands: 0";
            // 
            // ConsoleControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(51)))));
            this.Controls.Add(this.mainLayout);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "ConsoleControl";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Size = new System.Drawing.Size(1111, 665);
            this.Load += new System.EventHandler(this.ConsoleControl_Load);
            this.mainLayout.ResumeLayout(false);
            this.headerPanel.ResumeLayout(false);
            this.flowLayoutHeader.ResumeLayout(false);
            this.flowLayoutHeader.PerformLayout();
            this.consolePanel.ResumeLayout(false);
            this.inputPanel.ResumeLayout(false);
            this.flowLayoutInput.ResumeLayout(false);
            this.flowLayoutInput.PerformLayout();
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
        private Guna.UI2.WinForms.Guna2Button btnClear;
        private Guna.UI2.WinForms.Guna2Button btnExport;
        private System.Windows.Forms.Panel consolePanel;
        private System.Windows.Forms.RichTextBox rtbConsoleOutput;
        private System.Windows.Forms.Panel inputPanel;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutInput;
        private System.Windows.Forms.Label lblPrompt;
        private Guna.UI2.WinForms.Guna2TextBox txtCommand;
        private Guna.UI2.WinForms.Guna2Button btnExecute;
        private System.Windows.Forms.Panel statusPanel;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutStatus;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblCommandCount;
    }
}
