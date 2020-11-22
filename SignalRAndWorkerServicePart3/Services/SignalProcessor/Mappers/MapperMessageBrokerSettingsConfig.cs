using System;

namespace SignalRAndWorkerServicePart2
{
    internal static class MapperMessageBrokerSettingsConfig
    {
        public static MessageBrokerSettings MapToMessageBrokerSettings(MessageBrokerSettingsConfig monitorSettingsConfig)
        {
            return new MessageBrokerSettings(
                monitorSettingsConfig.MessageBrokerConnectionString,
                ConvertToMessageBrokerType(monitorSettingsConfig.MessageBrokerType)
                );
        }

        private static MessageBrokerType ConvertToMessageBrokerType(string messageBrokerTypeAsString)
        {
            switch (ValidatorString.DetermineNullEmptyOrWhiteSpaces(messageBrokerTypeAsString))
            {
                case StringState.Null:
                case StringState.Empty:
                case StringState.WhiteSpaces:
                    return MessageBrokerType.ServiceBus;
                default:
                    return (MessageBrokerType)Enum.Parse(typeof(MessageBrokerType), messageBrokerTypeAsString);
            }
        }
    }
}
