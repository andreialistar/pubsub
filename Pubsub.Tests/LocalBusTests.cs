using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Pubsub.Infrastructure;
using Pubsub.LocalPubSub;

namespace Pubsub.Tests;

public class Tests
{
    private LocalBus _bus;
    private object? _disposableSubscriber;

    [SetUp]
    public void Setup()
    {
        _bus = new LockedLocalBus();
        _disposableSubscriber = new object();
    }

    [Test]
    public void Publish_CallAllHooks()
    {
        int count = 0;
        _bus.Subscribe<int>(new object(), _ => { count++; });
        _bus.Subscribe<int>(new object(), _ => { count++; });

        _bus.Publish(0);

        Assert.AreEqual(2, count);
    }


    [Test]
    public void Publish_CallByTopic()
    {
        int count = 0;
        _bus.Subscribe<int>(new object(), _ => { count++; });
        _bus.Subscribe<string>(new object(), _ => { count++; });

        _bus.Publish(0);

        Assert.AreEqual(1, count);
    }

    [Test]
    public void Publish_CallByTopic_CheckResponse()
    {
        int count = 0;
        string responseString = "";
        int responseInt = -3;
        _bus.Subscribe(new object(), (int i) => { responseInt = i; });
        _bus.Subscribe(new object(), (string s) => { responseString = s; });

        _bus.Publish(99);
        _bus.Publish("test");

        Assert.AreEqual(99, responseInt);
        Assert.AreEqual("test", responseString);
    }

    [Test]
    public void Publish_SameSubscriberMultipleTopics()
    {
        int count = 0;
        var testSubscriber = new TestSubscriber();
        _bus.Subscribe<int>(testSubscriber, _ => { count++; });
        _bus.Subscribe<string>(testSubscriber, _ => { count++; });

        _bus.Publish(0);
        _bus.Publish("Test");


        Assert.AreEqual(2, count);
    }

    [Test]
    public void Publish_UnsubscribedShouldNotReceiveMessage()
    {
        int count = 0;
        var testSubscriber = new TestSubscriber();
        _bus.Subscribe<int>(testSubscriber, _ => { count++; });
        _bus.Subscribe<string>(testSubscriber, _ => { count++; });

        _bus.Publish(0);
        _bus.Publish("Test");

        _bus.Unsubscribe(testSubscriber);

        _bus.Publish(1);
        _bus.Publish("Second Test");

        Assert.AreEqual(2, count);
    }

    [Test]
    public void Publish_ListenTopicHierarchy()
    {
        int starred = 0;
        int baseCount = 0;
        int total = 0;

        _bus.Subscribe(new object(), (StarredMessage m) => starred++);
        _bus.Subscribe(new object(), (IBaseMessage m) => baseCount++);
        _bus.Subscribe(new object(), (object m) => total++);

        _bus.Publish(new BaseMessage());
        _bus.Publish(new StarredMessage());
        _bus.Publish(new LabelledMessage());
        _bus.Publish(new object());

        
        Assert.AreEqual(4, total);
        Assert.AreEqual(3, baseCount);
        Assert.AreEqual(1, starred);
    }


    [Test]
    [Ignore("TODO")]
    public void Publish_PruneGCSubscribers()
    {
        var aliveSubscriber = new object();
        int collectedLastValue = 0;
        int aliveLastValue = 0;

        var sb = _bus.Subscribe(_disposableSubscriber, (int v) => { collectedLastValue = v; });
        _bus.Subscribe(aliveSubscriber, (int v) => { aliveLastValue = v; });

        _bus.Publish(3);

        _disposableSubscriber = null;

        GC.Collect();
        GC.WaitForPendingFinalizers();

        _bus.Publish(10);

        Assert.AreEqual(3, collectedLastValue);
        Assert.AreEqual(10, aliveSubscriber);
    }

    [Test]
    public void Publish_SubscribeMany()
    {
        var rangeCount = 50000;
        Parallel.ForEach(Enumerable.Range(1, rangeCount), x => 
            _bus.Subscribe(new object(), (int _) => { }));
        
        Assert.AreEqual(rangeCount, _bus.SubscribersCount);
    }

    class BaseMessage :IBaseMessage
    {
        public string Text { get; set; }
    }
    
    class StarredMessage :BaseMessage,IBaseMessage
    {
        public string Text { get; set; }
        public bool Star { get; set; }
    }

    interface IBaseMessage
    {
        public string Text { get; set; }
    }

    class LabelledMessage : IBaseMessage
    {
        public string Label { get; set; }
        public string Text { get; set; }
    }


    class TestSubscriber
    {
    }
}