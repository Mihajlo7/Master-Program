﻿using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDataAccess
{
    public abstract class MongoRepository
    {
        private readonly string _mongoUrl;
        private readonly string _databaseName;

        protected MongoClient _client {  get;private set; }
        protected IMongoDatabase _database { get; private set; }
        

        protected MongoRepository(string database)
        {
            _mongoUrl="mongodb+srv://pavmihajlo:Master@cluster0.a9sgb.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0";
            //_mongoUrl = "mongodb://localhost:27017";
            _databaseName=database;
            var settings = MongoClientSettings.FromUrl(new MongoUrl(_mongoUrl));
            settings.MaxConnectionPoolSize = 200; // Povećajte limit konekcija
            settings.ConnectTimeout = TimeSpan.FromHours(2);
            settings.SocketTimeout = TimeSpan.FromHours(2);
            settings.ServerSelectionTimeout = TimeSpan.FromHours(3);
            _client = new MongoClient(settings);
            _database = _client.GetDatabase(_databaseName);
        }


    }
}
