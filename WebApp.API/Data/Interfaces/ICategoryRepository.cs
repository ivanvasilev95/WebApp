using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.API.Models;

namespace WebApp.API.Data.Interfaces
{
    public interface ICategoryRepository : IRepositoryBase
    {
        Task<string> GetCategoryName(int categoryId);
        Task<Category> GetCategory(int id);
        Task<IEnumerable<Category>> GetCategories();
        Task<bool> CheckIfCategoryExists(string categoryName);
    }
}