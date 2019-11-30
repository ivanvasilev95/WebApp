using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.API.Data;
using WebApp.API.DTOs;

namespace WebApp.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdsController : ControllerBase
    {
        private readonly IAdsRepository _repo;
        private readonly IMapper _mapper;
        public AdsController(IAdsRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAds()
        {
            var ads = await _repo.GetAds();
            var adsToReturn = _mapper.Map<IEnumerable<AdForListDTO>>(ads);

            return Ok(adsToReturn);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAd(int id)
        {
            var ad = await _repo.GetAd(id);
            var adToReturn = _mapper.Map<AdForDetailedDTO>(ad);
     
            return Ok(adToReturn);
        }

        [Authorize]
        [HttpGet]
        [Route("user")]
        public IActionResult GetUserAds(){
            int userId = int.Parse(this.User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value);
            var ads = _repo.GetUserAds(userId);
            var adsToReturn = _mapper.Map<IEnumerable<AdForListDTO>>(ads);

            return Ok(adsToReturn);
        }
    }
}