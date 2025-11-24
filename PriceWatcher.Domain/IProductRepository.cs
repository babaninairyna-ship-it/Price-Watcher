using PriceWatcher.Domain.Models;

namespace PriceWatcher.Domain;

public interface IProductRepository
{
    /// <summary>
    /// Saves new products or updates existing ones.
    /// </summary>
    Task SaveProductsAsync(List<Product> products);

    /// <summary>
    /// Retrieves a product by its Onliner key.
    /// </summary>
    Task<Product?> GetByKeyAsync(string key);

    /// <summary>
    /// Retrieves all products marked as tracked.
    /// </summary>
    Task<List<Product>> GetTrackedProductsAsync();

    /// <summary>
    /// Enables or disables tracking for a product.
    /// </summary>
    Task SetTrackingStateAsync(string key, bool state);

    /// <summary>
    /// Searches products by full name.
    /// </summary>
    Task<List<Product>> SearchAsync(string? search);
}