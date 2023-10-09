using ApplicationDAL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ApplicationDAL.ContextBuilder;

public class IndexerContextFactory : IDesignTimeDbContextFactory<IndexerContext>
{
    public IndexerContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<IndexerContext>();
        optionsBuilder.UseNpgsql("Host=35.226.61.207; Database=appDb; Username=postgres; Password=mgdI-Fot$4]+Fl:P; IncludeErrorDetail=true");

        return new IndexerContext(optionsBuilder.Options);
    }
}