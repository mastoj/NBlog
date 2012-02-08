namespace TJ.DDD.Infrastructure.Command
{
    public interface ICommandManager
    {
        void Execute<TCommand>(TCommand command) where TCommand : ICommand;
    }
}