@echo off
cd /d %~dp0
echo Assembling Syscall Library...
ml /c /coff syscall.asm

echo Linking Syscall DLL...
link /DLL /DEF:syscall.def /SUBSYSTEM:WINDOWS syscall.obj kernel32.lib user32.lib /OUT:syscall.dll

echo Done!
pause