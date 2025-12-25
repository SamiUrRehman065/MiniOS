using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace KernelApp.UserControls
{
    public partial class ProcessMgrControl : UserControl
    {
        // Syscall DLL imports
        [DllImport("syscall.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "Sys_Init")]
        private static extern void Native_Sys_Init();

        [DllImport("syscall.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "Sys_Log", CharSet = CharSet.Ansi)]
        private static extern void Native_Sys_Log(string message);

        [DllImport("syscall.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "Sys_MemAlloc")]
        private static extern IntPtr Native_Sys_MemAlloc(int sizeBytes);

        [DllImport("syscall.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "Sys_MemFree")]
        private static extern int Native_Sys_MemFree(IntPtr memPtr);

        [DllImport("syscall.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "Sys_Sleep")]
        private static extern void Native_Sys_Sleep(int milliseconds);

        // Track if syscall DLL is available
        private static bool syscallAvailable = false;
        private static bool syscallChecked = false;

        // Process data model
        private class SimulatedProcess
        {
            public int PID { get; set; }
            public string ProcessName { get; set; }
            public string Status { get; set; }
            public int MemoryMB { get; set; }
            public int Priority { get; set; }
            public DateTime StartTime { get; set; }
            public IntPtr AllocatedMemoryPtr { get; set; } // Track native memory allocation
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
            CheckSyscallAvailability();
            InitializeSystemProcesses();
            RefreshProcessList();
            SyncToKernelState();
            schedulerTimer.Start();

            Sys_Log("Process Manager module initialized");
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

        /// <summary>
        /// Allocate memory using native syscall (simulates kernel memory allocation)
        /// </summary>
        private IntPtr Sys_MemAlloc(int sizeBytes)
        {
            if (!syscallChecked) CheckSyscallAvailability();

            if (syscallAvailable)
            {
                try
                {
                    IntPtr ptr = Native_Sys_MemAlloc(sizeBytes);
                    if (ptr != IntPtr.Zero)
                    {
                        Sys_Log($"MEMALLOC: Allocated {sizeBytes} bytes at 0x{ptr.ToInt32():X8}");
                    }
                    return ptr;
                }
                catch { }
            }

            // Fallback: return simulated pointer (non-zero to indicate success)
            return new IntPtr(random.Next(0x10000, 0x7FFFFFFF));
        }

        /// <summary>
        /// Free memory using native syscall
        /// </summary>
        private bool Sys_MemFree(IntPtr memPtr)
        {
            if (!syscallChecked) CheckSyscallAvailability();

            if (memPtr == IntPtr.Zero) return false;

            if (syscallAvailable)
            {
                try
                {
                    int result = Native_Sys_MemFree(memPtr);
                    if (result != 0)
                    {
                        Sys_Log($"MEMFREE: Freed memory at 0x{memPtr.ToInt32():X8}");
                    }
                    return result != 0;
                }
                catch { }
            }

            // Fallback: always succeed in simulation
            return true;
        }

        /// <summary>
        /// Sleep using native syscall
        /// </summary>
        private void Sys_Sleep(int milliseconds)
        {
            if (!syscallChecked) CheckSyscallAvailability();

            if (syscallAvailable)
            {
                try
                {
                    Native_Sys_Sleep(milliseconds);
                    return;
                }
                catch { }
            }

            System.Threading.Thread.Sleep(milliseconds);
        }

        private void InitializeSystemProcesses()
        {
            // Create initial system-critical processes
            string[] systemProcesses = { "kernel.sys", "init.exe", "scheduler.sys", "memory_mgr.sys", "io_handler.sys" };

            foreach (var name in systemProcesses)
            {
                int memoryMB = random.Next(50, 200);
                
                // Allocate memory for the process using syscall
                IntPtr memPtr = Sys_MemAlloc(memoryMB * 1024); // Simulate KB allocation

                var proc = new SimulatedProcess
                {
                    PID = nextPID++,
                    ProcessName = name,
                    Status = "Running",
                    MemoryMB = memoryMB,
                    Priority = random.Next(1, 10),
                    StartTime = DateTime.Now.AddMinutes(-random.Next(1, 60)),
                    AllocatedMemoryPtr = memPtr
                };
                processList.Add(proc);

                Sys_Log($"PROC_CREATE: {name} (PID: {proc.PID}) allocated {memoryMB} MB");
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
                    string oldStatus = proc.Status;
                    proc.Status = statuses[newIndex];
                    
                    // Log state transitions
                    if (oldStatus != proc.Status)
                    {
                        Sys_Log($"PROC_STATE: {proc.ProcessName} (PID: {proc.PID}) {oldStatus} -> {proc.Status}");
                    }
                }

                // Simulate memory fluctuation
                if (random.Next(100) < 15)
                {
                    int oldMem = proc.MemoryMB;
                    proc.MemoryMB = Math.Max(10, proc.MemoryMB + random.Next(-20, 30));
                    
                    // Log significant memory changes
                    if (Math.Abs(oldMem - proc.MemoryMB) > 10)
                    {
                        Sys_Log($"PROC_MEM: {proc.ProcessName} memory changed {oldMem} -> {proc.MemoryMB} MB");
                    }
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

            int memoryMB = random.Next(50, 300);

            // Allocate memory using native syscall
            IntPtr memPtr = Sys_MemAlloc(memoryMB * 1024);

            var proc = new SimulatedProcess
            {
                PID = nextPID++,
                ProcessName = processName,
                Status = "Ready",
                MemoryMB = memoryMB,
                Priority = random.Next(1, 10),
                StartTime = DateTime.Now,
                AllocatedMemoryPtr = memPtr
            };

            processList.Add(proc);
            RefreshProcessList();
            SyncToKernelState();

            // Clear textbox after creation
            txtProcessName.Text = "";

            Sys_Log($"PROC_CREATE: {processName} (PID: {proc.PID}) created with {memoryMB} MB");
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
                Sys_Log($"PROC_KILL_DENIED: Attempted to kill kernel.sys");
                return;
            }

            var proc = processList.FirstOrDefault(p => p.PID == pid);
            if (proc != null)
            {
                // Free the allocated memory using syscall
                if (proc.AllocatedMemoryPtr != IntPtr.Zero)
                {
                    Sys_MemFree(proc.AllocatedMemoryPtr);
                }

                processList.Remove(proc);
                RefreshProcessList();
                SyncToKernelState();

                Sys_Log($"PROC_KILL: {name} (PID: {pid}) terminated, {proc.MemoryMB} MB freed");
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

            Sys_Log($"SCHEDULER: {(isPaused ? "Paused" : "Resumed")}");
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

            // Free all allocated memory when control is destroyed
            foreach (var proc in processList)
            {
                if (proc.AllocatedMemoryPtr != IntPtr.Zero)
                {
                    Sys_MemFree(proc.AllocatedMemoryPtr);
                }
            }

            Sys_Log("Process Manager module shutdown - all memory freed");
            base.OnHandleDestroyed(e);
        }
    }
}
