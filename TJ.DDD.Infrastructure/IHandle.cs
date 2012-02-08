using TJ.DDD.Infrastructure.Command;

namespace TJ.DDD.Infrastructure
{
    public interface IHandle<T> where T : ICommand
    {
        void Execute(T command);
    }
}