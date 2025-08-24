#!/usr/bin/env pwsh
# PowerShell script to set up the DTL Manager database

param(
    [Parameter(Mandatory=$true)]
    [ValidateSet("postgresql", "sqlserver")]
    [string]$DatabaseType,
    
    [Parameter(Mandatory=$false)]
    [string]$ConnectionString,
    
    [Parameter(Mandatory=$false)]
    [switch]$SkipSeedData
)

Write-Host "Setting up DTL Manager Database..." -ForegroundColor Green
Write-Host "Database Type: $DatabaseType" -ForegroundColor Yellow

$scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Definition
$databasePath = Join-Path $scriptPath "Database"

if ($DatabaseType -eq "postgresql") {
    $scriptsPath = Join-Path $databasePath "PostgreSQL"
    
    if (-not $ConnectionString) {
        $ConnectionString = "Host=localhost;Database=dtlmanager;Username=postgres;Password=postgres"
    }
    
    Write-Host "Running PostgreSQL setup scripts..." -ForegroundColor Blue
    
    # Check if psql is available
    try {
        psql --version | Out-Null
    }
    catch {
        Write-Error "psql command not found. Please install PostgreSQL client tools."
        exit 1
    }
    
    # Extract connection parameters
    $connParams = @{}
    $ConnectionString -split ';' | ForEach-Object {
        $parts = $_ -split '='
        if ($parts.Length -eq 2) {
            $connParams[$parts[0]] = $parts[1]
        }
    }
    
    $dbName = $connParams['Database']
    $dbHost = $connParams['Host']
    $username = $connParams['Username']
    $password = $connParams['Password']
    
    # Set PGPASSWORD environment variable
    $env:PGPASSWORD = $password
    
    # Create database if it doesn't exist
    Write-Host "Creating database '$dbName' if it doesn't exist..." -ForegroundColor Cyan
    psql -h $dbHost -U $username -d postgres -c "CREATE DATABASE $dbName;" 2>$null
    
    # Run setup scripts
    Write-Host "Creating tables..." -ForegroundColor Cyan
    psql -h $dbHost -U $username -d $dbName -f (Join-Path $scriptsPath "01_create_tables.sql")
    
    if (-not $SkipSeedData) {
        Write-Host "Inserting seed data..." -ForegroundColor Cyan
        psql -h $dbHost -U $username -d $dbName -f (Join-Path $scriptsPath "02_seed_data.sql")
    }
    
    Write-Host "Running update scripts..." -ForegroundColor Cyan
    psql -h $dbHost -U $username -d $dbName -f (Join-Path $scriptsPath "03_update_scripts.sql")
    
    # Clear password from environment
    Remove-Item Env:PGPASSWORD
}
elseif ($DatabaseType -eq "sqlserver") {
    $scriptsPath = Join-Path $databasePath "SqlServer"
    
    if (-not $ConnectionString) {
        $ConnectionString = "Server=localhost;Database=DTLManager;Trusted_Connection=true;TrustServerCertificate=true"
    }
    
    Write-Host "Running SQL Server setup scripts..." -ForegroundColor Blue
    
    # Check if sqlcmd is available
    try {
        sqlcmd -? | Out-Null
    }
    catch {
        Write-Error "sqlcmd command not found. Please install SQL Server command line tools."
        exit 1
    }
    
    # Extract server and database from connection string
    $serverName = "localhost"
    $databaseName = "DTLManager"
    $useTrustedConnection = $false
    
    $ConnectionString -split ';' | ForEach-Object {
        $parts = $_ -split '='
        if ($parts.Length -eq 2) {
            switch ($parts[0].ToLower()) {
                "server" { $serverName = $parts[1] }
                "database" { $databaseName = $parts[1] }
                "trusted_connection" { $useTrustedConnection = $parts[1] -eq "true" }
            }
        }
    }
    
    $sqlcmdArgs = @("-S", $serverName)
    if ($useTrustedConnection) {
        $sqlcmdArgs += "-E"
    }
    
    # Create database if it doesn't exist
    Write-Host "Creating database '$databaseName' if it doesn't exist..." -ForegroundColor Cyan
    $createDbSql = "IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = '$databaseName') CREATE DATABASE [$databaseName];"
    & sqlcmd @sqlcmdArgs -Q $createDbSql
    
    # Add database to connection
    $sqlcmdArgs += @("-d", $databaseName)
    
    # Run setup scripts
    Write-Host "Creating tables..." -ForegroundColor Cyan
    & sqlcmd @sqlcmdArgs -i (Join-Path $scriptsPath "01_create_tables.sql")
    
    if (-not $SkipSeedData) {
        Write-Host "Inserting seed data..." -ForegroundColor Cyan
        & sqlcmd @sqlcmdArgs -i (Join-Path $scriptsPath "02_seed_data.sql")
    }
    
    Write-Host "Running update scripts..." -ForegroundColor Cyan
    & sqlcmd @sqlcmdArgs -i (Join-Path $scriptsPath "03_update_scripts.sql")
}

Write-Host "Database setup completed successfully!" -ForegroundColor Green
Write-Host ""
Write-Host "Default user accounts (password: password123):" -ForegroundColor Yellow
Write-Host "- admin (admin@dtlmanager.com)" -ForegroundColor White
Write-Host "- john_doe (john.doe@example.com)" -ForegroundColor White
Write-Host "- jane_smith (jane.smith@example.com)" -ForegroundColor White
Write-Host "- mike_wilson (mike.wilson@example.com)" -ForegroundColor White
Write-Host "- sarah_davis (sarah.davis@example.com)" -ForegroundColor White
Write-Host ""
Write-Host "Remember to update your appsettings.json with the correct connection string!" -ForegroundColor Magenta
