using dtlapi.Models;
using Newtonsoft.Json;

namespace dtlapi.Data.Providers.Json
{
    public class JsonDataProvider : IDataProvider
    {
        private readonly string _dataPath;

        public JsonDataProvider(string dataPath)
        {
            _dataPath = dataPath;
        }

        public System.Data.IDbConnection CreateConnection()
        {
            // JSON provider doesn't use traditional connections
            throw new NotSupportedException("JSON provider doesn't use database connections");
        }

        public string GetConnectionString()
        {
            return _dataPath;
        }

        public async Task<List<Product>> LoadProductsAsync()
        {
            var filePath = Path.Combine(_dataPath, "products.json");
            if (!File.Exists(filePath))
            {
                return new List<Product>();
            }

            var json = await File.ReadAllTextAsync(filePath);
            return JsonConvert.DeserializeObject<List<Product>>(json) ?? new List<Product>();
        }

        public async Task SaveProductsAsync(List<Product> products)
        {
            Directory.CreateDirectory(_dataPath);
            var filePath = Path.Combine(_dataPath, "products.json");
            var json = JsonConvert.SerializeObject(products, Formatting.Indented);
            await File.WriteAllTextAsync(filePath, json);
        }
    }
}
