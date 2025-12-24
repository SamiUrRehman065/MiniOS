; ============================================================
; bootloader.asm — MiniOS Boot Loader (User-Mode Simulation)
; Purpose:
;   1) Show boot messages (assembly, console)
;   2) Launch KernelApp.exe (our simulated kernel)
; ============================================================

.386                                ; target 80386 instruction set
.model flat, stdcall                 ; flat memory model, stdcall calling convention
option casemap:none                  ; case-sensitive identifiers

; ------------------------------------------------------------
; WinAPI prototypes (manual, no .inc files)
; ------------------------------------------------------------
GetStdHandle      PROTO :DWORD      ; prototype for GetStdHandle(handle) -> HANDLE
WriteConsoleA     PROTO :DWORD, :DWORD, :DWORD, :DWORD, :DWORD ; WriteConsoleA(hConsole, lpBuffer, nNumberOfCharsToWrite, lpNumberOfCharsWritten, lpReserved)
Sleep             PROTO :DWORD      ; Sleep(milliseconds)
CreateProcessA    PROTO :DWORD, :DWORD, :DWORD, :DWORD, :DWORD, :DWORD, :DWORD, :DWORD, :DWORD, :DWORD ; CreateProcessA(...)

; ------------------------------------------------------------
; Constants
; ------------------------------------------------------------
STD_OUTPUT_HANDLE EQU -11           ; standard output handle constant (-11)

; ------------------------------------------------------------
; Structures (manual definitions)
; ------------------------------------------------------------

STARTUPINFO STRUCT                   ; STARTUPINFO structure begin
    cb              DWORD ?         ; size of the structure in bytes
    lpReserved      DWORD ?         ; reserved, must be NULL
    lpDesktop       DWORD ?         ; desktop name
    lpTitle         DWORD ?         ; console title
    dwX             DWORD ?         ; x position
    dwY             DWORD ?         ; y position
    dwXSize         DWORD ?         ; width
    dwYSize         DWORD ?         ; height
    dwXCountChars   DWORD ?         ; columns
    dwYCountChars   DWORD ?         ; rows
    dwFillAttribute DWORD ?         ; fill attribute
    dwFlags         DWORD ?         ; startup flags
    wShowWindow     WORD  ?         ; show window flag
    cbReserved2     WORD  ?         ; reserved2 size
    lpReserved2     DWORD ?         ; reserved2 pointer
    hStdInput       DWORD ?         ; standard input handle
    hStdOutput      DWORD ?         ; standard output handle
    hStdError       DWORD ?         ; standard error handle
STARTUPINFO ENDS                     ; END of STARTUPINFO structure

PROCESS_INFORMATION STRUCT           ; PROCESS_INFORMATION structure begin
    hProcess DWORD ?                 ; process handle
    hThread  DWORD ?                 ; thread handle
    dwProcessId DWORD ?              ; process id
    dwThreadId  DWORD ?              ; thread id
PROCESS_INFORMATION ENDS             ; END of PROCESS_INFORMATION structure

; ------------------------------------------------------------
; Data section
; ------------------------------------------------------------
.data                                ; data segment start

bootMsg1 db "Booting MiniOS Kernel...", 13, 10, 0 ; first boot message with CRLF and NUL
bootMsg2 db "Initializing System Memory...", 13, 10, 0 ; second boot message with CRLF and NUL
bootMsg3 db "Launching KernelApp.exe...", 13, 10, 0 ; third boot message with CRLF and NUL
bootMsg4 db "Kernel handoff complete.", 13, 10, 0 ; final boot message with CRLF and NUL

kernelPath db "..\KernelApp\bin\x86\Debug\KernelApp.exe", 0 ; path to the kernel executable, null-terminated

startupInfo STARTUPINFO <>           ; allocate STARTUPINFO structure instance
processInfo PROCESS_INFORMATION <>   ; allocate PROCESS_INFORMATION structure instance

; ------------------------------------------------------------
; Code section
; ------------------------------------------------------------
.code                                ; code segment start
start:                               ; program entry point

    ; === Simulated boot delay ===
    push 2000                        ; push 2000 ms onto stack for Sleep
    call Sleep                       ; call Sleep(2000) to simulate boot delay

    ; === Get console output handle ===
    push STD_OUTPUT_HANDLE           ; push STD_OUTPUT_HANDLE constant
    call GetStdHandle                ; GetStdHandle(STD_OUTPUT_HANDLE)
    mov ebx, eax                     ; save returned handle in EBX for console I/O

    ; === Print boot message 1 ===
    push 0                           ; lpReserved for WriteConsoleA (NULL)
    push 0                           ; lpNumberOfCharsWritten pointer (NULL)
    push LENGTHOF bootMsg1 - 1       ; number of chars to write (exclude terminating NUL)
    lea eax, bootMsg1                ; load effective address of bootMsg1 into EAX
    push eax                         ; push pointer to buffer
    push ebx                         ; push console handle
    call WriteConsoleA               ; call WriteConsoleA(hConsole, bootMsg1, len, NULL, NULL)

    push 2000                        ; push 2000 ms for another Sleep
    call Sleep                       ; Sleep(2000) to pause between messages

    ; === Print boot message 2 ===
    push 0                           ; lpReserved (NULL)
    push 0                           ; lpNumberOfCharsWritten (NULL)
    push LENGTHOF bootMsg2 - 1       ; length of bootMsg2 without NUL
    lea eax, bootMsg2                ; address of bootMsg2
    push eax                         ; pointer to bootMsg2
    push ebx                         ; console handle
    call WriteConsoleA               ; write second boot message to console

    push 2000                        ; delay before next message
    call Sleep                       ; Sleep(2000)

    ; === Print boot message 3 ===
    push 0                           ; lpReserved (NULL)
    push 0                           ; lpNumberOfCharsWritten (NULL)
    push LENGTHOF bootMsg3 - 1       ; length of bootMsg3 without NUL
    lea eax, bootMsg3                ; address of bootMsg3
    push eax                         ; pointer to bootMsg3
    push ebx                         ; console handle
    call WriteConsoleA               ; write third boot message to console

    ; === Initialize STARTUPINFO ===
    mov startupInfo.cb, SIZEOF STARTUPINFO ; set cb field to size of STARTUPINFO

    ; === CreateProcessA ===
    push OFFSET processInfo          ; lpProcessInformation -> address of PROCESS_INFORMATION
    push OFFSET startupInfo          ; lpStartupInfo -> address of STARTUPINFO
    push 0                           ; lpCurrentDirectory (NULL -> inherit)
    push 0                           ; lpEnvironment (NULL -> inherit)
    push 0                           ; dwCreationFlags (0 -> default)
    push 0                           ; bInheritHandles (FALSE)
    push 0                           ; lpThreadAttributes (NULL)
    push 0                           ; lpProcessAttributes (NULL)
    push OFFSET kernelPath           ; lpCommandLine -> pointer to command line (kernel path)
    push 0                           ; lpApplicationName (NULL -> use command line)
    call CreateProcessA              ; call CreateProcessA to launch KernelApp.exe

    ; === Kernel launched ===
    push 0                           ; lpReserved (NULL)
    push 0                           ; lpNumberOfCharsWritten (NULL)
    push LENGTHOF bootMsg4 - 1       ; number of chars to write for final message
    lea eax, bootMsg4                ; load address of final message
    push eax                         ; pointer to bootMsg4
    push ebx                         ; console handle
    call WriteConsoleA               ; write final message indicating handoff complete

    ; === Short delay before exit ===
    push 3000                        ; push 3000 ms for final Sleep
    call Sleep                       ; Sleep(3000) to give user time to read message

    ret                              ; return from start (exit program)

END start                            ; mark end of file and entry point
