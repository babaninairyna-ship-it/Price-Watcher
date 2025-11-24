using System.Text.Json;
using Microsoft.Extensions.Logging;
using PriceWatcher.Domain;
using PriceWatcher.Domain.Models;
using PriceWatcher.Services.Interfaces;
using PriceWatcher.Services.Messages;

namespace PriceWatcher.Services
{
    /// <summary>
    /// Handles business logic for detecting price changes in tracked products.
    /// Fetches updates, compares prices, saves history, and publishes events.
    /// </summary>
    public class PriceChangeProcessor : IPriceChangeProcessor
    {
        private readonly IProductRepository _productRepo;
        private readonly IPriceHistoryRepository _historyRepo;
        private readonly ICatalogClient _onlinerClient;
        private readonly IMessagePublisher _publisher;
        private readonly ILogger<PriceChangeProcessor> _logger;

        public PriceChangeProcessor(
            IProductRepository productRepo,
            IPriceHistoryRepository historyRepo,
            ICatalogClient onlinerClient,
            IMessagePublisher publisher,
            ILogger<PriceChangeProcessor> logger)
        {
            _productRepo = productRepo;
            _historyRepo = historyRepo;
            _onlinerClient = onlinerClient;
            _publisher = publisher;
            _logger = logger;
        }

        /// <summary>
        /// Processes all tracked products: loads current data from Onliner,
        /// detects price changes, updates DB, stores history, and publishes messages.
        /// </summary>
        public async Task ProcessTrackedProductsAsync(CancellationToken token)
        {
            var tracked = await _productRepo.GetTrackedProductsAsync();

            foreach (var product in tracked)
            {
                token.ThrowIfCancellationRequested();

                // Fetch updated product info from Onliner API
                var updated = await _onlinerClient.FetchProductByKeyAsync(product.OnlinerKey);
                if (updated == null) continue;

                // Skip if the price has not changed
                if (updated.PriceMin == product.PriceMin &&
                    updated.PriceMax == product.PriceMax)
                    continue;

                // Save old prices for history + event publishing
                var oldMin = product.PriceMin;
                var oldMax = product.PriceMax;

                // Update current product prices
                product.PriceMin = updated.PriceMin;
                product.PriceMax = updated.PriceMax;

                await _productRepo.SaveProductsAsync(new List<Product> { product });

                // Save price change to history
                var history = new PriceHistory
                {
                    OnlinerKey = product.OnlinerKey,
                    OldPriceMin = oldMin,
                    NewPriceMin = product.PriceMin,
                    OldPriceMax = oldMax,
                    NewPriceMax = product.PriceMax,
                    ChangedAt = DateTime.UtcNow
                };

                await _historyRepo.AddAsync(history);

                // Publish RabbitMQ message
                var msg = new PriceChangedMessage
                {
                    OnlinerKey = product.OnlinerKey,
                    OldPriceMin = oldMin,
                    NewPriceMin = product.PriceMin,
                    OldPriceMax = oldMax,
                    NewPriceMax = product.PriceMax,
                    ChangedAt = DateTime.UtcNow
                };

                await _publisher.PublishAsync("price_changes", JsonSerializer.Serialize(msg));

                _logger.LogInformation(
                    $"Price changed for {product.OnlinerKey}: {oldMin}->{product.PriceMin}, {oldMax}->{product.PriceMax}");
            }
        }
    }
}
