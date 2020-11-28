using Microsoft.AspNetCore.Mvc;
using WebBazar.API.Helpers;

namespace WebBazar.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ServiceFilter(typeof(LogUserActivity))]
    public abstract class ApiController : ControllerBase
    {
    }
}