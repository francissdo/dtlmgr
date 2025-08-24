@echo off
setlocal enabledelayedexpansion

echo Setting up DTL Manager Database...
echo.

if "%1"=="postgresql" goto postgresql
if "%1"=="sqlserver" goto sqlserver

echo Usage: setup-database.bat [postgresql^|sqlserver]
echo.
echo Examples:
echo   setup-database.bat postgresql
echo   setup-database.bat sqlserver
echo.
goto end

:postgresql
echo Setting up PostgreSQL database...
set PGPASSWORD=postgres

echo Creating database 'dtlmanager' if it doesn't exist...
psql -h localhost -U postgres -d postgres -c "CREATE DATABASE dtlmanager;" 2>nul

echo Running setup scripts...
psql -h localhost -U postgres -d dtlmanager -f "Database\PostgreSQL\01_create_tables.sql"
if errorlevel 1 goto error

psql -h localhost -U postgres -d dtlmanager -f "Database\PostgreSQL\02_seed_data.sql"
if errorlevel 1 goto error

psql -h localhost -U postgres -d dtlmanager -f "Database\PostgreSQL\03_update_scripts.sql"
if errorlevel 1 goto error

set PGPASSWORD=
echo PostgreSQL database setup completed successfully!
goto success

:sqlserver
echo Setting up SQL Server database...

echo Creating database 'DTLManager' if it doesn't exist...
sqlcmd -S localhost -E -Q "IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'DTLManager') CREATE DATABASE [DTLManager];"
if errorlevel 1 goto error

echo Running setup scripts...
sqlcmd -S localhost -E -d DTLManager -i "Database\SqlServer\01_create_tables.sql"
if errorlevel 1 goto error

sqlcmd -S localhost -E -d DTLManager -i "Database\SqlServer\02_seed_data.sql"
if errorlevel 1 goto error

sqlcmd -S localhost -E -d DTLManager -i "Database\SqlServer\03_update_scripts.sql"
if errorlevel 1 goto error

echo SQL Server database setup completed successfully!
goto success

:error
echo.
echo ERROR: Database setup failed!
echo Please check the error messages above and ensure:
echo - Database server is running and accessible
echo - You have appropriate permissions
echo - Command line tools (psql/sqlcmd) are installed and in PATH
echo.
exit /b 1

:success
echo.
echo Default user accounts (password: password123):
echo - admin (admin@dtlmanager.com)
echo - john_doe (john.doe@example.com)
echo - jane_smith (jane.smith@example.com)
echo - mike_wilson (mike.wilson@example.com)
echo - sarah_davis (sarah.davis@example.com)
echo.
echo Remember to update your appsettings.json with the correct connection string!
echo.

:end
endlocal
