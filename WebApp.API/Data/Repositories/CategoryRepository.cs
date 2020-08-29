using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApp.API.Data.Interfaces;
using WebApp.API.Models;

namespace WebApp.API.Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _context;

        public CategoryRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<string> GetCategoryName(int categoryId)
        {
            var categoryName = await _context.Categories
                .Where(c => c.Id == categoryId)
                .Select(c => c.Name)
                .FirstOrDefaultAsync();
                
            return categoryName;
        }
    }
}