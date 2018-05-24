using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

namespace Napo.msmq
{
    public class OrderPlacedHandler1 : IHandleMessages<OrderPlaced>
    {
        static ILog log = LogManager.GetLogger<OrderPlacedHandler1>();

        public Task Handle(OrderPlaced @event, IMessageHandlerContext context)
        {
            log.Info($"Received OrderPlaced for 1, OrderId = {11}");
            return Task.CompletedTask;
        }
    }
}