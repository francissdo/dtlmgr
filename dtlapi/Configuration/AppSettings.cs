namespace dtlapi.Configuration
{
    public class DatabaseSettings
    {
        public string Provider { get; set; } = "Json"; // Json, SqlServer, PostgreSql
        public string ConnectionString { get; set; } = string.Empty;
        public string DataPath { get; set; } = "SampleData"; // For JSON provider
    }

    public class JwtSettings
    {
        public string Key { get; set; } = "your-secret-key-must-be-at-least-32-characters-long";
        public string Issuer { get; set; } = "dtlapi";
        public string Audience { get; set; } = "dtlapi-client";
        public int ExpirationHours { get; set; } = 24;
    }
}
