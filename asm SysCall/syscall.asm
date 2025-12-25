; syscall.asm - Advanced MiniOS System Call Library
; Features: Console I/O, Input, Sleep, Memory, File I/O, Auto-Logging
; Compile as DLL

.386
.model flat, stdcall
option casemap:none

; ==========================================================
; WinAPI Prototypes
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
GetFileAttributesA PROTO :DWORD
SetCurrentDirectoryA PROTO :DWORD
GetCurrentDirectoryA PROTO :DWORD, :DWORD

; ==========================================================
; Constants
; ==========================================================
STD_INPUT_HANDLE    EQU -10
STD_OUTPUT_HANDLE   EQU -11
HEAP_ZERO_MEMORY    EQU 8

GENERIC_READ        EQU 80000000h
GENERIC_WRITE       EQU 40000000h
FILE_SHARE_READ     EQU 1
FILE_SHARE_WRITE    EQU 2
CREATE_ALWAYS       EQU 2
OPEN_EXISTING       EQU 3
OPEN_ALWAYS         EQU 4
FILE_ATTRIBUTE_NORMAL EQU 80h
FILE_ATTRIBUTE_DIRECTORY EQU 10h
INVALID_FILE_ATTRIBUTES EQU -1
FILE_END            EQU 2

.data
    folderName      db "Logs", 0
    logFileName     db "Logs\kernel.log", 0
    newline         db 13, 10, 0

.code

; ==========================================================
; DllMain - Entry Point
; ==========================================================
DllMain PROC hInstDLL:DWORD, fdwReason:DWORD, lpvReserved:DWORD
    mov eax, 1
    ret
DllMain ENDP

; ==========================================================
; Sys_Init - Creates 'Logs' folder
; ==========================================================
Sys_Init PROC
    push 0
    push OFFSET folderName
    call CreateDirectoryA
    ret
Sys_Init ENDP

; ==========================================================
; Sys_Print - Print string to console
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
; Sys_Input - Read input from keyboard
; ==========================================================
Sys_Input PROC buffer:DWORD, maxLen:DWORD
    LOCAL hInput:DWORD
    LOCAL bytesRead:DWORD

    push STD_INPUT_HANDLE
    call GetStdHandle
    mov hInput, eax

    push 0
    lea eax, bytesRead
    push eax
    push maxLen
    push buffer
    push hInput
    call ReadConsoleA
    ret
Sys_Input ENDP

; ==========================================================
; Sys_Sleep - Pause execution
; ==========================================================
Sys_Sleep PROC ms:DWORD
    push ms
    call Sleep
    ret
Sys_Sleep ENDP

; ==========================================================
; Sys_MemAlloc - Allocate heap memory
; ==========================================================
Sys_MemAlloc PROC sizeBytes:DWORD
    LOCAL hHeap:DWORD
    
    call GetProcessHeap
    mov hHeap, eax
    
    push sizeBytes
    push HEAP_ZERO_MEMORY
    push hHeap
    call HeapAlloc
    ret
Sys_MemAlloc ENDP

; ==========================================================
; Sys_MemFree - Free heap memory
; ==========================================================
Sys_MemFree PROC memPtr:DWORD
    LOCAL hHeap:DWORD
    
    call GetProcessHeap
    mov hHeap, eax
    
    push memPtr
    push 0
    push hHeap
    call HeapFree
    ret
Sys_MemFree ENDP

; ==========================================================
; Sys_FileCreate - Create or overwrite file
; ==========================================================
Sys_FileCreate PROC filename:DWORD
    LOCAL hFile:DWORD
    
    push 0
    push FILE_ATTRIBUTE_NORMAL
    push CREATE_ALWAYS
    push 0
    push 0
    push GENERIC_WRITE
    push filename
    call CreateFileA
    mov hFile, eax

    cmp eax, -1
    je _fc_exit
    
    push hFile
    call CloseHandle
    mov eax, 1
    ret
    
_fc_exit:
    xor eax, eax
    ret
Sys_FileCreate ENDP

; ==========================================================
; Sys_FileWrite - Append text to file (with sharing)
; ==========================================================
Sys_FileWrite PROC uses ebx esi filename:DWORD, text:DWORD
    LOCAL hFile:DWORD
    LOCAL textLen:DWORD
    LOCAL written:DWORD

    push text
    call lstrlenA
    mov textLen, eax
    
    cmp eax, 0
    je _fw_fail

    push 0
    push FILE_ATTRIBUTE_NORMAL
    push OPEN_ALWAYS
    push 0
    push FILE_SHARE_READ OR FILE_SHARE_WRITE
    push GENERIC_WRITE
    push filename
    call CreateFileA
    mov hFile, eax

    cmp eax, -1
    je _fw_fail

    push FILE_END
    push 0
    push 0
    push hFile
    call SetFilePointer

    push 0
    lea eax, written
    push eax
    push textLen
    push text
    push hFile
    call WriteFile

    push hFile
    call CloseHandle
    
    mov eax, 1
    ret

_fw_fail:
    xor eax, eax
    ret
Sys_FileWrite ENDP

; ==========================================================
; Sys_FileRead - Read file contents
; ==========================================================
Sys_FileRead PROC filename:DWORD, buffer:DWORD, maxLen:DWORD
    LOCAL hFile:DWORD
    LOCAL readBytes:DWORD

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
    je _fr_fail

    push 0
    lea eax, readBytes
    push eax
    push maxLen
    push buffer
    push hFile
    call ReadFile

    push hFile
    call CloseHandle
    
    mov eax, readBytes
    ret

_fr_fail:
    xor eax, eax
    ret
Sys_FileRead ENDP

; ==========================================================
; Sys_DirCreate - Create directory
; Returns: 1 = success, 0 = fail
; ==========================================================
Sys_DirCreate PROC dirPath:DWORD
    push 0
    push dirPath
    call CreateDirectoryA
    
    ; CreateDirectoryA returns non-zero on success
    cmp eax, 0
    je _dc_fail
    mov eax, 1
    ret
    
_dc_fail:
    xor eax, eax
    ret
Sys_DirCreate ENDP

; ==========================================================
; Sys_DirChange - Change current directory
; Returns: 1 = success, 0 = fail
; ==========================================================
Sys_DirChange PROC dirPath:DWORD
    push dirPath
    call SetCurrentDirectoryA
    
    cmp eax, 0
    je _chdir_fail
    mov eax, 1
    ret
    
_chdir_fail:
    xor eax, eax
    ret
Sys_DirChange ENDP

; ==========================================================
; Sys_DirGet - Get current directory
; Returns: length of path
; ==========================================================
Sys_DirGet PROC buffer:DWORD, bufferSize:DWORD
    push buffer
    push bufferSize
    call GetCurrentDirectoryA
    ret
Sys_DirGet ENDP

; ==========================================================
; Sys_PathExists - Check if path exists
; Returns: 0 = not exist, 1 = file, 2 = directory
; ==========================================================
Sys_PathExists PROC pathPtr:DWORD
    push pathPtr
    call GetFileAttributesA
    
    cmp eax, INVALID_FILE_ATTRIBUTES
    je _pe_notexist
    
    test eax, FILE_ATTRIBUTE_DIRECTORY
    jnz _pe_isdir
    
    ; It's a file
    mov eax, 1
    ret
    
_pe_isdir:
    mov eax, 2
    ret
    
_pe_notexist:
    xor eax, eax
    ret
Sys_PathExists ENDP

; ==========================================================
; Sys_Log - Append to kernel.log
; ==========================================================
Sys_Log PROC uses ebx message:DWORD
    push message
    push OFFSET logFileName
    call Sys_FileWrite

    push OFFSET newline
    push OFFSET logFileName
    call Sys_FileWrite
    
    ret
Sys_Log ENDP

END DllMain