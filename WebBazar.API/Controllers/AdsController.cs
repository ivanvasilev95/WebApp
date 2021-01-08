using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebBazar.API.DTOs.Ad;
using WebBazar.API.Infrastructure.Extensions;
using WebBazar.API.Infrastructure.Services;
using WebBazar.API.Services.Interfaces;

namespace WebBazar.API.Controllers
{
    public class AdsController : ApiController
    {
        private readonly IAdService ads;
        private readonly ICurrentUserService currentUser;
        
        public AdsController(IAdService ads, ICurrentUserService currentUser)
        {
            this.ads = ads;
            this.currentUser = currentUser;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<AdForListDTO>> All([FromQuery]AdParams adParams)
        {
            var model = await this.ads.AllAsync(adParams);

            this.HttpContext.Response.AddPagination(model.CurrentPage, model.PageSize, model.TotalCount);

            return model.Ads;
        }

        [HttpGet(nameof(Mine))]
        public async Task<IEnumerable<AdForListDTO>> Mine()
        {
            var userId = this.currentUser.GetId();

            return await this.ads.MineAsync(userId);
        }

        [HttpGet(nameof(Liked))]
        public async Task<IEnumerable<AdForListDTO>> Liked()
        {
            var userId = this.currentUser.GetId();

            return await this.ads.LikedAsync(userId);
        }

        [HttpGet(Id)]
        [AllowAnonymous]
        public async Task<AdForDetailedDTO> Details(int id)
        {
            return await this.ads.DetailsAsync(id);
        }

        [HttpPost]
        public async Task<ActionResult> Create(AdForCreateDTO model)
        {
            var id = await this.ads.CreateAsync(model);

            return Created(nameof(this.Create), id);
        }

        [HttpPut(Id)]
        public async Task<ActionResult> Update(int id, AdForUpdateDTO model)
        {
            return await this.ads
                .UpdateAsync(id, model)
                .ToActionResult();
        }

        [HttpDelete(Id)]
        public async Task<ActionResult> Delete(int id) 
        {
            return await this.ads
                .DeleteAsync(id)
                .ToActionResult();
        }
    }
}