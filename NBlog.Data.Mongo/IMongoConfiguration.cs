namespace NBlog.Domain.Mongo
{
    public interface IMongoConfiguration
    {
        string ConnectionString { get; }
        string Options { get; }
    }
}
