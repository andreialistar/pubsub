namespace Pubsub.Models;

public record OrderMessage : IOrderMessage
{
    public int OrderId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public string HubDestination { get; set; }

    public override string ToString()
    {
        return $"Order[{OrderId}|{ProductName}|{Quantity}|{HubDestination}]";
    }
}