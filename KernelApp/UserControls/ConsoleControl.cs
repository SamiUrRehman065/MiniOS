using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using KernelApp.Helpers;

namespace KernelApp.UserControls
{
    public partial class ConsoleControl : UserControl
    {
        #region Constants

        private const int TOTAL_MEMORY_MB = 4096;
        private const int MAX_ALLOC_BYTES = 1024 * 1024; // 1MB limit for testing

        #endregion

        #region Colors

        private static readonly Color ColorPrompt = Color.FromArgb(34, 197, 94);
        private static readonly Color ColorOutput = Color.FromArgb(166, 173, 186);
        private static readonly Color ColorError = Color.FromArgb(239, 68, 68);
        private static readonly Color ColorSystem = Color.FromArgb(99, 102, 241);
        private static readonly Color ColorWarning = Color.FromArgb(251, 191, 36);
        private static readonly Color ColorSuccess = Color.FromArgb(34, 197, 94);

        #endregion

        #region State

        private readonly string _vfsRoot;
        private string _currentDirectory;
        private readonly List<string> _commandHistory;
        private int _historyIndex = -1;
        private int _commandCount;
        private readonly Dictionary<string, string> _envVariables;

        private static readonly string[] DefaultProcessNames =
        {
            "shell.exe", "logger.sys", "network.sys", "display.sys",
            "audio.sys", "input.sys", "storage.sys", "crypto.sys",
            "timer.sys", "interrupt.sys", "driver.sys", "service.exe",
            "daemon.sys", "monitor.exe", "cache.sys"
        };

        #endregion

        public ConsoleControl()
        {
            InitializeComponent();
            _commandHistory = new List<string>();
            _envVariables = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            _vfsRoot = InitializeVirtualFileSystem();
            _currentDirectory = Path.Combine(_vfsRoot, "VFS");
            InitializeEnvironmentVariables();
        }

        #region Initialization

        private void InitializeEnvironmentVariables()
        {
            _envVariables["USER"] = "root";
            _envVariables["HOSTNAME"] = "minios-kernel";
            _envVariables["SHELL"] = "MiniOS Console";
            _envVariables["PATH"] = "/bin:/usr/bin";
            _envVariables["HOME"] = "/";
        }

        private static string InitializeVirtualFileSystem()
        {
            string exePath = AppDomain.CurrentDomain.BaseDirectory;
            string vfsRoot = Path.GetFullPath(Path.Combine(exePath, "..", "..", ".."));

            if (!Directory.Exists(vfsRoot))
            {
                vfsRoot = exePath;
            }

            string vfsFolder = Path.Combine(vfsRoot, "VFS");
            if (!Directory.Exists(vfsFolder))
            {
                Directory.CreateDirectory(vfsFolder);
            }

            return vfsRoot;
        }

        private void ConsoleControl_Load(object sender, EventArgs e)
        {
            InitializeConsole();
        }

        private void InitializeConsole()
        {
            SyscallHelper.EnsureInitialized();

            if (SyscallHelper.IsAvailable)
            {
                SyscallHelper.Log("Console module initialized");
                SyscallHelper.Print("MiniOS Console initialized via syscall.dll\r\n");
            }

            PrintBootSequence();
        }

        private void PrintBootSequence()
        {
            PrintLine("╔══════════════════════════════════════════════════════════════╗", ColorSystem);
            PrintLine("║                    MiniOS System Console                     ║", ColorSystem);
            PrintLine("║              Kernel Interface v1.0 (x86-32bit)               ║", ColorSystem);
            PrintLine("╚══════════════════════════════════════════════════════════════╝", ColorSystem);
            PrintLine("", ColorOutput);
            PrintLine("[BOOT] System console initialized...", ColorSuccess);

            if (SyscallHelper.IsAvailable)
            {
                PrintLine("[BOOT] Syscall interface loaded (syscall.dll)", ColorSuccess);
                PrintLine("[BOOT] Native I/O: Sys_Print, Sys_Input available", ColorSuccess);
            }
            else
            {
                PrintLine("[BOOT] Syscall interface: SIMULATION MODE", ColorWarning);
            }

            PrintLine($"[BOOT] VFS Root: {Path.Combine(_vfsRoot, "VFS")}", ColorOutput);
            PrintLine("[BOOT] Ready for commands.", ColorSuccess);
            PrintLine("", ColorOutput);
            PrintLine("Type 'help' for a list of available commands.", ColorOutput);
            PrintLine("", ColorOutput);
        }

        #endregion

        #region Event Handlers

        private void btnExecute_Click(object sender, EventArgs e)
        {
            ExecuteCommand();
        }

        private void txtCommand_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    e.SuppressKeyPress = true;
                    ExecuteCommand();
                    break;
                case Keys.Up:
                    e.SuppressKeyPress = true;
                    NavigateHistory(-1);
                    break;
                case Keys.Down:
                    e.SuppressKeyPress = true;
                    NavigateHistory(1);
                    break;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearConsole();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            ExportConsole();
        }

        #endregion

        #region Command Execution

        private void NavigateHistory(int direction)
        {
            if (_commandHistory.Count == 0) return;

            _historyIndex += direction;
            _historyIndex = Math.Max(0, Math.Min(_historyIndex, _commandHistory.Count - 1));

            txtCommand.Text = _commandHistory[_historyIndex];
        }

        private void ExecuteCommand()
        {
            string input = txtCommand.Text.Trim();
            if (string.IsNullOrEmpty(input)) return;

            _commandHistory.Add(input);
            _historyIndex = _commandHistory.Count;
            _commandCount++;

            PrintLine($"{GetPrompt()} {input}", ColorPrompt);

            SyscallHelper.Log($"CMD: {input}");
            SyscallHelper.Print($"{GetPrompt()} {input}\r\n");

            ProcessCommand(input);

            txtCommand.Text = "";
            lblCommandCount.Text = $"Commands: {_commandCount}";

            rtbConsoleOutput.SelectionStart = rtbConsoleOutput.Text.Length;
            rtbConsoleOutput.ScrollToCaret();
        }

        private string GetPrompt()
        {
            return $"MiniOS:{GetRelativePath(_currentDirectory)}>";
        }

        private string ExpandVariables(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            foreach (var kvp in _envVariables)
            {
                input = input.Replace($"${{{kvp.Key}}}", kvp.Value);
                input = input.Replace($"${kvp.Key}", kvp.Value);
            }

            return input;
        }

        private void ProcessCommand(string input)
        {
            input = ExpandVariables(input);

            string[] parts = input.Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) return;

            string command = parts[0].ToLowerInvariant();
            string args = parts.Length > 1 ? parts[1] : "";

            switch (command)
            {
                // System Commands
                case "help": ShowHelp(); break;
                case "clear":
                case "cls": ClearConsole(); break;
                case "ver":
                case "version": ShowVersion(); break;
                case "date": ShowDate(); break;
                case "time": ShowTime(); break;
                case "sysinfo": ShowSystemInfo(); break;
                case "uptime": ShowUptime(); break;
                case "syscall": SyscallInfo(); break;

                // I/O Commands
                case "echo": EchoCommand(args); break;
                case "print": PrintCommand(args); break;
                case "read": ReadCommand(args); break;

                // Environment Commands
                case "set": SetCommand(args); break;
                case "env": EnvCommand(); break;
                case "whoami": PrintLine(_envVariables["USER"], ColorOutput); break;
                case "hostname": PrintLine(_envVariables["HOSTNAME"], ColorOutput); break;

                // Process Commands
                case "ps":
                case "tasklist": ShowProcessList(); break;
                case "mem":
                case "memory": ShowMemoryStatus(); break;
                case "alloc": AllocCommand(args); break;

                // File System Commands
                case "pwd": PrintLine(GetRelativePath(_currentDirectory), ColorOutput); break;
                case "ls":
                case "dir": ListDirectory(args); break;
                case "cd": ChangeDirectory(args); break;
                case "mkdir": MakeDirectory(args); break;
                case "touch": TouchFile(args); break;
                case "cat": CatFile(args); break;
                case "write": WriteToFile(args); break;
                case "rm":
                case "del": DeleteFile(args); break;
                case "rmdir": RemoveDirectory(args); break;

                // Utility Commands
                case "history": ShowHistory(); break;
                case "sleep": SleepCommand(args); break;
                case "log": LogCommand(args); break;

                // Power Commands
                case "shutdown":
                case "exit": ShutdownCommand(); break;
                case "reboot": RebootCommand(); break;

                default:
                    PrintLine($"Unknown command: '{command}'. Type 'help' for available commands.", ColorError);
                    break;
            }

            PrintLine("", ColorOutput);
        }

        #endregion

        #region I/O Commands

        private void EchoCommand(string args)
        {
            PrintLine(args, ColorOutput);
        }

        private void PrintCommand(string args)
        {
            if (string.IsNullOrWhiteSpace(args))
            {
                PrintLine("Usage: print <message>", ColorWarning);
                PrintLine("Opens cmd.exe, prints message, and displays result here", ColorOutput);
                return;
            }

            try
            {
                PrintLine($"[CMD] Opening command prompt to print: {args}", ColorSystem);

                using (var process = new Process())
                {
                    process.StartInfo.FileName = "cmd.exe";
                    process.StartInfo.Arguments = $"/c echo {args}";
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.CreateNoWindow = false;
                    process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;

                    process.Start();

                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    process.WaitForExit();

                    if (!string.IsNullOrEmpty(output))
                    {
                        PrintLine($"[CMD OUTPUT] {output.Trim()}", ColorSuccess);
                    }

                    if (!string.IsNullOrEmpty(error))
                    {
                        PrintLine($"[CMD ERROR] {error.Trim()}", ColorError);
                    }

                    PrintLine($"[CMD] Process exited with code: {process.ExitCode}", ColorOutput);
                }

                SyscallHelper.Log($"PRINT CMD: {args}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ConsoleControl.PrintCommand failed: {ex.Message}");
                PrintLine($"[ERROR] Failed to execute print command: {ex.Message}", ColorError);
            }
        }

        private void ReadCommand(string args)
        {
            string varName = string.IsNullOrWhiteSpace(args) ? "INPUT" : args.Trim().ToUpperInvariant();
            string tempBatchFile = null;
            string tempOutputFile = null;

            try
            {
                PrintLine($"[CMD] Opening command prompt for input into ${varName}...", ColorSystem);
                PrintLine("(Enter your input in the cmd window that opens)", ColorWarning);

                tempBatchFile = Path.Combine(Path.GetTempPath(), $"minios_input_{Guid.NewGuid():N}.bat");
                tempOutputFile = Path.Combine(Path.GetTempPath(), $"minios_input_result_{Guid.NewGuid():N}.txt");

                string batchContent = $@"@echo off
echo ═══════════════════════════════════════════════════
echo         MiniOS Native Input - Enter value for {varName}
echo ═══════════════════════════════════════════════════
set /p USERINPUT=""Enter {varName}: ""
echo %USERINPUT%> ""{tempOutputFile}""
echo.
echo Input captured! This window will close...
timeout /t 2 >nul
";
                File.WriteAllText(tempBatchFile, batchContent);

                using (var process = new Process())
                {
                    process.StartInfo.FileName = "cmd.exe";
                    process.StartInfo.Arguments = $"/c \"{tempBatchFile}\"";
                    process.StartInfo.UseShellExecute = true;
                    process.StartInfo.CreateNoWindow = false;
                    process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;

                    process.Start();
                    process.WaitForExit();
                }

                if (File.Exists(tempOutputFile))
                {
                    string input = File.ReadAllText(tempOutputFile).Trim();

                    if (!string.IsNullOrEmpty(input))
                    {
                        _envVariables[varName] = input;
                        PrintLine($"[CMD INPUT] Read: '{input}'", ColorSuccess);
                        PrintLine($"Stored in ${varName}", ColorOutput);
                        SyscallHelper.Log($"INPUT CMD: Read '{input}' into ${varName}");
                    }
                    else
                    {
                        PrintLine("No input received or empty input.", ColorWarning);
                    }
                }
                else
                {
                    PrintLine("Input was cancelled or failed.", ColorWarning);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ConsoleControl.ReadCommand failed: {ex.Message}");
                PrintLine($"[ERROR] Failed to execute read command: {ex.Message}", ColorError);
            }
            finally
            {
                // Cleanup temp files
                CleanupTempFile(tempBatchFile);
                CleanupTempFile(tempOutputFile);
            }
        }

        private static void CleanupTempFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return;

            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to cleanup temp file {filePath}: {ex.Message}");
            }
        }

        #endregion

        #region Environment Commands

        private void SetCommand(string args)
        {
            if (string.IsNullOrWhiteSpace(args))
            {
                PrintLine("Usage: set <NAME>=<value>", ColorWarning);
                PrintLine("Example: set MYVAR=hello", ColorOutput);
                return;
            }

            int eqIndex = args.IndexOf('=');
            if (eqIndex <= 0)
            {
                string varName = args.Trim().ToUpperInvariant();
                if (_envVariables.TryGetValue(varName, out string value))
                {
                    PrintLine($"{varName}={value}", ColorOutput);
                }
                else
                {
                    PrintLine($"Variable not found: {varName}", ColorWarning);
                }
                return;
            }

            string name = args.Substring(0, eqIndex).Trim().ToUpperInvariant();
            string val = args.Substring(eqIndex + 1);

            _envVariables[name] = val;
            PrintLine($"Set {name}={val}", ColorSuccess);
            SyscallHelper.Log($"ENV: Set {name}={val}");
        }

        private void EnvCommand()
        {
            PrintLine("Environment Variables:", ColorSystem);
            PrintLine("──────────────────────────────────────", ColorSystem);

            foreach (var kvp in _envVariables)
            {
                PrintLine($"  {kvp.Key}={kvp.Value}", ColorOutput);
            }

            PrintLine($"\nTotal: {_envVariables.Count} variables", ColorOutput);
        }

        #endregion

        #region Memory Commands

        private void AllocCommand(string args)
        {
            if (string.IsNullOrWhiteSpace(args))
            {
                PrintLine("Usage: alloc <bytes>", ColorWarning);
                PrintLine("Allocates memory using Sys_MemAlloc syscall (test only)", ColorOutput);
                return;
            }

            if (!int.TryParse(args, out int bytes) || bytes <= 0)
            {
                PrintLine("Invalid size. Please specify a positive integer.", ColorError);
                return;
            }

            if (bytes > MAX_ALLOC_BYTES)
            {
                PrintLine("Size too large. Maximum 1MB for testing.", ColorError);
                return;
            }

            if (SyscallHelper.IsAvailable)
            {
                IntPtr ptr = SyscallHelper.MemAlloc(bytes, logAllocation: false);
                if (ptr != IntPtr.Zero)
                {
                    PrintLine($"Allocated {bytes} bytes at address 0x{ptr.ToInt32():X8}", ColorSuccess);

                    if (SyscallHelper.MemFree(ptr, logFree: false))
                    {
                        PrintLine($"Freed memory at 0x{ptr.ToInt32():X8}", ColorSuccess);
                    }
                }
                else
                {
                    PrintLine("Memory allocation failed.", ColorError);
                }
            }
            else
            {
                PrintLine("[SIM] Would allocate memory via syscall.", ColorWarning);
                PrintLine("Note: Syscall module not available.", ColorWarning);
            }
        }

        #endregion

        #region File System Commands

        private string GetFullPath(string relativePath)
        {
            if (Path.IsPathRooted(relativePath))
            {
                return relativePath;
            }
            return Path.GetFullPath(Path.Combine(_currentDirectory, relativePath));
        }

        private string GetRelativePath(string fullPath)
        {
            string vfsFolder = Path.Combine(_vfsRoot, "VFS");
            if (fullPath.StartsWith(vfsFolder, StringComparison.OrdinalIgnoreCase))
            {
                string relative = fullPath.Substring(vfsFolder.Length);
                if (relative.StartsWith("\\") || relative.StartsWith("/"))
                    relative = relative.Substring(1);
                return string.IsNullOrEmpty(relative) ? "/" : "/" + relative.Replace("\\", "/");
            }
            return fullPath;
        }

        private void ChangeDirectory(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                PrintLine(GetRelativePath(_currentDirectory), ColorOutput);
                return;
            }

            string vfsFolder = Path.Combine(_vfsRoot, "VFS");
            string newPath = ResolvePath(path, vfsFolder);

            if (!newPath.StartsWith(vfsFolder, StringComparison.OrdinalIgnoreCase))
            {
                PrintLine("cd: Access denied - Cannot navigate outside VFS", ColorError);
                SyscallHelper.Log($"CD DENIED: Attempted to access {path}");
                return;
            }

            if (Directory.Exists(newPath))
            {
                _currentDirectory = newPath;
                _envVariables["PWD"] = GetRelativePath(newPath);
                SyscallHelper.Log($"CD: Changed to {GetRelativePath(newPath)}");
                PrintLine($"Changed to: {GetRelativePath(newPath)}", ColorSuccess);
            }
            else
            {
                PrintLine($"cd: Directory not found: {path}", ColorError);
            }
        }

        private string ResolvePath(string path, string vfsFolder)
        {
            if (path == "..")
            {
                DirectoryInfo parent = Directory.GetParent(_currentDirectory);
                if (parent != null && _currentDirectory != vfsFolder)
                {
                    if (parent.FullName.StartsWith(vfsFolder, StringComparison.OrdinalIgnoreCase) ||
                        parent.FullName.Equals(vfsFolder, StringComparison.OrdinalIgnoreCase))
                    {
                        return parent.FullName;
                    }
                }
                return vfsFolder;
            }

            if (path == "/" || path == "~")
            {
                return vfsFolder;
            }

            return GetFullPath(path);
        }

        private void ListDirectory(string path)
        {
            string targetDir = string.IsNullOrWhiteSpace(path) ? _currentDirectory : GetFullPath(path);

            if (!Directory.Exists(targetDir))
            {
                PrintLine($"ls: Directory not found: {path}", ColorError);
                return;
            }

            PrintLine($"Directory: {GetRelativePath(targetDir)}", ColorSystem);
            PrintLine("", ColorOutput);

            try
            {
                string[] directories = Directory.GetDirectories(targetDir);
                string[] files = Directory.GetFiles(targetDir);

                foreach (var dir in directories)
                {
                    string name = Path.GetFileName(dir);
                    PrintLine($"  [DIR]  {name}/", ColorSystem);
                }

                foreach (var file in files)
                {
                    string name = Path.GetFileName(file);
                    long size = new FileInfo(file).Length;
                    PrintLine($"  [FILE] {name,-30} {FormatSize(size),10}", ColorOutput);
                }

                PrintLine("", ColorOutput);
                PrintLine($"  {directories.Length} directories, {files.Length} files", ColorOutput);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ConsoleControl.ListDirectory failed: {ex.Message}");
                PrintLine($"ls: Error reading directory: {ex.Message}", ColorError);
            }
        }

        private void MakeDirectory(string dirName)
        {
            if (string.IsNullOrWhiteSpace(dirName))
            {
                PrintLine("Usage: mkdir <directory_name>", ColorWarning);
                return;
            }

            string fullPath = GetFullPath(dirName);
            if (SyscallHelper.DirCreate(fullPath))
            {
                PrintLine($"Directory created: {dirName}", ColorSuccess);
                SyscallHelper.Log($"MKDIR: Created directory {dirName}");
            }
            else
            {
                PrintLine($"mkdir: Failed to create directory: {dirName}", ColorError);
            }
        }

        private void TouchFile(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                PrintLine("Usage: touch <filename>", ColorWarning);
                return;
            }

            string fullPath = GetFullPath(filename);
            if (SyscallHelper.FileCreate(fullPath))
            {
                PrintLine($"File created: {filename}", ColorSuccess);
                SyscallHelper.Log($"TOUCH: Created file {filename}");
            }
            else
            {
                PrintLine($"touch: Failed to create file: {filename}", ColorError);
            }
        }

        private void CatFile(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                PrintLine("Usage: cat <filename>", ColorWarning);
                return;
            }

            string fullPath = GetFullPath(filename);
            string content = SyscallHelper.FileRead(fullPath);

            if (content != null)
            {
                if (string.IsNullOrEmpty(content))
                {
                    PrintLine("(empty file)", ColorOutput);
                }
                else
                {
                    PrintLine(content, ColorOutput);
                    SyscallHelper.Print(content);
                }
                SyscallHelper.Log($"CAT: Read file {filename}");
            }
            else
            {
                PrintLine($"cat: File not found: {filename}", ColorError);
            }
        }

        private void WriteToFile(string args)
        {
            int spaceIndex = args.IndexOf(' ');
            if (spaceIndex <= 0)
            {
                PrintLine("Usage: write <filename> <content>", ColorWarning);
                return;
            }

            string filename = args.Substring(0, spaceIndex);
            string content = args.Substring(spaceIndex + 1);
            string fullPath = GetFullPath(filename);

            if (SyscallHelper.FileWrite(fullPath, content + Environment.NewLine))
            {
                PrintLine($"Written to: {filename}", ColorSuccess);
                SyscallHelper.Log($"WRITE: Wrote to file {filename}");
            }
            else
            {
                PrintLine($"write: Failed to write to file: {filename}", ColorError);
            }
        }

        private void DeleteFile(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                PrintLine("Usage: rm <filename>", ColorWarning);
                return;
            }

            string fullPath = GetFullPath(filename);
            try
            {
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    PrintLine($"Deleted: {filename}", ColorSuccess);
                    SyscallHelper.Log($"RM: Deleted file {filename}");
                }
                else
                {
                    PrintLine($"rm: File not found: {filename}", ColorError);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ConsoleControl.DeleteFile failed: {ex.Message}");
                PrintLine($"rm: Error deleting file: {ex.Message}", ColorError);
            }
        }

        private void RemoveDirectory(string dirName)
        {
            if (string.IsNullOrWhiteSpace(dirName))
            {
                PrintLine("Usage: rmdir <directory>", ColorWarning);
                return;
            }

            string fullPath = GetFullPath(dirName);
            try
            {
                if (Directory.Exists(fullPath))
                {
                    Directory.Delete(fullPath, false);
                    PrintLine($"Directory removed: {dirName}", ColorSuccess);
                    SyscallHelper.Log($"RMDIR: Removed directory {dirName}");
                }
                else
                {
                    PrintLine($"rmdir: Directory not found: {dirName}", ColorError);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ConsoleControl.RemoveDirectory failed: {ex.Message}");
                PrintLine($"rmdir: Error removing directory: {ex.Message}", ColorError);
            }
        }

        #endregion

        #region Info Commands

        private void ShowHelp()
        {
            PrintLine("╔══════════════════════════════════════════════════════════════╗", ColorSystem);
            PrintLine("║                    Available Commands                        ║", ColorSystem);
            PrintLine("╠══════════════════════════════════════════════════════════════╣", ColorSystem);
            PrintLine("║  SYSTEM:                                                     ║", ColorSystem);
            PrintLine("║    help, clear, version, sysinfo, uptime, syscall            ║", ColorOutput);
            PrintLine("║                                                              ║", ColorSystem);
            PrintLine("║  I/O (Native Syscalls):                                      ║", ColorSystem);
            PrintLine("║    print <msg> - Print to native console (Sys_Print)         ║", ColorOutput);
            PrintLine("║    read [var]  - Read from native console (Sys_Input)        ║", ColorOutput);
            PrintLine("║    alloc <n>   - Test memory allocation (Sys_MemAlloc)       ║", ColorOutput);
            PrintLine("║                                                              ║", ColorSystem);
            PrintLine("║  ENVIRONMENT:                                                ║", ColorSystem);
            PrintLine("║    set <VAR>=<val> - Set environment variable                ║", ColorOutput);
            PrintLine("║    env            - Show all environment variables           ║", ColorOutput);
            PrintLine("║    echo $VAR      - Print variable value                     ║", ColorOutput);
            PrintLine("║                                                              ║", ColorSystem);
            PrintLine("║  PROCESS:                                                    ║", ColorSystem);
            PrintLine("║    ps         - List running processes                       ║", ColorOutput);
            PrintLine("║    mem        - Show memory usage                            ║", ColorOutput);
            PrintLine("║                                                              ║", ColorSystem);
            PrintLine("║  FILE SYSTEM:                                                ║", ColorSystem);
            PrintLine("║    pwd, ls, cd, mkdir, rmdir, touch, cat, write, rm          ║", ColorOutput);
            PrintLine("║                                                              ║", ColorSystem);
            PrintLine("║  UTILITIES:                                                  ║", ColorSystem);
            PrintLine("║    echo, date, time, sleep, log, history, whoami, hostname   ║", ColorOutput);
            PrintLine("║                                                              ║", ColorSystem);
            PrintLine("║  POWER:                                                      ║", ColorSystem);
            PrintLine("║    shutdown, reboot                                          ║", ColorOutput);
            PrintLine("╚══════════════════════════════════════════════════════════════╝", ColorSystem);
        }

        private void ShowVersion()
        {
            PrintLine("MiniOS Kernel v1.0.0", ColorSystem);
            PrintLine("Build: 2024.01 (x86-32bit)", ColorOutput);
            string syscallStatus = SyscallHelper.IsAvailable ? "syscall.dll (loaded)" : "SIMULATION MODE";
            PrintLine($"Syscall Module: {syscallStatus}", ColorOutput);
            PrintLine($"VFS Root: {Path.Combine(_vfsRoot, "VFS")}", ColorOutput);
        }

        private void ShowDate()
        {
            PrintLine(DateTime.Now.ToString("dddd, MMMM dd, yyyy"), ColorOutput);
        }

        private void ShowTime()
        {
            PrintLine(DateTime.Now.ToString("HH:mm:ss"), ColorOutput);
        }

        private void ShowProcessList()
        {
            var processes = KernelState.GetProcessList();

            PrintLine("PID     NAME                    STATUS      MEMORY", ColorSystem);
            PrintLine("────────────────────────────────────────────────────", ColorSystem);

            if (processes.Count == 0)
            {
                PrintLine("No processes running.", ColorWarning);
                return;
            }

            foreach (var proc in processes)
            {
                string line = $"{proc.PID,-7} {proc.ProcessName,-23} {proc.Status,-11} {proc.MemoryMB} MB";
                Color statusColor = proc.Status == "Running" ? ColorSuccess : ColorOutput;
                PrintLine(line, statusColor);
            }

            PrintLine($"\nTotal: {processes.Count} processes", ColorOutput);
        }

        private void ShowMemoryStatus()
        {
            var processes = KernelState.GetProcessList();
            int usedMem = 0;
            foreach (var p in processes)
                usedMem += p.MemoryMB;

            int freeMem = TOTAL_MEMORY_MB - usedMem;
            double usagePercent = (double)usedMem / TOTAL_MEMORY_MB * 100;

            PrintLine("╔═══════════════════════════════════════╗", ColorSystem);
            PrintLine("║          Memory Status                ║", ColorSystem);
            PrintLine("╠═══════════════════════════════════════╣", ColorSystem);
            PrintLine($"║  Total Memory:     {TOTAL_MEMORY_MB,6} MB          ║", ColorOutput);
            PrintLine($"║  Used Memory:      {usedMem,6} MB          ║", ColorError);
            PrintLine($"║  Free Memory:      {freeMem,6} MB          ║", ColorSuccess);
            PrintLine($"║  Usage:            {usagePercent,6:F1} %          ║", ColorWarning);
            PrintLine("╚═══════════════════════════════════════╝", ColorSystem);
        }

        private void ShowSystemInfo()
        {
            string syscallStatus = SyscallHelper.IsAvailable ? "LOADED" : "SIMULATION";
            Color syscallColor = SyscallHelper.IsAvailable ? ColorSuccess : ColorWarning;

            PrintLine("╔═══════════════════════════════════════════════════╗", ColorSystem);
            PrintLine("║              System Information                   ║", ColorSystem);
            PrintLine("╠═══════════════════════════════════════════════════╣", ColorSystem);
            PrintLine("║  OS:           MiniOS Kernel v1.0                 ║", ColorOutput);
            PrintLine("║  Architecture: x86-32bit                          ║", ColorOutput);
            PrintLine($"║  Hostname:     {_envVariables["HOSTNAME"],-22}       ║", ColorOutput);
            PrintLine($"║  User:         {_envVariables["USER"],-22}       ║", ColorOutput);
            PrintLine("║  Shell:        MiniOS Console                     ║", ColorOutput);
            PrintLine($"║  Syscall:      {syscallStatus,-22}       ║", syscallColor);
            PrintLine("╚═══════════════════════════════════════════════════╝", ColorSystem);
        }

        private void ShowUptime()
        {
            TimeSpan uptime = TimeSpan.FromMilliseconds(Environment.TickCount);
            PrintLine($"System uptime: {uptime.Days}d {uptime.Hours}h {uptime.Minutes}m {uptime.Seconds}s", ColorOutput);
        }

        private void ShowHistory()
        {
            if (_commandHistory.Count == 0)
            {
                PrintLine("No command history.", ColorOutput);
                return;
            }

            for (int i = 0; i < _commandHistory.Count; i++)
            {
                PrintLine($"  {i + 1}  {_commandHistory[i]}", ColorOutput);
            }
        }

        private void SyscallInfo()
        {
            PrintLine("╔══════════════════════════════════════════════════════════════╗", ColorSystem);
            PrintLine("║                  Syscall Module Information                  ║", ColorSystem);
            PrintLine("╠══════════════════════════════════════════════════════════════╣", ColorSystem);
            PrintLine("║  Module:     syscall.dll                                     ║", ColorOutput);
            PrintLine("║  Language:   x86 Assembly (32-bit)                           ║", ColorOutput);
            PrintLine("║  Convention: stdcall                                         ║", ColorOutput);

            string statusLine = SyscallHelper.IsAvailable
                ? "║  Status:     LOADED                                          ║"
                : "║  Status:     NOT AVAILABLE (simulation mode)                 ║";
            PrintLine(statusLine, SyscallHelper.IsAvailable ? ColorSuccess : ColorWarning);

            PrintLine("║                                                              ║", ColorSystem);
            PrintLine("║  Exported Functions:                                         ║", ColorSystem);
            PrintLine("║    Sys_Init      - Initialize syscall module                 ║", ColorOutput);
            PrintLine("║    Sys_Print     - Print to stdout (native console)          ║", ColorOutput);
            PrintLine("║    Sys_Input     - Read from stdin (native console)          ║", ColorOutput);
            PrintLine("║    Sys_Sleep     - Sleep for milliseconds                    ║", ColorOutput);
            PrintLine("║    Sys_MemAlloc  - Allocate heap memory                      ║", ColorOutput);
            PrintLine("║    Sys_MemFree   - Free heap memory                          ║", ColorOutput);
            PrintLine("║    Sys_FileCreate, Sys_FileWrite, Sys_FileRead               ║", ColorOutput);
            PrintLine("║    Sys_DirCreate, Sys_Log                                    ║", ColorOutput);
            PrintLine("╚══════════════════════════════════════════════════════════════╝", ColorSystem);
        }

        #endregion

        #region Power & Utility Commands

        private void ShutdownCommand()
        {
            PrintLine("[SYSTEM] Initiating shutdown sequence...", ColorWarning);
            SyscallHelper.Print("[SYSTEM] Initiating shutdown sequence...\r\n");
            PrintLine("[SYSTEM] Saving state...", ColorWarning);
            PrintLine("[SYSTEM] Goodbye.", ColorSystem);
            SyscallHelper.Log("System shutdown initiated from console");
        }

        private void RebootCommand()
        {
            PrintLine("[SYSTEM] Initiating reboot sequence...", ColorWarning);
            SyscallHelper.Print("[SYSTEM] Initiating reboot sequence...\r\n");
            PrintLine("[SYSTEM] Restarting services...", ColorWarning);
            SyscallHelper.Log("System reboot initiated from console");
        }

        private void SleepCommand(string args)
        {
            if (string.IsNullOrWhiteSpace(args))
            {
                PrintLine("Usage: sleep <milliseconds>", ColorWarning);
                return;
            }

            if (int.TryParse(args, out int ms) && ms > 0)
            {
                PrintLine($"Sleeping for {ms}ms...", ColorOutput);
                SyscallHelper.Sleep(ms);
                PrintLine("Awake.", ColorSuccess);
            }
            else
            {
                PrintLine("Invalid milliseconds value.", ColorError);
            }
        }

        private void LogCommand(string args)
        {
            if (string.IsNullOrWhiteSpace(args))
            {
                PrintLine("Usage: log <message>", ColorWarning);
                return;
            }

            if (SyscallHelper.IsAvailable)
            {
                SyscallHelper.Log(args);
                PrintLine($"Logged: {args}", ColorSuccess);
            }
            else
            {
                PrintLine($"[SIM] Would log: {args}", ColorWarning);
                PrintLine("Note: Syscall module not available.", ColorWarning);
            }
        }

        #endregion

        #region UI Helpers

        private void ClearConsole()
        {
            rtbConsoleOutput.Clear();
            lblStatus.Text = "Console cleared";
        }

        private void ExportConsole()
        {
            try
            {
                string filename = $"console_output_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), filename);
                File.WriteAllText(path, rtbConsoleOutput.Text);
                PrintLine($"Console exported to: {path}", ColorSuccess);
                lblStatus.Text = "Console exported";
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ConsoleControl.ExportConsole failed: {ex.Message}");
                PrintLine($"Export failed: {ex.Message}", ColorError);
            }
        }

        private void PrintLine(string text, Color color)
        {
            rtbConsoleOutput.SelectionStart = rtbConsoleOutput.TextLength;
            rtbConsoleOutput.SelectionColor = color;
            rtbConsoleOutput.AppendText(text + Environment.NewLine);
            rtbConsoleOutput.SelectionColor = rtbConsoleOutput.ForeColor;
        }

        private static string FormatSize(long bytes)
        {
            if (bytes < 1024) return $"{bytes} B";
            if (bytes < 1024 * 1024) return $"{bytes / 1024.0:F1} KB";
            return $"{bytes / (1024.0 * 1024.0):F2} MB";
        }

        #endregion
    }
}
