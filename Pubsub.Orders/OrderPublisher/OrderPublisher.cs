using Bogus;
using Pubsub.Infrastructure;
using Pubsub.LocalPubSub;
using Pubsub.Models;

namespace Pubsub.OrderPublisher;

public class OrderPublisher
{
    private readonly IPublisher _bus;
    public OrderPublisher(IPublisher bus) => _bus = bus;


    public async Task Start(int numberOfOrders, int delayInMs)
    {
        var hubDestinations = new[] {"Norway", "Denmark", "Romania", "Spain"};
        var orderIds = 100;

        var testOrders = new Faker<OrderMessage>()
            .StrictMode(true)
            .RuleFor(o => o.OrderId, f => orderIds++)
            .RuleFor(o => o.Quantity, f => f.Random.Int(3, 20))
            .RuleFor(o => o.ProductName, f => f.Commerce.ProductName())
            .RuleFor(o => o.HubDestination, f => f.Random.ArrayElement(hubDestinations));

        var orders = testOrders.Generate(numberOfOrders);

        foreach (var o in orders)
        {
            _bus.Publish(o);

            await Task.Delay(delayInMs);
        }
        
        _bus.Publish(new EndOfDayMessage());

    }
}

