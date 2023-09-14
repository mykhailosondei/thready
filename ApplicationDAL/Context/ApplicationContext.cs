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
        
    }


    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Post> Posts { get; set; }
    public virtual DbSet<Comment> Comments { get; set; }
    public virtual DbSet<Image> Images { get; set; }
    public virtual DbSet<Like> Like { get; set; }
}