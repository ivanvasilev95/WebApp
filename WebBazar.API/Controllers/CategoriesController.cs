using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.API.Services.Interfaces;
using WebApp.API.DTOs.Category;

namespace WebApp.API.Controllers
{
    [Authorize(Policy = "RequireAdminRole")]
    public class CategoriesController : ApiController
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> All()
        {
            var categories = await _categoryService.AllAsync();

            return Ok(categories);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryForCreationDTO model)
        {
            var result = await _categoryService.CreateAsync(model);
            if (result.Failure)
                return BadRequest(result.Error);

            return Created(nameof(this.Created), result.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CategoryForCreationDTO model)
        {
            var result = await _categoryService.UpdateAsync(id, model);
            if (result.Failure)
                return BadRequest(result.Error);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) 
        {
            var result = await _categoryService.DeleteAsync(id);
            if (result.Failure)
                return BadRequest(result.Error);

            return Ok();
        }
    }
}