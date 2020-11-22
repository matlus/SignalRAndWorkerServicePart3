using System;
using System.Threading;

namespace SignalRAndWorkerServicePart2
{
    internal sealed class MessageReceivedEventArgs : EventArgs
    {
        public Message Message { get; }
        public string AcknowledgeToken { get; }
        public CancellationToken CancellationToken { get; }

        public MessageReceivedEventArgs(Message message, string acknowledgeToken, CancellationToken cancellationToken)
        {
            Message = message;
            AcknowledgeToken = acknowledgeToken;
            CancellationToken = cancellationToken;
        }
    }
}
