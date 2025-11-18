namespace CatalogLoader.Interfaces
{
    /// <summary>
    /// Defines a service responsible for processing tracked products
    /// and detecting price changes.
    /// </summary>
    public interface IPriceChangeProcessor
    {
        Task ProcessTrackedProductsAsync(CancellationToken token);
    }
}
