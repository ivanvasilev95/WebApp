using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApp.API.Data.Interfaces;
using WebApp.API.DTOs.Category;
using WebApp.API.Helpers;
using WebApp.API.Models;

namespace WebApp.API.Data.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public CategoryService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<CategoryToReturnDTO>> CreateAsync(CategoryForCreationDTO model)
        {
            var categoryAlreadyAdded = await _context
                .Categories
                .AnyAsync(c => c.Name == model.Name);

            if (categoryAlreadyAdded)
            {
                return "Категорията вече съществува.";
            }

            var category = _mapper.Map<Category>(model);

            await _context.AddAsync(category);
            await _context.SaveChangesAsync();

            return  _mapper.Map<Category, CategoryToReturnDTO>(category);
        }

        // implement update category also

        public async Task<Result> DeleteAsync(int id)
        {
            var category = await _context
                .Categories
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