namespace Pubsub.Models;

public class Package
{
    public int UnitsOccupied { get; set; }
    public bool KeepCold { get; set; }
    public Order Order { get; set; }
}