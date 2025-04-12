using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OrchesterApp.Domain.OrchesterMitgliedAggregate;
using OrchesterApp.Domain.TerminAggregate;
using OrchesterApp.Domain.UserAggregate;

namespace OrchesterApp.Infrastructure.Persistence
{
    public class OrchesterDbContext: IdentityDbContext<User>
    {
        public OrchesterDbContext(DbContextOptions<OrchesterDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrchesterDbContext).Assembly); 
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<OrchesterMitglied> OrchesterMitglieder { get; set; } = null!;
        public DbSet<Termin> Termin { get; set; } = null!;
    }
}
