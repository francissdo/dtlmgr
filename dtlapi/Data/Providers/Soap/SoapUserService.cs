using System.ServiceModel;

namespace dtlapi.Data.Providers.Soap
{
    public class SoapUserService : IExternalSoapUserService
    {
        private readonly IConfiguration _configuration;
        private readonly string _serviceUrl;

        public SoapUserService(IConfiguration configuration)
        {
            _configuration = configuration;
            _serviceUrl = configuration["ExternalServices:SoapUserService:Url"] ?? "http://example.com/UserService.asmx";
        }

        public async Task<SoapUserResponse[]> GetUsersAsync()
        {
            try
            {
                using var client = CreateSoapClient();
                // Mock implementation - in real scenario, call actual SOAP service
                return await Task.FromResult(new[]
                {
                    new SoapUserResponse
                    {
                        Id = "SOAPUSER001",
                        UserName = "external_user1",
                        Email = "user1@external.com",
                        FirstName = "John",
                        LastName = "External",
                        CreatedDate = DateTime.UtcNow.AddDays(-60),
                        IsActive = true,
                        IsSuccess = true
                    },
                    new SoapUserResponse
                    {
                        Id = "SOAPUSER002",
                        UserName = "external_user2",
                        Email = "user2@external.com",
                        FirstName = "Jane",
                        LastName = "External",
                        CreatedDate = DateTime.UtcNow.AddDays(-45),
                        IsActive = true,
                        IsSuccess = true
                    }
                });
            }
            catch (Exception ex)
            {
                throw new SoapServiceException($"Failed to retrieve users from SOAP service: {ex.Message}", ex);
            }
        }

        public async Task<SoapUserResponse> GetUserByIdAsync(string userId)
        {
            try
            {
                using var client = CreateSoapClient();
                // Mock implementation
                if (userId == "SOAPUSER001")
                {
                    return await Task.FromResult(new SoapUserResponse
                    {
                        Id = userId,
                        UserName = "external_user1",
                        Email = "user1@external.com",
                        FirstName = "John",
                        LastName = "External",
                        CreatedDate = DateTime.UtcNow.AddDays(-60),
                        IsActive = true,
                        IsSuccess = true
                    });
                }

                return new SoapUserResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "User not found"
                };
            }
            catch (Exception ex)
            {
                throw new SoapServiceException($"Failed to retrieve user {userId} from SOAP service: {ex.Message}", ex);
            }
        }

        public async Task<SoapUserResponse> CreateUserAsync(SoapUserRequest request)
        {
            try
            {
                using var client = CreateSoapClient();
                // Mock implementation
                return await Task.FromResult(new SoapUserResponse
                {
                    Id = $"SOAPUSER{DateTime.UtcNow.Ticks}",
                    UserName = request.UserName,
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    CreatedDate = DateTime.UtcNow,
                    IsActive = request.IsActive,
                    IsSuccess = true
                });
            }
            catch (Exception ex)
            {
                throw new SoapServiceException($"Failed to create user in SOAP service: {ex.Message}", ex);
            }
        }

        public async Task<SoapUserResponse> UpdateUserAsync(string userId, SoapUserRequest request)
        {
            try
            {
                using var client = CreateSoapClient();
                // Mock implementation
                return await Task.FromResult(new SoapUserResponse
                {
                    Id = userId,
                    UserName = request.UserName,
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    CreatedDate = DateTime.UtcNow.AddDays(-30),
                    IsActive = request.IsActive,
                    IsSuccess = true
                });
            }
            catch (Exception ex)
            {
                throw new SoapServiceException($"Failed to update user {userId} in SOAP service: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            try
            {
                using var client = CreateSoapClient();
                // Mock implementation
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                throw new SoapServiceException($"Failed to delete user {userId} from SOAP service: {ex.Message}", ex);
            }
        }

        private ChannelFactory<IExternalSoapUserService> CreateSoapClient()
        {
            var binding = new BasicHttpBinding();
            var endpoint = new EndpointAddress(_serviceUrl);
            return new ChannelFactory<IExternalSoapUserService>(binding, endpoint);
        }
    }
}
