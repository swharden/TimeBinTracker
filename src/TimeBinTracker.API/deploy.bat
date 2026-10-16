@echo off
setlocal

cd /d "%~dp0"
if errorlevel 1 exit /b %errorlevel%

where sam >nul 2>&1
if errorlevel 1 (
    echo AWS SAM CLI was not found. Install it with:
    echo   winget install -e --id Amazon.SAM-CLI
    echo Then open a new terminal and run this script again.
	pause
    exit /b 1
)

sam build
if errorlevel 1 exit /b %errorlevel%

sam deploy --stack-name time-bin-tracker-api --resolve-s3 --capabilities CAPABILITY_IAM
exit /b %errorlevel%
