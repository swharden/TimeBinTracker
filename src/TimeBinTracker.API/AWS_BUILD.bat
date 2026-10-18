:: One-time install of the Lambda tooling
dotnet tool install -g Amazon.Lambda.Tools

:: Build
dotnet lambda package --configuration Release --function-runtime dotnet10 --output-package bin\Release\function.zip