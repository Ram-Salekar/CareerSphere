using Microsoft.EntityFrameworkCore;
using CareerSphere.Models.UserTableModel;
using CareerSphere.Models.PostTableModel;
using CareerSphere.Models.ConnectionTableModel;
using CareerSphere.Models.MessageTableModel;


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
         public DbSet<Conversation> Conversations { get; set; }
            public DbSet<MessageModel> Messages { get; set; }

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

            modelBuilder.Entity<Conversation>()
            .HasKey(c => c.conversationId);

            modelBuilder.Entity<Conversation>()
                .HasOne(c => c.User)
                .WithMany(u => u.Conversations)
                .HasForeignKey(c => c.userId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MessageModel>()
                .HasKey(m => m.messageId);

            modelBuilder.Entity<MessageModel>()
                .HasOne(m => m.conversation)
                .WithMany(c => c.messages)
                .HasForeignKey(m => m.conversationId)
                .OnDelete(DeleteBehavior.Cascade);
                

               
               
        }
    }
}
