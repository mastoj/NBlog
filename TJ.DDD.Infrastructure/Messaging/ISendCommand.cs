namespace TJ.DDD.Infrastructure.Messaging
{
    public interface ISendCommand
    {
        void Send<TCommand>(TCommand command) where TCommand : class, ICommand;
    }
}