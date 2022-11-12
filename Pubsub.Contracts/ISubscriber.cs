namespace Pubsub.Contracts;

public record Subscriber
{
    public Delegate Action { get; set; } 
    public Type Topic { get; set; }
    public  WeakReference Sender { get; set; }
   
}

public record Subscriber<T> : Subscriber
{
    public Func<T, bool> Filter { get; set; }
}