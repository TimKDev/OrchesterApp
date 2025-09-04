using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace OrchesterApp.Infrastructure.Persistence
{
    public class OrchesterDbContextFactory : IDesignTimeDbContextFactory<OrchesterDbContext>
    {
        public OrchesterDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../OrchesterApp.Api"))
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<OrchesterDbContext>();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new Exception(
                    "For migrations a test db connection string should be defined in the appsettings.Development.json");
            }

            optionsBuilder.UseNpgsql(connectionString);

            return new OrchesterDbContext(optionsBuilder.Options);
        }
    }
}