using System.Configuration;
using TJ.Extensions;

namespace NBlog.Configuration
{
    public class NBlogConfiguration : INBlogConfiguration
    {
        private string _environment;
        public string Environment
        {
            get { 
                _environment = _environment ?? GetAppSetting("Environment");
                return _environment;
            }
        }

        private bool? _isSpecFlowTest;
        public bool IsSpecFlowTest
        {
            get
            {
                if (_isSpecFlowTest.HasValue.IsFalse())
                {
                    _isSpecFlowTest = false;
                    var appSetting = GetAppSetting("IsSpecFlowTest") ?? string.Empty;
                    bool isSpecFlowTest;
                    if (bool.TryParse(appSetting, out isSpecFlowTest))
                    {
                        _isSpecFlowTest = isSpecFlowTest;
                    }
                }
                return _isSpecFlowTest.Value;
            }
        }

        private string _specMongoConnection;
        public string SpecMongoConnection
        {
            get 
            { 
                _specMongoConnection = _specMongoConnection ?? GetAppSetting("SpecMongoConnection");
                return _specMongoConnection;
            }
        }

        private string _mongoHQUrl;
        public string MongoHQUrl
        {
            get
            {
                _mongoHQUrl = _mongoHQUrl ?? GetAppSetting("MONGOHQ_URL");
                return _mongoHQUrl;
            }
        }

        private string GetAppSetting(string appSettingName)
        {
            return ConfigurationManager.AppSettings[appSettingName];
        }
    }
}