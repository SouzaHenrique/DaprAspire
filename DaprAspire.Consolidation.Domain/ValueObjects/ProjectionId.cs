namespace DaprAspire.Consolidation.Domain.ValueObjects
{
    /// <summary>
    /// ProjectionId is a value object that represents the unique identifier for a projection.
    /// </summary>
    /// <param name="Value"></param>
    public readonly record struct ProjectionId(string Value)
    {
        public static ProjectionId FromLedgerAndDate(string ledgerId, DateOnly date) =>
        new($"{ledgerId}:{date:yyyy-MM-dd}");

        public static ProjectionId From(string id) => new(id);

        public override string ToString() => Value;
    }
}
