using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        [DllImport("syscall.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "Sys_Print", CharSet = CharSet.Ansi)]
        private static extern void Native_Sys_Print(string message);

        [DllImport("syscall.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "Sys_Input", CharSet = CharSet.Ansi)]
        private static extern void Native_Sys_Input(StringBuilder buffer, int maxLen);

        [DllImport("syscall.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "Sys_MemAlloc")]
        private static extern IntPtr Native_Sys_MemAlloc(int sizeBytes);

        [DllImport("syscall.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "Sys_MemFree")]
        private static extern int Native_Sys_MemFree(IntPtr memPtr);

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

        // Environment variables for scripting
        private Dictionary<string, string> envVariables;

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
            envVariables = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            InitializeVirtualFileSystem();
            InitializeEnvironmentVariables();
        }

        private void InitializeEnvironmentVariables()
        {
            envVariables["USER"] = "root";
            envVariables["HOSTNAME"] = "minios-kernel";
            envVariables["SHELL"] = "MiniOS Console";
            envVariables["PATH"] = "/bin:/usr/bin";
            envVariables["HOME"] = "/";
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

        /// <summary>
        /// Print to native console using syscall (writes to stdout)
        /// </summary>
        private static void Sys_Print(string message)
        {
            if (!syscallChecked) CheckSyscallAvailability();
            if (!syscallAvailable) return;

            try
            {
                Native_Sys_Print(message);
            }
            catch { }
        }

        /// <summary>
        /// Read input from native console using syscall (reads from stdin)
        /// </summary>
        private static string Sys_Input(int maxLength = 256)
        {
            if (!syscallChecked) CheckSyscallAvailability();

            if (syscallAvailable)
            {
                try
                {
                    StringBuilder buffer = new StringBuilder(maxLength);
                    Native_Sys_Input(buffer, maxLength);
                    string result = buffer.ToString();
                    // Trim CRLF from input
                    return result.TrimEnd('\r', '\n');
                }
                catch { }
            }

            return string.Empty;
        }

        /// <summary>
        /// Allocate memory using native syscall
        /// </summary>
        private static IntPtr Sys_MemAlloc(int sizeBytes)
        {
            if (!syscallChecked) CheckSyscallAvailability();

            if (syscallAvailable)
            {
                try
                {
                    IntPtr ptr = Native_Sys_MemAlloc(sizeBytes);
                    if (ptr != IntPtr.Zero)
                    {
                        Sys_Log($"MEMALLOC: Console allocated {sizeBytes} bytes at 0x{ptr.ToInt32():X8}");
                    }
                    return ptr;
                }
                catch { }
            }

            return IntPtr.Zero;
        }

        /// <summary>
        /// Free memory using native syscall
        /// </summary>
        private static bool Sys_MemFree(IntPtr memPtr)
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
                        Sys_Log($"MEMFREE: Console freed memory at 0x{memPtr.ToInt32():X8}");
                    }
                    return result != 0;
                }
                catch { }
            }

            return false;
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
                // Also print to native console if available
                Sys_Print("MiniOS Console initialized via syscall.dll\r\n");
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
                PrintLine("[BOOT] Native I/O: Sys_Print, Sys_Input available", colorSuccess);
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

            // Also echo command to native console
            Sys_Print($"{GetPrompt()} {input}\r\n");

            ProcessCommand(input);

            txtCommand.Text = "";
            lblCommandCount.Text = $"Commands: {commandCount}";

            rtbConsoleOutput.SelectionStart = rtbConsoleOutput.Text.Length;
            rtbConsoleOutput.ScrollToCaret();
        }

        private string GetPrompt()
        {
            return $"MiniOS:{GetRelativePath(currentDirectory)  }>";
        }

        /// <summary>
        /// Expand environment variables in a string ($VAR or ${VAR})
        /// </summary>
        private string ExpandVariables(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            foreach (var kvp in envVariables)
            {
                input = input.Replace($"${{{kvp.Key}}}", kvp.Value);
                input = input.Replace($"${kvp.Key}", kvp.Value);
            }

            return input;
        }

        private void ProcessCommand(string input)
        {
            // Expand environment variables
            input = ExpandVariables(input);

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
                    EchoCommand(args);
                    break;
                case "print":
                    PrintCommand(args);
                    break;
                case "read":
                    ReadCommand(args);
                    break;
                case "set":
                    SetCommand(args);
                    break;
                case "env":
                    EnvCommand();
                    break;
                case "ps":
                case "tasklist":
                    ShowProcessList();
                    break;
                case "mem":
                case "memory":
                    ShowMemoryStatus();
                    break;
                case "alloc":
                    AllocCommand(args);
                    break;
                case "sysinfo":
                    ShowSystemInfo();
                    break;
                case "uptime":
                    ShowUptime();
                    break;
                case "whoami":
                    PrintLine(envVariables["USER"], colorOutput);
                    break;
                case "hostname":
                    PrintLine(envVariables["HOSTNAME"], colorOutput);
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

        /// <summary>
        /// Echo command - prints to GUI console
        /// </summary>
        private void EchoCommand(string args)
        {
            PrintLine(args, colorOutput);
        }

        /// <summary>
        /// Print command - opens cmd.exe, echoes message, and shows result in MiniOS console
        /// </summary>
        private void PrintCommand(string args)
        {
            if (string.IsNullOrWhiteSpace(args))
            {
                PrintLine("Usage: print <message>", colorWarning);
                PrintLine("Opens cmd.exe, prints message, and displays result here", colorOutput);
                return;
            }

            try
            {
                PrintLine($"[CMD] Opening command prompt to print: {args}", colorSystem);

                using (Process process = new Process())
                {
                    process.StartInfo.FileName = "cmd.exe";
                    process.StartInfo.Arguments = $"/c echo {args}";
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.CreateNoWindow = false; // Show the cmd window
                    process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;

                    process.Start();

                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    process.WaitForExit();

                    if (!string.IsNullOrEmpty(output))
                    {
                        PrintLine($"[CMD OUTPUT] {output.Trim()}", colorSuccess);
                    }

                    if (!string.IsNullOrEmpty(error))
                    {
                        PrintLine($"[CMD ERROR] {error.Trim()}", colorError);
                    }

                    PrintLine($"[CMD] Process exited with code: {process.ExitCode}", colorOutput);
                }

                Sys_Log($"PRINT CMD: {args}");
            }
            catch (Exception ex)
            {
                PrintLine($"[ERROR] Failed to execute print command: {ex.Message}", colorError);
            }
        }

        /// <summary>
        /// Read command - opens cmd.exe with set /p for user input, captures result
        /// Stores result in environment variable
        /// </summary>
        private void ReadCommand(string args)
        {
            string varName = string.IsNullOrWhiteSpace(args) ? "INPUT" : args.Trim().ToUpper();

            try
            {
                PrintLine($"[CMD] Opening command prompt for input into ${varName}...", colorSystem);
                PrintLine("(Enter your input in the cmd window that opens)", colorWarning);

                // Create a temporary batch file for input
                string tempBatchFile = Path.Combine(Path.GetTempPath(), "minios_input.bat");
                string tempOutputFile = Path.Combine(Path.GetTempPath(), "minios_input_result.txt");

                // Write batch file that prompts for input and saves to temp file
                string batchContent = $@"@echo off
echo ═══════════════════════════════════════════════════
echo         MiniOS Native Input - Enter value for {varName}
echo ═══════════════════════════════════════════════════
set /p USERINPUT=""Enter {varName}: ""
echo %USERINPUT%> ""{tempOutputFile}"")
echo.
echo Input captured! This window will close...
timeout /t 2 >nul
";
                File.WriteAllText(tempBatchFile, batchContent);

                // Delete old output file if exists
                if (File.Exists(tempOutputFile))
                {
                    File.Delete(tempOutputFile);
                }

                using (Process process = new Process())
                {
                    process.StartInfo.FileName = "cmd.exe";
                    process.StartInfo.Arguments = $"/c \"{tempBatchFile}\"";
                    process.StartInfo.UseShellExecute = true; // Opens visible cmd window
                    process.StartInfo.CreateNoWindow = false;
                    process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;

                    process.Start();
                    process.WaitForExit(); // Wait for user to finish input
                }

                // Read the result from temp file
                if (File.Exists(tempOutputFile))
                {
                    string input = File.ReadAllText(tempOutputFile).Trim();

                    if (!string.IsNullOrEmpty(input))
                    {
                        envVariables[varName] = input;
                        PrintLine($"[CMD INPUT] Read: '{input}'", colorSuccess);
                        PrintLine($"Stored in ${varName}", colorOutput);
                        Sys_Log($"INPUT CMD: Read '{input}' into ${varName}");
                    }
                    else
                    {
                        PrintLine("No input received or empty input.", colorWarning);
                    }

                    // Cleanup temp files
                    File.Delete(tempOutputFile);
                }
                else
                {
                    PrintLine("Input was cancelled or failed.", colorWarning);
                }

                // Cleanup batch file
                if (File.Exists(tempBatchFile))
                {
                    File.Delete(tempBatchFile);
                }
            }
            catch (Exception ex)
            {
                PrintLine($"[ERROR] Failed to execute read command: {ex.Message}", colorError);
            }
        }

        /// <summary>
        /// Set command - sets environment variable
        /// </summary>
        private void SetCommand(string args)
        {
            if (string.IsNullOrWhiteSpace(args))
            {
                PrintLine("Usage: set <NAME>=<value>", colorWarning);
                PrintLine("Example: set MYVAR=hello", colorOutput);
                return;
            }

            int eqIndex = args.IndexOf('=');
            if (eqIndex <= 0)
            {
                // Just show variable value
                string varName = args.Trim().ToUpper();
                if (envVariables.TryGetValue(varName, out string value))
                {
                    PrintLine($"{varName}={value}", colorOutput);
                }
                else
                {
                    PrintLine($"Variable not found: {varName}", colorWarning);
                }
                return;
            }

            string name = args.Substring(0, eqIndex).Trim().ToUpper();
            string val = args.Substring(eqIndex + 1);

            envVariables[name] = val;
            PrintLine($"Set {name}={val}", colorSuccess);
            Sys_Log($"ENV: Set {name}={val}");
        }

        /// <summary>
        /// Env command - shows all environment variables
        /// </summary>
        private void EnvCommand()
        {
            PrintLine("Environment Variables:", colorSystem);
            PrintLine("──────────────────────────────────────", colorSystem);

            foreach (var kvp in envVariables)
            {
                PrintLine($"  {kvp.Key}={kvp.Value}", colorOutput);
            }

            PrintLine($"\nTotal: {envVariables.Count} variables", colorOutput);
        }

        /// <summary>
        /// Alloc command - test memory allocation syscall
        /// </summary>
        private void AllocCommand(string args)
        {
            if (string.IsNullOrWhiteSpace(args))
            {
                PrintLine("Usage: alloc <bytes>", colorWarning);
                PrintLine("Allocates memory using Sys_MemAlloc syscall (test only)", colorOutput);
                return;
            }

            if (!int.TryParse(args, out int bytes) || bytes <= 0)
            {
                PrintLine("Invalid size. Please specify a positive integer.", colorError);
                return;
            }

            if (bytes > 1024 * 1024) // Limit to 1MB for safety
            {
                PrintLine("Size too large. Maximum 1MB for testing.", colorError);
                return;
            }

            if (syscallAvailable)
            {
                IntPtr ptr = Sys_MemAlloc(bytes);
                if (ptr != IntPtr.Zero)
                {
                    PrintLine($"Allocated {bytes} bytes at address 0x{ptr.ToInt32():X8}", colorSuccess);

                    // Immediately free the test allocation
                    if (Sys_MemFree(ptr))
                    {
                        PrintLine($"Freed memory at 0x{ptr.ToInt32():X8}", colorSuccess);
                    }
                }
                else
                {
                    PrintLine("Memory allocation failed.", colorError);
                }
            }
            else
            {
                PrintLine("[SIM] Would allocate memory via syscall.", colorWarning);
                PrintLine("Note: Syscall module not available.", colorWarning);
            }
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
                DirectoryInfo parent = Directory.GetParent(currentDirectory);
                if (parent != null && currentDirectory != vfsFolder)
                {
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

            if (!newPath.StartsWith(vfsFolder, StringComparison.OrdinalIgnoreCase))
            {
                PrintLine("cd: Access denied - Cannot navigate outside VFS", colorError);
                Sys_Log($"CD DENIED: Attempted to access {path}");
                return;
            }

            if (Directory.Exists(newPath))
            {
                currentDirectory = newPath;
                envVariables["PWD"] = GetRelativePath(newPath);
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
                foreach (var dir in Directory.GetDirectories(targetDir))
                {
                    string name = Path.GetFileName(dir);
                    PrintLine($"  [DIR]  {name}/", Color.FromArgb(99, 102, 241));
                }

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
                    // Also print to native console
                    Sys_Print(content);
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
            PrintLine("║  I/O (Native Syscalls):                                      ║", colorSystem);
            PrintLine("║    print <msg> - Print to native console (Sys_Print)         ║", colorOutput);
            PrintLine("║    read [var]  - Read from native console (Sys_Input)        ║", colorOutput);
            PrintLine("║    alloc <n>   - Test memory allocation (Sys_MemAlloc)       ║", colorOutput);
            PrintLine("║                                                              ║", colorSystem);
            PrintLine("║  ENVIRONMENT:                                                ║", colorSystem);
            PrintLine("║    set <VAR>=<val> - Set environment variable                ║", colorOutput);
            PrintLine("║    env            - Show all environment variables           ║", colorOutput);
            PrintLine("║    echo $VAR      - Print variable value                     ║", colorOutput);
            PrintLine("║                                                              ║", colorSystem);
            PrintLine("║  PROCESS:                                                    ║", colorSystem);
            PrintLine("║    ps         - List running processes                       ║", colorOutput);
            PrintLine("║    mem        - Show memory usage                            ║", colorOutput);
            PrintLine("║                                                              ║", colorSystem);
            PrintLine("║  FILE SYSTEM:                                                ║", colorSystem);
            PrintLine("║    pwd, ls, cd, mkdir, rmdir, touch, cat, write, rm          ║", colorOutput);
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
            PrintLine($"║  Hostname:     {envVariables["HOSTNAME"],-22}       ║", colorOutput);
            PrintLine($"║  User:         {envVariables["USER"],-22}       ║", colorOutput);
            PrintLine("║  Shell:        MiniOS Console                     ║", colorOutput);
            string syscallStatus = syscallAvailable ? "LOADED" : "SIMULATION";
            PrintLine($"║  Syscall:      {syscallStatus,-22}       ║", syscallAvailable ? colorSuccess : colorWarning);
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
            Sys_Print("[SYSTEM] Initiating shutdown sequence...\r\n");
            PrintLine("[SYSTEM] Saving state...", colorWarning);
            PrintLine("[SYSTEM] Goodbye.", colorSystem);
            Sys_Log("System shutdown initiated from console");
        }

        private void RebootCommand()
        {
            PrintLine("[SYSTEM] Initiating reboot sequence...", colorWarning);
            Sys_Print("[SYSTEM] Initiating reboot sequence...\r\n");
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
            PrintLine("║    Sys_Init      - Initialize syscall module                 ║", colorOutput);
            PrintLine("║    Sys_Print     - Print to stdout (native console)          ║", colorOutput);
            PrintLine("║    Sys_Input     - Read from stdin (native console)          ║", colorOutput);
            PrintLine("║    Sys_Sleep     - Sleep for milliseconds                    ║", colorOutput);
            PrintLine("║    Sys_MemAlloc  - Allocate heap memory                      ║", colorOutput);
            PrintLine("║    Sys_MemFree   - Free heap memory                          ║", colorOutput);
            PrintLine("║    Sys_FileCreate, Sys_FileWrite, Sys_FileRead               ║", colorOutput);
            PrintLine("║    Sys_DirCreate, Sys_Log                                    ║", colorOutput);
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
