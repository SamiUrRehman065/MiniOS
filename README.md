### ??? MiniOS - Educational Operating System Simulation

> A comprehensive operating system simulation featuring an x86 Assembly bootloader, C# WinForms kernel application, and native syscall library — built for educational purposes to demonstrate core OS concepts with professional-grade architecture.

---

### ?? Project Overview

- ?? x86 Assembly bootloader with graphical boot sequence  
- ??? Windows Forms kernel with modern UI (Guna.UI2)  
- ?? Native syscall layer (syscall.dll) for low-level operations  
- ?? Virtual File System with security sandbox  
- ?? Real-time process and memory monitoring  
- ?? Full-featured console with 35+ commands  
- ?? System logging with categorized entries  
- ?? Responsive dark-themed UI with animations

---

### ?? Folder Structure

```plaintext
MiniOS/
??? BootLoader/
?   ??? bootloader.asm          ? x86 Assembly bootloader source
?   ??? bootloader.exe          ? Compiled bootloader executable
?
??? KernelApp/
?   ??? UserControls/
?   ?   ??? ConsoleControl.cs           ? Command-line interface
?   ?   ??? ProcessMgrControl.cs        ? Process manager
?   ?   ??? MemoryVisControl.cs         ? Memory visualization
?   ?   ??? SysLogControl.cs            ? System log viewer
?   ?
?   ??? Resources/
?   ?   ??? Documentation.md            ? Full documentation
?   ?   ??? FlowDiagrams.md             ? System flow diagrams
?   ?
?   ??? Home.cs                 ? Main application form
?   ??? KernelState.cs          ? Shared kernel state
?   ??? Program.cs              ? Application entry point
?
??? SysCall/
?   ??? syscall.asm             ? Native syscall source
?   ??? syscall.dll             ? Compiled native DLL
?
??? VFS/                        ? Virtual File System root
?   ??? (user files)
?
??? Logs/
?   ??? kernel.log              ? Kernel log file
?
??? MiniOS.sln                  ? Solution file
```

---

### ? Why This Architecture Works

- **Layered Design** separates boot, kernel, syscall, and resource layers
- **Native Interop** demonstrates real P/Invoke and DLL communication
- **Security Sandbox** prevents file system escapes via VFS
- **Fallback Mechanisms** ensure functionality without syscall.dll
- **Educational Focus** makes OS concepts tangible and interactive

---

### ?? Setup Instructions

1. **Clone the Repository**

```sh
git clone https://github.com/SamiUrRehman065/MiniOS.git
```

2. **Prerequisites**
   - Visual Studio 2019 or later
   - .NET Framework 4.7.2
   - Platform: x86 (32-bit)
   - NuGet Package: Guna.UI2.WinForms

3. **Build the Solution**
   - Open `MiniOS.sln` in Visual Studio
   - Set platform to **x86**
   - Restore NuGet packages
   - Build ? Build Solution (Ctrl+Shift+B)

4. **Run the Application**
   - **Full Boot:** Execute `BootLoader\bootloader.exe`
   - **Direct Launch:** Execute `KernelApp\bin\x86\Debug\KernelApp.exe`

---

### ?? Module Breakdown

| Module | Purpose | Technology | Status |
|--------|---------|------------|--------|
| `bootloader.asm` | Graphical boot sequence with countdown | x86 Assembly | ? Complete |
| `Home.cs` | Main kernel form with navigation | C# WinForms | ? Complete |
| `ConsoleControl` | Command-line interface (35+ commands) | C# | ? Complete |
| `ProcessMgrControl` | Process monitoring and display | C# | ? Complete |
| `MemoryVisControl` | Memory visualization with segments | C# | ? Complete |
| `SysLogControl` | System log viewer with categories | C# | ? Complete |
| `syscall.dll` | Native syscall library | x86 Assembly | ? Complete |
| `KernelState` | Shared state management | C# | ? Complete |
| `VFS` | Virtual file system with sandbox | C# | ? Complete |

---

### ?? Architecture Notes

- **Bootloader:** x86 Assembly with Windows API (StdCall convention)  
- **Kernel:** C# 7.3 on .NET Framework 4.7.2  
- **UI Framework:** Windows Forms with Guna.UI2.WinForms  
- **Native Layer:** P/Invoke to syscall.dll  
- **Security:** VFS sandboxing, path validation, access control  
- **Fallback:** Graceful degradation when syscall.dll unavailable

---

### ?? Suggested Enhancements

- ?? Add script execution support (.sh/.bat files)  
- ?? Implement command piping (cmd1 | cmd2)  
- ?? Add multi-user support with permissions  
- ?? Simulate network commands (ping, netstat)  
- ?? Add kernel analytics dashboard  
- ?? Implement theme switching (dark/light)

---

### ? Developer Reflection

> "MiniOS represents a deep dive into operating system fundamentals — from writing x86 Assembly for the bootloader to implementing P/Invoke for native syscalls. This project bridges the gap between theoretical OS concepts and practical implementation. Every component, from the 35-second boot countdown to the VFS security sandbox, was designed to make operating system internals tangible and educational."  
> — *Sami Ur Rehman*

---

### ????? Author

**Sami Ur Rehman**  
Karachi, Pakistan  
GitHub: [@SamiUrRehman065](https://github.com/SamiUrRehman065)

---

# ?? Detailed Overview Of Each Module

---

## ?? Bootloader Module

### ?? Overview
The bootloader is a 32-bit x86 Assembly program that simulates a real BIOS/UEFI boot process with visual feedback, progress bars, and audio cues before launching the kernel application.

### ?? Backend Logic
- **File:** `bootloader.asm`
- **Architecture:** x86 (32-bit), Flat memory model, StdCall convention
- **Boot Flow:**
  - Initialize console (GetStdHandle, SetConsoleTitleA)
  - Display ASCII logo with animation
  - Execute 5 boot phases with progress bar
  - 35-second countdown with beeps
  - Launch KernelApp.exe via CreateProcessA

### ?? Frontend Features
- **Visual Elements:**
  - ASCII art MiniOS logo
  - Color-coded status messages (11 colors)
  - 50-segment progress bar
  - Real-time countdown display
- **Audio Feedback:**
  - Boot beep on startup
  - Phase transition beeps
  - Countdown beeps (every 5 seconds)
  - Final launch fanfare

### ?? Boot Phases
| Phase | Description | Checks |
|-------|-------------|--------|
| Phase 1 | Hardware Detection | CPU, Cache, RAM, Memory Test |
| Phase 2 | Storage & I/O | Disk, Partition, File System, I/O Ports |
| Phase 3 | Kernel Loading | Boot Sector, Kernel Image, Decompression |
| Phase 4 | System Services | Scheduler, Memory Manager, VFS, Logger |
| Phase 5 | Final Checks | Integrity Verification, Ready Status |

### ?? Windows API Functions
- `GetStdHandle`, `WriteConsoleA`, `SetConsoleTextAttribute`
- `SetConsoleCursorPosition`, `FillConsoleOutputCharacterA`
- `Sleep`, `Beep`, `CreateProcessA`, `ExitProcess`

### ?? Suggested Enhancements
- Add boot configuration options
- Implement boot menu for multiple kernels
- Add keyboard interrupt handling

### ? Status
`? Complete`

---

## ??? Main Form Module (Home.cs)

### ?? Overview
The primary application window containing navigation, status bar, and dynamic content panel with modern UI and animations.

### ?? Backend Logic
- **File:** `Home.cs`
- **Base Class:** `Form`
- **Core Functions:**
  - `LoadControl()`: Async loading of UserControls
  - `AnimateIndicator()`: Smooth sidebar animation
  - `clockTimer_Tick()`: Real-time CPU/Memory monitoring

### ?? Frontend Features
- **Header Panel:**
  - Draggable title bar (P/Invoke)
  - Window controls (Minimize, Maximize, Close)
  - Real-time CPU, Memory, Time display
- **Sidebar Navigation:**
  - Process Manager button
  - System Console button
  - Memory Usage button
  - Syscall Logs button
  - Animated active indicator
- **Main Content Panel:**
  - Dynamic UserControl loading
  - Smooth view transitions

### ?? Security & Validation
- P/Invoke for window dragging (user32.dll)
- PerformanceCounter for system monitoring
- Async loading prevents UI freezing

### ?? P/Invoke Declarations
```csharp
[DllImport("user32.dll")]
private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

[DllImport("user32.dll")]
private static extern bool ReleaseCapture();
```

### ?? Suggested Enhancements
- Add window state persistence
- Implement keyboard shortcuts
- Add system tray support

### ? Status
`? Complete`

---

## ?? Console Control Module

### ?? Overview
Full-featured command-line interface with 35+ commands, environment variables, command history, and native syscall integration.

### ?? Backend Logic
- **File:** `ConsoleControl.cs`
- **Core Functions:**
  - `ExecuteCommand()`: Process user input
  - `ProcessCommand()`: Parse and dispatch commands
  - `ExpandVariables()`: Replace $VAR with values
  - `Sys_*()`: Syscall wrapper methods with fallback

### ?? Frontend Features
- **Console Output:**
  - Color-coded messages (6 colors)
  - Scrollable RichTextBox
  - Monospace font (Consolas)
- **Command Input:**
  - Single-line TextBox
  - History navigation (Up/Down arrows)
  - Enter to execute

### ?? Command Categories

| Category | Commands |
|----------|----------|
| System | help, clear, version, sysinfo, uptime, syscall |
| File System | pwd, ls, cd, mkdir, rmdir, touch, cat, write, rm |
| Environment | set, env, echo, whoami, hostname |
| Process | ps, mem, alloc |
| I/O | print, read |
| Utilities | date, time, sleep, log, history |
| Power | shutdown, reboot |

### ?? Security & Validation
- VFS sandbox (cannot escape /VFS folder)
- Path validation on all file operations
- Parameterized syscall wrappers
- Access denied logging

### ??? Environment Variables
| Variable | Default | Description |
|----------|---------|-------------|
| USER | root | Current user |
| HOSTNAME | minios-kernel | System name |
| SHELL | MiniOS Console | Shell name |
| PATH | /bin:/usr/bin | Search path |
| HOME | / | Home directory |
| PWD | (dynamic) | Current directory |

### ?? Suggested Enhancements
- Add script execution support
- Implement command aliases
- Add tab completion

### ? Status
`? Complete`

---

## ?? Syscall DLL Module

### ?? Overview
Native 32-bit DLL providing low-level system operations via Windows API, with P/Invoke integration from C#.

### ?? Backend Logic
- **File:** `syscall.dll` (Source: `syscall.asm`)
- **Architecture:** x86 (32-bit), StdCall convention
- **Entry Point:** `DllMain`

### ?? Exported Functions

| Function | Parameters | Return | Description |
|----------|------------|--------|-------------|
| `Sys_Init` | (none) | void | Initialize module |
| `Sys_Log` | message | void | Write to kernel.log |
| `Sys_Print` | message | void | Print to stdout |
| `Sys_Input` | buffer, maxLen | void | Read from stdin |
| `Sys_Sleep` | milliseconds | void | Sleep thread |
| `Sys_MemAlloc` | sizeBytes | IntPtr | Allocate heap |
| `Sys_MemFree` | memPtr | int | Free heap |
| `Sys_FileCreate` | filename | int | Create file |
| `Sys_FileWrite` | filename, text | int | Write to file |
| `Sys_FileRead` | filename, buffer, maxLen | int | Read file |
| `Sys_DirCreate` | dirPath | int | Create directory |

### ?? Fallback Mechanism
When syscall.dll is unavailable:
- File operations ? `System.IO`
- Sleep ? `Thread.Sleep`
- Memory operations ? Disabled (simulation message)

### ?? Security & Validation
- All functions use Windows API internally
- HeapAlloc/HeapFree for memory management
- CreateFileA/WriteFile/ReadFile for file I/O

### ?? Suggested Enhancements
- Add network syscalls
- Implement process creation
- Add registry operations

### ? Status
`? Complete`

---

## ?? Virtual File System Module

### ?? Overview
Sandboxed file system allowing safe file operations within the /VFS directory, preventing access to host system files.

### ?? Backend Logic
- **Root Path:** `<ProjectPath>/VFS/`
- **Initialization:** Auto-creates VFS folder if missing
- **Path Resolution:** Converts relative to absolute paths
- **Security Check:** Validates all paths stay within VFS

### ?? Frontend Features
- **Directory Listing:**
  - Color-coded [DIR] and [FILE] entries
  - File sizes formatted (B, KB, MB)
  - Directory/file counts
- **Navigation:**
  - `cd` with `.`, `..`, `/`, `~` support
  - `pwd` shows current location

### ?? Supported Operations

| Operation | Command | Method |
|-----------|---------|--------|
| List | `ls [path]` | `ListDirectory()` |
| Navigate | `cd <path>` | `ChangeDirectory()` |
| Create Dir | `mkdir <name>` | `MakeDirectory()` |
| Remove Dir | `rmdir <name>` | `RemoveDirectory()` |
| Create File | `touch <file>` | `TouchFile()` |
| Read File | `cat <file>` | `CatFile()` |
| Write File | `write <file> <text>` | `WriteToFile()` |
| Delete File | `rm <file>` | `DeleteFile()` |

### ?? Security & Validation
```csharp
// Security check prevents VFS escape
if (!newPath.StartsWith(vfsFolder, StringComparison.OrdinalIgnoreCase))
{
    PrintLine("cd: Access denied - Cannot navigate outside VFS", colorError);
    Sys_Log($"CD DENIED: Attempted to access {path}");
    return;
}
```

### ?? Suggested Enhancements
- Add file permissions (rwx)
- Implement symbolic links
- Add file search functionality

### ? Status
`? Complete`

---

## ?? Process Manager Module

### ?? Overview
Displays simulated process list with PID, name, status, and memory usage in a professional grid layout.

### ?? Backend Logic
- **File:** `ProcessMgrControl.cs`
- **Data Source:** `KernelState.GetProcessList()`
- **Refresh:** Timer-based auto-refresh

### ?? Frontend Features
- **DataGridView:**
  - PID, Process Name, Status, Memory columns
  - Color-coded status (Running = Green)
  - Row selection highlighting
- **Status Panel:**
  - Total process count
  - Memory usage summary

### ?? Process Properties
| Property | Type | Description |
|----------|------|-------------|
| PID | int | Process identifier |
| ProcessName | string | Process name |
| Status | string | Running/Stopped |
| MemoryMB | int | Memory in MB |

### ?? Security & Validation
- Thread-safe access via KernelState
- Deep copy of process list prevents modification
- Timer disposal on control unload

### ?? Suggested Enhancements
- Add process termination
- Implement sorting/filtering
- Add CPU usage column

### ? Status
`? Complete`

---

## ?? Memory Visualization Module

### ?? Overview
Visual representation of memory allocation with segment bars, statistics, and color-coded usage levels.

### ?? Backend Logic
- **File:** `MemoryVisControl.cs`
- **Total Memory:** 4096 MB (simulated)
- **Refresh:** Timer-based with change detection

### ?? Frontend Features
- **Segment Panel:**
  - Color-coded memory blocks per process
  - Free memory shown in gray
  - Proportional width based on usage
- **Statistics:**
  - Total, Used, Free memory
  - Usage percentage with color indicator
- **Process Grid:**
  - Memory per process with colors

### ?? Color Coding
| Usage Level | Color | Indicator |
|-------------|-------|-----------|
| < 60% | Green | Normal |
| 60-80% | Yellow | Warning |
| > 80% | Red | High |
| > 90% | Red + Log | Critical |

### ?? Security & Validation
- Hash-based change detection
- Memory change threshold logging (>50MB)
- Thread-safe state access

### ?? Suggested Enhancements
- Add memory allocation history
- Implement memory leak detection
- Add swap space visualization

### ? Status
`? Complete`

---

## ?? System Log Module

### ?? Overview
Displays kernel log entries with categorization, color-coding, filtering, and export capabilities.

### ?? Backend Logic
- **File:** `SysLogControl.cs`
- **Log Source:** `Logs/kernel.log`
- **Auto-Refresh:** 3-second interval (toggleable)

### ?? Frontend Features
- **DataGridView:**
  - Timestamp, Type, Message columns
  - Color-coded by log type
- **RichTextBox:**
  - Raw log content view
  - Scrollable with monospace font
- **Control Buttons:**
  - Refresh, Clear, Export, Auto-refresh toggle

### ?? Log Entry Types
| Type | Color | Detection Keywords |
|------|-------|-------------------|
| CMD | Blue | "CMD:", "COMMAND" |
| ERROR | Red | "ERROR", "FAIL" |
| WARN | Yellow | "WARN" |
| SYSTEM | Green | "INIT", "BOOT", "START" |
| POWER | Green | "SHUTDOWN", "REBOOT" |
| MEM | Cyan | "ALLOC", "FREE", "MEMORY" |
| PROC | Cyan | "PROCESS", "PID" |
| INFO | Gray | Default |

### ?? Security & Validation
- File existence check before read
- Exception handling for file access
- Timer disposal on control unload

### ?? Suggested Enhancements
- Add log filtering by type
- Implement log search
- Add log rotation

### ? Status
`? Complete`

---

## ?? Kernel State Module

### ?? Overview
Thread-safe shared state management for process list and kernel-wide data across all controls.

### ?? Backend Logic
- **File:** `KernelState.cs`
- **Pattern:** Singleton with lock-based synchronization
- **Thread Safety:** All operations protected by lock

### ?? Key Methods
```csharp
public static List<ProcessInfo> GetProcessList()
public static void UpdateProcessList(List<ProcessInfo> processes)
public static int AddProcess(string name, int memoryMB)
public static bool RemoveProcess(int pid)
```

### ?? Security & Validation
- Deep copy on get/set prevents external modification
- Lock ensures thread-safe access
- Null checks on all operations

### ?? Suggested Enhancements
- Add event notifications for state changes
- Implement state persistence
- Add state history/undo

### ? Status
`? Complete`

---

## ?? Error Handling Module

### ?? Overview
Comprehensive error handling with graceful degradation and user-friendly messages throughout the application.

### ?? Backend Logic
- **Try-Catch Blocks:** All critical operations wrapped
- **Fallback Mechanisms:** syscall.dll unavailable ? managed code
- **User Feedback:** Color-coded error messages in console

### ?? Security Measures
- No stack traces exposed to users
- Exception details logged to kernel.log
- Input sanitization on all commands

### ?? Frontend Features
- **Error Messages:**
  - Red color for errors
  - Yellow for warnings
  - Context-specific messages

### ?? Suggested Enhancements
- Add global exception handler
- Implement error analytics
- Add error recovery suggestions

### ? Status
`? Complete`

---

## ?? Power Management Module

### ?? Overview
Simulated shutdown and reboot commands with visual feedback and logging.

### ?? Backend Logic
- **Commands:** `shutdown`, `exit`, `reboot`
- **Actions:**
  - Display shutdown/reboot sequence
  - Log power events
  - Print to native console (if available)

### ?? Frontend Features
- **Visual Feedback:**
  - Warning-colored messages
  - Multi-step sequence display
  - Goodbye message

### ?? Security & Validation
- Events logged to kernel.log
- Optional confirmation prompts

### ?? Suggested Enhancements
- Add actual application shutdown
- Implement auto-save before shutdown
- Add scheduled shutdown

### ? Status
`? Complete`

---

## ?? Performance Metrics

### Startup Times
| Component | Time |
|-----------|------|
| Bootloader (full sequence) | ~60 seconds |
| KernelApp (direct launch) | < 2 seconds |
| View switching | < 500ms |

### Memory Usage
| State | Working Set |
|-------|-------------|
| Initial load | ~45 MB |
| Console active | ~50 MB |
| All views loaded | ~55 MB |

---

## ??? Build Instructions

### Bootloader (Assembly)
```sh
cd BootLoader
ml /c /coff bootloader.asm
link /subsystem:console /entry:start bootloader.obj kernel32.lib user32.lib
```

### Kernel Application (C#)
```sh
cd KernelApp
nuget restore
msbuild /p:Configuration=Debug /p:Platform=x86
```

### Syscall DLL (Assembly)
```sh
cd SysCall
ml /c /coff syscall.asm
link /DLL /DEF:syscall.def /entry:DllMain syscall.obj kernel32.lib user32.lib
```

---

## ?? License

This project is for **educational purposes only**.

---

## ?? Acknowledgments

- Microsoft for Windows API documentation
- Guna.UI2 for modern WinForms components
- The OS development community for inspiration

---

*Made with ?? for learning Operating System concepts*
