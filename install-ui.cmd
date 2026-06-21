@echo off
setlocal
cd /d "%~dp0"

echo.
echo === Autotests Trainer: install Playwright browser ===
echo.
echo This is needed only for future UI tests.
echo.

dotnet build
if errorlevel 1 goto error

powershell -NoProfile -ExecutionPolicy Bypass -File ".\tests\AutotestsTrainer.Tests\bin\Debug\net8.0\playwright.ps1" install chromium
if errorlevel 1 goto error

echo.
echo Playwright Chromium is installed.
pause
goto end

:error
echo.
echo Installation failed. Check the error above.
pause
exit /b 1

:end
endlocal
