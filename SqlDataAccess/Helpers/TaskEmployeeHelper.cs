using Core.Models.Exp1;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlDataAccess.Helpers
{
    public static class TaskEmployeeHelper
    {
        public static List<TaskModel> ToTasksFullFromSelectBadWay(SqlDataReader reader)
        {
            var taskModels = new List<TaskModel>();
            var taskDictionary = new Dictionary<long, TaskModel>();

            while (reader.Read())
            {
                // Mapiranje Task entiteta (prvih nekoliko kolona)
                long taskId = reader.GetInt64(0); // Prva kolona: "id" Task-a

                if (!taskDictionary.TryGetValue(taskId, out var taskModel))
                {
                    taskModel = new TaskModel
                    {
                        Id = taskId,
                        Name = reader.GetString(1), // "name"
                        Description = reader.IsDBNull(2) ? string.Empty : reader.GetString(2), // "description"
                        Priority = reader.GetInt32(3), // "priority"
                        Deadline = reader.GetDateTime(4), // "deadline"
                        Status = reader.GetString(5), // "status"

                        // Mapiranje Responsible (sledeće kolone)
                        Responsible = new EmloyeeModel
                        {
                            Id = reader.GetInt64(6), // "id" Responsible-a
                            FirstName = reader.GetString(9), // "firstName"
                            LastName = reader.GetString(10), // "lastName"
                            Email = reader.GetString(11), // "email"
                            BirthDay = reader.GetDateTime(12), // "birthDay"
                            Title = reader.GetString(13), // "title"
                            Phone = reader.GetString(14) // "phone"
                        },

                        Employees = new List<EmployeeTaskModel>()
                    };

                    // Mapiranje Supervisor (ako postoji)
                    if (!reader.IsDBNull(15)) // "id" Supervisor-a
                    {
                        taskModel.Supervisor = new EmloyeeModel
                        {
                            Id = reader.GetInt64(15), // "id"
                            FirstName = reader.GetString(16), // "firstName"
                            LastName = reader.GetString(17), // "lastName"
                            Email = reader.GetString(18), // "email"
                            BirthDay = reader.GetDateTime(19), // "birthDay"
                            Title = reader.GetString(20), // "title"
                            Phone = reader.GetString(21) // "phone"
                        };
                    }

                    taskDictionary[taskId] = taskModel;
                    taskModels.Add(taskModel);
                }

                // Mapiranje EmployeeTask entiteta
                if (!reader.IsDBNull(22)) // "employeeId"
                {
                    var employeeTask = new EmployeeTaskModel
                    {
                        EmployeeId = reader.GetInt64(22), // "employeeId"
                        TaskId = taskId,
                        Emloyee = new EmloyeeModel
                        {
                            Id = reader.GetInt64(22), // "employeeId"
                            FirstName = reader.GetString(25), // "firstName"
                            LastName = reader.GetString(26), // "lastName"
                            Email = reader.GetString(27), // "email"
                            BirthDay = reader.GetDateTime(28), // "birthDay"
                            Title = reader.GetString(29), // "title"
                            Phone = reader.GetString(30) // "phone"
                        }
                    };

                    taskModel.Employees.Add(employeeTask);
                }
            }

            return taskModels;
        }

        public static List<TaskModel> ToTasksFullFromSelect(SqlDataReader reader)
        {
            var taskModels = new List<TaskModel>();
            var taskDictionary = new Dictionary<long, TaskModel>();

            while (reader.Read())
            {
                // Task podaci
                long taskId = reader.GetInt64(reader.GetOrdinal("TaskId"));

                if (!taskDictionary.TryGetValue(taskId, out var taskModel))
                {
                    taskModel = new TaskModel
                    {
                        Id = taskId,
                        Name = reader.GetString(reader.GetOrdinal("TaskName")),
                        Description = reader.GetString(reader.GetOrdinal("TaskDescription")),
                        Priority = reader.GetInt32(reader.GetOrdinal("TaskPriority")),
                        Deadline = reader.GetDateTime(reader.GetOrdinal("TaskDeadline")),
                        Status = reader.GetString(reader.GetOrdinal("TaskStatus")),
                        Responsible = new EmloyeeModel
                        {
                            Id = reader.GetInt64(reader.GetOrdinal("ResponsibleId")),
                            FirstName = reader.GetString(reader.GetOrdinal("ResponsibleFirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("ResponsibleLastName")),
                            Email = reader.GetString(reader.GetOrdinal("ResponsibleEmail")),
                            BirthDay = reader.GetDateTime(reader.GetOrdinal("ResponsibleBirthDay")),
                            Title = reader.GetString(reader.GetOrdinal("ResponsibleTitle")),
                            Phone = reader.GetString(reader.GetOrdinal("ResponsiblePhone"))
                        },
                        Employees = new List<EmployeeTaskModel>()
                    };

                    if (!reader.IsDBNull(reader.GetOrdinal("SupervisorId")))
                    {
                        taskModel.Supervisor = new EmloyeeModel
                        {
                            Id = reader.GetInt64(reader.GetOrdinal("SupervisorId")),
                            FirstName = reader.GetString(reader.GetOrdinal("SupervisorFirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("SupervisorLastName")),
                            Email = reader.GetString(reader.GetOrdinal("SupervisorEmail")),
                            BirthDay = reader.GetDateTime(reader.GetOrdinal("SupervisorBirthDay")),
                            Title = reader.GetString(reader.GetOrdinal("SupervisorTitle")),
                            Phone = reader.GetString(reader.GetOrdinal("SupervisorPhone"))
                        };
                    }

                    taskDictionary[taskId] = taskModel;
                    taskModels.Add(taskModel);
                }

                // Mapiranje EmployeeTask
                var employeeTask = new EmployeeTaskModel
                {
                    EmployeeId = reader.GetInt64(reader.GetOrdinal("EmployeeId")),
                    TaskId = taskId,
                    Emloyee = new EmloyeeModel
                    {
                        Id = reader.GetInt64(reader.GetOrdinal("EmployeeId")),
                        FirstName = reader.GetString(reader.GetOrdinal("EmployeeFirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("EmployeeLastName")),
                        Email = reader.GetString(reader.GetOrdinal("EmployeeEmail")),
                        BirthDay = reader.GetDateTime(reader.GetOrdinal("EmployeeBirthDay")),
                        Title = reader.GetString(reader.GetOrdinal("EmployeeTitle")),
                        Phone = reader.GetString(reader.GetOrdinal("EmployeePhone"))
                    }
                };

                taskModel.Employees.Add(employeeTask);
            }

            return taskModels;
        }
    }
}
