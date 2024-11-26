using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

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

        protected void InsertBulkPriv(string tableName,DataTable table)
        {
            if (table == null)
            {
                throw new Exception("DataTable is null!");
            }

            using var bulkCopy = new SqlBulkCopy(_connectionString);
            bulkCopy.DestinationTableName = tableName;

            foreach(DataColumn column in table.Columns)
            {
                bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
            }
            bulkCopy.WriteToServer(table);
        }
    }
}
