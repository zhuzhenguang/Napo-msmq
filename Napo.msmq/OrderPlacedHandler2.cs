using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

namespace Napo.msmq
{
    public class OrderPlacedHandler2 : IHandleMessages<OrderPlaced>
    {
        static ILog log = LogManager.GetLogger<OrderPlacedHandler2>();

        public Task Handle(OrderPlaced @event, IMessageHandlerContext context)
        {
            log.Info($"Received OrderPlaced for 2, OrderId = {22}");
            return Task.CompletedTask;
        }
    }
}