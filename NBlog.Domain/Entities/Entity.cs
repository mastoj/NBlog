using System;
using System.Runtime.Serialization;
using NBlog.Domain;

namespace NBlog.Domain
{
    [Serializable]
    public abstract class Entity : IEntity
    {
        public Guid Id { get; set; }
    }
}
