using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebBazar.API.Data.Models;
using WebBazar.API.Data.Models.Base;
using WebBazar.API.Services.Interfaces;

namespace WebBazar.API.Data
{
    public class DataContext : IdentityDbContext<User, Role, int,
        IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>,
        IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        private readonly ICurrentUserService _currentUser;

        public DataContext(DbContextOptions<DataContext> options, ICurrentUserService currentUser)
            : base(options)
        {
            _currentUser = currentUser;
        }
        
        public DbSet<Ad> Ads { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet <Message> Messages { get; set; }
        public DbSet <Like> Likes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder) 
        {
            base.OnModelCreating(builder);

            builder.Entity<UserRole>(userRole => {
                userRole.HasKey(ur => new {ur.UserId, ur.RoleId});

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.User)
                    .WithMany(u => u.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            builder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(u => u.MessagesSent)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
                .HasOne(m => m.Recipient)
                .WithMany(u => u.MessagesReceived)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Like>()
                .HasKey(l => new {l.UserId, l.AdId});
            
            builder.Entity<User>()
                .HasQueryFilter(u => !u.IsDeleted);

            builder.Entity<Category>()
                .HasQueryFilter(c => !c.IsDeleted);

            builder.Entity<Ad>()
                .HasQueryFilter(a => a.IsApproved && !a.IsDeleted);

            builder.Entity<Photo>()
                .HasQueryFilter(p => !p.IsDeleted);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            this.ApplyAuditInformation();

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            this.ApplyAuditInformation();

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void ApplyAuditInformation()
        {
            this.ChangeTracker
                .Entries()
                .ToList()
                .ForEach(entry => {
                    var userName = _currentUser.GetUserName();

                    if (entry.Entity is IDeletableEntity deletableEntity)
                    {
                        if (entry.State == EntityState.Deleted)
                        {
                            deletableEntity.DeletedOn = DateTime.Now;
                            deletableEntity.DeletedBy = userName;
                            deletableEntity.IsDeleted = true;

                            entry.State = EntityState.Modified;

                            return;
                        }
                    }
                    
                    if (entry.Entity is IEntity entity)
                    {
                        if (entry.State == EntityState.Added)
                        {
                            entity.CreatedOn = DateTime.UtcNow;
                            entity.CreatedBy = userName;
                        }
                        else if (entry.State == EntityState.Modified)
                        {
                            entity.ModifiedOn = DateTime.UtcNow;
                            entity.ModifiedBy = userName;
                        }
                    }
                });
        }
    }
}