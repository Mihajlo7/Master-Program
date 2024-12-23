using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using ConsoleProgram.Generator;
using Core.Models.Exp3;
using DataAccess;
using SqlDataAccess.Implementation;

namespace ConsoleProgram.Benchmark.Exp3
{
    [SimpleJob(RunStrategy.Monitoring, launchCount: 1,
     warmupCount: 2, iterationCount: 5)]
    public class Experiment3Benchmark
    {
        public IEmployeeRepository _repository;
        public GeneratorService generatorService = new();
        public List<ManagerModel> managers;
        public List<SoftwareDeveloperModel> softwareDevelopers;

        const int employeeSize = 5000;

        [GlobalSetup]
        public void GlobalSetup()
        {
            _repository = new SqlEmployeeRepository();
            (managers, softwareDevelopers) = generatorService.GenerateDataManagersAndDevelopers(employeeSize);
        }

        [IterationSetup (Targets = new[]
        {
            nameof(Benchmark_InsertManager),
            nameof(Benchmark_InsertSoftwareDeveloper),
            nameof(Benchmark_InsertManyManager),
            nameof(Benchmark_InsertManySoftwareDeveloper),
            nameof(Benchmark_InsertBulkManager),
            //nameof(Benchmark_InsertBulkSoftwareDeveloper),

        })]
        public void IterationSetup() => _repository.ExecuteCreationTable();
        [IterationSetup(Target = nameof(Benchmark_InsertBulkSoftwareDeveloper))]
        public void IterationSetupS()
        {
            _repository.ExecuteCreationTable();
            _repository.InsertBulkManager(managers);
        }
        [Benchmark] public void Benchmark_InsertManager() => _repository.InsertManager(managers.First());
        [Benchmark] public void Benchmark_InsertSoftwareDeveloper() => _repository.InsertSoftwareDeveloper(softwareDevelopers.First());
        [Benchmark] public void Benchmark_InsertManyManager() => _repository.InsertManyManager(managers);
        [Benchmark] public void Benchmark_InsertManySoftwareDeveloper() => _repository.InsertManySoftwareDeveloper(softwareDevelopers);
        [Benchmark] public void Benchmark_InsertBulkManager() => _repository.InsertBulkManager(managers);
        [Benchmark] public void Benchmark_InsertBulkSoftwareDeveloper() => _repository.InsertBulkSoftwareDeveloper(softwareDevelopers);
        [Benchmark] public void Benchmark_GetAllEmployees() => _repository.GetAllEmployees();
        [Benchmark] public void Benchmark_GetAllManagers() => _repository.GetAllManagers();
        [Benchmark] public void Benchmark_GetAllSoftwareDevelopers() => _repository.GetAllSoftwareDevelopers();
        [Benchmark] public void Benchmark_GetEmployeeById() => _repository.GetEmployeeById(1);
        [Benchmark] public void Benchmark_GetManagersYoungerThan30AndAgileMethodSorted() => _repository.GetManagersYoungerThan30AndAgileMethodSorted();
        [Benchmark] public void Benchmark_GetSoftwareDevelopersRemoteAndUseVisualStudio() => _repository.GetSoftwareDevelopersRemoteAndUseVisualStudio();
        [Benchmark] public void Benchmark_GetSoftwareDevelopersOlderThan25AndMediorAndJavaAndC() => _repository.GetSoftwareDevelopersOlderThan25AndMediorAndJavaAndC();
        [Benchmark] public void Benchmark_GetAllMethodsWithCountManagers() => _repository.GetAllMethodsWithCountManagers();
        [Benchmark] public void Benchmark_GetProgrammingLanguagesCountDevelopersAndAvgYearsExp() => _repository.GetProgrammingLanguagesCountDevelopersAndAvgYearsExp();
        [Benchmark] public void Benchmark_UpdatePhoneById() => _repository.UpdatePhoneById(1, "1234567890");
        [Benchmark] public void Benchmark_UpdateMethodById() => _repository.UpdateMethodById(1, "Agile");
        [Benchmark] public void Benchmark_UpdateFullstackById() => _repository.UpdateFullstackById(1);
        [Benchmark] public void Benchmark_UpdateMethodByYearsAndDepartment() => _repository.UpdateMethodByYearsAndDepartment();
        [Benchmark] public void Benchmark_UpdateFullStackByExpYearsAndTitle() => _repository.UpdateFullStackByExpYearsAndTitle();
        [Benchmark] public void Benchmark_UpdateTitleByFullstackAndSeniorityAndYearsExp() => _repository.UpdateTitleByFullstackAndSeniorityAndYearsExp();
    }
}
