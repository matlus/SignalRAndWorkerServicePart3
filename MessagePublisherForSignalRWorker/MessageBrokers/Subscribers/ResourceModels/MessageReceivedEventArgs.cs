using System;
using System.Threading;

namespace MessagePublisherForSignalRWorker
{
    internal sealed class MessageReceivedEventArgs : EventArgs
    {
        public ReceivedMessage ReceivedMessage { get; }
        public string AcknowledgeToken { get; }
        public CancellationToken CancellationToken { get; }

        public MessageReceivedEventArgs(ReceivedMessage receivedMessage, string acknowledgeToken, CancellationToken cancellationToken)
        {
            ReceivedMessage = receivedMessage;
            AcknowledgeToken = acknowledgeToken;
            CancellationToken = cancellationToken;
        }
    }
}
