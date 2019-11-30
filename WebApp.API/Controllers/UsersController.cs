using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp.API.Data;

namespace WebApp.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repo;
        public UsersController(IUserRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id){
            var user = await _repo.GetUser(id);
            return Ok(user);
        }
    }
}