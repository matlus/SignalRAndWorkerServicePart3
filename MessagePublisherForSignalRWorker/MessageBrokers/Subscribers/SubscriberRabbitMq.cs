using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MessagePublisherForSignalRWorker
{
    internal sealed class SubscriberRabbitMq : SubscriberBase
    {
        private bool _disposed;
        private IConnection _connection;
        private string _queueName;
        private IModel _channel;

        protected override Task InitializeCore(string connectionString, string topicName, string queueName)
        {
            var connectionFactory = new ConnectionFactory()
            {
                Uri = new Uri(connectionString),
            };

            _connection = connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false);
            _channel.QueueBind(queueName, topicName, routingKey: string.Empty, arguments: null);

            _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
            _queueName = queueName;
            return Task.CompletedTask;
        }

        protected override void SubscribeCore(Func<SubscriberBase, MessageReceivedEventArgs, Task> receiveCallback)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var messageReceivedEventArgs = new MessageReceivedEventArgs(
                new ReceivedMessage(
                body: ea.Body.ToArray(),
                messageId: ea.BasicProperties.MessageId,
                contentType: ea.BasicProperties.ContentType,
                creationDateTime: GetDateTimeFromHeaderValue(ea.BasicProperties.Headers, "creation_date")),
                acknowledgeToken: ea.DeliveryTag.ToString(),
                cancellationToken: new CancellationToken());

                await receiveCallback(this, messageReceivedEventArgs);
            };

            _channel.BasicConsume(_queueName, autoAck: false, consumer);
        }

        private static string GetHeaderValueOrNull(IDictionary<string, object> dictionary, string key)
        {
            if (dictionary != null && dictionary.TryGetValue(key, out var value))
            {
                return Encoding.UTF8.GetString((byte[])value);
            }

            return null;
        }

        private static DateTime GetDateTimeFromHeaderValue(IDictionary<string, object> dictionary, string key)
        {
            var value = GetHeaderValueOrNull(dictionary, key);
            return value == null ? DateTime.UtcNow : DateTime.Parse(value);
        }

        protected override Task AcknowledgeCore(string acknowledgetoken)
        {
            _channel.BasicAck(ulong.Parse(acknowledgetoken), multiple: false);
            return Task.CompletedTask;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _connection.Close();
                _connection.Dispose();

                _disposed = true;
            }
        }
    }
}