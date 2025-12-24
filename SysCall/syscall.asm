; syscall.asm - Advanced MiniOS System Call Library
; Features: Console I/O, Input, Sleep, Memory, File I/O, Auto-Logging
; Compile as DLL

.386
.model flat, stdcall
option casemap:none

; ==========================================================
; WinAPI Prototypes (Manual - No .inc files needed)
; ==========================================================
GetStdHandle      PROTO :DWORD
WriteConsoleA     PROTO :DWORD, :DWORD, :DWORD, :DWORD, :DWORD
ReadConsoleA      PROTO :DWORD, :DWORD, :DWORD, :DWORD, :DWORD
Sleep             PROTO :DWORD
lstrlenA          PROTO :DWORD
GetProcessHeap    PROTO
HeapAlloc         PROTO :DWORD, :DWORD, :DWORD
HeapFree          PROTO :DWORD, :DWORD, :DWORD
CreateFileA       PROTO :DWORD, :DWORD, :DWORD, :DWORD, :DWORD, :DWORD, :DWORD
WriteFile         PROTO :DWORD, :DWORD, :DWORD, :DWORD, :DWORD
ReadFile          PROTO :DWORD, :DWORD, :DWORD, :DWORD, :DWORD
CloseHandle       PROTO :DWORD
SetFilePointer    PROTO :DWORD, :DWORD, :DWORD, :DWORD
CreateDirectoryA  PROTO :DWORD, :DWORD

; ==========================================================
; Constants & Data
; ==========================================================
STD_INPUT_HANDLE    EQU -10
STD_OUTPUT_HANDLE   EQU -11
HEAP_ZERO_MEMORY    EQU 8

; File Constants
GENERIC_READ        EQU 80000000h
GENERIC_WRITE       EQU 40000000h
FILE_SHARE_READ     EQU 1
CREATE_ALWAYS       EQU 2
OPEN_EXISTING       EQU 3
OPEN_ALWAYS         EQU 4
FILE_ATTRIBUTE_NORMAL EQU 80h
FILE_END            EQU 2

.data
    folderName      db "Logs", 0
    logFileName     db "Logs\kernel.log", 0
    newline         db 13, 10, 0   ; CRLF for logs

.code

; ==========================================================
; DllMain - Entry Point
; ==========================================================
DllMain PROC hInstDLL:DWORD, fdwReason:DWORD, lpvReserved:DWORD
    mov eax, 1      ; Return TRUE
    ret
DllMain ENDP

; ==========================================================
; Sys_Init
; Action: Creates 'Logs' folder automatically
; ==========================================================
Sys_Init PROC
    ; CreateDirectoryA("Logs", NULL)
    push 0              ; Security Attributes (NULL)
    lea eax, folderName
    push eax            ; Folder Name
    call CreateDirectoryA
    ret
Sys_Init ENDP

; ==========================================================
; Sys_Print
; Input: String Pointer | Output: Console
; ==========================================================
Sys_Print PROC uses ebx stringPtr:DWORD
    LOCAL hConsole:DWORD
    LOCAL sLength:DWORD
    LOCAL bytesWritten:DWORD

    push STD_OUTPUT_HANDLE
    call GetStdHandle
    mov hConsole, eax

    push stringPtr
    call lstrlenA
    mov sLength, eax

    push 0
    lea eax, bytesWritten
    push eax
    push sLength
    push stringPtr
    push hConsole
    call WriteConsoleA
    ret
Sys_Print ENDP

; ==========================================================
; Sys_Input
; Input: Buffer Ptr, MaxSize | Action: Reads from Keyboard
; ==========================================================
Sys_Input PROC buffer:DWORD, maxLen:DWORD
    LOCAL hInput:DWORD
    LOCAL bytesRead:DWORD

    push STD_INPUT_HANDLE
    call GetStdHandle
    mov hInput, eax

    push 0                  ; Reserved
    lea eax, bytesRead
    push eax                ; Bytes Read
    push maxLen             ; Max chars to read
    push buffer             ; Buffer
    push hInput             ; Input Handle
    call ReadConsoleA
    ret
Sys_Input ENDP

; ==========================================================
; Sys_Sleep
; Input: Milliseconds | Action: Delay
; ==========================================================
Sys_Sleep PROC ms:DWORD
    push ms
    call Sleep
    ret
Sys_Sleep ENDP

; ==========================================================
; Sys_MemAlloc
; Input: Size | Output: Address (EAX)
; ==========================================================
Sys_MemAlloc PROC sizeBytes:DWORD
    call GetProcessHeap
    push sizeBytes
    push HEAP_ZERO_MEMORY
    push eax
    call HeapAlloc
    ret
Sys_MemAlloc ENDP

; ==========================================================
; Sys_MemFree
; Input: Pointer | Output: Success/Fail
; ==========================================================
Sys_MemFree PROC memPtr:DWORD
    call GetProcessHeap
    push memPtr
    push 0
    push eax
    call HeapFree
    ret
Sys_MemFree ENDP

; ==========================================================
; Sys_FileCreate
; Input: Filename | Action: Creates empty file
; ==========================================================
Sys_FileCreate PROC filename:DWORD
    push 0
    push FILE_ATTRIBUTE_NORMAL
    push CREATE_ALWAYS      ; Overwrite if exists
    push 0
    push 0
    push GENERIC_WRITE
    push filename
    call CreateFileA
    
    ; Close handle immediately (we just wanted to create it)
    cmp eax, -1
    je _fc_exit
    push eax
    call CloseHandle
_fc_exit:
    ret
Sys_FileCreate ENDP

; ==========================================================
; Sys_FileWrite
; Input: Filename, Text | Action: Appends text to file
; ==========================================================
Sys_FileWrite PROC uses ebx filename:DWORD, text:DWORD
    LOCAL hFile:DWORD
    LOCAL len:DWORD
    LOCAL written:DWORD

    push text
    call lstrlenA
    mov len, eax

    ; Open File (Append Mode)
    push 0
    push FILE_ATTRIBUTE_NORMAL
    push OPEN_ALWAYS
    push 0
    push 0
    push GENERIC_WRITE
    push filename
    call CreateFileA
    mov hFile, eax

    cmp eax, -1
    je _fw_exit

    ; Seek to End
    push FILE_END
    push 0
    push 0
    push hFile
    call SetFilePointer

    ; Write Text
    push 0
    lea eax, written
    push eax
    push len
    push text
    push hFile
    call WriteFile

    push hFile
    call CloseHandle
_fw_exit:
    ret
Sys_FileWrite ENDP

; ==========================================================
; Sys_FileRead
; Input: Filename, Buffer, MaxSize
; ==========================================================
Sys_FileRead PROC filename:DWORD, buffer:DWORD, maxLen:DWORD
    LOCAL hFile:DWORD
    LOCAL readBytes:DWORD

    ; Open Existing
    push 0
    push FILE_ATTRIBUTE_NORMAL
    push OPEN_EXISTING
    push 0
    push FILE_SHARE_READ
    push GENERIC_READ
    push filename
    call CreateFileA
    mov hFile, eax

    cmp eax, -1
    je _fr_exit

    ; Read
    push 0
    lea eax, readBytes
    push eax
    push maxLen
    push buffer
    push hFile
    call ReadFile

    push hFile
    call CloseHandle
_fr_exit:
    ret
Sys_FileRead ENDP

; ==========================================================
; Sys_Log (Auto-Logger)
; Input: Message | Action: Appends to "Logs\kernel.log"
; ==========================================================
Sys_Log PROC message:DWORD
    ; Reuse Sys_FileWrite logic but hardcoded to log file
    ; 1. Write Message
    lea eax, logFileName
    push message
    push eax
    call Sys_FileWrite

    ; 2. Write Newline (so logs are not on one line)
    lea eax, logFileName
    lea ebx, newline
    push ebx
    push eax
    call Sys_FileWrite
    ret
Sys_Log ENDP

END DllMain