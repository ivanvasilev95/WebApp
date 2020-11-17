using System;

namespace WebApp.API.DTOs.Message
{
    public class MessageForCreationDTO
    {
        public MessageForCreationDTO()
        {
            MessageSent = DateTime.Now;
        }
        
        public int SenderId { get; set; }
        public int RecipientId { get; set; }
        public int AdId { get; set; }
        public DateTime MessageSent { get; set; }
        public string Content { get; set; }
    }
}