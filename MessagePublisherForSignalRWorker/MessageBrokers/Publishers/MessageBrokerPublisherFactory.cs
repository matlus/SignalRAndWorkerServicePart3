namespace MessagePublisherForSignalRWorker
{
    internal static class MessageBrokerPublisherFactory
    {
        const string brokerConnectionStringRabbitMq = "amqp://youtube:password@localhost/youtube.vhost";
        const string brokerConnectionStringServiceBus = "<Your ServiceBus Connection string>";
        const string titleTopic = "title.events.topic";

        public static PublisherBase Create(MessageBrokerType messageBrokerType)
        {
            switch (messageBrokerType)
            {
                case MessageBrokerType.RabbitMq:
                    return new PublisherRabbitMq(brokerConnectionStringRabbitMq, titleTopic);
                        
                case MessageBrokerType.ServiceBus:
                    return new PublisherServiceBus(brokerConnectionStringServiceBus, titleTopic);
            }

            throw new MessageBrokerTypeNotSupportedException($"The MessageBrokerType: {messageBrokerType}, is not supported yet");
        }
    }

    internal enum MessageBrokerType { RabbitMq, ServiceBus }
}
