using Microsoft.AspNetCore.Identity;

using MongoDB.Bson.Serialization.Attributes;

namespace DaprAspire.IdentityService.Domain.Entities.Identity
{
    public class ApplicationUserToken : IdentityUserToken<string>
    {
        /// <summary>
        /// Gets or sets the user associated with this token.
        /// </summary>
        [BsonIgnore]
        public ApplicationUser User { get; set; }
    }
}
