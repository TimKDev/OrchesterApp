using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TvJahnOrchesterApp.Application.Features.Authorization.Models;
using TvJahnOrchesterApp.Infrastructure.Persistence;

namespace TvJahnOrchesterApp.Infrastructure.Services
{
    public class InitDatabaseService
    {
        private OrchesterDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public InitDatabaseService(OrchesterDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
        }

        public async Task OnStartUp()
        {
            ApplyMigrationsToDB();
            await AddRolesToDB();
        }

        private void ApplyMigrationsToDB()
        {
            _context.Database.Migrate();
        }

        private async Task AddRolesToDB()
        {
            foreach (var roleName in RoleNames.AllRoles)
            {
                if (!(await _roleManager.RoleExistsAsync(roleName)))
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
    }
}
