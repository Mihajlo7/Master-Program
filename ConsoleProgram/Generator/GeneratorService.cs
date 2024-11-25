using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models.Exp1;
using Generator;

namespace ConsoleProgram.Generator
{
    public sealed class GeneratorService
    {
        public (List<EmployeeModel>,List<TaskModel>) GenerateDataTaskWithEmployeers(int size,int min, int max)
        {
            var employees = new EmployeeFaker().Generate(size)
                .Select(e => new EmployeeModel(e)).ToList();

            var tasks = new TaskFaker().Generate(size);

            Random rnd = new Random();

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
                Dictionary<long, bool> lookUp = new Dictionary<long, bool>();
                int links = rnd.Next(min, max);
                for (int i = 0; i < links; i++)
                {
                    long foundEmployeeId;
                    while (true)
                    {
                        foundEmployeeId = employees[rnd.Next(size - 1)].Id;
                        if (lookUp.TryAdd(foundEmployeeId, true)) { break; }
                    }
                    EmployeeTaskModel employeeTaskModel = new();
                    //employeeTaskModel.TaskId = taskModel.Id;
                    //employeeTaskModel.EmployeeId = foundEmployeeId;
                    employeeTaskModel.Employee = employees.First(e => e.Id == foundEmployeeId);
                    employeeTaskModels.Add(employeeTaskModel);
                }
                taskModel.Employees = employeeTaskModels;
                taskModels.Add(taskModel);
            }
            //_taskModels = taskModels;
            return (employees,taskModels);
        }
    }
}
