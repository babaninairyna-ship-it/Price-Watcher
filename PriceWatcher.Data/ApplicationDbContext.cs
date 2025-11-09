using Microsoft.EntityFrameworkCore;
using PriceWatcher.Data.Models;

namespace PriceWatcher.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<SearchQuery> SearchQueries { get; set; } = null!;
        public DbSet<PriceHistory> PriceHistories { get; set; } = null!;


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

    }
}
