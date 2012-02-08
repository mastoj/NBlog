using System;
using System.Collections.Generic;
using TJ.DDD.Infrastructure.Exceptions;

namespace TJ.DDD.Infrastructure.Command
{
    public class CommandManager : ICommandManager
    {
        private IDictionary<Type, object> _commandHandlers;

        public CommandManager(ICommandRepository commandRepository)
        {
            _commandHandlers = commandRepository.GetCommandHandlers();
        }

        public void Execute<TCommand>(TCommand command) where TCommand : ICommand
        {
            var commandType = command.GetType();
            if (_commandHandlers.ContainsKey(commandType))
            {
                var commandHandler = _commandHandlers[commandType] as IHandle<TCommand>;
                commandHandler.Execute(command);
            }
            else
            {
                throw new UnregisteredCommandException("No command handler registered for command type: " + commandType);
            }
        }
    }
}