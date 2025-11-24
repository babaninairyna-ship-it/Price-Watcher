namespace PriceWatcher.Services.Messages
{
    public class PriceChangedMessage
    {
        public string OnlinerKey { get; set; } = null!;
        public decimal OldPriceMin { get; set; }
        public decimal NewPriceMin { get; set; }
        public decimal OldPriceMax { get; set; }
        public decimal NewPriceMax { get; set; }
        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
    }
}
