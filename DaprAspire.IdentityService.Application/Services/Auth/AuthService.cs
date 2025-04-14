using DaprAspire.IdentityService.Domain.Entities.Identity;
using DaprAspire.IdentityService.Infrastructure.Identity.Services;
using Microsoft.AspNetCore.Identity;

namespace DaprAspire.IdentityService.Application.Services.Auth
{
    public class AuthService(UserManager<ApplicationUser> userManager,
                            SignInManager<ApplicationUser> signInManager,
                            TokenGeneratorService tokenGeneratorService)
    {
        public async Task<(bool success, string? token, string? error)> LoginAsync(string username, string password)
        {
            var result = await signInManager.PasswordSignInAsync(username, password, isPersistent: false, lockoutOnFailure: false);

            if (!result.Succeeded)
                return (false, null, "Invalid credentials");

            var user = await userManager.FindByNameAsync(username);

            if (user == null)
                return (false, null, "User not found");

            var token = await tokenGeneratorService.GenerateAccessTokenAsync(user);

            return (true, token, null);
        }
    }
}
