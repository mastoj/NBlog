using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NBlog.Configuration
{
    public interface INBlogConfiguration
    {
        string Environment { get; }
        bool IsSpecFlowTest { get; }
        string SpecMongoConnection { get; }
        string MongoHQUrl { get; }
    }
}