# REST API Data Provider

This provider implements HTTP REST API-based data access for products and users. It enables integration with external REST APIs and third-party services, providing a standardized way to consume remote data sources.

## Features

- **HTTP Client Integration**: Uses .NET's HttpClient for reliable HTTP communication
- **JSON Serialization**: Automatic JSON serialization/deserialization
- **Configurable Endpoints**: Flexible endpoint configuration for different services
- **Authentication Support**: Supports various authentication methods (API keys, Bearer tokens, Basic auth)
- **Error Handling**: Comprehensive error handling and retry logic
- **Async Operations**: Full async/await support for non-blocking operations
- **Timeout Configuration**: Configurable request timeouts
- **Custom Headers**: Support for custom HTTP headers

## Configuration

Configure the REST API provider in your `appsettings.json`:

```json
{
  "ExternalServices": {
    "RestApi": {
      "BaseUrl": "https://api.example.com",
      "Timeout": "00:00:30",
      "ApiKey": "your-api-key-here",
      "AuthType": "ApiKey",
      "HeaderName": "X-API-Key",
      "Endpoints": {
        "Products": "/api/v1/products",
        "Users": "/api/v1/users"
      }
    }
  }
}
```

## Authentication Types

### API Key Authentication
```json
{
  "RestApi": {
    "AuthType": "ApiKey",
    "ApiKey": "your-api-key",
    "HeaderName": "X-API-Key"
  }
}
```

### Bearer Token Authentication
```json
{
  "RestApi": {
    "AuthType": "Bearer",
    "BearerToken": "your-bearer-token"
  }
}
```

### Basic Authentication
```json
{
  "RestApi": {
    "AuthType": "Basic",
    "Username": "your-username",
    "Password": "your-password"
  }
}
```

## API Endpoints

The REST API provider expects the following endpoint structure:

### Product Endpoints
- `GET /api/v1/products` - Get all products
- `GET /api/v1/products/{id}` - Get product by ID
- `POST /api/v1/products` - Create new product
- `PUT /api/v1/products/{id}` - Update product
- `DELETE /api/v1/products/{id}` - Delete product
- `GET /api/v1/products/search?query={query}` - Search products

### User Endpoints
- `GET /api/v1/users` - Get all users
- `GET /api/v1/users/{id}` - Get user by ID
- `GET /api/v1/users/by-username/{username}` - Get user by username
- `GET /api/v1/users/by-email/{email}` - Get user by email
- `POST /api/v1/users` - Create new user
- `PUT /api/v1/users/{id}` - Update user
- `DELETE /api/v1/users/{id}` - Delete user

## Expected Data Formats

### Product JSON Format
```json
{
  "id": 1,
  "name": "Product Name",
  "description": "Product Description",
  "price": 29.99,
  "category": "Electronics",
  "stockQuantity": 100,
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z"
}
```

### User JSON Format
```json
{
  "id": 1,
  "username": "user@example.com",
  "email": "user@example.com",
  "passwordHash": "$2a$11$hashedPassword...",
  "firstName": "John",
  "lastName": "Doe",
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z"
}
```

## Usage Examples

### Basic Configuration
```csharp
// In Program.cs - automatically configured based on appsettings.json
builder.Services.AddHttpClient<RestApiProductService>();
builder.Services.AddHttpClient<RestApiUserService>();
```

### Manual Usage
```csharp
// Create HTTP client
var httpClient = new HttpClient();
httpClient.BaseAddress = new Uri("https://api.example.com");
httpClient.DefaultRequestHeaders.Add("X-API-Key", "your-api-key");

// Use services
var productService = new RestApiProductService(httpClient);
var userService = new RestApiUserService(httpClient);

// Perform operations
var products = await productService.GetAllProductsAsync();
var user = await userService.GetUserByUsernameAsync("user@example.com");
```

### Custom Configuration
```csharp
// Configure HTTP client with custom settings
builder.Services.AddHttpClient<RestApiProductService>(client =>
{
    client.BaseAddress = new Uri("https://api.example.com");
    client.Timeout = TimeSpan.FromSeconds(30);
    client.DefaultRequestHeaders.Add("X-API-Key", "your-api-key");
    client.DefaultRequestHeaders.Add("User-Agent", "MyApp/1.0");
});
```

## Error Handling

The REST API provider includes comprehensive error handling:

### HTTP Status Code Handling
- **200-299**: Success responses
- **400**: Bad Request - Invalid data
- **401**: Unauthorized - Authentication required
- **403**: Forbidden - Access denied
- **404**: Not Found - Resource doesn't exist
- **429**: Too Many Requests - Rate limiting
- **500-599**: Server errors

### Retry Logic
```csharp
// Implement retry policy with Polly
builder.Services.AddHttpClient<RestApiProductService>()
    .AddPolicyHandler(GetRetryPolicy());

static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => !msg.IsSuccessStatusCode)
        .WaitAndRetryAsync(3, retryAttempt => 
            TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
}
```

## Performance Considerations

### Connection Pooling
- Uses HttpClientFactory for efficient connection pooling
- Avoids socket exhaustion issues
- Automatic connection lifecycle management

### Caching
```csharp
// Implement response caching
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient<RestApiProductService>()
    .AddPolicyHandler(GetCachePolicy());
```

### Compression
```csharp
// Enable response compression
builder.Services.AddHttpClient<RestApiProductService>(client =>
{
    client.DefaultRequestHeaders.AcceptEncoding.Add(
        new StringWithQualityHeaderValue("gzip"));
    client.DefaultRequestHeaders.AcceptEncoding.Add(
        new StringWithQualityHeaderValue("deflate"));
});
```

## Security Best Practices

1. **API Key Management**: Store API keys securely in configuration
2. **HTTPS Only**: Always use HTTPS for production APIs
3. **Rate Limiting**: Implement client-side rate limiting
4. **Input Validation**: Validate all data before sending to API
5. **Error Logging**: Log errors without exposing sensitive information

## Integration Examples

### Third-party E-commerce API
```csharp
// Shopify REST API integration
builder.Services.AddHttpClient<RestApiProductService>(client =>
{
    client.BaseAddress = new Uri("https://your-shop.myshopify.com");
    client.DefaultRequestHeaders.Add("X-Shopify-Access-Token", "your-token");
});
```

### Custom Internal API
```csharp
// Internal microservice integration
builder.Services.AddHttpClient<RestApiUserService>(client =>
{
    client.BaseAddress = new Uri("https://internal-user-service.company.com");
    client.DefaultRequestHeaders.Authorization = 
        new AuthenticationHeaderValue("Bearer", "internal-token");
});
```

## Monitoring and Logging

### Request/Response Logging
```csharp
// Add logging handler
builder.Services.AddHttpClient<RestApiProductService>()
    .AddHttpMessageHandler<LoggingHandler>();

public class LoggingHandler : DelegatingHandler
{
    private readonly ILogger<LoggingHandler> _logger;

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Sending request: {Method} {Url}", 
            request.Method, request.RequestUri);
        
        var response = await base.SendAsync(request, cancellationToken);
        
        _logger.LogInformation("Received response: {StatusCode}", 
            response.StatusCode);
        
        return response;
    }
}
```

### Health Checks
```csharp
// Add health checks for external APIs
builder.Services.AddHealthChecks()
    .AddCheck<RestApiHealthCheck>("rest-api");
```

## Troubleshooting

### Common Issues

1. **Connection Timeouts**: Increase timeout values or implement retry logic
2. **Authentication Failures**: Verify API keys and authentication headers
3. **Rate Limiting**: Implement exponential backoff and respect rate limits
4. **SSL/TLS Issues**: Ensure proper certificate validation
5. **Serialization Errors**: Verify JSON format compatibility

### Debugging

Enable detailed HTTP logging:

```csharp
// In appsettings.json
{
  "Logging": {
    "LogLevel": {
      "System.Net.Http.HttpClient": "Debug",
      "dtlapi.Data.Providers.RestApi": "Debug"
    }
  }
}
```

This provider is ideal for integrating with external REST APIs, microservices, cloud services, and third-party data sources while maintaining a consistent data access pattern in your application.
