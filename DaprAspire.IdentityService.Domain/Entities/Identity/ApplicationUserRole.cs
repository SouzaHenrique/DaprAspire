using Microsoft.AspNetCore.Identity;

using MongoDB.Bson.Serialization.Attributes;

namespace DaprAspire.IdentityService.Domain.Entities.Identity
{
    public class ApplicationUserRole : IdentityUserRole<string>
    {
    }
}
