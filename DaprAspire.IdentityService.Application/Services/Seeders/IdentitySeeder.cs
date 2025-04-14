using DaprAspire.IdentityService.Domain.Entities.Identity;

using Microsoft.AspNetCore.Identity;

namespace DaprAspire.IdentityService.Application.Services.Seeders
{
    public class IdentitySeeder
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentitySeeder(
            RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task SeedAsync(CancellationToken cancellationToken)
        {
            if(cancellationToken.IsCancellationRequested) return;

            var roles = new[] { "admin", "entries", "consolidation" };

            foreach (var roleName in roles)
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.CreateAsync(new ApplicationRole { Name = roleName });
                }
            }

            await SeedUserAsync("admin", "Admin@123", roles, cancellationToken); // Admin gets all roles
            await SeedUserAsync("user", "User@123", ["consolidation"], cancellationToken); // Basic user gets consolidation only
        }

        private async Task SeedUserAsync(string username, string password, string[] roles, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                await Task.CompletedTask;

            if (await _userManager.FindByNameAsync(username) != null)
                return;

            var user = new ApplicationUser
            {
                UserName = username,
                Email = $"{username}@dapraspire.local",
                EmailConfirmed = true,
            };

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await _userManager.AddToRolesAsync(user, roles);
            }
            else
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to create user '{username}': {errors}");
            }
        }
    }
}
