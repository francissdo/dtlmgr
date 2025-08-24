# JSON Provider

This folder contains the JSON file-based data provider implementation for the DTL Manager application.

## Overview

The JSON provider stores data in JSON files on the file system, making it ideal for:
- Development and testing
- Small-scale deployments
- Prototyping
- Scenarios where you don't want to set up a database

## Components

### `JsonDataProvider.cs`
- Implements `IDataProvider` interface
- Manages the data path for JSON files
- Provides utility methods for loading and saving JSON data

### `JsonProductRepository.cs`
- Implements `IProductRepository` interface
- Handles CRUD operations for products stored in `products.json`
- Provides in-memory sorting and filtering capabilities

### `JsonUserRepository.cs`
- Implements `IUserRepository` interface
- Handles CRUD operations for users stored in `users.json`
- Automatically creates default users if file doesn't exist
- Uses BCrypt for password hashing

## File Structure

When using the JSON provider, the following files will be created in the configured data path:

```
SampleData/
├── products.json    # Product data
└── users.json       # User data with hashed passwords
```

## Configuration

To use the JSON provider, set the following in your `appsettings.json`:

```json
{
  "Database": {
    "Provider": "json",
    "DataPath": "SampleData"
  }
}
```

## Features

- **Auto-ID Generation**: Automatically assigns incremental IDs to new records
- **Default Data**: Creates default users when starting with empty data
- **Thread-Safe**: Uses async file operations
- **Password Security**: Implements BCrypt password hashing
- **Data Persistence**: All changes are immediately saved to disk

## Limitations

- **Performance**: Not suitable for high-volume applications
- **Concurrency**: Limited concurrent access support
- **Querying**: No advanced query capabilities
- **Relationships**: No foreign key constraints or relationships

This provider is perfect for development and small deployments where simplicity is more important than performance.
