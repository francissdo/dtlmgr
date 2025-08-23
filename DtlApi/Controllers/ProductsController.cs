using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DtlApi.Data.Interfaces;
using DtlApi.DTOs;
using DtlApi.Models;

namespace DtlApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public ProductsController(IProductRepository productRepository, ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
    {
        var products = await _productRepository.GetAllAsync();
        var productDtos = products.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            CategoryId = p.CategoryId,
            CategoryName = p.Category?.Name ?? "",
            IsActive = p.IsActive,
            CreatedAt = p.CreatedAt,
            UpdatedAt = p.UpdatedAt
        });

        return Ok(productDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProduct(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        var productDto = new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            CategoryId = product.CategoryId,
            CategoryName = product.Category?.Name ?? "",
            IsActive = product.IsActive,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt
        };

        return Ok(productDto);
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> CreateProduct(CreateProductDto createProductDto)
    {
        // Validate category exists
        if (!await _categoryRepository.ExistsAsync(createProductDto.CategoryId))
        {
            return BadRequest("Invalid category ID");
        }

        var product = new Product
        {
            Name = createProductDto.Name,
            Description = createProductDto.Description,
            Price = createProductDto.Price,
            CategoryId = createProductDto.CategoryId,
            IsActive = createProductDto.IsActive,
            CreatedAt = DateTime.UtcNow
        };

        var createdProduct = await _productRepository.CreateAsync(product);
        
        // Get the product with category for response
        var productWithCategory = await _productRepository.GetByIdAsync(createdProduct.Id);
        
        var productDto = new ProductDto
        {
            Id = productWithCategory!.Id,
            Name = productWithCategory.Name,
            Description = productWithCategory.Description,
            Price = productWithCategory.Price,
            CategoryId = productWithCategory.CategoryId,
            CategoryName = productWithCategory.Category?.Name ?? "",
            IsActive = productWithCategory.IsActive,
            CreatedAt = productWithCategory.CreatedAt,
            UpdatedAt = productWithCategory.UpdatedAt
        };

        return CreatedAtAction(nameof(GetProduct), new { id = productDto.Id }, productDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ProductDto>> UpdateProduct(int id, UpdateProductDto updateProductDto)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        // Validate category exists
        if (!await _categoryRepository.ExistsAsync(updateProductDto.CategoryId))
        {
            return BadRequest("Invalid category ID");
        }

        product.Name = updateProductDto.Name;
        product.Description = updateProductDto.Description;
        product.Price = updateProductDto.Price;
        product.CategoryId = updateProductDto.CategoryId;
        product.IsActive = updateProductDto.IsActive;
        product.UpdatedAt = DateTime.UtcNow;

        var updatedProduct = await _productRepository.UpdateAsync(product);
        
        // Get the product with category for response
        var productWithCategory = await _productRepository.GetByIdAsync(updatedProduct.Id);
        
        var productDto = new ProductDto
        {
            Id = productWithCategory!.Id,
            Name = productWithCategory.Name,
            Description = productWithCategory.Description,
            Price = productWithCategory.Price,
            CategoryId = productWithCategory.CategoryId,
            CategoryName = productWithCategory.Category?.Name ?? "",
            IsActive = productWithCategory.IsActive,
            CreatedAt = productWithCategory.CreatedAt,
            UpdatedAt = productWithCategory.UpdatedAt
        };

        return Ok(productDto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var deleted = await _productRepository.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpGet("category/{categoryId}")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByCategory(int categoryId)
    {
        var products = await _productRepository.GetByCategoryAsync(categoryId);
        var productDtos = products.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            CategoryId = p.CategoryId,
            CategoryName = p.Category?.Name ?? "",
            IsActive = p.IsActive,
            CreatedAt = p.CreatedAt,
            UpdatedAt = p.UpdatedAt
        });

        return Ok(productDtos);
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> SearchProducts([FromQuery] string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return BadRequest("Search name is required");
        }

        var products = await _productRepository.SearchByNameAsync(name);
        var productDtos = products.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            CategoryId = p.CategoryId,
            CategoryName = p.Category?.Name ?? "",
            IsActive = p.IsActive,
            CreatedAt = p.CreatedAt,
            UpdatedAt = p.UpdatedAt
        });

        return Ok(productDtos);
    }
}
