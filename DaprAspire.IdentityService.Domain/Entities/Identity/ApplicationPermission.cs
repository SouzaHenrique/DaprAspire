namespace DaprAspire.IdentityService.Domain.Entities.Identity
{
    /// <summary>
    /// Represents a permission within the system.
    /// </summary>
    public class ApplicationPermission
    {
        /// <summary>
        /// Unique identifier of the permission.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Name of the permission, e.g., "Entries.Create"
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Group or domain this permission belongs to, e.g., "Entries" or "Consolidation"
        /// </summary>
        public string Group { get; set; } = null!;

        /// <summary>
        /// Optional parent permission identifier for hierarchy
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// Parent permission reference (not required in MongoDB schema)
        /// </summary>
        public Guid? Parent { get; set; }

        /// <summary>
        /// List of child permissions
        /// </summary>
        public IList<ApplicationPermission> Permissions { get; set; } = new List<ApplicationPermission>();
    }
}
