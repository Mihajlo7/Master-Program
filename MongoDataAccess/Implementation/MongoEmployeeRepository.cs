using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models.Exp2;
using Core.Models.Exp3;
using DataAccess;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDataAccess.Implementation
{
    public sealed class MongoEmployeeRepository : MongoRepository,IEmployeeRepository
    {
        private const string _EMPLOYEE_COLLECTION = "EmployeesTypes";
        private readonly IMongoCollection<EmployeeModel3> _employeesCollection;
        private  MongoEmployeeRepository(string database) : base(database)
        {
            _employeesCollection = _database.GetCollection<EmployeeModel3>(_EMPLOYEE_COLLECTION);
        }

        public MongoEmployeeRepository() : this("master_db")
        {
        }

        private IMongoCollection<ManagerModel> ManagersCollection 
            => _database.GetCollection<ManagerModel>(_EMPLOYEE_COLLECTION);

        private IMongoCollection<SoftwareDeveloperModel> SoftwareCollection
            => _database.GetCollection<SoftwareDeveloperModel>(_EMPLOYEE_COLLECTION);

        private ProjectionDefinition<EmployeeModel3> EmployeeProjection
            => Builders<EmployeeModel3>.Projection
                .Include(e => e.Id)
                .Include(e => e.Email)
                .Include(e => e.FirstName)
                .Include(e => e.LastName)
                .Include(e => e.Title)
                .Include(e => e.BirthDay)
                .Include(e => e.Phone)
                .Include(e => e.Role);

        private ProjectionDefinition<ManagerModel> ManagerProjection
            => Builders<ManagerModel>.Projection
                .Include(e => e.Id)
                .Include(e => e.Email)
                .Include(e => e.FirstName)
                .Include(e => e.LastName)
                .Include(e => e.Title)
                .Include(e => e.BirthDay)
                .Include(e => e.RealisedProject)
                .Include(e => e.Method)
                .Include(e => e.Department);
        
        private ProjectionDefinition<SoftwareDeveloperModel> SoftwareDeveloperProjection
            => Builders<SoftwareDeveloperModel>.Projection
                .Include(e => e.Id)
                .Include(e => e.Email)
                .Include(e => e.FirstName)
                .Include(e => e.LastName)
                .Include(e => e.Title)
                .Include(e => e.BirthDay)
                .Include(e => e.Seniority)
                .Include(e => e.YearsOfExperience)
                .Include(e => e.IsRemote)
                .Include(e => e.IDE)
                .Include(e => e.ProgrammingLanguage)
                .Include(e => e.IsFullStack);


        public void ExecuteCreationTable()
        {
            _database.DropCollection(_EMPLOYEE_COLLECTION);
            _database.CreateCollection(_EMPLOYEE_COLLECTION);
        }

        public void ExecuteCreationAdditional()
        {
            throw new NotImplementedException();
        }

        public void InsertManager(ManagerModel manager)
        {
            manager.Role = "Manager";
            _employeesCollection.InsertOne(manager);
        }

        public void InsertSoftwareDeveloper(SoftwareDeveloperModel softwareDeveloper)
        {
            softwareDeveloper.Role = "SoftwareDeveloper";
            _employeesCollection.InsertOne(softwareDeveloper);
        }

        public void InsertManyManager(List<ManagerModel> managers)
        {
            managers.ForEach(manager => manager.Role = "Manager");
            _employeesCollection.InsertMany(managers);
        }

        public void InsertManySoftwareDeveloper(List<SoftwareDeveloperModel> softwareDevelopers)
        {
            softwareDevelopers.ForEach(sd => sd.Role = "SoftwareDeveloper");
            _employeesCollection.InsertMany(softwareDevelopers);
        }

        public void InsertBulkManager(List<ManagerModel> managers)
        {
            var bulkWriteManagers = managers.Select(m => new InsertOneModel<ManagerModel>(m));
            _database.GetCollection<ManagerModel>(_EMPLOYEE_COLLECTION).BulkWrite(bulkWriteManagers);
            
        }

        public void InsertBulkSoftwareDeveloper(List<SoftwareDeveloperModel> softwareDevelopers)
        {
            var bulkWrite = softwareDevelopers.Select(sd => new InsertOneModel<SoftwareDeveloperModel>(sd));
            _database.GetCollection<SoftwareDeveloperModel>(_EMPLOYEE_COLLECTION).BulkWrite(bulkWrite);
        }

        public List<EmployeeModel3> GetAllEmployees()
        {
            
            var employees= _employeesCollection.Find(FilterDefinition<EmployeeModel3>.Empty)
                .Project<EmployeeModel3>(EmployeeProjection)
                .ToList();
            return employees;
        }

        public List<ManagerModel> GetAllManagers()
        {
            
            var managers = ManagersCollection.Find(Builders<ManagerModel>.Filter.Eq("Role","Manager"))
                .Project<ManagerModel>(ManagerProjection)
                .ToList();

            return managers;
        }

        public List<SoftwareDeveloperModel> GetAllSoftwareDevelopers()
        {
            
            var softwareDevelopers = SoftwareCollection
                .Find(Builders<SoftwareDeveloperModel>.Filter.Eq("Role", "SoftwareDeveloper"))
                .Project<SoftwareDeveloperModel>(SoftwareDeveloperProjection)
                .ToList();
            return softwareDevelopers;
        }

        public EmployeeModel3 GetEmployeeById(long id)
        {
            var employeeById = _employeesCollection
                .Find(Builders<EmployeeModel3>.Filter.Eq("id",id))
                .Project<EmployeeModel3>(EmployeeProjection)
                .FirstOrDefault();

            return employeeById;
        }

        public List<ManagerModel> GetManagersYoungerThan30AndAgileMethodSorted()
        {
            var filterForManagers = Builders<ManagerModel>.Filter.And(
                Builders<ManagerModel>.Filter.Eq("Role", "Manager"),
                Builders<ManagerModel>.Filter.Gt(m=>m.BirthDay,DateTime.UtcNow.AddYears(-30)),
                Builders<ManagerModel>.Filter.Eq(m=>m.Method,"Agile")
                );
            var foundManagers = ManagersCollection
                .Find(filterForManagers)
                .Project<ManagerModel>(ManagerProjection)
                .ToList();
            return foundManagers;
        }

        public List<SoftwareDeveloperModel> GetSoftwareDevelopersRemoteAndUseVisualStudio()
        {
            var filterForDevelopers = Builders<SoftwareDeveloperModel>.Filter.And(
                Builders<SoftwareDeveloperModel>.Filter.Eq("Role", "SoftwareDeveloper"),
                Builders<SoftwareDeveloperModel>.Filter.Eq(m => m.IsRemote, true),
                Builders<SoftwareDeveloperModel>.Filter.Eq(m => m.IDE, "Visual Studio")
                );
            var foundSoftwareDevelopers = SoftwareCollection
                .Find(filterForDevelopers)
                .Project<SoftwareDeveloperModel>(SoftwareDeveloperProjection)
                .ToList();
            return foundSoftwareDevelopers;
        }

        public List<SoftwareDeveloperModel> GetSoftwareDevelopersOlderThan25AndMediorAndJavaAndC()
        {
            var filterForDevelopers = Builders<SoftwareDeveloperModel>.Filter.And(
                Builders<SoftwareDeveloperModel>.Filter.Eq("Role", "SoftwareDeveloper"),
                Builders<SoftwareDeveloperModel>.Filter.Eq(m => m.Seniority, "Medior"),
                Builders<SoftwareDeveloperModel>.Filter.In(m => m.ProgrammingLanguage,new string[] {"Java","C#"}),
                Builders<SoftwareDeveloperModel>.Filter.Lt(m=>m.BirthDay,DateTime.UtcNow.AddYears(-25))
                );

            var foundSoftDevelopers = SoftwareCollection
                .Find(filterForDevelopers)
                .Project<SoftwareDeveloperModel>(SoftwareDeveloperProjection)
                .ToList();

            return foundSoftDevelopers;
        }

        public List<ManagerAggModel> GetAllMethodsWithCountManagers()
        {
            var groupBy = new[]
            {
                new BsonDocument("$match", new BsonDocument("Role", "Manager")), 
                new BsonDocument("$group", new BsonDocument
                {
                    { "_id", "$Method" }, 
                    { "ManagerCount", new BsonDocument("$sum", 1) } 
                }),
                new BsonDocument("$project", new BsonDocument
                {
                    { "Method", "$_id" }, 
                    { "ManagerCount", 1 },
                    { "_id", 0 } 
                })
            };

            var result = _employeesCollection
                 .Aggregate<ManagerAggModel>(groupBy).ToList();
            return result;    
        }

        public List<SoftwareDevelopersAggModel> GetProgrammingLanguagesCountDevelopersAndAvgYearsExp()
        {
            var pipeline = new[]
            {
                // Filtriraj samo SoftwareDeveloper role
                new BsonDocument("$match", new BsonDocument("Role", "SoftwareDeveloper")),
        
                // Grupisanje po programskom jeziku
                new BsonDocument("$group", new BsonDocument
                {
                    { "_id", "$ProgrammingLanguage" }, // Grupisanje po jeziku
                    { "DeveloperCount", new BsonDocument("$sum", 1) }, // Broj programera
                    { "AvgExperience", new BsonDocument("$avg", "$YearsOfExperience") } // Prosečna godina iskustva
                }),

                // Preimenuj "_id" u "ProgrammingLanguage"
                new BsonDocument("$project", new BsonDocument
                {
                    { "ProgrammingLanguage", "$_id" },
                    { "DeveloperCount", 1 },
                    { "AvgExperience", 1 },
                    { "_id", 0 }
                })
            };

            return _employeesCollection.Aggregate<SoftwareDevelopersAggModel>(pipeline).ToList();
        }

        public void UpdatePhoneById(long id, string newPhone)
        {
            var filterEmployee = Builders<EmployeeModel3>.Filter.Eq(e => e.Id, id);
            var updateEmployeePhone = Builders<EmployeeModel3>.Update.Set(e => e.Phone, newPhone);

            _employeesCollection.UpdateOne(filterEmployee, updateEmployeePhone);
        }

        public void UpdateMethodById(long id, string newMethod)
        {
            var filterManager = Builders<ManagerModel>.Filter.Eq(m => m.Id, id);
            var updateManagerMethod = Builders<ManagerModel>.Update.Set(m => m.Method, newMethod);

            ManagersCollection.UpdateOne(filterManager, updateManagerMethod);
        }

        public void UpdateFullstackById(long id)
        {
            var filterSoftwareDev = Builders<SoftwareDeveloperModel>.Filter.Eq(m => m.Id, id);
            var updateFullstack = Builders<SoftwareDeveloperModel>.Update.Set(m => m.IsFullStack, true);

            SoftwareCollection.UpdateOne(filterSoftwareDev, updateFullstack);
        }

        public void UpdateMethodByYearsAndDepartment()
        {
            var filterManager = Builders<ManagerModel>.Filter.And(
                Builders<ManagerModel>.Filter.In(m => m.Department,new []{ "IT", "Logistics" }),
                Builders<ManagerModel>.Filter.Gt(m=>m.BirthDay,DateTime.UtcNow.AddYears(-50)),
                Builders<ManagerModel>.Filter.Lt(m=>m.BirthDay,DateTime.UtcNow.AddYears(-40))
                );
            var updateMethod = Builders<ManagerModel>.Update.Set(m => m.Method, "Lean");

            ManagersCollection.UpdateMany(filterManager, updateMethod);
        }

        public void UpdateFullStackByExpYearsAndTitle()
        {
            var filterSoftwareDevs = Builders<SoftwareDeveloperModel>.Filter.And(
                Builders<SoftwareDeveloperModel>.Filter.Gt(sd=>sd.YearsOfExperience,10),
                Builders<SoftwareDeveloperModel>.Filter.Regex(sd=>sd.Title,new BsonRegularExpression("Engineer","i"))
                );
            var updateFullstack = Builders<SoftwareDeveloperModel>.Update.Set(m => m.IsFullStack, true);

            SoftwareCollection.UpdateMany(filterSoftwareDevs, updateFullstack);
        }

        public void UpdateTitleByFullstackAndSeniorityAndYearsExp()
        {
            var filter = Builders<SoftwareDeveloperModel>.Filter.And(
                Builders<SoftwareDeveloperModel>.Filter.Gt(sd => sd.YearsOfExperience, 20),
                Builders<SoftwareDeveloperModel>.Filter.Eq(sd => sd.Seniority, "Senior"),
                Builders<SoftwareDeveloperModel>.Filter.Eq(sd => sd.IsFullStack, true)
            );

            var update = Builders<SoftwareDeveloperModel>.Update
                .Set("Title", new BsonDocument("$concat", new BsonArray { "Principle ", "$Title" }));

            var pipeline =

                new BsonDocument("$set", new BsonDocument
                {
                    { "Title", new BsonDocument("$concat", new BsonArray { "Principle ", "$Title" }) }
                });
            

            // Pokretanje updateMany sa agregacijom
            //SoftwareCollection
                ;
                
        }
    }
}
