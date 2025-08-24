using dtlapi.Models;
using Dapper;

namespace dtlapi.Data.Providers.SqlServer
{
    public class SqlServerUserRepository : IUserRepository
    {
        private readonly IDataProvider _dataProvider;

        public SqlServerUserRepository(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            using var connection = _dataProvider.CreateConnection();
            const string sql = @"
                SELECT Id, Username, Email, PasswordHash, CreatedAt 
                FROM Users 
                ORDER BY Id";
            
            return await connection.QueryAsync<User>(sql);
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            using var connection = _dataProvider.CreateConnection();
            const string sql = @"
                SELECT Id, Username, Email, PasswordHash, CreatedAt 
                FROM Users 
                WHERE Id = @Id";
            
            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            using var connection = _dataProvider.CreateConnection();
            const string sql = @"
                SELECT Id, Username, Email, PasswordHash, CreatedAt 
                FROM Users 
                WHERE Username = @Username";
            
            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Username = username });
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            using var connection = _dataProvider.CreateConnection();
            const string sql = @"
                SELECT Id, Username, Email, PasswordHash, CreatedAt 
                FROM Users 
                WHERE Email = @Email";
            
            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });
        }

        public async Task<User> CreateAsync(User user)
        {
            using var connection = _dataProvider.CreateConnection();
            user.CreatedAt = DateTime.UtcNow;

            const string sql = @"
                INSERT INTO Users (Username, Email, PasswordHash, CreatedAt)
                VALUES (@Username, @Email, @PasswordHash, @CreatedAt);
                SELECT CAST(SCOPE_IDENTITY() as int);";
            
            var id = await connection.QuerySingleAsync<int>(sql, user);
            user.Id = id;
            return user;
        }

        public async Task<User?> UpdateAsync(int id, User user)
        {
            using var connection = _dataProvider.CreateConnection();

            const string sql = @"
                UPDATE Users 
                SET Username = @Username, Email = @Email, PasswordHash = @PasswordHash
                WHERE Id = @Id";
            
            var affectedRows = await connection.ExecuteAsync(sql, new
            {
                Id = id,
                user.Username,
                user.Email,
                user.PasswordHash
            });

            return affectedRows > 0 ? await GetByIdAsync(id) : null;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = _dataProvider.CreateConnection();
            const string sql = "DELETE FROM Users WHERE Id = @Id";
            var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });
            return affectedRows > 0;
        }
    }
}
