using System.ServiceModel;

namespace dtlapi.Data.Providers.Soap
{
    public class SoapProductService : IExternalSoapProductService
    {
        private readonly IConfiguration _configuration;
        private readonly string _serviceUrl;

        public SoapProductService(IConfiguration configuration)
        {
            _configuration = configuration;
            _serviceUrl = configuration["ExternalServices:SoapProductService:Url"] ?? "http://example.com/ProductService.asmx";
        }

        public async Task<SoapProductResponse[]> GetProductsAsync()
        {
            try
            {
                using var client = CreateSoapClient();
                // In a real implementation, you would call the actual SOAP service
                // For demo purposes, returning mock data
                return await Task.FromResult(new[]
                {
                    new SoapProductResponse
                    {
                        Id = "SOAP001",
                        Name = "External SOAP Product 1",
                        Description = "Product from external SOAP service",
                        Price = 99.99m,
                        Stock = 10,
                        Category = "External",
                        CreatedDate = DateTime.UtcNow.AddDays(-30),
                        ModifiedDate = DateTime.UtcNow,
                        IsSuccess = true
                    },
                    new SoapProductResponse
                    {
                        Id = "SOAP002",
                        Name = "External SOAP Product 2",
                        Description = "Another product from external SOAP service",
                        Price = 149.99m,
                        Stock = 5,
                        Category = "External",
                        CreatedDate = DateTime.UtcNow.AddDays(-15),
                        ModifiedDate = DateTime.UtcNow,
                        IsSuccess = true
                    }
                });
            }
            catch (Exception ex)
            {
                throw new SoapServiceException($"Failed to retrieve products from SOAP service: {ex.Message}", ex);
            }
        }

        public async Task<SoapProductResponse> GetProductByIdAsync(string productId)
        {
            try
            {
                using var client = CreateSoapClient();
                // Mock implementation - in real scenario, call actual SOAP service
                if (productId == "SOAP001")
                {
                    return await Task.FromResult(new SoapProductResponse
                    {
                        Id = productId,
                        Name = "External SOAP Product 1",
                        Description = "Product from external SOAP service",
                        Price = 99.99m,
                        Stock = 10,
                        Category = "External",
                        CreatedDate = DateTime.UtcNow.AddDays(-30),
                        ModifiedDate = DateTime.UtcNow,
                        IsSuccess = true
                    });
                }

                return new SoapProductResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "Product not found"
                };
            }
            catch (Exception ex)
            {
                throw new SoapServiceException($"Failed to retrieve product {productId} from SOAP service: {ex.Message}", ex);
            }
        }

        public async Task<SoapProductResponse> CreateProductAsync(SoapProductRequest request)
        {
            try
            {
                using var client = CreateSoapClient();
                // Mock implementation
                return await Task.FromResult(new SoapProductResponse
                {
                    Id = $"SOAP{DateTime.UtcNow.Ticks}",
                    Name = request.Name,
                    Description = request.Description,
                    Price = request.Price,
                    Stock = request.Stock,
                    Category = request.Category,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    IsSuccess = true
                });
            }
            catch (Exception ex)
            {
                throw new SoapServiceException($"Failed to create product in SOAP service: {ex.Message}", ex);
            }
        }

        public async Task<SoapProductResponse> UpdateProductAsync(string productId, SoapProductRequest request)
        {
            try
            {
                using var client = CreateSoapClient();
                // Mock implementation
                return await Task.FromResult(new SoapProductResponse
                {
                    Id = productId,
                    Name = request.Name,
                    Description = request.Description,
                    Price = request.Price,
                    Stock = request.Stock,
                    Category = request.Category,
                    CreatedDate = DateTime.UtcNow.AddDays(-10),
                    ModifiedDate = DateTime.UtcNow,
                    IsSuccess = true
                });
            }
            catch (Exception ex)
            {
                throw new SoapServiceException($"Failed to update product {productId} in SOAP service: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteProductAsync(string productId)
        {
            try
            {
                using var client = CreateSoapClient();
                // Mock implementation
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                throw new SoapServiceException($"Failed to delete product {productId} from SOAP service: {ex.Message}", ex);
            }
        }

        private ChannelFactory<IExternalSoapProductService> CreateSoapClient()
        {
            var binding = new BasicHttpBinding();
            var endpoint = new EndpointAddress(_serviceUrl);
            return new ChannelFactory<IExternalSoapProductService>(binding, endpoint);
        }
    }

    public class SoapServiceException : Exception
    {
        public SoapServiceException(string message) : base(message) { }
        public SoapServiceException(string message, Exception innerException) : base(message, innerException) { }
    }
}
