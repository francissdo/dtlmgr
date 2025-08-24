# Database Providers

This folder contains the database provider implementations for the DTL Manager application. Each provider is organized in its own subfolder for better maintainability and clear separation of concerns.

## Overview

The DTL Manager application supports multiple database providers through a clean abstraction layer. This design allows you to:

- **Switch databases easily** by changing configuration
- **Maintain provider-specific optimizations** without affecting other providers
- **Add new providers** without modifying existing code
- **Test with different databases** for compatibility and performance

## Provider Structure

```
Providers/
├── Json/                       # JSON file-based provider (development)
│   ├── JsonDataProvider.cs
│   ├── JsonProductRepository.cs
│   ├── JsonUserRepository.cs
│   └── README.md
├── SqlServer/                  # Microsoft SQL Server provider (enterprise)
│   ├── SqlServerDataProvider.cs
│   ├── SqlServerProductRepository.cs
│   ├── SqlServerUserRepository.cs
│   └── README.md
├── PostgreSql/                 # PostgreSQL provider (open-source)
│   ├── PostgreSqlDataProvider.cs
│   ├── PostgreSqlProductRepository.cs
│   ├── PostgreSqlUserRepository.cs
│   └── README.md
└── README.md                   # This file
```

## Provider Selection

The application automatically selects the appropriate provider based on your configuration in `appsettings.json`:

```json
{
  "Database": {
    "Provider": "json|sqlserver|postgresql",
    "ConnectionString": "...",
    "DataPath": "..."
  }
}
```

## Supported Providers

### 🗃️ JSON Provider (`json`)
- **Best for**: Development, testing, prototyping
- **Storage**: JSON files on disk
- **Setup**: No database required
- **Performance**: Low to medium
- **Concurrency**: Limited

### 🏢 SQL Server Provider (`sqlserver`)
- **Best for**: Enterprise applications, Windows environments
- **Storage**: Microsoft SQL Server database
- **Setup**: Requires SQL Server installation
- **Performance**: High
- **Concurrency**: Excellent

### 🐘 PostgreSQL Provider (`postgresql`)
- **Best for**: Open-source environments, cross-platform
- **Storage**: PostgreSQL database
- **Setup**: Requires PostgreSQL installation
- **Performance**: High
- **Concurrency**: Excellent

## Interface Contracts

All providers implement the same interfaces to ensure consistent behavior:

### `IDataProvider`
- `CreateConnection()` - Creates database connections
- `GetConnectionString()` - Returns connection information

### `IProductRepository`
- `GetAllAsync()` - Retrieve all products
- `GetByIdAsync(id)` - Get specific product
- `CreateAsync(product)` - Create new product
- `UpdateAsync(id, product)` - Update existing product
- `DeleteAsync(id)` - Delete product

### `IUserRepository`
- `GetAllAsync()` - Retrieve all users
- `GetByIdAsync(id)` - Get specific user
- `GetByUsernameAsync(username)` - Find user by username
- `GetByEmailAsync(email)` - Find user by email
- `CreateAsync(user)` - Create new user
- `UpdateAsync(id, user)` - Update existing user
- `DeleteAsync(id)` - Delete user

## Provider-Specific Features

### JSON Provider Features
- ✅ Auto-ID generation
- ✅ Default data creation
- ✅ File-based persistence
- ❌ Advanced querying
- ❌ Concurrent access

### SQL Server Provider Features
- ✅ ACID transactions
- ✅ Advanced indexing
- ✅ Windows Authentication
- ✅ Enterprise scalability
- ✅ Automatic triggers

### PostgreSQL Provider Features
- ✅ ACID transactions
- ✅ Advanced data types
- ✅ Full-text search
- ✅ Cross-platform support
- ✅ RETURNING clause optimization

## Adding New Providers

To add a new database provider:

1. **Create provider folder**: `Providers/YourProvider/`
2. **Implement interfaces**:
   - `YourDataProvider : IDataProvider`
   - `YourProductRepository : IProductRepository`
   - `YourUserRepository : IUserRepository`
3. **Update Program.cs**: Add new case in provider switch
4. **Add documentation**: Create README.md explaining the provider
5. **Add tests**: Implement provider-specific tests

## Migration Between Providers

When switching providers:

1. **Export data** from current provider
2. **Update configuration** to new provider
3. **Run database setup** for new provider
4. **Import data** to new provider
5. **Test application** with new provider

## Performance Considerations

| Provider   | Read Performance | Write Performance | Concurrency | Setup Complexity |
|------------|------------------|-------------------|-------------|------------------|
| JSON       | Low              | Low               | Poor        | Minimal          |
| SQL Server | High             | High              | Excellent   | Medium           |
| PostgreSQL | High             | High              | Excellent   | Medium           |

## Development Workflow

1. **Start with JSON** provider for rapid development
2. **Test with PostgreSQL** for cross-platform compatibility
3. **Deploy with SQL Server** for enterprise requirements
4. **Use same codebase** for all environments

This modular approach ensures that your application can grow from a simple prototype to an enterprise-grade solution without major architectural changes.
