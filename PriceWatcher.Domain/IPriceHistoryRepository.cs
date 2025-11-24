using PriceWatcher.Domain.Models;

namespace PriceWatcher.Domain
{
    /// <summary>
    /// Defines operations for managing and retrieving product price change history.
    /// </summary>
    public interface IPriceHistoryRepository
    {
        /// <summary>
        /// Adds a new price history record to the storage.
        /// </summary>
        Task AddAsync(PriceHistory history);

        /// <summary>
        /// Retrieves all price change history entries associated with a specific product.
        /// </summary>
        Task<List<PriceHistory>> GetByProductKeyAsync(string onlinerKey);

        /// <summary>
        /// Retrieves all price history records stored in the system.
        /// </summary>
        Task<List<PriceHistory>> GetAllAsync();
    }
}
