using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebBazar.API.Services.Interfaces;
using WebBazar.API.DTOs.Message;
using System.Collections.Generic;
using WebBazar.API.Infrastructure.Services;
using WebBazar.API.Infrastructure.Extensions;

namespace WebBazar.API.Controllers
{
    public class MessagesController : ApiController
    {
        private readonly IMessageService messages;
        private readonly ICurrentUserService currentUser;

        public MessagesController(IMessageService messages, ICurrentUserService currentUser)
        {
            this.messages = messages;
            this.currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IEnumerable<MessageToReturnDTO>> Mine([FromQuery]MessageParams messageParams)
        {
            var userId = this.currentUser.GetId();
            
            var model = await this.messages.MineAsync( messageParams, userId);

            this.HttpContext.Response.AddPagination(model.CurrentPage, model.PageSize, model.TotalCount);

            return model.Messages;
        }

        [HttpGet(nameof(Thread))]
        public async Task<IEnumerable<MessageToReturnDTO>> Thread([FromQuery]int adId, [FromQuery]int recipientId)
        {
            var senderId = this.currentUser.GetId();
            
            return await this.messages.ThreadAsync(adId, senderId, recipientId);
        }

        [HttpGet(nameof(UnreadCount))]
        public async Task<int> UnreadCount()
        {
            var userId = this.currentUser.GetId();
            
            return await this.messages.UnreadCountAsync(userId);
        }

        [HttpPost]
        public async Task<ActionResult> Create(MessageForCreationDTO model)
        {
            var result = await this.messages.CreateAsync(model);

            if (result.Failure)
            {
                return BadRequest(result.Error);
            }

            return Created(nameof(this.Create), result.Data);
        }

        [HttpPut(nameof(MarkAsRead) + PathSeparator + Id)]
        public async Task<ActionResult> MarkAsRead(int id)
        {
            return await this.messages
                .MarkAsReadAsync(id)
                .ToActionResult();
        }

        [HttpDelete(Id)]
        public async Task<ActionResult> Delete(int id)
        {
            return await this.messages
                .DeleteAsync(id)
                .ToActionResult();
        }
    }
}