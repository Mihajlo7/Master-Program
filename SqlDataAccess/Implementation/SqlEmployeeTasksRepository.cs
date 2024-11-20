using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using Microsoft.Data.SqlClient;
using SqlDataAccess.Queries.Exp1;

namespace SqlDataAccess.Implementation
{
    public class SqlEmployeeTasksRepository : SqlRepository, IEmployeeTasksRepository
    {
        public SqlEmployeeTasksRepository():base("exp1_db"){ }
        public SqlEmployeeTasksRepository(string database) : base(database) { }


        public override void ExecuteCreationAdditional()
        {
            throw new NotImplementedException();
        }

        public override void ExecuteCreationTable()
        {
            string[] statements = GenerateQueriesFromQuery(Experiment1Sql.Tables);

            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            foreach (var statement in statements)
            {
                var command= new SqlCommand(statement,connection);
                command.ExecuteNonQuery();
            }
        }

        public void InsertBulk()
        {
            throw new NotImplementedException();
        }

        public int InsertMany()
        {
            throw new NotImplementedException();
        }

        public void InsertOne()
        {
            string[] statements = GenerateQueriesFromQuery(Experiment1Sql.Insert);
        }
    }
}
