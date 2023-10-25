using ApplicationDAL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ApplicationDAL.DesignTimeDbContextFactories;

public class ApplicationContextDesignFactory : IDesignTimeDbContextFactory<ApplicationContext>
{
    // retrieve connection string from args
    public ApplicationContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
        optionsBuilder.UseNpgsql("Host=35.226.61.207; Database=appDb; Username=postgres; Password=mgdI-Fot$4]+Fl:P; IncludeErrorDetail=true");
        return new ApplicationContext(optionsBuilder.Options);
    }
}