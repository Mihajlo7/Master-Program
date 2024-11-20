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

        public Experiment1SetupService(object repo,int size,int mode) :base(mode,repo,size)
        {
            employeeTasksRepository = new();
        }
        public Experiment1SetupService(object repo,string database,int size, int mode) : base(mode, repo, size)
        {
            if (_name == "sql")
            {
                employeeTasksRepository = new(database);
            }
            employeeTasksRepository = new(database);
        }
        protected override void CreateIndexes()
        {
            throw new NotImplementedException();
        }

        protected override void CreateTables()
        {
            employeeTasksRepository.ExecuteCreationTable();
        }

        protected override List<TaskModel> GenerateData(int links)
        {
            Random rnd = new Random();
            var employees = new EmployeeFaker().Generate(_size);
            var tasks= new TaskFaker().Generate(_size);
            List<TaskModel> taskModels = new List<TaskModel>();
            foreach (var task in tasks)
            {
                TaskModel taskModel = new TaskModel(task);
                EmloyeeModel responsible = new EmloyeeModel(employees[rnd.Next(employees.Count - 1)]);
                taskModel.Responsible = responsible;
                if (rnd.Next(1, 3) == 2)
                {
                    EmloyeeModel supervisor = new EmloyeeModel(employees[rnd.Next(employees.Count - 1)]);
                    taskModel.Supervisor = supervisor;
                }
                else
                {
                    taskModel.Supervisor = null;
                }
                List<EmployeeTaskModel> employeeTaskModels = new List<EmployeeTaskModel>();
                Dictionary<long,bool> lookUp= new Dictionary<long,bool>();
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
                    employeeTaskModel.EmloyeeId = foundEmployeeId;
                    employeeTaskModels.Add(employeeTaskModel);
                }
                taskModel.Employees=employeeTaskModels;
                taskModels.Add(taskModel);
            }
            return taskModels;
        }

        protected override void PopulateData()
        {
            // insert employees
            var employees = new EmployeeFaker().Generate(_size);
            
        }
    }
}
