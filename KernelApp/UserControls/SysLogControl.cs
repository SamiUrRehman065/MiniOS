using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using KernelApp.Helpers;

namespace KernelApp.UserControls
{
    public partial class SysLogControl : UserControl
    {
        #region Constants

        private const int MESSAGE_RESET_INTERVAL_MS = 3000;
        private const long BYTES_PER_KB = 1024;
        private const long BYTES_PER_MB = 1024 * 1024;

        #endregion

        #region Colors

        private static readonly Color ColorInfo = Color.FromArgb(166, 173, 186);
        private static readonly Color ColorCommand = Color.FromArgb(99, 102, 241);
        private static readonly Color ColorSystem = Color.FromArgb(34, 197, 94);
        private static readonly Color ColorWarning = Color.FromArgb(251, 191, 36);
        private static readonly Color ColorError = Color.FromArgb(239, 68, 68);
        private static readonly Color ColorMemProc = Color.FromArgb(14, 165, 233);

        #endregion

        #region State

        private readonly string _logFilePath;
        private string _lastLogContent = "";
        private int _logEntryCount;
        private Timer _messageResetTimer;

        #endregion

        public SysLogControl()
        {
            InitializeComponent();
            _logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", "kernel.log");
        }

        #region Event Handlers

        private void SysLogControl_Load(object sender, EventArgs e)
        {
            SyscallHelper.EnsureInitialized();
            UpdateLogPathDisplay();
            RefreshLogs();
        }

        private void refreshTimer_Tick(object sender, EventArgs e)
        {
            RefreshLogs();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshLogs();
            ShowMessage("Logs refreshed", ColorCommand);
        }

        private void chkAutoRefresh_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAutoRefresh.Checked)
            {
                refreshTimer.Start();
                ShowMessage("Auto-refresh enabled (3s interval)", ColorSystem);
            }
            else
            {
                refreshTimer.Stop();
                ShowMessage("Auto-refresh disabled", ColorWarning);
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

        private void btnExport_Click(object sender, EventArgs e)
        {
            ExportLog();
        }

        private void dgvLogs_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value == null) return;

            string columnName = dgvLogs.Columns[e.ColumnIndex].Name;

            if (columnName == "colType")
            {
                e.CellStyle.ForeColor = GetLogTypeColor(e.Value.ToString());
            }
            else if (columnName == "colMessage" && e.RowIndex >= 0)
            {
                var typeCell = dgvLogs.Rows[e.RowIndex].Cells["colType"];
                if (typeCell.Value != null)
                {
                    string type = typeCell.Value.ToString();
                    if (type == "ERROR")
                        e.CellStyle.ForeColor = ColorError;
                    else if (type == "WARN")
                        e.CellStyle.ForeColor = ColorWarning;
                }
            }
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            refreshTimer?.Stop();
            _messageResetTimer?.Stop();
            _messageResetTimer?.Dispose();
            base.OnHandleDestroyed(e);
        }

        #endregion

        #region Log Operations

        private void UpdateLogPathDisplay()
        {
            lblLogPath.Text = $"Path: {_logFilePath}";
        }

        private void RefreshLogs()
        {
            try
            {
                if (!File.Exists(_logFilePath))
                {
                    EnsureLogDirectoryExists();
                    ShowEmptyState();
                    return;
                }

                string logContent = ReadLogFileContent();

                // Check if content changed
                if (logContent == _lastLogContent)
                {
                    UpdateLastRefreshTime();
                    return;
                }

                _lastLogContent = logContent;

                UpdateFileInfo();
                ParseAndDisplayLogs(logContent);
                UpdateRawLogView(logContent);
                UpdateLastRefreshTime();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"SysLogControl.RefreshLogs failed: {ex.Message}");
                ShowMessage($"Error reading log: {ex.Message}", ColorError);
            }
        }

        private void EnsureLogDirectoryExists()
        {
            string logsDir = Path.GetDirectoryName(_logFilePath);
            if (!Directory.Exists(logsDir))
            {
                Directory.CreateDirectory(logsDir);
            }
        }

        private void ShowEmptyState()
        {
            dgvLogs.Rows.Clear();
            rtbRawLog.Text = "[No log file found. Logs will appear when syscall.dll writes to kernel.log]";
            lblLogCount.Text = "Entries: 0";
            lblFileSize.Text = "Size: 0 bytes";
            UpdateLastRefreshTime();
        }

        private string ReadLogFileContent()
        {
            using (var fs = new FileStream(_logFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var sr = new StreamReader(fs))
            {
                return sr.ReadToEnd();
            }
        }

        private void UpdateFileInfo()
        {
            var fileInfo = new FileInfo(_logFilePath);
            lblFileSize.Text = $"Size: {FormatFileSize(fileInfo.Length)}";
        }

        private void UpdateRawLogView(string logContent)
        {
            rtbRawLog.Text = logContent;
            rtbRawLog.SelectionStart = rtbRawLog.Text.Length;
            rtbRawLog.ScrollToCaret();
        }

        private void UpdateLastRefreshTime()
        {
            lblLastRefresh.Text = $"Last Refresh: {DateTime.Now:HH:mm:ss}";
        }

        private void ParseAndDisplayLogs(string logContent)
        {
            dgvLogs.SuspendLayout();
            dgvLogs.Rows.Clear();

            string[] lines = logContent.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            _logEntryCount = 0;

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                var entry = ParseLogEntry(line);
                dgvLogs.Rows.Add(entry.Timestamp, entry.Type, entry.Message);
                _logEntryCount++;
            }

            lblLogCount.Text = $"Entries: {_logEntryCount}";
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

            // Use IndexOf with StringComparison for case-insensitive matching
            // This avoids creating a new uppercase string for each line
            if (line.StartsWith("CMD:", StringComparison.OrdinalIgnoreCase))
            {
                entry.Type = "CMD";
                entry.Message = line.Substring(4).Trim();
            }
            else if (ContainsIgnoreCase(line, "COMMAND"))
            {
                entry.Type = "CMD";
            }
            else if (ContainsIgnoreCase(line, "ERROR") || ContainsIgnoreCase(line, "FAIL"))
            {
                entry.Type = "ERROR";
            }
            else if (ContainsIgnoreCase(line, "WARN"))
            {
                entry.Type = "WARN";
            }
            else if (ContainsIgnoreCase(line, "INIT") || ContainsIgnoreCase(line, "BOOT") || ContainsIgnoreCase(line, "START"))
            {
                entry.Type = "SYSTEM";
            }
            else if (ContainsIgnoreCase(line, "SHUTDOWN") || ContainsIgnoreCase(line, "REBOOT"))
            {
                entry.Type = "POWER";
            }
            else if (ContainsIgnoreCase(line, "ALLOC") || ContainsIgnoreCase(line, "FREE") || ContainsIgnoreCase(line, "MEMORY"))
            {
                entry.Type = "MEM";
            }
            else if (ContainsIgnoreCase(line, "PROCESS") || ContainsIgnoreCase(line, "PID"))
            {
                entry.Type = "PROC";
            }

            return entry;
        }

        private static bool ContainsIgnoreCase(string source, string value)
        {
            return source.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static Color GetLogTypeColor(string type)
        {
            switch (type)
            {
                case "CMD":
                    return ColorCommand;
                case "ERROR":
                    return ColorError;
                case "WARN":
                    return ColorWarning;
                case "SYSTEM":
                case "POWER":
                    return ColorSystem;
                case "MEM":
                case "PROC":
                    return ColorMemProc;
                default:
                    return ColorInfo;
            }
        }

        private void ClearLogFile()
        {
            try
            {
                if (File.Exists(_logFilePath))
                {
                    File.WriteAllText(_logFilePath, "");
                    _lastLogContent = "";
                }

                dgvLogs.Rows.Clear();
                rtbRawLog.Clear();
                lblLogCount.Text = "Entries: 0";
                lblFileSize.Text = "Size: 0 bytes";

                SyscallHelper.Log("Kernel log cleared by user");
                ShowMessage("Log file cleared", ColorSystem);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"SysLogControl.ClearLogFile failed: {ex.Message}");
                ShowMessage($"Failed to clear log: {ex.Message}", ColorError);
            }
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

                        string header = $"=== MiniOS Kernel Log Export ==={Environment.NewLine}" +
                                       $"Exported: {DateTime.Now:yyyy-MM-dd HH:mm:ss}{Environment.NewLine}" +
                                       $"Entries: {_logEntryCount}{Environment.NewLine}" +
                                       $"================================={Environment.NewLine}{Environment.NewLine}";

                        File.WriteAllText(sfd.FileName, header + content);

                        SyscallHelper.Log($"Kernel log exported to: {sfd.FileName}");
                        ShowMessage($"Log exported to: {Path.GetFileName(sfd.FileName)}", ColorSystem);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"SysLogControl.ExportLog failed: {ex.Message}");
                ShowMessage($"Export failed: {ex.Message}", ColorError);
            }
        }

        #endregion

        #region Utilities

        private static string FormatFileSize(long bytes)
        {
            if (bytes < BYTES_PER_KB)
                return $"{bytes} bytes";
            if (bytes < BYTES_PER_MB)
                return $"{bytes / (double)BYTES_PER_KB:F1} KB";
            return $"{bytes / (double)BYTES_PER_MB:F2} MB";
        }

        private void ShowMessage(string message, Color color)
        {
            lblStatus.Text = message;
            lblStatus.ForeColor = color;

            // Dispose existing timer to prevent accumulation
            _messageResetTimer?.Stop();
            _messageResetTimer?.Dispose();

            _messageResetTimer = new Timer { Interval = MESSAGE_RESET_INTERVAL_MS };
            _messageResetTimer.Tick += (s, ev) =>
            {
                lblStatus.ForeColor = ColorSystem;
                lblStatus.Text = "Log Monitor Active";
                _messageResetTimer.Stop();
            };
            _messageResetTimer.Start();
        }

        #endregion

        #region Internal Types

        private class LogEntry
        {
            public string Timestamp { get; set; }
            public string Type { get; set; }
            public string Message { get; set; }
        }

        #endregion
    }
}
