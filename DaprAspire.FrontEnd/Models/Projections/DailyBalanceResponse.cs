namespace DaprAspire.FrontEnd.Models.Projections
{
    public class DailyBalanceResponse
    {
        public string LedgerId { get; set; } = default!;
        public double Balance { get; set; }
        public DateTime Date { get; set; }
    }
}
