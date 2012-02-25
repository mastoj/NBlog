using System;
using System.Collections.Generic;
using System.Linq;
using TJ.DDD.Infrastructure.Respositories;

namespace NBlog.Domain.Tests.Stubs
{
    public class StubPostRepository : IDomainRepository<Entities.Post>
    {
        private List<Entities.Post> _posts = new List<Entities.Post>();
        public List<Entities.Post> Posts
        {
            get { return _posts; }
        }

        public Entities.Post Get(Guid aggregateId)
        {
            return _posts.FirstOrDefault(y => y.AggregateId == aggregateId);
        }

        public void Insert(Entities.Post aggregate)
        {
            _posts.Add(aggregate);
        }
    }
}
