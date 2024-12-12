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
            _database.DropCollection(_collectionName);
            _database.CreateCollection(_collectionName);
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

        public void InsertEmployeeBulk(List<EmployeeModel> emloyees)
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
            var filterForUpdating = Builders<TaskModel>.Filter.And
                (
                    Builders<TaskModel>.Filter.Eq(t=>t.Status,"Pending"),
                    Builders<TaskModel>.Filter.Lt(t=>t.Deadline,DateTime.UtcNow)
                );
            var update = Builders<TaskModel>.Update.Set(t => t.Status, "Cancelled");

            var result= _collection.UpdateMany(filterForUpdating, update);

            return (int)result.MatchedCount;
        }

        public int UpdatePhoneById(string phone, long id)
        {
            int result= 0;

            // Update Responsible
            var filterResponsible = Builders<TaskModel>.Filter.Eq(t => t.Responsible.Id, id);
            var updateResponsible = Builders<TaskModel>.Update.Set(t=>t.Responsible.Phone, phone);
            result += (int)_collection.UpdateMany(filterResponsible, updateResponsible).MatchedCount;
            //Update Supervisor
            var filterSupervisor = Builders<TaskModel>.Filter.And(
                    Builders<TaskModel>.Filter.Ne(t=>t.Supervisor,null),
                    Builders<TaskModel>.Filter.Eq(t => t.Supervisor.Id, id)
                );
            var updateSupervisor = Builders<TaskModel>.Update.Set(t => t.Supervisor.Phone, phone);
            result += (int)_collection.UpdateMany(filterSupervisor, updateSupervisor).MatchedCount;
            // Update Employees
            var filter = Builders<TaskModel>.Filter.Eq("Employees.Employee._id", id);

            var update = Builders<TaskModel>.Update
                .Set("Employees.$[elem].Employee.Phone", phone);

            var arrayFilters = new[]
            {
                new BsonDocumentArrayFilterDefinition<BsonDocument>(
                    new BsonDocument("elem.Employee._id", id))
            };

            var updateOptions = new UpdateOptions { ArrayFilters = arrayFilters };

            result+=(int) _collection.UpdateMany(filter, update, updateOptions).MatchedCount;

            return result;
        }

        public int UpdatePhoneByEmail(string phone, string email)
        {
            int result = 0;

            // Update Responsible
            var filterResponsible = Builders<TaskModel>.Filter.Eq(t => t.Responsible.Email, email);
            var updateResponsible = Builders<TaskModel>.Update.Set(t => t.Responsible.Phone, phone);
            result += (int)_collection.UpdateMany(filterResponsible, updateResponsible).MatchedCount;
            //Update Supervisor
            var filterSupervisor = Builders<TaskModel>.Filter.And(
                    Builders<TaskModel>.Filter.Ne(t => t.Supervisor, null),
                    Builders<TaskModel>.Filter.Eq(t => t.Supervisor.Email, email)
                );
            var updateSupervisor = Builders<TaskModel>.Update.Set(t => t.Supervisor.Phone, phone);
            result += (int)_collection.UpdateMany(filterSupervisor, updateSupervisor).MatchedCount;
            // Update Employees
            var filter = Builders<TaskModel>.Filter.Eq("Employees.Employee.Email", email);

            var update = Builders<TaskModel>.Update
                .Set("Employees.$[elem].Employee.Phone", phone);

            var arrayFilters = new[]
            {
                new BsonDocumentArrayFilterDefinition<BsonDocument>(
                    new BsonDocument("elem.Employee.Email", email))
            };

            var updateOptions = new UpdateOptions { ArrayFilters = arrayFilters };

            result += (int)_collection.UpdateMany(filter, update, updateOptions).MatchedCount;

            return result;
        }

        public int UpdateDeadlineByPriorityByDeadline(int priority, int day)
        {
            var filter = Builders<TaskModel>.Filter.And(
                    Builders<TaskModel>.Filter.Lt(t=>t.Priority,priority),
                    Builders<TaskModel>.Filter.Lt(t => t.Deadline, DateTime.UtcNow)
                );
            var update = Builders<TaskModel>.Update.Set(
                "Deadline",
                new BsonDocument("$dateAdd", new BsonDocument
                {
                    { "startDate", "$Deadline" },
                    { "unit", "day" },
                    { "amount", 5 }
                })
            );
            return (int) _collection.UpdateMany(filter,update).MatchedCount;
        }

        public int UpdateDeadlineByResponsibleLastName(string lastName)
        {
            var filter = Builders<TaskModel>.Filter.And(
                    Builders<TaskModel>.Filter.Regex(t=>t.Responsible.LastName,
                    new MongoDB.Bson.BsonRegularExpression($"^{lastName}", "i"))
                   
                );
            var update = Builders<TaskModel>.Update.Set(
                "Deadline",
                new BsonDocument("$dateAdd", new BsonDocument
                {
                    { "startDate", "$Deadline" },
                    { "unit", "day" },
                    { "amount", 3 }
                })
            );
            return (int)_collection.UpdateMany(filter, update).MatchedCount;
        }

        public int UpdateDeadlineByResponsibleTitleAndBirthday()
        {
            var filter = Builders<TaskModel>.Filter.And(
                    Builders<TaskModel>.Filter.Regex(t => t.Responsible.Title,
                    new MongoDB.Bson.BsonRegularExpression($"engineer", "i")),
                    Builders<TaskModel>.Filter.Lt(t=>t.Supervisor.BirthDay,new DateTime(1980,1,1)),
                    Builders<TaskModel>.Filter.Ne(t=>t.Supervisor,null)

                );
            var update = Builders<TaskModel>.Update.Set(
                "Deadline",
                new BsonDocument("$dateAdd", new BsonDocument
                {
                    { "startDate", "$Deadline" },
                    { "unit", "day" },
                    { "amount", 3 }
                })
            );
            return (int)_collection.UpdateMany(filter, update).MatchedCount;
        }

        public int UpdateTasksFromOneEmployeeToOther(long fromEmployee, long toEmployee)
        {
           
            var aggPipeline = new[]
            {
                // Unwind the Employees array
                new BsonDocument("$unwind", "$Employees"),

                // Match the employee with the specified ID
                new BsonDocument("$match", new BsonDocument
                {
                    { "Employees.Employee._id", toEmployee }
                }),

                // Project the Employee fields into EmployeeModel structure
                new BsonDocument("$project", new BsonDocument
                {
                    { "_id", 0 },  // Exclude the Task _id
                    { "Id", "$Employees.Employee.Id" },
                    { "FirstName", "$Employees.Employee.FirstName" },
                    { "LastName", "$Employees.Employee.LastName" },
                    { "Email", "$Employees.Employee.Email" },
                    { "BirthDay", "$Employees.Employee.BirthDay" },
                    { "Title", "$Employees.Employee.Title" },
                    { "Phone", "$Employees.Employee.Phone" }
                }),

                // Limit to one result
                new BsonDocument("$limit", 1)
            };

            var employeeToAdd= _collection.Aggregate<EmployeeModel>(aggPipeline).First();
            // Updating all tasks
            var filterTasksForAdding = Builders<TaskModel>.Filter.ElemMatch(t => t.Employees, e => e.Employee.Id == fromEmployee);
            var updateAdded = Builders<TaskModel>.Update.Combine(
                    Builders<TaskModel>.Update.Push(t => t.Employees, new EmployeeTaskModel
                    {
                        Employee = employeeToAdd,
                    })
                    //Builders<TaskModel>.Update.PullFilter(t=>t.Employees,e=>e.Employee.Id==fromEmployee)
            );
            var resultAdded= _collection.UpdateMany(filterTasksForAdding, updateAdded);
            //Deleting employees
            var updateTasksForDeleting = Builders<TaskModel>.Update
                .PullFilter(t => t.Employees, e => e.Employee.Id == fromEmployee);
            var resultUpdated = _collection.UpdateMany(filterTasksForAdding, updateTasksForDeleting);

            return (int)resultAdded.MatchedCount+ (int) resultUpdated.MatchedCount;
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
