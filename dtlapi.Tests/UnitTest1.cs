using dtlapi.Controllers;
using dtlapi.Data;
using dtlapi.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace dtlapi.Tests;

public class ProductsControllerTests
{
    private readonly Mock<IProductRepository> _mockRepository;
    private readonly ProductsController _controller;

    public ProductsControllerTests()
    {
        _mockRepository = new Mock<IProductRepository>();
        _controller = new ProductsController(_mockRepository.Object);
    }

    [Fact]
    public async Task GetProducts_ReturnsOkResult_WithListOfProducts()
    {
        // Arrange
        var products = new List<Product>
        {
            new Product { Id = 1, Name = "Test Product 1", Price = 10.0m },
            new Product { Id = 2, Name = "Test Product 2", Price = 20.0m }
        };
        _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(products);

        // Act
        var result = await _controller.GetProducts();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedProducts = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);
        Assert.Equal(2, returnedProducts.Count());
    }

    [Fact]
    public async Task GetProduct_WithValidId_ReturnsOkResult()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Test Product", Price = 10.0m };
        _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(product);

        // Act
        var result = await _controller.GetProduct(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedProduct = Assert.IsType<Product>(okResult.Value);
        Assert.Equal(1, returnedProduct.Id);
    }

    [Fact]
    public async Task GetProduct_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Product?)null);

        // Act
        var result = await _controller.GetProduct(1);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task CreateProduct_WithValidProduct_ReturnsCreatedAtAction()
    {
        // Arrange
        var product = new Product { Name = "New Product", Price = 15.0m, Stock = 10 };
        var createdProduct = new Product { Id = 1, Name = "New Product", Price = 15.0m, Stock = 10 };
        _mockRepository.Setup(repo => repo.CreateAsync(product)).ReturnsAsync(createdProduct);

        // Act
        var result = await _controller.CreateProduct(product);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnedProduct = Assert.IsType<Product>(createdResult.Value);
        Assert.Equal(1, returnedProduct.Id);
    }

    [Fact]
    public async Task DeleteProduct_WithValidId_ReturnsNoContent()
    {
        // Arrange
        _mockRepository.Setup(repo => repo.DeleteAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteProduct(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteProduct_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        _mockRepository.Setup(repo => repo.DeleteAsync(1)).ReturnsAsync(false);

        // Act
        var result = await _controller.DeleteProduct(1);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }
}