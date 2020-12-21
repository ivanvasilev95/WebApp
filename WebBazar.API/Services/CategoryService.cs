using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebBazar.API.Data;
using WebBazar.API.DTOs.Category;
using WebBazar.API.Helpers;
using WebBazar.API.Data.Models;
using WebBazar.API.Services.Interfaces;

namespace WebBazar.API.Services
{
    public class CategoryService : BaseService, ICategoryService
    {
        public CategoryService(DataContext context, IMapper mapper)
            : base(context, mapper) {}

        public async Task<Result<CategoryToReturnDTO>> CreateAsync(CategoryForCreationDTO model)
        {
            var categoryAlreadyExists = await CategoryNameIsTakenAsync(model.Name);

            if (categoryAlreadyExists)
            {
                return "Категорията вече съществува.";
            }

            var category = _mapper.Map<Category>(model);

            await _context.AddAsync(category);
            
            await _context.SaveChangesAsync();

            return  _mapper.Map<Category, CategoryToReturnDTO>(category);
        }

        public async Task<Result> UpdateAsync(int id, CategoryForCreationDTO model)
        {
            var categoryAlreadyExists = await CategoryNameIsTakenAsync(model.Name);

            if (categoryAlreadyExists)
            {
                return "Категорията вече съществува.";
            }

            var category = await _context.Categories
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();
            
            _mapper.Map(model, category);

            return true;
        }

        private async Task<bool> CategoryNameIsTakenAsync(string name) {
            return await _context.Categories
                .AnyAsync(c => c.Name.ToLower() == name.ToLower());
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var category = await _context.Categories
                .Include(c => c.Ads)
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();

            if (category == null)
            {
                return "Категорията не е намерена";
            }
            
            if (category.Ads.Count > 0) 
            {
                return "Има налични обяви по тази категория";
            }

            _context.Remove(category);

            await _context.SaveChangesAsync();

            return true;
        }
        
        public async Task<IEnumerable<CategoryToReturnDTO>> AllAsync()
        { 
            return await _mapper
                .ProjectTo<CategoryToReturnDTO>(_context.Set<Category>().AsNoTracking())
                .ToListAsync();
        }
    }
}