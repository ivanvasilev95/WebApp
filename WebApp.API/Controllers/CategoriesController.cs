using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.API.Services.Interfaces;
using WebApp.API.DTOs.Category;
using WebApp.API.Models;

namespace WebApp.API.Controllers
{
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
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> Create(CategoryForCreationDTO categoryForCreationDTO)
        {
            var result = await _categoryService.CreateAsync(categoryForCreationDTO);
            if (result.Failure)
                return BadRequest(result.Error);

            return Created(nameof(this.Created), result.Data);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult<Category>> Delete(int id) 
        {
            var result = await _categoryService.DeleteAsync(id);
            if (result.Failure)
                return BadRequest(result.Error);

            return Ok();
        }
    }
}