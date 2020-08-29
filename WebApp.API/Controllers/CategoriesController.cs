using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.API.Data.Interfaces;

namespace WebApp.API.Controllers
{
    public class CategoriesController : ApiController
    {
        private readonly ICategoryRepository _categoryRepo;

        public CategoriesController(ICategoryRepository categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAdCategories()
        {
            var categories = await _categoryRepo.GetCategories();

            return Ok(categories);
        }
    }
}