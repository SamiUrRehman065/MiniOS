; ===============================================
; bootloader.asm — Minimal Standalone BootLoader
; Purpose : Display  “boot” messages before our KernelApp runs.
; ===============================================

.386 ; Target 80386+ instruction set (32-bit mode)
.model flat, stdcall ; Use flat memory model with stdcall calling convention
option casemap:none  ; Make assembler case-sensitive (safer for WinAPI names)

; --------------------------------------------------
; Manual function prototypes (we skip .inc includes)
; --------------------------------------------------
GetStdHandle PROTO :DWORD  ; Retrieves a handle to STDOUT (console)
WriteConsoleA PROTO :DWORD, :DWORD, :DWORD, :DWORD, :DWORD  ; Writes text to console
Sleep PROTO :DWORD  ; Suspends execution for given milliseconds

; --------------------------------------------------
; Constants (Win32 standard handle IDs)
; --------------------------------------------------
STD_OUTPUT_HANDLE EQU -11  ; Constant telling GetStdHandle we want the console output stream

; --------------------------------------------------
; Data segment — text that we’ll print out
; --------------------------------------------------
.data ; Data segment
bootMsg1 db "Booting MiniOS Kernel...", 13, 10, 0 ; Message 1 + newline (13,10) + null terminator
bootMsg2 db "Initializing System Memory...", 13, 10, 0 ; Message 2 + newline + null terminator
bootMsg3 db "Launching KernelApp.exe...", 13, 10, 0 ; Message 3 + newline + null terminator

; --------------------------------------------------
; Code segment — actual execution begins
; --------------------------------------------------
.code ; Code segment
start: ; Entry point


    ; --------------------------------------------------
    ; Sleep for 3 seconds (simulate boot delay)
    ; --------------------------------------------------
    push 3000              ; 3000 milliseconds = 3 seconds
    call Sleep              ; call Sleep function


    ; === Get handle to standard output (console) ===
    push STD_OUTPUT_HANDLE ; push -11 onto stack (argument for GetStdHandle)
    call GetStdHandle      ; returns handle in EAX
    mov ebx, eax           ; save that handle in EBX for later reuse

    ; --------------------------------------------------
    ; Print Message #1: “Booting MiniOS Kernel...”
    ; --------------------------------------------------
    push 0                 ; lpReserved (not used)
    push 0                 ; lpNumberOfCharsWritten (optional out param — ignored)
    push LENGTHOF bootMsg1 - 1 ; nNumberOfCharsToWrite (length of message, excluding null)
    lea eax, bootMsg1      ; load address of message into EAX
    push eax               ; lpBuffer (pointer to message)
    push ebx               ; hConsoleOutput (console handle)
    call WriteConsoleA     ; call the function to write to console
    

    ; --------------------------------------------------
    ; Sleep for 3 seconds (simulate boot delay)
    ; --------------------------------------------------
    push 3000              ; 3000 milliseconds = 3 seconds
    call Sleep              ; call Sleep function


    ; --------------------------------------------------
    ; Print Message #2: “Initializing System Memory...”
    ; --------------------------------------------------
    push 0                 ; lpReserved (not used)
    push 0                 ; lpNumberOfCharsWritten (optional out param — ignored)
    push LENGTHOF bootMsg2 - 1 ; nNumberOfCharsToWrite (length of message, excluding null)
    lea eax, bootMsg2      ; load address of message into EAX
    push eax               ; lpBuffer (pointer to message)
    push ebx               ; hConsoleOutput (console handle)
    call WriteConsoleA     ; call the function to write to console


    ; --------------------------------------------------
    ; Sleep for 3 seconds (simulate boot delay)
    ; --------------------------------------------------
    push 3000              ; 3000 milliseconds = 3 seconds
    call Sleep              ; call Sleep function


    ; --------------------------------------------------
    ; Print Message #3: “Launching KernelApp.exe...”
    ; --------------------------------------------------
    push 0                 ; lpReserved (not used)
    push 0                 ; lpNumberOfCharsWritten (optional out param — ignored)
    push LENGTHOF bootMsg3 - 1 ; nNumberOfCharsToWrite (length of message, excluding null)
    lea eax, bootMsg3      ; load address of message into EAX
    push eax               ; lpBuffer (pointer to message)
    push ebx               ; hConsoleOutput (console handle)
    call WriteConsoleA     ; call the function to write to console

    ; --------------------------------------------------
    ; Sleep for 10 seconds (simulate boot delay)
    ; --------------------------------------------------
    push 10000              ; 10000 milliseconds = 10 seconds
    call Sleep              ; call Sleep function

    ; --------------------------------------------------
    ; Return control to OS (process exit)
    ; --------------------------------------------------
    ret                     ; Return from program → will cleanly exit
END start                   ; Define entry point for linker (start label)
