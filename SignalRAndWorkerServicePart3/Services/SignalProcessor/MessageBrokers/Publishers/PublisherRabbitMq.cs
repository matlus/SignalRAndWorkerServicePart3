using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalRAndWorkerServicePart2
{
    internal sealed class PublisherRabbitMq<TMessage> : PublisherBase<TMessage>
    {
        private bool _disposed;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _topicName;

        public PublisherRabbitMq(string connectionString, string topicName)
        {
            ConnectionFactory connectionFactory = new ConnectionFactory()
            {
                Uri = new Uri(connectionString),
            };

            _connection = connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
            _topicName = topicName;
        }

        protected override Task PublishCore(TMessage message, string id)
        {
            var basicProperties = InitializeBasicProperties(_channel.CreateBasicProperties(), id);
            byte[] messageBody = SerializeMessage(message);
            _channel.BasicPublish(_topicName, routingKey: string.Empty, basicProperties, messageBody);
            return Task.CompletedTask;
        }

        private static IBasicProperties InitializeBasicProperties(IBasicProperties basicProperties, string id)
        {
            basicProperties.Persistent = true;

            var propertiesDictionary = new Dictionary<string, object>();
            basicProperties.Headers = propertiesDictionary;
            propertiesDictionary.Add("message_id", id);
            propertiesDictionary.Add("content_type", "application/json");
            propertiesDictionary.Add("creation_date", DateTimeOffset.UtcNow.ToString("o"));

            return basicProperties;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _channel.Close();
                _channel.Dispose();

                _connection.Close();
                _connection.Dispose();

                _disposed = true;
            }
        }
    }
}
