namespace NBlog.Data.Mongo
{
    public interface IMongoConfiguration
    {
        string ConnectionString { get; }
        string Options { get; }
    }
}
