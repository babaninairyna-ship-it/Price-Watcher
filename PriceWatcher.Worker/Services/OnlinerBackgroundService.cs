using PriceWatcher.Data.API.Onliner;
using PriceWatcher.Domain;
using PriceWatcher.Domain.Models;

namespace PriceWatcher.Worker.Services
{
    /// <summary>
    /// Background service responsible for fetching products from Onliner
    /// and providing data to API on demand.
    /// </summary>
    public class OnlinerBackgroundService : BackgroundService
    {
        private readonly ILogger<OnlinerBackgroundService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ICatalogClient _onlinerClient;

        public OnlinerBackgroundService(
            ILogger<OnlinerBackgroundService> logger,
            ICatalogClient onlinerClient,
            IServiceScopeFactory scopeFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _onlinerClient = onlinerClient ?? throw new ArgumentNullException(nameof(onlinerClient));
            _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
        }

        /// <summary>
        /// Fetches multiple products from Onliner by search query.
        /// </summary>
        public async Task<List<Product>> FetchProductsAsync(string query)
        {
            return await _onlinerClient.FetchProductsAsync(query);
        }

        /// <summary>
        /// Fetches a single product from Onliner by its key.
        /// </summary>
        public async Task<Product?> FetchProductByKeyAsync(string key)
        {
            return await _onlinerClient.FetchProductByKeyAsync(key);
        }

        /// <summary>
        /// Not used actively, but required by BackgroundService.
        /// Could implement periodic refresh if needed.
        /// </summary>
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // No periodic work is required here since this service just responds to API calls
            return Task.CompletedTask;
        }
    }
}
