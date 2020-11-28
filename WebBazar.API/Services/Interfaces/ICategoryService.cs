using System.Collections.Generic;
using System.Threading.Tasks;
using WebBazar.API.DTOs.Category;
using WebBazar.API.Helpers;

namespace WebBazar.API.Services.Interfaces
{
    public interface ICategoryService
    {
         Task<Result<CategoryToReturnDTO>> CreateAsync(CategoryForCreationDTO model);
         Task<Result> UpdateAsync(int id, CategoryForCreationDTO model); // using the same dto for convenience
         Task<Result> DeleteAsync(int id);
         Task<IEnumerable<CategoryToReturnDTO>> AllAsync();
    }
}