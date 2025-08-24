using System.Text;
using System.Text.Json;

namespace dtlapi.Data.Providers.RestApi
{
    public class RestApiUserService : IExternalUserService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly JsonSerializerOptions _jsonOptions;

        public RestApiUserService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseUrl = configuration["ExternalServices:UserApi:BaseUrl"] ?? "https://api.example.com/users";
            
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            // Configure default headers
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            var apiKey = configuration["ExternalServices:UserApi:ApiKey"];
            if (!string.IsNullOrEmpty(apiKey))
            {
                _httpClient.DefaultRequestHeaders.Add("X-API-Key", apiKey);
            }
        }

        public async Task<IEnumerable<ExternalUser>> GetUsersAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(_baseUrl);
                response.EnsureSuccessStatusCode();
                
                var jsonContent = await response.Content.ReadAsStringAsync();
                var users = JsonSerializer.Deserialize<List<ExternalUser>>(jsonContent, _jsonOptions);
                
                return users ?? new List<ExternalUser>();
            }
            catch (HttpRequestException ex)
            {
                throw new ExternalServiceException($"Failed to retrieve users from external API: {ex.Message}", ex);
            }
        }

        public async Task<ExternalUser?> GetUserByIdAsync(string externalId)
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
                return JsonSerializer.Deserialize<ExternalUser>(jsonContent, _jsonOptions);
            }
            catch (HttpRequestException ex)
            {
                throw new ExternalServiceException($"Failed to retrieve user {externalId} from external API: {ex.Message}", ex);
            }
        }

        public async Task<ExternalUser> CreateUserAsync(ExternalUser user)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(user, _jsonOptions);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                
                var response = await _httpClient.PostAsync(_baseUrl, content);
                response.EnsureSuccessStatusCode();
                
                var responseContent = await response.Content.ReadAsStringAsync();
                var createdUser = JsonSerializer.Deserialize<ExternalUser>(responseContent, _jsonOptions);
                
                return createdUser ?? throw new ExternalServiceException("Failed to deserialize created user");
            }
            catch (HttpRequestException ex)
            {
                throw new ExternalServiceException($"Failed to create user in external API: {ex.Message}", ex);
            }
        }

        public async Task<ExternalUser?> UpdateUserAsync(string externalId, ExternalUser user)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(user, _jsonOptions);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                
                var response = await _httpClient.PutAsync($"{_baseUrl}/{externalId}", content);
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                
                response.EnsureSuccessStatusCode();
                
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ExternalUser>(responseContent, _jsonOptions);
            }
            catch (HttpRequestException ex)
            {
                throw new ExternalServiceException($"Failed to update user {externalId} in external API: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteUserAsync(string externalId)
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
                throw new ExternalServiceException($"Failed to delete user {externalId} from external API: {ex.Message}", ex);
            }
        }
    }
}
