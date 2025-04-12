using EventFlow;
using EventFlow.Core;

namespace DaprAspire.Entries.Domain.Aggregates
{
    /// <summary>
    /// Represents the unique identifier for a ledger entry.
    /// </summary>
    public class LedgerEntryId : Identity<LedgerEntryId>
    {
        public LedgerEntryId(string value) : base(value) { }
    }
}
