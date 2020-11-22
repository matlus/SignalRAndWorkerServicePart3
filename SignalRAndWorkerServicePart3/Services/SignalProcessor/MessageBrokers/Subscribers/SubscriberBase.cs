using System;
using System.Threading.Tasks;

namespace SignalRAndWorkerServicePart2
{
    internal abstract class SubscriberBase : IDisposable
    {
        public Task Initialize(string connectionString, string topicName, string queueName)
        {
            return InitializeCore(connectionString, topicName, queueName);
        }

        public void Subscribe(Func<SubscriberBase, MessageReceivedEventArgs, Func<EventMessage, Task>, Task> receiveCallback, Func<EventMessage, Task> onMessageCallback)
        {
            SubscribeCore(receiveCallback, onMessageCallback);
        }

        public Task Acknowledge(string acknowledgetoken)
        {
            return AcknowledgeCore(acknowledgetoken);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected abstract Task InitializeCore(string connectionString, string topicName, string queueName);
        protected abstract void SubscribeCore(Func<SubscriberBase, MessageReceivedEventArgs, Func<EventMessage, Task>, Task> receiveCallback, Func<EventMessage, Task> onMessageCallback);
        protected abstract Task AcknowledgeCore(string acknowledgetoken);
        protected abstract void Dispose(bool disposing);
    }
}
