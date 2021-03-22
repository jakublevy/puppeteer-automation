@echo off
setlocal

net session >nul 2>&1
if %errorLevel% neq 0 (
	echo Administrative permission required, please run %0 again as an administrator.
	goto :exit_handler
)

set "olddir=%cd%"
cd /d "%~dp0"

if exist "%programfiles(x86)%" (
	rem 64-bit
	set "prodcode={179B1CCA-65E3-4A97-8208-590C0160E347}"
	set "nodejsurl=https://nodejs.org/download/release/v15.5.0/node-v15.5.0-x64.msi"
	rem sha256
	set "checksum=8e3837f4b3ca1df94c9a7380dcdc0b623f94d0cd1d580eb4d28fef52dbb59967"
) else (
	rem 32-bit
	set "prodcode={7136155B-06A7-43C8-BB15-C01141CBBB8C}"
	set "nodejsurl=https://nodejs.org/download/release/v15.5.0/node-v15.5.0-x86.msi"
	rem sha256
	set "checksum=40d2239d59bcab4a2d8cdc1d65727c61ede2b39945f5b6343a9d2ba8acaf0960"
)

rem replace / with space
for %%I in (%nodejsurl:/= %) do set "nodejsfile=%%I"

rem check if installed
reg query "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall" | findstr /I /C:"%prodcode%" >nul
if %errorlevel% equ 0 (
	rem already installed, exiting
	echo %nodejsfile% with GUID: %prodcode% is installed.
	goto :exit_handler
)

set "installerpath=%~dp0%nodejsfile%"
set "created=0"
rem download suitable node.js installer
if not exist "%installerpath%" (
	set "created=1"
	bitsadmin /transfer "Downloading %nodejsfile%" /priority FOREGROUND "%nodejsurl%" "%installerpath%"
)

rem installation file already exists
if %created% equ 0 (
	echo Found existing %nodejsfile%.
)

rem checksum check
for /F "skip=1" %%I in ('certutil -hashfile "%installerpath%" SHA256') do if not defined comphash set "comphash=%%I"
if "%comphash%" == "%checksum%" (
	echo Checksum matches.
) else (
	echo Invalid checksum.
	echo Nothing was installed.
	call :cleanup
	goto :exit_handler
)

echo Installing... Please wait...
rem install it on the background
%nodejsfile% /quiet /qn /norestart

if %errorlevel% equ 0 (
	echo Installation done.
) else (
	echo Installation failed.
)

call :cleanup
goto :exit_handler

:exit_handler
	cd /d "%olddir%"
	endlocal
	rem Launched using double-click?
	echo %cmdcmdline% | findstr /I /C:/c >nul
	if %errorlevel% equ 0 (
		rem double-clicked
		pause
	)
exit /b

:cleanup
	rem remove downloaded installer
	if %created% equ 1 (
		del /Q /F "%installerpath%"
		echo Downloaded installer file "%nodejsfile%" removed.
	)
exit /b
