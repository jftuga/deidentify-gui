@echo off
setlocal

rem VIRTUAL_ENV should point to the deidentify path
rem for example: c:\work\deidentify  (deidentify.py resides within this folder)
set VIRTUAL_ENV=%~d0%~p0
set PYTHONHOME=
set PATH=%VIRTUAL_ENV%\Scripts;%PATH%

start deidentify-gui.exe

:END
endlocal
