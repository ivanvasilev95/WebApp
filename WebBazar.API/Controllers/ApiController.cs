using Microsoft.AspNetCore.Mvc;
using WebApp.API.Helpers;

namespace WebApp.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ServiceFilter(typeof(LogUserActivity))]
    public abstract class ApiController : ControllerBase
    {
    }
}