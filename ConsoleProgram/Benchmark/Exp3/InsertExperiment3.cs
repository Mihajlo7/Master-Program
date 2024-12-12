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
    public class InsertExperiment3
    {
        public IEmployeeRepository repository;
        public GeneratorService generatorService = new();
        public List<ManagerModel> managers;
        public List<SoftwareDeveloperModel> softwareDevelopers;

        const int employeeSize = 5000;

        [GlobalSetup] public void GlobalSetup()
        {
            repository = new SqlEmployeeRepository();
            (managers, softwareDevelopers) = generatorService.GenerateDataManagersAndDevelopers(employeeSize);
        }
        [IterationSetup] public void IterationSetup() => repository.ExecuteCreationTable();
        [Benchmark] public void InsertManager() => repository.InsertManager(managers.First());
        [Benchmark] public void InsertSoftwareDevs() => repository.InsertSoftwareDeveloper(softwareDevelopers.First());

        [Benchmark] public void InsertManyManagers() => repository.InsertManyManager(managers);
        [Benchmark] public void InsertManyDevelopers() => repository.InsertManySoftwareDeveloper(softwareDevelopers);

        [Benchmark] public void InsertBulkManagers() => repository.InsertBulkManager(managers);
        [Benchmark] public void InsertBulkSoftwareDevs() => repository.InsertBulkSoftwareDeveloper(softwareDevelopers);
    }
}
