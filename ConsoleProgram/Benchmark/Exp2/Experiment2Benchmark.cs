using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using ConsoleProgram.Generator;
using Core.Models.Exp2;
using DataAccess;
using HybridDataAccess.Implementation;
using MongoDataAccess.Implementation;
using SqlDataAccess.Implementation;

namespace ConsoleProgram.Benchmark.Exp2
{
    [SimpleJob(RunStrategy.Monitoring, launchCount: 1,
     warmupCount: 2, iterationCount: 5)]
    public  class Experiment2Benchmark
    {
        public IDepartmentTeamEmployeeRepository repository;
        public GeneratorService generatorService = new();
        public List<DepartmentModel> departments;
        public List<EmployeeModel2> employees;
        
        const int emp_size = 2;
        const int team_size = 5;
        
        [GlobalSetup]
        public void SetupOnce()
        {
            Console.WriteLine("Start.");
            Console.WriteLine("Generating data ...");
            if(departments == null || employees == null)
             (departments, employees) = generatorService.GenerateDepartmentsAndEmployeers(emp_size, team_size);
            Console.WriteLine("New repository ...");
            if(repository == null)
                    repository = new MongoDepartmentTeamEmployeeRepository();

            Console.WriteLine("End.");
        
        }
        
        
        [IterationSetup (Targets = new[] 
        {
            //nameof(InsertDepartmentWithTeams),
            nameof(InsertEmployee),
            //nameof(InsertDepartmentsWithTeams),
            nameof(InsertEmployees),
            //nameof(InsertBulkDepartmentsWithTeams),
            nameof(InsertBulkEmployees)
        })] 
        public void IterationSetupEmployees()
        {
            generatorService = new();
            //(departments, employees) = 
             //   generatorService.GenerateDepartmentsAndEmployeers(emp_size, team_size);
            //repository = new HybridDepartmentTeamEmployeeRepository ();
            repository.ExecuteCreationTable();
            repository.InsertBulkDepartmenstWithTeams(departments);
        }
        
        [IterationSetup(Targets = new[]
        {
            nameof(InsertDepartmentWithTeams),
            //nameof(InsertEmployee),
            nameof(InsertDepartmentsWithTeams),
            //nameof(InsertEmployees),
            nameof(InsertBulkDepartmentsWithTeams),
            //nameof(InsertBulkEmployees)
        })]
        public void IterationSetupDT()
        {
            //repository = new MongoDepartmentTeamEmployeeRepository();
            repository.ExecuteCreationTable();
            //repository.InsertBulkEmployees(employees);
        }
        

        
        [Benchmark] public void InsertDepartmentWithTeams() => repository.InsertDepartmentWithTeams(departments.First());
        [Benchmark] public void InsertEmployee() => repository.InsertEmployee(employees.First());
        [Benchmark] public void InsertDepartmentsWithTeams() => repository.InsertDepartmenstWithTeams(departments);
        [Benchmark] public void InsertBulkDepartmentsWithTeams() => repository.InsertBulkDepartmenstWithTeams(departments);
        [Benchmark] public void InsertEmployees() => repository.InsertEmployees(employees);
        [Benchmark] public void InsertBulkEmployees() => repository.InsertBulkEmployees(employees);
        
        [Benchmark] public void GetAllDepartmentsBadWay() => repository.GetAllDepartmentsBadWay();
        [Benchmark] public void GetAllDepartments() => repository.GetAllDepartments();
        [Benchmark] public void GetAllTeams() => repository.GetAllTeams(departments.First().Id);
        [Benchmark] public void GetAllEmployees() => repository.GetAllEmployees(departments.First().Teams.First().Id);
        [Benchmark] public void GetEmployee() => repository.GetEmployee(15);
        [Benchmark] public void GetDepartmentsWithTeamsInBelgradeAndSorted() => repository.GetDepartmentsWithTeamsInBelgradeAndSorted();
        [Benchmark] public void GetDepartmentsWithTeamsYoungerThan35AndEngineer() => repository.GetDepartmentsWithTeamsYoungerThan35AndEngineer();
        [Benchmark] public void GetDepartmentWithEmployeesYearsBetweenGroupBy() => repository.GetDepartmentWithEmployeesYearsBetweenGroupBy();
        [Benchmark] public void GetDepartmentWithEmployeeGroupByHavingBy() => repository.GetDepartmentWithEmployeeGroupByHavingBy();
        [Benchmark] public void UpdateStatusTeam() => repository.UpdateStatusTeam(departments.First().Teams.First().Id, "Active");
        [Benchmark] public void UpdateEmployeePhone() => repository.UpdateEmployeePhone(employees.First().Id, "123-456-7890");
        [Benchmark] public void UpdateDescriptionTeamsFromPrague() => repository.UpdateDescriptionTeamsFromPrague("New Description");
        [Benchmark] public void UpdateDescriptionTeamsForYoungEmployees() => repository.UpdateDescriptionTeamsForYoungEmployees();
        
        [Benchmark] public void UpdateDescriptionComplex() => repository.UpdateDescriptionComplex();
        
    }
}
