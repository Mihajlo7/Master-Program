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

        public List<EmployeeModel3> GetAllEmployees()
        {
            string query = GenerateQueriesFromQuery(Experiment3Sql.Select)[0];

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);

            connection.Open();
            using var reader= command.ExecuteReader();
            return reader.GetEmployees3();
        }

        public List<ManagerModel> GetAllManagers()
        {
            string query = GenerateQueriesFromQuery(Experiment3Sql.Select)[1];

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);

            connection.Open();
            using var reader = command.ExecuteReader();
            return reader.GetManagers();
        }

        public List<ManagerAggModel> GetAllMethodsWithCountManagers()
        {
            string query = GenerateQueriesFromQuery(Experiment3Sql.Select)[7];

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);

            connection.Open();
            using var reader = command.ExecuteReader();
            var result= new List<ManagerAggModel>();
            while (reader.Read())
            {
                var res= new ManagerAggModel()
                {
                    Method = reader.GetString(0),
                    ManagerCount=reader.GetInt32(1)
                };
                result.Add(res);
            }
            return result;
        }

        public List<SoftwareDeveloperModel> GetAllSoftwareDevelopers()
        {
            string query = GenerateQueriesFromQuery(Experiment3Sql.Select)[2];

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);

            connection.Open();
            using var reader = command.ExecuteReader();
            return reader.GetSoftwareDevelopers();
        }

        public EmployeeModel3 GetEmployeeById(long id)
        {
            string query = GenerateQueriesFromQuery(Experiment3Sql.Select)[3];

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@EmployeeId", id);
            connection.Open();
            using var reader = command.ExecuteReader();
            return reader.GetEmployees3().First();
        }

        public List<ManagerModel> GetManagersYoungerThan30AndAgileMethodSorted()
        {
            string query = GenerateQueriesFromQuery(Experiment3Sql.Select)[4];

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);

            connection.Open();
            using var reader = command.ExecuteReader();
            return reader.GetManagers();
        }

        public List<SoftwareDevelopersAggModel> GetProgrammingLanguagesCountDevelopersAndAvgYearsExp()
        {
            string query = GenerateQueriesFromQuery(Experiment3Sql.Select)[8];

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);

            connection.Open();
            using var reader = command.ExecuteReader();
            var list = new List<SoftwareDevelopersAggModel>();
            while (reader.Read())
            {
                var result = new SoftwareDevelopersAggModel
                {
                    ProgrammingLanguage = reader.GetString(reader.GetOrdinal("programmingLanguage")), // Preporuka: Korišćenje naziva kolona
                    DeveloperCount = reader.GetInt32(reader.GetOrdinal("DeveloperCount")),
                    AvgExperience = reader.GetInt32(reader.GetOrdinal("AvgExperience"))
                };
                list.Add(result);
            }
            return list;
        }

        public List<SoftwareDeveloperModel> GetSoftwareDevelopersOlderThan25AndMediorAndJavaAndC()
        {
            string query = GenerateQueriesFromQuery(Experiment3Sql.Select)[6];

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);

            connection.Open();
            using var reader = command.ExecuteReader();
            return reader.GetSoftwareDevelopers();
        }

        public List<SoftwareDeveloperModel> GetSoftwareDevelopersRemoteAndUseVisualStudio()
        {
            string query = GenerateQueriesFromQuery(Experiment3Sql.Select)[5];

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);

            connection.Open();
            using var reader = command.ExecuteReader();
            return reader.GetSoftwareDevelopers();
        }

        public void InsertBulkManager(List<ManagerModel> managers)
        {
            using var bulkCopy = new SqlBulkCopy(_connectionString);
            bulkCopy.DestinationTableName = "Employee";
            bulkCopy.ColumnMappings.Add("id", "id");
            bulkCopy.ColumnMappings.Add("firstname", "firstname");
            bulkCopy.ColumnMappings.Add("lastname", "lastname");
            bulkCopy.ColumnMappings.Add("email", "email");
            bulkCopy.ColumnMappings.Add("birthday", "birthday");
            bulkCopy.ColumnMappings.Add("title", "title");
            bulkCopy.ColumnMappings.Add("phone", "phone");
            var employees= managers.Select(m=> (EmployeeModel3) m).ToList();
            bulkCopy.WriteToServer(employees.CreateDataTableFromEmployees());

            using var bulkCopyM= new SqlBulkCopy(_connectionString);
            bulkCopyM.DestinationTableName = "Manager";
            bulkCopyM.ColumnMappings.Add("id", "id");
            bulkCopyM.ColumnMappings.Add("department", "department");
            bulkCopyM.ColumnMappings.Add("realisedProject", "realisedProject");
            bulkCopyM.ColumnMappings.Add("method", "method");

            bulkCopyM.WriteToServer(managers.CreateDataTableFromManagaers());
        }

        public void InsertBulkSoftwareDeveloper(List<SoftwareDeveloperModel> softwareDevelopers)
        {
            var employees=softwareDevelopers.Select(s=>(EmployeeModel3)s).ToList();
            var developers = softwareDevelopers.Select(d=>(DeveloperModel)d).ToList();

            InsertBulkPriv("Employee", employees.CreateDataTableFromEmployees());
            InsertBulkPriv("Developer", developers.CreateDataTableFromDevelopers());
            InsertBulkPriv("SoftwareDeveloper",softwareDevelopers.CreateDataTableFromSoftwareDevelopers());
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
            string employeeQuery = GenerateQueriesFromQuery(Experiment3Sql.Insert)[0];
            string managerQuery = GenerateQueriesFromQuery(Experiment3Sql.Insert)[3];

            using var connection = new SqlConnection(_connectionString);
            var commandEmp = new SqlCommand(employeeQuery, connection);
            var commandManager = new SqlCommand(managerQuery, connection);

            connection.Open();
            foreach (var manager in managers)
            {
                commandManager.ToCommandManagerEmployeer(manager);
                commandEmp.ToCommandManagerEmployeer(manager);

                commandEmp.ExecuteNonQuery();
                commandManager.ExecuteNonQuery();
            }
        }

        public void InsertManySoftwareDeveloper(List<SoftwareDeveloperModel> softwareDevelopers)
        {
            string employeeQuery = GenerateQueriesFromQuery(Experiment3Sql.Insert)[0];
            string developerQuery = GenerateQueriesFromQuery(Experiment3Sql.Insert)[1];
            string softwareDeveloperQuery = GenerateQueriesFromQuery(Experiment3Sql.Insert)[4];

            using var connection = new SqlConnection(_connectionString);
            var commandEmp = new SqlCommand(employeeQuery, connection);
            var commandDev = new SqlCommand(developerQuery, connection);
            var commandSoftDev = new SqlCommand(softwareDeveloperQuery, connection);

            connection.Open();

            foreach(var softwareDeveloper in softwareDevelopers)
            {
                commandEmp.ToCommandSoftwareDeveloper(softwareDeveloper);
                commandDev.ToCommandSoftwareDeveloper(softwareDeveloper);
                commandSoftDev.ToCommandSoftwareDeveloper(softwareDeveloper);

                commandEmp.ExecuteNonQuery();
                commandDev.ExecuteNonQuery();
                commandSoftDev.ExecuteNonQuery();
            }
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

            connection.Open();
            commandEmp.ExecuteNonQuery();
            commandDev.ExecuteNonQuery();
            commandSoftDev.ExecuteNonQuery();
        }
    }
}
