using MongoDB.Driver;

namespace NBlog.Domain.Mongo
{
    public class MongoConfiguration : IMongoConfiguration
    {
        //private string _options;
        //public virtual string Options
        //{
        //    get 
        //    {
        //        _options = _options ?? "strict=false";
        //        return _options;
        //    }
        //    set { _options = value; }
        //}

        private string _databaseName;
        public string DatabaseName
        {
            get
            {
                _databaseName = _databaseName ?? "NBlogDB";
                return _databaseName;
            }
            set { _databaseName = value; }
        }

        private string _url;
        public string Url
        {
            get
            {
                _url = _url ?? "mongodb://localhost";
                return _url;
            }
            set { _url = value; }
        }
    }
}