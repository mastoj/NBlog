using TJ.DDD.Infrastructure.Command;

namespace TJ.DDD.Infrastructure
{
    public interface IHandle<T>
    {
        void Handle(T thingToHandle);
    }
}