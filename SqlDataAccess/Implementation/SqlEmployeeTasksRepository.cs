using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models.Exp1;
using DataAccess;
using Microsoft.Data.SqlClient;
using SqlDataAccess.Helpers;
using SqlDataAccess.Queries.Exp1;

namespace SqlDataAccess.Implementation
{
    public class SqlEmployeeTasksRepository : SqlRepository, IEmployeeTasksRepository
    {
        public SqlEmployeeTasksRepository():base("exp1_db"){ }
        public SqlEmployeeTasksRepository(string database) : base(database) { }


        public  void ExecuteCreationAdditional()
        {
            throw new NotImplementedException();
        }

        public  void ExecuteCreationTable()
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

        public void InsertBulk(List<TaskModel> tasks)
        {
            // Insert Tasks
            using var copy= new SqlBulkCopy(_connectionString);

            copy.DestinationTableName = "dbo.Task";

            copy.ColumnMappings.Add(nameof(TaskModel.Id),"id");
            copy.ColumnMappings.Add(nameof(TaskModel.Name),"name");
            copy.ColumnMappings.Add(nameof(TaskModel.Description),"description");
            copy.ColumnMappings.Add(nameof(TaskModel.Priority),"priority");
            copy.ColumnMappings.Add(nameof(TaskModel.Status),"status");
            copy.ColumnMappings.Add(nameof(TaskModel.Deadline),"deadline");
            copy.ColumnMappings.Add(nameof(TaskModel.Responsible.Id), "responsible");
            copy.ColumnMappings.Add(nameof(TaskModel.Supervisor.Id), "supervisor");

            DataTable dt = new DataTable();
            dt.Columns.Add("id",typeof(long));
            dt.Columns.Add("name",typeof(string));
            dt.Columns.Add("description",typeof(string));
            dt.Columns.Add("priority",typeof(int));
            dt.Columns.Add("status",typeof(string));
            dt.Columns.Add("deadline",typeof(DateTime));
            dt.Columns.Add("responsible",typeof(long));
            dt.Columns.Add("supervisor",typeof(long));

            foreach(var task in tasks)
            {
                dt.Rows.Add(task.Id, task.Name, task.Description, task.Priority, task.Status, task.Deadline, task.Responsible.Id, task.Supervisor?.Id ?? (object)DBNull.Value);

            }

            copy.WriteToServer(dt);
            List<EmployeeTaskModel> list = new List<EmployeeTaskModel>();
            list= tasks.SelectMany(t=>t.Employees).ToList();

            copy.ColumnMappings.Clear();

            copy.DestinationTableName = "dbo.EmployeeTask";
            copy.ColumnMappings.Add(nameof(EmployeeTaskModel.EmployeeId),"employeeId");
            copy.ColumnMappings.Add(nameof(EmployeeTaskModel.TaskId),"taskId");
            DataTable dt2 = new DataTable();
            dt2.Columns.Add("employeeId", typeof(long));
            dt2.Columns.Add("taskId",typeof(long));
            foreach(var l in list)
            {
                dt2.Rows.Add(l.EmployeeId, l.TaskId);
            }
            copy.WriteToServer(dt2);
        }

        public void InsertEmployeeBulk(List<EmployeeModel> emloyees)
        {
            using var copy= new SqlBulkCopy(_connectionString);

            copy.DestinationTableName = "dbo.Employee";

            copy.ColumnMappings.Add(nameof(EmployeeModel.Id),"id");
            copy.ColumnMappings.Add(nameof(EmployeeModel.Email),"email");
            copy.ColumnMappings.Add(nameof(EmployeeModel.FirstName),"firstName");
            copy.ColumnMappings.Add(nameof(EmployeeModel.LastName),"lastName");
            copy.ColumnMappings.Add(nameof(EmployeeModel.BirthDay),"birthDay");
            copy.ColumnMappings.Add(nameof(EmployeeModel.Phone),"phone");
            copy.ColumnMappings.Add(nameof(EmployeeModel.Title),"title");

            copy.WriteToServer(ToDataTableEmployee(emloyees));
        }

        public int InsertMany(List<TaskModel> tasks)
        {
            int count=0;
            string[] statements = GenerateQueriesFromQuery(Experiment1Sql.Insert);
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            foreach(var task in tasks)
            {
                ExecuteInsertion(connection,task,statements);
                count++;
            }
            return count;
        }

        public void InsertOne(TaskModel newTask)
        {
            string[] statements = GenerateQueriesFromQuery(Experiment1Sql.Insert);
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            ExecuteInsertion(connection, newTask, statements);
        }

       
        //////////////////
        private DataTable ToDataTableEmployee(List<EmployeeModel> emloyees)
        {
            DataTable dt= new DataTable();
            dt.Columns.Add("id", typeof(long));
            dt.Columns.Add("email", typeof(string));
            dt.Columns.Add("firstName", typeof(string));
            dt.Columns.Add("lastName", typeof(string));
            dt.Columns.Add("phone", typeof(string));
            dt.Columns.Add("title", typeof(string));
            dt.Columns.Add("birthDay", typeof(DateTime));

            foreach (var emloyee in emloyees)
            {
                dt.Rows.Add(emloyee.Id,emloyee.Email,emloyee.FirstName,emloyee.LastName,emloyee.Phone,emloyee.Title,emloyee.BirthDay);
            }
            return dt;
        }

        

        private void ExecuteInsertion(SqlConnection connection,TaskModel newTask, string[] statements)
        {
            using var transaction = connection.BeginTransaction();
            try
            {
                // Insert Task
                string queryTask = statements[1];
                var commandTask = new SqlCommand(queryTask, connection, transaction);
                commandTask.Parameters.Clear();
                commandTask.Parameters.AddWithValue("@TaskId", newTask.Id);
                commandTask.Parameters.AddWithValue("@TaskName", newTask.Name);
                commandTask.Parameters.AddWithValue("@TaskDescription", newTask.Description);
                commandTask.Parameters.AddWithValue("@TaskDeadline", newTask.Deadline);
                commandTask.Parameters.AddWithValue("@TaskStatus", newTask.Status);
                commandTask.Parameters.AddWithValue("@TaskPriority", newTask.Priority);
                commandTask.Parameters.AddWithValue("@Responsible", newTask.Responsible.Id);
                commandTask.Parameters.AddWithValue("@Supervisor", newTask.Supervisor?.Id ?? (object)DBNull.Value);

                commandTask.ExecuteNonQuery();

                //Insert TaskEmployee
                string queryEmployeeTask = statements[2];
                var commandEmployeeTask = new SqlCommand(queryEmployeeTask, connection, transaction);

                foreach (var employeeTask in newTask.Employees)
                {
                    commandEmployeeTask.Parameters.Clear();
                    commandEmployeeTask.Parameters.AddWithValue("@TaskId", employeeTask.TaskId);
                    commandEmployeeTask.Parameters.AddWithValue("@EmployeeId", employeeTask.EmployeeId);

                    commandEmployeeTask.ExecuteNonQuery();

                    
                }
               transaction.Commit();

            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public List<TaskModel> GetAllTasksWithEmployeesBadWay()
        {
            string query = GenerateQueriesFromQuery(Experiment1Sql.Select)[0];

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);

            connection.Open();
            using var reader= command.ExecuteReader();
            var tasks= TaskEmployeeHelper.ToTasksFullFromSelectBadWay(reader);
            return tasks;
        }

        public List<TaskModel> GetAllTasksWithEmployees()
        {
            string query = GenerateQueriesFromQuery(Experiment1Sql.Select)[1];

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);

            connection.Open();
            using var reader = command.ExecuteReader();
            var tasks = TaskEmployeeHelper.ToTasksFullFromSelect(reader);
            return tasks;
        }

        public List<TaskModel> GetAllTasksWithEmployeesSorted()
        {
            string query = GenerateQueriesFromQuery(Experiment1Sql.Select)[2];

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);

            connection.Open();
            using var reader = command.ExecuteReader();
            var tasks = TaskEmployeeHelper.ToTasksFullFromSelect(reader);
            return tasks;
        }

        public TaskModel GetTaskWithEmployeesById(long id)
        {
            string query = GenerateQueriesFromQuery(Experiment1Sql.Select)[3];

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TaskId", id);

            connection.Open();
            using var reader = command.ExecuteReader();
            var tasks = TaskEmployeeHelper.ToTasksFullFromSelect(reader);
            return tasks.First();
        }

        public List<TaskModel> GetAllTasksWithEmployeesByPriorityAndStatus(int priority)
        {
            string query = GenerateQueriesFromQuery(Experiment1Sql.Select)[4];

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Priority", priority);

            connection.Open();
            using var reader = command.ExecuteReader();
            var tasks = TaskEmployeeHelper.ToTasksFullFromSelect(reader);
            return tasks;
        }

        public List<TaskModel> GetAllTasksByDeadilineAndNotComplited(int day)
        {
            string query = GenerateQueriesFromQuery(Experiment1Sql.Select)[5];

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Day",day);

            connection.Open();
            using var reader = command.ExecuteReader();
            var tasks=TaskEmployeeHelper.ToTaskWithResponsible(reader);

            return tasks;
        }
        public List<TaskModel> GetAllTasksByResponsibleNameAndSupervisorBirthday(string firstname, DateTime birthday)
        {
            string query = GenerateQueriesFromQuery(Experiment1Sql.Select)[6];

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Firstname", $"{firstname}%");
            command.Parameters.AddWithValue("@Birthday",birthday);

            connection.Open();
            using var reader = command.ExecuteReader();

            var tasks= TaskEmployeeHelper.ToTaskOnly(reader);
            return tasks;
        }
        
        public List<EmployeeWithCountTasksModel> GetEmployeesWithCountTasks()
        {
            string query = GenerateQueriesFromQuery(Experiment1Sql.Select)[7];

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);

            connection.Open();
            using var reader = command.ExecuteReader();
            var employeesWithTasks= TaskEmployeeHelper.ToEmployeeWithTasks(reader);
            return employeesWithTasks;
        }
        // Get Employee with count of tasks and order by
        public List<EmployeeWithCountTasksModel> GetEmployeesWithCountTasksHavingAndOrder(int numOfTasks)
        {
            string query = GenerateQueriesFromQuery(Experiment1Sql.Select)[8];

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@NumOfEmployees",numOfTasks);

            connection.Open();
            using var reader = command.ExecuteReader();
            var employeesWithTasks = TaskEmployeeHelper.ToEmployeeWithTasks(reader);
            return employeesWithTasks;
        }

        public int UpdateExpiredTaskByDeadline()
        {
            string query = GenerateQueriesFromQuery(Experiment1Sql.Update)[0];

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);

            connection.Open();
            var result= command.ExecuteNonQuery();
            return result;
        }

        public int UpdatePhoneById(string phone, long id)
        {
            string query = GenerateQueriesFromQuery(Experiment1Sql.Update)[1];

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Phone",phone);
            command.Parameters.AddWithValue("@Id",id);

            connection.Open();
            var result = command.ExecuteNonQuery();
            return result;
        }

        public int UpdatePhoneByEmail(string phone, string email)
        {
            string query = GenerateQueriesFromQuery(Experiment1Sql.Update)[2];

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Phone", phone);
            command.Parameters.AddWithValue("@Email", email);

            connection.Open();
            var result = command.ExecuteNonQuery();
            return result;

        }

        public int UpdateDeadlineByPriorityByDeadline(int priority, int day)
        {
            string query = GenerateQueriesFromQuery(Experiment1Sql.Update)[3];

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);

            connection.Open();
            var result = command.ExecuteNonQuery();
            return result;
        }

        public int UpdateDeadlineByResponsibleLastName(string lastName)
        {
            string query = GenerateQueriesFromQuery(Experiment1Sql.Update)[4];

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);

            connection.Open();
            var result = command.ExecuteNonQuery();
            return result;
        }

        public int UpdateDeadlineByResponsibleTitleAndBirthday()
        {
            string query = GenerateQueriesFromQuery(Experiment1Sql.Update)[5];

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);

            connection.Open();
            var result = command.ExecuteNonQuery();
            return result;
        }

        public int UpdateTasksFromOneEmployeeToOther(long fromEmployee, long toEmployee)
        {
            string query = GenerateQueriesFromQuery(Experiment1Sql.Update)[6];

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);

            command.Parameters.Clear();
            command.Parameters.AddWithValue("@NewId",toEmployee);
            command.Parameters.AddWithValue("@Id",fromEmployee);

            connection.Open();
            var result = command.ExecuteNonQuery();
            return result;
        }

        public bool DeleteAllTasks()
        {
            string query = GenerateQueriesFromQuery(Experiment1Sql.Delete)[0];

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);

            connection.Open();
            var result = command.ExecuteNonQuery();

            return result > 0;
        }
        public bool DeleteTaskById(long id)
        {
            string query = GenerateQueriesFromQuery(Experiment1Sql.Delete)[1];

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", id);

            connection.Open();
            var result = command.ExecuteNonQuery();

            return result > 0;
        }
        public bool DeleteTasksByStatus(string status)
        {
            string query = GenerateQueriesFromQuery(Experiment1Sql.Delete)[2];

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Status", status);

            connection.Open();
            var result = command.ExecuteNonQuery();

            return result > 0;
        }
        public bool DeleteTasksByResponsibleId(long employeeId)
        {
            string query = GenerateQueriesFromQuery(Experiment1Sql.Delete)[3];

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", employeeId);

            connection.Open();
            var result = command.ExecuteNonQuery();

            return result > 0;
        }
        public bool DeleteTasksBySupervisorFirstName()
        {
            string query = GenerateQueriesFromQuery(Experiment1Sql.Delete)[4];

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);

            connection.Open();
            var result = command.ExecuteNonQuery();

            return result > 0;
        }
    }
}
