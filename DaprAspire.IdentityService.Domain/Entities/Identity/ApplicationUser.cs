using Microsoft.AspNetCore.Identity;

namespace DaprAspire.IdentityService.Domain.Entities.Identity
{
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the refresh token of the user.
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// Gets or sets the time span of the refresh token of the user.
        /// </summary>
        public DateTime RefreshTokenTimeSpan { get; set; }

        /// <summary>
        /// Gets or sets the claims of the user.
        /// </summary>
        public List<ApplicationUserClaim> Claims { get; set; } = new();

        /// <summary>
        /// Gets or sets the logins of the user.
        /// </summary>
        public List<ApplicationUserLogin> Logins { get; set; } = new();

        /// <summary>
        /// Gets or sets the tokens of the user.
        /// </summary>
        public List<ApplicationUserToken> Tokens { get; set; } = new();

        /// <summary>
        /// Gets or sets the roles of the user.
        /// </summary>
        public List<ApplicationUserRole> UserRoles { get; set; } = new();
    }
}
