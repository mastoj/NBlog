using NBlog.Domain.Infrastructure;

namespace NBlog.Domain.Command
{
    public interface ICommandHandleManager
    {
        void RegisterCommandHandler<T>(IHandle<T> commandHandler) where T : class;
        void UnRegisterCommandHandler<T>() where T : class;
        void Handle<T>(T command) where T : class;
    }
}