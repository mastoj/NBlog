using System;
using NBlog.Domain.Commands;
using NBlog.Domain.Commands.Validators;
using NBlog.Domain.Entities;
using TJ.CQRS;
using TJ.CQRS.Respositories;

namespace NBlog.Domain.CommandHandlers
{
    public class UserCommandHandlers : IHandle<CreateUserCommand>
    {
        private readonly IDomainRepository<User> _userRepository;

        public UserCommandHandlers(IDomainRepositoryFactory repositoryFactory)
        {
            _userRepository = repositoryFactory.GetDomainRepository<User>();
        }

        public void Handle(CreateUserCommand creatUserCommand)
        {
            var createValidator = new CreateUserCommandValidator();
            var validationResult = createValidator.Validate(creatUserCommand);
            if (validationResult.IsValid)
            {
                var user = User.Create(creatUserCommand.AuthenticationId, creatUserCommand.Name, creatUserCommand.Email,
                                       creatUserCommand.AggregateId);
                _userRepository.Insert(user);
            }
            else
            {
                string errorMessage = "Invalid arguments: " + Environment.NewLine;
                foreach (var validationFailure in validationResult.Errors)
                {
                    errorMessage += validationFailure.PropertyName + ": " + validationFailure.ErrorMessage +
                                    Environment.NewLine;
                }
                throw new ArgumentException(errorMessage);
            }
        }
    }
}