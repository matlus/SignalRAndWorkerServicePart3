using System;

namespace SignalRAndWorkerServicePart2
{
    internal sealed class Message
    {
        public Byte[] Body { get; }
        public DateTime CreationDate { get; }

        public Message(byte[] body, DateTime creationDate)
        {
            Body = body;
            CreationDate = creationDate;
        }
    }
}
