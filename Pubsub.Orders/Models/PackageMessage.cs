namespace Pubsub.Models;

public record PackageMessage : IOrderMessage
{
    public int UnitsOccupied { get; set; }
    public OrderMessage Order { get; set; }
    public int PackageId { get; set; }

    public override string ToString()
    {
        return $"PackageMessage[{PackageId}|#{UnitsOccupied}]";
    }
}