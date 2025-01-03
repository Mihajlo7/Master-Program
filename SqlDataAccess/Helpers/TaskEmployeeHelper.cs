﻿using Core.Models.Exp1;
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
                        Responsible = new EmployeeModel
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
                        taskModel.Supervisor = new EmployeeModel
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
                        Employee = new EmployeeModel
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
                        Responsible = new EmployeeModel
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
                        taskModel.Supervisor = new EmployeeModel
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
                    Employee = new EmployeeModel
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

        public static List<TaskModel> ToTasksFullFromSelectHybrid(SqlDataReader reader)
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
                        Responsible = new EmployeeModel
                        {
                            Id = reader.IsDBNull(reader.GetOrdinal("ResponsibleId"))
                                    ? 0L
                                    : reader.GetInt64(reader.GetOrdinal("ResponsibleId")),
                            FirstName = reader.GetString(reader.GetOrdinal("ResponsibleFirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("ResponsibleLastName")),
                            Email = reader.GetString(reader.GetOrdinal("ResponsibleEmail")),
                            BirthDay = DateTime.Parse(reader.GetString(reader.GetOrdinal("ResponsibleBirthDay"))),
                            Title = reader.GetString(reader.GetOrdinal("ResponsibleTitle")),
                            Phone = reader.GetString(reader.GetOrdinal("ResponsiblePhone"))
                        },
                        Employees = new List<EmployeeTaskModel>()
                    };

                    if (!reader.IsDBNull(reader.GetOrdinal("SupervisorId")))
                    {
                        taskModel.Supervisor = new EmployeeModel
                        {
                            Id = reader.GetInt64(reader.GetOrdinal("SupervisorId")),
                            FirstName = reader.GetString(reader.GetOrdinal("SupervisorFirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("SupervisorLastName")),
                            Email = reader.GetString(reader.GetOrdinal("SupervisorEmail")),
                            BirthDay = DateTime.Parse(reader.GetString(reader.GetOrdinal("SupervisorBirthDay"))),
                            Title = reader.GetString(reader.GetOrdinal("SupervisorTitle")),
                            Phone = reader.GetString(reader.GetOrdinal("SupervisorPhone"))
                        };
                    }

                    taskDictionary[taskId] = taskModel;
                    taskModels.Add(taskModel);
                }

                // Mapiranje EmployeeTask
                // Provera i čitanje EmployeeId
                long employeeId = reader.IsDBNull(reader.GetOrdinal("EmployeeId")) ? 0L : reader.GetInt64(reader.GetOrdinal("EmployeeId"));

                // Čitanje FirstName sa proverom za NULL
                string? firstName = reader.IsDBNull(reader.GetOrdinal("EmployeeFirstName"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("EmployeeFirstName"));

                // Čitanje LastName sa proverom za NULL
                string? lastName = reader.IsDBNull(reader.GetOrdinal("EmployeeLastName"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("EmployeeLastName"));

                // Čitanje Email sa proverom za NULL
                string? email = reader.IsDBNull(reader.GetOrdinal("EmployeeEmail"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("EmployeeEmail"));

                // Čitanje BirthDay sa proverom za NULL
                DateTime birthDay = reader.IsDBNull(reader.GetOrdinal("EmployeeBirthDay"))
                    ? DateTime.Now
                    : DateTime.Parse(reader.GetString(reader.GetOrdinal("EmployeeBirthDay")));

                // Čitanje Title sa proverom za NULL
                string? title = reader.IsDBNull(reader.GetOrdinal("EmployeeTitle"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("EmployeeTitle"));

                // Čitanje Phone sa proverom za NULL
                string? phone = reader.IsDBNull(reader.GetOrdinal("EmployeePhone"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("EmployeePhone"));


                // Kreiranje EmployeeModel
                var employee = new EmployeeModel
                {
                    Id = employeeId,
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    BirthDay = birthDay,
                    Title = title,
                    Phone = phone
                };

                // Čitanje EmployeeTaskModel.EmployeeId
                long employeeTaskId = reader.IsDBNull(reader.GetOrdinal("EmployeeId"))
                        ? 0L
                        : reader.GetInt64(reader.GetOrdinal("EmployeeId"));


                // Kreiranje EmployeeTaskModel
                var employeeTask = new EmployeeTaskModel
                {
                    EmployeeId = employeeTaskId,
                    TaskId = taskId,
                    Employee = employee
                };

                taskModel.Employees.Add(employeeTask);
            }

            return taskModels;
        }

        public static List<TaskModel> ToTaskWithResponsible(SqlDataReader reader)
        {
            var taskModels = new List<TaskModel>();

            while (reader.Read())
            {
                var taskModel = new TaskModel
                {
                    Id = reader.GetInt64(reader.GetOrdinal("TaskId")),
                    Name = reader.GetString(reader.GetOrdinal("TaskName")),
                    Description = reader.GetString(reader.GetOrdinal("TaskDescription")),
                    Priority = reader.GetInt32(reader.GetOrdinal("TaskPriority")),
                    Deadline = reader.GetDateTime(reader.GetOrdinal("TaskDeadline")),
                    Status = reader.GetString(reader.GetOrdinal("TaskStatus")),
                    Responsible = new EmployeeModel
                    {
                        Id = reader.GetInt64(reader.GetOrdinal("ResponsibleId")),
                        Email = reader.GetString(reader.GetOrdinal("ResponsibleEmail")),
                        FirstName = reader.GetString(reader.GetOrdinal("ResponsibleFirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("ResponsibleLastName")),
                        BirthDay = reader.GetDateTime(reader.GetOrdinal("ResponsibleBirthDay")),
                        Title = reader.GetString(reader.GetOrdinal("ResponsibleTitle")),
                        Phone = reader.GetString(reader.GetOrdinal("ResponsiblePhone"))
                    }
                };

                taskModels.Add(taskModel);
            }

            return taskModels;
        }

        public static List<TaskModel> ToTaskWithResponsibleHibrid(SqlDataReader reader)
        {
            var taskModels = new List<TaskModel>();

            while (reader.Read())
            {
                var taskModel = new TaskModel
                {
                    Id = reader.GetInt64(reader.GetOrdinal("TaskId")),
                    Name = reader.GetString(reader.GetOrdinal("TaskName")),
                    Description = reader.GetString(reader.GetOrdinal("TaskDescription")),
                    Priority = reader.GetInt32(reader.GetOrdinal("TaskPriority")),
                    Deadline = reader.GetDateTime(reader.GetOrdinal("TaskDeadline")),
                    Status = reader.GetString(reader.GetOrdinal("TaskStatus")),
                    Responsible = new EmployeeModel
                    {
                        Id = reader.GetInt64(reader.GetOrdinal("ResponsibleId")),
                        Email = reader.GetString(reader.GetOrdinal("ResponsibleEmail")),
                        FirstName = reader.GetString(reader.GetOrdinal("ResponsibleFirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("ResponsibleLastName")),
                        BirthDay = DateTime.Parse(reader.GetString(reader.GetOrdinal("ResponsibleBirthDay"))),
                        Title = reader.GetString(reader.GetOrdinal("ResponsibleTitle")),
                        Phone = reader.GetString(reader.GetOrdinal("ResponsiblePhone"))
                    }
                };

                taskModels.Add(taskModel);
            }

            return taskModels;
        }

        public static List<TaskModel> ToTaskOnly(SqlDataReader reader)
        {
            var taskModels = new List<TaskModel>();
            while (reader.Read())
            {
                var model = new TaskModel()
                {

                    Id = reader.GetInt64(reader.GetOrdinal("TaskId")),
                    Name = reader.GetString(reader.GetOrdinal("TaskName")),
                    Description = reader.GetString(reader.GetOrdinal("TaskDescription")),
                    Priority = reader.GetInt32(reader.GetOrdinal("TaskPriority")),
                    Deadline = reader.GetDateTime(reader.GetOrdinal("TaskDeadline")),
                    Status = reader.GetString(reader.GetOrdinal("TaskStatus"))
                };
                taskModels.Add(model);
            }
            return taskModels;
        }

        public static List<TaskModel> ToTaskOnlyHibrid(SqlDataReader reader)
        {
            var taskModels = new List<TaskModel>();
            while (reader.Read())
            {
                var model = new TaskModel()
                {

                    Id = reader.GetInt64(reader.GetOrdinal("TaskId")),
                    Name = reader.GetString(reader.GetOrdinal("TaskName")),
                    Description = reader.GetString(reader.GetOrdinal("TaskDescription")),
                    Priority = reader.GetInt32(reader.GetOrdinal("TaskPriority")),
                    Deadline = reader.GetDateTime(reader.GetOrdinal("TaskDeadline")),
                    Status = reader.GetString(reader.GetOrdinal("TaskStatus"))
                };
                taskModels.Add(model);
            }
            return taskModels;
        }

        public static List<EmployeeWithCountTasksModel> ToEmployeeWithTasks(SqlDataReader reader)
        {
            var employees= new List<EmployeeWithCountTasksModel>();
            while (reader.Read())
            {
                var model = new EmployeeWithCountTasksModel()
                {
                    Id = reader.GetInt64(reader.GetOrdinal("Id")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),
                    Count = reader.GetInt32(reader.GetOrdinal("TaskCount"))
                };
                employees.Add(model);
            }
            return employees;
        }

        public static List<EmployeeWithCountTasksModel> ToEmployeeWithTasksHybrid(SqlDataReader reader)
        {
            var employees = new List<EmployeeWithCountTasksModel>();
            while (reader.Read())
            {
                var model = new EmployeeWithCountTasksModel()
                {
                    Id = long.Parse(reader.GetString(reader.GetOrdinal("Id"))),
                    Email = reader.GetString(reader.GetOrdinal("Email")),
                    Count = reader.GetInt32(reader.GetOrdinal("TaskCount"))
                };
                employees.Add(model);
            }
            return employees;
        }
    }
}
