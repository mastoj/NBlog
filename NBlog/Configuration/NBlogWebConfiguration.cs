using System.Configuration;
using TJ.Extensions;

namespace NBlog.Configuration
{
    public class NBlogWebConfiguration : INBlogWebConfiguration
    {
        private string _environment;
        public string Environment
        {
            get { 
                _environment = _environment ?? GetAppSetting("Environment");
                return _environment;
            }
        }

        private bool? _isProd;
        public bool IsProd
        {
            get { 
                _isProd = _isProd ?? Environment == "Release";
                return _isProd.Value;
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