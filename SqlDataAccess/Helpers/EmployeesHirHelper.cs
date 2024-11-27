using Core.Models.Exp3;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SqlDataAccess.Helpers
{
    public static class EmployeesHirHelper
    {
        public static SqlCommand ToCommandManagerEmployeer(this SqlCommand command,ManagerModel manager)
        {
            command.Parameters.Clear();

            command.Parameters.AddWithValue("@Id", manager.Id);
            command.Parameters.AddWithValue("@FirstName", manager.FirstName);
            command.Parameters.AddWithValue("@LastName", manager.LastName);
            command.Parameters.AddWithValue("@Email", manager.Email);
            command.Parameters.AddWithValue("@Birthday", manager.BirthDay);
            command.Parameters.AddWithValue("@Title", manager.Title);
            command.Parameters.AddWithValue("@Phone", manager.Phone);

            command.Parameters.AddWithValue("@Department", manager.Department);
            command.Parameters.AddWithValue("@RealisedProject", manager.RealisedProject);
            command.Parameters.AddWithValue("@Method", manager.Method);

            return command;
        }

        public static SqlCommand ToCommandSoftwareDeveloper(this SqlCommand command,SoftwareDeveloperModel softwareDeveloperModel)
        {
            command.Parameters.Clear();

            command.Parameters.AddWithValue("@Id", softwareDeveloperModel.Id);
            command.Parameters.AddWithValue("@FirstName", softwareDeveloperModel.FirstName);
            command.Parameters.AddWithValue("@LastName", softwareDeveloperModel.LastName);
            command.Parameters.AddWithValue("@Email", softwareDeveloperModel.Email);
            command.Parameters.AddWithValue("@Birthday", softwareDeveloperModel.BirthDay);
            command.Parameters.AddWithValue("@Title", softwareDeveloperModel.Title);
            command.Parameters.AddWithValue("@Phone", softwareDeveloperModel.Phone);

            command.Parameters.AddWithValue("@Seniority", softwareDeveloperModel.Seniority);
            command.Parameters.AddWithValue("@YearsOfExperience", softwareDeveloperModel.YearsOfExperience);
            command.Parameters.AddWithValue("@IsRemote", softwareDeveloperModel.IsRemote);
            command.Parameters.AddWithValue("@ProgrammingLanguage", softwareDeveloperModel.ProgrammingLanguage);
            command.Parameters.AddWithValue("@IDE", softwareDeveloperModel.IDE);
            command.Parameters.AddWithValue("@IsFullStack", softwareDeveloperModel.IsFullStack);

            return command;
        }

        public static DataTable CreateDataTableFromManagaers(this List<ManagerModel> managers)
        {
            var managerTable = new DataTable("Manager");
            managerTable.Columns.Add("id", typeof(long));
            managerTable.Columns.Add("department", typeof(string));
            managerTable.Columns.Add("realisedProject", typeof(int));
            managerTable.Columns.Add("method", typeof(string));

            foreach (var manager in managers)
            {
                managerTable.Rows.Add(
                     manager.Id,
                     manager.Department,
                     manager.RealisedProject,
                     manager.Method);
            }
            return managerTable;
        }

        public static DataTable CreateDataTableFromEmployees(this List<EmployeeModel3> employees)
        {
            var employeeTable = new DataTable("Employee");
            employeeTable.Columns.Add("id", typeof(long));
            employeeTable.Columns.Add("firstname", typeof(string));
            employeeTable.Columns.Add("lastname", typeof(string));
            employeeTable.Columns.Add("email", typeof(string));
            employeeTable.Columns.Add("birthday", typeof(DateTime));
            employeeTable.Columns.Add("title", typeof(string));
            employeeTable.Columns.Add("phone", typeof(string));

            foreach(var manager in employees)
            {
                employeeTable.Rows.Add(
                   manager.Id,
                   manager.FirstName,
                   manager.LastName,
                   manager.Email,
                   manager.BirthDay,
                   manager.Title,
                   manager.Phone);
            }
            return employeeTable;
        }
        public static DataTable CreateDataTableFromDevelopers(this List<DeveloperModel> developers)
        {
            var developerTable = new DataTable("Developer");
            developerTable.Columns.Add("id", typeof(long));
            developerTable.Columns.Add("seniority", typeof(string));
            developerTable.Columns.Add("yearsOfExperience", typeof(int));
            developerTable.Columns.Add("isRemote", typeof(bool));

            foreach(var developer in developers)
            {
                developerTable.Rows.Add(
                    developer.Id,
                    developer.Seniority,
                    developer.YearsOfExperience,
                    developer.IsRemote);
            }
            return developerTable;
        }

        public static DataTable CreateDataTableFromSoftwareDevelopers(this List<SoftwareDeveloperModel> softwareDevelopers)
        {
            var softwareDeveloperTable = new DataTable("SoftwareDeveloper");
            softwareDeveloperTable.Columns.Add("id", typeof(long));
            softwareDeveloperTable.Columns.Add("programmingLanguage", typeof(string));
            softwareDeveloperTable.Columns.Add("ide", typeof(string));
            softwareDeveloperTable.Columns.Add("isfullstack", typeof(bool));

            foreach (var softwareDeveloper in softwareDevelopers)
            {
                softwareDeveloperTable.Rows.Add(
                   softwareDeveloper.Id,
                   softwareDeveloper.ProgrammingLanguage,
                   softwareDeveloper.IDE,
                   softwareDeveloper.IsFullStack);
            }
            return softwareDeveloperTable;
        }
        public static List<EmployeeModel3> GetEmployees3(this SqlDataReader reader)
        {
            var employees = new List<EmployeeModel3>();

            while (reader.Read())
            {
                // Mapiranje osnovnih podataka u EmployeeModel3
                var employee = new EmployeeModel3
                {
                    Id = reader.GetInt64(reader.GetOrdinal("id")),
                    FirstName = reader.GetString(reader.GetOrdinal("firstname")),
                    LastName = reader.GetString(reader.GetOrdinal("lastname")),
                    Email = reader.GetString(reader.GetOrdinal("email")),
                    BirthDay = reader.GetDateTime(reader.GetOrdinal("birthday")),
                    Title = reader.GetString(reader.GetOrdinal("title")),
                    Phone = reader.GetString(reader.GetOrdinal("phone"))
                };

                employees.Add(employee);
            }

            return employees;
        }

        public static List<SoftwareDeveloperModel> GetSoftwareDevelopers(this SqlDataReader reader)
        {
            var developers = new List<SoftwareDeveloperModel>();

            while (reader.Read())
            {
                // Mapiranje podataka u SoftwareDeveloperModel
                var developer = new SoftwareDeveloperModel
                {
                    Id = reader.GetInt64(reader.GetOrdinal("id")),
                    FirstName = reader.GetString(reader.GetOrdinal("firstname")),
                    LastName = reader.GetString(reader.GetOrdinal("lastname")),
                    Email = reader.GetString(reader.GetOrdinal("email")),
                    BirthDay = reader.GetDateTime(reader.GetOrdinal("birthday")),
                    Title = reader.GetString(reader.GetOrdinal("title")),
                    Phone = reader.GetString(reader.GetOrdinal("phone")),
                    Seniority = reader.GetString(reader.GetOrdinal("seniority")),
                    YearsOfExperience = reader.GetInt32(reader.GetOrdinal("yearsOfExperience")),
                    IsRemote = reader.GetBoolean(reader.GetOrdinal("isRemote")),
                    ProgrammingLanguage = reader.GetString(reader.GetOrdinal("programmingLanguage")),
                    IDE = reader.GetString(reader.GetOrdinal("ide")),
                    IsFullStack = reader.GetBoolean(reader.GetOrdinal("isFullStack"))
                };

                developers.Add(developer);
            }

            return developers;
        }

        public static List<ManagerModel> GetManagers( this SqlDataReader reader)
        {
            var managers = new List<ManagerModel>();

            while (reader.Read())
            {
                // Mapiranje podataka u ManagerModel
                var manager = new ManagerModel
                {
                    Id = reader.GetInt64(reader.GetOrdinal("id")),
                    FirstName = reader.GetString(reader.GetOrdinal("firstname")),
                    LastName = reader.GetString(reader.GetOrdinal("lastname")),
                    Email = reader.GetString(reader.GetOrdinal("email")),
                    BirthDay = reader.GetDateTime(reader.GetOrdinal("birthday")),
                    Title = reader.GetString(reader.GetOrdinal("title")),
                    Phone = reader.GetString(reader.GetOrdinal("phone")),
                    Department = reader.GetString(reader.GetOrdinal("department")),
                    RealisedProject = reader.GetInt32(reader.GetOrdinal("realisedProject")),
                    Method = reader.GetString(reader.GetOrdinal("method"))
                };

                managers.Add(manager);
            }

            return managers;
        }
    }
}
