using System;
using System.Collections.Generic;
using NBlog.Domain.Infrastructure;
using TJ.Extensions;

namespace NBlog.Domain.Command
{
    public class CommandHandleManager : ICommandHandleManager
    {
        private static readonly Dictionary<Type, object> commandHandlers = new Dictionary<Type, object>(); 

        public void RegisterCommandHandler<T>(IHandle<T> commandHandler) where T : class
        {
            var type = typeof (T);
            if (commandHandlers.ContainsKey(type).IsTrue())
            {
                throw new ArgumentException("Handler already exist for type {0}", type.ToString());
            }
            commandHandlers.Add(type, commandHandler);
        }

        public void UnRegisterCommandHandler<T>() where T : class
        {
            var type = typeof(T);
            if (commandHandlers.ContainsKey(type).IsFalse())
            {
                throw new ArgumentException("Handler doesn't exist for type {0}", type.ToString());
            }
            commandHandlers.Remove(type);
        }

        public void Handle<T>(T command) where T : class
        {
            IHandle<T> commandHandler = GetCommandHandler<T>();
            commandHandler.Handle(command);
        }

        private IHandle<T> GetCommandHandler<T>() where T : class
        {
            var type = typeof (T);
            if (commandHandlers.ContainsKey(type))
            {
                return commandHandlers[type] as IHandle<T>;
            }
            throw new ArgumentException("Handler already exist for type {0}", type.ToString());
        }
    }
}