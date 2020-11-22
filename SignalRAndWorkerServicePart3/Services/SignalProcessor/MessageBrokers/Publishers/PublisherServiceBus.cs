using Microsoft.Azure.ServiceBus;
using System;
using System.Threading.Tasks;
using ServiceBusMessage = Microsoft.Azure.ServiceBus.Message;

namespace SignalRAndWorkerServicePart2
{
    internal class PublisherServiceBus<TMessage> : PublisherBase<TMessage>
    {
        private bool _disposed;
        private readonly TopicClient _topicClient;

        public PublisherServiceBus(string connectionString, string topicName)
        {
            _topicClient = new TopicClient(connectionString, topicName);
        }

        protected override async Task PublishCore(TMessage message, string id)
        {
            byte[] messageBody = SerializeMessage(message);
            var serviceBusMessage = InitializeMessageProperties(messageBody, id);
            await _topicClient.SendAsync(serviceBusMessage);
        }

        private static ServiceBusMessage InitializeMessageProperties(byte[] messageBody, string id)
        {
            var serviceBusMessage = new ServiceBusMessage(messageBody)
            {
                MessageId = id,
                ContentType = "application/json"
            };
            serviceBusMessage.UserProperties.Add("CreationDate", DateTimeOffset.UtcNow.ToString("o"));

            return serviceBusMessage;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _topicClient.CloseAsync().ContinueWith(continuationTask =>
                {
                    continuationTask.Wait();
                });

                _disposed = true;
            }
        }
    }
}
