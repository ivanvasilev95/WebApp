using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebBazar.API.Services.Interfaces;
using WebBazar.API.DTOs.Message;
using WebBazar.API.Helpers;
using WebBazar.API.Extensions;

namespace WebBazar.API.Controllers
{
    public class MessagesController : ApiController
    {
        private readonly IMessageService _messageService;
        private readonly ICurrentUserService _currentUser;

        public MessagesController(IMessageService messageService, ICurrentUserService currentUser)
        {
            _messageService = messageService;
            _currentUser = currentUser;
        }

        [HttpGet("thread")]
        public async Task<IActionResult> MessageThread([FromQuery]int adId, [FromQuery]int recipientId)
        {
            var senderId = _currentUser.GetId();

            var messageThread = await _messageService.MessageThreadAsync(adId, senderId, recipientId);

            return Ok(messageThread);
        }

        [HttpGet]
        public async Task<IActionResult> UserMessages([FromQuery]MessageParams messageParams)
        {
            var userId = _currentUser.GetId();
            
            var model = await _messageService.UserMessagesAsync(messageParams, userId);

            this.HttpContext.Response.AddPagination(model.CurrentPage, model.PageSize, model.TotalCount);

            return Ok(model.Messages);
        }

        [HttpPost]
        public async Task<IActionResult> Create(MessageForCreationDTO messageForCreationDTO)
        {
            var userId = _currentUser.GetId();
            
            if (messageForCreationDTO.SenderId != userId)
            {
                return Unauthorized();
            }

            var result = await _messageService.CreateAsync(messageForCreationDTO);

            if (result.Failure)
            {
                return BadRequest(result.Error);
            }

            return Created(nameof(this.Create), result.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _currentUser.GetId();

            var result = await _messageService.DeleteAsync(id, userId);

            if (result.Failure)
            {
                return BadRequest(result.Error);
            }

            return Ok();
        }

        [HttpPut("{id}/read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var userId = _currentUser.GetId();

            var result = await _messageService.MarkAsReadAsync(id, userId);
            
            if (result.Failure)
            {
                return Unauthorized();
            }

            return Ok();
        }

        [HttpGet("unread/count")]
        public async Task<IActionResult> UnreadCount()
        {
            var userId = _currentUser.GetId();
            
            var count = await _messageService.UnreadMessagesCountAsync(userId);
            
            return Ok(count);
        }
    }
}