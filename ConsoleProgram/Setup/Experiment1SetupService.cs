using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models.Exp1;
using Generator;
using SqlDataAccess.Implementation;

namespace ConsoleProgram.Setup
{
    internal class Experiment1SetupService : SetupService<TaskModel>
    {
        private SqlEmployeeTasksRepository employeeTasksRepository;

        private List<EmloyeeModel> _emloyeeModels=new();
        private List<TaskModel> _taskModels=new();
        private int _min; private int _max;

        public Experiment1SetupService(string repo,int size,int mode) :base(mode,repo,size)
        {
            employeeTasksRepository = new();
            if (_size==SetSizeInterface.SMALL_SET)
            {
                _min = 5; _max=15;
            }
            else if (_size == SetSizeInterface.MEDIUM_SET)
            {
                _min=25; _max=45;
            }
            else if (size == SetSizeInterface.LARGE_SET)
            {
                _min = 40; _max=70;
            }
            
        }
        public Experiment1SetupService(string repo,string database, int size, int mode) : base(mode, repo, size)
        {
            
            employeeTasksRepository = new(database);
            if (_size == SetSizeInterface.SMALL_SET)
            {
                _min = 5; _max = 15;
            }
            else if (_size == SetSizeInterface.MEDIUM_SET)
            {
                _min = 25; _max = 45;
            }
            else if (size == SetSizeInterface.LARGE_SET)
            {
                _min = 40; _max = 70;
            }
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
                EmloyeeModel responsible = employees[rnd.Next(employees.Count - 1)];
                taskModel.Responsible = responsible;
                if (rnd.Next(1, 3) == 2)
                {
                    EmloyeeModel supervisor = employees[rnd.Next(employees.Count - 1)];
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
                    employeeTaskModel.TaskId = taskModel.Id;
                    employeeTaskModel.EmployeeId = foundEmployeeId;
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
            
            employeeTasksRepository.InsertMany(_taskModels);
        }

        protected override void PrepareData()
        {
            _emloyeeModels = new EmployeeFaker().Generate(_size).Select(e=>new EmloyeeModel(e)).ToList();
            employeeTasksRepository.InsertEmployeeBulk(_emloyeeModels);
        }
    }
}
