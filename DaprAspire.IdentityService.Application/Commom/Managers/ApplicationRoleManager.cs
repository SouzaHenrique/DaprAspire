using DaprAspire.IdentityService.Domain.Entities.Identity;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace DaprAspire.IdentityService.Application.Commom.Managers
{
    public class ApplicationRoleManager(IRoleStore<ApplicationRole> store,
                                        IEnumerable<IRoleValidator<ApplicationRole>> roleValidators,
                                        ILookupNormalizer keyNormalizer,
                                        IdentityErrorDescriber errors,
                                        ILogger<RoleManager<ApplicationRole>> logger)
        : RoleManager<ApplicationRole>(store, roleValidators, keyNormalizer, errors, logger)
    {
    }
}
