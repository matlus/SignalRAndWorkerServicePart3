namespace MessagePublisherForSignalRWorker
{
    internal sealed class Message
    {
        public byte[] Body { get; }
        public string MessageId { get; }
        public string ContentType { get; }

        public Message(byte[] body, string messageId, string contentType)
        {
            Body = body;
            MessageId = messageId;
            ContentType = contentType;
        }
    }
}
