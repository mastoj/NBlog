namespace NBlog.Configuration
{
    public class NBlogMongoConfiguration //: MongoConfiguration
    {
        public INBlogWebConfiguration InBlogWebConfiguration { get; set; }

        public NBlogMongoConfiguration(INBlogWebConfiguration inBlogWebConfiguration)
        {
            InBlogWebConfiguration = inBlogWebConfiguration;
        }

//        private string _connectionString;
//        public override string ConnectionString
//        {
//            get
//            {
//                _connectionString = _connectionString ?? NBlogWebConfiguration.MongoHQUrl;
////                _connectionString = _connectionString.IsNullOrEmpty() ? base.ConnectionString : _connectionString;
//                return _connectionString;
//            }
//        }
    }
}