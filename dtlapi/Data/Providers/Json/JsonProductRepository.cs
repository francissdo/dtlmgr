using dtlapi.Models;

namespace dtlapi.Data.Providers.Json
{
    public class JsonProductRepository : IProductRepository
    {
        private readonly JsonDataProvider _dataProvider;

        public JsonProductRepository(JsonDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var products = await _dataProvider.LoadProductsAsync();
            return products.OrderBy(p => p.Id);
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            var products = await _dataProvider.LoadProductsAsync();
            return products.FirstOrDefault(p => p.Id == id);
        }

        public async Task<Product> CreateAsync(Product product)
        {
            var products = await _dataProvider.LoadProductsAsync();
            
            product.Id = products.Any() ? products.Max(p => p.Id) + 1 : 1;
            product.CreatedAt = DateTime.UtcNow;
            product.UpdatedAt = DateTime.UtcNow;
            
            products.Add(product);
            await _dataProvider.SaveProductsAsync(products);
            
            return product;
        }

        public async Task<Product?> UpdateAsync(int id, Product product)
        {
            var products = await _dataProvider.LoadProductsAsync();
            var existingProduct = products.FirstOrDefault(p => p.Id == id);
            
            if (existingProduct == null)
                return null;

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.Stock = product.Stock;
            existingProduct.UpdatedAt = DateTime.UtcNow;
            
            await _dataProvider.SaveProductsAsync(products);
            return existingProduct;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var products = await _dataProvider.LoadProductsAsync();
            var product = products.FirstOrDefault(p => p.Id == id);
            
            if (product == null)
                return false;

            products.Remove(product);
            await _dataProvider.SaveProductsAsync(products);
            return true;
        }
    }
}
