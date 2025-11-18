using PriceWatcher.Data.Models;

namespace PriceWatcher.Services.Interfaces
{
    public interface IProductService
    {
        Task<List<Product>> SearchWithTrackingAsync(string search);
        Task ToggleTrackingAsync(string key, bool state);
        Task<List<Product>> GetTrackedAsync();
    }
}
