using ApplicationDAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApplicationDAL.Context;

public class IndexerContext : DbContext
{
    public IndexerContext(DbContextOptions<IndexerContext> options) : base(options)
    {
        
    }

    public IndexerContext()
    {
        
    }

    public virtual DbSet<IndexedWord> IndexedWords { get; set; }
    public virtual DbSet<WordCountInPostId> WordCountInPostIds { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IndexedWord>()
            .HasIndex(iw => iw.Word)
            .IsUnique();
        modelBuilder.Entity<IndexedWord>()
            .HasMany(i => i.WordCountInPostId)
            .WithOne(wp => wp.IndexedWord);
    }
}