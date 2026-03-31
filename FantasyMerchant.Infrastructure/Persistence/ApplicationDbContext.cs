using Microsoft.EntityFrameworkCore;


namespace FantasyMerchant.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

  
   // public DbSet<City> Cities => Set<City>();
    //public DbSet<Road> Roads => Set<Road>();
}