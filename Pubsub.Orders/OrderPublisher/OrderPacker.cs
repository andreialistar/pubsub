using Pubsub.Contracts;
using Pubsub.Models;

namespace Pubsub.OrderPublisher;

public class OrderPacker{
    private readonly IPublisher _bus;
    public OrderPacker(IPublisher bus) => _bus = bus;

    public Package Pack(Order order)
    {
        var pack = new Package();
        pack.Order = order;
        pack.UnitsOccupied = GetUnits(order.Quantity);
        pack.KeepCold = new Random().NextDouble() > 0.7;
        
        _bus.Publish(pack);
        return pack;
    }
    

    private int GetUnits(int orderQuantity)
    {
        return orderQuantity / 4;
    }
}