using System;
using System.Collections.Generic;
using TJ.CQRS;
using TJ.CQRS.Respositories;

namespace NBlog.Domain.Tests.CommandHandlers
{
    public class DomainRepositoryStub<T> : IDomainRepository<T> where T : AggregateRoot
    {
        private Dictionary<Guid, T> _aggregateDictionary;

        public DomainRepositoryStub()
        {
            _aggregateDictionary = new Dictionary<Guid, T>();
        }

        public T Get(Guid aggregateId)
        {
            T aggregate = default(T);
            var foundAggregate = _aggregateDictionary.TryGetValue(aggregateId, out aggregate);
            return aggregate;
        }

        public void Insert(T aggregate)
        {
            _aggregateDictionary.Add(aggregate.AggregateId, aggregate);
        }
    }
}