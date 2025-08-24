# SQL Server Provider

This folder contains the SQL Server database provider implementation for the DTL Manager application.

## Overview

The SQL Server provider connects to Microsoft SQL Server databases, making it suitable for:
- Enterprise applications
- Production deployments
- High-performance requirements
- Windows-centric environments
- Applications requiring ACID compliance

## Components

### `SqlServerDataProvider.cs`
- Implements `IDataProvider` interface
- Creates SQL Server connections using Microsoft.Data.SqlClient
- Manages connection strings and database connectivity

### `SqlServerProductRepository.cs`
- Implements `IProductRepository` interface
- Uses Dapper ORM for database operations
- Optimized for SQL Server-specific features
- Uses `SCOPE_IDENTITY()` for getting new record IDs

### `SqlServerUserRepository.cs`
- Implements `IUserRepository` interface
- Handles user authentication and management
- Uses parameterized queries to prevent SQL injection
- Supports all CRUD operations with proper error handling

## Database Requirements

- **SQL Server 2016** or later (recommended: SQL Server 2019+)
- **Database**: DTLManager (automatically created by setup scripts)
- **Authentication**: Windows Authentication or SQL Server Authentication
- **Permissions**: db_datareader, db_datawriter, db_ddladmin

## Configuration

To use the SQL Server provider, set the following in your `appsettings.json`:

```json
{
  "Database": {
    "Provider": "sqlserver",
    "ConnectionString": "Server=localhost;Database=DTLManager;Trusted_Connection=true;TrustServerCertificate=true"
  }
}
```

### Connection String Options

**Windows Authentication:**
```
Server=localhost;Database=DTLManager;Trusted_Connection=true;TrustServerCertificate=true
```

**SQL Server Authentication:**
```
Server=localhost;Database=DTLManager;User Id=sa;Password=YourPassword;TrustServerCertificate=true
```

**Azure SQL Database:**
```
Server=tcp:yourserver.database.windows.net,1433;Database=DTLManager;User ID=yourusername;Password=yourpassword;Encrypt=true
```

## Database Setup

1. **Automated Setup** (Recommended):
   ```powershell
   .\setup-database.ps1 -DatabaseType sqlserver
   ```

2. **Manual Setup**:
   ```sql
   -- Run these scripts in order:
   Database\SqlServer\01_create_tables.sql
   Database\SqlServer\02_seed_data.sql
   Database\SqlServer\03_update_scripts.sql
   ```

## Features

- **High Performance**: Optimized for SQL Server's query engine
- **ACID Compliance**: Full transaction support
- **Scalability**: Handles large datasets efficiently
- **Security**: Integrated Windows Authentication support
- **Triggers**: Automatic UpdatedAt timestamp management
- **Indexing**: Performance indexes on commonly queried fields
- **Constraints**: Data integrity through check constraints

## Performance Considerations

- Uses connection pooling automatically
- Implements proper parameter binding
- Leverages SQL Server's built-in functions (GETUTCDATE(), SCOPE_IDENTITY())
- Optimized queries with appropriate indexes

## Security Features

- **Parameterized Queries**: Prevents SQL injection attacks
- **Connection Security**: Supports encrypted connections
- **Authentication**: Windows and SQL Server authentication modes
- **Password Hashing**: BCrypt implementation in application layer

This provider is ideal for production environments requiring enterprise-grade database capabilities.
