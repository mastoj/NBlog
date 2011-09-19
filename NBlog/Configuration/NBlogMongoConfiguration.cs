using System.Configuration;
using NBlog.Data.Mongo;
using TJ.Extensions;

namespace NBlog.Configuration
{
    public class NBlogMongoConfiguration : MongoConfiguration
    {
        private string _connectionString;
        public override string ConnectionString
        {
            get
            {
                _connectionString = _connectionString ?? ConfigurationManager.AppSettings["MONGOHQ_URL"];
                _connectionString = _connectionString.IsNullOrEmpty() ? base.ConnectionString : _connectionString;
                return _connectionString;
            }
        }
    }
}