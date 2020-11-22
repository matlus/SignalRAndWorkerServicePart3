using System;

namespace SignalRAndWorkerServicePart2
{
    public sealed class CommandMessage
    {
        public string Id { get; }
        public string LightColor { get; }
        public bool State { get; }
        public DateTime CreatedDateTime { get; }

        public CommandMessage(string id, string lightColor, bool state, DateTime createdDateTime)
        {
            Id = id;
            LightColor = lightColor;
            State = state;
            CreatedDateTime = createdDateTime;
        }
    }
}
