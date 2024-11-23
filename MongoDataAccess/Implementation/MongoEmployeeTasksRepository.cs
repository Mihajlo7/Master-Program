using Core.Models.Exp1;
using DataAccess;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDataAccess.Implementation
{
    public sealed class MongoEmployeeTasksRepository : MongoRepository,IEmployeeTasksRepository
    {
        private readonly IMongoCollection<TaskModel> _collection;
        private string _collectionName = "TasksWithEmployees";
        public MongoEmployeeTasksRepository(string database):base(database)
        {
            _collection = _database.GetCollection<TaskModel>(_collectionName);
        }
        public MongoEmployeeTasksRepository() : base("master_db")
        {
            _collection = _database.GetCollection<TaskModel>(_collectionName);
        }

        public void ExecuteCreationTable()
        {
            //_database.CreateCollection("NovaCollection");
        }

        public void ExecuteCreationAdditional()
        {
            throw new NotImplementedException();
        }

        public void InsertOne(TaskModel newTask)
        {
            _collection.InsertOne(newTask);
        }

        public int InsertMany(List<TaskModel> tasks)
        {
            _collection.InsertMany(tasks);
            return 1;
        }

        public void InsertBulk(List<TaskModel> tasks)
        {
            var bulkWriteInsert = tasks.Select(task=>new InsertOneModel<TaskModel>(task));
            _collection.BulkWrite(bulkWriteInsert);
        }

        public void InsertEmployeeBulk(List<EmloyeeModel> emloyees)
        {
            throw new NotImplementedException();
        }

        public List<TaskModel> GetAllTasksWithEmployeesBadWay()
        {
            throw new NotImplementedException();
        }

        public List<TaskModel> GetAllTasksWithEmployees()
        {
            throw new NotImplementedException();
        }

        public List<TaskModel> GetAllTasksWithEmployeesSorted()
        {
            throw new NotImplementedException();
        }

        public TaskModel GetTaskWithEmployeesById(long id)
        {
            throw new NotImplementedException();
        }

        public List<TaskModel> GetAllTasksWithEmployeesByPriorityAndStatus(int priority)
        {
            throw new NotImplementedException();
        }

        public List<TaskModel> GetAllTasksByDeadilineAndNotComplited(int day)
        {
            throw new NotImplementedException();
        }

        public List<TaskModel> GetAllTasksByResponsibleNameAndSupervisorBirthday(string firstname, DateTime birthday)
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

        public int UpdateExpiredTaskByDeadline()
        {
            throw new NotImplementedException();
        }

        public int UpdatePhoneById(string phone, long id)
        {
            throw new NotImplementedException();
        }

        public int UpdatePhoneByEmail(string phone, string email)
        {
            throw new NotImplementedException();
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

        public int UpdateTasksFromOneEmployeeToOther(long fromEmployee, long toEmployee)
        {
            throw new NotImplementedException();
        }

        public bool DeleteAllTasks()
        {
            var result = _collection.DeleteMany(FilterDefinition<TaskModel>.Empty);
            return result.DeletedCount > 0;
        }

        public bool DeleteTaskById(long id)
        {
            throw new NotImplementedException();
        }

        public bool DeleteTasksByStatus(string status)
        {
            throw new NotImplementedException();
        }

        public bool DeleteTasksByResponsibleId(long employeeId)
        {
            throw new NotImplementedException();
        }

        public bool DeleteTasksBySupervisorFirstName()
        {
            throw new NotImplementedException();
        }
    }
}
