using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models.Exp1;
using Core.Models.Exp2;
using DataAccess;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
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
            _database.DropCollection(_departmentsCollectionName);
            _database.DropCollection(_employeesCollectionName);
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
                    { "path", "$Teams" },
                    { "preserveNullAndEmptyArrays", true }
                }),

                // Povezivanje (lookup) timova sa kolekcijom employees
                new BsonDocument("$lookup", new BsonDocument
                {
                    { "from", "Employees" }, // Kolekcija zaposlenih
                    { "localField", "Teams._id" }, // ID tima u kolekciji departments
                    { "foreignField", "TeamId" }, // ID tima u kolekciji employees
                    { "as", "Teams.Employees" } // Dodavanje liste zaposlenih timu
                }),

                // Grupisanje timova nazad u liste unutar svakog departmenta
                new BsonDocument("$group", new BsonDocument
                {
                    { "_id", "$_id" },
                    { "Name", new BsonDocument("$first", "$Name") },
                    { "Location", new BsonDocument("$first", "$Location") },
                    { "Teams", new BsonDocument("$push", "$Teams") }
                })
            };

            var results = _departmentCollection.Aggregate<BsonDocument>(pipeline).ToList();

            var departments = results.Select(d => new DepartmentModel
            {
                Id = d["_id"].AsInt64,
                Name = d["Name"].AsString,
                Location = d["Location"].AsString,
                Teams = d["Teams"].AsBsonArray.Select(t => new TeamModel
                {
                    Id = t["_id"].AsInt64,
                    Name = t["Name"].AsString,
                    Status = t["Status"].AsString,
                    Description = t["Description"].AsString,
                    Employees = t["Employees"].AsBsonArray.Select(e => new EmployeeModel2
                    {
                        Id = e["_id"].AsInt64,
                        FirstName = e["FirstName"].AsString,
                        LastName = e["LastName"].AsString,
                        Email = e["Email"].AsString,
                        BirthDay = e["BirthDay"].ToNullableUniversalTime(),
                        Title = e["Title"].AsString,
                        Phone = e["Phone"].AsString,
                        TeamId = e["TeamId"].AsInt64
                    }).ToList()
                }).ToList()
            }).ToList();

            return departments;
        }

        public List<TeamModel> GetAllTeams(long departmentId)
        {
            var pipeline = new[]
            {
                // Razvijanje (unwind) liste timova
                new BsonDocument("$unwind", new BsonDocument
                {
                    { "path", "$Teams" },
                    { "preserveNullAndEmptyArrays", true }
                }),
                // Filtriranje po ID-u departmana
                new BsonDocument("$match", new BsonDocument("_id", departmentId)),
                // Povezivanje (lookup) timova sa kolekcijom employees
                new BsonDocument("$lookup", new BsonDocument
                {
                    { "from", "Employees" }, // Kolekcija zaposlenih
                    { "localField", "Teams._id" }, // ID tima u kolekciji departments
                    { "foreignField", "TeamId" }, // ID tima u kolekciji employees
                    { "as", "Teams.Employees" } // Dodavanje liste zaposlenih timu
                }),
                // Projekcija podataka
                new BsonDocument("$project", new BsonDocument
                {
                    { "_id", 0 }, // Uklanja polje _id iz rezultata
                    { "Id", "$Teams._id" }, // Preimenuje _id u Id
                    { "Name", "$Teams.Name" },
                    { "Status", "$Teams.Status" },
                    { "Description", "$Teams.Description" },
                    { "LeaderId", "$Teams.LeaderId" },
                    { "Employees", "$Teams.Employees" }
                })
            };

            var results = _departmentCollection.Aggregate<BsonDocument>(pipeline).ToList();

            return [];
        }

        public List<EmployeeModel2> GetAllEmployees(long teamId)
        {
            var filterFindEmployeesByTeamId = Builders<EmployeeModel2>.Filter.Eq(e => e.TeamId, teamId);

            var foundEmployees= _employeesCollection
                .Find(filterFindEmployeesByTeamId).ToList();
            return foundEmployees;
        }

        public EmployeeModel2 GetEmployee(int id)
        {
            var filterFindEmployeeById = Builders<EmployeeModel2>.Filter.Eq(e=>e.Id,id);

            var foundEmployeeById= _employeesCollection.Find(filterFindEmployeeById).ToList().First();
            return foundEmployeeById;
        }

        public List<DepartmentModel> GetDepartmentsWithTeamsInBelgradeAndSorted()
        {
            var filteredTeams = Builders<DepartmentModel>.Filter.Eq(d => d.Location, "Belgrade");
            var sortOptions = Builders<DepartmentModel>.Sort.Ascending(d => d.Id);

            var foundDepartment= _departmentCollection.Find(filteredTeams).Sort(sortOptions).ToList();
            return foundDepartment;
        }

        public List<DepartmentModel> GetDepartmentsWithTeamsYoungerThan35AndEngineer()
        {
            var validTeamIds = _employeesCollection
                .AsQueryable()
                .Where(emp => emp.BirthDay.HasValue && emp.BirthDay.Value.AddYears(35) > DateTime.UtcNow &&
                emp.Title != null && emp.Title.Contains("Engineer"))
                .Select(emp=> emp.TeamId)
                .Distinct()
                .ToList();

            var result = _departmentCollection
                .AsQueryable()
                .Where(dept => dept.Teams != null && dept.Teams.Any(team => validTeamIds.Contains(team.Id)))
                .ToList()
                .Select(dept => new DepartmentModel
                {
                    Id = dept.Id,
                    Name = dept.Name,
                    Location = dept.Location,
                    Teams = dept.Teams
                        ?.Where(team => validTeamIds.Contains(team.Id))
                        .Select(team => new TeamModel
                        {
                            Id = team.Id,
                            Name = team.Name,
                            Status = team.Status,
                            Description = team.Description
                        })
                        .ToList()
                })
                .ToList();

            return result;
        }

        public List<DepartmentAndTeamAgg> GetDepartmentWithEmployeesYearsBetweenGroupBy()
        {
            var pipeline = new[]
{
                // Razvijanje departmana i njihovih timova
                new BsonDocument("$lookup", new BsonDocument
                {
                    { "from", "Employees" },
                    { "localField", "Teams._id" },
                    { "foreignField", "TeamId" },
                    { "as", "Employees" }
                }),
                // Filtriranje zaposlenih po starosnoj dobi (30-40 godina)
                new BsonDocument("$project", new BsonDocument
                {
                    { "DepartmentId", "$_id" },
                    { "_id",0 },
                    { "DepartmentName", "$Name" },
                    { "EmployeesCount", new BsonDocument("$size", new BsonDocument("$filter", new BsonDocument
                        {
                            { "input", "$Employees" },
                            { "as", "emp" },
                            { "cond", new BsonDocument("$and", new BsonArray
                                {
                                    new BsonDocument("$gte", new BsonArray { "$$emp.BirthDay", DateTime.UtcNow.AddYears(-40) }),
                                    new BsonDocument("$lt", new BsonArray { "$$emp.BirthDay", DateTime.UtcNow.AddYears(-30) })
                                })
                            }
                        })
                    )}
                })
            };

            return _departmentCollection.Aggregate<DepartmentAndTeamAgg>(pipeline).ToList();
        }

        public List<DepartmentAndTeamAgg> GetDepartmentWithEmployeeGroupByHavingBy()
        {
            var pipeline = new[]
            {
                // Razdvajanje timova
                new BsonDocument("$unwind",new BsonDocument
                {
                    {"path","$Teams"},
                    {"preserveNullAndEmptyArrays",true}
                }),

                // Spajanje zaposlenih sa timovima
                new BsonDocument("$lookup", new BsonDocument
                {
                    {"from","Employees"},
                    {"localField","Teams._id" },
                    {"foreignField","TeamId" },
                    {"as","Employees" }
                }),

                // filtriranje zaposlenih na osnovu titule
                new BsonDocument("$project", new BsonDocument
                {
                    { "DepartmentId", "$_id" },
                    { "DepartmentName", "$Name" },
                    { "TeamId", "$Teams._id" },
                    { "TeamName", "$Teams.Name" },
                    { "FilteredEmployees", new BsonDocument("$filter",new BsonDocument
                    {
                        { "input","$Employees" },
                        { "as", "emp" },
                        { "cond", new BsonDocument("$regexMatch", new BsonDocument
                        {
                             { "input", "$$emp.Title" },
                             { "regex", "Engineer" },
                             { "options", "i" }
                        }) }
                    })}
                }),

                // Grupisanje po department i timu
                new BsonDocument("$group", new BsonDocument
                {
                    { "_id", new BsonDocument
                        {
                            { "DepartmentId", "$DepartmentId" },
                            { "DepartmentName", "$DepartmentName" },
                            { "TeamId", "$TeamId" },
                            { "TeamName", "$TeamName" }
                        }
                    },
                    {"EmployeesCount",new BsonDocument("$sum",new BsonDocument("$size","$FilteredEmployees")) }
                }),

                // Vraca samo ako ima vise od 2 zaposlena
                new BsonDocument("$match", new BsonDocument
                {
                    { "EmployeesCount",new BsonDocument("$gt",2) }
                }),

                // Sortiranje prema broju zaposlenih
                new BsonDocument("$sort", new BsonDocument
                {
                    { "EmployeesCount", -1 }
                }),

                 // Projekcija konačnih rezultata
                new BsonDocument("$project", new BsonDocument
                {
                    { "DepartmentId", "$_id.DepartmentId" },
                    { "DepartmentName", "$_id.DepartmentName" },
                    { "TeamId", "$_id.TeamId" },
                    { "TeamName", "$_id.TeamName" },
                    { "EmployeesCount", 1 },
                    { "_id",0 }
                })
            };

            return _departmentCollection.Aggregate<DepartmentAndTeamAgg>(pipeline).ToList();
        }

        public void UpdateStatusTeam(long id, string status)
        {
            var filter = Builders<DepartmentModel>.Filter
                .ElemMatch<TeamModel>("Teams", Builders<TeamModel>.Filter.Eq("_id", id));
            var update = Builders<DepartmentModel>.Update.Set("Teams.$.status", status);

            _departmentCollection.UpdateOne(filter, update);
        }

        public void UpdateEmployeePhone(long id, string phone)
        {
            var filter = Builders<EmployeeModel2>.Filter.Eq("_id", id);
            var update = Builders<EmployeeModel2>.Update.Set("Phone", phone);

            _employeesCollection.UpdateOne(filter,update);
        }

        public void UpdateDescriptionTeamsFromPrague(string description)
        {
            var filter = Builders<DepartmentModel>.Filter.Eq("Location", "Prague");
            var update = Builders<DepartmentModel>.Update.Set("Teams.$.Description", description);

            _departmentCollection.UpdateMany(filter,update);
        }

        public void UpdateDescriptionTeamsForYoungEmployees()
        {
            var teamsIds = _employeesCollection
                .Find(Builders<EmployeeModel2>.Filter.Gt(emp=>emp.BirthDay,DateTime.UtcNow.AddYears(-18)))
                .Project(Builders<EmployeeModel2>.Projection.Include("TeamId"))
                .ToList();

            var formatedTeamsIds= teamsIds.Select(doc => doc["TeamId"].AsInt64).Distinct().ToList();

            var filter = Builders<DepartmentModel>.Filter.In("Teams._id",formatedTeamsIds);
            var update = Builders<DepartmentModel>.Update.Set("Teams.$[].Description", "Very very Young");

            _departmentCollection.UpdateMany(filter,update);
        }

        public void UpdateDescriptionComplex()
        {
            // Pronadji timove sa 5 inzenjera
            var engineerTeams = _employeesCollection
                .Aggregate()
                .Match(Builders<EmployeeModel2>.Filter.Regex("Title", new BsonRegularExpression("Engineer", "i")))
                .Group(new BsonDocument
                {
                    {"_id","$TeamId" },
                    {"count",new BsonDocument("$sum",1) }
                })
                .Match(Builders<BsonDocument>.Filter.Gt("count", 5))
                .ToList();

            // Vrati samo id timova
            var teamIds = engineerTeams.Select(doc => doc["_id"].AsInt64).Distinct().ToList();

            // Pronadji departmente
            var departmentFilter = Builders<DepartmentModel>.Filter.Or(
                Builders<DepartmentModel>.Filter.Eq("location", "London"),
                Builders<DepartmentModel>.Filter.Regex("name", new BsonRegularExpression("^H", "i"))
            );

            var update = Builders<DepartmentModel>.Update.Set("Teams.$[Team].Description", Builders<BsonDocument>.Update.Combine(
               Builders<BsonDocument>.Update.Set("Description", new BsonString(" SuperTeam"))
            ));

            var arrayFilters = new List<ArrayFilterDefinition>
            {
                new JsonArrayFilterDefinition<BsonDocument>($"{{ 'team.id': {{ $in: {teamIds.ToJson()} }} }}")
            };

            var updateOptions = new UpdateOptions
            {
                ArrayFilters = arrayFilters
            };

            _departmentCollection.UpdateMany(departmentFilter, update, updateOptions);
        }
    }
}
