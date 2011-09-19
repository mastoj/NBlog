namespace NBlog.Data.Mongo
{
    public class MongoConfiguration : IMongoConfiguration
    {
        private string _mongoConnection;
        public virtual string ConnectionString
        {
            get
            {
                _mongoConnection = _mongoConnection ?? "mongodb://localhost/BlogDB"; 
                return  _mongoConnection;
            }
            set { _mongoConnection = value; }
        }

        private string _options;
        public virtual string Options
        {
            get 
            {
                _options = _options ?? "strict=false";
                return _options;
            }
            set { _options = value; }
        }
    }
}