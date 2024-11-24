using Core.Models.Exp1;
using DataAccess;
using MongoDB.Bson;
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
            Console.WriteLine("Insertion Employees is not required!");
        }

        public List<TaskModel> GetAllTasksWithEmployeesBadWay()
        {
            return _collection.Find(FilterDefinition<TaskModel>.Empty).ToList();
        }

        public List<TaskModel> GetAllTasksWithEmployees()
        {
            var projection =
                Builders<TaskModel>.Projection.Expression(t => new TaskModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    Deadline = t.Deadline,
                    Description = t.Description,
                    Priority = t.Priority,
                    Status = t.Status,
                    Responsible = t.Responsible,
                    Supervisor = t.Supervisor,
                    Employees = t.Employees,
                });
            return _collection.Find(FilterDefinition<TaskModel>.Empty).Project(projection).ToList();
        }

        public List<TaskModel> GetAllTasksWithEmployeesSorted()
        {
            return _collection.Find(FilterDefinition<TaskModel>.Empty)
                .Sort(Builders<TaskModel>.Sort.Ascending(t=>t.Id))
                .ToList();
        }

        public TaskModel GetTaskWithEmployeesById(long id)
        {
            var filter=Builders<TaskModel>.Filter.Eq("_id", id);
            return _collection.Find(filter).FirstOrDefault();
        }

        public List<TaskModel> GetAllTasksWithEmployeesByPriorityAndStatus(int priority)
        {
            var filter = Builders<TaskModel>.Filter.And(
                    Builders<TaskModel>.Filter.Gt(t=>t.Priority,priority),
                    Builders<TaskModel>.Filter.In(t => t.Status, new[] {"Pending","New"})
                );
            return _collection.Find(filter).ToList();
        }

        public List<TaskModel> GetAllTasksByDeadilineAndNotComplited(int day)
        {
            var dateTime= DateTime.UtcNow.AddDays(day);
            var projection =
                Builders<TaskModel>.Projection.Expression(t => new TaskModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    Deadline = t.Deadline,
                    Description = t.Description,
                    Priority = t.Priority,
                    Status = t.Status,
                    Responsible = t.Responsible,
                });
            var filter = Builders<TaskModel>.Filter.And(
                    Builders<TaskModel>.Filter.Lt(t => t.Deadline, dateTime),
                    Builders<TaskModel>.Filter.Nin(t => t.Status, new[] {"Complited"})
                );
            return _collection.Find(filter).Project(projection).ToList();
        }

        public List<TaskModel> GetAllTasksByResponsibleNameAndSupervisorBirthday(string firstname, DateTime birthday)
        {
            var filter = Builders<TaskModel>.Filter.And(
                    Builders<TaskModel>.Filter.Regex(t=>t.Responsible.FirstName,
                    new MongoDB.Bson.BsonRegularExpression($"^{firstname}","i"))
                    ,Builders<TaskModel>.Filter.Exists(t => t.Supervisor, true)
                    ,Builders<TaskModel>.Filter.Lt(t=>t.Supervisor.BirthDay, birthday)
                );
            var projection =
                Builders<TaskModel>.Projection.Expression(t => new TaskModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    Deadline = t.Deadline,
                    Description = t.Description,
                    Priority = t.Priority,
                    Status = t.Status
                });
            return _collection.Find(filter).Project(projection).ToList();
        }

        public List<EmployeeWithCountTasksModel> GetEmployeesWithCountTasks()
        {
            var aggPipeline = new[]
            {
                new BsonDocument("$unwind","$Employees"),
                new BsonDocument("$group",new BsonDocument
                {
                    {"_id","$Employees.Employee._id"},
                    { "Email", new BsonDocument("$first", "$Employees.Employee.Email")},
                    { "Count", new BsonDocument("$sum", 1) }
                }),
            };
            var result =  _collection.Aggregate<EmployeeWithCountTasksModel>(aggPipeline).ToList();
            return result;
        }

        public List<EmployeeWithCountTasksModel> GetEmployeesWithCountTasksHavingAndOrder(int numOfTasks)
        {
            var aggPipeline = new[]
            {
                new BsonDocument("$unwind","$Employees"),
                new BsonDocument("$group",new BsonDocument
                {
                    {"_id","$Employees.Employee._id"},
                    { "Email", new BsonDocument("$first", "$Employees.Employee.Email")},
                    { "Count", new BsonDocument("$sum", 1) }
                }),
                new BsonDocument ("$match",new BsonDocument
                {
                    {"Count",new BsonDocument("$gt",numOfTasks)}
                }),
                new BsonDocument("$sort", new BsonDocument("Count",-1))
            };
            var result = _collection.Aggregate<EmployeeWithCountTasksModel>(aggPipeline).ToList();
            return result;
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
