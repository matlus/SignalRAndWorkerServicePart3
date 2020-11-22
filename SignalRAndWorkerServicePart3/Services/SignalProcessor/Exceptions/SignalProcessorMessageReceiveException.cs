using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace SignalRAndWorkerServicePart2
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public sealed class SignalProcessorMessageReceiveException : SignalProcessorTechnicalBaseException
    {
        public override string Reason => "Signal Processor Message Received Exception";
        public SignalProcessorMessageReceiveException() { }
        public SignalProcessorMessageReceiveException(string message) : base(message) { }
        public SignalProcessorMessageReceiveException(string message, Exception inner) : base(message, inner) { }
        private SignalProcessorMessageReceiveException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
