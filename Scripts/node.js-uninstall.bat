@echo off
setlocal

net session >nul 2>&1
if %errorLevel% neq 0 (
	echo Administrative permission required, please run %0 again as an administrator.
	goto :exit_handler
)

if exist "%programfiles(x86)%" (
	rem 64-bit
	set "prodcode={179B1CCA-65E3-4A97-8208-590C0160E347}"
	set "nodejsfile=node-v15.5.0-x64.msi"
) else (
	rem 32-bit
	set "prodcode={7136155B-06A7-43C8-BB15-C01141CBBB8C}"
	set "nodejsfile=node-v15.5.0-x86.msi"
)

rem check if installed
reg query "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall" | findstr /I /C:"%prodcode%" >nul
if %errorLevel% equ 0 (
	rem it is installed
	echo Uninstalling... Please wait...
	rem uninstall it on the background
	msiexec /uninstall "%prodcode%" /quiet /qn /norestart
	echo Uninstallation done.
) else (
	echo %nodejsfile% with GUID: %prodcode% is not installed.
)

goto :exit_handler

:exit_handler
	endlocal
	rem Launched using double-click?
	echo %cmdcmdline% | findstr /I /C:/c >nul
	if %errorlevel% equ 0 (
		rem double-clicked
		pause
	)
exit /b
