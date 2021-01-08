using System;

namespace WebBazar.API.DTOs.Message
{
    public class MessageForCreationDTO
    {
        public MessageForCreationDTO()
        {
            SentOn = DateTime.Now;
        }
        
        public int AdId { get; set; }
        public int SenderId { get; set; }
        public int RecipientId { get; set; }
        public string Content { get; set; }
        public DateTime SentOn { get; set; }
    }
}