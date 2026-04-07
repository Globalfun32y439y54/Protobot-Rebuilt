@echo off
REM Protobot-Rebuilt Automated Build Script
REM This script builds the EXE and can be run from command line

setlocal enabledelayedexpansion

REM Set paths
set "UNITY_PATH=C:\Program Files\Unity\Hub\Editor"
set "PROJECT_PATH=%~dp0"
set "BUILD_OUTPUT=%PROJECT_PATH%Builds\"
set "BUILD_TYPE=%1"

if "%BUILD_TYPE%"=="" set "BUILD_TYPE=release"

echo.
echo ========================================
echo Protobot-Rebuilt Build Script
echo ========================================
echo Build Type: %BUILD_TYPE%
echo Project Path: %PROJECT_PATH%
echo.

REM Find Unity installation
if not exist "%UNITY_PATH%" (
    echo Error: Unity not found at %UNITY_PATH%
    echo Please install Unity or update the UNITY_PATH variable
    pause
    exit /b 1
)

REM Find the latest Unity version
for /f "delims=" %%A in ('dir /b "%UNITY_PATH%" ^| findstr /r "^[0-9]" ^| sort /r') do (
    set "LATEST_VERSION=%%A"
    goto found_version
)

:found_version
set "UNITY_EXE=%UNITY_PATH%\%LATEST_VERSION%\Editor\Unity.exe"

if not exist "!UNITY_EXE!" (
    echo Error: Unity executable not found at !UNITY_EXE!
    pause
    exit /b 1
)

echo Using Unity: !UNITY_EXE!
echo.

REM Create build output directory
if not exist "%BUILD_OUTPUT%" mkdir "%BUILD_OUTPUT%"

REM Build the project
echo Building Protobot-Rebuilt...
if "%BUILD_TYPE%"=="release" (
    "!UNITY_EXE!" -projectPath "%PROJECT_PATH%" -executeMethod BuildManager.BuildWindowsEXE -logFile "%BUILD_OUTPUT%build.log" -quit -batchmode
) else if "%BUILD_TYPE%"=="dev" (
    "!UNITY_EXE!" -projectPath "%PROJECT_PATH%" -executeMethod BuildManager.BuildWindowsEXEDevelopment -logFile "%BUILD_OUTPUT%build.log" -quit -batchmode
) else (
    echo Invalid build type: %BUILD_TYPE%
    echo Usage: build.bat [release^|dev]
    pause
    exit /b 1
)

echo.
echo Build process started. Check the build log for details:
echo %BUILD_OUTPUT%build.log
echo.
echo Build output will be in: %BUILD_OUTPUT%
echo.
pause
