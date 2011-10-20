using System;
using NBlog.Data;

namespace NBlog.Data
{
    public abstract class Entity : IEntity
    {
        public Guid Id { get; set; }
    }
}
