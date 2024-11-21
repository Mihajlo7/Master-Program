using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HybridDataAccess.DataSerializator;

namespace HybridDataAccess
{
    public abstract class HybridRepository
    {
        protected string _database { get; private set; }
        protected string _connectionString { get; private set; }
        protected JsonHandler _jsonHandler { get;private set; }

        public HybridRepository(string database)
        {
            _database = database;
            _connectionString = $"Data Source=.;Initial Catalog={_database};Integrated Security=True;TrustServerCertificate=True;";
            _jsonHandler = new JsonHandler();
        }

        public abstract void ExecuteCreationTable();
        public abstract void ExecuteCreationAdditional();

        protected string[] GenerateQueriesFromQuery(string query)
        {
            string[] queries = query.Split(';')
                .Select(line => line.Trim())
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .ToArray();
            return queries;
        }
    }
}
