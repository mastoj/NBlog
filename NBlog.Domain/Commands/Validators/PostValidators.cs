using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentValidation;

namespace NBlog.Domain.Commands.Validators
{
    public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
    {
        public CreatePostCommandValidator()
        {
            RuleFor(y => y.Content).NotEmpty().WithMessage("Content cannot be empty");
            RuleFor(y => y.Slug).NotEmpty().WithMessage("Slug cannot be empty");
            RuleFor(y => y.Title).NotEmpty().WithMessage("Title cannot be empty");
        }
    }

    public class PublishPostCommandValidator : AbstractValidator<PublishPostCommand>
    {
        public PublishPostCommandValidator()
        {
            RuleFor(y => y.AggregateId).NotEqual(Guid.Empty).WithMessage("Post id cannot be empty");
        }
    }
}
