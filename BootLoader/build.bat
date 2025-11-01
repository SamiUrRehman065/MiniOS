@echo off
echo Assembling BootLoader...
ml /c /coff bootloader.asm
echo Linking BootLoader...
link /subsystem:console bootloader.obj kernel32.lib user32.lib /OUT:bootloader.exe
echo Done!
pause
