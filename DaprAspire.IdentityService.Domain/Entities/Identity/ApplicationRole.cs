
using Microsoft.AspNetCore.Identity;

namespace DaprAspire.IdentityService.Domain.Entities.Identity
{
    public class ApplicationRole : IdentityRole
    {
        public ICollection<ApplicationRoleClaim> RoleClaims { get; set; } = [];
    }
}
