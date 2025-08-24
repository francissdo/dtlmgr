using dtlapi.Models;
using Dapper;

namespace dtlapi.Data.Providers.PostgreSql
{
    public class PostgreSqlProductRepository : IProductRepository
    {
        private readonly IDataProvider _dataProvider;

        public PostgreSqlProductRepository(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            using var connection = _dataProvider.CreateConnection();
            const string sql = @"
                SELECT Id, Name, Description, Price, Stock, CreatedAt, UpdatedAt 
                FROM Products 
                ORDER BY Id";
            
            return await connection.QueryAsync<Product>(sql);
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            using var connection = _dataProvider.CreateConnection();
            const string sql = @"
                SELECT Id, Name, Description, Price, Stock, CreatedAt, UpdatedAt 
                FROM Products 
                WHERE Id = @Id";
            
            return await connection.QueryFirstOrDefaultAsync<Product>(sql, new { Id = id });
        }

        public async Task<Product> CreateAsync(Product product)
        {
            using var connection = _dataProvider.CreateConnection();
            product.CreatedAt = DateTime.UtcNow;
            product.UpdatedAt = DateTime.UtcNow;

            const string sql = @"
                INSERT INTO Products (Name, Description, Price, Stock, CreatedAt, UpdatedAt)
                VALUES (@Name, @Description, @Price, @Stock, @CreatedAt, @UpdatedAt)
                RETURNING Id;";
            
            var id = await connection.QuerySingleAsync<int>(sql, product);
            product.Id = id;
            return product;
        }

        public async Task<Product?> UpdateAsync(int id, Product product)
        {
            using var connection = _dataProvider.CreateConnection();
            product.UpdatedAt = DateTime.UtcNow;

            const string sql = @"
                UPDATE Products 
                SET Name = @Name, Description = @Description, Price = @Price, 
                    Stock = @Stock, UpdatedAt = @UpdatedAt
                WHERE Id = @Id";
            
            var affectedRows = await connection.ExecuteAsync(sql, new
            {
                Id = id,
                product.Name,
                product.Description,
                product.Price,
                product.Stock,
                product.UpdatedAt
            });

            return affectedRows > 0 ? await GetByIdAsync(id) : null;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = _dataProvider.CreateConnection();
            const string sql = "DELETE FROM Products WHERE Id = @Id";
            var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });
            return affectedRows > 0;
        }
    }
}
