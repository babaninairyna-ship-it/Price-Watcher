using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogLoader.Messages
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
