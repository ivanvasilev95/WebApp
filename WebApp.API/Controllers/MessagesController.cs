using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.API.Data;
using WebApp.API.DTOs;
using WebApp.API.Helpers;
using WebApp.API.Models;

namespace WebApp.API.Controllers
{
    [Authorize]
    [ServiceFilter(typeof(LogUserActivity))]
    [ApiController]
    [Route("[controller]")]
    //[Route("user/{userId}/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;
        private readonly IAdsRepository _adsRepo;

        public MessagesController(IAdsRepository adsRepo, IUserRepository userRepo, IMapper mapper)
        {
            _adsRepo = adsRepo;
            _userRepo = userRepo;
            _mapper = mapper;
        }

        [HttpGet("thread/{adId}/{recipientId}")]
        public async Task<IActionResult> GetMessageThread(/* int userId */ int adId, int recipientId)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var messageFromRepo = await _userRepo.GetMessageThread(userId, recipientId, adId);
            var messageThread = _mapper.Map<IEnumerable<MessageToReturnDTO>>(messageFromRepo);

            return Ok(messageThread);
        }

        [HttpGet("{id}", Name = "GetMessage")]
        public async Task<IActionResult> GetMessage(/*int userId,*/ int id)
        {
            //if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            //return Unauthorized();

            var messageFromRepo = await _userRepo.GetMessage(id);
            if (messageFromRepo == null)
                return NotFound();

            return Ok(messageFromRepo);
        }

        [HttpGet]
        public IActionResult GetMessages(/*int userId,*/
            [FromQuery]MessageParams messageParams) // async Task<IActionResult>
        {
            //if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            //return Unauthorized();
            messageParams.UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var messagesFromRepo = _userRepo.GetMessagesForUser(messageParams);
            var messages = _mapper.Map<IEnumerable<MessageToReturnDTO>>(messagesFromRepo);

            return Ok(messages);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(/*int userId,*/ /*int adId,*/ MessageForCreationDTO messageForCreationDTO)
        {
            var sender = await _userRepo.GetUser(messageForCreationDTO.SenderId);
            
            //if (messageForCreationDTO.SenderId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            if (sender.Id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            /*
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            messageForCreationDTO.SenderId = userId;
            */

            //var ad = await _adsRepo.GetAd(adId);
            var ad = await _adsRepo.GetAd(messageForCreationDTO.AdId);

            if (ad == null)
                return BadRequest("Could not find ad");

            var recipient = await _userRepo.GetUser(messageForCreationDTO.RecipientId);

            if (recipient == null)
                return BadRequest("Could not find user");

            var message = _mapper.Map<Message>(messageForCreationDTO);

            _adsRepo.Add(message);

            if (await _userRepo.SaveAll()) {
                //return NoContent();
                var messageToReturn = _mapper.Map<MessageToReturnDTO>(message);
                return CreatedAtRoute("GetMessage", new { controller = "Messages", id = message.Id }, messageToReturn);
            }

            throw new Exception("Creating the message failed on save");
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> DeleteMessage(int id/*, int userId*/)
        {
            //if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                //return Unauthorized();  
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var messageFromRepo = await _userRepo.GetMessage(id);

            if (messageFromRepo.SenderId == userId)
                messageFromRepo.SenderDeleted = true;

            if (messageFromRepo.RecipientId == userId)
                messageFromRepo.RecipientDeleted = true;

            if (messageFromRepo.SenderDeleted && messageFromRepo.RecipientDeleted)
                _adsRepo.Delete(messageFromRepo);
            
            if (await _userRepo.SaveAll())
                return NoContent();

            throw new Exception("Error deleting the message");
        }

        [HttpPost("{id}/read")]
        public async Task<IActionResult> MarkMessageAsRead(int id)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var message = await _userRepo.GetMessage(id);

            if(message.RecipientId != userId)
                return Unauthorized();
            
            message.IsRead = true;
            message.DateRead = DateTime.Now;

            await _userRepo.SaveAll();

            return NoContent();
        }
    }
}