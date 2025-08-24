using System.Text;
using System.Text.Json;

namespace dtlapi.Data.Providers.RestApi
{
    public class RestApiProductService : IExternalProductService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly JsonSerializerOptions _jsonOptions;

        public RestApiProductService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseUrl = configuration["ExternalServices:ProductApi:BaseUrl"] ?? "https://api.example.com/products";
            
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            // Configure default headers
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            var apiKey = configuration["ExternalServices:ProductApi:ApiKey"];
            if (!string.IsNullOrEmpty(apiKey))
            {
                _httpClient.DefaultRequestHeaders.Add("X-API-Key", apiKey);
            }
        }

        public async Task<IEnumerable<ExternalProduct>> GetProductsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(_baseUrl);
                response.EnsureSuccessStatusCode();
                
                var jsonContent = await response.Content.ReadAsStringAsync();
                var products = JsonSerializer.Deserialize<List<ExternalProduct>>(jsonContent, _jsonOptions);
                
                return products ?? new List<ExternalProduct>();
            }
            catch (HttpRequestException ex)
            {
                // Log the exception (in real implementation)
                throw new ExternalServiceException($"Failed to retrieve products from external API: {ex.Message}", ex);
            }
        }

        public async Task<ExternalProduct?> GetProductByIdAsync(string externalId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/{externalId}");
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                
                response.EnsureSuccessStatusCode();
                
                var jsonContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ExternalProduct>(jsonContent, _jsonOptions);
            }
            catch (HttpRequestException ex)
            {
                throw new ExternalServiceException($"Failed to retrieve product {externalId} from external API: {ex.Message}", ex);
            }
        }

        public async Task<ExternalProduct> CreateProductAsync(ExternalProduct product)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(product, _jsonOptions);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                
                var response = await _httpClient.PostAsync(_baseUrl, content);
                response.EnsureSuccessStatusCode();
                
                var responseContent = await response.Content.ReadAsStringAsync();
                var createdProduct = JsonSerializer.Deserialize<ExternalProduct>(responseContent, _jsonOptions);
                
                return createdProduct ?? throw new ExternalServiceException("Failed to deserialize created product");
            }
            catch (HttpRequestException ex)
            {
                throw new ExternalServiceException($"Failed to create product in external API: {ex.Message}", ex);
            }
        }

        public async Task<ExternalProduct?> UpdateProductAsync(string externalId, ExternalProduct product)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(product, _jsonOptions);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                
                var response = await _httpClient.PutAsync($"{_baseUrl}/{externalId}", content);
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                
                response.EnsureSuccessStatusCode();
                
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ExternalProduct>(responseContent, _jsonOptions);
            }
            catch (HttpRequestException ex)
            {
                throw new ExternalServiceException($"Failed to update product {externalId} in external API: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteProductAsync(string externalId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_baseUrl}/{externalId}");
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return false;
                }
                
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (HttpRequestException ex)
            {
                throw new ExternalServiceException($"Failed to delete product {externalId} from external API: {ex.Message}", ex);
            }
        }
    }

    public class ExternalServiceException : Exception
    {
        public ExternalServiceException(string message) : base(message) { }
        public ExternalServiceException(string message, Exception innerException) : base(message, innerException) { }
    }
}
