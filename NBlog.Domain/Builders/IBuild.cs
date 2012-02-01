namespace NBlog.Domain.Builders
{
    public interface IBuild<T>
    {
        T Build();
    }
}
