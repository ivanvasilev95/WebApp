using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp.API.Data.Interfaces;
using WebApp.API.DTOs.Message;
using WebApp.API.Extensions;
using WebApp.API.Helpers;

namespace WebApp.API.Controllers
{
    public class MessagesController : ApiController
    {
        private readonly IMessageService _messages;

        public MessagesController(IMessageService messages)
        {
            _messages = messages;
        }

        [HttpGet("thread")]
        public async Task<IActionResult> GetMessageThread([FromQuery]int adId, [FromQuery]int recipientId)
        {
            var currentUserId = int.Parse(this.User.GetId());
            var messageThread = await _messages.MessageThreadAsync(adId, currentUserId, recipientId);

            return Ok(messageThread);
        }

        [HttpGet("{id}", Name = "GetMessage")]
        public async Task<IActionResult> GetMessage(int id)
        {
            var result = await _messages.ByIdAsync(id);
            if (result.Failure)
            {
                return NotFound();
            }

            return Ok(result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserMessages([FromQuery]MessageParams messageParams)
        {
            messageParams.UserId = int.Parse(this.User.GetId());
            
            var messages = await _messages.UserMessagesAsync(messageParams, this.Response);

            return Ok(messages);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(MessageForCreationDTO messageForCreationDTO)
        {
            if (messageForCreationDTO.SenderId != int.Parse(this.User.GetId()))
                return Unauthorized();

            var result = await _messages.CreateAsync(messageForCreationDTO);
            if (result.Failure)
            {
                return BadRequest(result.Error);
            }

            // test it
            return CreatedAtRoute("GetMessage", new {controller = "Messages", id = result.Data.Id}, result.Data);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var currentUserId = int.Parse(this.User.GetId());

            var result = await _messages.DeleteAsync(id, currentUserId);
            if (result.Failure)
            {
                return BadRequest(result.Error);
            }

            return NoContent();
        }

        [HttpPost("{id}/read")]
        public async Task<IActionResult> MarkMessageAsRead(int id)
        {
            var currentUserId = int.Parse(this.User.GetId());

            var result = await _messages.MarkAsReadAsync(id, currentUserId);
            if (result.Failure)
            {
                return Unauthorized();
            }

            return NoContent();
        }

        [HttpGet("unread/count")]
        public async Task<IActionResult> GetUnreadMessagesCount()
        {
            var currentUserId = int.Parse(this.User.GetId());
            var count = await _messages.UnreadMessagesCountAsync(currentUserId);
            
            return Ok(count);
        }
    }
}