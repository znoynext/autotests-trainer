@echo off
setlocal
cd /d "%~dp0"

echo.
echo === Autotests Trainer: run tests ===
echo.

dotnet test
if errorlevel 1 goto error

echo.
echo Tests finished successfully.
pause
goto end

:error
echo.
echo Tests failed. Read the error above.
pause
exit /b 1

:end
endlocal
