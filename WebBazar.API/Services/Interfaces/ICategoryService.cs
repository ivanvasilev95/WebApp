using System.Collections.Generic;
using System.Threading.Tasks;
using WebBazar.API.DTOs.Category;
using WebBazar.API.Infrastructure.Services;

namespace WebBazar.API.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryToReturnDTO>> AllAsync();
        Task<Result<int>> CreateAsync(CategoryForCreationDTO model);
        Task<Result> UpdateAsync(int id, CategoryForCreationDTO model); // using the same dto for convenience
        Task<Result> DeleteAsync(int id);
    }
}