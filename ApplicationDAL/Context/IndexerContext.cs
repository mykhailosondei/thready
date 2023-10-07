using ApplicationDAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApplicationDAL.Context;

public class IndexerContext : DbContext
{
    public IndexerContext(DbContextOptions options) : base(options)
    {
        
    }

    public IndexerContext()
    {
        
    }

    public virtual DbSet<IndexedWord> IndexedWords { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IndexedWord>()
            .HasIndex(iw => iw.Word)
            .IsUnique();
    }
}