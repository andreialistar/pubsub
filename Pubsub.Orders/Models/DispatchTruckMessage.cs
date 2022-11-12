namespace Pubsub.Models;

public record DispatchTruckMessage : IOrderMessage
{
    public List<PackageMessage> Packages { get; set; } = new();
    public int TruckId { get; set; } 
    public int Load { get; set; }

    public override string ToString()
    {
        return $"DispatchTruckMessage[{TruckId}|Load:{Load}|Packages:{Packages.Count}]";
    }
}