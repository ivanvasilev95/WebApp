using System;

namespace WebBazar.API.Data.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime SentOn { get; set; }
        public bool IsRead { get; set; }
        public DateTime? ReadOn { get; set; }
        public bool SenderDeleted { get; set; }
        public bool RecipientDeleted { get; set; }
        public int AdId { get; set; }
        public Ad Ad { get; set; }
        public int SenderId { get; set; }
        public User Sender { get; set; }
        public int RecipientId { get; set; }
        public User Recipient { get; set; }
    }
}