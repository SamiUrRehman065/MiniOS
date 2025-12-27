using KernelApp.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KernelApp.UserControls
{
    public partial class MemoryVisControl : UserControl
    {
        #region Constants

        private const int TOTAL_MEMORY_MB = 4096;
        private const int MEMORY_CHANGE_THRESHOLD_MB = 50;
        private const int SEGMENT_HEIGHT = 78;
        private const int MIN_SEGMENT_WIDTH = 70;
        private const int MAX_SEGMENT_WIDTH = 180;
        private const int MESSAGE_RESET_INTERVAL_MS = 2500;

        #endregion

        #region Cached Fonts (Prevents GDI Handle Leaks)

        private readonly Font _fontSegmentName = new Font("Consolas", 8F, FontStyle.Bold);
        private readonly Font _fontSegmentPid = new Font("Consolas", 7F);
        private readonly Font _fontSegmentMem = new Font("Consolas", 9F, FontStyle.Bold);
        private readonly Font _fontGridHeader = new Font("Consolas", 10F, FontStyle.Bold);
        private readonly Font _fontGridCell = new Font("Consolas", 10F);

        #endregion

        #region Colors

        private static readonly Color ColorPurple = Color.FromArgb(99, 102, 241);
        private static readonly Color ColorRed = Color.FromArgb(239, 68, 68);
        private static readonly Color ColorGreen = Color.FromArgb(34, 197, 94);
        private static readonly Color ColorYellow = Color.FromArgb(251, 191, 36);
        private static readonly Color ColorGray = Color.FromArgb(166, 173, 186);
        private static readonly Color ColorDarkBg = Color.FromArgb(24, 24, 37);
        private static readonly Color ColorPanelBg = Color.FromArgb(30, 30, 46);
        private static readonly Color ColorSegmentFree = Color.FromArgb(55, 55, 75);

        private readonly Color[] _segmentColors = new Color[]
        {
            ColorPurple,
            ColorRed,
            ColorGreen,
            ColorYellow,
            Color.FromArgb(236, 72, 153),   // Pink
            Color.FromArgb(14, 165, 233),   // Sky Blue
            Color.FromArgb(249, 115, 22),   // Orange
            Color.FromArgb(168, 85, 247),   // Violet
            Color.FromArgb(20, 184, 166),   // Teal
            Color.FromArgb(132, 204, 22)    // Lime
        };

        #endregion

        #region State Tracking

        private string _lastProcessHash = "";
        private int _previousUsedMemory;
        private Timer _messageResetTimer;

        #endregion

        public MemoryVisControl()
        {
            InitializeComponent();
            ApplyDataGridViewStyles();
        }

        #region Event Handlers

        private void MemoryVisControl_Load(object sender, EventArgs e)
        {
            SyscallHelper.EnsureInitialized();
            ForceRefreshMemoryData();
            refreshTimer.Start();

            SyscallHelper.Log("Memory Visualizer module initialized");
        }

        private void refreshTimer_Tick(object sender, EventArgs e)
        {
            RefreshMemoryData();
        }

        private void btnRefreshMemory_Click(object sender, EventArgs e)
        {
            ForceRefreshMemoryData();
            ShowMessage("Memory data refreshed from Process Manager.", ColorPurple);
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            refreshTimer?.Stop();
            _messageResetTimer?.Stop();
            _messageResetTimer?.Dispose();

            SyscallHelper.Log("Memory Visualizer module shutdown");
            base.OnHandleDestroyed(e);
        }

        #endregion

        #region Data Refresh

        private void ForceRefreshMemoryData()
        {
            _lastProcessHash = "";
            RefreshMemoryData();
        }

        private void RefreshMemoryData()
        {
            var currentProcesses = KernelState.GetProcessList();
            string currentHash = BuildProcessHash(currentProcesses);

            // Cache the sum once to avoid multiple LINQ iterations
            int usedMemory = currentProcesses?.Sum(p => p.MemoryMB) ?? 0;

            // Always update metrics and bar (lightweight operations)
            UpdateMemoryMetrics(usedMemory);
            UpdateMemoryBar(usedMemory);

            // Only rebuild heavy UI components if data actually changed
            if (currentHash != _lastProcessHash)
            {
                _lastProcessHash = currentHash;
                RebuildMemorySegments(currentProcesses, usedMemory);
                UpdateProcessGrid(currentProcesses);

                // Log significant memory changes
                if (Math.Abs(usedMemory - _previousUsedMemory) > MEMORY_CHANGE_THRESHOLD_MB)
                {
                    SyscallHelper.Log($"MEMORY_UPDATE: Total used memory changed {_previousUsedMemory} -> {usedMemory} MB");
                    _previousUsedMemory = usedMemory;
                }
            }

            lblLastUpdate.Text = $"Last Update: {DateTime.Now:HH:mm:ss}";
        }

        private string BuildProcessHash(List<KernelState.ProcessInfo> processes)
        {
            if (processes == null || processes.Count == 0)
                return "EMPTY:0";

            // Use StringBuilder for efficient string building
            var sorted = processes.OrderBy(p => p.PID);
            var sb = new StringBuilder();
            sb.Append("COUNT:").Append(processes.Count);

            foreach (var p in sorted)
            {
                sb.Append('|').Append(p.PID).Append(':').Append(p.MemoryMB);
            }

            return sb.ToString();
        }

        #endregion

        #region UI Updates

        private void ApplyDataGridViewStyles()
        {
            // Header style
            dgvProcessMemory.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 45, 65);
            dgvProcessMemory.ColumnHeadersDefaultCellStyle.ForeColor = ColorPurple;
            dgvProcessMemory.ColumnHeadersDefaultCellStyle.Font = _fontGridHeader;
            dgvProcessMemory.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(45, 45, 65);

            // Row style
            dgvProcessMemory.DefaultCellStyle.BackColor = ColorDarkBg;
            dgvProcessMemory.DefaultCellStyle.ForeColor = ColorGray;
            dgvProcessMemory.DefaultCellStyle.Font = _fontGridCell;
            dgvProcessMemory.DefaultCellStyle.SelectionBackColor = Color.FromArgb(55, 55, 85);
            dgvProcessMemory.DefaultCellStyle.SelectionForeColor = Color.White;

            // Alternating row style
            dgvProcessMemory.AlternatingRowsDefaultCellStyle.BackColor = ColorPanelBg;
        }

        private void UpdateMemoryMetrics(int usedMemory)
        {
            int freeMemory = TOTAL_MEMORY_MB - usedMemory;
            double usagePercent = (double)usedMemory / TOTAL_MEMORY_MB * 100;

            lblTotalMemory.Text = $"TOTAL: {TOTAL_MEMORY_MB} MB";
            lblUsedMemory.Text = $"USED: {usedMemory} MB";
            lblFreeMemory.Text = $"FREE: {freeMemory} MB";
            lblUsagePercent.Text = $"USAGE: {usagePercent:F1}%";

            // Change color based on usage level
            if (usagePercent > 80)
            {
                lblUsagePercent.ForeColor = ColorRed;
                if (usagePercent > 90)
                {
                    SyscallHelper.Log($"MEMORY_WARNING: High memory usage detected ({usagePercent:F1}%)");
                }
            }
            else if (usagePercent > 60)
            {
                lblUsagePercent.ForeColor = ColorYellow;
            }
            else
            {
                lblUsagePercent.ForeColor = ColorGreen;
            }
        }

        private void UpdateMemoryBar(int usedMemory)
        {
            double usagePercent = (double)usedMemory / TOTAL_MEMORY_MB * 100;

            // Calculate bar width
            int availableWidth = pnlMemoryBarOuter.Width - 6;
            int barWidth = (int)(availableWidth * usagePercent / 100);

            pnlMemoryBarInner.Width = Math.Max(0, barWidth);
            lblBarPercent.Text = $"{usagePercent:F1}%";

            // Change bar color based on usage
            if (usagePercent > 80)
            {
                pnlMemoryBarInner.BackColor = ColorRed;
            }
            else if (usagePercent > 60)
            {
                pnlMemoryBarInner.BackColor = ColorYellow;
            }
            else
            {
                pnlMemoryBarInner.BackColor = ColorPurple;
            }
        }

        private void UpdateProcessGrid(List<KernelState.ProcessInfo> processes)
        {
            dgvProcessMemory.SuspendLayout();
            dgvProcessMemory.Rows.Clear();

            if (processes != null && processes.Count > 0)
            {
                foreach (var proc in processes.OrderByDescending(p => p.MemoryMB))
                {
                    double memPercent = (double)proc.MemoryMB / TOTAL_MEMORY_MB * 100;
                    dgvProcessMemory.Rows.Add(
                        proc.PID,
                        proc.ProcessName,
                        proc.MemoryMB,
                        $"{memPercent:F2}%"
                    );
                }
            }

            dgvProcessMemory.ResumeLayout(true);
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
                lblStatus.ForeColor = ColorGreen;
                lblStatus.Text = "Memory Manager Active";
                _messageResetTimer.Stop();
            };
            _messageResetTimer.Start();
        }

        #endregion

        #region Memory Segment Visualization

        private void RebuildMemorySegments(List<KernelState.ProcessInfo> processes, int usedMemory)
        {
            pnlSegmentsContainer.SuspendLayout();

            // Properly dispose and clear all existing controls
            while (pnlSegmentsContainer.Controls.Count > 0)
            {
                var ctrl = pnlSegmentsContainer.Controls[0];
                pnlSegmentsContainer.Controls.RemoveAt(0);
                ctrl.Dispose();
            }

            // Handle empty process list
            if (processes == null || processes.Count == 0)
            {
                var freeSegment = CreateFreeMemoryBlock(TOTAL_MEMORY_MB, 150);
                pnlSegmentsContainer.Controls.Add(freeSegment);
                pnlSegmentsContainer.ResumeLayout(true);
                return;
            }

            int colorIndex = 0;
            int freeMemory = TOTAL_MEMORY_MB - usedMemory;
            int availableWidth = pnlSegmentsContainer.Width - 30;

            // Create segments for each process (sorted by memory descending)
            foreach (var proc in processes.OrderByDescending(p => p.MemoryMB))
            {
                double memPercent = (double)proc.MemoryMB / TOTAL_MEMORY_MB;
                int segmentWidth = Math.Max(MIN_SEGMENT_WIDTH, Math.Min(MAX_SEGMENT_WIDTH, (int)(availableWidth * memPercent)));

                var segmentPanel = CreateSegmentBlock(proc, _segmentColors[colorIndex % _segmentColors.Length], segmentWidth);
                pnlSegmentsContainer.Controls.Add(segmentPanel);

                colorIndex++;
            }

            // Add free memory segment if there's space available
            if (freeMemory > 0)
            {
                double freePercent = (double)freeMemory / TOTAL_MEMORY_MB;
                int freeWidth = Math.Max(MIN_SEGMENT_WIDTH, Math.Min(MAX_SEGMENT_WIDTH, (int)(availableWidth * freePercent)));

                var freeSegment = CreateFreeMemoryBlock(freeMemory, freeWidth);
                pnlSegmentsContainer.Controls.Add(freeSegment);
            }

            pnlSegmentsContainer.ResumeLayout(true);
        }

        private Panel CreateSegmentBlock(KernelState.ProcessInfo proc, Color color, int width)
        {
            var panel = new Panel
            {
                Width = width,
                Height = SEGMENT_HEIGHT,
                BackColor = color,
                Margin = new Padding(2),
                Padding = new Padding(2)
            };

            var lblName = new Label
            {
                Text = TruncateText(proc.ProcessName, 14),
                Font = _fontSegmentName,
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Dock = DockStyle.Top,
                Height = 20,
                TextAlign = ContentAlignment.MiddleCenter,
                AutoEllipsis = true
            };

            var lblPid = new Label
            {
                Text = $"PID: {proc.PID}",
                Font = _fontSegmentPid,
                ForeColor = Color.FromArgb(220, 220, 220),
                BackColor = Color.Transparent,
                Dock = DockStyle.Top,
                Height = 16,
                TextAlign = ContentAlignment.MiddleCenter
            };

            var lblMem = new Label
            {
                Text = $"{proc.MemoryMB} MB",
                Font = _fontSegmentMem,
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Dock = DockStyle.Bottom,
                Height = 20,
                TextAlign = ContentAlignment.MiddleCenter
            };

            panel.Controls.Add(lblMem);
            panel.Controls.Add(lblPid);
            panel.Controls.Add(lblName);

            return panel;
        }

        private Panel CreateFreeMemoryBlock(int freeMB, int width)
        {
            var panel = new Panel
            {
                Width = width,
                Height = SEGMENT_HEIGHT,
                BackColor = ColorSegmentFree,
                Margin = new Padding(2),
                Padding = new Padding(2)
            };

            var lblName = new Label
            {
                Text = "FREE",
                Font = _fontSegmentName,
                ForeColor = ColorGreen,
                BackColor = Color.Transparent,
                Dock = DockStyle.Top,
                Height = 22,
                TextAlign = ContentAlignment.MiddleCenter
            };

            var lblMem = new Label
            {
                Text = $"{freeMB} MB",
                Font = _fontSegmentMem,
                ForeColor = ColorGreen,
                BackColor = Color.Transparent,
                Dock = DockStyle.Bottom,
                Height = 22,
                TextAlign = ContentAlignment.MiddleCenter
            };

            panel.Controls.Add(lblMem);
            panel.Controls.Add(lblName);

            return panel;
        }

        private static string TruncateText(string text, int maxLength)
        {
            if (string.IsNullOrEmpty(text)) return text;
            return text.Length <= maxLength ? text : text.Substring(0, maxLength - 2) + "..";
        }

        #endregion

        #region Cleanup

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Dispose cached fonts
                _fontSegmentName?.Dispose();
                _fontSegmentPid?.Dispose();
                _fontSegmentMem?.Dispose();
                _fontGridHeader?.Dispose();
                _fontGridCell?.Dispose();

                // Dispose message timer
                _messageResetTimer?.Dispose();

                components?.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion
    }

    // Shared Kernel State - Static class to share data between modules
    public static class KernelState
    {
        private static List<ProcessInfo> _processList = new List<ProcessInfo>();
        private static readonly object _lock = new object();

        public static void UpdateProcessList(List<ProcessInfo> processes)
        {
            lock (_lock)
            {
                _processList.Clear();

                if (processes != null)
                {
                    foreach (var p in processes)
                    {
                        _processList.Add(new ProcessInfo
                        {
                            PID = p.PID,
                            ProcessName = p.ProcessName,
                            Status = p.Status,
                            MemoryMB = p.MemoryMB,
                            Priority = p.Priority
                        });
                    }
                }
            }
        }

        public static List<ProcessInfo> GetProcessList()
        {
            lock (_lock)
            {
                var result = new List<ProcessInfo>(_processList.Count);

                foreach (var p in _processList)
                {
                    result.Add(new ProcessInfo
                    {
                        PID = p.PID,
                        ProcessName = p.ProcessName,
                        Status = p.Status,
                        MemoryMB = p.MemoryMB,
                        Priority = p.Priority
                    });
                }

                return result;
            }
        }

        public static void Clear()
        {
            lock (_lock)
            {
                _processList.Clear();
            }
        }

        public class ProcessInfo
        {
            public int PID { get; set; }
            public string ProcessName { get; set; }
            public string Status { get; set; }
            public int MemoryMB { get; set; }
            public int Priority { get; set; }
        }
    }
}
