namespace CatalogLoader.Messaging
{
    /// <summary>
    /// Interface for publishing messages to a message broker.
    /// </summary>
    public interface IMessagePublisher
    {
        Task PublishAsync(string queueName, string message);
    }
}