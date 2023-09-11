using ApplicationDAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApplicationDAL.Context;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions options) : base(options)
    {
        
    }

    public ApplicationContext()
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasMany(e => e.Posts)
            .WithOne(e => e.Author)
            .HasForeignKey(e => e.UserId)
            .IsRequired();

        modelBuilder.Entity<Post>()
            .HasMany(e => e.Comments)
            .WithOne(e => e.Post)
            .HasForeignKey(e => e.PostId);
        
        modelBuilder.Entity<Comment>()
            .HasOne(e => e.ParentComment)
            .WithMany(e => e.Comments)
            .HasForeignKey(e => e.CommentId);

        modelBuilder.Entity<Post>()
            .HasMany(e => e.Reposters)
            .WithMany(e => e.Reposts);


        modelBuilder.Entity<User>()
            .HasMany(e => e.BookmarkedPosts);
    }


    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Post> Posts { get; set; }
    public virtual DbSet<Comment> Comments { get; set; }
    public virtual DbSet<Image> Images { get; set; }
    public virtual DbSet<Like> Like { get; set; }
}