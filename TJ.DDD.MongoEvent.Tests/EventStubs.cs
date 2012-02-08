using TJ.DDD.Infrastructure;
using TJ.DDD.Infrastructure.Event;

namespace TJ.DDD.MongoEvent.Tests
{
    public class MyEvent : DomainEventBase
    {
        public string SomeText { get; set; }
    }

    public class MyEvent2 : DomainEventBase
    {
        public string SomeText2 { get; set; }
    }
}
