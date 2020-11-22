using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace SignalRAndWorkerServicePart2
{
    public sealed class MessageBrokerHub : Hub
    {
        private readonly SignalProcessorManager _signalProcessorManager;

        public MessageBrokerHub(SignalProcessorManager signalProcessorManager)
        {
            _signalProcessorManager = signalProcessorManager;
        }

        public async Task CommandReceived(string lightColor, bool state)
        {
            await _signalProcessorManager.PublishCommandMessage(new CommandMessage(
                id: Guid.NewGuid().ToString("N"),
                lightColor: lightColor,
                state: state,
                createdDateTime: DateTime.UtcNow));
        }
    }
}
