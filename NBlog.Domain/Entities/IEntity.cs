using System;

namespace NBlog.Domain
{
    public interface IEntity
    {
        Guid Id { get; set; }
    }
}