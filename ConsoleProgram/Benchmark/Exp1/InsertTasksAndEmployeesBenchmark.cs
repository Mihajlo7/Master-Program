using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using ConsoleProgram.Generator;
using ConsoleProgram.Setup;
using Core.Models.Exp1;
using DataAccess;
using HybridDataAccess.Implementation;
using MongoDataAccess.Implementation;
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
    public class InsertTasksAndEmployeesBenchmark
    {
        public IEmployeeTasksRepository repository;
        public GeneratorService generatorService;
        public List<TaskModel> tasks;
        public List<EmployeeModel> employees;
        const int size= 50000;
        [GlobalSetup]
        public void Setup() 
        {
            generatorService = new GeneratorService();
            repository = new MongoEmployeeTasksRepository();
            (employees, tasks) = generatorService.GenerateDataTaskWithEmployeers(size,1,20);
            //repository.InsertEmployeeBulk(employees);
            //repository.InsertBulk(tasks);
        }

        [IterationSetup (Targets = new[] {nameof(InsertOne), nameof(InsertMany), nameof(InsertBulk) })] 
        public void IterationSetup() { repository.ExecuteCreationTable(); repository.InsertEmployeeBulk(employees); }

        [Benchmark] public void InsertOne() => repository.InsertOne(tasks.First());
        [Benchmark] public void InsertMany() => repository.InsertMany(tasks);
        [Benchmark] public void InsertBulk() => repository.InsertBulk(tasks);

       
        [Benchmark] public void GetAllTasksWithEmployeesBadWay() => repository.GetAllTasksWithEmployeesBadWay();
        [Benchmark] public void GetAllTasksWithEmployees() => repository.GetAllTasksWithEmployees();
        [Benchmark] public void GetAllTasksWithEmployeesSorted() => repository.GetAllTasksWithEmployeesSorted();
        [Benchmark] public void GetTaskWithEmployeesById() => repository.GetTaskWithEmployeesById(tasks.First().Id);
        [Benchmark] public void GetAllTasksWithEmployeesByPriorityAndStatus() => repository.GetAllTasksWithEmployeesByPriorityAndStatus(1);
        [Benchmark] public void GetAllTasksByDeadilineAndNotComplited() => repository.GetAllTasksByDeadilineAndNotComplited(7);
        [Benchmark] public void GetAllTasksByResponsibleNameAndSupervisorBirthday() => repository.GetAllTasksByResponsibleNameAndSupervisorBirthday("John", DateTime.Now.AddYears(-30));
        [Benchmark] public void GetEmployeesWithCountTasks() => repository.GetEmployeesWithCountTasks();
        [Benchmark] public void GetEmployeesWithCountTasksHavingAndOrder() => repository.GetEmployeesWithCountTasksHavingAndOrder(5);
        [Benchmark] public void UpdateExpiredTaskByDeadline() => repository.UpdateExpiredTaskByDeadline();
        [Benchmark] public void UpdatePhoneById() => repository.UpdatePhoneById("123-456-7890", tasks.First().Id);
        [Benchmark] public void UpdatePhoneByEmail() => repository.UpdatePhoneByEmail("123-456-7890", "test@example.com");
        [Benchmark] public void UpdateDeadlineByPriorityByDeadline() => repository.UpdateDeadlineByPriorityByDeadline(1, 7);
        [Benchmark] public void UpdateDeadlineByResponsibleLastName() => repository.UpdateDeadlineByResponsibleLastName("Smith");
        [Benchmark] public void UpdateDeadlineByResponsibleTitleAndBirthday() => repository.UpdateDeadlineByResponsibleTitleAndBirthday();
        [Benchmark] public void UpdateTasksFromOneEmployeeToOther() => repository.UpdateTasksFromOneEmployeeToOther(44, 66);
    }
}
