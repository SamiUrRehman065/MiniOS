using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace KernelApp.UserControls
{
    public partial class SysLogControl : UserControl
    {
        // Syscall DLL imports
        [DllImport("syscall.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "Sys_Init")]
        private static extern void Native_Sys_Init();

        [DllImport("syscall.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "Sys_Log", CharSet = CharSet.Ansi)]
        private static extern void Native_Sys_Log(string message);

        // Track if syscall DLL is available
        private static bool syscallAvailable = false;
        private static bool syscallChecked = false;

        // Log file path
        private readonly string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", "kernel.log");
        private string lastLogContent = "";
        private int logEntryCount = 0;

        // Colors
        private readonly Color colorInfo = Color.FromArgb(166, 173, 186);
        private readonly Color colorCommand = Color.FromArgb(99, 102, 241);
        private readonly Color colorSystem = Color.FromArgb(34, 197, 94);
        private readonly Color colorWarning = Color.FromArgb(251, 191, 36);
        private readonly Color colorError = Color.FromArgb(239, 68, 68);

        public SysLogControl()
        {
            InitializeComponent();
        }

        private void SysLogControl_Load(object sender, EventArgs e)
        {
            CheckSyscallAvailability();
            UpdateLogPathDisplay();
            RefreshLogs();
        }

        private static void CheckSyscallAvailability()
        {
            if (syscallChecked) return;

            syscallChecked = true;
            try
            {
                Native_Sys_Init();
                syscallAvailable = true;
            }
            catch (DllNotFoundException)
            {
                syscallAvailable = false;
            }
            catch (EntryPointNotFoundException)
            {
                syscallAvailable = false;
            }
            catch (Exception)
            {
                syscallAvailable = false;
            }
        }

        private static void Sys_Log(string message)
        {
            if (!syscallChecked) CheckSyscallAvailability();
            if (!syscallAvailable) return;

            try
            {
                Native_Sys_Log(message);
            }
            catch { }
        }

        private void UpdateLogPathDisplay()
        {
            lblLogPath.Text = $"Path: {logFilePath}";
        }

        private void refreshTimer_Tick(object sender, EventArgs e)
        {
            RefreshLogs();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshLogs();
            ShowMessage("Logs refreshed", Color.FromArgb(99, 102, 241));
        }

        private void chkAutoRefresh_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAutoRefresh.Checked)
            {
                refreshTimer.Start();
                ShowMessage("Auto-refresh enabled (3s interval)", Color.FromArgb(34, 197, 94));
            }
            else
            {
                refreshTimer.Stop();
                ShowMessage("Auto-refresh disabled", Color.FromArgb(251, 191, 36));
            }
        }

        private void RefreshLogs()
        {
            try
            {
                // Check if log file exists
                if (!File.Exists(logFilePath))
                {
                    // Try to create Logs directory
                    string logsDir = Path.GetDirectoryName(logFilePath);
                    if (!Directory.Exists(logsDir))
                    {
                        Directory.CreateDirectory(logsDir);
                    }

                    // Show empty state
                    dgvLogs.Rows.Clear();
                    rtbRawLog.Text = "[No log file found. Logs will appear when syscall.dll writes to kernel.log]";
                    lblLogCount.Text = "Entries: 0";
                    lblFileSize.Text = "Size: 0 bytes";
                    lblLastRefresh.Text = $"Last Refresh: {DateTime.Now:HH:mm:ss}";
                    return;
                }

                // Read log file with shared access
                string logContent;
                using (var fs = new FileStream(logFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var sr = new StreamReader(fs))
                {
                    logContent = sr.ReadToEnd();
                }

                // Check if content changed
                if (logContent == lastLogContent)
                {
                    lblLastRefresh.Text = $"Last Refresh: {DateTime.Now:HH:mm:ss}";
                    return;
                }

                lastLogContent = logContent;

                // Update file info
                var fileInfo = new FileInfo(logFilePath);
                lblFileSize.Text = $"Size: {FormatFileSize(fileInfo.Length)}";

                // Parse and display logs
                ParseAndDisplayLogs(logContent);

                // Update raw log view
                rtbRawLog.Text = logContent;
                rtbRawLog.SelectionStart = rtbRawLog.Text.Length;
                rtbRawLog.ScrollToCaret();

                lblLastRefresh.Text = $"Last Refresh: {DateTime.Now:HH:mm:ss}";
            }
            catch (Exception ex)
            {
                ShowMessage($"Error reading log: {ex.Message}", colorError);
            }
        }

        private void ParseAndDisplayLogs(string logContent)
        {
            dgvLogs.SuspendLayout();
            dgvLogs.Rows.Clear();

            string[] lines = logContent.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            logEntryCount = 0;

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                var entry = ParseLogEntry(line);
                dgvLogs.Rows.Add(entry.Timestamp, entry.Type, entry.Message);
                logEntryCount++;
            }

            lblLogCount.Text = $"Entries: {logEntryCount}";

            dgvLogs.ResumeLayout(true);

            // Scroll to bottom
            if (dgvLogs.Rows.Count > 0)
            {
                dgvLogs.FirstDisplayedScrollingRowIndex = dgvLogs.Rows.Count - 1;
            }
        }

        private LogEntry ParseLogEntry(string line)
        {
            var entry = new LogEntry
            {
                Timestamp = DateTime.Now.ToString("HH:mm:ss.fff"),
                Type = "INFO",
                Message = line
            };

            // Try to detect log type from content
            string upperLine = line.ToUpper();

            if (line.StartsWith("CMD:") || upperLine.Contains("COMMAND"))
            {
                entry.Type = "CMD";
                entry.Message = line.StartsWith("CMD:") ? line.Substring(4).Trim() : line;
            }
            else if (upperLine.Contains("ERROR") || upperLine.Contains("FAIL"))
            {
                entry.Type = "ERROR";
            }
            else if (upperLine.Contains("WARN"))
            {
                entry.Type = "WARN";
            }
            else if (upperLine.Contains("INIT") || upperLine.Contains("BOOT") || upperLine.Contains("START"))
            {
                entry.Type = "SYSTEM";
            }
            else if (upperLine.Contains("SHUTDOWN") || upperLine.Contains("REBOOT"))
            {
                entry.Type = "POWER";
            }
            else if (upperLine.Contains("ALLOC") || upperLine.Contains("FREE") || upperLine.Contains("MEMORY"))
            {
                entry.Type = "MEM";
            }
            else if (upperLine.Contains("PROCESS") || upperLine.Contains("PID"))
            {
                entry.Type = "PROC";
            }

            return entry;
        }

        private void dgvLogs_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvLogs.Columns[e.ColumnIndex].Name == "colType" && e.Value != null)
            {
                string type = e.Value.ToString();
                switch (type)
                {
                    case "CMD":
                        e.CellStyle.ForeColor = colorCommand;
                        break;
                    case "ERROR":
                        e.CellStyle.ForeColor = colorError;
                        break;
                    case "WARN":
                        e.CellStyle.ForeColor = colorWarning;
                        break;
                    case "SYSTEM":
                    case "POWER":
                        e.CellStyle.ForeColor = colorSystem;
                        break;
                    case "MEM":
                    case "PROC":
                        e.CellStyle.ForeColor = Color.FromArgb(14, 165, 233);
                        break;
                    default:
                        e.CellStyle.ForeColor = colorInfo;
                        break;
                }
            }

            // Color the message column based on type as well
            if (dgvLogs.Columns[e.ColumnIndex].Name == "colMessage" && e.RowIndex >= 0)
            {
                var typeCell = dgvLogs.Rows[e.RowIndex].Cells["colType"];
                if (typeCell.Value != null)
                {
                    string type = typeCell.Value.ToString();
                    switch (type)
                    {
                        case "ERROR":
                            e.CellStyle.ForeColor = colorError;
                            break;
                        case "WARN":
                            e.CellStyle.ForeColor = colorWarning;
                            break;
                    }
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Are you sure you want to clear the kernel log file?",
                "Confirm Clear",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                ClearLogFile();
            }
        }

        private void ClearLogFile()
        {
            try
            {
                if (File.Exists(logFilePath))
                {
                    File.WriteAllText(logFilePath, "");
                    lastLogContent = "";
                }

                dgvLogs.Rows.Clear();
                rtbRawLog.Clear();
                lblLogCount.Text = "Entries: 0";
                lblFileSize.Text = "Size: 0 bytes";

                // Log the clear action
                Sys_Log("Kernel log cleared by user");

                ShowMessage("Log file cleared", Color.FromArgb(34, 197, 94));
            }
            catch (Exception ex)
            {
                ShowMessage($"Failed to clear log: {ex.Message}", colorError);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            ExportLog();
        }

        private void ExportLog()
        {
            try
            {
                using (var sfd = new SaveFileDialog())
                {
                    sfd.Filter = "Log Files (*.log)|*.log|Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                    sfd.FileName = $"kernel_log_export_{DateTime.Now:yyyyMMdd_HHmmss}.log";
                    sfd.Title = "Export Kernel Log";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        string content = rtbRawLog.Text;

                        // Add export header
                        string header = $"=== MiniOS Kernel Log Export ==={Environment.NewLine}" +
                                       $"Exported: {DateTime.Now:yyyy-MM-dd HH:mm:ss}{Environment.NewLine}" +
                                       $"Entries: {logEntryCount}{Environment.NewLine}" +
                                       $"================================={Environment.NewLine}{Environment.NewLine}";

                        File.WriteAllText(sfd.FileName, header + content);

                        Sys_Log($"Kernel log exported to: {sfd.FileName}");
                        ShowMessage($"Log exported to: {Path.GetFileName(sfd.FileName)}", Color.FromArgb(34, 197, 94));
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"Export failed: {ex.Message}", colorError);
            }
        }

        private string FormatFileSize(long bytes)
        {
            if (bytes < 1024)
                return $"{bytes} bytes";
            else if (bytes < 1024 * 1024)
                return $"{bytes / 1024.0:F1} KB";
            else
                return $"{bytes / (1024.0 * 1024.0):F2} MB";
        }

        private void ShowMessage(string message, Color color)
        {
            lblStatus.Text = message;
            lblStatus.ForeColor = color;

            var resetTimer = new Timer { Interval = 3000 };
            resetTimer.Tick += (s, ev) =>
            {
                lblStatus.ForeColor = Color.FromArgb(34, 197, 94);
                lblStatus.Text = "Log Monitor Active";
                resetTimer.Stop();
                resetTimer.Dispose();
            };
            resetTimer.Start();
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            refreshTimer?.Stop();
            base.OnHandleDestroyed(e);
        }

        // Internal class for log entry
        private class LogEntry
        {
            public string Timestamp { get; set; }
            public string Type { get; set; }
            public string Message { get; set; }
        }
    }
}
