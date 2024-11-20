using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models.Exp1;
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

        public void InsertEmployeeBulk(List<EmloyeeModel> emloyees)
        {
            using var copy= new SqlBulkCopy(_connectionString);

            copy.DestinationTableName = "dbo.Employee";

            copy.ColumnMappings.Add(nameof(EmloyeeModel.Id),"id");
            copy.ColumnMappings.Add(nameof(EmloyeeModel.Email),"email");
            copy.ColumnMappings.Add(nameof(EmloyeeModel.FirstName),"firstName");
            copy.ColumnMappings.Add(nameof(EmloyeeModel.LastName),"lastName");
            copy.ColumnMappings.Add(nameof(EmloyeeModel.BirthDay),"birthDay");
            copy.ColumnMappings.Add(nameof(EmloyeeModel.Phone),"phone");
            copy.ColumnMappings.Add(nameof(EmloyeeModel.Title),"title");

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
        private DataTable ToDataTableEmployee(List<EmloyeeModel> emloyees)
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
    }
}
