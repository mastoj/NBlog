using System;
using System.Collections.Generic;
using TJ.DDD.Infrastructure.Event;
using TJ.DDD.Infrastructure.Exceptions;
using TJ.DDD.Infrastructure.Messaging;
using TJ.Extensions;

namespace TJ.DDD.Infrastructure.Command
{
    public class InMemoryBus : ISendCommand, IPublishEvent
    {
        private readonly IUnitOfWork _unitOfWork;
        private Dictionary<Type, List<Action<IMessage>>> _messageRoutes;

        public InMemoryBus(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _messageRoutes = new Dictionary<Type, List<Action<IMessage>>>();
        }

        public void Register<TMessage>(Action<TMessage> route) where TMessage : class, IMessage
        {
            List<Action<IMessage>> routes;
            var type = typeof (TMessage);
            if (_messageRoutes.TryGetValue(type, out routes).IsFalse())
            {
                routes = new List<Action<IMessage>>();
                _messageRoutes.Add(type, routes);
            }
            routes.Add((y) => route(y as TMessage));
        }

        public void Send<TCommand>(TCommand command) where TCommand : class, ICommand
        {
            var commandType = command.GetType();
            List<Action<IMessage>> handlers;
            if (_messageRoutes.TryGetValue(commandType, out handlers))
            {
                foreach (var handler in handlers)
                {
                    handler(command);                    
                }
                _unitOfWork.Commit();
            }
            else
            {
                throw new UnregisteredCommandException("No command handler registered for command type: " + commandType);
            }
        }

        public void Publish<TEvent>(TEvent @event) where TEvent : class, IDomainEvent
        {
            var eventType = @event.GetType();
            List<Action<IMessage>> eventHandlers;
            if (_messageRoutes.TryGetValue(eventType, out eventHandlers).IsFalse())
            {
                return;
            }
            foreach (var eventHandler in eventHandlers)
            {
                eventHandler(@event);
            }
        }
    }
}