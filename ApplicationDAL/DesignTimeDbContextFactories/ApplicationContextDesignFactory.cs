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
        Console.WriteLine(args[1]);
        optionsBuilder.UseNpgsql(args[1]);
        return new ApplicationContext(optionsBuilder.Options);
    }
}