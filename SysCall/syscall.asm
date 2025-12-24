; syscall.asm - Advanced MiniOS System Call Library                                ; Title and brief description of this assembly module
; Features: Console I/O, Input, Sleep, Memory, File I/O, Auto-Logging               ; Short feature list for quick reference
; Compile as DLL                                                                   ; Note indicating intended output format

.386                                                                              ; Target 80386 instruction set
.model flat, stdcall                                                              ; Flat memory model, stdcall calling convention
option casemap:none                                                               ; Case sensitivity for symbol names

; ==========================================================                      ; Section divider for readability
; WinAPI Prototypes (Manual - No .inc files needed)                               ; Begin declarations of external API functions
; ==========================================================                      ; Visual separator

GetStdHandle      PROTO :DWORD                                                   ; Prototype: returns handle to standard device (stdin/stdout)
WriteConsoleA     PROTO :DWORD, :DWORD, :DWORD, :DWORD, :DWORD                   ; Prototype: write characters to console (ANSI)
ReadConsoleA      PROTO :DWORD, :DWORD, :DWORD, :DWORD, :DWORD                   ; Prototype: read characters from console (ANSI)
Sleep             PROTO :DWORD                                                    ; Prototype: suspend execution for given milliseconds
lstrlenA          PROTO :DWORD                                                    ; Prototype: get ANSI string length
GetProcessHeap    PROTO                                                          ; Prototype: get process default heap handle
HeapAlloc         PROTO :DWORD, :DWORD, :DWORD                                    ; Prototype: allocate memory from heap
HeapFree          PROTO :DWORD, :DWORD, :DWORD                                    ; Prototype: free heap memory
CreateFileA       PROTO :DWORD, :DWORD, :DWORD, :DWORD, :DWORD, :DWORD, :DWORD    ; Prototype: create or open file (ANSI)
WriteFile         PROTO :DWORD, :DWORD, :DWORD, :DWORD, :DWORD                    ; Prototype: write bytes to file/handle
ReadFile          PROTO :DWORD, :DWORD, :DWORD, :DWORD, :DWORD                    ; Prototype: read bytes from file/handle
CloseHandle       PROTO :DWORD                                                    ; Prototype: close an open handle
SetFilePointer    PROTO :DWORD, :DWORD, :DWORD, :DWORD                           ; Prototype: move file pointer within a file
CreateDirectoryA  PROTO :DWORD, :DWORD                                           ; Prototype: create a directory (ANSI)

; ==========================================================                      ; Section divider for constants
; Constants & Data                                                                 ; Begin constant definitions and static data
; ==========================================================                      ; Visual separator
STD_INPUT_HANDLE    EQU -10                                                       ; Constant: standard input handle index (-10)
STD_OUTPUT_HANDLE   EQU -11                                                       ; Constant: standard output handle index (-11)
HEAP_ZERO_MEMORY    EQU 8                                                         ; Constant: flag to zero memory on allocation

; File Constants                                                                    ; Begin file-related constant definitions
GENERIC_READ        EQU 80000000h                                                 ; File access: read permission
GENERIC_WRITE       EQU 40000000h                                                 ; File access: write permission
FILE_SHARE_READ     EQU 1                                                         ; File share mode: allow read-sharing
CREATE_ALWAYS       EQU 2                                                         ; Creation disposition: always create (overwrite)
OPEN_EXISTING       EQU 3                                                         ; Creation disposition: open only if exists
OPEN_ALWAYS         EQU 4                                                         ; Creation disposition: open if exists or create
FILE_ATTRIBUTE_NORMAL EQU 80h                                                     ; File attribute: normal file
FILE_END            EQU 2                                                         ; Seek constant: move pointer to file end

.data                                                                             ; Begin initialized data section
    folderName      db "Logs", 0                                                  ; Zero-terminated string containing folder name "Logs"
    logFileName     db "Logs\kernel.log", 0                                       ; Zero-terminated path for log file inside Logs folder
    newline         db 13, 10, 0   ; CRLF for logs                                  ; CRLF sequence to separate log entries

.code                                                                             ; Begin code section

; ==========================================================                      ; Section divider for entry point
; DllMain - Entry Point                                                            ; DLL entry point routine
; ==========================================================                      ; Visual separator
DllMain PROC hInstDLL:DWORD, fdwReason:DWORD, lpvReserved:DWORD                   ; DllMain signature: instance, reason, reserved
    mov eax, 1      ; Return TRUE                                                   ; Return TRUE in EAX to indicate successful load
    ret                                                                    ; Return to caller (OS)
DllMain ENDP                                                                    ; End of DllMain procedure

; ==========================================================                      ; Section divider for initialization
; Sys_Init                                                                         ; Initializes runtime (creates Logs folder)
; Action: Creates 'Logs' folder automatically                                      ; Description of action
; ==========================================================                      ; Visual separator
Sys_Init PROC                                                                     ; Sys_Init procedure start
    ; CreateDirectoryA("Logs", NULL)                                              ; Comment describing upcoming call
    push 0              ; Security Attributes (NULL)                              ; Push NULL security attributes onto stack
    lea eax, folderName                                                           ; Load effective address of folderName into EAX
    push eax            ; Folder Name                                              ; Push pointer to "Logs" string
    call CreateDirectoryA                                                         ; Call WinAPI to create directory (no-op if exists)
    ret                                                                          ; Return to caller
Sys_Init ENDP                                                                    ; End of Sys_Init procedure

; ==========================================================                      ; Section divider for printing
; Sys_Print                                                                        ; Print null-terminated string to console
; Input: String Pointer | Output: Console                                          ; Parameter and effect
; ==========================================================                      ; Visual separator
Sys_Print PROC uses ebx stringPtr:DWORD                                           ; Sys_Print declaration, preserving EBX, parameter is stringPtr
    LOCAL hConsole:DWORD                                                           ; Local variable to hold console handle
    LOCAL sLength:DWORD                                                            ; Local variable to hold string length
    LOCAL bytesWritten:DWORD                                                       ; Local variable to receive number of chars written

    push STD_OUTPUT_HANDLE                                                         ; Push constant index for stdout
    call GetStdHandle                                                              ; Get handle for standard output device
    mov hConsole, eax                                                              ; Store returned handle in local hConsole

    push stringPtr                                                                 ; Push pointer to string as parameter for lstrlenA
    call lstrlenA                                                                  ; Call WinAPI to measure length of the ANSI string
    mov sLength, eax                                                               ; Store measured length in sLength

    push 0                                                                         ; Push reserved parameter (NULL) for WriteConsoleA
    lea eax, bytesWritten                                                          ; Load address of bytesWritten local into EAX
    push eax                                                                       ; Push pointer to bytesWritten for WriteConsoleA
    push sLength                                                                   ; Push number of characters to write
    push stringPtr                                                                 ; Push pointer to string buffer to write
    push hConsole                                                                  ; Push console handle as first parameter
    call WriteConsoleA                                                             ; Write the string to the console
    ret                                                                            ; Return to caller
Sys_Print ENDP                                                                    ; End of Sys_Print procedure

; ==========================================================                      ; Section divider for input
; Sys_Input                                                                        ; Read input from keyboard into buffer
; Input: Buffer Ptr, MaxSize | Action: Reads from Keyboard                        ; Parameters and effect
; ==========================================================                      ; Visual separator
Sys_Input PROC buffer:DWORD, maxLen:DWORD                                         ; Sys_Input procedure with buffer and maxLen params
    LOCAL hInput:DWORD                                                             ; Local variable to hold input handle
    LOCAL bytesRead:DWORD                                                          ; Local variable to track number of bytes read

    push STD_INPUT_HANDLE                                                          ; Push constant index for stdin
    call GetStdHandle                                                              ; Retrieve standard input handle
    mov hInput, eax                                                                ; Store input handle locally

    push 0                  ; Reserved                                              ; Push reserved parameter NULL for ReadConsoleA
    lea eax, bytesRead                                                             ; Load address of bytesRead into EAX
    push eax                ; Bytes Read                                              ; Push pointer to receive number of chars read
    push maxLen             ; Max chars to read                                      ; Push maximum characters to read
    push buffer             ; Buffer                                                   ; Push pointer to destination buffer
    push hInput             ; Input Handle                                             ; Push input handle
    call ReadConsoleA                                                              ; Call to read from console into buffer
    ret                                                                            ; Return to caller
Sys_Input ENDP                                                                    ; End of Sys_Input procedure

; ==========================================================                      ; Section divider for sleep
; Sys_Sleep                                                                        ; Pause execution for given milliseconds
; Input: Milliseconds | Action: Delay                                               ; Parameter and effect
; ==========================================================                      ; Visual separator
Sys_Sleep PROC ms:DWORD                                                           ; Sys_Sleep accepts milliseconds in ms
    push ms                                                                        ; Push milliseconds argument for Sleep
    call Sleep                                                                     ; Call WinAPI Sleep to suspend thread
    ret                                                                            ; Return to caller
Sys_Sleep ENDP                                                                    ; End of Sys_Sleep procedure

; ==========================================================                      ; Section divider for memory allocation
; Sys_MemAlloc                                                                     ; Allocate memory from process heap
; Input: Size | Output: Address (EAX)                                               ; Parameter and return convention
; ==========================================================                      ; Visual separator
Sys_MemAlloc PROC sizeBytes:DWORD                                                  ; Sys_MemAlloc takes sizeBytes parameter
    call GetProcessHeap                                                            ; Obtain handle to process default heap
    push sizeBytes                                                                 ; Push number of bytes to allocate
    push HEAP_ZERO_MEMORY                                                          ; Push flags to zero memory on allocation
    push eax                                                                       ; Push heap handle returned in EAX
    call HeapAlloc                                                                 ; Call HeapAlloc to allocate memory
    ret                                                                            ; Return pointer in EAX to caller
Sys_MemAlloc ENDP                                                                 ; End of Sys_MemAlloc procedure

; ==========================================================                      ; Section divider for freeing memory
; Sys_MemFree                                                                      ; Free previously allocated heap memory
; Input: Pointer | Output: Success/Fail                                              ; Parameter and return expectation
; ==========================================================                      ; Visual separator
Sys_MemFree PROC memPtr:DWORD                                                      ; Sys_MemFree takes memPtr pointer parameter
    call GetProcessHeap                                                            ; Get handle to process heap for freeing
    push memPtr                                                                    ; Push pointer to free onto stack
    push 0                                                                         ; Push flags (reserved) for HeapFree
    push eax                                                                       ; Push heap handle returned in EAX
    call HeapFree                                                                  ; Call HeapFree to release memory
    ret                                                                            ; Return to caller (BOOL in EAX if needed)
Sys_MemFree ENDP                                                                   ; End of Sys_MemFree procedure

; ==========================================================                      ; Section divider for file creation
; Sys_FileCreate                                                                   ; Create or overwrite an empty file
; Input: Filename | Action: Creates empty file                                      ; Parameter and action
; ==========================================================                      ; Visual separator
Sys_FileCreate PROC filename:DWORD                                                 ; Sys_FileCreate signature with filename pointer
    push 0                                                                         ; Push template file handle (NULL)
    push FILE_ATTRIBUTE_NORMAL                                                     ; Push file attributes for created file
    push CREATE_ALWAYS      ; Overwrite if exists                                  ; Push creation disposition to always create
    push 0                                                                         ; Push security attributes (NULL)
    push 0                                                                         ; Push share mode (no sharing)
    push GENERIC_WRITE                                                           ; Push desired access: write
    push filename                                                                  ; Push pointer to filename string
    call CreateFileA                                                               ; Call CreateFileA to create or overwrite the file
    
    ; Close handle immediately (we just wanted to create it)                      ; Comment explaining intent to close handle
    cmp eax, -1                                                                    ; Compare returned handle with INVALID_HANDLE_VALUE (-1)
    je _fc_exit                                                                    ; If invalid, jump to exit without closing
    push eax                                                                       ; Push valid handle to close
    call CloseHandle                                                               ; Close the file handle we just created
_fc_exit:                                                                         ; Label marking exit point
    ret                                                                            ; Return to caller
Sys_FileCreate ENDP                                                               ; End of Sys_FileCreate procedure

; ==========================================================                      ; Section divider for file writing
; Sys_FileWrite                                                                    ; Append text to specified file
; Input: Filename, Text | Action: Appends text to file                             ; Parameters and action
; ==========================================================                      ; Visual separator
Sys_FileWrite PROC uses ebx filename:DWORD, text:DWORD                            ; Sys_FileWrite declaration preserving EBX, params filename,text
    LOCAL hFile:DWORD                                                              ; Local to store opened file handle
    LOCAL len:DWORD                                                                ; Local to store length of text
    LOCAL written:DWORD                                                            ; Local to receive number of bytes written

    push text                                                                       ; Push pointer to text to compute its length
    call lstrlenA                                                                  ; Get length of ANSI text to append
    mov len, eax                                                                   ; Store length in local variable len

    ; Open File (Append Mode)                                                      ; Comment describing file open intent
    push 0                                                                         ; Push template handle (NULL) for CreateFileA
    push FILE_ATTRIBUTE_NORMAL                                                     ; Push file attributes
    push OPEN_ALWAYS                                                               ; Push disposition: open if exists or create
    push 0                                                                         ; Push security attributes (NULL)
    push 0                                                                         ; Push share mode (none, exclusive)
    push GENERIC_WRITE                                                             ; Push desired access: write
    push filename                                                                  ; Push pointer to filename to open/ create
    call CreateFileA                                                               ; Call CreateFileA to obtain handle
    mov hFile, eax                                                                 ; Save returned handle in local hFile

    cmp eax, -1                                                                    ; Compare returned handle to INVALID_HANDLE_VALUE
    je _fw_exit                                                                    ; Jump to exit if handle is invalid

    ; Seek to End                                                                  ; Comment: move file pointer to file end for appending
    push FILE_END                                                                  ; Push move method FILE_END to set pointer relative to end
    push 0                                                                         ; Push low-order displacement (0)
    push 0                                                                         ; Push high-order displacement (0)
    push hFile                                                                     ; Push file handle for SetFilePointer
    call SetFilePointer                                                            ; Call API to reposition file pointer at end

    ; Write Text                                                                    ; Comment indicating write step
    push 0                                                                         ; Push overlapped structure (NULL) or reserved param
    lea eax, written                                                               ; Load address of written local into EAX
    push eax                                                                       ; Push pointer to receive number of bytes written
    push len                                                                       ; Push number of bytes to write
    push text                                                                      ; Push pointer to text buffer to write
    push hFile                                                                     ; Push handle of file to write into
    call WriteFile                                                                 ; Call API to write bytes to file

    push hFile                                                                     ; Push file handle to close it
    call CloseHandle                                                               ; Close the file handle to flush and release it
_fw_exit:                                                                         ; Label marking file write exit
    ret                                                                            ; Return to caller
Sys_FileWrite ENDP                                                                 ; End of Sys_FileWrite procedure

; ==========================================================                      ; Section divider for file reading
; Sys_FileRead                                                                     ; Read contents of existing file into buffer
; Input: Filename, Buffer, MaxSize                                                  ; Parameters and purpose
; ==========================================================                      ; Visual separator
Sys_FileRead PROC filename:DWORD, buffer:DWORD, maxLen:DWORD                       ; Sys_FileRead signature with filename, buffer, maxLen
    LOCAL hFile:DWORD                                                              ; Local to store opened file handle
    LOCAL readBytes:DWORD                                                          ; Local to receive number of bytes actually read

    ; Open Existing                                                                 ; Comment: open file for reading only if it exists
    push 0                                                                         ; Push template param (NULL)
    push FILE_ATTRIBUTE_NORMAL                                                     ; Push file attributes
    push OPEN_EXISTING                                                              ; Push creation disposition: only open if exists
    push 0                                                                         ; Push security attributes (NULL)
    push FILE_SHARE_READ                                                           ; Push share mode: allow others to read while open
    push GENERIC_READ                                                              ; Push desired access: read
    push filename                                                                  ; Push pointer to filename string
    call CreateFileA                                                               ; Open the file for reading
    mov hFile, eax                                                                 ; Store returned handle in local hFile

    cmp eax, -1                                                                    ; Compare returned handle to INVALID_HANDLE_VALUE
    je _fr_exit                                                                    ; If invalid, jump to exit (file not opened)

    ; Read                                                                          ; Comment: perform the ReadFile operation
    push 0                                                                         ; Push overlapped param (NULL)
    lea eax, readBytes                                                              ; Load address of readBytes local
    push eax                                                                        ; Push pointer to receive number of bytes read
    push maxLen                                                                     ; Push maximum bytes to read into buffer
    push buffer                                                                     ; Push pointer to destination buffer
    push hFile                                                                      ; Push file handle to read from
    call ReadFile                                                                   ; Call ReadFile to read bytes into buffer

    push hFile                                                                      ; Push file handle to close
    call CloseHandle                                                                ; Close file handle to finish operation
_fr_exit:                                                                         ; Label marking file read exit
    ret                                                                            ; Return to caller
Sys_FileRead ENDP                                                                 ; End of Sys_FileRead procedure

; ==========================================================                      ; Section divider for logging convenience
; Sys_Log (Auto-Logger)                                                            ; Append messages to Logs\kernel.log automatically
; Input: Message | Action: Appends to "Logs\kernel.log"                            ; Purpose and parameter
; ==========================================================                      ; Visual separator
Sys_Log PROC message:DWORD                                                         ; Sys_Log procedure taking a message pointer
    ; Reuse Sys_FileWrite logic but hardcoded to log file                            ; Comment: this routine calls Sys_FileWrite twice

    ; 1. Write Message                                                              ; Step 1: append the message content
    lea eax, logFileName                                                            ; Load address of the constant log file path into EAX
    push message                                                                    ; Push pointer to message string as second parameter
    push eax                                                                        ; Push pointer to logFileName as first parameter
    call Sys_FileWrite                                                              ; Call internal Sys_FileWrite to append message

    ; 2. Write Newline (so logs are not on one line)                                ; Step 2: append CRLF after message
    lea eax, logFileName                                                            ; Load address of log file path again into EAX
    lea ebx, newline                                                                ; Load address of newline buffer into EBX for second write
    push ebx                                                                        ; Push pointer to newline string
    push eax                                                                        ; Push pointer to log file path
    call Sys_FileWrite                                                              ; Call Sys_FileWrite to append newline
    ret                                                                             ; Return to caller
Sys_Log ENDP                                                                       ; End of Sys_Log procedure

END DllMain                                                                        ; Mark end of module and specify DllMain as entry point