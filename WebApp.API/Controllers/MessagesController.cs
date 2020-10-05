using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApp.API.Data.Interfaces;
using WebApp.API.DTOs.Message;
using WebApp.API.Extensions;
using WebApp.API.Helpers;
using WebApp.API.Models;

namespace WebApp.API.Controllers
{
    public class MessagesController : ApiController
    {
        private readonly IAdsRepository _adsRepo;
        private readonly IMessageRepository _messageRepo;
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;
        

        public MessagesController(
            IAdsRepository adsRepo, 
            IMessageRepository messageRepo,
            IUserRepository userRepo, 
            IMapper mapper)
        {
            _adsRepo = adsRepo;
            _messageRepo = messageRepo;
            _userRepo = userRepo;
            _mapper = mapper;
        }

        [HttpGet("thread")]
        public async Task<IActionResult> GetMessageThread([FromQuery]int adId, [FromQuery]int recipientId)
        {
            int userId = int.Parse(this.User.GetId());
            var messageFromRepo = await _messageRepo.GetMessageThread(userId, recipientId, adId);
            var messageThread = _mapper.Map<IEnumerable<MessageToReturnDTO>>(messageFromRepo);

            return Ok(messageThread);
        }

        [HttpGet("{id}", Name = "GetMessage")]
        public async Task<IActionResult> GetMessage(int id)
        {
            var messageFromRepo = await _messageRepo.GetMessage(id);
            if (messageFromRepo == null)
                return NotFound();

            return Ok(messageFromRepo);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserMessages([FromQuery]MessageParams messageParams)
        {
            messageParams.UserId = int.Parse(this.User.GetId());
            
            var messagesFromRepo = await _messageRepo.GetUserMessages(messageParams);
            var messages = _mapper.Map<IEnumerable<MessageToReturnDTO>>(messagesFromRepo);
            
            Response.AddPagination(messagesFromRepo.CurrentPage, messagesFromRepo.PageSize,
                messagesFromRepo.TotalCount, messagesFromRepo.TotalPages);
            
            return Ok(messages);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(MessageForCreationDTO messageForCreationDTO)
        {
            var sender = await _userRepo.GetUser(messageForCreationDTO.SenderId, false);
            
            if (sender.Id != int.Parse(this.User.GetId()))
                return Unauthorized();

            var ad = await _adsRepo.GetAd(messageForCreationDTO.AdId);

            if (ad == null)
                return BadRequest("Обявата не е намерена");

            var recipient = await _userRepo.GetUser(messageForCreationDTO.RecipientId, false);

            if (recipient == null)
                return BadRequest("Потребителят не е намерен");

            var message = _mapper.Map<Message>(messageForCreationDTO);

            await _messageRepo.Add(message);

            if (await _messageRepo.SaveAll()) {
                var messageToReturn = _mapper.Map<MessageToReturnDTO>(message);
                return CreatedAtRoute("GetMessage", new { controller = "Messages", id = message.Id }, messageToReturn);
            }

            throw new Exception("Грешка при запазване на съобщението");
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        { 
            int userId = int.Parse(this.User.GetId());

            var messageFromRepo = await _messageRepo.GetMessage(id);
            if (messageFromRepo == null) {
                return BadRequest("Съобщението не е намерено");
            }

            if (messageFromRepo.SenderId == userId)
                messageFromRepo.SenderDeleted = true;

            if (messageFromRepo.RecipientId == userId)
                messageFromRepo.RecipientDeleted = true;

            // if (messageFromRepo.SenderDeleted && messageFromRepo.RecipientDeleted)
            //     _messageRepo.Delete(messageFromRepo);
            
            if (await _messageRepo.SaveAll())
                return NoContent();

            return BadRequest("Грешка при изтриване на съобщението");
        }

        [HttpPost("{id}/read")]
        public async Task<IActionResult> MarkMessageAsRead(int id)
        {
            int userId = int.Parse(this.User.GetId());
            var message = await _messageRepo.GetMessage(id);

            if(message.RecipientId != userId)
                return Unauthorized();
            
            message.IsRead = true;
            message.DateRead = DateTime.Now;

            await _messageRepo.SaveAll();

            return NoContent();
        }

        [HttpGet("user/unread")]
        public async Task<IActionResult> GetUnreadMessagesCount()
        {
            int userId = int.Parse(this.User.GetId());
            int count = await _messageRepo.GetUnreadMessagesCount(userId);
            
            return Ok(count);
        }
    }
}