using Pubsub.LocalPubSub;
using Pubsub.Models;
using Pubsub.OrderPublisher;
using Pubsub.OrderSubscribers;

var bus = new LocalBus();
var orderPublisher = new OrderPublisher(bus);

var logger = new LogSubscriber();
var orderPacker = new OrderPacker();

var romaniaDispatcher = new PackageDispatcher(15, bus);
var denmarkDispatcher = new PackageDispatcher(20, bus);
var otherDispatcher = new PackageDispatcher(10, bus);

bus.Subscribe<IOrderMessage>(logger, logger.LogToConsole);

bus.Subscribe<OrderMessage>(orderPacker, o =>
    bus.Publish(orderPacker.Pack(o))
);

bus.Subscribe<PackageMessage>(romaniaDispatcher,
    romaniaDispatcher.LoadTruck,
    PackageDispatcher.RomaniaHubFilter);
bus.Subscribe<PackageMessage>(denmarkDispatcher,
    denmarkDispatcher.LoadTruck,
    PackageDispatcher.DenmarkHubFilter);

bus.Subscribe<PackageMessage>(otherDispatcher,
    otherDispatcher.LoadTruck,
    PackageDispatcher.OtherHubFilter);


bus.Subscribe<EndOfDayMessage>(new object(), _ =>
{
    romaniaDispatcher.EndDay();
    denmarkDispatcher.EndDay();
    otherDispatcher.EndDay();
});

await orderPublisher.Start(100, 10);