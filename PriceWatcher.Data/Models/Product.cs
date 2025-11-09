using Microsoft.EntityFrameworkCore;

namespace PriceWatcher.Data.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string OnlinerKey { get; set; } = null!;
        public string FullName { get; set; } = null!;

        [Precision(18, 2)]
        public decimal PriceMin { get; set; }

        [Precision(18, 2)]
        public decimal PriceMax { get; set; }

        public bool IsTracked { get; set; }
    }
}