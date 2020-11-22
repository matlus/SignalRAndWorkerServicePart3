using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SignalRAndWorkerServicePart2
{
    internal static class MessageProcessor
    {
        private static readonly JsonStringEnumConverter _jsonStringEnumConverter = new JsonStringEnumConverter(namingPolicy: default, allowIntegerValues: false);
        private static readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions();

        static MessageProcessor()
        {
            _jsonSerializerOptions.PropertyNameCaseInsensitive = true;
            _jsonSerializerOptions.Converters.Add(_jsonStringEnumConverter);
        }

        public static EventMessage DeserializeMessage(Message message)
        {
            var messageBody = Encoding.UTF8.GetString(message.Body);
            return Deserialize<EventMessage>(messageBody);
        }

        private static T Deserialize<T>(string jsonString)
        {
            return JsonSerializer.Deserialize<T>(jsonString, _jsonSerializerOptions);
        }
    }
}
