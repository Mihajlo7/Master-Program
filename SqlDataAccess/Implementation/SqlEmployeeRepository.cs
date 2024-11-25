using Core.Models.Exp3;
using DataAccess;
using Microsoft.Data.SqlClient;
using SqlDataAccess.Helpers;
using SqlDataAccess.Queries.Exp1;
using SqlDataAccess.Queries.Exp3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlDataAccess.Implementation
{
    public sealed class SqlEmployeeRepository :SqlRepository, IEmployeeRepository
    {
        public SqlEmployeeRepository() : base("exp1_db") { }
        public SqlEmployeeRepository(string database) : base(database) { }

        public void ExecuteCreationAdditional()
        {
            throw new NotImplementedException();
        }

        public void ExecuteCreationTable()
        {
            string[] statements = GenerateQueriesFromQuery(Experiment3Sql.Tables);

            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            foreach (var statement in statements)
            {
                var command = new SqlCommand(statement, connection);
                command.ExecuteNonQuery();
            }
        }

        public void InsertBulkManager(List<ManagerModel> managers)
        {
            throw new NotImplementedException();
        }

        public void InsertBulkSoftwareDeveloper(List<SoftwareDeveloperModel> softwareDevelopers)
        {
            throw new NotImplementedException();
        }

        public void InsertManager(ManagerModel manager)
        {
            string employeeQuery = GenerateQueriesFromQuery(Experiment3Sql.Insert)[0];
            string managerQuery = GenerateQueriesFromQuery(Experiment3Sql.Insert)[3];

            using var connection = new SqlConnection(_connectionString);
            var commandEmp= new SqlCommand(employeeQuery, connection);
            var commandManager = new SqlCommand(managerQuery, connection);

            commandManager.ToCommandManagerEmployeer(manager);
            commandEmp.ToCommandManagerEmployeer(manager);

            connection.Open();
            commandEmp.ExecuteNonQuery();
            commandManager.ExecuteNonQuery();

        }

        public void InsertManyManager(List<ManagerModel> managers)
        {
            throw new NotImplementedException();
        }

        public void InsertManySoftwareDeveloper(List<SoftwareDeveloperModel> softwareDevelopers)
        {
            throw new NotImplementedException();
        }

        public void InsertSoftwareDeveloper(SoftwareDeveloperModel softwareDeveloper)
        {
            string employeeQuery = GenerateQueriesFromQuery(Experiment3Sql.Insert)[0];
            string developerQuery = GenerateQueriesFromQuery(Experiment3Sql.Insert)[1];
            string softwareDeveloperQuery = GenerateQueriesFromQuery(Experiment3Sql.Insert)[4];

            using var connection = new SqlConnection(_connectionString);
            var commandEmp= new SqlCommand(employeeQuery,connection);
            var commandDev= new SqlCommand(developerQuery,connection);
            var commandSoftDev= new SqlCommand(softwareDeveloperQuery,connection);

            commandEmp.ToCommandSoftwareDeveloper(softwareDeveloper);
            commandDev.ToCommandSoftwareDeveloper(softwareDeveloper);
            commandSoftDev.ToCommandSoftwareDeveloper(softwareDeveloper);

            commandEmp.ExecuteNonQuery();
            commandDev.ExecuteNonQuery();
            commandSoftDev.ExecuteNonQuery();
        }
    }
}
