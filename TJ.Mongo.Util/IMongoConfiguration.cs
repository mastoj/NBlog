namespace TJ.Mongo.Util
{
    public interface IMongoConfiguration
    {
        string DatabaseName { get; set; }
        string Url { get; set; }
    }
}
