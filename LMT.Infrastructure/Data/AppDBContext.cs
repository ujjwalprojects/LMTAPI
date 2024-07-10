using LMT.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LMT.Infrastructure.Data
{
    public class AppDBContext(DbContextOptions<AppDBContext> options) : IdentityDbContext<IdentityUser>(options)
    {
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the RefreshToken entity if needed
            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(e => e.Token); // Assuming Token is unique
            });
        }
    }
}
