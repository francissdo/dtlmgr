using Microsoft.EntityFrameworkCore;
using DtlApi.Data;
using DtlApi.Models;
using System.Xml.Linq;

namespace DtlApi.Services;

public enum DatabaseProvider
{
    SqlServer,
    PostgreSQL,
    Xml
}

public static class DatabaseConfigurationService
{
    public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var databaseProvider = configuration.GetValue<string>("DatabaseProvider") ?? "SqlServer";
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        switch (databaseProvider.ToLower())
        {
            case "sqlserver":
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(connectionString));
                break;
            
            case "postgresql":
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseNpgsql(connectionString));
                break;
            
            case "xml":
                // For XML, we'll use In-Memory database and populate from XML
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("XmlDatabase"));
                services.AddScoped<IXmlDataService, XmlDataService>();
                break;
            
            default:
                throw new ArgumentException($"Unsupported database provider: {databaseProvider}");
        }
    }
}

public interface IXmlDataService
{
    Task LoadDataFromXmlAsync();
    Task SaveDataToXmlAsync();
}

public class XmlDataService : IXmlDataService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly string _xmlFilePath;

    public XmlDataService(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
        _xmlFilePath = _configuration.GetValue<string>("XmlDataPath") ?? "data.xml";
    }

    public async Task LoadDataFromXmlAsync()
    {
        if (!File.Exists(_xmlFilePath))
            return;

        try
        {
            var doc = XDocument.Load(_xmlFilePath);
            
            // Clear existing data
            _context.Products.RemoveRange(_context.Products);
            _context.Categories.RemoveRange(_context.Categories);
            await _context.SaveChangesAsync();

            // Load categories
            var categoriesElement = doc.Root?.Element("Categories");
            if (categoriesElement != null)
            {
                foreach (var categoryElement in categoriesElement.Elements("Category"))
                {
                    var category = new Category
                    {
                        Id = int.Parse(categoryElement.Attribute("Id")?.Value ?? "0"),
                        Name = categoryElement.Attribute("Name")?.Value ?? "",
                        Description = categoryElement.Attribute("Description")?.Value ?? "",
                        IsActive = bool.Parse(categoryElement.Attribute("IsActive")?.Value ?? "true"),
                        CreatedAt = DateTime.Parse(categoryElement.Attribute("CreatedAt")?.Value ?? DateTime.UtcNow.ToString())
                    };
                    _context.Categories.Add(category);
                }
            }

            // Load products
            var productsElement = doc.Root?.Element("Products");
            if (productsElement != null)
            {
                foreach (var productElement in productsElement.Elements("Product"))
                {
                    var product = new Product
                    {
                        Id = int.Parse(productElement.Attribute("Id")?.Value ?? "0"),
                        Name = productElement.Attribute("Name")?.Value ?? "",
                        Description = productElement.Attribute("Description")?.Value ?? "",
                        Price = decimal.Parse(productElement.Attribute("Price")?.Value ?? "0"),
                        CategoryId = int.Parse(productElement.Attribute("CategoryId")?.Value ?? "0"),
                        IsActive = bool.Parse(productElement.Attribute("IsActive")?.Value ?? "true"),
                        CreatedAt = DateTime.Parse(productElement.Attribute("CreatedAt")?.Value ?? DateTime.UtcNow.ToString()),
                        UpdatedAt = DateTime.TryParse(productElement.Attribute("UpdatedAt")?.Value, out var updatedAt) ? updatedAt : null
                    };
                    _context.Products.Add(product);
                }
            }

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // Log error (implement logging as needed)
            throw new Exception($"Error loading XML data: {ex.Message}", ex);
        }
    }

    public async Task SaveDataToXmlAsync()
    {
        try
        {
            var categories = await _context.Categories.ToListAsync();
            var products = await _context.Products.ToListAsync();

            var doc = new XDocument(
                new XElement("Data",
                    new XElement("Categories",
                        categories.Select(c => new XElement("Category",
                            new XAttribute("Id", c.Id),
                            new XAttribute("Name", c.Name),
                            new XAttribute("Description", c.Description),
                            new XAttribute("IsActive", c.IsActive),
                            new XAttribute("CreatedAt", c.CreatedAt.ToString("O"))
                        ))
                    ),
                    new XElement("Products",
                        products.Select(p => new XElement("Product",
                            new XAttribute("Id", p.Id),
                            new XAttribute("Name", p.Name),
                            new XAttribute("Description", p.Description),
                            new XAttribute("Price", p.Price),
                            new XAttribute("CategoryId", p.CategoryId),
                            new XAttribute("IsActive", p.IsActive),
                            new XAttribute("CreatedAt", p.CreatedAt.ToString("O")),
                            p.UpdatedAt.HasValue ? new XAttribute("UpdatedAt", p.UpdatedAt.Value.ToString("O")) : null
                        ))
                    )
                )
            );

            doc.Save(_xmlFilePath);
        }
        catch (Exception ex)
        {
            // Log error (implement logging as needed)
            throw new Exception($"Error saving XML data: {ex.Message}", ex);
        }
    }
}
