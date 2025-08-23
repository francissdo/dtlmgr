@echo off
echo Starting DtlMgr Development Environment...
echo.

echo Starting backend API (DtlApi)...
start cmd /k "cd /d %~dp0DtlApi && dotnet run"

echo Waiting for API to start...
timeout /t 5 /nobreak > nul

echo Starting frontend React app (dtlmgr)...
start cmd /k "cd /d %~dp0dtlmgr && npm start"

echo.
echo Both applications are starting...
echo Backend API: https://localhost:7148
echo Frontend App: http://localhost:3000
echo.
pause
