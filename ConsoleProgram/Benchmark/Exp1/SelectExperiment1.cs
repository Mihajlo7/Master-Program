using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using DataAccess;
using SqlDataAccess.Implementation;

namespace ConsoleProgram.Benchmark.Exp1
{
    [SimpleJob(RunStrategy.Monitoring, launchCount: 1,
     warmupCount: 2, iterationCount: 5)]
    public  class SelectExperiment1
    {
        public IEmployeeTasksRepository repository;

        [GlobalSetup] public void GlobalSetup() => repository =new SqlEmployeeTasksRepository();
    }
}
