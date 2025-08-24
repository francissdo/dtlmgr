using System.ServiceModel;
using System.Runtime.Serialization;

namespace dtlapi.Data.Providers.Soap
{
    [ServiceContract]
    public interface IExternalSoapProductService
    {
        [OperationContract]
        Task<SoapProductResponse[]> GetProductsAsync();

        [OperationContract]
        Task<SoapProductResponse> GetProductByIdAsync(string productId);

        [OperationContract]
        Task<SoapProductResponse> CreateProductAsync(SoapProductRequest request);

        [OperationContract]
        Task<SoapProductResponse> UpdateProductAsync(string productId, SoapProductRequest request);

        [OperationContract]
        Task<bool> DeleteProductAsync(string productId);
    }

    [ServiceContract]
    public interface IExternalSoapUserService
    {
        [OperationContract]
        Task<SoapUserResponse[]> GetUsersAsync();

        [OperationContract]
        Task<SoapUserResponse> GetUserByIdAsync(string userId);

        [OperationContract]
        Task<SoapUserResponse> CreateUserAsync(SoapUserRequest request);

        [OperationContract]
        Task<SoapUserResponse> UpdateUserAsync(string userId, SoapUserRequest request);

        [OperationContract]
        Task<bool> DeleteUserAsync(string userId);
    }

    // SOAP Data Contracts
    [DataContract]
    public class SoapProductRequest
    {
        [DataMember]
        public string Name { get; set; } = string.Empty;

        [DataMember]
        public string Description { get; set; } = string.Empty;

        [DataMember]
        public decimal Price { get; set; }

        [DataMember]
        public int Stock { get; set; }

        [DataMember]
        public string Category { get; set; } = string.Empty;
    }

    [DataContract]
    public class SoapProductResponse
    {
        [DataMember]
        public string Id { get; set; } = string.Empty;

        [DataMember]
        public string Name { get; set; } = string.Empty;

        [DataMember]
        public string Description { get; set; } = string.Empty;

        [DataMember]
        public decimal Price { get; set; }

        [DataMember]
        public int Stock { get; set; }

        [DataMember]
        public string Category { get; set; } = string.Empty;

        [DataMember]
        public DateTime CreatedDate { get; set; }

        [DataMember]
        public DateTime ModifiedDate { get; set; }

        [DataMember]
        public bool IsSuccess { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; } = string.Empty;
    }

    [DataContract]
    public class SoapUserRequest
    {
        [DataMember]
        public string UserName { get; set; } = string.Empty;

        [DataMember]
        public string Email { get; set; } = string.Empty;

        [DataMember]
        public string FirstName { get; set; } = string.Empty;

        [DataMember]
        public string LastName { get; set; } = string.Empty;

        [DataMember]
        public bool IsActive { get; set; } = true;
    }

    [DataContract]
    public class SoapUserResponse
    {
        [DataMember]
        public string Id { get; set; } = string.Empty;

        [DataMember]
        public string UserName { get; set; } = string.Empty;

        [DataMember]
        public string Email { get; set; } = string.Empty;

        [DataMember]
        public string FirstName { get; set; } = string.Empty;

        [DataMember]
        public string LastName { get; set; } = string.Empty;

        [DataMember]
        public DateTime CreatedDate { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public bool IsSuccess { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
