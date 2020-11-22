using System;

namespace SignalRAndWorkerServicePart2
{
    public sealed class EventMessage
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedDateTime { get; set; }

        public EventMessage()
        {
        }

        public EventMessage(string id, string title, DateTime createdDateTime)
        {
            Id = id;
            Title = title;
            CreatedDateTime = createdDateTime;
        }
    }
}
