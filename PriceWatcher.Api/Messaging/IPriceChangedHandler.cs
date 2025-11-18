using CatalogLoader.Messaging;

/// <summary>
/// Abstraction for handling price change messages.
/// </summary>
public interface IPriceChangedHandler
{
    /// <summary>
    /// Handles a price change message.
    /// Updates product prices, logs history, and notifies connected clients.
    /// </summary>
    Task HandleAsync(PriceChangedMessage message);
}