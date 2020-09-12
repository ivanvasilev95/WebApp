using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApp.API.Data.Interfaces;
using WebApp.API.Models;

namespace WebApp.API.Data.Repositories
{
    public class CategoryRepository : RepositoryBase, ICategoryRepository
    {
        public CategoryRepository(DataContext context) : base(context) {}

        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetCategory(int id)
        {
            return await _context.Categories.Include(c => c.Ads).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<string> GetCategoryName(int categoryId)
        {
            var categoryName = await _context.Categories
                .Where(c => c.Id == categoryId)
                .Select(c => c.Name)
                .FirstOrDefaultAsync();
                
            return categoryName;
        }

        public async Task<bool> CheckIfCategoryExists(string categoryName) {
            return await _context.Categories.AnyAsync(c => c.Name == categoryName);
        }
    }
}