using Pubsub.Contracts;
using Pubsub.Models;

namespace Pubsub.OrderSubscribers;

public class PackageDispatcher
{
    private readonly IPublisher _bus;

    private static int _truckIdSequence = 0;
    private readonly int _truckCapacity;

    private DispatchTruckMessage _loadingDispatchTruckMessage = new DispatchTruckMessage();

    public PackageDispatcher(int truckCapacity, IPublisher bus)
    {
        _bus = bus;
        _truckCapacity = truckCapacity;
        CreateNewTruck();
    }

    private void CreateNewTruck()
    {
        _loadingDispatchTruckMessage = new DispatchTruckMessage();
        _loadingDispatchTruckMessage.TruckId = ++_truckIdSequence;
    }

    public void LoadTruck(PackageMessage p)
    {
        if (_loadingDispatchTruckMessage.Load + p.UnitsOccupied > _truckCapacity)
        {
            _bus.Publish(_loadingDispatchTruckMessage);
            CreateNewTruck();
        }

        _loadingDispatchTruckMessage.Packages.Add(p);
        _loadingDispatchTruckMessage.Load += p.UnitsOccupied;
    }

    public void EndDay()
    {
        if (_loadingDispatchTruckMessage.Load > 0)
        {
            _bus.Publish(_loadingDispatchTruckMessage);
            CreateNewTruck();
        }
    }


    public static bool RomaniaHubFilter(PackageMessage x) => x.Order.HubDestination == "Romania";
    public static bool DenmarkHubFilter(PackageMessage x) => x.Order.HubDestination == "Denmark";
    public static bool OtherHubFilter(PackageMessage x) => !RomaniaHubFilter(x) && !DenmarkHubFilter(x);
}