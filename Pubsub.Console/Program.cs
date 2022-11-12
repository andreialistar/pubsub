
using Pubsub.LocalPubSub;
using Pubsub.Models;
using Pubsub.OrderPublisher;
using Pubsub.OrderSubscribers;

var bus = new LocalBus();
var op = new OrderPublisher(bus);

var logger = new LogSubscriber();



bus.Subscribe<IOrderMessage>(logger, logger.LogToConsole);

await op.Start(10, 1000);