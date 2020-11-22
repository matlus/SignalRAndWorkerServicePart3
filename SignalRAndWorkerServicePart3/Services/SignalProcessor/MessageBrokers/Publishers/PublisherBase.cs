using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SignalRAndWorkerServicePart2
{
    internal abstract class PublisherBase<TMessage> : IDisposable
    {
        private readonly JsonStringEnumConverter _jsonStringEnumConverter = new JsonStringEnumConverter(namingPolicy: default, allowIntegerValues: false);
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        protected PublisherBase()
        {
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            _jsonSerializerOptions.Converters.Add(_jsonStringEnumConverter);
        }

        public Task Publish(TMessage message, string id)
        {
            return PublishCore(message, id);
        }

        protected byte[] SerializeMessage(TMessage message)
        {
            string messageJson = JsonSerializer.Serialize<TMessage>(message, _jsonSerializerOptions);
            return Encoding.UTF8.GetBytes(messageJson);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected abstract Task PublishCore(TMessage message, string id);
        protected abstract void Dispose(bool disposing);
    }
}
