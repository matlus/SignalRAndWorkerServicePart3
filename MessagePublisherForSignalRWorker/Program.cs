using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MessagePublisherForSignalRWorker
{
    internal static class Program
    {
        static async Task Main()
        {
            var messageBrokerType = MessageBrokerType.RabbitMq;
            var subscriber = MessageBrokerSubscriberFactory.Create(messageBrokerType);

            var publishMessages = false;

            subscriber.Subscribe(async (subs, messageReceivedEventArgs) =>
            {
                var body = messageReceivedEventArgs.ReceivedMessage.Body;
                var commandMessage = SubscriberServiceBus.Deserialize<CommandMessage>(body);
                publishMessages = commandMessage.State;
                await subs.Acknowledge(messageReceivedEventArgs.AcknowledgeToken);
            });

            var messageBrokerPublisher = MessageBrokerPublisherFactory.Create(messageBrokerType);

            Console.WriteLine("Waiting for Start Publishing Message");

            do
            {
                while (publishMessages)
                {
                    foreach (var title in GetTitles(@"..\..\..\RandomTitles.txt"))
                    {
                        var messageId = Guid.NewGuid().ToString("N");
                        var eventMessage = new EventMessage(messageId, title, DateTime.UtcNow);
                        var eventMessageJson = JsonSerializer.Serialize(eventMessage);
                        var messageBytes = Encoding.UTF8.GetBytes(eventMessageJson);
                        var message = new Message(messageBytes, messageId, "application/json");
                        await messageBrokerPublisher.Publish(message);
                        await Task.Delay(1000);
                        if (!publishMessages)
                        {
                            break;
                        }
                    }
                }
            }
            while (true);
        }

        private static IEnumerable<string> GetTitles(string filename)
        {
            var fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
            using var streamReader = new StreamReader(fileStream);

            string line = null;
            while ((line = streamReader.ReadLine()) != null)
            {
                yield return line;
            }
        }
    }
}
