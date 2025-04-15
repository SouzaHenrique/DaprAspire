namespace DaprAspire.Consolidation.Domain.Projections
{
    /// <summary>
    /// Base class for all projections.
    /// </summary>
    public abstract class Projection
    {
        public string Id { get; set; } = default!;
        public DateTime LastUpdated { get; set; }
        public string LedgerId { get; set; } = default!;
    }
}
