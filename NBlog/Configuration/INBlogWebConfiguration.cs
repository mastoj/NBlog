using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NBlog.Configuration
{
    public interface INBlogWebConfiguration
    {
        string Environment { get; }
        string MongoHQUrl { get; }
        bool IsProd { get; }
    }
}