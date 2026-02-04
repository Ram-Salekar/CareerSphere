using Microsoft.EntityFrameworkCore;
using CareerSphere.Models.UserTableModel;
using CareerSphere.Models.PostTableModel;
using CareerSphere.Models.ConnectionTableModel;


namespace CareerSphere.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
         public DbSet<User> Users { get; set; }
         public DbSet<Post> Posts { get; set; }
         public DbSet<Connection> Connections { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasKey(u => u.id);

            modelBuilder.Entity<Post>()
                .HasKey(p => p.id);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Posts)
                .WithOne(p => p.user)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Connection>()
                .HasKey(c => new { c.followerId, c.followingId });

            modelBuilder.Entity<Connection>()
                .HasOne(c => c.follower)
                .WithMany(u => u.followings)
                .HasForeignKey(c => c.followerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Connection>()
                .HasOne(c => c.following)
                .WithMany(u => u.followers)
                .HasForeignKey(c => c.followingId)
                .OnDelete(DeleteBehavior.Restrict);

               
               
        }
    }
}
