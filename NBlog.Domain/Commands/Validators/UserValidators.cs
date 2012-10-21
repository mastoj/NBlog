using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentValidation;

namespace NBlog.Domain.Commands.Validators
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(y => y.AuthenticationId).NotEmpty().WithMessage("Open id cannot be empty");
            RuleFor(y => y.Name).NotEmpty().WithMessage("Name cannot be empty");
            RuleFor(y => y.Email).NotEmpty().WithMessage("Email cannot be empty").EmailAddress().WithMessage(
                "Input is not an email address");
        }
    }
}
