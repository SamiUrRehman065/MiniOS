@echo off

REM Force working directory to this .bat file's location
cd /d %~dp0

echo Assembling BootLoader...
ml /c /coff bootloader.asm

echo Linking BootLoader...
link /subsystem:console bootloader.obj kernel32.lib user32.lib /OUT:bootloader.exe

echo Done!
pause
