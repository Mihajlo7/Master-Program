using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlDataAccess
{
    public abstract class SqlRepository
    {
        protected string _database {  get; private set; }
        protected string _connectionString { get; private set; }

        protected SqlRepository(string database)
        {
            _database = database;
            _connectionString = $"Data Source=.;Initial Catalog={_database};Integrated Security=True;TrustServerCertificate=True;";
        }
        

        protected string[] GenerateQueriesFromQuery(string query) 
        {
            string [] queries = query.Split(';')
                .Select(line=>line.Trim())
                .Where(line=>!string.IsNullOrWhiteSpace(line))
                .ToArray();
            return queries;
        }
    }
}
