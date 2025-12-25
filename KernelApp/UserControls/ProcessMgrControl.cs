using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace KernelApp.UserControls
{
    public partial class ProcessMgrControl : UserControl
    {
        // Process data model
        private class SimulatedProcess
        {
            public int PID { get; set; }
            public string ProcessName { get; set; }
            public string Status { get; set; }
            public int MemoryMB { get; set; }
            public int Priority { get; set; }
            public DateTime StartTime { get; set; }
        }

        private List<SimulatedProcess> processList;
        private Random random;
        private int nextPID = 1000;
        private bool isPaused = false;

        // Predefined process names for random selection
        private readonly string[] defaultProcessNames = new string[]
        {
            "shell.exe", "logger.sys", "network.sys", "display.sys",
            "audio.sys", "input.sys", "storage.sys", "crypto.sys",
            "timer.sys", "interrupt.sys", "driver.sys", "service.exe",
            "daemon.sys", "monitor.exe", "cache.sys"
        };

        private readonly string[] statuses = { "Running", "Ready", "Waiting", "Blocked" };

        public ProcessMgrControl()
        {
            InitializeComponent();
            random = new Random();
            processList = new List<SimulatedProcess>();
        }

        private void ProcessMgrControl_Load(object sender, EventArgs e)
        {
            InitializeSystemProcesses();
            RefreshProcessList();
            SyncToKernelState();
            schedulerTimer.Start();
        }

        private void InitializeSystemProcesses()
        {
            // Create initial system-critical processes
            string[] systemProcesses = { "kernel.sys", "init.exe", "scheduler.sys", "memory_mgr.sys", "io_handler.sys" };

            foreach (var name in systemProcesses)
            {
                var proc = new SimulatedProcess
                {
                    PID = nextPID++,
                    ProcessName = name,
                    Status = "Running",
                    MemoryMB = random.Next(50, 200),
                    Priority = random.Next(1, 10),
                    StartTime = DateTime.Now.AddMinutes(-random.Next(1, 60))
                };
                processList.Add(proc);
            }
        }

        private void schedulerTimer_Tick(object sender, EventArgs e)
        {
            if (isPaused || processList.Count == 0) return;

            // Simulate state transitions
            foreach (var proc in processList)
            {
                // kernel.sys always stays running
                if (proc.ProcessName == "kernel.sys") continue;

                if (random.Next(100) < 20)
                {
                    int currentIndex = Array.IndexOf(statuses, proc.Status);
                    int newIndex = (currentIndex + 1) % statuses.Length;
                    proc.Status = statuses[newIndex];
                }

                // Simulate memory fluctuation
                if (random.Next(100) < 15)
                {
                    proc.MemoryMB = Math.Max(10, proc.MemoryMB + random.Next(-20, 30));
                }
            }

            RefreshProcessList();
            SyncToKernelState();
        }

        private void SyncToKernelState()
        {
            // Sync process data to shared kernel state for Memory Manager
            var kernelProcesses = processList.Select(p => new KernelState.ProcessInfo
            {
                PID = p.PID,
                ProcessName = p.ProcessName,
                Status = p.Status,
                MemoryMB = p.MemoryMB,
                Priority = p.Priority
            }).ToList();

            KernelState.UpdateProcessList(kernelProcesses);
        }

        private void RefreshProcessList()
        {
            dgvProcesses.Rows.Clear();

            foreach (var proc in processList.OrderBy(p => p.PID))
            {
                dgvProcesses.Rows.Add(
                    proc.PID,
                    proc.ProcessName,
                    proc.Status,
                    proc.MemoryMB,
                    proc.Priority,
                    proc.StartTime.ToString("HH:mm:ss")
                );
            }

            UpdateStatusBar();
        }

        private void UpdateStatusBar()
        {
            int total = processList.Count;
            int running = processList.Count(p => p.Status == "Running");
            int totalMem = processList.Sum(p => p.MemoryMB);

            lblTotalProcesses.Text = $"Total Processes: {total}";
            lblRunningCount.Text = $"Running: {running}";
            lblTotalMemory.Text = $"Total Memory: {totalMem} MB";
        }

        private void dgvProcesses_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvProcesses.Columns[e.ColumnIndex].Name == "colStatus" && e.Value != null)
            {
                switch (e.Value.ToString())
                {
                    case "Running":
                        e.CellStyle.ForeColor = Color.FromArgb(34, 197, 94);
                        break;
                    case "Ready":
                        e.CellStyle.ForeColor = Color.FromArgb(99, 102, 241);
                        break;
                    case "Waiting":
                        e.CellStyle.ForeColor = Color.FromArgb(251, 191, 36);
                        break;
                    case "Blocked":
                        e.CellStyle.ForeColor = Color.FromArgb(239, 68, 68);
                        break;
                }
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            // Get process name from textbox or generate random
            string processName = txtProcessName.Text.Trim();

            if (string.IsNullOrEmpty(processName))
            {
                processName = defaultProcessNames[random.Next(defaultProcessNames.Length)];
            }
            else
            {
                // Ensure valid extension
                if (!processName.Contains("."))
                {
                    processName += ".exe";
                }
            }

            var proc = new SimulatedProcess
            {
                PID = nextPID++,
                ProcessName = processName,
                Status = "Ready",
                MemoryMB = random.Next(50, 300),
                Priority = random.Next(1, 10),
                StartTime = DateTime.Now
            };

            processList.Add(proc);
            RefreshProcessList();
            SyncToKernelState();

            // Clear textbox after creation
            txtProcessName.Text = "";

            ShowMessage($"Process '{processName}' created (PID: {proc.PID})", Color.FromArgb(34, 197, 94));
        }

        private void btnKill_Click(object sender, EventArgs e)
        {
            if (dgvProcesses.SelectedRows.Count == 0)
            {
                ShowMessage("Select a process to terminate.", Color.FromArgb(251, 191, 36));
                return;
            }

            var selectedRow = dgvProcesses.SelectedRows[0];
            int pid = Convert.ToInt32(selectedRow.Cells["colPID"].Value);
            string name = selectedRow.Cells["colProcessName"].Value.ToString();

            // Prevent killing kernel.sys
            if (name == "kernel.sys")
            {
                ShowMessage("Cannot terminate kernel.sys - system critical!", Color.FromArgb(239, 68, 68));
                return;
            }

            var proc = processList.FirstOrDefault(p => p.PID == pid);
            if (proc != null)
            {
                processList.Remove(proc);
                RefreshProcessList();
                SyncToKernelState();
                ShowMessage($"Process '{name}' (PID: {pid}) terminated.", Color.FromArgb(34, 197, 94));
            }
        }

        private void btnPauseResume_Click(object sender, EventArgs e)
        {
            isPaused = !isPaused;
            btnPauseResume.Text = isPaused ? "> Resume" : "|| Pause";
            btnPauseResume.FillColor = isPaused
                ? Color.FromArgb(34, 197, 94)
                : Color.FromArgb(251, 191, 36);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshProcessList();
            SyncToKernelState();
            ShowMessage("Process list refreshed.", Color.FromArgb(99, 102, 241));
        }

        private void ShowMessage(string message, Color color)
        {
            lblTotalProcesses.Text = message;
            lblTotalProcesses.ForeColor = color;

            var resetTimer = new Timer { Interval = 2500 };
            resetTimer.Tick += (s, ev) =>
            {
                lblTotalProcesses.ForeColor = Color.FromArgb(166, 173, 186);
                UpdateStatusBar();
                resetTimer.Stop();
                resetTimer.Dispose();
            };
            resetTimer.Start();
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            schedulerTimer?.Stop();
            base.OnHandleDestroyed(e);
        }
    }
}
