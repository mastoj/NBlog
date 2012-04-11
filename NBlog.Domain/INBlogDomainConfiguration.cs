using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NBlog.Domain.CommandHandlers;
using NBlog.Domain.Commands;
using NBlog.Domain.Entities;
using TJ.CQRS.Messaging;
using TJ.CQRS.Respositories;

namespace NBlog.Domain
{
    public interface INBlogDomainConfiguration
    {
        void ConfigureMessageRouter(IMessageRouter messageRouter);
    }
}
