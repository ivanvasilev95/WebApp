using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.API.Models;

namespace WebApp.API.Data.Interfaces
{
    public interface ICategoryRepository
    {
        Task<string> GetCategoryName(int categoryId);
        Task<IEnumerable<Category>> GetCategories();
    }
}