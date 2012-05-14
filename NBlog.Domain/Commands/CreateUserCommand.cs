using System;
using FluentValidation.Attributes;
using NBlog.Domain.Commands.Validators;
using TJ.CQRS.Messaging;

namespace NBlog.Domain.Commands
{
    [Validator(typeof(CreateUserCommandValidator))]
    public class CreateUserCommand : Command
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public CreateUserCommand() : base(Guid.NewGuid())
        {
            
        }
    }
}