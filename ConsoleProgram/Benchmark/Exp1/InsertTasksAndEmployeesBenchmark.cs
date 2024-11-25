using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using ConsoleProgram.Setup;
using Core.Models.Exp1;
using DataAccess;
using SqlDataAccess.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleProgram.Benchmark.Exp1
{
    [SimpleJob(RunStrategy.Monitoring, launchCount: 1,
     warmupCount: 2, iterationCount: 5)]
    internal class InsertTasksAndEmployeesBenchmark
    {
        public IEmployeeTasksRepository repository;
        public Experiment1SetupService experimentSetupService;
        public List<TaskModel> tasks;

        [GlobalSetup]
        public void Setup() 
        {
            experimentSetupService = new Experiment1SetupService("sql",5000,1);
            tasks=experimentSetupService.RunGenerateData();
            repository = new SqlEmployeeTasksRepository();
        }

        [IterationSetup] public void IterationSetup() => repository.DeleteAllTasks();

        [Benchmark] public void InsertOne() => repository.InsertOne(tasks.First());
        [Benchmark] public void InsertMany() => repository.InsertMany(tasks);
        [Benchmark] public void InsertBulk() => repository.InsertBulk(tasks);
    }
}
