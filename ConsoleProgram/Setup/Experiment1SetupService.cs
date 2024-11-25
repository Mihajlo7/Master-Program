using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models.Exp1;
using DataAccess;
using Generator;
using HybridDataAccess.Implementation;
using SqlDataAccess.Implementation;

namespace ConsoleProgram.Setup
{
    internal class Experiment1SetupService : SetupService<TaskModel>
    {
        private IEmployeeTasksRepository employeeTasksRepository;

        private List<EmployeeModel> _emloyeeModels=new();
        private List<TaskModel> _taskModels=new();

        private int _min; private int _max;

        public Experiment1SetupService(string repo, int size, int mode)
       : base(mode, repo, size)
        {
            employeeTasksRepository = CreateRepository(repo);
            SetMinMax(size);
        }

        public Experiment1SetupService(string repo, string database, int size, int mode)
            : base(mode, repo, size)
        {
            employeeTasksRepository = new HybridEmployeeTasksRepository(database);
            SetMinMax(size);
        }

        private IEmployeeTasksRepository CreateRepository(string repo)
        {
            return repo == "sql"
                ? new SqlEmployeeTasksRepository()
                : new HybridEmployeeTasksRepository();
        }

        private void SetMinMax(int size)
        {
            (_min, _max) = size switch
            {
                SetSizeInterface.SMALL_SET => (1, 4),
                SetSizeInterface.MEDIUM_SET => (1, 4),
                SetSizeInterface.LARGE_SET => (1, 4),
                _ => throw new ArgumentOutOfRangeException(nameof(size), "Invalid set size")
            };
        }
        protected override void CreateIndexes()
        {
            throw new NotImplementedException();
        }

        protected override void CreateTables()
        {
            employeeTasksRepository.ExecuteCreationTable();
        }

        protected override List<TaskModel> GenerateData()
        {
            Random rnd = new Random();
            var employees = _emloyeeModels;
            var tasks= new TaskFaker().Generate(_size);
            List<TaskModel> taskModels = new List<TaskModel>();
            foreach (var task in tasks)
            {
                TaskModel taskModel = new TaskModel(task);
                EmployeeModel responsible = employees[rnd.Next(employees.Count - 1)];
                taskModel.Responsible = responsible;
                if (rnd.Next(1, 3) == 2)
                {
                    EmployeeModel supervisor = employees[rnd.Next(employees.Count - 1)];
                    taskModel.Supervisor = supervisor;
                }
                else
                {
                    taskModel.Supervisor = null;
                }
                List<EmployeeTaskModel> employeeTaskModels = new List<EmployeeTaskModel>();
                Dictionary<long,bool> lookUp= new Dictionary<long,bool>();
                int links = rnd.Next(_min,_max);
                for(int i=0;i<links;i++)
                {
                    long foundEmployeeId;
                    while (true) 
                    {
                        foundEmployeeId = employees[rnd.Next(_size-1)].Id;
                        if (lookUp.TryAdd(foundEmployeeId, true)) { break; }
                    }
                    EmployeeTaskModel employeeTaskModel = new();
                    //employeeTaskModel.TaskId = taskModel.Id;
                    //employeeTaskModel.EmployeeId = foundEmployeeId;
                    employeeTaskModel.Employee = _emloyeeModels.First(e=>e.Id==foundEmployeeId);
                    employeeTaskModels.Add(employeeTaskModel);
                }
                taskModel.Employees=employeeTaskModels;
                taskModels.Add(taskModel);
            }
            _taskModels = taskModels;
            return taskModels;
        }

        protected override void PopulateData()
        {
            
            employeeTasksRepository.InsertBulk(_taskModels);
        }

        protected override void PrepareData()
        {
            _emloyeeModels = new EmployeeFaker().Generate(_size).Select(e=>new EmployeeModel(e)).ToList();
            employeeTasksRepository.InsertEmployeeBulk(_emloyeeModels);
        }
    }
}
