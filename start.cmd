@echo off
setlocal
cd /d "%~dp0"

echo.
echo === Autotests Trainer: start site ===
echo.
echo Site address after start:
echo http://localhost:5171
echo.
echo Demo login:
echo admin / admin
echo.

dotnet restore
if errorlevel 1 goto error

dotnet run --project ".\src\AutotestsTrainer.Web" --urls "http://localhost:5171"
if errorlevel 1 goto error

goto end

:error
echo.
echo Something went wrong. Check that .NET 8 SDK is installed:
echo https://dotnet.microsoft.com/download/dotnet/8.0
echo.
pause
exit /b 1

:end
endlocal
