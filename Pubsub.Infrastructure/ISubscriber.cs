using System.Diagnostics;

namespace Pubsub.Infrastructure;

public record Subscriber
{
    public Delegate Action { get; set; } 
    public Type Topic { get; set; }
    public  WeakReference Sender { get; set; }

    public Delegate? Filter { get; set; }

}

