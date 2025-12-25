; ============================================================
; bootloader.asm — MiniOS Boot Loader (Graphical User-Mode Simulation)
; Purpose:
;   1) Display beautiful boot sequence with graphics
;   2) Show progress bars and system checks
;   3) Launch KernelApp.exe (our simulated kernel)
; ============================================================

.386
.model flat, stdcall
option casemap:none

; ------------------------------------------------------------
; WinAPI prototypes
; ------------------------------------------------------------
GetStdHandle            PROTO :DWORD
WriteConsoleA           PROTO :DWORD, :DWORD, :DWORD, :DWORD, :DWORD
SetConsoleCursorPosition PROTO :DWORD, :DWORD
SetConsoleTextAttribute PROTO :DWORD, :WORD
SetConsoleTitleA        PROTO :DWORD
Sleep                   PROTO :DWORD
CreateProcessA          PROTO :DWORD, :DWORD, :DWORD, :DWORD, :DWORD, :DWORD, :DWORD, :DWORD, :DWORD, :DWORD
FillConsoleOutputCharacterA PROTO :DWORD, :BYTE, :DWORD, :DWORD, :DWORD
FillConsoleOutputAttribute PROTO :DWORD, :WORD, :DWORD, :DWORD, :DWORD
SetConsoleCursorInfo    PROTO :DWORD, :DWORD
GetConsoleScreenBufferInfo PROTO :DWORD, :DWORD
Beep                    PROTO :DWORD, :DWORD
ExitProcess             PROTO :DWORD
wsprintfA               PROTO C :DWORD, :DWORD, :VARARG

; ------------------------------------------------------------
; Constants
; ------------------------------------------------------------
STD_OUTPUT_HANDLE       EQU -11

; Console colors
CLR_TITLE               EQU 0Bh     ; Bright Cyan
CLR_LOGO                EQU 0Fh     ; Bright White
CLR_SUCCESS             EQU 0Ah     ; Bright Green
CLR_WARNING             EQU 0Eh     ; Bright Yellow
CLR_ERROR               EQU 0Ch     ; Bright Red
CLR_INFO                EQU 07h     ; Gray
CLR_PROGRESS_BG         EQU 08h     ; Dark Gray
CLR_PROGRESS_FG         EQU 0Bh     ; Bright Cyan
CLR_BORDER              EQU 09h     ; Bright Blue
CLR_COUNTDOWN           EQU 0Fh     ; Bright White

; ------------------------------------------------------------
; Structures
; ------------------------------------------------------------
COORD STRUCT
    X WORD ?
    Y WORD ?
COORD ENDS

SMALL_RECT STRUCT
    Left_   WORD ?
    Top_    WORD ?
    Right_  WORD ?
    Bottom_ WORD ?
SMALL_RECT ENDS

CONSOLE_SCREEN_BUFFER_INFO STRUCT
    dwSize              COORD <>
    dwCursorPosition    COORD <>
    wAttributes         WORD ?
    srWindow            SMALL_RECT <>
    dwMaximumWindowSize COORD <>
CONSOLE_SCREEN_BUFFER_INFO ENDS

CONSOLE_CURSOR_INFO STRUCT
    dwSize   DWORD ?
    bVisible DWORD ?
CONSOLE_CURSOR_INFO ENDS

STARTUPINFO STRUCT
    cb              DWORD ?
    lpReserved      DWORD ?
    lpDesktop       DWORD ?
    lpTitle         DWORD ?
    dwX             DWORD ?
    dwY             DWORD ?
    dwXSize         DWORD ?
    dwYSize         DWORD ?
    dwXCountChars   DWORD ?
    dwYCountChars   DWORD ?
    dwFillAttribute DWORD ?
    dwFlags         DWORD ?
    wShowWindow     WORD  ?
    cbReserved2     WORD  ?
    lpReserved2     DWORD ?
    hStdInput       DWORD ?
    hStdOutput      DWORD ?
    hStdError       DWORD ?
STARTUPINFO ENDS

PROCESS_INFORMATION STRUCT
    hProcess    DWORD ?
    hThread     DWORD ?
    dwProcessId DWORD ?
    dwThreadId  DWORD ?
PROCESS_INFORMATION ENDS

; ------------------------------------------------------------
; Data section
; ------------------------------------------------------------
.data

hConsole        DWORD 0
charsWritten    DWORD 0
countdownVal    DWORD 35

consoleTitle db "MiniOS Boot Loader v1.0", 0

; ASCII Art Logo
logo1 db "       __  __ _       _  ____   _____  ", 13, 10, 0
logo2 db "      |  \/  (_)     (_)/ __ \ / ____| ", 13, 10, 0
logo3 db "      | \  / |_ _ __  _| |  | | (___   ", 13, 10, 0
logo4 db "      | |\/| | | '_ \| | |  | |\___ \  ", 13, 10, 0
logo5 db "      | |  | | | | | | | |__| |____) | ", 13, 10, 0
logo6 db "      |_|  |_|_|_| |_|_|\____/|_____/  ", 13, 10, 0

; Boot messages
msgBorder    db "================================================================================", 13, 10, 0
msgNewLine   db 13, 10, 0
msgSpaces    db "                                                                          ", 0

; Phase 1: Hardware Detection
msgPhase1    db "  [PHASE 1] Hardware Detection & Initialization", 13, 10, 0
msgBios      db "  [BIOS  ] POST Complete - System OK", 13, 10, 0
msgCpu       db "  [CPU   ] Detecting processor..................", 0
msgCpuVal    db " Intel x86 Compatible", 13, 10, 0
msgCpuCache  db "  [CACHE ] L1: 32KB, L2: 256KB, L3: 8MB.........", 0
msgCpuCacheV db " Configured", 13, 10, 0
msgMem       db "  [MEMORY] Detecting RAM........................", 0
msgMemVal    db " 4096 MB DDR4 OK", 13, 10, 0
msgMemTest   db "  [MEMTST] Running memory diagnostics...........", 0
msgMemTestV  db " PASSED", 13, 10, 0

; Phase 2: Storage & I/O
msgPhase2    db 13, 10, "  [PHASE 2] Storage & I/O Configuration", 13, 10, 0
msgDisk      db "  [DISK  ] Scanning storage devices.............", 0
msgDiskVal   db " VFS Mounted", 13, 10, 0
msgPart      db "  [PART  ] Reading partition table..............", 0
msgPartVal   db " 1 partition(s)", 13, 10, 0
msgFs        db "  [FS    ] Initializing file system.............", 0
msgFsVal     db " VFS Ready", 13, 10, 0
msgIo        db "  [I/O   ] Configuring I/O ports................", 0
msgIoVal     db " COM1, LPT1", 13, 10, 0

; Phase 3: Kernel Loading
msgPhase3    db 13, 10, "  [PHASE 3] Kernel Loading & Initialization", 13, 10, 0
msgBoot      db "  [BOOT  ] Loading boot sector..................", 0
msgBootVal   db " Sector 0 OK", 13, 10, 0
msgKernel    db "  [KERNEL] Loading kernel image.................", 0
msgKernelVal db " KernelApp.exe", 13, 10, 0
msgKernSz    db "  [KERNEL] Kernel size..........................", 0
msgKernSzV   db " 2.4 MB", 13, 10, 0
msgDecomp    db "  [KERNEL] Decompressing kernel.................", 0
msgDecompV   db " Done", 13, 10, 0

; Phase 4: System Services
msgPhase4    db 13, 10, "  [PHASE 4] System Services Initialization", 13, 10, 0
msgInit      db "  [INIT  ] Initializing kernel subsystems.......", 0
msgInitVal   db " OK", 13, 10, 0
msgSched     db "  [SCHED ] Starting process scheduler...........", 0
msgSchedV    db " Active", 13, 10, 0
msgMm        db "  [MM    ] Setting up memory manager............", 0
msgMmV       db " 4096 MB mapped", 13, 10, 0
msgVfs       db "  [VFS   ] Mounting virtual file system.........", 0
msgVfsV      db " / mounted", 13, 10, 0
msgLog       db "  [LOG   ] Starting system logger...............", 0
msgLogV      db " kernel.log", 13, 10, 0

; Phase 5: Final Checks
msgPhase5    db 13, 10, "  [PHASE 5] Final System Checks", 13, 10, 0
msgInteg     db "  [CHECK ] Verifying system integrity...........", 0
msgIntegV    db " PASSED", 13, 10, 0
msgReady     db "  [READY ] All systems operational..............", 0
msgReadyV    db " GO", 13, 10, 0

statusOK     db " [  OK  ]", 0
statusWait   db " [ WAIT ]", 0
statusPass   db " [ PASS ]", 0

; Progress bar
progressL    db "          [", 0
progressF    db 219, 0          ; Full block
progressE    db 177, 0          ; Empty block  
progressR    db "]", 13, 10, 0

; Countdown messages
msgCountHdr  db 13, 10, "================================================================================", 13, 10, 0
msgCountTxt  db "             Launching MiniOS Kernel in ", 0
msgCountSec  db " seconds...              ", 0
msgCountFmt  db "%d", 0

; Launch messages
msgLaunchHdr db 13, 10, "================================================================================", 13, 10, 0
msgLaunch1   db "                                                                                ", 13, 10, 0
msgLaunch2   db "            >>>>  TRANSFERRING CONTROL TO KERNEL  <<<<                          ", 13, 10, 0
msgLaunch3   db "                                                                                ", 13, 10, 0
msgLaunchNow db "                        >>> LAUNCHING NOW <<<                                   ", 13, 10, 0

; Footer messages
msgVersion   db 13, 10, "       MiniOS Boot Loader v1.0 - x86 Architecture", 13, 10, 0
msgCopy      db "       (c) 2024 MiniOS Project - Educational Purpose", 13, 10, 0

; Kernel path
kernelPath db "..\KernelApp\bin\x86\Debug\KernelApp.exe", 0

; Buffer for countdown number
countBuf    db 8 DUP(0)

; Structures
startupInfo     STARTUPINFO <>
processInfo     PROCESS_INFORMATION <>
screenInfo      CONSOLE_SCREEN_BUFFER_INFO <>
cursorInfo      CONSOLE_CURSOR_INFO <>

.code

; ============================================================
; PrintString - Print null-terminated string
; ============================================================
PrintString PROC lpString:DWORD
    LOCAL strLen:DWORD
    
    push esi
    mov esi, lpString
    xor eax, eax
@@countLoop:
    cmp BYTE PTR [esi + eax], 0
    je @@countDone
    inc eax
    jmp @@countLoop
@@countDone:
    mov strLen, eax
    pop esi
    
    invoke WriteConsoleA, hConsole, lpString, strLen, ADDR charsWritten, 0
    ret
PrintString ENDP

; ============================================================
; SetColor - Set console text color
; ============================================================
SetColor PROC color:WORD
    invoke SetConsoleTextAttribute, hConsole, color
    ret
SetColor ENDP

; ============================================================
; SetPos - Set cursor position
; ============================================================
SetPos PROC xPos:DWORD, yPos:DWORD
    LOCAL coordVal:DWORD
    
    mov eax, yPos
    shl eax, 16
    mov ax, WORD PTR xPos
    mov coordVal, eax
    
    invoke SetConsoleCursorPosition, hConsole, coordVal
    ret
SetPos ENDP

; ============================================================
; ClearScreen - Clear the console
; ============================================================
ClearScreen PROC
    LOCAL screenSize:DWORD
    
    invoke GetConsoleScreenBufferInfo, hConsole, ADDR screenInfo
    
    movzx eax, screenInfo.dwSize.X
    movzx ecx, screenInfo.dwSize.Y
    imul eax, ecx
    mov screenSize, eax
    
    invoke FillConsoleOutputCharacterA, hConsole, 32, screenSize, 0, ADDR charsWritten
    invoke FillConsoleOutputAttribute, hConsole, 07h, screenSize, 0, ADDR charsWritten
    invoke SetConsoleCursorPosition, hConsole, 0
    ret
ClearScreen ENDP

; ============================================================
; HideCursor - Hide cursor
; ============================================================
HideCursor PROC
    mov cursorInfo.dwSize, 1
    mov cursorInfo.bVisible, 0
    invoke SetConsoleCursorInfo, hConsole, ADDR cursorInfo
    ret
HideCursor ENDP

; ============================================================
; ShowCursor - Show cursor
; ============================================================
ShowCursor PROC
    mov cursorInfo.dwSize, 25
    mov cursorInfo.bVisible, 1
    invoke SetConsoleCursorInfo, hConsole, ADDR cursorInfo
    ret
ShowCursor ENDP

; ============================================================
; DrawProgress - Draw progress bar
; ============================================================
DrawProgress PROC progress:DWORD
    LOCAL i:DWORD
    
    invoke SetColor, CLR_BORDER
    invoke PrintString, ADDR progressL
    
    invoke SetColor, CLR_PROGRESS_FG
    mov i, 0
@@fillLoop:
    mov eax, i
    cmp eax, progress
    jge @@emptyStart
    invoke PrintString, ADDR progressF
    inc i
    jmp @@fillLoop
    
@@emptyStart:
    invoke SetColor, CLR_PROGRESS_BG
@@emptyLoop:
    mov eax, i
    cmp eax, 50
    jge @@done
    invoke PrintString, ADDR progressE
    inc i
    jmp @@emptyLoop
    
@@done:
    invoke SetColor, CLR_BORDER
    invoke PrintString, ADDR progressR
    ret
DrawProgress ENDP

; ============================================================
; PrintCheck - Print check message then value with delay
; ============================================================
PrintCheck PROC lpMsg:DWORD, lpVal:DWORD, delayMs:DWORD
    invoke SetColor, CLR_INFO
    invoke PrintString, lpMsg
    
    ; Short pause before showing result
    invoke Sleep, 300
    
    invoke SetColor, CLR_SUCCESS
    invoke PrintString, lpVal
    
    ; Beep on success
    invoke Beep, 1200, 30
    
    ; Delay after action
    invoke Sleep, delayMs
    ret
PrintCheck ENDP

; ============================================================
; PrintPhase - Print phase header
; ============================================================
PrintPhase PROC lpMsg:DWORD
    invoke Sleep, 500
    invoke SetColor, CLR_WARNING
    invoke PrintString, lpMsg
    invoke Beep, 800, 50
    invoke Sleep, 800
    ret
PrintPhase ENDP

; ============================================================
; DoCountdown - 35 second countdown before launch
; ============================================================
DoCountdown PROC
    LOCAL seconds:DWORD
    LOCAL yPos:DWORD
    
    ; Print countdown header
    invoke SetColor, CLR_BORDER
    invoke PrintString, ADDR msgCountHdr
    
    ; Get current Y position for countdown updates
    invoke GetConsoleScreenBufferInfo, hConsole, ADDR screenInfo
    movzx eax, screenInfo.dwCursorPosition.Y
    mov yPos, eax
    
    ; Start countdown from 35
    mov seconds, 35
    
@@countLoop:
    ; Position cursor
    invoke SetPos, 0, yPos
    
    ; Print countdown text
    invoke SetColor, CLR_COUNTDOWN
    invoke PrintString, ADDR msgCountTxt
    
    ; Print number
    invoke SetColor, CLR_WARNING
    invoke wsprintfA, ADDR countBuf, ADDR msgCountFmt, seconds
    invoke PrintString, ADDR countBuf
    
    ; Print "seconds..."
    invoke SetColor, CLR_COUNTDOWN
    invoke PrintString, ADDR msgCountSec
    
    ; Beep every 5 seconds and on last 5
    mov eax, seconds
    cmp eax, 5
    jle @@beepLoud
    
    ; Check if divisible by 5
    xor edx, edx
    mov ecx, 5
    div ecx
    cmp edx, 0
    jne @@noBeep
    invoke Beep, 600, 100
    jmp @@noBeep
    
@@beepLoud:
    ; Louder beeps for last 5 seconds
    invoke Beep, 1000, 150
    
@@noBeep:
    ; Wait 1 second
    invoke Sleep, 1000
    
    ; Decrement counter
    dec seconds
    cmp seconds, 0
    jg @@countLoop
    
    ret
DoCountdown ENDP

; ============================================================
; Main Entry Point
; ============================================================
start:
    ; Get console handle
    invoke GetStdHandle, STD_OUTPUT_HANDLE
    mov hConsole, eax
    
    ; Set console title
    invoke SetConsoleTitleA, ADDR consoleTitle
    
    ; Clear screen and hide cursor
    invoke ClearScreen
    invoke HideCursor
    
    ; === BOOT BEEP ===
    invoke Beep, 800, 150
    invoke Sleep, 200
    
    ; === Draw top border ===
    invoke SetColor, CLR_BORDER
    invoke PrintString, ADDR msgBorder
    invoke PrintString, ADDR msgNewLine
    
    ; === Draw ASCII Logo ===
    invoke SetColor, CLR_LOGO
    invoke PrintString, ADDR logo1
    invoke Sleep, 100
    invoke PrintString, ADDR logo2
    invoke Sleep, 100
    invoke PrintString, ADDR logo3
    invoke Sleep, 100
    invoke PrintString, ADDR logo4
    invoke Sleep, 100
    invoke PrintString, ADDR logo5
    invoke Sleep, 100
    invoke PrintString, ADDR logo6
    
    invoke Sleep, 1000
    
    ; === Draw separator ===
    invoke PrintString, ADDR msgNewLine
    invoke SetColor, CLR_BORDER
    invoke PrintString, ADDR msgBorder
    
    ; === BIOS POST ===
    invoke Sleep, 500
    invoke SetColor, CLR_TITLE
    invoke PrintString, ADDR msgBios
    invoke Beep, 1000, 100
    invoke Sleep, 800
    
    ; =====================================================
    ; PHASE 1: Hardware Detection
    ; =====================================================
    invoke PrintPhase, ADDR msgPhase1
    
    invoke PrintCheck, ADDR msgCpu, ADDR msgCpuVal, 600
    invoke DrawProgress, 5
    
    invoke PrintCheck, ADDR msgCpuCache, ADDR msgCpuCacheV, 500
    invoke DrawProgress, 8
    
    invoke PrintCheck, ADDR msgMem, ADDR msgMemVal, 700
    invoke DrawProgress, 12
    
    invoke PrintCheck, ADDR msgMemTest, ADDR msgMemTestV, 900
    invoke DrawProgress, 16
    
    ; =====================================================
    ; PHASE 2: Storage & I/O
    ; =====================================================
    invoke PrintPhase, ADDR msgPhase2
    
    invoke PrintCheck, ADDR msgDisk, ADDR msgDiskVal, 600
    invoke DrawProgress, 20
    
    invoke PrintCheck, ADDR msgPart, ADDR msgPartVal, 500
    invoke DrawProgress, 24
    
    invoke PrintCheck, ADDR msgFs, ADDR msgFsVal, 700
    invoke DrawProgress, 28
    
    invoke PrintCheck, ADDR msgIo, ADDR msgIoVal, 500
    invoke DrawProgress, 32
    
    ; =====================================================
    ; PHASE 3: Kernel Loading
    ; =====================================================
    invoke PrintPhase, ADDR msgPhase3
    
    invoke PrintCheck, ADDR msgBoot, ADDR msgBootVal, 600
    invoke DrawProgress, 36
    
    invoke PrintCheck, ADDR msgKernel, ADDR msgKernelVal, 800
    invoke DrawProgress, 40
    
    invoke PrintCheck, ADDR msgKernSz, ADDR msgKernSzV, 500
    invoke DrawProgress, 43
    
    invoke PrintCheck, ADDR msgDecomp, ADDR msgDecompV, 900
    invoke DrawProgress, 46
    
    ; =====================================================
    ; PHASE 4: System Services
    ; =====================================================
    invoke PrintPhase, ADDR msgPhase4
    
    invoke PrintCheck, ADDR msgInit, ADDR msgInitVal, 600
    invoke DrawProgress, 48
    
    invoke PrintCheck, ADDR msgSched, ADDR msgSchedV, 500
    invoke DrawProgress, 49
    
    invoke PrintCheck, ADDR msgMm, ADDR msgMmV, 600
    invoke DrawProgress, 50
    
    invoke PrintCheck, ADDR msgVfs, ADDR msgVfsV, 500
    invoke DrawProgress, 50
    
    invoke PrintCheck, ADDR msgLog, ADDR msgLogV, 500
    invoke DrawProgress, 50
    
    ; =====================================================
    ; PHASE 5: Final Checks
    ; =====================================================
    invoke PrintPhase, ADDR msgPhase5
    
    invoke PrintCheck, ADDR msgInteg, ADDR msgIntegV, 800
    invoke DrawProgress, 50
    
    invoke PrintCheck, ADDR msgReady, ADDR msgReadyV, 500
    invoke DrawProgress, 50
    
    ; === Success beeps ===
    invoke Beep, 600, 100
    invoke Beep, 800, 100
    invoke Beep, 1000, 150
    
    ; =====================================================
    ; COUNTDOWN: 35 seconds before kernel launch
    ; =====================================================
    invoke DoCountdown
    
    ; =====================================================
    ; LAUNCH SEQUENCE
    ; =====================================================
    invoke SetColor, CLR_BORDER
    invoke PrintString, ADDR msgLaunchHdr
    
    invoke SetColor, CLR_ERROR
    invoke PrintString, ADDR msgLaunch1
    invoke Sleep, 200
    invoke PrintString, ADDR msgLaunch2
    invoke Sleep, 200
    invoke PrintString, ADDR msgLaunch3
    
    ; Final beeps
    invoke Beep, 1200, 200
    invoke Sleep, 500
    invoke Beep, 1500, 300
    invoke Sleep, 500
    
    invoke SetColor, CLR_WARNING
    invoke PrintString, ADDR msgLaunchNow
    
    invoke Sleep, 1000
    
    ; === Initialize STARTUPINFO ===
    mov startupInfo.cb, SIZEOF STARTUPINFO
    
    ; === Launch KernelApp.exe ===
    invoke CreateProcessA, 0, ADDR kernelPath, 0, 0, 0, 0, 0, 0, ADDR startupInfo, ADDR processInfo
    
    ; === Footer ===
    invoke SetColor, CLR_INFO
    invoke PrintString, ADDR msgVersion
    invoke PrintString, ADDR msgCopy
    
    ; === Brief wait then exit ===
    invoke Sleep, 2000
    invoke ExitProcess, 0

END start