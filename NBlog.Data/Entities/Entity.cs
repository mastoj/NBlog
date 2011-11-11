using System;
using NBlog.Domain;

namespace NBlog.Domain
{
    public abstract class Entity : IEntity
    {
        public Guid Id { get; set; }
    }
}
