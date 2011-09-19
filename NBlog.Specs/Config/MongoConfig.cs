using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NBlog.Data.Mongo;

namespace NBlog.Specs.Config
{
    public class MongoConfig : MongoConfiguration
    {
        public override string ConnectionString
        {
            get { 
                base.ConnectionString = "mongodb://localhost/spectest";
                return base.ConnectionString;
            }
            set
            {
                base.ConnectionString = value;
            }
        }
    }
}
