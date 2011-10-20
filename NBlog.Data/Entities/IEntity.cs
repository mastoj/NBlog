using System;

namespace NBlog.Data
{
    public interface IEntity
    {
        Guid Id { get; set; }
    }
}