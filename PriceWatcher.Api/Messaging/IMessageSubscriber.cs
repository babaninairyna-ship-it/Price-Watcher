namespace PriceWatcher.Api.Messaging
{
    public interface IMessageSubscriber
    {
        Task SubscribeAsync(string queueName, Func<string, Task> onMessageReceived);
    }
}
