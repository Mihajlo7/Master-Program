using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models.Exp3;
using DataAccess;
using HybridDataAccess.DataSerializator;
using HybridDataAccess.Helper;
using HybridDataAccess.Queries.Exp3;
using Microsoft.Data.SqlClient;

namespace HybridDataAccess.Implementation
{
    public sealed class HybridEmployeeRepository : HybridRepository,IEmployeeRepository
    {
        public HybridEmployeeRepository(): base("exp_db")
        {
            
        }

        public HybridEmployeeRepository(string database): base(database)
        {
            
        }

        public void ExecuteCreationAdditional()
        {
            throw new NotImplementedException();
        }

        public void ExecuteCreationTable()
        {
            ExecuteCreateTablePriv(Experiment3Hybrid.Tables);
        }

        public List<EmployeeModel3> GetAllEmployees()
        {
            throw new NotImplementedException();
        }

        public List<ManagerModel> GetAllManagers()
        {
            throw new NotImplementedException();
        }

        public List<ManagerAggModel> GetAllMethodsWithCountManagers()
        {
            throw new NotImplementedException();
        }

        public List<SoftwareDeveloperModel> GetAllSoftwareDevelopers()
        {
            throw new NotImplementedException();
        }

        public EmployeeModel3 GetEmployeeById(long id)
        {
            throw new NotImplementedException();
        }

        public List<ManagerModel> GetManagersYoungerThan30AndAgileMethodSorted()
        {
            throw new NotImplementedException();
        }

        public List<SoftwareDevelopersAggModel> GetProgrammingLanguagesCountDevelopersAndAvgYearsExp()
        {
            throw new NotImplementedException();
        }

        public List<SoftwareDeveloperModel> GetSoftwareDevelopersOlderThan25AndMediorAndJavaAndC()
        {
            throw new NotImplementedException();
        }

        public List<SoftwareDeveloperModel> GetSoftwareDevelopersRemoteAndUseVisualStudio()
        {
            throw new NotImplementedException();
        }

        public async void InsertBulkManager(List<ManagerModel> managers)
        {
            var employeeTable = new DataTable("Employee");
            employeeTable.Columns.Add("id", typeof(long));
            employeeTable.Columns.Add("firstname", typeof(string));
            employeeTable.Columns.Add("lastname", typeof(string));
            employeeTable.Columns.Add("email", typeof(string));
            employeeTable.Columns.Add("birthday", typeof(DateTime));
            employeeTable.Columns.Add("title", typeof(string));
            employeeTable.Columns.Add("phone", typeof(string));
            employeeTable.Columns.Add("manager",typeof(string));

            foreach (var manager in managers)
            {
                employeeTable.Rows.Add(
                   manager.Id,
                   manager.FirstName,
                   manager.LastName,
                   manager.Email,
                   manager.BirthDay,
                   manager.Title,
                   manager.Phone,
                   new JsonHandler().SerializeOne(
                       new ManagerModel() { Department=manager.Department,Method=manager.Method,RealisedProject=manager.RealisedProject}));
            }

            InsertBulkPriv("Employee",employeeTable);

        }

        public void InsertBulkSoftwareDeveloper(List<SoftwareDeveloperModel> softwareDevelopers)
        {
            var employeeTable = new DataTable("Employee");
            employeeTable.Columns.Add("id", typeof(long));
            employeeTable.Columns.Add("firstname", typeof(string));
            employeeTable.Columns.Add("lastname", typeof(string));
            employeeTable.Columns.Add("email", typeof(string));
            employeeTable.Columns.Add("birthday", typeof(DateTime));
            employeeTable.Columns.Add("title", typeof(string));
            employeeTable.Columns.Add("phone", typeof(string));
            

            foreach (var manager in softwareDevelopers)
            {
                employeeTable.Rows.Add(
                   manager.Id,
                   manager.FirstName,
                   manager.LastName,
                   manager.Email,
                   manager.BirthDay,
                   manager.Title,
                   manager.Phone);
            }
            var developerTable = new DataTable("Developer");
            developerTable.Columns.Add("id", typeof(long));
            developerTable.Columns.Add("seniority", typeof(string));
            developerTable.Columns.Add("yearsOfExperience", typeof(int));
            developerTable.Columns.Add("isRemote", typeof(bool));
            developerTable.Columns.Add("softwareDeveloper",typeof(string));

            foreach (var developer in softwareDevelopers)
            {
                developerTable.Rows.Add(
                    developer.Id,
                    developer.Seniority,
                    developer.YearsOfExperience,
                    developer.IsRemote,
                    new JsonHandler().SerializeOne(new SoftwareDeveloperModel() { ProgrammingLanguage=developer.ProgrammingLanguage,IDE=developer.IDE,IsFullStack=developer.IsFullStack}));
            }

            InsertBulkPriv("Employee", employeeTable);
            InsertBulkPriv("Developer", developerTable);
        }

        public void InsertManager(ManagerModel manager)
        {
            string query = GenerateQueriesFromQuery(Experiment3Hybrid.Insert)[0];
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            command.ToCommandManagerEmployeerHybrid(manager);

            connection.Open();
            command.ExecuteNonQuery();

        }

        public void InsertManyManager(List<ManagerModel> managers)
        {
            string query = GenerateQueriesFromQuery(Experiment3Hybrid.Insert)[0];
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);

            connection.Open();

            foreach (var manager in managers)
            {
                command.ToCommandManagerEmployeerHybrid(manager);
                command.ExecuteNonQuery();
            }
        }

        public void InsertManySoftwareDeveloper(List<SoftwareDeveloperModel> softwareDevelopers)
        {
            string queryEmployee = GenerateQueriesFromQuery(Experiment3Hybrid.Insert)[0];
            string querySoftwareDeveloper = GenerateQueriesFromQuery(Experiment3Hybrid.Insert)[1];

            using var connection = new SqlConnection(_connectionString);
            var commandEmployee = new SqlCommand(queryEmployee, connection);
            var commandDeveloper = new SqlCommand(querySoftwareDeveloper, connection);
            connection.Open();
            foreach(var softwareDeveloper in softwareDevelopers)
            {
                commandDeveloper.ToCommandSoftwareDeveloperHybrid(softwareDeveloper);
                commandEmployee.ToCommandSoftwareDeveloperHybrid(softwareDeveloper);

                commandEmployee.ExecuteNonQuery();
                commandDeveloper.ExecuteNonQuery();
            }
        }

        public void InsertSoftwareDeveloper(SoftwareDeveloperModel softwareDeveloper)
        {
            string queryEmployee = GenerateQueriesFromQuery(Experiment3Hybrid.Insert)[0];
            string querySoftwareDeveloper = GenerateQueriesFromQuery(Experiment3Hybrid.Insert)[1];

            using var connection = new SqlConnection(_connectionString);

            var commandEmployee= new SqlCommand(queryEmployee, connection);
            var commandDeveloper = new SqlCommand(querySoftwareDeveloper, connection);
            commandDeveloper.ToCommandSoftwareDeveloperHybrid(softwareDeveloper);
            commandEmployee.ToCommandSoftwareDeveloperHybrid(softwareDeveloper);

            connection.Open();
            commandEmployee.ExecuteNonQuery();
            commandDeveloper.ExecuteNonQuery();
        }

        public void UpdateFullStackByExpYearsAndTitle()
        {
            throw new NotImplementedException();
        }

        public void UpdateFullstackById(long id)
        {
            throw new NotImplementedException();
        }

        public void UpdateMethodById(long id, string newMethod)
        {
            throw new NotImplementedException();
        }

        public void UpdateMethodByYearsAndDepartment()
        {
            throw new NotImplementedException();
        }

        public void UpdatePhoneById(long id, string newPhone)
        {
            throw new NotImplementedException();
        }

        public void UpdateTitleByFullstackAndSeniorityAndYearsExp()
        {
            throw new NotImplementedException();
        }
    }
}
