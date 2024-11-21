using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using ConsoleProgram.Setup;
using Core.Models.Exp1;
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
    internal class InsertTasksAndEmployeesSqlBenchmark
    {
        public SqlEmployeeTasksRepository repository;
        public Experiment1SetupService experimentSetupService;
        public List<TaskModel> tasks;

        [GlobalSetup]
        public void Setup() 
        {
           

        }
    }
}
