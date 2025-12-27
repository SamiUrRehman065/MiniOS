using KernelApp.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace KernelApp.UserControls
{
    public partial class ProcessMgrControl : UserControl
    {
        #region Constants

        private const int MESSAGE_RESET_INTERVAL_MS = 2500;
        private const int INITIAL_PID = 1000;
        private const int MIN_MEMORY_MB = 10;
        private const int STATE_CHANGE_PROBABILITY = 20;
        private const int MEMORY_CHANGE_PROBABILITY = 15;
        private const int MEMORY_FLUCTUATION_MIN = -20;
        private const int MEMORY_FLUCTUATION_MAX = 30;
        private const int SIGNIFICANT_MEMORY_CHANGE_MB = 10;

        #endregion

        #region Colors

        private static readonly Color ColorGreen = Color.FromArgb(34, 197, 94);
        private static readonly Color ColorPurple = Color.FromArgb(99, 102, 241);
        private static readonly Color ColorYellow = Color.FromArgb(251, 191, 36);
        private static readonly Color ColorRed = Color.FromArgb(239, 68, 68);
        private static readonly Color ColorGray = Color.FromArgb(166, 173, 186);

        #endregion

        #region Process Data

        private readonly List<SimulatedProcess> _processList;
        private readonly Random _random;
        private int _nextPID;
        private bool _isPaused;
        private Timer _messageResetTimer;

        private static readonly string[] DefaultProcessNames =
        {
            "shell.exe", "logger.sys", "network.sys", "display.sys",
            "audio.sys", "input.sys", "storage.sys", "crypto.sys",
            "timer.sys", "interrupt.sys", "driver.sys", "service.exe",
            "daemon.sys", "monitor.exe", "cache.sys"
        };

        private static readonly string[] SystemProcessNames =
        {
            "kernel.sys", "init.exe", "scheduler.sys", "memory_mgr.sys", "io_handler.sys"
        };

        private static readonly string[] ProcessStatuses = { "Running", "Ready", "Waiting", "Blocked" };

        #endregion

        public ProcessMgrControl()
        {
            InitializeComponent();
            _random = new Random();
            _processList = new List<SimulatedProcess>();
            _nextPID = INITIAL_PID;
        }

        #region Event Handlers

        private void ProcessMgrControl_Load(object sender, EventArgs e)
        {
            SyscallHelper.EnsureInitialized();
            InitializeSystemProcesses();
            RefreshProcessList();
            SyncToKernelState();
            schedulerTimer.Start();

            SyscallHelper.Log("Process Manager module initialized");
        }

        private void schedulerTimer_Tick(object sender, EventArgs e)
        {
            if (_isPaused || _processList.Count == 0) return;

            SimulateProcessStateChanges();
            RefreshProcessList();
            SyncToKernelState();
        }

        private void dgvProcesses_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvProcesses.Columns[e.ColumnIndex].Name != "colStatus" || e.Value == null)
                return;

            e.CellStyle.ForeColor = GetStatusColor(e.Value.ToString());
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            CreateNewProcess();
        }

        private void btnKill_Click(object sender, EventArgs e)
        {
            KillSelectedProcess();
        }

        private void btnPauseResume_Click(object sender, EventArgs e)
        {
            TogglePauseResume();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshProcessList();
            SyncToKernelState();
            ShowMessage("Process list refreshed.", ColorPurple);
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            schedulerTimer?.Stop();
            _messageResetTimer?.Stop();
            _messageResetTimer?.Dispose();

            FreeAllProcessMemory();

            SyscallHelper.Log("Process Manager module shutdown - all memory freed");
            base.OnHandleDestroyed(e);
        }

        #endregion

        #region Process Management

        private void InitializeSystemProcesses()
        {
            foreach (var name in SystemProcessNames)
            {
                int memoryMB = _random.Next(50, 200);
                IntPtr memPtr = SyscallHelper.MemAlloc(memoryMB * 1024, logAllocation: false);

                var proc = new SimulatedProcess
                {
                    PID = _nextPID++,
                    ProcessName = name,
                    Status = "Running",
                    MemoryMB = memoryMB,
                    Priority = _random.Next(1, 10),
                    StartTime = DateTime.Now.AddMinutes(-_random.Next(1, 60)),
                    AllocatedMemoryPtr = memPtr
                };

                _processList.Add(proc);
                SyscallHelper.Log($"PROC_CREATE: {name} (PID: {proc.PID}) allocated {memoryMB} MB");
            }
        }

        private void CreateNewProcess()
        {
            string processName = GetProcessName();
            int memoryMB = _random.Next(50, 300);

            IntPtr memPtr = SyscallHelper.MemAlloc(memoryMB * 1024, logAllocation: false);

            var proc = new SimulatedProcess
            {
                PID = _nextPID++,
                ProcessName = processName,
                Status = "Ready",
                MemoryMB = memoryMB,
                Priority = _random.Next(1, 10),
                StartTime = DateTime.Now,
                AllocatedMemoryPtr = memPtr
            };

            _processList.Add(proc);
            RefreshProcessList();
            SyncToKernelState();

            txtProcessName.Text = "";

            SyscallHelper.Log($"PROC_CREATE: {processName} (PID: {proc.PID}) created with {memoryMB} MB");
            ShowMessage($"Process '{processName}' created (PID: {proc.PID})", ColorGreen);
        }

        private string GetProcessName()
        {
            string name = txtProcessName.Text.Trim();

            if (string.IsNullOrEmpty(name))
            {
                return DefaultProcessNames[_random.Next(DefaultProcessNames.Length)];
            }

            // Ensure valid extension
            if (!name.Contains("."))
            {
                name += ".exe";
            }

            return name;
        }

        private void KillSelectedProcess()
        {
            if (dgvProcesses.SelectedRows.Count == 0)
            {
                ShowMessage("Select a process to terminate.", ColorYellow);
                return;
            }

            var selectedRow = dgvProcesses.SelectedRows[0];
            int pid = Convert.ToInt32(selectedRow.Cells["colPID"].Value);
            string name = selectedRow.Cells["colProcessName"].Value.ToString();

            // Prevent killing kernel.sys
            if (name == "kernel.sys")
            {
                ShowMessage("Cannot terminate kernel.sys - system critical!", ColorRed);
                SyscallHelper.Log("PROC_KILL_DENIED: Attempted to kill kernel.sys");
                return;
            }

            var proc = _processList.FirstOrDefault(p => p.PID == pid);
            if (proc != null)
            {
                FreeProcessMemory(proc);
                _processList.Remove(proc);
                RefreshProcessList();
                SyncToKernelState();

                SyscallHelper.Log($"PROC_KILL: {name} (PID: {pid}) terminated, {proc.MemoryMB} MB freed");
                ShowMessage($"Process '{name}' (PID: {pid}) terminated.", ColorGreen);
            }
        }

        private void TogglePauseResume()
        {
            _isPaused = !_isPaused;

            btnPauseResume.Text = _isPaused ? "> Resume" : "|| Pause";
            btnPauseResume.FillColor = _isPaused ? ColorGreen : ColorYellow;

            SyscallHelper.Log($"SCHEDULER: {(_isPaused ? "Paused" : "Resumed")}");
        }

        private void SimulateProcessStateChanges()
        {
            foreach (var proc in _processList)
            {
                // kernel.sys always stays running
                if (proc.ProcessName == "kernel.sys") continue;

                SimulateStateTransition(proc);
                SimulateMemoryFluctuation(proc);
            }
        }

        private void SimulateStateTransition(SimulatedProcess proc)
        {
            if (_random.Next(100) >= STATE_CHANGE_PROBABILITY) return;

            int currentIndex = Array.IndexOf(ProcessStatuses, proc.Status);
            int newIndex = (currentIndex + 1) % ProcessStatuses.Length;
            string oldStatus = proc.Status;
            proc.Status = ProcessStatuses[newIndex];

            if (oldStatus != proc.Status)
            {
                SyscallHelper.Log($"PROC_STATE: {proc.ProcessName} (PID: {proc.PID}) {oldStatus} -> {proc.Status}");
            }
        }

        private void SimulateMemoryFluctuation(SimulatedProcess proc)
        {
            if (_random.Next(100) >= MEMORY_CHANGE_PROBABILITY) return;

            int oldMem = proc.MemoryMB;
            proc.MemoryMB = Math.Max(MIN_MEMORY_MB, proc.MemoryMB + _random.Next(MEMORY_FLUCTUATION_MIN, MEMORY_FLUCTUATION_MAX));

            if (Math.Abs(oldMem - proc.MemoryMB) > SIGNIFICANT_MEMORY_CHANGE_MB)
            {
                SyscallHelper.Log($"PROC_MEM: {proc.ProcessName} memory changed {oldMem} -> {proc.MemoryMB} MB");
            }
        }

        #endregion

        #region Memory Management

        private void FreeProcessMemory(SimulatedProcess proc)
        {
            if (proc.AllocatedMemoryPtr != IntPtr.Zero)
            {
                SyscallHelper.MemFree(proc.AllocatedMemoryPtr, logFree: false);
            }
        }

        private void FreeAllProcessMemory()
        {
            foreach (var proc in _processList)
            {
                FreeProcessMemory(proc);
            }
        }

        #endregion

        #region Kernel State Sync

        private void SyncToKernelState()
        {
            var kernelProcesses = _processList.Select(p => new KernelState.ProcessInfo
            {
                PID = p.PID,
                ProcessName = p.ProcessName,
                Status = p.Status,
                MemoryMB = p.MemoryMB,
                Priority = p.Priority
            }).ToList();

            KernelState.UpdateProcessList(kernelProcesses);
        }

        #endregion

        #region UI Updates

        private void RefreshProcessList()
        {
            dgvProcesses.SuspendLayout();
            dgvProcesses.Rows.Clear();

            foreach (var proc in _processList.OrderBy(p => p.PID))
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

            dgvProcesses.ResumeLayout(true);
            UpdateStatusBar();
        }

        private void UpdateStatusBar()
        {
            int total = _processList.Count;
            int running = _processList.Count(p => p.Status == "Running");
            int totalMem = _processList.Sum(p => p.MemoryMB);

            lblTotalProcesses.Text = $"Total Processes: {total}";
            lblRunningCount.Text = $"Running: {running}";
            lblTotalMemory.Text = $"Total Memory: {totalMem} MB";
        }

        private static Color GetStatusColor(string status)
        {
            switch (status)
            {
                case "Running":
                    return ColorGreen;
                case "Ready":
                    return ColorPurple;
                case "Waiting":
                    return ColorYellow;
                case "Blocked":
                    return ColorRed;
                default:
                    return ColorGray;
            }
        }

        private void ShowMessage(string message, Color color)
        {
            lblTotalProcesses.Text = message;
            lblTotalProcesses.ForeColor = color;

            // Dispose existing timer to prevent accumulation
            _messageResetTimer?.Stop();
            _messageResetTimer?.Dispose();

            _messageResetTimer = new Timer { Interval = MESSAGE_RESET_INTERVAL_MS };
            _messageResetTimer.Tick += (s, ev) =>
            {
                lblTotalProcesses.ForeColor = ColorGray;
                UpdateStatusBar();
                _messageResetTimer.Stop();
            };
            _messageResetTimer.Start();
        }

        #endregion

        #region Internal Types

        private class SimulatedProcess
        {
            public int PID { get; set; }
            public string ProcessName { get; set; }
            public string Status { get; set; }
            public int MemoryMB { get; set; }
            public int Priority { get; set; }
            public DateTime StartTime { get; set; }
            public IntPtr AllocatedMemoryPtr { get; set; }
        }

        #endregion
    }
}
