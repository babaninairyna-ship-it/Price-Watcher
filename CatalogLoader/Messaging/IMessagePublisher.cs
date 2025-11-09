using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogLoader.Messaging
{
    /// <summary>
    /// Interface for publishing messages to a message broker.
    /// </summary>
    public interface IMessagePublisher
    {
        /// <summary>
        /// Publishes a message to the specified queue.
        /// </summary>
        /// <param name="queueName">The target queue.</param>
        /// <param name="message">The message payload.</param>
        Task PublishAsync<T>(T message, string queueName);
    }
}