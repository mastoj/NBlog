using System.Configuration;
using NBlog.Domain.Mongo;
using TJ.Extensions;

namespace NBlog.Configuration
{
    public class NBlogMongoConfiguration : MongoConfiguration
    {
        public INBlogConfiguration NBlogConfiguration { get; set; }

        public NBlogMongoConfiguration(INBlogConfiguration nBlogConfiguration)
        {
            NBlogConfiguration = nBlogConfiguration;
        }

        private string _connectionString;
        public override string ConnectionString
        {
            get
            {
                if (NBlogConfiguration.IsProd.IsFalse() && NBlogConfiguration.IsSpecFlowTest.IsTrue())
                {
                    _connectionString = NBlogConfiguration.SpecMongoConnection;
                }
                else
                {
                    _connectionString = _connectionString ?? NBlogConfiguration.MongoHQUrl;
                    _connectionString = _connectionString.IsNullOrEmpty() ? base.ConnectionString : _connectionString;                    
                }
                return _connectionString;
            }
        }
    }
}