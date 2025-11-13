using RabbitMQ.Client;
using System.Security.Cryptography;
using System.Text;

namespace CatalogLoader.Messaging
{
    public class RabbitMqPublisher : IMessagePublisher, IAsyncDisposable
    {
        private readonly IConnection _connection;
        private readonly IChannel _channel;

        public RabbitMqPublisher(string hostName = "localhost")
        {
            var factory = new ConnectionFactory
            {
                HostName = hostName,
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };

            _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
            _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Asynchronously publishes a message to the specified queue
        /// </summary>
        public async Task PublishAsync(string queueName, string message)
        {
            string safeQueueName = EnsureRoutingKey(queueName);

            // Declare a durable queue
            await _channel.QueueDeclareAsync(
                queue: safeQueueName,
                durable: true,         // queue survives broker restarts
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var body = Encoding.UTF8.GetBytes(message);
            var properties = new BasicProperties
            {
                Persistent = true      // make the message persistent
            };

            // Publish the message
            await _channel.BasicPublishAsync(
                exchange: "",
                routingKey: safeQueueName,
                mandatory: false,
                basicProperties: properties,
                body: body);
        }

        /// <summary>
        /// Ensures the routing key does not exceed 255 bytes; hashes if too long
        /// </summary>
        private static string EnsureRoutingKey(string key)
        {
            var bytes = Encoding.UTF8.GetBytes(key);
            if (bytes.Length <= 255) return key;

            using var sha1 = SHA1.Create();
            var hashBytes = sha1.ComputeHash(bytes);
            return Convert.ToHexString(hashBytes).Substring(0, 40);
        }

        /// <summary>
        /// Asynchronously disposes of the connection and channel
        /// </summary>
        public async ValueTask DisposeAsync()
        {
            if (_channel != null)
                await _channel.DisposeAsync();

            if (_connection != null)
                await _connection.DisposeAsync();
        }
    }
}
