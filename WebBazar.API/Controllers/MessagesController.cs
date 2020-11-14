using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp.API.Services.Interfaces;
using WebApp.API.DTOs.Message;
using WebApp.API.Extensions;
using WebApp.API.Helpers;

namespace WebApp.API.Controllers
{
    public class MessagesController : ApiController
    {
        private readonly IMessageService _messageService;

        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpGet("thread")]
        public async Task<IActionResult> GetMessageThread([FromQuery]int adId, [FromQuery]int recipientId)
        {
            var currentUserId = int.Parse(this.User.GetId());
            var messageThread = await _messageService.MessageThreadAsync(adId, currentUserId, recipientId);

            return Ok(messageThread);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserMessages([FromQuery]MessageParams messageParams)
        {
            messageParams.UserId = int.Parse(this.User.GetId());
            
            var messages = await _messageService.UserMessagesAsync(messageParams);

            return Ok(messages);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(MessageForCreationDTO messageForCreationDTO)
        {
            if (messageForCreationDTO.SenderId != int.Parse(this.User.GetId()))
                return Unauthorized();

            var result = await _messageService.CreateAsync(messageForCreationDTO);
            if (result.Failure)
            {
                return BadRequest(result.Error);
            }

            return Created(nameof(this.CreateMessage), result.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var currentUserId = int.Parse(this.User.GetId());

            var result = await _messageService.DeleteAsync(id, currentUserId);
            if (result.Failure)
            {
                return BadRequest(result.Error);
            }

            return Ok();
        }

        [HttpPost("{id}/read")]
        public async Task<IActionResult> MarkMessageAsRead(int id)
        {
            var currentUserId = int.Parse(this.User.GetId());

            var result = await _messageService.MarkAsReadAsync(id, currentUserId);
            if (result.Failure)
            {
                return Unauthorized();
            }

            return Ok();
        }

        [HttpGet("unread/count")]
        public async Task<IActionResult> GetUnreadMessagesCount()
        {
            var currentUserId = int.Parse(this.User.GetId());
            var count = await _messageService.UnreadMessagesCountAsync(currentUserId);
            
            return Ok(count);
        }
    }
}