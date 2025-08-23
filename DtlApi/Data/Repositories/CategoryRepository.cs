using Microsoft.EntityFrameworkCore;
using DtlApi.Data.Interfaces;
using DtlApi.Models;

namespace DtlApi.Data.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Category>> GetActiveCategoriesAsync()
    {
        return await _dbSet.Where(c => c.IsActive).ToListAsync();
    }

    public async Task<Category?> GetCategoryWithProductsAsync(int id)
    {
        return await _dbSet.Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
}
