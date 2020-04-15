using Microsoft.EntityFrameworkCore;
using UserManagementSystem.DAL.Entities;

namespace UserManagementSystem.DAL.DbContexts
{
    public class UserManagementSystemDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public UserManagementSystemDbContext(DbContextOptions<UserManagementSystemDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserGroup>()
                .HasKey(bc => new { bc.UserId, bc.GroupId });

            modelBuilder.Entity<UserGroup>()
                .HasOne(bc => bc.User)
                .WithMany(b => b.UserGroups)
                .HasForeignKey(bc => bc.UserId);

            modelBuilder.Entity<UserGroup>()
                .HasOne(bc => bc.Group)
                .WithMany(c => c.UserGroups)
                .HasForeignKey(bc => bc.GroupId);
        }
    }
}
