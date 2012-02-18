using System.Collections.Generic;

namespace TJ.DDD.Infrastructure.Respositories
{
    public interface IDomainRepository<TAggregate> where TAggregate : AggregateRoot
    {
        void Insert(TAggregate aggregate);
    }
}