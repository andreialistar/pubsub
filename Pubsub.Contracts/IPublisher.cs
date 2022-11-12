namespace Pubsub.Contracts;

public interface IPublisher
{
    public void Publish<T>(T message);
    void Unsubscribe(object? subscriber);
    Subscriber Subscribe<T>(object subscriber, Action<T> hook);
}