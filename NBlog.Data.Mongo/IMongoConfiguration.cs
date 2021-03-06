﻿using MongoDB.Driver;

namespace NBlog.Domain.Mongo
{
    public interface IMongoConfiguration
    {
        //string ConnectionString { get; }
        //string Options { get; }
        string DatabaseName { get; set; }
        string Url { get; set; }
    }
}
