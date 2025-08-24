namespace dtlapi.Data.Providers.RestApi
{
    public interface IExternalProductService
    {
        Task<IEnumerable<ExternalProduct>> GetProductsAsync();
        Task<ExternalProduct?> GetProductByIdAsync(string externalId);
        Task<ExternalProduct> CreateProductAsync(ExternalProduct product);
        Task<ExternalProduct?> UpdateProductAsync(string externalId, ExternalProduct product);
        Task<bool> DeleteProductAsync(string externalId);
    }

    public interface IExternalUserService
    {
        Task<IEnumerable<ExternalUser>> GetUsersAsync();
        Task<ExternalUser?> GetUserByIdAsync(string externalId);
        Task<ExternalUser> CreateUserAsync(ExternalUser user);
        Task<ExternalUser?> UpdateUserAsync(string externalId, ExternalUser user);
        Task<bool> DeleteUserAsync(string externalId);
    }

    // DTOs for external services
    public class ExternalProduct
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Category { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }

    public class ExternalUser
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
