using Microsoft.EntityFrameworkCore;

namespace Auth.Context;

public class AuthDbContext(IConfiguration configuration) : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(configuration.GetConnectionString("Database"));
        optionsBuilder.UseOpenIddict();
    }
}
