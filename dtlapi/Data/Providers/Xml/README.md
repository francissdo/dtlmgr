# XML Data Provider

This provider implements XML file-based data storage for products and users. It provides a simple file-based alternative to database storage using XML serialization.

## Features

- **XML Serialization**: Uses .NET's built-in XmlSerializer for data persistence
- **File-based Storage**: Stores data in separate XML files for products and users
- **Automatic Directory Creation**: Creates data directory if it doesn't exist
- **Thread-safe Operations**: Uses file locking for concurrent access safety
- **Data Validation**: Validates XML structure and data integrity
- **Backup Support**: Easy to backup and restore using file system operations

## Configuration

Configure the XML provider in your `appsettings.json`:

```json
{
  "DatabaseSettings": {
    "Provider": "xml",
    "DataPath": "Data/XmlData"
  }
}
```

## Data Structure

### Products XML Format
```xml
<?xml version="1.0" encoding="utf-8"?>
<ArrayOfProduct xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <Product>
    <Id>1</Id>
    <Name>Product Name</Name>
    <Description>Product Description</Description>
    <Price>29.99</Price>
    <Category>Electronics</Category>
    <StockQuantity>100</StockQuantity>
    <CreatedAt>2024-01-01T00:00:00</CreatedAt>
    <UpdatedAt>2024-01-01T00:00:00</UpdatedAt>
  </Product>
</ArrayOfProduct>
```

### Users XML Format
```xml
<?xml version="1.0" encoding="utf-8"?>
<ArrayOfUser xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <User>
    <Id>1</Id>
    <Username>user@example.com</Username>
    <Email>user@example.com</Email>
    <PasswordHash>$2a$11$hashedPassword...</PasswordHash>
    <FirstName>John</FirstName>
    <LastName>Doe</LastName>
    <CreatedAt>2024-01-01T00:00:00</CreatedAt>
    <UpdatedAt>2024-01-01T00:00:00</UpdatedAt>
  </User>
</ArrayOfUser>
```

## Usage Examples

### Basic Configuration
```csharp
// In Program.cs - automatically configured based on appsettings.json
var xmlDataPath = Path.Combine(builder.Environment.ContentRootPath, "Data/XmlData");
builder.Services.AddSingleton(provider => new XmlDataProvider(xmlDataPath));
builder.Services.AddScoped<IProductRepository, XmlProductRepository>();
builder.Services.AddScoped<IUserRepository, XmlUserRepository>();
```

### Manual Usage
```csharp
// Create provider instance
var dataPath = @"C:\MyApp\Data\XmlData";
var xmlProvider = new XmlDataProvider(dataPath);

// Use repositories
var productRepo = new XmlProductRepository(xmlProvider);
var userRepo = new XmlUserRepository(xmlProvider);

// Perform operations
var products = await productRepo.GetAllAsync();
var user = await userRepo.GetByUsernameAsync("user@example.com");
```

## File Structure

When using the XML provider, the following files will be created in your configured data path:

```
Data/XmlData/
├── products.xml      # Product data storage
├── users.xml         # User data storage
└── backups/          # Optional backup directory
    ├── products_backup_20240101.xml
    └── users_backup_20240101.xml
```

## Advantages

- **Human Readable**: XML files can be easily read and edited by humans
- **No Database Required**: No need for database installation or configuration
- **Version Control Friendly**: XML files work well with version control systems
- **Cross-platform**: Works on any platform that supports .NET
- **Easy Backup**: Simple file-based backup and restore operations
- **Lightweight**: Minimal dependencies and overhead

## Limitations

- **Performance**: Not suitable for large datasets or high-concurrency scenarios
- **Scalability**: Limited by file system performance
- **Concurrent Access**: File locking may cause delays under heavy load
- **Memory Usage**: Entire datasets loaded into memory
- **Transaction Support**: No built-in transaction support
- **Query Performance**: No indexing or advanced query optimization

## Best Practices

1. **Regular Backups**: Implement regular backup procedures for your XML files
2. **File Monitoring**: Monitor file sizes and performance
3. **Error Handling**: Implement proper error handling for file operations
4. **Data Validation**: Validate data before serialization
5. **Concurrent Access**: Consider implementing application-level locking for high-concurrency scenarios

## Migration

### From JSON Provider
The XML provider uses the same data models as the JSON provider, making migration straightforward. You can convert existing JSON data to XML format or use both providers simultaneously during transition.

### To Database Provider
When ready to migrate to a database provider, the XML data can be easily imported using the same repository interfaces.

## Troubleshooting

### Common Issues

1. **File Access Errors**: Ensure the application has read/write permissions to the data directory
2. **XML Parsing Errors**: Validate XML file structure and encoding
3. **Performance Issues**: Consider migrating to a database provider for large datasets
4. **Concurrent Access**: Implement application-level coordination for high-concurrency scenarios

### Debugging

Enable detailed logging to troubleshoot XML provider issues:

```csharp
// In appsettings.json
{
  "Logging": {
    "LogLevel": {
      "dtlapi.Data.Providers.Xml": "Debug"
    }
  }
}
```

This provider is ideal for development, testing, small applications, or scenarios where a database is not available or desired.
