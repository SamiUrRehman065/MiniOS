using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace KernelApp.Helpers
{
    /// <summary>
    /// Centralized syscall helper providing P/Invoke wrappers for syscall.dll
    /// Eliminates duplicated DllImport declarations across UserControls
    /// </summary>
    public static class SyscallHelper
    {
        #region Native P/Invoke Declarations

        [DllImport("syscall.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "Sys_Init")]
        private static extern void Native_Sys_Init();

        [DllImport("syscall.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "Sys_Log", CharSet = CharSet.Ansi)]
        private static extern void Native_Sys_Log(string message);

        [DllImport("syscall.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "Sys_Sleep")]
        private static extern void Native_Sys_Sleep(int milliseconds);

        [DllImport("syscall.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "Sys_Print", CharSet = CharSet.Ansi)]
        private static extern void Native_Sys_Print(string message);

        [DllImport("syscall.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "Sys_Input", CharSet = CharSet.Ansi)]
        private static extern void Native_Sys_Input(StringBuilder buffer, int maxLen);

        [DllImport("syscall.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "Sys_MemAlloc")]
        private static extern IntPtr Native_Sys_MemAlloc(int sizeBytes);

        [DllImport("syscall.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "Sys_MemFree")]
        private static extern int Native_Sys_MemFree(IntPtr memPtr);

        [DllImport("syscall.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "Sys_FileCreate", CharSet = CharSet.Ansi)]
        private static extern int Native_Sys_FileCreate(string filename);

        [DllImport("syscall.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "Sys_FileWrite", CharSet = CharSet.Ansi)]
        private static extern int Native_Sys_FileWrite(string filename, string text);

        [DllImport("syscall.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "Sys_FileRead", CharSet = CharSet.Ansi)]
        private static extern int Native_Sys_FileRead(string filename, StringBuilder buffer, int maxLen);

        [DllImport("syscall.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "Sys_DirCreate", CharSet = CharSet.Ansi)]
        private static extern int Native_Sys_DirCreate(string dirPath);

        #endregion

        #region State Tracking

        private static bool _isAvailable;
        private static bool _isChecked;
        private static readonly object _lock = new object();

        /// <summary>
        /// Indicates whether syscall.dll is loaded and available
        /// </summary>
        public static bool IsAvailable
        {
            get
            {
                EnsureInitialized();
                return _isAvailable;
            }
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Ensures the syscall module is initialized (thread-safe)
        /// </summary>
        public static void EnsureInitialized()
        {
            if (_isChecked) return;

            lock (_lock)
            {
                if (_isChecked) return;

                _isChecked = true;
                try
                {
                    Native_Sys_Init();
                    _isAvailable = true;
                }
                catch (DllNotFoundException)
                {
                    _isAvailable = false;
                    Debug.WriteLine("SyscallHelper: syscall.dll not found");
                }
                catch (EntryPointNotFoundException)
                {
                    _isAvailable = false;
                    Debug.WriteLine("SyscallHelper: Sys_Init entry point not found");
                }
                catch (Exception ex)
                {
                    _isAvailable = false;
                    Debug.WriteLine($"SyscallHelper: Initialization failed - {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Force re-initialization (useful for testing or DLL reload)
        /// </summary>
        public static void Reset()
        {
            lock (_lock)
            {
                _isChecked = false;
                _isAvailable = false;
            }
        }

        #endregion

        #region Logging

        /// <summary>
        /// Log a message using native syscall
        /// </summary>
        public static void Log(string message)
        {
            EnsureInitialized();
            if (!_isAvailable) return;

            try
            {
                Native_Sys_Log(message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"SyscallHelper.Log failed: {ex.Message}");
            }
        }

        #endregion

        #region Console I/O

        /// <summary>
        /// Print to native console (stdout)
        /// </summary>
        public static void Print(string message)
        {
            EnsureInitialized();
            if (!_isAvailable) return;

            try
            {
                Native_Sys_Print(message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"SyscallHelper.Print failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Read input from native console (stdin)
        /// </summary>
        /// <param name="maxLength">Maximum input length</param>
        /// <returns>Input string or empty if unavailable</returns>
        public static string Input(int maxLength = 256)
        {
            EnsureInitialized();
            if (!_isAvailable) return string.Empty;

            try
            {
                var buffer = new StringBuilder(maxLength);
                Native_Sys_Input(buffer, maxLength);
                return buffer.ToString().TrimEnd('\r', '\n');
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"SyscallHelper.Input failed: {ex.Message}");
                return string.Empty;
            }
        }

        #endregion

        #region Sleep

        /// <summary>
        /// Sleep using native syscall with managed fallback
        /// </summary>
        public static void Sleep(int milliseconds)
        {
            EnsureInitialized();

            if (_isAvailable)
            {
                try
                {
                    Native_Sys_Sleep(milliseconds);
                    return;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"SyscallHelper.Sleep failed: {ex.Message}");
                }
            }

            // Fallback to managed sleep
            System.Threading.Thread.Sleep(milliseconds);
        }

        #endregion

        #region Memory Management

        /// <summary>
        /// Allocate memory using native syscall
        /// </summary>
        /// <param name="sizeBytes">Size in bytes to allocate</param>
        /// <param name="logAllocation">Whether to log the allocation</param>
        /// <returns>Pointer to allocated memory, or IntPtr.Zero on failure</returns>
        public static IntPtr MemAlloc(int sizeBytes, bool logAllocation = true)
        {
            EnsureInitialized();
            if (!_isAvailable) return IntPtr.Zero;

            try
            {
                IntPtr ptr = Native_Sys_MemAlloc(sizeBytes);
                if (ptr != IntPtr.Zero && logAllocation)
                {
                    Log($"MEMALLOC: Allocated {sizeBytes} bytes at 0x{ptr.ToInt32():X8}");
                }
                return ptr;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"SyscallHelper.MemAlloc failed: {ex.Message}");
                return IntPtr.Zero;
            }
        }

        /// <summary>
        /// Free memory using native syscall
        /// </summary>
        /// <param name="memPtr">Pointer to memory to free</param>
        /// <param name="logFree">Whether to log the free operation</param>
        /// <returns>True if successful</returns>
        public static bool MemFree(IntPtr memPtr, bool logFree = true)
        {
            if (memPtr == IntPtr.Zero) return false;

            EnsureInitialized();
            if (!_isAvailable) return false;

            try
            {
                int result = Native_Sys_MemFree(memPtr);
                if (result != 0 && logFree)
                {
                    Log($"MEMFREE: Freed memory at 0x{memPtr.ToInt32():X8}");
                }
                return result != 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"SyscallHelper.MemFree failed: {ex.Message}");
                return false;
            }
        }

        #endregion

        #region File System

        /// <summary>
        /// Create a file using native syscall with managed fallback
        /// </summary>
        public static bool FileCreate(string fullPath)
        {
            EnsureInitialized();

            if (_isAvailable)
            {
                try
                {
                    if (Native_Sys_FileCreate(fullPath) != 0)
                        return true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"SyscallHelper.FileCreate native failed: {ex.Message}");
                }
            }

            // Fallback to managed code
            try
            {
                File.Create(fullPath).Close();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"SyscallHelper.FileCreate managed failed: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Write to a file using native syscall with managed fallback
        /// </summary>
        public static bool FileWrite(string fullPath, string text)
        {
            EnsureInitialized();

            if (_isAvailable)
            {
                try
                {
                    if (Native_Sys_FileWrite(fullPath, text) != 0)
                        return true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"SyscallHelper.FileWrite native failed: {ex.Message}");
                }
            }

            // Fallback to managed code
            try
            {
                File.AppendAllText(fullPath, text);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"SyscallHelper.FileWrite managed failed: {ex.Message}");
                return false;
            }
        }

        private const int DefaultFileReadBufferSize = 8192;

        /// <summary>
        /// Read from a file using native syscall with managed fallback
        /// </summary>
        /// <param name="fullPath">Full path to file</param>
        /// <param name="bufferSize">Buffer size for native read</param>
        /// <returns>File contents or null on failure</returns>
        public static string FileRead(string fullPath, int bufferSize = DefaultFileReadBufferSize)
        {
            EnsureInitialized();

            if (_isAvailable)
            {
                try
                {
                    var buffer = new StringBuilder(bufferSize);
                    int bytesRead = Native_Sys_FileRead(fullPath, buffer, bufferSize);
                    if (bytesRead > 0)
                    {
                        return buffer.ToString();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"SyscallHelper.FileRead native failed: {ex.Message}");
                }
            }

            // Fallback to managed code
            try
            {
                if (File.Exists(fullPath))
                {
                    return File.ReadAllText(fullPath);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"SyscallHelper.FileRead managed failed: {ex.Message}");
            }

            return null;
        }

        /// <summary>
        /// Create a directory using native syscall with managed fallback
        /// </summary>
        public static bool DirCreate(string fullPath)
        {
            EnsureInitialized();

            if (_isAvailable)
            {
                try
                {
                    if (Native_Sys_DirCreate(fullPath) != 0)
                        return true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"SyscallHelper.DirCreate native failed: {ex.Message}");
                }
            }

            // Fallback to managed code
            try
            {
                Directory.CreateDirectory(fullPath);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"SyscallHelper.DirCreate managed failed: {ex.Message}");
                return false;
            }
        }

        #endregion
    }
}