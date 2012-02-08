using System;
using TJ.DDD.MongoEvent;

namespace NBlog.Data.Mongo.Tests.EventRepository
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
