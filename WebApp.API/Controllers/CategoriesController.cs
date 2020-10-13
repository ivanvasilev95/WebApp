using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.API.Data.Interfaces;
using WebApp.API.DTOs.Category;
using WebApp.API.Models;

namespace WebApp.API.Controllers
{
    public class CategoriesController : ApiController
    {
        private readonly ICategoryService _categories;

        public CategoriesController(ICategoryService categories)
        {
            _categories = categories;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> All()
        {
            var categories = await _categories.AllAsync();

            return Ok(categories);
        }

        [HttpGet("{id}", Name = "GetCategory")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategory(int id)
        {
            var result = await _categories.ByIdAsync(id);
            if (result.Failure)
                return NotFound(result.Error);
     
            return Ok(result.Data);
        }

        [HttpPost]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> Create(CategoryForCreationDTO categoryForCreationDTO)
        {
            var result = await _categories.CreateAsync(categoryForCreationDTO);
            if (result.Failure)
                return BadRequest(result.Error);

            // test it
            return CreatedAtRoute("GetCategory", new {controller = "Categories", id = result.Data.Id}, result.Data);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult<Category>> Delete(int id) 
        {
            var result = await _categories.DeleteAsync(id);
            if (result.Failure)
                return BadRequest(result.Error);

            return Ok();
        }
    }
}