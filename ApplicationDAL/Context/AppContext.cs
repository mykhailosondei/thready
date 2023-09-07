using Microsoft.EntityFrameworkCore;

namespace ApplicationDAL.Context;

public class AppContext : DbContext
{
    public AppContext(DbContextOptions options) : base(options)
    {
        
    }
}