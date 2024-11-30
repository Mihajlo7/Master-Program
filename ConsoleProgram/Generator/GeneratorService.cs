﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models.Exp1;
using Core.Models.Exp2;
using Core.Models.Exp3;
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

        public (List<ManagerModel>,List<SoftwareDeveloperModel>) GenerateDataManagersAndDevelopers(int size)
        {
            return(new ManagerFaker().Generate(size / 2),new SoftwareDeveloperFaker().Generate(size / 2));
        }

        public (List<DepartmentModel>,List<EmployeeModel2>) GenerateDepartmentsAndEmployeers(int empNum,int teamNum)
        {
            
            int depNum = 20;
            int teamCount=depNum*teamNum;
            int empCount=teamCount*empNum;
            List<DepartmentModel> departments= new DepartmentFaker().Generate(20);
            List<TeamModel> teams= new TeamFaker().Generate(teamCount);
            List<EmployeeModel2> employees= new Employee2Faker().Generate(empCount);

            for(int i = 0; i < departments.Count; i++)
            {
                int startPosTeam= i*teamNum;
                departments[i].Teams= teams.Slice(startPosTeam,teamNum);
            }

            var updatedEmployees = employees.Select((e, i) =>
            {
                e.Team = teams[i / empNum];
                return e;
            }).ToList();
            return (departments, updatedEmployees);
        }
    }
}
