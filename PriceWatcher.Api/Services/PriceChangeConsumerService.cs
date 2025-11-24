using System.Text.Json;
using PriceWatcher.Api.Messaging;
using PriceWatcher.Services.Messages;

namespace PriceWatcher.Api.Services
{
    /// <summary>
    /// Background service that consumes price change messages and
    /// delegates processing to IPriceChangedHandler inside DI scope.
    /// </summary>
    public class PriceChangeConsumerService : BackgroundService
    {
        private readonly ILogger<PriceChangeConsumerService> _logger;
        private readonly IMessageSubscriber _subscriber;
        private readonly IServiceScopeFactory _scopeFactory;

        public PriceChangeConsumerService(
            ILogger<PriceChangeConsumerService> logger,
            IMessageSubscriber subscriber,
            IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _subscriber = subscriber;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("PriceChangeConsumerService started. Waiting for messages...");

            await _subscriber.SubscribeAsync("price_changes", async msg =>
            {
                try
                {
                    var priceChange = JsonSerializer.Deserialize<PriceChangedMessage>(msg);
                    if (priceChange == null) return;

                    using var scope = _scopeFactory.CreateScope();
                    var handler = scope.ServiceProvider.GetRequiredService<IPriceChangedHandler>();

                    await handler.HandleAsync(priceChange);

                    _logger.LogInformation(
                        $"Price updated for {priceChange.OnlinerKey}. " +
                        $"Min: {priceChange.OldPriceMin} -> {priceChange.NewPriceMin}, " +
                        $"Max: {priceChange.OldPriceMax} -> {priceChange.NewPriceMax}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while processing price change message");
                }
            });
        }
    }
}
