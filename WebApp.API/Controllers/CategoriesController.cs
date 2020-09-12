using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.API.Data.Interfaces;
using WebApp.API.DTOs.Category;
using WebApp.API.Models;
using AutoMapper;

namespace WebApp.API.Controllers
{
    public class CategoriesController : ApiController
    {
        private readonly ICategoryRepository _categoryRepo;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryRepository categoryRepo, IMapper mapper)
        {
            _categoryRepo = categoryRepo;
            _mapper = mapper;
        }

        [HttpGet("{id}", Name = "GetCategory")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _categoryRepo.GetCategory(id);
            if (category == null)
                return NotFound("Категорията не е намерена");
     
            return Ok(_mapper.Map<Category, CategoryToReturnDTO>(category));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryRepo.GetCategories();
            
            return Ok(_mapper.Map<IEnumerable<Category>, IEnumerable<CategoryToReturnDTO>>(categories));
        }

        [HttpPost]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> AddCategory(CategoryForCreationDTO categoryForCreationDTO)
        {
            categoryForCreationDTO.Name = categoryForCreationDTO.Name.Trim();

            if (await _categoryRepo.CheckIfCategoryExists(categoryForCreationDTO.Name))
                return BadRequest("Категорията вече съществува.");
            
            var categoryToCreate = _mapper.Map<Category>(categoryForCreationDTO);
            _categoryRepo.Add(categoryToCreate);
            await _categoryRepo.SaveAll();

            var categoryToReturn = _mapper.Map<CategoryToReturnDTO>(categoryToCreate);

            return CreatedAtRoute("GetCategory", new {controller = "Categories", id = categoryToCreate.Id}, categoryToReturn);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult<Category>> RemoveCategory(int id) 
        {
            var categoryToDelete = await _categoryRepo.GetCategory(id);
            if (categoryToDelete == null)
                return NotFound("Категорията не е намерена");
            
            if (categoryToDelete.Ads.Count > 0) {
                return BadRequest("Има налични обяви по тази категория");
            }

            _categoryRepo.Delete(categoryToDelete);
            await _categoryRepo.SaveAll();

            return categoryToDelete;
        }
    }
}