# PostgreSQL Provider

This folder contains the PostgreSQL database provider implementation for the DTL Manager application.

## Overview

The PostgreSQL provider connects to PostgreSQL databases, making it suitable for:
- Open-source environments
- Cross-platform deployments
- High-performance applications
- Applications requiring advanced SQL features
- Cloud-native deployments

## Components

### `PostgreSqlDataProvider.cs`
- Implements `IDataProvider` interface
- Creates PostgreSQL connections using Npgsql
- Manages connection strings and database connectivity

### `PostgreSqlProductRepository.cs`
- Implements `IProductRepository` interface
- Uses Dapper ORM for database operations
- Optimized for PostgreSQL-specific features
- Uses `RETURNING Id` for getting new record IDs

### `PostgreSqlUserRepository.cs`
- Implements `IUserRepository` interface
- Handles user authentication and management
- Uses parameterized queries to prevent SQL injection
- Supports all CRUD operations with proper error handling

## Database Requirements

- **PostgreSQL 12** or later (recommended: PostgreSQL 15+)
- **Database**: dtlmanager (automatically created by setup scripts)
- **Extensions**: None required (uses standard PostgreSQL features)
- **Permissions**: CREATEDB, CONNECT, CREATE privileges

## Configuration

To use the PostgreSQL provider, set the following in your `appsettings.json`:

```json
{
  "Database": {
    "Provider": "postgresql",
    "ConnectionString": "Host=localhost;Database=dtlmanager;Username=postgres;Password=yourpassword"
  }
}
```

### Connection String Options

**Local Development:**
```
Host=localhost;Database=dtlmanager;Username=postgres;Password=yourpassword
```

**With Port Specification:**
```
Host=localhost;Port=5432;Database=dtlmanager;Username=postgres;Password=yourpassword
```

**SSL Connection:**
```
Host=localhost;Database=dtlmanager;Username=postgres;Password=yourpassword;SSL Mode=Require
```

**Connection Pooling:**
```
Host=localhost;Database=dtlmanager;Username=postgres;Password=yourpassword;Pooling=true;MinPoolSize=1;MaxPoolSize=20
```

## Database Setup

1. **Automated Setup** (Recommended):
   ```powershell
   .\setup-database.ps1 -DatabaseType postgresql
   ```

2. **Manual Setup**:
   ```bash
   # Connect to PostgreSQL
   psql -U postgres
   
   # Create database
   CREATE DATABASE dtlmanager;
   \c dtlmanager;
   
   # Run setup scripts
   \i Database/PostgreSQL/01_create_tables.sql
   \i Database/PostgreSQL/02_seed_data.sql
   \i Database/PostgreSQL/03_update_scripts.sql
   ```

## Features

- **ACID Compliance**: Full transaction support with excellent performance
- **Advanced Data Types**: Support for JSON, arrays, and custom types
- **Performance**: Excellent query optimization and execution plans
- **Triggers**: Automatic UpdatedAt timestamp management with PL/pgSQL
- **Indexing**: B-tree indexes on commonly queried fields
- **Constraints**: Data integrity through check constraints and foreign keys
- **Extensibility**: Support for custom functions and extensions

## PostgreSQL-Specific Advantages

- **RETURNING Clause**: Efficiently returns inserted/updated data
- **SERIAL Types**: Auto-incrementing primary keys
- **Triggers**: PL/pgSQL trigger functions for automatic timestamps
- **JSON Support**: Native JSON data type support (future extensibility)
- **Full-Text Search**: Built-in search capabilities
- **Vacuum**: Automatic space reclamation

## Performance Considerations

- Uses Npgsql connection pooling
- Implements proper parameter binding
- Leverages PostgreSQL's advanced query planner
- Optimized queries with appropriate indexes
- Uses prepared statements for repeated queries

## Security Features

- **Parameterized Queries**: Prevents SQL injection attacks
- **SSL Support**: Encrypted connections supported
- **Role-Based Security**: PostgreSQL's robust user management
- **Row-Level Security**: Fine-grained access control (if needed)
- **Password Hashing**: BCrypt implementation in application layer

## Development Tips

1. **pgAdmin**: Use pgAdmin for database management
2. **Logging**: Enable query logging for development
3. **Monitoring**: Use pg_stat_statements for query analysis
4. **Backup**: Regular backups with pg_dump
5. **Migrations**: Version control your schema changes

This provider is ideal for applications requiring a robust, open-source database with excellent performance and advanced features.
