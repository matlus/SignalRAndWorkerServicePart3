using System;
using System.Diagnostics.CodeAnalysis;

namespace MessagePublisherForSignalRWorker
{
    [ExcludeFromCodeCoverage]
    public sealed class AgentMessageReceivedException : Exception
    {
        public AgentMessageReceivedException() { }
        public AgentMessageReceivedException(string message) : base(message) { }
        public AgentMessageReceivedException(string message, Exception inner) : base(message, inner) { }
    }
}