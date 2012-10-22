using FluentValidation;

namespace NBlog.Domain.Commands.Validators
{
    public class CreateBlogCommandValidator : AbstractValidator<CreateBlogCommand>
    {
        public CreateBlogCommandValidator()
        {
            RuleFor(y => y.BlogTitle).NotEmpty().WithMessage("No no no, you must give your blog a title");
        }
    }
}