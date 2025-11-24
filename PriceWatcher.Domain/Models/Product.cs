namespace PriceWatcher.Domain.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string OnlinerKey { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public decimal PriceMin { get; set; }
        public decimal PriceMax { get; set; }
        public bool IsTracked { get; set; }
    }
}