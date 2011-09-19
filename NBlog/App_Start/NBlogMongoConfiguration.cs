using System.Configuration;
using NBlog.Data.Mongo;

namespace NBlog.App_Start
{
    public class NBlogMongoConfiguration : MongoConfiguration
    {
        private string _connectionString;
        public override string ConnectionString
        {
            get
            {
                _connectionString = _connectionString ?? ConfigurationManager.AppSettings["MONGOHQ_URL"];
                return _connectionString;
            }
        }
    }
}