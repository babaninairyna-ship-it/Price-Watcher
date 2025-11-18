using Microsoft.EntityFrameworkCore;
using PriceWatcher.Data.Interfaces;
using PriceWatcher.Data.Models;

namespace PriceWatcher.Data.Repositories
{
    /// <summary>
    /// Repository for managing Product entities in the database.
    /// Provides methods to save, retrieve, track, and search products.
    /// </summary>
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        /// <summary>
        /// Saves a list of products. Adds new products or updates existing ones by OnlinerKey.
        /// </summary>
        /// <param name="products">List of products to save or update.</param>
        public async Task SaveProductsAsync(List<Product> products)
        {
            foreach (var p in products)
            {
                var existing = await _db.Products.FirstOrDefaultAsync(x => x.OnlinerKey == p.OnlinerKey);
                if (existing is null)
                {
                    _db.Products.Add(p);
                }
                else
                {
                    existing.FullName = p.FullName;
                    existing.PriceMin = p.PriceMin;
                    existing.PriceMax = p.PriceMax;
                    existing.IsTracked = p.IsTracked;
                }
            }

            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Retrieves a single product by its unique Onliner key.
        /// </summary>
        /// <param name="key">Unique Onliner product key.</param>
        /// <returns>The product if found; otherwise null.</returns>
        public async Task<Product?> GetByKeyAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) return null;
            return await _db.Products.FirstOrDefaultAsync(p => p.OnlinerKey == key);
        }

        /// <summary>
        /// Retrieves all products currently being tracked.
        /// </summary>
        /// <returns>List of tracked products.</returns>
        public async Task<List<Product>> GetTrackedProductsAsync()
        {
            return await _db.Products.Where(p => p.IsTracked).ToListAsync();
        }

        /// <summary>
        /// Sets the tracking state of a product by its Onliner key.
        /// </summary>
        /// <param name="key">Unique Onliner product key.</param>
        /// <param name="state">True to track the product, false to untrack.</param>
        public async Task SetTrackingStateAsync(string key, bool state)
        {
            if (string.IsNullOrWhiteSpace(key)) return;

            var p = await _db.Products.FirstOrDefaultAsync(x => x.OnlinerKey == key);
            if (p is null) return;

            p.IsTracked = state;
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Searches products by full name.
        /// </summary>
        /// <param name="search">Search query (optional).</param>
        /// <returns>List of products matching the search criteria.</returns>
        public async Task<List<Product>> SearchAsync(string? search)
        {
            var query = _db.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(p => p.FullName.Contains(search));

            return await query.OrderBy(p => p.FullName).ToListAsync();
        }
    }
}
