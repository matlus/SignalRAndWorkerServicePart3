using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace MessagePublisherForSignalRWorker
{
    internal abstract class SubscriberBase : IDisposable
    {
        public Task Initialize(string connectionString, string topicName, string queueName)
        {
            return InitializeCore(connectionString, topicName, queueName);
        }

        public void Subscribe(Func<SubscriberBase, MessageReceivedEventArgs, Task> receiveCallback)
        {
            SubscribeCore(receiveCallback);
        }

        public Task Acknowledge(string acknowledgetoken)
        {
            return AcknowledgeCore(acknowledgetoken);
        }

        public static T Deserialize<T>(byte[] jsonBytes)
        {
            var jsonString = Encoding.UTF8.GetString(jsonBytes);

            try
            {
                return JsonConvert.DeserializeObject<T>(jsonString);
            }
            catch (JsonException e)
            {
                throw new MessageDeserializationFailedException($@"Exception occurred while attempting to deserialize JSON Message body to target of Type: {typeof(T).Name}. JSON Body received: {jsonString}. The Original Exception type is: {e.GetType().Name}. Original Exception message: {e.Message}", e);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected abstract Task AcknowledgeCore(string acknowledgetoken);
        protected abstract void Dispose(bool disposing);
        protected abstract Task InitializeCore(string connectionString, string topicName, string queueName);
        protected abstract void SubscribeCore(Func<SubscriberBase, MessageReceivedEventArgs, Task> receiveCallback);
    }
}