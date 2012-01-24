namespace NBlog.Domain.Infrastructure
{
    public interface IHandle<T> where T : class
    {
        void Handle(T command);
    }
}