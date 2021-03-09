@echo off

if exist "%programfiles(x86)%" (
	:: 64-bit
	set "prodcode={179B1CCA-65E3-4A97-8208-590C0160E347}"
	set "nodejsfile=node-v15.5.0-x64.msi"
) else (
	:: 32-bit
	set "prodcode={7136155B-06A7-43C8-BB15-C01141CBBB8C}"
	set "nodejsfile=node-v15.5.0-x86.msi"
)

:: check if installed
reg query "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall" | findstr /I /C:"%prodcode%" >nul
if %ERRORLEVEL% equ 0 (
	:: it is installed
	echo Uninstalling... Please wait...
	:: uninstall it on the background
	msiexec /uninstall "%prodcode%" /quiet /qn /norestart
	echo Uninstallation done.
) else (
	echo %nodejsfile% with GUID: %prodcode% is not installed.
)