using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HybridDataAccess.DataSerializator;
using HybridDataAccess.Queries;
using Microsoft.Data.SqlClient;

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

        

        protected string[] GenerateQueriesFromQuery(string query)
        {
            string[] queries = query.Split(';')
                .Select(line => line.Trim())
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .ToArray();
            return queries;
        }
        protected string[] GenerateProceduresFromQuery(string query)
        {
            string[] queries = query.Split('?')
                .Select(line => line.Trim())
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .ToArray();
            return queries;
        }

        protected void InsertBulkPriv(string tableName, DataTable table)
        {
            if (table == null)
            {
                throw new Exception("DataTable is null!");
            }

            using var bulkCopy = new SqlBulkCopy(_connectionString);
            bulkCopy.DestinationTableName = tableName;

            foreach (DataColumn column in table.Columns)
            {
                bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
            }
            bulkCopy.WriteToServer(table);
        }

        protected void ExecuteCreateTablePriv(string tables,string procedures=null)
        {
            string[] statements = GenerateQueriesFromQuery(tables);
            if(procedures != null)
            {
                statements.Concat(GenerateProceduresFromQuery(procedures));
            }

            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            foreach (var statement in statements)
            {
                var command = new SqlCommand(statement, connection);
                command.ExecuteNonQuery();
            }
        }
    }
}
