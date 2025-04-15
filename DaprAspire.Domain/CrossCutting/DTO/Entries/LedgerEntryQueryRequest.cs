namespace DaprAspire.Domain.CrossCutting.DTO.Entries
{
    /// <summary>
    /// Represents a request to query ledger entries with pagination.
    /// </summary>
    public class LedgerEntryQueryRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
