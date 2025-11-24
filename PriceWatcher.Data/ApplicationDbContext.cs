using Microsoft.EntityFrameworkCore;
using PriceWatcher.Domain.Models;

namespace PriceWatcher.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<SearchQuery> SearchQueries { get; set; } = null!;
        public DbSet<PriceHistory> PriceHistories { get; set; } = null!;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(setup =>
            {
                setup.ToTable("Products");
                setup.Property(x => x.PriceMin).HasPrecision(18, 2);
                setup.Property(x => x.PriceMax).HasPrecision(18, 2);
            });

            modelBuilder.Entity<PriceHistory>(setup =>
            {
                setup.Property(e => e.OldPriceMin).HasPrecision(18, 2);
                setup.Property(e => e.NewPriceMin).HasPrecision(18, 2);
                setup.Property(e => e.OldPriceMax).HasPrecision(18, 2);
                setup.Property(e => e.NewPriceMax).HasPrecision(18, 2);
            });
        }
    }
}
