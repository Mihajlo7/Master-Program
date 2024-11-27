using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models.Exp3;
using HybridDataAccess.DataSerializator;
using Microsoft.Data.SqlClient;

namespace HybridDataAccess.Helper
{
    internal static class EmployeeHybridHelper
    {
        internal static SqlCommand ToCommandManagerEmployeerHybrid(this SqlCommand command, ManagerModel manager)
        {
            command.Parameters.Clear();

            command.Parameters.AddWithValue("@Id", manager.Id);
            command.Parameters.AddWithValue("@FirstName", manager.FirstName);
            command.Parameters.AddWithValue("@LastName", manager.LastName);
            command.Parameters.AddWithValue("@Email", manager.Email);
            command.Parameters.AddWithValue("@Birthday", manager.BirthDay);
            command.Parameters.AddWithValue("@Title", manager.Title);
            command.Parameters.AddWithValue("@Phone", manager.Phone);
            JsonHandler jsonHandler = new JsonHandler();
            string json = jsonHandler
                .SerializeOne<ManagerModel>(new ManagerModel() { RealisedProject = manager.RealisedProject, Method = manager.Method, Department = manager.Department });
            command.Parameters.AddWithValue("@Manager",json );

            return command;
        }

        public static SqlCommand ToCommandSoftwareDeveloperHybrid(this SqlCommand command, SoftwareDeveloperModel softwareDeveloperModel)
        {
            command.Parameters.Clear();

            command.Parameters.AddWithValue("@Id", softwareDeveloperModel.Id);
            command.Parameters.AddWithValue("@FirstName", softwareDeveloperModel.FirstName);
            command.Parameters.AddWithValue("@LastName", softwareDeveloperModel.LastName);
            command.Parameters.AddWithValue("@Email", softwareDeveloperModel.Email);
            command.Parameters.AddWithValue("@Birthday", softwareDeveloperModel.BirthDay);
            command.Parameters.AddWithValue("@Title", softwareDeveloperModel.Title);
            command.Parameters.AddWithValue("@Phone", softwareDeveloperModel.Phone);
            command.Parameters.AddWithValue("@Manager", DBNull.Value);
            command.Parameters.AddWithValue("@Seniority", softwareDeveloperModel.Seniority);
            command.Parameters.AddWithValue("@YearsOfExperience", softwareDeveloperModel.YearsOfExperience);
            command.Parameters.AddWithValue("@IsRemote", softwareDeveloperModel.IsRemote);
            JsonHandler jsonHandler = new JsonHandler();
            string json = jsonHandler
                .SerializeOne<SoftwareDeveloperModel>(new SoftwareDeveloperModel() {Id=softwareDeveloperModel.Id ,ProgrammingLanguage=softwareDeveloperModel.ProgrammingLanguage,IDE=softwareDeveloperModel.IDE,IsFullStack=softwareDeveloperModel.IsFullStack});
            command.Parameters.AddWithValue("@SoftwareDeveloper", json);
            return command;
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

        public static List<ManagerModel> GetManagers(this SqlDataReader reader)
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
