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

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryRepo.GetCategories();
            
            return Ok(_mapper.Map<IEnumerable<Category>, IEnumerable<CategoryToReturnDTO>>(categories));
        }
    }
}