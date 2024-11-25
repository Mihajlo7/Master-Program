using Core.Models.Exp3;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
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
    }
}
