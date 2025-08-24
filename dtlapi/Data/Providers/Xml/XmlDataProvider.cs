using dtlapi.Models;
using System.Xml.Serialization;
using System.Xml;

namespace dtlapi.Data.Providers.Xml
{
    public class XmlDataProvider : IDataProvider
    {
        private readonly string _dataPath;

        public XmlDataProvider(string dataPath)
        {
            _dataPath = dataPath;
        }

        public System.Data.IDbConnection CreateConnection()
        {
            // XML provider doesn't use traditional connections
            throw new NotSupportedException("XML provider doesn't use database connections");
        }

        public string GetConnectionString()
        {
            return _dataPath;
        }

        public async Task<List<Product>> LoadProductsAsync()
        {
            var filePath = Path.Combine(_dataPath, "products.xml");
            if (!File.Exists(filePath))
            {
                return new List<Product>();
            }

            try
            {
                var serializer = new XmlSerializer(typeof(List<Product>));
                using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                var products = (List<Product>?)serializer.Deserialize(fileStream);
                return products ?? new List<Product>();
            }
            catch (Exception)
            {
                // If XML is corrupted or invalid, return empty list
                return new List<Product>();
            }
        }

        public async Task SaveProductsAsync(List<Product> products)
        {
            Directory.CreateDirectory(_dataPath);
            var filePath = Path.Combine(_dataPath, "products.xml");
            
            var serializer = new XmlSerializer(typeof(List<Product>));
            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
                NewLineChars = Environment.NewLine
            };

            using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            using var xmlWriter = XmlWriter.Create(fileStream, settings);
            serializer.Serialize(xmlWriter, products);
        }

        public async Task<List<User>> LoadUsersAsync()
        {
            var filePath = Path.Combine(_dataPath, "users.xml");
            if (!File.Exists(filePath))
            {
                return new List<User>();
            }

            try
            {
                var serializer = new XmlSerializer(typeof(List<User>));
                using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                var users = (List<User>?)serializer.Deserialize(fileStream);
                return users ?? new List<User>();
            }
            catch (Exception)
            {
                // If XML is corrupted or invalid, return empty list
                return new List<User>();
            }
        }

        public async Task SaveUsersAsync(List<User> users)
        {
            Directory.CreateDirectory(_dataPath);
            var filePath = Path.Combine(_dataPath, "users.xml");
            
            var serializer = new XmlSerializer(typeof(List<User>));
            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
                NewLineChars = Environment.NewLine
            };

            using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            using var xmlWriter = XmlWriter.Create(fileStream, settings);
            serializer.Serialize(xmlWriter, users);
        }
    }
}
