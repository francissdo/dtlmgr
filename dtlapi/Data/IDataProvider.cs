using System.Data;

namespace dtlapi.Data
{
    public interface IDataProvider
    {
        IDbConnection CreateConnection();
        string GetConnectionString();
    }
}
