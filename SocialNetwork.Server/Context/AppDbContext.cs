using Microsoft.EntityFrameworkCore;
using SocialNetwork.Server.Domain.Entities;

namespace SocialNetwork.Server.Infrastructure.Persistence
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<UserScaffold> Users { get; set; }
        public DbSet<RefreshTokenScaffold> RefreshTokens { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
            .UserBuilder()
            .RefreshTokenBuilder();
        }
    }
}
