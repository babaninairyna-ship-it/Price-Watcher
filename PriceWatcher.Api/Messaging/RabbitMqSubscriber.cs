using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace PriceWatcher.Api.Messaging
{
    public class RabbitMqSubscriber : IMessageSubscriber, IAsyncDisposable
    {
        private readonly IConnection _connection;
        private readonly IChannel _channel;

        public RabbitMqSubscriber(string hostName = "localhost")
        {
            var factory = new ConnectionFactory
            {
                HostName = hostName
            };

            _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
            _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();
        }

        public async Task SubscribeAsync(string queueName, Func<string, Task> onMessageReceived)
        {
            await _channel.QueueDeclareAsync(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += async (_, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                await onMessageReceived(message);

                await _channel.BasicAckAsync(ea.DeliveryTag, false);
            };

            await _channel.BasicConsumeAsync(
                queue: queueName,
                autoAck: false,
                consumer: consumer);
        }

        public async ValueTask DisposeAsync()
        {
            if (_channel != null)
                await _channel.DisposeAsync();

            if (_connection != null)
                await _connection.DisposeAsync();
        }
    }
}