using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Logging;
using NServiceBus.Persistence;
using NServiceBus.Persistence.Legacy;

namespace Napo.msmq
{
    internal class Program
    {
        static readonly ILog Log = LogManager.GetLogger<Program>();

        static async Task Main()
        {
            Console.Title = "Client";

            var endpointConfiguration = new EndpointConfiguration("napo.subscriptions");
            
            var transport = endpointConfiguration.UseTransport<MsmqTransport>();
            endpointConfiguration.SendFailedMessagesTo("napo.error");
            endpointConfiguration.EnableInstallers();
            //endpointConfiguration.DisableFeature<TimeoutManager>();
            endpointConfiguration.UsePersistence<MsmqPersistence, StorageType.Subscriptions>()
                .SubscriptionQueue("napo.subscriptions");
            endpointConfiguration.UsePersistence<InMemoryPersistence, StorageType.Timeouts>();
            //endpointConfiguration.UseSerialization<JsonSerializer>();
            //endpointConfiguration.UseTransport<LearningTransport>();

            var routing = transport.Routing();
            routing.RegisterPublisher(typeof(OrderPlaced), "napo.subscriptions");
            IEndpointInstance endpointInstance = await Endpoint
                .Start(endpointConfiguration)
                .ConfigureAwait(false);

            await RunLoop(endpointInstance);

            await endpointInstance
                .Stop()
                .ConfigureAwait(false);
        }

        static async Task RunLoop(IMessageSession endpointInstance)
        {
            while (true)
            {
                Log.Info("Press 'P' to place an order, or 'Q' to quit.");
                ConsoleKeyInfo key = Console.ReadKey();
                Console.WriteLine();

                switch (key.Key)
                {
                    case ConsoleKey.P:
                        var @event = new OrderPlaced();

                        await endpointInstance
                            .Publish(@event)
                            .ConfigureAwait(false);
                        Log.Info($"Sending OrderPlaced event, OrderId = {33}");

                        break;

                    case ConsoleKey.Q:
                        return;

                    default:
                        Log.Info("Unknown input. Please try again.");
                        break;
                }
            }
        }
    }
}