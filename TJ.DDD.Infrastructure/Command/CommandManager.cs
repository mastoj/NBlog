using System;
using System.Collections.Generic;
using TJ.DDD.Infrastructure.Exceptions;

namespace TJ.DDD.Infrastructure.Command
{
    public class CommandManager : ICommandManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private IDictionary<Type, object> _commandHandlers;

        public CommandManager(ICommandProvider commandProvider, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _commandHandlers = commandProvider.GetCommandHandlers();
        }

        public void Execute<TCommand>(TCommand command) where TCommand : class, ICommand
        {
            var commandType = command.GetType();
            if (_commandHandlers.ContainsKey(commandType))
            {
                var commandHandler = _commandHandlers[commandType] as IHandle<TCommand>;
                commandHandler.Execute(command);
                _unitOfWork.Commit();
            }
            else
            {
                throw new UnregisteredCommandException("No command handler registered for command type: " + commandType);
            }
        }
    }
}