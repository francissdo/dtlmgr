using System.Data;
using Microsoft.Data.SqlClient;

namespace dtlapi.Data.Providers.SqlServer
{
    public class SqlServerDataProvider : IDataProvider
    {
        private readonly string _connectionString;

        public SqlServerDataProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public string GetConnectionString()
        {
            return _connectionString;
        }
    }
}
