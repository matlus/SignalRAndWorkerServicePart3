using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics.CodeAnalysis;

namespace SignalRAndWorkerServicePart2
{
    internal class ConfigurationProvider
    {
        private readonly IConfigurationRoot _configurationRoot;

        public ConfigurationProvider()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appsettings.json");
            LoadEnvironmentSpecificAppSettings(configurationBuilder);

            _configurationRoot = configurationBuilder.Build();
        }

        private static void LoadEnvironmentSpecificAppSettings(ConfigurationBuilder configurationBuilder)
        {
            var aspNetCoreEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (aspNetCoreEnvironment != null)
            {
                configurationBuilder.AddJsonFile($"appsettings.{aspNetCoreEnvironment}.json");
            }
        }

        [ExcludeFromCodeCoverage]
        internal ConfigurationProvider(IConfigurationRoot configurationRoot)
        {
            _configurationRoot = configurationRoot;
        }

        internal string RetrieveConfigurationSettingValue(string key)
        {
            return _configurationRoot[key];
        }

        public MessageBrokerSettings GetMessageBrokerSettings()
        {
            var monitorSettingsConfig = GetMessageBrokerSettingsPreValidated();
            ValidatorMessageBrokerSettingsConfig.Validate(monitorSettingsConfig);
            return MapperMessageBrokerSettingsConfig.MapToMessageBrokerSettings(monitorSettingsConfig);
        }

        private MessageBrokerSettingsConfig GetMessageBrokerSettingsPreValidated()
        {
            const string messageBrokerSettingsKey = "MessageBroker";
            var messageBrokerSettingsConfig = _configurationRoot.GetSection(messageBrokerSettingsKey).Get<MessageBrokerSettingsConfig>();
            return messageBrokerSettingsConfig;
        }
    }
}
