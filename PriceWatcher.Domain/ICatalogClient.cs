using PriceWatcher.Domain.Models;

namespace PriceWatcher.Domain;

public interface ICatalogClient
{
    Task<Product?> FetchProductByKeyAsync(string key);
    Task<List<Product>> FetchProductsAsync(string query);
}