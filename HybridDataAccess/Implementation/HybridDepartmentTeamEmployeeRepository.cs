using Core.Models.Exp2;
using DataAccess;
using HybridDataAccess.Queries.Exp2;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HybridDataAccess.Implementation
{
    public sealed class HybridDepartmentTeamEmployeeRepository : HybridRepository,IDepartmentTeamEmployeeRepository
    {
        public HybridDepartmentTeamEmployeeRepository(string database) : base(database)
        {
        }

        public HybridDepartmentTeamEmployeeRepository() : base("exp2_db")
        {
        }

        public void ExecuteCreationTable()
        {
            string[] statements = GenerateQueriesFromQuery(Experiment2Hybrid.Tables);

            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            foreach (var statement in statements)
            {
                var command = new SqlCommand(statement, connection);
                command.ExecuteNonQuery();
            }
        }

        public void ExecuteCreationAdditional()
        {
            throw new NotImplementedException();
        }

        public void InsertDepartmentWithTeams(DepartmentModel department)
        {
            string query = GenerateQueriesFromQuery(Experiment2Hybrid.Insert)[0];

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);

            command.Parameters.Clear();

            command.Parameters.AddWithValue("@Id", department.Id);
            command.Parameters.AddWithValue("@Name", department.Name ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Location", department.Location ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@TeamsWithEmployees",_jsonHandler.SerializeMany(department.Teams));

            connection.Open();
            command.ExecuteNonQuery();
        }

        public void InsertEmployee(EmployeeModel2 employee)
        {
            string query = GenerateQueriesFromQuery(Experiment2Hybrid.Insert)[1];

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);

            command.Parameters.Clear();
            employee.Team = null;
            command.Parameters.AddWithValue("@TeamId",employee.Id);
            command.Parameters.AddWithValue("@Employees",
                _jsonHandler.SerializeMany(new List<EmployeeModel2> { employee }));

            connection.Open();
            command.ExecuteNonQuery();
        }

        public void InsertDepartmenstWithTeams(List<DepartmentModel> departments)
        {
            string query = GenerateQueriesFromQuery(Experiment2Hybrid.Insert)[0];

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);

            connection.Open();

            foreach (var department in departments)
            {
                command.Parameters.Clear();

                command.Parameters.AddWithValue("@Id", department.Id);
                command.Parameters.AddWithValue("@Name", department.Name ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Location", department.Location ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@TeamsWithEmployees", _jsonHandler.SerializeMany(department.Teams));

                command.ExecuteNonQuery();
            }

            
        }

        public void InsertEmployees(List<EmployeeModel2> employees)
        {
            var groupedByTeam = employees
                .GroupBy(emp => emp.Team)
                .Select(group => new
                    {
                        Team = group.Key,
                        Employees = group.ToList()
                    });
            string query = GenerateQueriesFromQuery(Experiment2Hybrid.Insert)[1];

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);

            connection.Open();
            foreach(var teamGroup in groupedByTeam)
            {
                foreach(var emp in teamGroup.Employees)
                {
                    emp.Team = null;
                }
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@TeamId", teamGroup.Team.Id);
                command.Parameters.AddWithValue("@Employees",_jsonHandler.SerializeMany(teamGroup.Employees));

                command.ExecuteNonQuery();
            }
        }

        public void InsertBulkDepartmenstWithTeams(List<DepartmentModel> departments)
        {
            var dataTable = new DataTable("Department");

            // Definišemo kolone
            dataTable.Columns.Add("Id", typeof(long));
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("Location", typeof(string));
            dataTable.Columns.Add("Teams", typeof(string));

            // Popunjavamo redove
            foreach (var department in departments)
            {
                dataTable.Rows.Add(
                    department.Id,
                    department.Name ?? (object)DBNull.Value,
                    department.Location ?? (object)DBNull.Value,
                    _jsonHandler.SerializeMany(department.Teams) ?? (object)DBNull.Value
                );
            }

            InsertBulkPriv("Department", dataTable);

        }

        public void InsertBulkEmployees(List<EmployeeModel2> employees)
        {
            throw new NotImplementedException();
        }

        public List<DepartmentModel> GetAllDepartmentsBadWay()
        {
            //using var connection = new SqlConnection(_connectionString);
            return null;
        }

        public List<DepartmentModel> GetAllDepartments()
        {
            throw new NotImplementedException();
        }

        public List<TeamModel> GetAllTeams(long departmentId)
        {
            throw new NotImplementedException();
        }

        public List<EmployeeModel2> GetAllEmployees(long teamId)
        {
            throw new NotImplementedException();
        }

        public EmployeeModel2 GetEmployee(int id)
        {
            throw new NotImplementedException();
        }

        public List<DepartmentModel> GetDepartmentsWithTeamsInBelgradeAndSorted()
        {
            throw new NotImplementedException();
        }

        public List<DepartmentModel> GetDepartmentsWithTeamsYoungerThan35AndEngineer()
        {
            throw new NotImplementedException();
        }

        public List<DepartmentAndTeamAgg> GetDepartmentWithEmployeesYearsBetweenGroupBy()
        {
            throw new NotImplementedException();
        }

        public List<DepartmentAndTeamAgg> GetDepartmentWithEmployeeGroupByHavingBy()
        {
            throw new NotImplementedException();
        }

        public void UpdateStatusTeam(long id, string status)
        {
            throw new NotImplementedException();
        }

        public void UpdateEmployeePhone(long id, string phone)
        {
            throw new NotImplementedException();
        }

        public void UpdateDescriptionTeamsFromPrague(string description)
        {
            throw new NotImplementedException();
        }

        public void UpdateDescriptionTeamsForYoungEmployees()
        {
            throw new NotImplementedException();
        }

        public void UpdateDescriptionComplex()
        {
            throw new NotImplementedException();
        }
    }
}
