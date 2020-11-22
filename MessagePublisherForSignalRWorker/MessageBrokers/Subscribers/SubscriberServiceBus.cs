using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MessagePublisherForSignalRWorker
{
    internal sealed class SubscriberServiceBus : SubscriberBase
    {
        private bool _disposed;
        private SubscriptionClient _subscriptionClient;

        protected override async Task InitializeCore(string connectionString, string topicName, string queueName)
        {
            var managementClient = new ManagementClient(connectionString);
            try
            {
                if (!await managementClient.SubscriptionExistsAsync(topicName, queueName))
                {
                    await managementClient.CreateSubscriptionAsync(new SubscriptionDescription(topicName, queueName) { MaxDeliveryCount = 1, LockDuration = TimeSpan.FromMinutes(5d) });
                }

                _subscriptionClient = new SubscriptionClient(connectionString, topicName, queueName);
            }
            finally
            {
                await managementClient.CloseAsync();
            }
        }

        protected override void SubscribeCore(Func<SubscriberBase, MessageReceivedEventArgs, Task> receiveCallback)
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                AutoComplete = false,
                MaxConcurrentCalls = 1
            };

            _subscriptionClient.RegisterMessageHandler(async (sbMessage, cancellationToken) =>
            {
                cancellationToken.ThrowIfCancellationRequested();

                var messageReceivedEventArgs = new MessageReceivedEventArgs(
                    new ReceivedMessage(
                        body: sbMessage.Body,
                        messageId: sbMessage.MessageId,
                        contentType: sbMessage.ContentType,
                        creationDateTime: GetDateTimeFromHeaderValue(sbMessage.UserProperties, "CreationDate")),
                        acknowledgeToken: sbMessage.SystemProperties.LockToken,
                        cancellationToken: cancellationToken);

                await receiveCallback(this, messageReceivedEventArgs);
            }, messageHandlerOptions);
        }

        private static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            throw new AgentMessageReceivedException(exceptionReceivedEventArgs.Exception.Message, exceptionReceivedEventArgs.Exception);
        }

        private static string GetHeaderValueOrNull(IDictionary<string, object> dictionary, string key)
        {
            if (dictionary != null && dictionary.TryGetValue(key, out var value))
            {
                return (string)value;
            }

            return null;
        }

        private static DateTime GetDateTimeFromHeaderValue(IDictionary<string, object> dictionary, string key)
        {
            var value = GetHeaderValueOrNull(dictionary, key);
            return value == null ? DateTime.UtcNow : DateTime.Parse(value);
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
