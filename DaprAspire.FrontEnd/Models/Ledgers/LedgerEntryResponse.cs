namespace DaprAspire.FrontEnd.Models.Ledgers
{
    public class LedgerEntryResponse
    {
        public string Id { get; set; } = default!;
        public decimal Balance { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
