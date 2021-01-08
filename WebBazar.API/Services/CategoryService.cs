using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebBazar.API.Data;
using WebBazar.API.DTOs.Category;
using WebBazar.API.Data.Models;
using WebBazar.API.Services.Interfaces;
using WebBazar.API.Infrastructure.Services;

namespace WebBazar.API.Services
{
    public class CategoryService : BaseService, ICategoryService
    {
        public CategoryService(DataContext data, IMapper mapper)
            : base(data, mapper) {}

        public async Task<IEnumerable<CategoryToReturnDTO>> AllAsync()
        { 
            return await this.mapper
                .ProjectTo<CategoryToReturnDTO>(this.data.Set<Category>().AsNoTracking())
                .ToListAsync();
        }

        public async Task<Result<int>> CreateAsync(CategoryForCreationDTO model)
        {
            var categoryAlreadyExists = await CategoryNameIsTakenAsync(model.Name);
            
            if (categoryAlreadyExists)
            {
                return "Категорията вече съществува.";
            }

            var category = this.mapper.Map<Category>(model);

            await this.data.AddAsync(category);
            await this.data.SaveChangesAsync();

            return category.Id;
        }

        public async Task<Result> UpdateAsync(int id, CategoryForCreationDTO model)
        {
            var categoryAlreadyExists = await CategoryNameIsTakenAsync(model.Name);

            if (categoryAlreadyExists)
            {
                return "Категорията вече съществува.";
            }

            var category = await this.data.Categories
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();
            
            this.mapper.Map(model, category);

            return true;
        }

        private async Task<bool> CategoryNameIsTakenAsync(string name) {
            return await this.data.Categories
                .AnyAsync(c => c.Name.ToLower() == name.ToLower());
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var category = await this.data.Categories
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

            this.data.Remove(category);
            await this.data.SaveChangesAsync();

            return true;
        }
    }
}