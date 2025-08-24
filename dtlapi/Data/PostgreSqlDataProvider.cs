using System.Data;
using Npgsql;

namespace dtlapi.Data
{
    public class PostgreSqlDataProvider : IDataProvider
    {
        private readonly string _connectionString;

        public PostgreSqlDataProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection CreateConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }

        public string GetConnectionString()
        {
            return _connectionString;
        }
    }
}
