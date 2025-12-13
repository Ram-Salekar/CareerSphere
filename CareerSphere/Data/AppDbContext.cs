using Microsoft.EntityFrameworkCore;
using CareerSphere.Models.UserTableModel;
using CareerSphere.Models.PostTableModel;


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

               
               
        }
    }
}
