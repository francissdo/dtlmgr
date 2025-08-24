# Start DTL API and React App
Write-Host "Starting DTL API..." -ForegroundColor Green
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd dtlapi; dotnet run"

Write-Host "Waiting for API to start..." -ForegroundColor Yellow
Start-Sleep -Seconds 5

Write-Host "Starting React App..." -ForegroundColor Green  
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd dtlmgr; npm start"

Write-Host ""
Write-Host "Both applications are starting..." -ForegroundColor Cyan
Write-Host "API: http://localhost:5272" -ForegroundColor White
Write-Host "React App: http://localhost:3000" -ForegroundColor White
Write-Host ""
Write-Host "Press any key to exit..." -ForegroundColor Yellow
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
