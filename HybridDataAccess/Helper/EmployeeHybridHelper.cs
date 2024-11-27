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

            command.Parameters.AddWithValue("@Seniority", softwareDeveloperModel.Seniority);
            command.Parameters.AddWithValue("@YearsOfExperience", softwareDeveloperModel.YearsOfExperience);
            command.Parameters.AddWithValue("@IsRemote", softwareDeveloperModel.IsRemote);
            JsonHandler jsonHandler = new JsonHandler();
            string json = jsonHandler
                .SerializeOne<SoftwareDeveloperModel>(new SoftwareDeveloperModel() { ProgrammingLanguage=softwareDeveloperModel.ProgrammingLanguage,IDE=softwareDeveloperModel.IDE,IsFullStack=softwareDeveloperModel.IsFullStack});
            command.Parameters.AddWithValue("@SoftwareDeveloper", json);
            return command;
        }
    }
}
