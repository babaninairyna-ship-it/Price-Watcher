using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceWatcher.Data.Models
{
    public class PriceHistory
    {
        public int Id { get; set; }

        public string OnlinerKey { get; set; } = null!;

        [Precision(18, 2)]
        public decimal OldPriceMin { get; set; }

        [Precision(18, 2)]
        public decimal NewPriceMin { get; set; }

        [Precision(18, 2)]
        public decimal OldPriceMax { get; set; }

        [Precision(18, 2)]
        public decimal NewPriceMax { get; set; }

        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
    }
}
