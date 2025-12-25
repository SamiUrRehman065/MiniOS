using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace KernelApp.UserControls
{
    public partial class MemoryVisControl : UserControl
    {
        // Memory configuration
        private const int TOTAL_MEMORY_MB = 4096; // 4 GB simulated RAM

        // Color palette for memory segments
        private readonly Color[] segmentColors = new Color[]
        {
            Color.FromArgb(99, 102, 241),   // Purple
            Color.FromArgb(239, 68, 68),    // Red
            Color.FromArgb(34, 197, 94),    // Green
            Color.FromArgb(251, 191, 36),   // Yellow
            Color.FromArgb(236, 72, 153),   // Pink
            Color.FromArgb(14, 165, 233),   // Sky Blue
            Color.FromArgb(249, 115, 22),   // Orange
            Color.FromArgb(168, 85, 247),   // Violet
            Color.FromArgb(20, 184, 166),   // Teal
            Color.FromArgb(132, 204, 22)    // Lime
        };

        // Track previous state to detect changes
        private string lastProcessHash = "";

        public MemoryVisControl()
        {
            InitializeComponent();
            ApplyDataGridViewStyles();
        }

        private void MemoryVisControl_Load(object sender, EventArgs e)
        {
            ForceRefreshMemoryData();
            refreshTimer.Start();
        }

        private void ApplyDataGridViewStyles()
        {
            // Header style
            dgvProcessMemory.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 45, 65);
            dgvProcessMemory.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(99, 102, 241);
            dgvProcessMemory.ColumnHeadersDefaultCellStyle.Font = new Font("Consolas", 10F, FontStyle.Bold);
            dgvProcessMemory.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(45, 45, 65);

            // Row style
            dgvProcessMemory.DefaultCellStyle.BackColor = Color.FromArgb(24, 24, 37);
            dgvProcessMemory.DefaultCellStyle.ForeColor = Color.FromArgb(166, 173, 186);
            dgvProcessMemory.DefaultCellStyle.Font = new Font("Consolas", 10F);
            dgvProcessMemory.DefaultCellStyle.SelectionBackColor = Color.FromArgb(55, 55, 85);
            dgvProcessMemory.DefaultCellStyle.SelectionForeColor = Color.White;

            // Alternating row style
            dgvProcessMemory.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(30, 30, 46);
        }

        private void refreshTimer_Tick(object sender, EventArgs e)
        {
            RefreshMemoryData();
        }

        private void btnRefreshMemory_Click(object sender, EventArgs e)
        {
            ForceRefreshMemoryData();
            ShowMessage("Memory data refreshed from Process Manager.", Color.FromArgb(99, 102, 241));
        }

        private void ForceRefreshMemoryData()
        {
            // Force full rebuild by clearing hash
            lastProcessHash = "";
            RefreshMemoryData();
        }

        private void RefreshMemoryData()
        {
            // Get FRESH process data directly from KernelState each time
            var currentProcesses = KernelState.GetProcessList();

            // Build hash including count and all PIDs to detect ANY change
            string currentHash = BuildProcessHash(currentProcesses);

            // Always update metrics and bar (lightweight operations)
            UpdateMemoryMetrics(currentProcesses);
            UpdateMemoryBar(currentProcesses);

            // Only rebuild heavy UI components if data actually changed
            if (currentHash != lastProcessHash)
            {
                lastProcessHash = currentHash;
                RebuildMemorySegments(currentProcesses);
                UpdateProcessGrid(currentProcesses);
            }

            lblLastUpdate.Text = $"Last Update: {DateTime.Now:HH:mm:ss}";
        }

        private string BuildProcessHash(List<KernelState.ProcessInfo> processes)
        {
            if (processes == null || processes.Count == 0)
                return "EMPTY:0";

            // Include count and all PID:Memory pairs sorted by PID
            var sorted = processes.OrderBy(p => p.PID).ToList();
            var pairs = string.Join("|", sorted.Select(p => $"{p.PID}:{p.MemoryMB}"));
            return $"COUNT:{processes.Count}|{pairs}";
        }

        private void UpdateMemoryMetrics(List<KernelState.ProcessInfo> processes)
        {
            int usedMemory = processes?.Sum(p => p.MemoryMB) ?? 0;
            int freeMemory = TOTAL_MEMORY_MB - usedMemory;
            double usagePercent = (double)usedMemory / TOTAL_MEMORY_MB * 100;

            lblTotalMemory.Text = $"TOTAL: {TOTAL_MEMORY_MB} MB";
            lblUsedMemory.Text = $"USED: {usedMemory} MB";
            lblFreeMemory.Text = $"FREE: {freeMemory} MB";
            lblUsagePercent.Text = $"USAGE: {usagePercent:F1}%";

            // Change color based on usage level
            if (usagePercent > 80)
            {
                lblUsagePercent.ForeColor = Color.FromArgb(239, 68, 68); // Red
            }
            else if (usagePercent > 60)
            {
                lblUsagePercent.ForeColor = Color.FromArgb(251, 191, 36); // Yellow
            }
            else
            {
                lblUsagePercent.ForeColor = Color.FromArgb(34, 197, 94); // Green
            }
        }

        private void UpdateMemoryBar(List<KernelState.ProcessInfo> processes)
        {
            int usedMemory = processes?.Sum(p => p.MemoryMB) ?? 0;
            double usagePercent = (double)usedMemory / TOTAL_MEMORY_MB * 100;

            // Calculate bar width
            int availableWidth = pnlMemoryBarOuter.Width - 6;
            int barWidth = (int)(availableWidth * usagePercent / 100);

            pnlMemoryBarInner.Width = Math.Max(0, barWidth);
            lblBarPercent.Text = $"{usagePercent:F1}%";

            // Change bar color based on usage
            if (usagePercent > 80)
            {
                pnlMemoryBarInner.BackColor = Color.FromArgb(239, 68, 68);
            }
            else if (usagePercent > 60)
            {
                pnlMemoryBarInner.BackColor = Color.FromArgb(251, 191, 36);
            }
            else
            {
                pnlMemoryBarInner.BackColor = Color.FromArgb(99, 102, 241);
            }
        }

        private void RebuildMemorySegments(List<KernelState.ProcessInfo> processes)
        {
            // Suspend layout to prevent flickering
            pnlSegmentsContainer.SuspendLayout();

            // Properly dispose and clear all existing controls
            while (pnlSegmentsContainer.Controls.Count > 0)
            {
                var ctrl = pnlSegmentsContainer.Controls[0];
                pnlSegmentsContainer.Controls.RemoveAt(0);
                ctrl.Dispose();
            }

            int segmentHeight = 78; // Fixed height for consistency

            // Handle empty process list
            if (processes == null || processes.Count == 0)
            {
                var freeSegment = CreateFreeMemoryBlock(TOTAL_MEMORY_MB, 150, segmentHeight);
                pnlSegmentsContainer.Controls.Add(freeSegment);
                pnlSegmentsContainer.ResumeLayout(true);
                return;
            }

            int colorIndex = 0;
            int totalUsed = processes.Sum(p => p.MemoryMB);
            int freeMemory = TOTAL_MEMORY_MB - totalUsed;
            int availableWidth = pnlSegmentsContainer.Width - 30;

            // Create segments for each process (sorted by memory descending)
            foreach (var proc in processes.OrderByDescending(p => p.MemoryMB))
            {
                double memPercent = (double)proc.MemoryMB / TOTAL_MEMORY_MB;
                int segmentWidth = Math.Max(70, Math.Min(180, (int)(availableWidth * memPercent)));

                var segmentPanel = CreateSegmentBlock(proc, segmentColors[colorIndex % segmentColors.Length], segmentWidth, segmentHeight);
                pnlSegmentsContainer.Controls.Add(segmentPanel);

                colorIndex++;
            }

            // Add free memory segment if there's space available
            if (freeMemory > 0)
            {
                double freePercent = (double)freeMemory / TOTAL_MEMORY_MB;
                int freeWidth = Math.Max(70, Math.Min(180, (int)(availableWidth * freePercent)));

                var freeSegment = CreateFreeMemoryBlock(freeMemory, freeWidth, segmentHeight);
                pnlSegmentsContainer.Controls.Add(freeSegment);
            }

            pnlSegmentsContainer.ResumeLayout(true);
        }

        private Panel CreateSegmentBlock(KernelState.ProcessInfo proc, Color color, int width, int height)
        {
            var panel = new Panel
            {
                Width = width,
                Height = height,
                BackColor = color,
                Margin = new Padding(2),
                Padding = new Padding(2)
            };

            var lblName = new Label
            {
                Text = TruncateText(proc.ProcessName, 14),
                Font = new Font("Consolas", 8F, FontStyle.Bold),
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
                Font = new Font("Consolas", 7F),
                ForeColor = Color.FromArgb(220, 220, 220),
                BackColor = Color.Transparent,
                Dock = DockStyle.Top,
                Height = 16,
                TextAlign = ContentAlignment.MiddleCenter
            };

            var lblMem = new Label
            {
                Text = $"{proc.MemoryMB} MB",
                Font = new Font("Consolas", 9F, FontStyle.Bold),
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

        private Panel CreateFreeMemoryBlock(int freeMB, int width, int height)
        {
            var panel = new Panel
            {
                Width = width,
                Height = height,
                BackColor = Color.FromArgb(55, 55, 75),
                Margin = new Padding(2),
                Padding = new Padding(2)
            };

            var lblName = new Label
            {
                Text = "FREE",
                Font = new Font("Consolas", 8F, FontStyle.Bold),
                ForeColor = Color.FromArgb(34, 197, 94),
                BackColor = Color.Transparent,
                Dock = DockStyle.Top,
                Height = 22,
                TextAlign = ContentAlignment.MiddleCenter
            };

            var lblMem = new Label
            {
                Text = $"{freeMB} MB",
                Font = new Font("Consolas", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(34, 197, 94),
                BackColor = Color.Transparent,
                Dock = DockStyle.Bottom,
                Height = 22,
                TextAlign = ContentAlignment.MiddleCenter
            };

            panel.Controls.Add(lblMem);
            panel.Controls.Add(lblName);

            return panel;
        }

        private string TruncateText(string text, int maxLength)
        {
            if (string.IsNullOrEmpty(text)) return text;
            return text.Length <= maxLength ? text : text.Substring(0, maxLength - 2) + "..";
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

            var resetTimer = new Timer { Interval = 2500 };
            resetTimer.Tick += (s, ev) =>
            {
                lblStatus.ForeColor = Color.FromArgb(34, 197, 94);
                lblStatus.Text = "Memory Manager Active";
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
                // Completely replace with fresh copy
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
                // Return a completely fresh copy
                var result = new List<ProcessInfo>();

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
