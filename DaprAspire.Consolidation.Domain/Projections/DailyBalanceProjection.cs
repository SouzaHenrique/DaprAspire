namespace DaprAspire.Consolidation.Domain.Projections
{
    /// <summary>
    /// Projection for daily balance of a ledger.
    /// </summary>
    public class DailyBalanceProjection : Projection
    {
        public DateOnly Date { get; set; }
        public decimal Balance { get; set; }
        public int CreditCount { get; set; }
        public int DebitCount { get; set; }

        public void ApplyCredit(decimal value)
        {
            Balance += value;
            CreditCount++;
            LastUpdated = DateTime.UtcNow;
        }

        public void ApplyDebit(decimal value)
        {
            Balance -= value;
            DebitCount++;
            LastUpdated = DateTime.UtcNow;
        }
    }
}
