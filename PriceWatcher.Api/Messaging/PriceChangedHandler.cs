using CatalogLoader.Messaging;
using Microsoft.AspNetCore.SignalR;
using PriceWatcher.Api.Hubs;
using PriceWatcher.Data.Models;
using PriceWatcher.Data.Repositories;

namespace PriceWatcher.Api.Messaging
{
    public class PriceChangedHandler
    {
        private readonly ProductRepository _productRepo;
        private readonly PriceHistoryRepository _historyRepo;
        private readonly IHubContext<PriceChangeHub> _hubContext;

        public PriceChangedHandler(
           ProductRepository productRepo,
           PriceHistoryRepository historyRepo,
           IHubContext<PriceChangeHub> hubContext)
        {
            _productRepo = productRepo;
            _historyRepo = historyRepo;
            _hubContext = hubContext;
        }

        /// <summary>
        /// Handles a price change message by updating the product and saving price history
        /// </summary>
        public async Task HandleAsync(PriceChangedMessage message)
        {
            var product = await _productRepo.GetByKeyAsync(message.OnlinerKey);
            if (product == null) return;

            // Update the product's current prices
            product.PriceMin = message.NewPriceMin;
            product.PriceMax = message.NewPriceMax;
            await _productRepo.SaveProductsAsync(new List<Product> { product });

            // Add a new entry to the price history
            var history = new PriceHistory
            {
                OnlinerKey = message.OnlinerKey,
                OldPriceMin = message.OldPriceMin,
                NewPriceMin = message.NewPriceMin,
                OldPriceMax = message.OldPriceMax,
                NewPriceMax = message.NewPriceMax,
                ChangedAt = message.ChangedAt
            };
            await _historyRepo.AddAsync(history);

            await _hubContext.Clients.All.SendAsync(
                "ReceivePriceChange",
                message.OnlinerKey,
                message.OldPriceMin,
                message.NewPriceMin,
                message.OldPriceMax,
                message.NewPriceMax
            );
        }
    }
}
