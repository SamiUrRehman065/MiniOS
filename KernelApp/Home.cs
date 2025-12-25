using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KernelApp
{
    public partial class MiniOs : Form
    {
        // For dragging the borderless form
        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;

        private PerformanceCounter cpuCounter;
        private Process currentProcess;
        private Timer indicatorTimer; // Single timer instance for indicator animation

        public MiniOs()
        {
            InitializeComponent();
            lbldate.Text = DateTime.Now.ToString("dddd, MMMM dd, yyyy");
            lblTime.Text = DateTime.Now.ToString("HH:mm:ss");

            // Initialize performance counters
            try
            {
                cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                currentProcess = Process.GetCurrentProcess();
            }
            catch { }

            // Initialize indicator animation timer
            indicatorTimer = new Timer { Interval = 10 };

            clockTimer.Start();
        }

        private async Task LoadControl(UserControl control, string viewName, int buttonY)
        {
            MainPanel.Controls.Clear();
            control.Dock = DockStyle.Fill;
            control.Margin = new Padding(2);
            control.BackColor = Color.FromArgb(36, 36, 51);

            MainPanel.Padding = new Padding(10);

            // Animate the active indicator
            AnimateIndicator(buttonY);

            // Update header
            lblCurrentView.Text = viewName;
            lblStatus.Text = "Loading " + viewName + "...";

            await Task.Run(() =>
            {
                Invoke(new Action(() =>
                {
                    MainPanel.Controls.Add(control);
                }));
            });

            lblStatus.Text = "System Ready";
        }

        private int targetIndicatorY; // Store target position

        private void AnimateIndicator(int targetY)
        {
            // Stop any existing animation
            indicatorTimer.Stop();

            // Remove previous event handlers to avoid stacking
            indicatorTimer.Tick -= IndicatorTimer_Tick;

            // Set the new target
            targetIndicatorY = targetY;

            // Attach fresh handler and start
            indicatorTimer.Tick += IndicatorTimer_Tick;
            indicatorTimer.Start();
        }

        private void IndicatorTimer_Tick(object sender, EventArgs e)
        {
            int currentY = pnlActiveIndicator.Top;
            int diff = targetIndicatorY - currentY;

            if (Math.Abs(diff) < 2)
            {
                // Snap to final position and stop
                pnlActiveIndicator.Top = targetIndicatorY;
                indicatorTimer.Stop();
            }
            else
            {
                // Smooth easing movement
                int step = diff / 3;
                if (step == 0)
                    step = diff > 0 ? 1 : -1;
                pnlActiveIndicator.Top = currentY + step;
            }
        }

        private void ResetButtonStyles()
        {
            btnProcessManager.FillColor = Color.Transparent;
            btnProcessManager.ForeColor = Color.FromArgb(166, 173, 186);
            btnSystemConsole.FillColor = Color.Transparent;
            btnSystemConsole.ForeColor = Color.FromArgb(166, 173, 186);
            btnMemoryUsage.FillColor = Color.Transparent;
            btnMemoryUsage.ForeColor = Color.FromArgb(166, 173, 186);
            btnSyscallLogs.FillColor = Color.Transparent;
            btnSyscallLogs.ForeColor = Color.FromArgb(166, 173, 186);
        }

        private void SetActiveButton(Guna.UI2.WinForms.Guna2Button btn)
        {
            ResetButtonStyles();
            btn.FillColor = Color.FromArgb(45, 45, 65);
            btn.ForeColor = Color.White;
        }

        private async void MiniOs_Load(object sender, EventArgs e)
        {
            // Set initial indicator position without animation
            pnlActiveIndicator.Top = btnProcessManager.Top;
            SetActiveButton(btnProcessManager);
            await LoadControl(new UserControls.ProcessMgrControl(), "Task Manager", btnProcessManager.Top);
        }

        private async void btnProcessManager_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnProcessManager);
            await LoadControl(new UserControls.ProcessMgrControl(), "Task Manager", btnProcessManager.Top);
        }

        private async void btnSystemConsole_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnSystemConsole);
            await LoadControl(new UserControls.ConsoleControl(), "Console", btnSystemConsole.Top);
        }

        private async void btnMemoryUsage_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnMemoryUsage);
            await LoadControl(new UserControls.MemoryVisControl(), "Memory Usage", btnMemoryUsage.Top);
        }

        private async void btnSyscallLogs_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnSyscallLogs);
            await LoadControl(new UserControls.SysLogControl(), "Syscall Logs", btnSyscallLogs.Top);
        }

        private void btnShutdown_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "Shutting down...";
            clockTimer.Stop();
            indicatorTimer.Stop();
            this.Close();
        }

        private void clockTimer_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString("HH:mm:ss");

            // Update system stats
            try
            {
                float cpuUsage = cpuCounter?.NextValue() ?? 0;
                lblCpu.Text = $"CPU: {cpuUsage:F0}%";

                currentProcess?.Refresh();
                long memoryMB = (currentProcess?.WorkingSet64 ?? 0) / (1024 * 1024);
                lblMemory.Text = $"MEM: {memoryMB} MB";
            }
            catch { }
        }

        private void headerpanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnMaximize_Click(object sender, EventArgs e)
        {
            this.WindowState = this.WindowState == FormWindowState.Maximized
                ? FormWindowState.Normal
                : FormWindowState.Maximized;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
