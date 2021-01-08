using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebBazar.API.Services.Interfaces;
using WebBazar.API.DTOs.Category;
using System.Collections.Generic;
using WebBazar.API.Infrastructure.Extensions;

namespace WebBazar.API.Controllers
{
    [Authorize(Policy = "RequireAdminOrModeratorRole")]
    public class CategoriesController : ApiController
    {
        private readonly ICategoryService categories;

        public CategoriesController(ICategoryService categories)
        {
            this.categories = categories;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<CategoryToReturnDTO>> All()
        {
            return await this.categories.AllAsync();
        }

        [HttpPost]
        public async Task<ActionResult> Create(CategoryForCreationDTO model)
        {
            var result = await this.categories.CreateAsync(model);

            if (result.Failure)
            {
                return BadRequest(result.Error);
            }

            return Created(nameof(this.Create), result.Data);
        }

        [HttpPut(Id)]
        public async Task<ActionResult> Update(int id, CategoryForCreationDTO model)
        {
            return await this.categories
                .UpdateAsync(id, model)
                .ToActionResult();
        }

        [HttpDelete(Id)]
        public async Task<ActionResult> Delete(int id) 
        {
            return await this.categories
                .DeleteAsync(id)
                .ToActionResult();
        }
    }
}