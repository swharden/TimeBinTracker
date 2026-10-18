@echo off
set FUNCTION_NAME=time-bin-tracker-lambda
set AWS_PROFILE=default
set AWS_REGION=us-east-2

dotnet lambda package --configuration Release --function-runtime dotnet10 --output-package bin\Release\function.zip
if errorlevel 1 exit /b %errorlevel%

aws --no-cli-pager lambda update-function-code --function-name %FUNCTION_NAME% --zip-file fileb://bin/Release/function.zip --profile %AWS_PROFILE% --region %AWS_REGION%
