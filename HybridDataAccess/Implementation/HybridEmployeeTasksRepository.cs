using Core.Models.Exp1;
using DataAccess;
using HybridDataAccess.Queries;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HybridDataAccess.Implementation
{
    public sealed class HybridEmployeeTasksRepository : HybridRepository, IEmployeeTasksRepository
    {
        public HybridEmployeeTasksRepository() : base("exp_db")
        {
            
        }
        public HybridEmployeeTasksRepository(string database) : base(database)
        {
        }

        public bool DeleteAllTasks()
        {
            throw new NotImplementedException();
        }

        public bool DeleteTaskById(long id)
        {
            throw new NotImplementedException();
        }

        public bool DeleteTasksByResponsibleId(long employeeId)
        {
            throw new NotImplementedException();
        }

        public bool DeleteTasksByStatus(string status)
        {
            throw new NotImplementedException();
        }

        public bool DeleteTasksBySupervisorFirstName()
        {
            throw new NotImplementedException();
        }

        public override void ExecuteCreationAdditional()
        {
            throw new NotImplementedException();
        }

        public override void ExecuteCreationTable()
        {
            string[] statements = GenerateQueriesFromQuery(Exp1HQueries.Tables);

            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            foreach (var statement in statements)
            {
                var command = new SqlCommand(statement, connection);
                command.ExecuteNonQuery();
            }
        }

        public List<TaskModel> GetAllTasksByDeadilineAndNotComplited(int day)
        {
            throw new NotImplementedException();
        }

        public List<TaskModel> GetAllTasksByResponsibleNameAndSupervisorBirthday(string firstname, DateTime birthday)
        {
            throw new NotImplementedException();
        }

        public List<TaskModel> GetAllTasksWithEmployees()
        {
            throw new NotImplementedException();
        }

        public List<TaskModel> GetAllTasksWithEmployeesBadWay()
        {
            throw new NotImplementedException();
        }

        public List<TaskModel> GetAllTasksWithEmployeesByPriorityAndStatus(int priority)
        {
            throw new NotImplementedException();
        }

        public List<TaskModel> GetAllTasksWithEmployeesSorted()
        {
            throw new NotImplementedException();
        }

        public List<EmployeeWithCountTasksModel> GetEmployeesWithCountTasks()
        {
            throw new NotImplementedException();
        }

        public List<EmployeeWithCountTasksModel> GetEmployeesWithCountTasksHavingAndOrder(int numOfTasks)
        {
            throw new NotImplementedException();
        }

        public TaskModel GetTaskWithEmployeesById(long id)
        {
            throw new NotImplementedException();
        }

        public void InsertBulk(List<TaskModel> tasks)
        {
            throw new NotImplementedException();
        }

        public void InsertEmployeeBulk(List<EmloyeeModel> emloyees)
        {
            throw new NotImplementedException();
        }

        public int InsertMany(List<TaskModel> tasks)
        {
            throw new NotImplementedException();
        }

        public void InsertOne(TaskModel newTask)
        {
            string query = GenerateQueriesFromQuery(Exp1HQueries.Tables)[0];
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand();
        }

        public int UpdateDeadlineByPriorityByDeadline(int priority, int day)
        {
            throw new NotImplementedException();
        }

        public int UpdateDeadlineByResponsibleLastName(string lastName)
        {
            throw new NotImplementedException();
        }

        public int UpdateDeadlineByResponsibleTitleAndBirthday()
        {
            throw new NotImplementedException();
        }

        public int UpdateExpiredTaskByDeadline()
        {
            throw new NotImplementedException();
        }

        public int UpdatePhoneByEmail(string phone, string email)
        {
            throw new NotImplementedException();
        }

        public int UpdatePhoneById(string phone, long id)
        {
            throw new NotImplementedException();
        }

        public int UpdateTasksFromOneEmployeeToOther()
        {
            throw new NotImplementedException();
        }
    }
}
