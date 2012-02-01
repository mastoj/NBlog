namespace NBlog.Domain.Event
{
    public interface IHandle<T> where T : class
    {
        void Handle(T command);
    }
}