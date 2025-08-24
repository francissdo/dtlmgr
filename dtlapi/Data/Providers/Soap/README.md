# SOAP Web Service Data Provider

This provider implements SOAP web service-based data access for products and users. It enables integration with legacy SOAP services and enterprise systems that expose data through SOAP endpoints.

## Features

- **WCF Integration**: Uses Windows Communication Foundation for SOAP communication
- **XML Serialization**: Handles SOAP XML message serialization/deserialization
- **Multiple Bindings**: Supports various SOAP bindings (BasicHttp, WSHttp, etc.)
- **Authentication Support**: WS-Security, Basic authentication, and custom authentication
- **Fault Handling**: Comprehensive SOAP fault handling and error reporting
- **Async Operations**: Full async/await support for non-blocking operations
- **Timeout Configuration**: Configurable operation timeouts
- **Custom Headers**: Support for custom SOAP headers

## Configuration

Configure the SOAP provider in your `appsettings.json`:

```json
{
  "ExternalServices": {
    "Soap": {
      "ProductService": {
        "EndpointUrl": "https://api.example.com/ProductService.asmx",
        "Binding": "BasicHttpBinding",
        "Timeout": "00:05:00",
        "Username": "your-username",
        "Password": "your-password",
        "AuthType": "Basic"
      },
      "UserService": {
        "EndpointUrl": "https://api.example.com/UserService.asmx",
        "Binding": "WSHttpBinding",
        "Timeout": "00:05:00",
        "AuthType": "WSUsernameToken",
        "Username": "your-username",
        "Password": "your-password"
      }
    }
  }
}
```

## Supported Bindings

### BasicHttpBinding
```json
{
  "Binding": "BasicHttpBinding",
  "SecurityMode": "Transport",
  "AuthType": "Basic"
}
```

### WSHttpBinding
```json
{
  "Binding": "WSHttpBinding",
  "SecurityMode": "Message",
  "AuthType": "WSUsernameToken"
}
```

### NetTcpBinding
```json
{
  "Binding": "NetTcpBinding",
  "SecurityMode": "Transport",
  "AuthType": "Windows"
}
```

## Authentication Types

### Basic Authentication
```xml
<soap:Header>
  <Authentication>
    <Username>your-username</Username>
    <Password>your-password</Password>
  </Authentication>
</soap:Header>
```

### WS-Security Username Token
```xml
<soap:Header>
  <wsse:Security>
    <wsse:UsernameToken>
      <wsse:Username>your-username</wsse:Username>
      <wsse:Password Type="PasswordText">your-password</wsse:Password>
    </wsse:UsernameToken>
  </wsse:Security>
</soap:Header>
```

### Custom Token Authentication
```xml
<soap:Header>
  <CustomAuth>
    <Token>your-custom-token</Token>
    <ClientId>your-client-id</ClientId>
  </CustomAuth>
</soap:Header>
```

## SOAP Operations

The SOAP provider expects the following operations to be available:

### Product Operations
- `GetAllProducts()` - Get all products
- `GetProductById(int id)` - Get product by ID
- `CreateProduct(Product product)` - Create new product
- `UpdateProduct(Product product)` - Update existing product
- `DeleteProduct(int id)` - Delete product
- `SearchProducts(string query)` - Search products

### User Operations
- `GetAllUsers()` - Get all users
- `GetUserById(int id)` - Get user by ID
- `GetUserByUsername(string username)` - Get user by username
- `GetUserByEmail(string email)` - Get user by email
- `CreateUser(User user)` - Create new user
- `UpdateUser(User user)` - Update existing user
- `DeleteUser(int id)` - Delete user

## SOAP Message Examples

### Product Service Request
```xml
<?xml version="1.0" encoding="utf-8"?>
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/" 
               xmlns:prod="http://api.example.com/ProductService">
  <soap:Header>
    <Authentication>
      <Username>your-username</Username>
      <Password>your-password</Password>
    </Authentication>
  </soap:Header>
  <soap:Body>
    <prod:GetProductById>
      <prod:id>123</prod:id>
    </prod:GetProductById>
  </soap:Body>
</soap:Envelope>
```

### Product Service Response
```xml
<?xml version="1.0" encoding="utf-8"?>
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/" 
               xmlns:prod="http://api.example.com/ProductService">
  <soap:Body>
    <prod:GetProductByIdResponse>
      <prod:GetProductByIdResult>
        <prod:Id>123</prod:Id>
        <prod:Name>Sample Product</prod:Name>
        <prod:Description>Product Description</prod:Description>
        <prod:Price>29.99</prod:Price>
        <prod:Category>Electronics</prod:Category>
        <prod:StockQuantity>100</prod:StockQuantity>
        <prod:CreatedAt>2024-01-01T00:00:00Z</prod:CreatedAt>
        <prod:UpdatedAt>2024-01-01T00:00:00Z</prod:UpdatedAt>
      </prod:GetProductByIdResult>
    </prod:GetProductByIdResponse>
  </soap:Body>
</soap:Envelope>
```

## Usage Examples

### Basic Configuration
```csharp
// In Program.cs - automatically configured based on appsettings.json
builder.Services.AddScoped<SoapProductService>();
builder.Services.AddScoped<SoapUserService>();
```

### Manual Client Creation
```csharp
// Create SOAP client manually
var binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
var endpoint = new EndpointAddress("https://api.example.com/ProductService.asmx");
var client = new ProductServiceClient(binding, endpoint);

// Configure authentication
client.ClientCredentials.UserName.UserName = "your-username";
client.ClientCredentials.UserName.Password = "your-password";

// Use client
var products = await client.GetAllProductsAsync();
```

### Custom Binding Configuration
```csharp
// Configure custom binding
var binding = new WSHttpBinding(SecurityMode.Message);
binding.Security.Message.ClientCredentialType = MessageCredentialType.UserName;
binding.Security.Message.EstablishSecurityContext = false;

var endpoint = new EndpointAddress("https://api.example.com/UserService.svc");
var client = new UserServiceClient(binding, endpoint);

// Configure credentials
client.ClientCredentials.UserName.UserName = "your-username";
client.ClientCredentials.UserName.Password = "your-password";
```

## Service Reference Generation

### Add Service Reference
```bash
# Using dotnet CLI
dotnet add package Microsoft.Extensions.Http.WCF
dotnet svcutil https://api.example.com/ProductService.asmx?wsdl

# Using Visual Studio
# Right-click project > Add > Service Reference > WCF Web Service
```

### Manual Proxy Generation
```csharp
// Generate proxy classes
[ServiceContract]
public interface IProductService
{
    [OperationContract]
    Task<Product[]> GetAllProductsAsync();
    
    [OperationContract]
    Task<Product> GetProductByIdAsync(int id);
    
    [OperationContract]
    Task<int> CreateProductAsync(Product product);
    
    [OperationContract]
    Task<bool> UpdateProductAsync(Product product);
    
    [OperationContract]
    Task<bool> DeleteProductAsync(int id);
}
```

## Error Handling

### SOAP Fault Handling
```csharp
try
{
    var product = await soapProductService.GetProductByIdAsync(id);
    return product;
}
catch (FaultException<ServiceFault> ex)
{
    // Handle known service faults
    _logger.LogError("Service fault: {FaultCode} - {FaultReason}", 
        ex.Detail.FaultCode, ex.Detail.FaultReason);
    throw new ServiceException(ex.Detail.FaultReason);
}
catch (FaultException ex)
{
    // Handle generic SOAP faults
    _logger.LogError("SOAP fault: {Code} - {Reason}", 
        ex.Code, ex.Reason);
    throw new ServiceException(ex.Reason.ToString());
}
catch (CommunicationException ex)
{
    // Handle communication errors
    _logger.LogError("Communication error: {Message}", ex.Message);
    throw new ServiceException("Service communication failed");
}
```

### Custom Fault Contracts
```csharp
[DataContract]
public class ServiceFault
{
    [DataMember]
    public string FaultCode { get; set; }
    
    [DataMember]
    public string FaultReason { get; set; }
    
    [DataMember]
    public string Details { get; set; }
}

[ServiceContract]
public interface IProductService
{
    [OperationContract]
    [FaultContract(typeof(ServiceFault))]
    Task<Product> GetProductByIdAsync(int id);
}
```

## Security Configuration

### WS-Security Username Token
```csharp
var binding = new WSHttpBinding(SecurityMode.Message);
binding.Security.Message.ClientCredentialType = MessageCredentialType.UserName;

var client = new ProductServiceClient(binding, endpoint);
client.ClientCredentials.UserName.UserName = "username";
client.ClientCredentials.UserName.Password = "password";
```

### Certificate Authentication
```csharp
var binding = new WSHttpBinding(SecurityMode.Message);
binding.Security.Message.ClientCredentialType = MessageCredentialType.Certificate;

var client = new ProductServiceClient(binding, endpoint);
client.ClientCredentials.ClientCertificate.Certificate = GetClientCertificate();
```

### Custom Headers
```csharp
// Add custom SOAP headers
using (var scope = new OperationContextScope(client.InnerChannel))
{
    var header = MessageHeader.CreateHeader("CustomAuth", "http://api.example.com", 
        new CustomAuthHeader { Token = "your-token", ClientId = "your-client-id" });
    OperationContext.Current.OutgoingMessageHeaders.Add(header);
    
    var result = await client.GetAllProductsAsync();
}
```

## Performance Optimization

### Connection Pooling
```csharp
// Configure connection pooling
builder.Services.AddScoped<SoapProductService>(provider =>
{
    var client = CreateProductServiceClient();
    // Reuse client instance for multiple operations
    return new SoapProductService(client);
});
```

### Async Best Practices
```csharp
// Use async operations properly
public async Task<IEnumerable<Product>> GetAllProductsAsync()
{
    try
    {
        // Use ConfigureAwait(false) for library code
        var response = await _client.GetAllProductsAsync().ConfigureAwait(false);
        return response ?? new List<Product>();
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Failed to get products from SOAP service");
        return new List<Product>();
    }
}
```

### Timeout Configuration
```csharp
// Configure operation timeouts
var binding = new WSHttpBinding();
binding.OpenTimeout = TimeSpan.FromMinutes(1);
binding.CloseTimeout = TimeSpan.FromMinutes(1);
binding.SendTimeout = TimeSpan.FromMinutes(5);
binding.ReceiveTimeout = TimeSpan.FromMinutes(5);
```

## Integration Examples

### Enterprise ERP System
```csharp
// SAP or Oracle ERP integration
var binding = new WSHttpBinding(SecurityMode.Transport);
var endpoint = new EndpointAddress("https://erp.company.com/ProductService.svc");
var client = new ERPProductServiceClient(binding, endpoint);
```

### Legacy Mainframe Service
```csharp
// Mainframe SOAP wrapper integration
var binding = new BasicHttpBinding(BasicHttpSecurityMode.TransportCredentialOnly);
binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
var client = new MainframeServiceClient(binding, endpoint);
```

## Monitoring and Logging

### Message Logging
```xml
<!-- In app.config or web.config -->
<system.serviceModel>
  <diagnostics>
    <messageLogging logEntireMessage="true" 
                   logMalformedMessages="true" 
                   logMessagesAtServiceLevel="true" 
                   logMessagesAtTransportLevel="true" />
  </diagnostics>
</system.serviceModel>
```

### Performance Counters
```csharp
// Add performance monitoring
builder.Services.AddSingleton<IMetrics, Metrics>();

public class SoapProductService
{
    private readonly IMetrics _metrics;
    
    public async Task<Product> GetProductByIdAsync(int id)
    {
        using var timer = _metrics.Measure.Timer.Time("soap_product_get_duration");
        // Service call implementation
    }
}
```

## Troubleshooting

### Common Issues

1. **WSDL Import Errors**: Verify WSDL accessibility and format
2. **Authentication Failures**: Check credentials and security configuration
3. **Timeout Issues**: Adjust timeout values for slow services
4. **Serialization Errors**: Verify data contract compatibility
5. **Security Policy Mismatches**: Ensure client and service security policies match

### Debugging Tools

- **SoapUI**: Test SOAP services independently
- **Fiddler**: Monitor SOAP message traffic
- **WCF Service Configuration Editor**: Configure WCF settings
- **Visual Studio WCF Test Client**: Test WCF services

### Debug Configuration
```csharp
// In appsettings.json
{
  "Logging": {
    "LogLevel": {
      "System.ServiceModel": "Debug",
      "dtlapi.Data.Providers.Soap": "Debug"
    }
  }
}
```

This provider is ideal for integrating with legacy SOAP services, enterprise systems, and third-party services that only expose SOAP endpoints while maintaining a consistent data access pattern in your application.
