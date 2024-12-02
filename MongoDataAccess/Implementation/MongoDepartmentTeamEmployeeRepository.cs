using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models.Exp1;
using Core.Models.Exp2;
using DataAccess;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDataAccess.Implementation
{
    public sealed class MongoDepartmentTeamEmployeeRepository : MongoRepository,IDepartmentTeamEmployeeRepository
    {
        private string _departmentsCollectionName= "DepartmentsWithTeams";
        private string _employeesCollectionName = "Employees";

        private IMongoCollection<DepartmentModel> _departmentCollection;
        private IMongoCollection<EmployeeModel2> _employeesCollection;
        public MongoDepartmentTeamEmployeeRepository(string database) : base(database)
        {
            _departmentCollection = _database.GetCollection<DepartmentModel>(_departmentsCollectionName);
            _employeesCollection= _database.GetCollection<EmployeeModel2>(_employeesCollectionName);
        }

        public MongoDepartmentTeamEmployeeRepository(): base("master_db")
        {
            _departmentCollection = _database.GetCollection<DepartmentModel>(_departmentsCollectionName);
            _employeesCollection = _database.GetCollection<EmployeeModel2>(_employeesCollectionName);
        }

        public void ExecuteCreationTable()
        {
            _database.CreateCollection(_departmentsCollectionName);
            _database.CreateCollection(_employeesCollectionName);
        }

        public void ExecuteCreationAdditional()
        {
            throw new NotImplementedException();
        }

        public void InsertDepartmentWithTeams(DepartmentModel department)
        {
            _departmentCollection.InsertOne(department);
        }

        public void InsertEmployee(EmployeeModel2 employee)
        {
            _employeesCollection.InsertOne(employee);
        }

        public void InsertDepartmenstWithTeams(List<DepartmentModel> departments)
        {
            _departmentCollection.InsertMany(departments);
        }

        public void InsertEmployees(List<EmployeeModel2> employees)
        {
            _employeesCollection.InsertMany(employees);
        }

        public void InsertBulkDepartmenstWithTeams(List<DepartmentModel> departments)
        {
            var bulkWriteinsert = departments.Select(d => new InsertOneModel<DepartmentModel>(d));
            _departmentCollection.BulkWrite(bulkWriteinsert);
        }

        public void InsertBulkEmployees(List<EmployeeModel2> employees)
        {
            var bulkWriteInsert= employees.Select(e=> new InsertOneModel<EmployeeModel2>(e));
            _employeesCollection.BulkWrite(bulkWriteInsert);
        }

        public List<DepartmentModel> GetAllDepartmentsBadWay()
        {
            var departments = _departmentCollection.Find(FilterDefinition<DepartmentModel>.Empty).ToList();
            var employments = _employeesCollection.Find(FilterDefinition<EmployeeModel2>.Empty).ToList();

            foreach (var department in departments)
            {
                foreach(var team in department.Teams)
                {
                    var foundEployees = employments.FindAll(e => e.TeamId == team.Id);
                    team.Employees=foundEployees;
                }
            }
            return departments;
        }

        public List<DepartmentModel> GetAllDepartments()
        {
            var pipeline = new[]
            {
                // Razvijanje (unwind) liste timova
                new BsonDocument("$unwind", new BsonDocument
                {
                    { "path", "$teams" },
                    { "preserveNullAndEmptyArrays", true }
                }),

                // Povezivanje (lookup) timova sa kolekcijom employees
                new BsonDocument("$lookup", new BsonDocument
                {
                    { "from", "employees" }, // Kolekcija zaposlenih
                    { "localField", "teams.id" }, // ID tima u kolekciji departments
                    { "foreignField", "teamId" }, // ID tima u kolekciji employees
                    { "as", "teams.employees" } // Dodavanje liste zaposlenih timu
                }),

                // Grupisanje timova nazad u liste unutar svakog departmenta
                new BsonDocument("$group", new BsonDocument
                {
                    { "_id", "$_id" },
                    { "name", new BsonDocument("$first", "$name") },
                    { "location", new BsonDocument("$first", "$location") },
                    { "teams", new BsonDocument("$push", "$teams") }
                })
            };

            var results = _departmentCollection.Aggregate<BsonDocument>(pipeline).ToList();

            var departments = results.Select(d => new DepartmentModel
            {
                Id = d["_id"].AsInt64,
                Name = d["name"].AsString,
                Location = d["location"].AsString,
                Teams = d["teams"].AsBsonArray.Select(t => new TeamModel
                {
                    Id = t["id"].AsInt64,
                    Name = t["name"].AsString,
                    Status = t["status"].AsString,
                    Description = t["description"].AsString,
                    Employees = t["employees"].AsBsonArray.Select(e => new EmployeeModel2
                    {
                        Id = e["_id"].AsInt64,
                        FirstName = e["firstName"].AsString,
                        LastName = e["lastName"].AsString,
                        Email = e["email"].AsString,
                        BirthDay = e["birthDay"].ToNullableUniversalTime(),
                        Title = e["title"].AsString,
                        Phone = e["phone"].AsString,
                        TeamId = e["teamId"].AsInt64
                    }).ToList()
                }).ToList()
            }).ToList();
        }

        public List<TeamModel> GetAllTeams(long departmentId)
        {
            var pipeline = new[]
            {
                new BsonDocument("$unwind","$Teams"),
                new BsonDocument("$lookup",new BsonDocument
                {
                    {"from","Employees"},
                    {"localField","Teams.Id" },
                    {"foreignField","teamId"},
                    {"as","teams.employees" }
                }),

                new BsonDocument("$group", new BsonDocument
                {
                    { "_id", "$_id" },
                    { "name", new BsonDocument("$first", "$name") },
                    { "location", new BsonDocument("$first", "$location") },
                    { "teams", new BsonDocument("$push", "$teams") }
                })
            };
            return [];
        }

        public List<EmployeeModel2> GetAllEmployees(long teamId)
        {
            throw new NotImplementedException();
        }

        public EmployeeModel2 GetEmployee(int id)
        {
            throw new NotImplementedException();
        }

        public List<DepartmentModel> GetDepartmentsWithTeamsInBelgradeAndSorted()
        {
            throw new NotImplementedException();
        }

        public List<DepartmentModel> GetDepartmentsWithTeamsYoungerThan35AndEngineer()
        {
            throw new NotImplementedException();
        }

        public List<DepartmentAndTeamAgg> GetDepartmentWithEmployeesYearsBetweenGroupBy()
        {
            throw new NotImplementedException();
        }

        public List<DepartmentAndTeamAgg> GetDepartmentWithEmployeeGroupByHavingBy()
        {
            throw new NotImplementedException();
        }

        public void UpdateStatusTeam(long id, string status)
        {
            throw new NotImplementedException();
        }

        public void UpdateEmployeePhone(long id, string phone)
        {
            throw new NotImplementedException();
        }

        public void UpdateDescriptionTeamsFromPrague(string description)
        {
            throw new NotImplementedException();
        }

        public void UpdateDescriptionTeamsForYoungEmployees()
        {
            throw new NotImplementedException();
        }

        public void UpdateDescriptionComplex()
        {
            throw new NotImplementedException();
        }
    }
}
