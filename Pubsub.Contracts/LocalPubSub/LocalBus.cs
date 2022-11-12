﻿using System.Collections.Concurrent;
using Pubsub.Contracts;

namespace Pubsub.LocalPubSub;

public class LocalBus : IPublisher
{
    private readonly ICollection<Subscriber> _subscribers = new List<Subscriber>();
    public virtual int SubscribersCount
    {
        get
        {
            PruneSubscribers();
            return _subscribers.Count;
        }
    }

    public virtual Subscriber Subscribe<T>(object subscriber, Action<T> hook )
    {
        var sub = new Subscriber
        {
            Action = hook,
            Sender = new WeakReference(subscriber, false),
            Topic = typeof(T),
        };

        _subscribers.Add(sub);

        return sub;
    }

    public virtual void Publish<T>(T message)
    {
        PruneSubscribers();
        foreach (var s in _subscribers)
        {
            if (typeof(T).IsAssignableTo(s.Topic))
            {
                ((Action<T>) s.Action)(message);
            }
        }
    }

    private void PruneSubscribers()
    {
        var toRemove = _subscribers.Where(x => !x.Sender.IsAlive);

        foreach (var r in toRemove.ToList())
        {
            _subscribers.Remove(r);
        }
    }

    public virtual void Unsubscribe(object? subscriber)
    {
        var toRemove = _subscribers.Where(x => x.Sender.Target == subscriber);

        foreach (var r in toRemove.ToList())
        {
            _subscribers.Remove(r);
        }
    }
}