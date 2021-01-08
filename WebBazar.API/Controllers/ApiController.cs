using Microsoft.AspNetCore.Mvc;
using WebBazar.API.Infrastructure.Filters;

namespace WebBazar.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ServiceFilter(typeof(LogUserActivity))]
    public abstract class ApiController : ControllerBase
    {
        protected const string Id = "{id}";
        protected const string PathSeparator = "/";
    }
}