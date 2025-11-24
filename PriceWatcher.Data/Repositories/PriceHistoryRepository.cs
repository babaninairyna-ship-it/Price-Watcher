using Microsoft.EntityFrameworkCore;
using PriceWatcher.Domain;
using PriceWatcher.Domain.Models;

namespace PriceWatcher.Data.Repositories
{
    /// <summary>
    /// Repository for saving and retrieving price change history.
    /// </summary>
    public class PriceHistoryRepository : IPriceHistoryRepository
    {
        private readonly ApplicationDbContext _db;

        public PriceHistoryRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Adds a new price change record.
        /// </summary>
        public async Task AddAsync(PriceHistory history)
        {
            if (history == null) return;

            _db.PriceHistories.Add(history);
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Retrieves all price change history for a specific product.
        /// </summary>
        public async Task<List<PriceHistory>> GetByProductKeyAsync(string onlinerKey)
        {
            return await _db.PriceHistories
                .AsNoTracking()
                .Where(h => h.OnlinerKey == onlinerKey)
                .OrderByDescending(h => h.ChangedAt)
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves all price change history records.
        /// </summary>
        public async Task<List<PriceHistory>> GetAllAsync()
        {
            return await _db.PriceHistories
                .AsNoTracking()
                .OrderByDescending(h => h.ChangedAt)
                .ToListAsync();
        }
    }
}
