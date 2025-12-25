using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace KernelApp.UserControls
{
    public partial class ConsoleControl : UserControl
    {
        // Syscall DLL imports
        [DllImport("syscall.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "Sys_Init")]
        private static extern void Native_Sys_Init();

        [DllImport("syscall.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "Sys_Log", CharSet = CharSet.Ansi)]
        private static extern void Native_Sys_Log(string message);

        [DllImport("syscall.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "Sys_Sleep")]
        private static extern void Native_Sys_Sleep(int milliseconds);

        [DllImport("syscall.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "Sys_FileCreate", CharSet = CharSet.Ansi)]
        private static extern int Native_Sys_FileCreate(string filename);

        [DllImport("syscall.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "Sys_FileWrite", CharSet = CharSet.Ansi)]
        private static extern int Native_Sys_FileWrite(string filename, string text);

        [DllImport("syscall.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "Sys_FileRead", CharSet = CharSet.Ansi)]
        private static extern int Native_Sys_FileRead(string filename, StringBuilder buffer, int maxLen);

        [DllImport("syscall.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "Sys_DirCreate", CharSet = CharSet.Ansi)]
        private static extern int Native_Sys_DirCreate(string dirPath);

        [DllImport("syscall.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "Sys_DirChange", CharSet = CharSet.Ansi)]
        private static extern int Native_Sys_DirChange(string dirPath);

        [DllImport("syscall.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "Sys_DirGet", CharSet = CharSet.Ansi)]
        private static extern int Native_Sys_DirGet(StringBuilder buffer, int bufferSize);

        [DllImport("syscall.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "Sys_PathExists", CharSet = CharSet.Ansi)]
        private static extern int Native_Sys_PathExists(string path);

        // Track if syscall DLL is available
        private static bool syscallAvailable = false;
        private static bool syscallChecked = false;

        // Virtual file system root (MiniOS project folder)
        private string vfsRoot;
        private string currentDirectory;

        // Command history
        private List<string> commandHistory;
        private int historyIndex = -1;
        private int commandCount = 0;

        // Console colors
        private readonly Color colorPrompt = Color.FromArgb(34, 197, 94);
        private readonly Color colorOutput = Color.FromArgb(166, 173, 186);
        private readonly Color colorError = Color.FromArgb(239, 68, 68);
        private readonly Color colorSystem = Color.FromArgb(99, 102, 241);
        private readonly Color colorWarning = Color.FromArgb(251, 191, 36);
        private readonly Color colorSuccess = Color.FromArgb(34, 197, 94);

        public ConsoleControl()
        {
            InitializeComponent();
            commandHistory = new List<string>();
            InitializeVirtualFileSystem();
        }

        private void InitializeVirtualFileSystem()
        {
            // Set VFS root to MiniOS project folder (3 levels up from bin\Debug)
            string exePath = AppDomain.CurrentDomain.BaseDirectory;
            vfsRoot = Path.GetFullPath(Path.Combine(exePath, "..", "..", ".."));

            // If that doesn't exist, use exe directory
            if (!Directory.Exists(vfsRoot))
            {
                vfsRoot = exePath;
            }

            // Create VFS folder structure
            string vfsFolder = Path.Combine(vfsRoot, "VFS");
            if (!Directory.Exists(vfsFolder))
            {
                Directory.CreateDirectory(vfsFolder);
            }

            currentDirectory = vfsFolder;
        }

        private void ConsoleControl_Load(object sender, EventArgs e)
        {
            InitializeConsole();
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

        private static void Sys_Init()
        {
            CheckSyscallAvailability();
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

        private static void Sys_Sleep(int milliseconds)
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

        private bool Sys_FileCreate(string filename)
        {
            if (!syscallChecked) CheckSyscallAvailability();

            string fullPath = GetFullPath(filename);

            if (syscallAvailable)
            {
                try
                {
                    return Native_Sys_FileCreate(fullPath) != 0;
                }
                catch { }
            }

            // Fallback to managed code
            try
            {
                File.Create(fullPath).Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool Sys_FileWrite(string filename, string text)
        {
            if (!syscallChecked) CheckSyscallAvailability();

            string fullPath = GetFullPath(filename);

            if (syscallAvailable)
            {
                try
                {
                    return Native_Sys_FileWrite(fullPath, text) != 0;
                }
                catch { }
            }

            // Fallback
            try
            {
                File.AppendAllText(fullPath, text);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private string Sys_FileRead(string filename)
        {
            if (!syscallChecked) CheckSyscallAvailability();

            string fullPath = GetFullPath(filename);

            if (syscallAvailable)
            {
                try
                {
                    StringBuilder buffer = new StringBuilder(8192);
                    int bytesRead = Native_Sys_FileRead(fullPath, buffer, 8192);
                    if (bytesRead > 0)
                    {
                        // Use the StringBuilder's content directly instead of substring
                        // The buffer will contain the null-terminated string
                        return buffer.ToString();
                    }
                }
                catch { }
            }

            // Fallback
            try
            {
                if (File.Exists(fullPath))
                {
                    return File.ReadAllText(fullPath);
                }
            }
            catch { }

            return null;
        }

        private bool Sys_DirCreate(string dirName)
        {
            if (!syscallChecked) CheckSyscallAvailability();

            string fullPath = GetFullPath(dirName);

            if (syscallAvailable)
            {
                try
                {
                    return Native_Sys_DirCreate(fullPath) != 0;
                }
                catch { }
            }

            // Fallback
            try
            {
                Directory.CreateDirectory(fullPath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private string GetFullPath(string relativePath)
        {
            if (Path.IsPathRooted(relativePath))
            {
                return relativePath;
            }
            return Path.GetFullPath(Path.Combine(currentDirectory, relativePath));
        }

        private string GetRelativePath(string fullPath)
        {
            string vfsFolder = Path.Combine(vfsRoot, "VFS");
            if (fullPath.StartsWith(vfsFolder, StringComparison.OrdinalIgnoreCase))
            {
                string relative = fullPath.Substring(vfsFolder.Length);
                if (relative.StartsWith("\\") || relative.StartsWith("/"))
                    relative = relative.Substring(1);
                return string.IsNullOrEmpty(relative) ? "/" : "/" + relative.Replace("\\", "/");
            }
            return fullPath;
        }

        private void InitializeConsole()
        {
            Sys_Init();

            if (syscallAvailable)
            {
                Sys_Log("Console module initialized");
            }

            PrintBootSequence();
        }

        private void PrintBootSequence()
        {
            PrintLine("╔══════════════════════════════════════════════════════════════╗", colorSystem);
            PrintLine("║                    MiniOS System Console                     ║", colorSystem);
            PrintLine("║              Kernel Interface v1.0 (x86-32bit)               ║", colorSystem);
            PrintLine("╚══════════════════════════════════════════════════════════════╝", colorSystem);
            PrintLine("", colorOutput);
            PrintLine("[BOOT] System console initialized...", colorSuccess);

            if (syscallAvailable)
            {
                PrintLine("[BOOT] Syscall interface loaded (syscall.dll)", colorSuccess);
            }
            else
            {
                PrintLine("[BOOT] Syscall interface: SIMULATION MODE", colorWarning);
            }

            PrintLine($"[BOOT] VFS Root: {Path.Combine(vfsRoot, "VFS")}", colorOutput);
            PrintLine("[BOOT] Ready for commands.", colorSuccess);
            PrintLine("", colorOutput);
            PrintLine("Type 'help' for a list of available commands.", colorOutput);
            PrintLine("", colorOutput);
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            ExecuteCommand();
        }

        private void txtCommand_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                ExecuteCommand();
            }
            else if (e.KeyCode == Keys.Up)
            {
                e.SuppressKeyPress = true;
                NavigateHistory(-1);
            }
            else if (e.KeyCode == Keys.Down)
            {
                e.SuppressKeyPress = true;
                NavigateHistory(1);
            }
        }

        private void NavigateHistory(int direction)
        {
            if (commandHistory.Count == 0) return;

            historyIndex += direction;

            if (historyIndex < 0)
                historyIndex = 0;
            else if (historyIndex >= commandHistory.Count)
                historyIndex = commandHistory.Count - 1;

            txtCommand.Text = commandHistory[historyIndex];
        }

        private void ExecuteCommand()
        {
            string input = txtCommand.Text.Trim();
            if (string.IsNullOrEmpty(input)) return;

            commandHistory.Add(input);
            historyIndex = commandHistory.Count;
            commandCount++;

            PrintLine($"{GetPrompt()} {input}", colorPrompt);

            Sys_Log($"CMD: {input}");

            ProcessCommand(input);

            txtCommand.Text = "";
            lblCommandCount.Text = $"Commands: {commandCount}";

            rtbConsoleOutput.SelectionStart = rtbConsoleOutput.Text.Length;
            rtbConsoleOutput.ScrollToCaret();
        }

        private string GetPrompt()
        {
            return $"MiniOS:{GetRelativePath(currentDirectory)}>";
        }

        private void ProcessCommand(string input)
        {
            string[] parts = input.Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) return;

            string command = parts[0].ToLower();
            string args = parts.Length > 1 ? parts[1] : "";

            switch (command)
            {
                case "help":
                    ShowHelp();
                    break;
                case "clear":
                case "cls":
                    ClearConsole();
                    break;
                case "ver":
                case "version":
                    ShowVersion();
                    break;
                case "date":
                    ShowDate();
                    break;
                case "time":
                    ShowTime();
                    break;
                case "echo":
                    PrintLine(args, colorOutput);
                    break;
                case "ps":
                case "tasklist":
                    ShowProcessList();
                    break;
                case "mem":
                case "memory":
                    ShowMemoryStatus();
                    break;
                case "sysinfo":
                    ShowSystemInfo();
                    break;
                case "uptime":
                    ShowUptime();
                    break;
                case "whoami":
                    PrintLine("root@minios", colorOutput);
                    break;
                case "hostname":
                    PrintLine("minios-kernel", colorOutput);
                    break;

                // File System Commands
                case "pwd":
                    PrintLine(GetRelativePath(currentDirectory), colorOutput);
                    break;
                case "ls":
                case "dir":
                    ListDirectory(args);
                    break;
                case "cd":
                    ChangeDirectory(args);
                    break;
                case "mkdir":
                    MakeDirectory(args);
                    break;
                case "touch":
                    TouchFile(args);
                    break;
                case "cat":
                    CatFile(args);
                    break;
                case "write":
                    WriteToFile(args);
                    break;
                case "rm":
                case "del":
                    DeleteFile(args);
                    break;
                case "rmdir":
                    RemoveDirectory(args);
                    break;

                case "history":
                    ShowHistory();
                    break;
                case "shutdown":
                case "exit":
                    ShutdownCommand();
                    break;
                case "reboot":
                    RebootCommand();
                    break;
                case "sleep":
                    SleepCommand(args);
                    break;
                case "log":
                    LogCommand(args);
                    break;
                case "syscall":
                    SyscallInfo();
                    break;
                default:
                    PrintLine($"Unknown command: '{command}'. Type 'help' for available commands.", colorError);
                    break;
            }

            PrintLine("", colorOutput);
        }

        private void ChangeDirectory(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                PrintLine(GetRelativePath(currentDirectory), colorOutput);
                return;
            }

            string vfsFolder = Path.Combine(vfsRoot, "VFS");
            string newPath;

            if (path == "..")
            {
                // Go up one directory
                DirectoryInfo parent = Directory.GetParent(currentDirectory);
                if (parent != null && currentDirectory != vfsFolder)
                {
                    // Don't go above VFS root
                    if (parent.FullName.StartsWith(vfsFolder, StringComparison.OrdinalIgnoreCase) ||
                        parent.FullName.Equals(vfsFolder, StringComparison.OrdinalIgnoreCase))
                    {
                        newPath = parent.FullName;
                    }
                    else
                    {
                        newPath = vfsFolder;
                    }
                }
                else
                {
                    newPath = vfsFolder;
                }
            }
            else if (path == "/" || path == "~")
            {
                newPath = vfsFolder;
            }
            else
            {
                newPath = GetFullPath(path);
            }

            // Security: Ensure we stay within VFS
            if (!newPath.StartsWith(vfsFolder, StringComparison.OrdinalIgnoreCase))
            {
                PrintLine("cd: Access denied - Cannot navigate outside VFS", colorError);
                Sys_Log($"CD DENIED: Attempted to access {path}");
                return;
            }

            if (Directory.Exists(newPath))
            {
                currentDirectory = newPath;
                Sys_Log($"CD: Changed to {GetRelativePath(newPath)}");
                PrintLine($"Changed to: {GetRelativePath(newPath)}", colorSuccess);
            }
            else
            {
                PrintLine($"cd: Directory not found: {path}", colorError);
            }
        }

        private void ListDirectory(string path)
        {
            string targetDir = string.IsNullOrWhiteSpace(path) ? currentDirectory : GetFullPath(path);

            if (!Directory.Exists(targetDir))
            {
                PrintLine($"ls: Directory not found: {path}", colorError);
                return;
            }

            PrintLine($"Directory: {GetRelativePath(targetDir)}", colorSystem);
            PrintLine("", colorOutput);

            try
            {
                // List directories first
                foreach (var dir in Directory.GetDirectories(targetDir))
                {
                    string name = Path.GetFileName(dir);
                    PrintLine($"  [DIR]  {name}/", Color.FromArgb(99, 102, 241));
                }

                // Then files
                foreach (var file in Directory.GetFiles(targetDir))
                {
                    string name = Path.GetFileName(file);
                    long size = new FileInfo(file).Length;
                    PrintLine($"  [FILE] {name,-30} {FormatSize(size),10}", colorOutput);
                }

                int dirCount = Directory.GetDirectories(targetDir).Length;
                int fileCount = Directory.GetFiles(targetDir).Length;
                PrintLine("", colorOutput);
                PrintLine($"  {dirCount} directories, {fileCount} files", colorOutput);
            }
            catch (Exception ex)
            {
                PrintLine($"ls: Error reading directory: {ex.Message}", colorError);
            }
        }

        private void MakeDirectory(string dirName)
        {
            if (string.IsNullOrWhiteSpace(dirName))
            {
                PrintLine("Usage: mkdir <directory_name>", colorWarning);
                return;
            }

            if (Sys_DirCreate(dirName))
            {
                PrintLine($"Directory created: {dirName}", colorSuccess);
                Sys_Log($"MKDIR: Created directory {dirName}");
            }
            else
            {
                PrintLine($"mkdir: Failed to create directory: {dirName}", colorError);
            }
        }

        private void TouchFile(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                PrintLine("Usage: touch <filename>", colorWarning);
                return;
            }

            if (Sys_FileCreate(filename))
            {
                PrintLine($"File created: {filename}", colorSuccess);
                Sys_Log($"TOUCH: Created file {filename}");
            }
            else
            {
                PrintLine($"touch: Failed to create file: {filename}", colorError);
            }
        }

        private void CatFile(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                PrintLine("Usage: cat <filename>", colorWarning);
                return;
            }

            string content = Sys_FileRead(filename);
            if (content != null)
            {
                if (string.IsNullOrEmpty(content))
                {
                    PrintLine("(empty file)", colorOutput);
                }
                else
                {
                    PrintLine(content, colorOutput);
                }
                Sys_Log($"CAT: Read file {filename}");
            }
            else
            {
                PrintLine($"cat: File not found: {filename}", colorError);
            }
        }

        private void WriteToFile(string args)
        {
            // Syntax: write <filename> <content>
            int spaceIndex = args.IndexOf(' ');
            if (spaceIndex <= 0)
            {
                PrintLine("Usage: write <filename> <content>", colorWarning);
                return;
            }

            string filename = args.Substring(0, spaceIndex);
            string content = args.Substring(spaceIndex + 1);

            if (Sys_FileWrite(filename, content + Environment.NewLine))
            {
                PrintLine($"Written to: {filename}", colorSuccess);
                Sys_Log($"WRITE: Wrote to file {filename}");
            }
            else
            {
                PrintLine($"write: Failed to write to file: {filename}", colorError);
            }
        }

        private void DeleteFile(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                PrintLine("Usage: rm <filename>", colorWarning);
                return;
            }

            string fullPath = GetFullPath(filename);
            try
            {
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    PrintLine($"Deleted: {filename}", colorSuccess);
                    Sys_Log($"RM: Deleted file {filename}");
                }
                else
                {
                    PrintLine($"rm: File not found: {filename}", colorError);
                }
            }
            catch (Exception ex)
            {
                PrintLine($"rm: Error deleting file: {ex.Message}", colorError);
            }
        }

        private void RemoveDirectory(string dirName)
        {
            if (string.IsNullOrWhiteSpace(dirName))
            {
                PrintLine("Usage: rmdir <directory>", colorWarning);
                return;
            }

            string fullPath = GetFullPath(dirName);
            try
            {
                if (Directory.Exists(fullPath))
                {
                    Directory.Delete(fullPath, false);
                    PrintLine($"Directory removed: {dirName}", colorSuccess);
                    Sys_Log($"RMDIR: Removed directory {dirName}");
                }
                else
                {
                    PrintLine($"rmdir: Directory not found: {dirName}", colorError);
                }
            }
            catch (Exception ex)
            {
                PrintLine($"rmdir: Error removing directory: {ex.Message}", colorError);
            }
        }

        private string FormatSize(long bytes)
        {
            if (bytes < 1024) return $"{bytes} B";
            if (bytes < 1024 * 1024) return $"{bytes / 1024.0:F1} KB";
            return $"{bytes / (1024.0 * 1024.0):F2} MB";
        }

        private void ShowHelp()
        {
            PrintLine("╔══════════════════════════════════════════════════════════════╗", colorSystem);
            PrintLine("║                    Available Commands                        ║", colorSystem);
            PrintLine("╠══════════════════════════════════════════════════════════════╣", colorSystem);
            PrintLine("║  SYSTEM:                                                     ║", colorSystem);
            PrintLine("║    help, clear, version, sysinfo, uptime, syscall            ║", colorOutput);
            PrintLine("║                                                              ║", colorSystem);
            PrintLine("║  PROCESS:                                                    ║", colorSystem);
            PrintLine("║    ps         - List running processes                       ║", colorOutput);
            PrintLine("║    mem        - Show memory usage                            ║", colorOutput);
            PrintLine("║                                                              ║", colorSystem);
            PrintLine("║  FILE SYSTEM:                                                ║", colorSystem);
            PrintLine("║    pwd        - Print current directory                      ║", colorOutput);
            PrintLine("║    ls [path]  - List directory contents                      ║", colorOutput);
            PrintLine("║    cd <path>  - Change directory (.. for parent, / for root) ║", colorOutput);
            PrintLine("║    mkdir      - Create directory                             ║", colorOutput);
            PrintLine("║    rmdir      - Remove empty directory                       ║", colorOutput);
            PrintLine("║    touch      - Create empty file                            ║", colorOutput);
            PrintLine("║    cat        - Display file contents                        ║", colorOutput);
            PrintLine("║    write      - Write text to file (write file.txt hello)    ║", colorOutput);
            PrintLine("║    rm/del     - Delete file                                  ║", colorOutput);
            PrintLine("║                                                              ║", colorSystem);
            PrintLine("║  UTILITIES:                                                  ║", colorSystem);
            PrintLine("║    echo, date, time, sleep, log, history, whoami, hostname   ║", colorOutput);
            PrintLine("║                                                              ║", colorSystem);
            PrintLine("║  POWER:                                                      ║", colorSystem);
            PrintLine("║    shutdown, reboot                                          ║", colorOutput);
            PrintLine("╚══════════════════════════════════════════════════════════════╝", colorSystem);
        }

        private void ShowVersion()
        {
            PrintLine("MiniOS Kernel v1.0.0", colorSystem);
            PrintLine("Build: 2024.01 (x86-32bit)", colorOutput);
            PrintLine($"Syscall Module: {(syscallAvailable ? "syscall.dll (loaded)" : "SIMULATION MODE")}", colorOutput);
            PrintLine($"VFS Root: {Path.Combine(vfsRoot, "VFS")}", colorOutput);
        }

        private void ShowDate()
        {
            PrintLine(DateTime.Now.ToString("dddd, MMMM dd, yyyy"), colorOutput);
        }

        private void ShowTime()
        {
            PrintLine(DateTime.Now.ToString("HH:mm:ss"), colorOutput);
        }

        private void ShowProcessList()
        {
            var processes = KernelState.GetProcessList();

            PrintLine("PID     NAME                    STATUS      MEMORY", colorSystem);
            PrintLine("────────────────────────────────────────────────────", colorSystem);

            if (processes.Count == 0)
            {
                PrintLine("No processes running.", colorWarning);
                return;
            }

            foreach (var proc in processes)
            {
                string line = $"{proc.PID,-7} {proc.ProcessName,-23} {proc.Status,-11} {proc.MemoryMB} MB";
                Color statusColor = proc.Status == "Running" ? colorSuccess : colorOutput;
                PrintLine(line, statusColor);
            }

            PrintLine($"\nTotal: {processes.Count} processes", colorOutput);
        }

        private void ShowMemoryStatus()
        {
            var processes = KernelState.GetProcessList();
            int totalMem = 4096;
            int usedMem = 0;
            foreach (var p in processes)
                usedMem += p.MemoryMB;
            int freeMem = totalMem - usedMem;
            double usagePercent = (double)usedMem / totalMem * 100;

            PrintLine("╔═══════════════════════════════════════╗", colorSystem);
            PrintLine("║          Memory Status                ║", colorSystem);
            PrintLine("╠═══════════════════════════════════════╣", colorSystem);
            PrintLine($"║  Total Memory:     {totalMem,6} MB          ║", colorOutput);
            PrintLine($"║  Used Memory:      {usedMem,6} MB          ║", colorError);
            PrintLine($"║  Free Memory:      {freeMem,6} MB          ║", colorSuccess);
            PrintLine($"║  Usage:            {usagePercent,6:F1} %          ║", colorWarning);
            PrintLine("╚═══════════════════════════════════════╝", colorSystem);
        }

        private void ShowSystemInfo()
        {
            PrintLine("╔═══════════════════════════════════════════════════╗", colorSystem);
            PrintLine("║              System Information                   ║", colorSystem);
            PrintLine("╠═══════════════════════════════════════════════════╣", colorSystem);
            PrintLine("║  OS:           MiniOS Kernel v1.0                 ║", colorOutput);
            PrintLine("║  Architecture: x86-32bit                          ║", colorOutput);
            PrintLine("║  Hostname:     minios-kernel                      ║", colorOutput);
            PrintLine("║  User:         root                               ║", colorOutput);
            PrintLine("║  Shell:        MiniOS Console                     ║", colorOutput);
            string syscallStatus = syscallAvailable ? "LOADED" : "SIMULATION";
            PrintLine($"║  Syscall:      {syscallStatus,-20}            ║", syscallAvailable ? colorSuccess : colorWarning);
            PrintLine("╚═══════════════════════════════════════════════════╝", colorSystem);
        }

        private void ShowUptime()
        {
            TimeSpan uptime = TimeSpan.FromMilliseconds(Environment.TickCount);
            PrintLine($"System uptime: {uptime.Days}d {uptime.Hours}h {uptime.Minutes}m {uptime.Seconds}s", colorOutput);
        }

        private void ShowHistory()
        {
            if (commandHistory.Count == 0)
            {
                PrintLine("No command history.", colorOutput);
                return;
            }

            for (int i = 0; i < commandHistory.Count; i++)
            {
                PrintLine($"  {i + 1}  {commandHistory[i]}", colorOutput);
            }
        }

        private void ShutdownCommand()
        {
            PrintLine("[SYSTEM] Initiating shutdown sequence...", colorWarning);
            PrintLine("[SYSTEM] Saving state...", colorWarning);
            PrintLine("[SYSTEM] Goodbye.", colorSystem);
            Sys_Log("System shutdown initiated from console");
        }

        private void RebootCommand()
        {
            PrintLine("[SYSTEM] Initiating reboot sequence...", colorWarning);
            PrintLine("[SYSTEM] Restarting services...", colorWarning);
            Sys_Log("System reboot initiated from console");
        }

        private void SleepCommand(string args)
        {
            if (string.IsNullOrWhiteSpace(args))
            {
                PrintLine("Usage: sleep <milliseconds>", colorWarning);
                return;
            }

            if (int.TryParse(args, out int ms))
            {
                PrintLine($"Sleeping for {ms}ms...", colorOutput);
                Sys_Sleep(ms);
                PrintLine("Awake.", colorSuccess);
            }
            else
            {
                PrintLine("Invalid milliseconds value.", colorError);
            }
        }

        private void LogCommand(string args)
        {
            if (string.IsNullOrWhiteSpace(args))
            {
                PrintLine("Usage: log <message>", colorWarning);
                return;
            }

            if (syscallAvailable)
            {
                Sys_Log(args);
                PrintLine($"Logged: {args}", colorSuccess);
            }
            else
            {
                PrintLine($"[SIM] Would log: {args}", colorWarning);
                PrintLine("Note: Syscall module not available.", colorWarning);
            }
        }

        private void SyscallInfo()
        {
            PrintLine("╔══════════════════════════════════════════════════════════════╗", colorSystem);
            PrintLine("║                  Syscall Module Information                  ║", colorSystem);
            PrintLine("╠══════════════════════════════════════════════════════════════╣", colorSystem);
            PrintLine("║  Module:     syscall.dll                                     ║", colorOutput);
            PrintLine("║  Language:   x86 Assembly (32-bit)                           ║", colorOutput);
            PrintLine("║  Convention: stdcall                                         ║", colorOutput);

            string statusLine = syscallAvailable
                ? "║  Status:     LOADED                                          ║"
                : "║  Status:     NOT AVAILABLE (simulation mode)                 ║";
            PrintLine(statusLine, syscallAvailable ? colorSuccess : colorWarning);

            PrintLine("║                                                              ║", colorSystem);
            PrintLine("║  Exported Functions:                                         ║", colorSystem);
            PrintLine("║    Sys_Init, Sys_Print, Sys_Input, Sys_Sleep                 ║", colorOutput);
            PrintLine("║    Sys_MemAlloc, Sys_MemFree                                 ║", colorOutput);
            PrintLine("║    Sys_FileCreate, Sys_FileWrite, Sys_FileRead               ║", colorOutput);
            PrintLine("║    Sys_DirCreate, Sys_DirChange, Sys_DirGet                  ║", colorOutput);
            PrintLine("║    Sys_PathExists, Sys_Log                                   ║", colorOutput);
            PrintLine("╚══════════════════════════════════════════════════════════════╝", colorSystem);
        }

        private void ClearConsole()
        {
            rtbConsoleOutput.Clear();
            lblStatus.Text = "Console cleared";
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearConsole();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                string filename = $"console_output_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), filename);
                File.WriteAllText(path, rtbConsoleOutput.Text);
                PrintLine($"Console exported to: {path}", colorSuccess);
                lblStatus.Text = "Console exported";
            }
            catch (Exception ex)
            {
                PrintLine($"Export failed: {ex.Message}", colorError);
            }
        }

        private void PrintLine(string text, Color color)
        {
            rtbConsoleOutput.SelectionStart = rtbConsoleOutput.TextLength;
            rtbConsoleOutput.SelectionColor = color;
            rtbConsoleOutput.AppendText(text + Environment.NewLine);
            rtbConsoleOutput.SelectionColor = rtbConsoleOutput.ForeColor;
        }
    }
}
