using NServiceBus;

namespace Napo.msmq
{
    public class OrderPlaced : IEvent
    {
        public string OrderId { get; set; }
    }
}