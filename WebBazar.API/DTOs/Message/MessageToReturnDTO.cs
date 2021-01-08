using System;

namespace WebBazar.API.DTOs.Message
{
    public class MessageToReturnDTO
    {
        public int Id { get; set; }
        public int AdId { get; set; }
        public string AdTitle { get; set; }
        public int SenderId { get; set; }
        public string SenderUsername { get; set; }
        public bool SenderDeleted { get; set; }
        public int RecipientId { get; set; }
        public string RecipientUsername { get; set; }
        // public bool RecipientDeleted { get; set; }
        public string Content { get; set; }
        public DateTime SentOn { get; set; }
        public bool IsRead { get; set; }
        // public DateTime? ReadOn { get; set; }
    }
}