using System;

namespace MessagePublisherForSignalRWorker
{
    internal sealed class ReceivedMessage
    {
        public byte[] Body { get; }
        public string MessageId { get; }
        public string ContentType { get; }
        public DateTime CreationDateTime { get; }

        public ReceivedMessage(byte[] body, string messageId, string contentType,  DateTime creationDateTime)
        {
            Body = body;
            MessageId = messageId;
            ContentType = contentType;
            CreationDateTime = creationDateTime;
        }
    }
}
