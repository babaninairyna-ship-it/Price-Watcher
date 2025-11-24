using PriceWatcher.Services.Interfaces;

namespace PriceWatcher.Worker.Services;

public class PriceTrackingService : BackgroundService
{
    private readonly ILogger<PriceTrackingService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public PriceTrackingService(
        ILogger<PriceTrackingService> logger,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    /// <summary>
    /// Main background loop that periodically triggers processing of tracked products.
    /// </summary>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("PriceTrackingService started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var processor = scope.ServiceProvider.GetRequiredService<IPriceChangeProcessor>();
                await processor.ProcessTrackedProductsAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in price tracking loop.");
            }

            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }
}