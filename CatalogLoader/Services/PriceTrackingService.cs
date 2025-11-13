using CatalogLoader.Messaging;
using PriceWatcher.Data.Models;
using PriceWatcher.Data.Repositories;
using System.Text.Json;

namespace CatalogLoader.Services
{
    /// <summary>
    /// Background service responsible for tracking price changes of tracked products.
    /// </summary>
    public class PriceTrackingService : BackgroundService
    {
        private readonly ILogger<PriceTrackingService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly OnlinerClient _onlinerClient;
        private readonly IMessagePublisher _messagePublisher;

        public PriceTrackingService(
            ILogger<PriceTrackingService> logger,
            IServiceScopeFactory scopeFactory,
            OnlinerClient onlinerClient,
            IMessagePublisher messagePublisher)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
            _onlinerClient = onlinerClient ?? throw new ArgumentNullException(nameof(onlinerClient));
            _messagePublisher = messagePublisher ?? throw new ArgumentNullException(nameof(messagePublisher));
        }

        /// <summary>
        /// Background execution loop for checking price changes.
        /// </summary>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();

                    var productRepo = scope.ServiceProvider.GetRequiredService<ProductRepository>();
                    var priceHistoryRepo = scope.ServiceProvider.GetRequiredService<PriceHistoryRepository>();

                    var trackedProducts = await productRepo.GetTrackedProductsAsync();

                    foreach (var product in trackedProducts)
                    {
                        var updatedProduct = await _onlinerClient.FetchProductByKeyAsync(product.OnlinerKey);
                        if (updatedProduct == null)
                            continue;

                        bool priceChanged = false;

                        if (updatedProduct.PriceMin != product.PriceMin || updatedProduct.PriceMax != product.PriceMax)
                        {
                            decimal oldMin = product.PriceMin;
                            decimal oldMax = product.PriceMax;

                            product.PriceMin = updatedProduct.PriceMin;
                            product.PriceMax = updatedProduct.PriceMax;
                            await productRepo.SaveProductsAsync(new List<Product> { product });
                            priceChanged = true;

                            var historyEntry = new PriceHistory
                            {
                                OnlinerKey = product.OnlinerKey,
                                OldPriceMin = oldMin,
                                NewPriceMin = product.PriceMin,
                                OldPriceMax = oldMax,
                                NewPriceMax = product.PriceMax,
                                ChangedAt = DateTime.UtcNow
                            };
                            await priceHistoryRepo.AddAsync(historyEntry);

                            if (priceChanged)
                            {
                                var message = new PriceChangedMessage
                                {
                                    OnlinerKey = product.OnlinerKey,
                                    OldPriceMin = oldMin,
                                    NewPriceMin = product.PriceMin,
                                    OldPriceMax = oldMax,
                                    NewPriceMax = product.PriceMax,
                                    ChangedAt = DateTime.UtcNow
                                };

                                string messageJson = JsonSerializer.Serialize(message);

                                await _messagePublisher.PublishAsync("price_changes", messageJson);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while checking price changes.");
                }

                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }
}
