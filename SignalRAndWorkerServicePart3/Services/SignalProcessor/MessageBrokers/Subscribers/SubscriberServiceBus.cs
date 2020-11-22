using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using System;
using System.Threading.Tasks;

namespace SignalRAndWorkerServicePart2
{
    internal sealed class SubscriberServiceBus : SubscriberBase
    {
        private bool _disposed;
        private SubscriptionClient _subscriptionClient;

        protected override async Task InitializeCore(string connectionString, string topicName, string queueName)
        {
            var managementClient = new ManagementClient(connectionString);
            if (!await managementClient.SubscriptionExistsAsync(topicName, queueName))
            {
                await managementClient.CreateSubscriptionAsync(new SubscriptionDescription(topicName, queueName) { MaxDeliveryCount = 1, LockDuration = TimeSpan.FromMinutes(5d) });
            }

            _subscriptionClient = new SubscriptionClient(connectionString, topicName, queueName);
        }

        protected override void SubscribeCore(Func<SubscriberBase, MessageReceivedEventArgs, Func<EventMessage, Task>, Task> receiveCallback, Func<EventMessage, Task> onMessageCallback)
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                AutoComplete = false
            };

            _subscriptionClient.RegisterMessageHandler(async (sbMessage, cancellationToken) =>
            {
                cancellationToken.ThrowIfCancellationRequested();

                var messageReceivedEventArgs = new MessageReceivedEventArgs(
                    new Message(sbMessage.Body,
                                DateTime.UtcNow),
                    sbMessage.SystemProperties.LockToken,
                    cancellationToken);

                await receiveCallback(this, messageReceivedEventArgs, onMessageCallback);
                
            }, messageHandlerOptions);
        }

        private static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            throw new SignalProcessorMessageReceiveException(exceptionReceivedEventArgs.Exception.Message, exceptionReceivedEventArgs.Exception);
        }

        protected override async Task AcknowledgeCore(string acknowledgetoken)
        {
            await _subscriptionClient.CompleteAsync(acknowledgetoken);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && !_disposed && _subscriptionClient != null)
            {
                _subscriptionClient.CloseAsync().ContinueWith(continuationAction =>
                {
                    continuationAction.Wait();
                });

                _disposed = true;
            }
        }       
    }
}
