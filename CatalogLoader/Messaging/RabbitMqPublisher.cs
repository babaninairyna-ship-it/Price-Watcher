using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace CatalogLoader.Messaging
{
    /// <summary>
    /// RabbitMQ publisher for sending messages to a queue.
    /// </summary>
    public class RabbitMqPublisher : IMessagePublisher
    {
        public Task PublishAsync<T>(T message, string queueName)
        {
            throw new NotImplementedException();
        }
    }
}
