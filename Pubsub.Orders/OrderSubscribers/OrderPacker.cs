using Pubsub.Infrastructure;
using Pubsub.Models;

namespace Pubsub.OrderPublisher;

public class OrderPacker{
    public static int PackageIdSequence = 300 ;
    public OrderPacker()
    {
    }

    public PackageMessage Pack(OrderMessage order)
    {
        var pack = new PackageMessage();
        pack.Order = order;
        pack.PackageId = ++PackageIdSequence;
        pack.UnitsOccupied = GetUnits(order.Quantity);
       
        return pack;
    }
    

    private int GetUnits(int orderQuantity)
    {
        return orderQuantity / 4+1;
    }
}