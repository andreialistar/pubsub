using Pubsub.Contracts;
using Pubsub.Models;

namespace Pubsub.OrderSubscribers;

public class LogSubscriber
{
    private static object _lock = new object(); 

    public LogSubscriber()
    {
    }

    public void LogToConsole(IOrderMessage message)
    {
        lock (_lock)
        {
            Console.WriteLine(message.ToString());
        }
    }
}