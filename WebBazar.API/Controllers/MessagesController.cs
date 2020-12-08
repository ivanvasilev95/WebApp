using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebBazar.API.Services.Interfaces;
using WebBazar.API.DTOs.Message;
using WebBazar.API.Extensions;
using WebBazar.API.Helpers;

namespace WebBazar.API.Controllers
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
            var senderId = int.Parse(this.User.GetId());

            var messageThread = await _messageService.MessageThreadAsync(adId, senderId, recipientId);

            return Ok(messageThread);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserMessages([FromQuery]MessageParams messageParams)
        {
            var userId = int.Parse(this.User.GetId());
            
            var messages = await _messageService.UserMessagesAsync(messageParams, userId);

            return Ok(messages);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(MessageForCreationDTO messageForCreationDTO)
        {
            var userId = int.Parse(this.User.GetId());
            
            if (messageForCreationDTO.SenderId != userId)
            {
                return Unauthorized();
            }

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
            var userId = int.Parse(this.User.GetId());

            var result = await _messageService.DeleteAsync(id, userId);

            if (result.Failure)
            {
                return BadRequest(result.Error);
            }

            return Ok();
        }

        [HttpPost("{id}/read")]
        public async Task<IActionResult> MarkMessageAsRead(int id)
        {
            var userId = int.Parse(this.User.GetId());

            var result = await _messageService.MarkAsReadAsync(id, userId);
            
            if (result.Failure)
            {
                return Unauthorized();
            }

            return Ok();
        }

        [HttpGet("unread/count")]
        public async Task<IActionResult> GetUnreadMessagesCount()
        {
            var userId = int.Parse(this.User.GetId());
            
            var count = await _messageService.UnreadMessagesCountAsync(userId);
            
            return Ok(count);
        }
    }
}