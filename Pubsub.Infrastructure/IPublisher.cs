namespace Pubsub.Infrastructure;

public interface IPublisher
{
    public void Publish<T>(T message);
    void Unsubscribe(object? subscriber);
    Subscriber Subscribe<T>(object subscriber, Action<T> hook, Func<T, bool> filter = null);
}