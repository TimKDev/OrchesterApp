using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate;
using TvJahnOrchesterApp.Domain.UserAggregate;

namespace TvJahnOrchesterApp.Infrastructure.Persistence
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
    }
}
