using Microsoft.EntityFrameworkCore;
using WebApp.API.Models;

namespace WebApp.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Value> Values { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Ad> Ads { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet <Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder) {
            builder.Entity<Message>()
                .HasOne(u => u.Sender)
                .WithMany(m => m.MessagesSent)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
                .HasOne(u => u.Recipient)
                .WithMany(m => m.MessagesReceived)
                .OnDelete(DeleteBehavior.Restrict);
            /*
            builder.Entity<Message>()
                .HasOne(a => a.Ad)
                .WithMany(m => m.Messages)
                .OnDelete(DeleteBehavior.Restrict);
            */
        }
    }
}