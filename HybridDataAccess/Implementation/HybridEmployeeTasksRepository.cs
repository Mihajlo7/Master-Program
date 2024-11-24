using Core.Models.Exp1;
using DataAccess;
using HybridDataAccess.Queries;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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

        public void ExecuteCreationAdditional()
        {
            throw new NotImplementedException();
        }

        public void ExecuteCreationTable()
        {
            string[] statements = GenerateQueriesFromQuery(Experiment1Hybrid.Tables)
                .Concat(GenerateProceduresFromQuery(Experiment1Hybrid.Procedures)).ToArray();
            
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
            using var copy = new SqlBulkCopy(_connectionString);

            copy.DestinationTableName = "dbo.Task";

            copy.ColumnMappings.Add(nameof(TaskModel.Id), "id");
            copy.ColumnMappings.Add(nameof(TaskModel.Name), "name");
            copy.ColumnMappings.Add(nameof(TaskModel.Description), "description");
            copy.ColumnMappings.Add(nameof(TaskModel.Priority), "priority");
            copy.ColumnMappings.Add(nameof(TaskModel.Status), "status");
            copy.ColumnMappings.Add(nameof(TaskModel.Deadline), "deadline");
            copy.ColumnMappings.Add(nameof(TaskModel.Responsible), "responsible");
            copy.ColumnMappings.Add(nameof(TaskModel.Supervisor), "supervisor");
            copy.ColumnMappings.Add(nameof(TaskModel.Employees), "employees");

            DataTable dt = new DataTable();
            dt.Columns.Add("id", typeof(long));
            dt.Columns.Add("name", typeof(string));
            dt.Columns.Add("description", typeof(string));
            dt.Columns.Add("priority", typeof(int));
            dt.Columns.Add("status", typeof(string));
            dt.Columns.Add("deadline", typeof(DateTime));
            dt.Columns.Add("responsible", typeof(string));
            dt.Columns.Add("supervisor", typeof(string));
            dt.Columns.Add("employees", typeof(string));

            foreach (var task in tasks)
            {
                dt.Rows.Add(task.Id, task.Name, task.Description, task.Priority, task.Status, task.Deadline, 
                    task.Responsible!= null ?_jsonHandler.SerializeOne<EmployeeModel>(task.Responsible) : (object)DBNull.Value, 
                    task.Supervisor !=null ? _jsonHandler.SerializeOne<EmployeeModel>(task.Supervisor) : (object)DBNull.Value, 
                    task.Employees !=null ? _jsonHandler.SerializeMany<EmployeeTaskModel>(task.Employees) :(object)DBNull.Value);

            }
            copy.WriteToServer(dt);
        }

        public void InsertEmployeeBulk(List<EmployeeModel> emloyees)
        {
            Console.WriteLine("Insertion Employees is not required!");
        }

        public int InsertMany(List<TaskModel> tasks)
        {
            string query = GenerateQueriesFromQuery(Experiment1Hybrid.Insert)[0];
            using var connection = new SqlConnection(_connectionString);
            using var commandTask = new SqlCommand(query, connection);
            int count = 0;

            connection.Open();
            foreach (var newTask in tasks)
            {
                commandTask.Parameters.Clear();
                commandTask.Parameters.AddWithValue("@TaskId", newTask.Id);
                commandTask.Parameters.AddWithValue("@TaskName", newTask.Name);
                commandTask.Parameters.AddWithValue("@TaskDescription", newTask.Description);
                commandTask.Parameters.AddWithValue("@TaskDeadline", newTask.Deadline);
                commandTask.Parameters.AddWithValue("@TaskStatus", newTask.Status);
                commandTask.Parameters.AddWithValue("@TaskPriority", newTask.Priority);
                commandTask.Parameters.AddWithValue("@Responsible", newTask.Responsible != null ? _jsonHandler.SerializeOne<EmployeeModel>(newTask.Responsible) : (object)DBNull.Value);
                commandTask.Parameters.AddWithValue("@Supervisor", newTask.Supervisor != null ? _jsonHandler.SerializeOne<EmployeeModel>(newTask.Supervisor) : (object)DBNull.Value);
                commandTask.Parameters.AddWithValue("@Employees", newTask.Employees != null ? _jsonHandler.SerializeMany<EmployeeTaskModel>(newTask.Employees) : (object)DBNull.Value);

                commandTask.ExecuteNonQuery();
                count++;
            }
            return count;
        }

        public void InsertOne(TaskModel newTask)
        {
            string query = GenerateQueriesFromQuery(Experiment1Hybrid.Insert)[0];
            using var connection = new SqlConnection(_connectionString);
            using var commandTask = new SqlCommand(query,connection);

            commandTask.Parameters.Clear();
            commandTask.Parameters.AddWithValue("@TaskId", newTask.Id);
            commandTask.Parameters.AddWithValue("@TaskName", newTask.Name);
            commandTask.Parameters.AddWithValue("@TaskDescription", newTask.Description);
            commandTask.Parameters.AddWithValue("@TaskDeadline", newTask.Deadline);
            commandTask.Parameters.AddWithValue("@TaskStatus", newTask.Status);
            commandTask.Parameters.AddWithValue("@TaskPriority", newTask.Priority);
            commandTask.Parameters.AddWithValue("@Responsible", newTask.Responsible != null ? _jsonHandler.SerializeOne<EmployeeModel>(newTask.Responsible) : (object)DBNull.Value);
            commandTask.Parameters.AddWithValue("@Supervisor", newTask.Supervisor != null ? _jsonHandler.SerializeOne<EmployeeModel>(newTask.Supervisor) : (object)DBNull.Value);
            commandTask.Parameters.AddWithValue("@Employees", newTask.Employees != null ? _jsonHandler.SerializeMany<EmployeeTaskModel>(newTask.Employees) : (object)DBNull.Value);
            
            connection.Open();
            commandTask.ExecuteNonQuery();
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

        public int UpdateTasksFromOneEmployeeToOther(long fromEmployee, long toEmployee)
        {
            throw new NotImplementedException();
        }
    }
}
