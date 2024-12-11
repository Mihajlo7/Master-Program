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
using SqlDataAccess.Implementation;

namespace ConsoleProgram.Benchmark.Exp2
{
    [SimpleJob(RunStrategy.Monitoring, launchCount: 1,
     warmupCount: 2, iterationCount: 5)]
    public sealed class InsertExperiment2
    {
        public IDepartmentTeamEmployeeRepository _repository;
        public GeneratorService generatorService= new();
        public List<DepartmentModel> departmentList;
        public List<EmployeeModel2> employeeList;

        const int emp_size=50;
        const int team_size = 20;

        [GlobalSetup]
        public void GlobalSetup()
        {
            _repository = new SqlDepartementTeamEmployeeRepository();
            (departmentList, employeeList) = generatorService.GenerateDepartmentsAndEmployeers(emp_size, team_size);
        }

        [IterationSetup] public void IterationSetup() => _repository.ExecuteCreationTable();

        [Benchmark] public void InsertDepartmentWithTeams() => _repository.InsertDepartmentWithTeams(departmentList.First());
        [Benchmark] public void InsertEmployee() => _repository.InsertEmployee(employeeList.First());

        [Benchmark] public void InsertDepartmentsWithTeams() => _repository.InsertDepartmenstWithTeams(departmentList);
        [Benchmark] public void InsertEmployees() => _repository.InsertEmployees(employeeList);

        [Benchmark] public void InsertBulkDepartmentsWithTeamsBulk() => _repository.InsertBulkDepartmenstWithTeams(departmentList);
        [Benchmark] public void InsertbulkEmployees() => _repository.InsertEmployees(employeeList);
    }
}
