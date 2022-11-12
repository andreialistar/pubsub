using Pubsub.Contracts;

namespace Pubsub.LocalPubSub;

public class LockedLocalBus : LocalBus
{
    private readonly LocalBus _localBus = new();

    private static readonly object lck = new();

    public override int SubscribersCount
    {
        get { return _localBus.SubscribersCount; }
    }

    public override Subscriber Subscribe<T>(object subscriber, Action<T> hook, Func<T,bool> filter = null)
    {
        lock (lck)
        {
            return _localBus.Subscribe<T>(subscriber, hook, filter);
        }
    }

    public override void Unsubscribe(object? subscriber)
    {
        lock (lck)
        {
            _localBus.Unsubscribe(subscriber);
        }
    }

    public override void Publish<T>(T message)
    {
        lock (lck)
        {
            _localBus.Publish(message);
        }
    }
}