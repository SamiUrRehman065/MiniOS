using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using KernelApp.Helpers;
using KernelApp.UserControls;

namespace KernelApp
{
    public partial class MiniOs : Form
    {
        #region P/Invoke for Window Dragging

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;

        #endregion

        #region Colors

        private static readonly Color ColorButtonActive = Color.FromArgb(45, 45, 65);
        private static readonly Color ColorButtonInactive = Color.Transparent;
        private static readonly Color ColorTextActive = Color.White;
        private static readonly Color ColorTextInactive = Color.FromArgb(166, 173, 186);
        private static readonly Color ColorPanelBackground = Color.FromArgb(36, 36, 51);

        #endregion

        #region State

        private PerformanceCounter _cpuCounter;
        private Process _currentProcess;
        private readonly Timer _indicatorTimer;
        private int _targetIndicatorY;

        #endregion

        public MiniOs()
        {
            InitializeComponent();

            lbldate.Text = DateTime.Now.ToString("dddd, MMMM dd, yyyy");
            lblTime.Text = DateTime.Now.ToString("HH:mm:ss");

            InitializePerformanceCounters();

            _indicatorTimer = new Timer { Interval = 10 };
            _indicatorTimer.Tick += IndicatorTimer_Tick;

            clockTimer.Start();
        }

        #region Initialization

        private void InitializePerformanceCounters()
        {
            try
            {
                _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                _currentProcess = Process.GetCurrentProcess();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"MiniOs: Failed to initialize performance counters: {ex.Message}");
            }
        }

        private async void MiniOs_Load(object sender, EventArgs e)
        {
            SyscallHelper.EnsureInitialized();
            SyscallHelper.Log("MiniOS Kernel started");

            // Set initial indicator position without animation
            pnlActiveIndicator.Top = btnProcessManager.Top;
            SetActiveButton(btnProcessManager);

            await LoadControlAsync(new ProcessMgrControl(), "Task Manager", btnProcessManager.Top);
        }

        #endregion

        #region View Loading

        private async Task LoadControlAsync(UserControl control, string viewName, int buttonY)
        {
            // Clear existing controls and dispose them
            while (MainPanel.Controls.Count > 0)
            {
                var ctrl = MainPanel.Controls[0];
                MainPanel.Controls.RemoveAt(0);
                ctrl.Dispose();
            }

            control.Dock = DockStyle.Fill;
            control.Margin = new Padding(2);
            control.BackColor = ColorPanelBackground;

            MainPanel.Padding = new Padding(10);

            AnimateIndicator(buttonY);

            lblCurrentView.Text = viewName;
            lblStatus.Text = $"Loading {viewName}...";

            await Task.Run(() =>
            {
                Invoke(new Action(() =>
                {
                    MainPanel.Controls.Add(control);
                }));
            });

            lblStatus.Text = "System Ready";
            SyscallHelper.Log($"VIEW: Loaded {viewName}");
        }

        #endregion

        #region Indicator Animation

        private void AnimateIndicator(int targetY)
        {
            _indicatorTimer.Stop();
            _targetIndicatorY = targetY;
            _indicatorTimer.Start();
        }

        private void IndicatorTimer_Tick(object sender, EventArgs e)
        {
            int currentY = pnlActiveIndicator.Top;
            int diff = _targetIndicatorY - currentY;

            if (Math.Abs(diff) < 2)
            {
                pnlActiveIndicator.Top = _targetIndicatorY;
                _indicatorTimer.Stop();
            }
            else
            {
                int step = diff / 3;
                if (step == 0)
                    step = diff > 0 ? 1 : -1;
                pnlActiveIndicator.Top = currentY + step;
            }
        }

        #endregion

        #region Button Styling

        private void ResetButtonStyles()
        {
            SetButtonStyle(btnProcessManager, false);
            SetButtonStyle(btnSystemConsole, false);
            SetButtonStyle(btnMemoryUsage, false);
            SetButtonStyle(btnSyscallLogs, false);
        }

        private static void SetButtonStyle(Guna.UI2.WinForms.Guna2Button btn, bool isActive)
        {
            btn.FillColor = isActive ? ColorButtonActive : ColorButtonInactive;
            btn.ForeColor = isActive ? ColorTextActive : ColorTextInactive;
        }

        private void SetActiveButton(Guna.UI2.WinForms.Guna2Button btn)
        {
            ResetButtonStyles();
            SetButtonStyle(btn, true);
        }

        #endregion

        #region Navigation Event Handlers

        private async void btnProcessManager_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnProcessManager);
            await LoadControlAsync(new ProcessMgrControl(), "Task Manager", btnProcessManager.Top);
        }

        private async void btnSystemConsole_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnSystemConsole);
            await LoadControlAsync(new ConsoleControl(), "Console", btnSystemConsole.Top);
        }

        private async void btnMemoryUsage_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnMemoryUsage);
            await LoadControlAsync(new MemoryVisControl(), "Memory Usage", btnMemoryUsage.Top);
        }

        private async void btnSyscallLogs_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnSyscallLogs);
            await LoadControlAsync(new SysLogControl(), "Syscall Logs", btnSyscallLogs.Top);
        }

        private void btnShutdown_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "Shutting down...";
            SyscallHelper.Log("MiniOS Kernel shutdown initiated");

            clockTimer.Stop();
            _indicatorTimer.Stop();

            Close();
        }

        #endregion

        #region Clock & System Stats

        private void clockTimer_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString("HH:mm:ss");
            UpdateSystemStats();
        }

        private void UpdateSystemStats()
        {
            try
            {
                if (_cpuCounter != null)
                {
                    float cpuUsage = _cpuCounter.NextValue();
                    lblCpu.Text = $"CPU: {cpuUsage:F0}%";
                }

                if (_currentProcess != null)
                {
                    _currentProcess.Refresh();
                    long memoryMB = _currentProcess.WorkingSet64 / (1024 * 1024);
                    lblMemory.Text = $"MEM: {memoryMB} MB";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"MiniOs.UpdateSystemStats failed: {ex.Message}");
            }
        }

        #endregion

        #region Window Controls

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
            WindowState = FormWindowState.Minimized;
        }

        private void btnMaximize_Click(object sender, EventArgs e)
        {
            WindowState = WindowState == FormWindowState.Maximized
                ? FormWindowState.Normal
                : FormWindowState.Maximized;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion

        #region Cleanup

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            clockTimer?.Stop();
            _indicatorTimer?.Stop();
            _indicatorTimer?.Dispose();

            _cpuCounter?.Dispose();
            _currentProcess?.Dispose();

            // Dispose all loaded UserControls
            foreach (Control ctrl in MainPanel.Controls)
            {
                ctrl.Dispose();
            }

            SyscallHelper.Log("MiniOS Kernel shutdown complete");
            base.OnFormClosing(e);
        }

        #endregion
    }
}
