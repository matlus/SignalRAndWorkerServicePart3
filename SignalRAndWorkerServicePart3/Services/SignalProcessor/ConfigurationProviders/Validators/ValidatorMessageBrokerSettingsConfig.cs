using System;
using System.Text;

namespace SignalRAndWorkerServicePart2
{
    internal static class ValidatorMessageBrokerSettingsConfig
    {
        private static string _messageBrokerTypeNames;

        internal static string MessageBrokerTypeNames
        {
            get
            {
                return _messageBrokerTypeNames ??= string.Join(" ", Enum.GetNames(typeof(MessageBrokerType)));
            }
        }

        public static void Validate(MessageBrokerSettingsConfig messageBrokerSettingsConfig)
        {
            if (messageBrokerSettingsConfig == null)
            {
                throw new ConfigurationSettingMissingException("MessageBroker section is missing in the appsettings.json file, this is a required section.");
            }

            var errorMessages = new StringBuilder();

            ValidateConnectionString(errorMessages, messageBrokerSettingsConfig.MessageBrokerConnectionString);
            ValidateMessageBrokerType(errorMessages, messageBrokerSettingsConfig.MessageBrokerType);
            if (errorMessages.Length != 0) throw new ConfigurationSettingMissingException(errorMessages.ToString());
        }

        private static void ValidateConnectionString(StringBuilder errorMessages, string messageBrokerConnectionString)
        {
            errorMessages.AppendLineIfNotNull(ValidatorString.Validate("MessageBrokerSettings.MessageBrokerConnectionString", messageBrokerConnectionString));
        }

        private static void ValidateMessageBrokerType(StringBuilder errorMessages, string messageBrokerTypeAsString)
        {
            switch (ValidatorString.DetermineNullEmptyOrWhiteSpaces(messageBrokerTypeAsString))
            {
                case StringState.Null:
                case StringState.Empty:
                case StringState.WhiteSpaces:
                    break;
                case StringState.Valid:
                    if (!MessageBrokerTypeNames.Contains(messageBrokerTypeAsString))
                    {
                        errorMessages.Append($"Invalid MessageBrokerType Setting \"{ messageBrokerTypeAsString }\" was provided. The MessageBrokerType Setting can have only one the following values: { MessageBrokerTypeNames }.");
                    }
                    break;
            }
        }
    }
}