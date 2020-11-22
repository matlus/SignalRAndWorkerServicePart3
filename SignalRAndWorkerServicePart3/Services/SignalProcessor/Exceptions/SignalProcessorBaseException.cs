using System;
using System.Diagnostics.CodeAnalysis;

namespace SignalRAndWorkerServicePart2
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public abstract class SignalProcessorBaseException : Exception
    {
        public abstract string Reason { get; }
        protected SignalProcessorBaseException() { }
        protected SignalProcessorBaseException(string message) : base(message) { }
        protected SignalProcessorBaseException(string message, Exception inner) : base(message, inner) { }
        protected SignalProcessorBaseException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
